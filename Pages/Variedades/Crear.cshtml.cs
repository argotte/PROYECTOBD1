using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Variedades
{
    public class CrearModel : PageModel
    {
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<VariedadesModelo> variedades = new List<VariedadesModelo>();
        public VariedadesModelo variedadesModelo = new VariedadesModelo();
        public string error = "";
        public string correcto = "";
        public string pais, ciudad;

        public void OnGet()
        {
        }
        public void OnPost()
        {
            variedadesModelo.PRECOCIDAD = Request.Form["PRECOCIDAD"];
            variedadesModelo.NOMBRE = Request.Form["NOMBRE"];
            variedadesModelo.FIRMEZA = Request.Form["FIRMEZA"];
            variedadesModelo.COLOR = Request.Form["COLOR"];
            variedadesModelo.ESPECIE = Request.Form["ESPECIE"];
            variedadesModelo.DESCRIPCION = Request.Form["DESCRIPCION"];
            

            if (variedadesModelo.PRECOCIDAD.Length == 0 || variedadesModelo.NOMBRE.Length == 0 || variedadesModelo.FIRMEZA.Length == 0 || variedadesModelo.COLOR.Length == 0 || variedadesModelo.ESPECIE.Length==0 || variedadesModelo.DESCRIPCION.Length==0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_VARIEDADES " +
                                "(PRECOCIDAD,NOMBRE,FIRMEZA,COLOR,ESPECIE,DESCRIPCION) VALUES" +
                                "(@PRECOCIDAD,@NOMBRE,@FIRMEZA,@COLOR,@ESPECIE,@DESCRIPCION);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PRECOCIDAD", variedadesModelo.PRECOCIDAD);
                        command.Parameters.AddWithValue("@NOMBRE", variedadesModelo.NOMBRE);
                        command.Parameters.AddWithValue("@FIRMEZA", variedadesModelo.FIRMEZA);
                        command.Parameters.AddWithValue("@COLOR", variedadesModelo.COLOR);
                        command.Parameters.AddWithValue("@ESPECIE", variedadesModelo.ESPECIE);
                        command.Parameters.AddWithValue("@DESCRIPCION", variedadesModelo.DESCRIPCION);
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
            variedadesModelo.PRECOCIDAD = ""; variedadesModelo.NOMBRE = ""; variedadesModelo.FIRMEZA = ""; variedadesModelo.COLOR = ""; variedadesModelo.ESPECIE = ""; variedadesModelo.DESCRIPCION = "";

            correcto = "VARIEDAD AGREGADA CORRECTAMENTE";
            Response.Redirect("/Variedades/Index");
        }
    }
}
