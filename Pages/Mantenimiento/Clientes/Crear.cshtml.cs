using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Clientes
{
    public class CrearModel : PageModel
    {
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<CiudadModelo> listaCiudades = new List<CiudadModelo>();
        public ClienteModelo clienteModelo = new ClienteModelo();
        public string error = "";
        public string correcto="";
        public string pais, ciudad;

        public void OnGet()
        {

        }
        public void OnPost()
        {
            clienteModelo.NOMBRE = Request.Form["NOMBRE"];
            clienteModelo.FK_ID_PAIS = Request.Form["FK_ID_PAIS"];
            clienteModelo.FK_ID_CIUDAD = Request.Form["FK_ID_CIUDAD"];
            if (clienteModelo.FK_ID_PAIS.Length == 0 || clienteModelo.FK_ID_PAIS.Length == 0 || clienteModelo.NOMBRE.Length == 0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                { 
                    connection.Open();
                    String sql = "SELECT P.ID,C.ID FROM DJR_PAISES AS P INNER JOIN DJR_CIUDADES C ON P.ID=C.FK_ID_PAIS WHERE P.NOMBRE= @PAIS AND C.NOMBRE= @CIUDAD";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {            
                        command.Parameters.AddWithValue("@PAIS", clienteModelo.FK_ID_PAIS);
                        command.Parameters.AddWithValue("@CIUDAD", clienteModelo.FK_ID_CIUDAD);
                       // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                pais = "" + reader.GetInt32(0);
                                ciudad = "" + reader.GetInt32(1);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_CLIENTES" +
                                "(NOMBRE,FK_ID_PAIS,FK_ID_CIUDAD) VALUES" +
                                "(@NOMBRE,@FK_ID_PAIS,@FK_ID_CIUDAD);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", clienteModelo.NOMBRE);
                        command.Parameters.AddWithValue("@FK_ID_PAIS", pais);
                        command.Parameters.AddWithValue("@FK_ID_CIUDAD", ciudad);
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
            clienteModelo.NOMBRE = ""; clienteModelo.FK_ID_CIUDAD = ""; clienteModelo.FK_ID_PAIS = "";
            correcto = "CLIENTE AGREGADO CORRECTAMENTE";
            Response.Redirect("/Mantenimiento/Clientes/Index");
        }
    }
}
