using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Variedades
{
    public class IndexModel : PageModel
    {
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<VariedadesModelo> listaVariedades = new List<VariedadesModelo>();
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT V.ID, V.PRECOCIDAD, V.NOMBRE, V.FIRMEZA, V.COLOR, V.ESPECIE, V.DESCRIPCION FROM DJR_VARIEDADES AS V";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VariedadesModelo variedadesModelo = new VariedadesModelo();
                                variedadesModelo.ID             = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                variedadesModelo.PRECOCIDAD     = (reader.IsDBNull(1) != true) ? reader.GetString(1) : "";
                                variedadesModelo.NOMBRE         = (reader.IsDBNull(1) != true) ? reader.GetString(1) : "";
                                variedadesModelo.FIRMEZA        = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                variedadesModelo.COLOR          = (reader.IsDBNull(3) != true) ? reader.GetString(3) : "";
                                variedadesModelo.ESPECIE        = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                variedadesModelo.DESCRIPCION    = (reader.IsDBNull(5) != true) ? reader.GetString(5) : "";
                                listaVariedades.Add(variedadesModelo);
                            }
                        }
                    }
                }
                //CiudadModelo ciudadModelo = new CiudadModelo();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
