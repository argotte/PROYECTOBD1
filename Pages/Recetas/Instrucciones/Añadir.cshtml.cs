using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Recetas.Instrucciones
{
    public class AñadirModel : PageModel
    {
        public InstruccionModelo instruccionInfo = new InstruccionModelo();
        public List<InstruccionModelo> listInstruccion = new List<InstruccionModelo>();
        public string errorMessage = "";
        public string successMessage = "";
      //  String connectionString = "Data Source=CHUBE;Initial Catalog=ProyectoCereza;Integrated Security=True";
        public Connection connection2 = new Connection();
        String connectionString = "";
        //    connectionString=connection2.ConnectionString;
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sql = "SELECT RE.ID, RE.NOMBRE FROM DJR_RECETAS AS RE";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InstruccionModelo instruccionIn = new InstruccionModelo();
                            instruccionIn.FK_ID_RECETA = "" + reader.GetInt32(0);
                            instruccionIn.NOMBRERECETA = reader.GetString(1);
                            listInstruccion.Add(instruccionIn);
                        }
                    }
                }
            }
        }
        public void OnPost() 
        {
            connectionString = connection2.ConnectionString;
            instruccionInfo.PASO = Request.Form["PASO"];
            instruccionInfo.DESCRIPCION = Request.Form["DESCRIPCION"];

            if (instruccionInfo.PASO.Length == 0 || instruccionInfo.DESCRIPCION.Length == 0 )
            {
                errorMessage = "Debe rellenar todos los campos.";
                return;
            }
            try 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_INSTRUCCIONES" +
                                 "(PASO, DESCRIPCION) VALUES " +
                                 "(@PASO, @DESCRIPCION);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PASO", instruccionInfo.PASO);
                        command.Parameters.AddWithValue("@DESCRIPCION",instruccionInfo.DESCRIPCION);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
            }

            instruccionInfo.FK_ID_RECETA = "";
            instruccionInfo.PASO = "";
            instruccionInfo.DESCRIPCION = "";

            successMessage = "Instrucciones añadidas correctamente.";

            OnGet();
        }
    }
}
