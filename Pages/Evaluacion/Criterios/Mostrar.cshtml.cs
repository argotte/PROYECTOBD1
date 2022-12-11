using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion.Criterios
{
    public class MostrarModel : PageModel
    {
        public List<CriterioModelo> listaCri=new List<CriterioModelo>();
        String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID,C.NOMBRE,C.DESCRIPCION,C.TIPO FROM DJR_CRITERIOS_VAR C";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CriterioModelo criterio=new CriterioModelo(); 
                                criterio.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                criterio.NOMBRE= (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                criterio.DESCRIPCION = (reader.IsDBNull(2) != true) ? "" + reader.GetString(2) : "";
                                criterio.TIPO = (reader.IsDBNull(3) != true) ? "" + reader.GetString(3) : "";
                                //       paisModelo.Ciudades= new List<CiudadModelo>();
                                listaCri.Add(criterio);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}