using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Variedades.PrecioVariedad
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString = "";
        public List<PrecioVariedadesModelo> listaprecio = new List<PrecioVariedadesModelo>();
        PrecioVariedadesModelo precioVariedadesModelo = new PrecioVariedadesModelo();

        public string error = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            precioVariedadesModelo.FK_ID_VARIEDADES = Request.Query["id"];
       //     produccionAnualModelo.FK_ID_DETALLE_CONTRATOS = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT V.ID,PV.ID,P.NOMBRE,V.NOMBRE,PV.FECHA_INICIO,PV.FECHA_FIN,PV.PRECIO,PV.CALIBRE FROM DJR_PRECIO_VARIEDADES PV " +
                                 "INNER JOIN DJR_VARIEDADES V ON V.ID = PV.FK_ID_VARIEDADES " +
                                 "INNER JOIN DJR_PAISES P ON P.ID = PV.FK_ID_PAISES " +
                                 "WHERE V.ID = @IDVARIEDAD ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDVARIEDAD", precioVariedadesModelo.FK_ID_VARIEDADES);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PrecioVariedadesModelo precioVariedadesModelo = new PrecioVariedadesModelo();
                                precioVariedadesModelo.FK_ID_VARIEDADES = "" + reader.GetInt32(0);
                                precioVariedadesModelo.ID = "" + reader.GetInt32(1);
                                precioVariedadesModelo.PAIS = reader.GetString(2);
                                precioVariedadesModelo.VARIEDAD = reader.GetString(3);
                                precioVariedadesModelo.FECHAINICIO = reader.GetDateTime(4).ToString("yyyy-MM-dd");
                                precioVariedadesModelo.FECHAFIN =(reader.IsDBNull(5)!=true)? reader.GetDateTime(5).ToString("yyyy-MM-dd"):null;
                                precioVariedadesModelo.PRECIO = "" + reader.GetDecimal(6);
                                precioVariedadesModelo.CALIBRE = "" + reader.GetInt32(7);
                                listaprecio.Add(precioVariedadesModelo);
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
        }
    }
}
