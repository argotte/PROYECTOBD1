using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Clientes
{
    public class CrearModel : PageModel
    {
        public ClienteInfo clienteInfo = new ClienteInfo();
        public string error = "";
        public string correcto="";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            clienteInfo.NOMBRE = Request.Form["NOMBRE"];
            clienteInfo.FK_ID_PAIS = Request.Form["FK_ID_PAIS"];
            clienteInfo.FK_ID_CIUDAD = Request.Form["FK_ID_CIUDAD"];
            if (clienteInfo.FK_ID_PAIS.Length == 0 || clienteInfo.FK_ID_PAIS.Length == 0 || clienteInfo.NOMBRE.Length == 0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                { 
                    connection.Open();
                    String sql = "INSERT INTO CLIENTES" +
                                "(NOMBRE,FK_ID_PAIS,FK_ID_CIUDAD) VALUES" +
                                "(@NOMBRE,@FK_ID_PAIS,@FK_ID_CIUDAD);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", clienteInfo.NOMBRE);
                        command.Parameters.AddWithValue("FK_ID_PAIS", clienteInfo.FK_ID_PAIS);
                        command.Parameters.AddWithValue("FK_ID_CIUDAD", clienteInfo.FK_ID_CIUDAD);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;
               
            }
            //ahora salva la info en la bd
            clienteInfo.NOMBRE = "";clienteInfo.FK_ID_CIUDAD = "";clienteInfo.FK_ID_PAIS = "";
            correcto = "CLIENTE AGREGADO CORRECTAMENTE";
            Response.Redirect("/Clientes/Index");
        }
    }
}
