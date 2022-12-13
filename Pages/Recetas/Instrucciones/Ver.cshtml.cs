using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Recetas
{
    public class VerModel : PageModel
    {
        public Connection connection2 = new Connection();
        String connectionString = "";
        //    connectionString=connection2.ConnectionString;
        public List<InstruccionModelo> listInstrucciones = new List<InstruccionModelo>();
        bool repetir=false;
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            string idReceta = Request.Query["id"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT R.NOMBRE, R.ID, I.PASO, I.DESCRIPCION FROM DJR_INSTRUCCIONES AS I " +
                                 "INNER JOIN DJR_RECETAS AS R ON R.ID = I.FK_ID_RECETA " +
                                 "WHERE I.FK_ID_RECETA = @IDRECETA";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDRECETA", idReceta);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            repetir= true;
                            while (reader.Read())
                            {
                                InstruccionModelo instruccion_info = new InstruccionModelo();
                                instruccion_info.NOMBRERECETA = reader.GetString(0);
                                instruccion_info.FK_ID_RECETA = ""+reader.GetInt32(1); 
                                instruccion_info.PASO = "" + reader.GetInt32(2);
                                instruccion_info.DESCRIPCION = reader.GetString(3);

                                listInstrucciones.Add(instruccion_info);
                                repetir= false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            if (repetir==true)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "SELECT R.NOMBRE,R.ID FROM DJR_RECETAS AS R " +
                                     "WHERE R.ID = @IDRECETA";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@IDRECETA", idReceta);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    InstruccionModelo instruccion_info = new InstruccionModelo();
                                    instruccion_info.NOMBRERECETA = reader.GetString(0);
                                    instruccion_info.FK_ID_RECETA = ""+reader.GetInt32(1);
                                    listInstrucciones.Add(instruccion_info);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
            }
        }
    }
}
