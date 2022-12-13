using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Contratos.DetalleContratos.EnvioReal
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString = "";
        EnvioRealModelo envioRealModelo = new EnvioRealModelo();
        public List<EnvioRealModelo> listaEnvio = new List<EnvioRealModelo>();

        public string error = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            envioRealModelo.FK_ID_DETALLE_CONTRATO=Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT ER.FK_ID_DETALLE_CONTRATO,ER.ID,PROD.NOMBRE,CLI.NOMBRE,ER.FECHA,ER.KG FROM DJR_ENVIOS_REALES ER "+
                                 "INNER JOIN DJR_DETALLE_CONTRATOS DC ON DC.ID = ER.FK_ID_DETALLE_CONTRATO "+
                                 "INNER JOIN DJR_CONTRATOS CONTRATO ON CONTRATO.ID = ER.FK_ID_CONTRATO "+
                                 "INNER JOIN DJR_PRODUCTORES PROD ON PROD.ID = ER.FK_ID_PRODUCTOR "+
                                 "INNER JOIN DJR_CLIENTES CLI ON CLI.ID = ER.FK_ID_CLIENTE "+
                                 "where DC.ID = @IDDETALLE ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDDETALLE", envioRealModelo.FK_ID_DETALLE_CONTRATO);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EnvioRealModelo envioRealModelo = new EnvioRealModelo();
                                envioRealModelo.FK_ID_DETALLE_CONTRATO = "" + reader.GetInt32(0);
                                envioRealModelo.ID = "" + reader.GetInt32(1);
                                envioRealModelo.NOMBREPRODUCTOR = reader.GetString(2);
                                envioRealModelo.NOMBRECLIENTE = reader.GetString(3);
                                envioRealModelo.FECHA = reader.GetDateTime(4).ToString("yyyy-MM-dd");
                                envioRealModelo.KG = "" + reader.GetInt32(5);
                                listaEnvio.Add(envioRealModelo);
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
