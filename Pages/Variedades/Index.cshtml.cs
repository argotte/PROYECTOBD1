using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Variedades
{
    public class IndexModel : PageModel
    {
        string pais = "";
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<VariedadesModelo> listaVariedades = new List<VariedadesModelo>();
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public void OnGet()
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT P.ID,P.NOMBRE,P.CONTINENTE FROM DJR_PAISES AS P";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PaisModelo paisModelo = new PaisModelo();
                                paisModelo.ID = "" + reader.GetInt32(0);
                                paisModelo.NOMBRE = reader.GetString(1);
                                paisModelo.CONTINENTE = reader.GetString(2);
                                //       paisModelo.Ciudades= new List<PadrinosModelo>();
                                listaPaises.Add(paisModelo);
                            }
                        }
                    }
                }
                //PadrinosModelo ciudadModelo = new PadrinosModelo();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void OnPost() 
        {
            try
            {
                pais = Request.Form["pais"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select PA.NOMBRE AS PAIS,VA.ID,VA.PRECOCIDAD,VA.NOMBRE,VA.FIRMEZA,VA.COLOR,VA.ESPECIE,VA.DESCRIPCION FROM DJR_PAISES_ORIGEN AS PO " +
                                 "INNER JOIN DJR_PAISES AS PA ON PO.FK_ID_PAIS = PA.ID " +
                                 "INNER JOIN DJR_VARIEDADES AS VA ON VA.ID=PO.FK_ID_VARIEDAD " +
                                 "WHERE PA.NOMBRE = @PAIS";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PAIS", pais);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VariedadesModelo variedadesModelo = new VariedadesModelo();
                                variedadesModelo.PAISNOMBRE = (reader.IsDBNull(0) != true) ? reader.GetString(0) : "";
                                variedadesModelo.ID = (reader.IsDBNull(1) != true) ? "" + reader.GetInt32(1) : "";
                                variedadesModelo.PRECOCIDAD = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                variedadesModelo.NOMBRE = (reader.IsDBNull(3) != true) ? reader.GetString(3) : "";
                                variedadesModelo.FIRMEZA = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                variedadesModelo.COLOR = (reader.IsDBNull(5) != true) ? reader.GetString(5) : "";
                                variedadesModelo.ESPECIE = (reader.IsDBNull(6) != true) ? reader.GetString(6) : "";
                                variedadesModelo.DESCRIPCION = (reader.IsDBNull(7) != true) ? reader.GetString(7) : "";
                                listaVariedades.Add(variedadesModelo);
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
