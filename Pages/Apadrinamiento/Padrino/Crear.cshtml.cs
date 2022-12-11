using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;
using System.Security.Policy;

namespace PROYECTOBD1.Pages.Apadrinamiento.Padrino
{
    public class CrearModel : PageModel
    {
      //  String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<PadrinosModelo> listaPadrinos = new List<PadrinosModelo>();
        public string error = "";
        public string correcto = "";
        public string cedula, nombre, nombre2, nombre3, nombre4 = "";
        Connection connection2 = new Connection();
        // connectionString = connection2.ConnectionString;
        String connectionString = "";
        public void OnGet()
        {
        }
        public void OnPost() 
        {
            connectionString = connection2.ConnectionString;
            correcto = "";
            cedula = Request.Form["cedula"];
            nombre = Request.Form["nombre"];
            nombre2 = Request.Form["nombre2"];
            nombre3 = Request.Form["nombre3"];
            nombre4 = Request.Form["nombre4"];
            if (cedula.Length == 0 || nombre.Length==0 || nombre3.Length==0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                OnGet();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_PADRINOS " +
                                "(DOC_IDENTIDAD,NOMBRE,SEGUNDO_NOMBRE,APELLIDO,SEGUNDO_APELLIDO) VALUES" +
                                "(@DOC_IDENTIDAD,@NOMBRE,@SEGUNDO_NOMBRE,@APELLIDO,@SEGUNDO_APELLIDO);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@DOC_IDENTIDAD", cedula);
                        command.Parameters.AddWithValue("@NOMBRE", nombre);
                        command.Parameters.AddWithValue("@SEGUNDO_NOMBRE", (nombre2!="")?nombre2:DBNull.Value);
                        command.Parameters.AddWithValue("@APELLIDO", nombre3);
                        command.Parameters.AddWithValue("@SEGUNDO_APELLIDO", (nombre4 != "") ? nombre4 : DBNull.Value);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                OnGet();

            }
            cedula = nombre = nombre2 = nombre3 = nombre4 = "";
            correcto = "PRODUCTOR AGREGADO CORRECTAMENTE";
            OnGet();
        }
    }
}
