using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Asociaciones
{
    public class IndexModel : PageModel
    {
        public List<RegionModelo> listaRegion = new List<RegionModelo>();
        public List<AsociacionesModelo> listaAsociacion = new List<AsociacionesModelo>();
        string error = "";
        string productor = "";
        public string idregion = "";
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT R.ID, R.NOMBRE FROM DJR_REGIONES AS R";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegionModelo regionModelo = new RegionModelo();
                                regionModelo.ID = "" + reader.GetInt32(0);
                                regionModelo.NOMBRE = reader.GetString(1);
                                listaRegion.Add(regionModelo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                error = ex.Message;
            }
        }
        public void OnPost() 
        {
            idregion = Request.Form["idregion"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT A.ID,A.NOMBRE, R.NOMBRE FROM DJR_ASOCIACIONES AS A " +
                                 "INNER JOIN DJR_REGIONES AS R ON R.ID=A.FK_ID_REGION " +
                                 "WHERE R.ID=@IDREGION";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDREGION", idregion);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AsociacionesModelo asociacionesModelo = new AsociacionesModelo();
                                asociacionesModelo.ID = ""+reader.GetInt32(0);
                                asociacionesModelo.NOMBRE= reader.GetString(1);
                                asociacionesModelo.NOMBREREGION=reader.GetString(2);
                                listaAsociacion.Add(asociacionesModelo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                error = ex.Message;
                OnGet();
            }
            OnGet();

        }
       
    }
}
