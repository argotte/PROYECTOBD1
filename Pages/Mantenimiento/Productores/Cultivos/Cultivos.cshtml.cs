using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace PROYECTOBD1.Pages.Mantenimiento.Productores.Cultivos
{
    public class CultivosModel : PageModel
    {
        public ProductorModelo productorModelo = new ProductorModelo();
        public List<CultivosModelo> listaCultivos = new List<CultivosModelo>();
        public string error = "";
        //public string correcto = "";
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";

        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.FK_ID_VARIEDAD, C.FK_ID_PRODUCTOR,C.ID,V.NOMBRE VARIEDAD, C.CALIBRE,C.PRODUCCION_ESTIMADA, "+ 
                                 "C.PERIODO_INICIO_DISPONIBILIDAD, C.PERIODO_FIN_DISPONIBILIDAD, C.MAX_DESTINADO_EXPORTACION "+
                                 "FROM DJR_CULTIVOS AS C "+
                                 "INNER JOIN DJR_VARIEDADES AS V "+
                                 "ON V.ID = C.FK_ID_VARIEDAD "+
                                 "WHERE C.FK_ID_PRODUCTOR = @IDPRODUCTOR";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDPRODUCTOR", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CultivosModelo cultivosModelo = new CultivosModelo();
                                cultivosModelo.FK_ID_VARIEDAD=(reader.IsDBNull(0)!=true)?""+reader.GetInt32(0):"";
                                cultivosModelo.FK_ID_PRODUCTOR = (reader.IsDBNull(1) != true) ? "" + reader.GetInt32(1):"";
                                cultivosModelo.ID = (reader.IsDBNull(2) != true) ? "" + reader.GetInt32(2) : "";
                                cultivosModelo.NOMBREVARIEDAD = (reader.IsDBNull(3) != true) ? reader.GetString(3) : "";
                                cultivosModelo.CALIBRE = (reader.IsDBNull(4) != true) ? ""+ reader.GetInt32(4) :"" ;
                                cultivosModelo.PRODUCCION_ESTIMADA_KG = (reader.IsDBNull(5) != true) ? "" + reader.GetInt32(5) : "";
                                cultivosModelo.PERIODO_INICIO_DISPONIBILIDAD = (reader.IsDBNull(6) != true) ? reader.GetDateTime(6).ToString("yyyy-MM-dd") : "";
                                cultivosModelo.PERIODO_FIN_DISPONIBILIDAD = (reader.IsDBNull(7) != true) ? reader.GetDateTime(7).ToString("yyyy-MM-dd") : "";
                                cultivosModelo.MAX_DESTINADO_EXPORTACION = (reader.IsDBNull(8) != true) ? "" + reader.GetInt32(8) : "";
                                listaCultivos.Add(cultivosModelo);
                                //Convert.ToDateTime(MyReader["DateField"]).ToString("dd/MM/yyyy");
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

    }
}
