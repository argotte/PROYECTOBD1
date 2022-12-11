using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion.Criterios
{
    public class CrearModel : PageModel
    {
        Connection connection2 = new Connection();
        // connectionString = connection2.ConnectionString;
        String connectionString = "";
       // String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public CriterioModelo criterio=new CriterioModelo();
        public string error = "";
        public string correcto = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            criterio.NOMBRE = Request.Form["NOMBRE"];
            criterio.DESCRIPCION = Request.Form["DESCRIPCION"];
            criterio.TIPO = Request.Form["TIPO"];
            if (criterio.NOMBRE.Length == 0 || criterio.DESCRIPCION.Length == 0 || criterio.TIPO.Length == 0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_CRITERIOS_VAR" +
                                "(NOMBRE,DESCRIPCION,TIPO) VALUES" +
                                "(@NOMBRE,@DESCRIPCION,@TIPO);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", criterio.NOMBRE);
                        command.Parameters.AddWithValue("@DESCRIPCION", criterio.DESCRIPCION);
                        command.Parameters.AddWithValue("@TIPO", criterio.TIPO);
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
            criterio.NOMBRE = ""; criterio.DESCRIPCION = ""; criterio.TIPO = "";
            correcto = "CRITERIO AGREGADO CORRECTAMENTE";
            Response.Redirect("/Evaluacion/Criterios/Mostrar");
        }
    }
}
