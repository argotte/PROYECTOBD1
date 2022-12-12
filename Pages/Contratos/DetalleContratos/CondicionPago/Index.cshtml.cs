using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Contratos.DetalleContratos.CondicionPago
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString = "";
        CondicionPagoModelo condicionPagoModelo = new CondicionPagoModelo();
        public List<CondicionPagoModelo> listacp = new List<CondicionPagoModelo>();

        public string error = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            condicionPagoModelo.FK_ID_DETALLE_CONTRATOS = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select CP.FK_ID_DETALLE_CONTRATOS,CP.ID,PROD.NOMBRE,CLI.NOMBRE,CP.TIPO,CP.CANTIDADCUOTAS,CP.PORCENTAJE_POR_CUOTA,CP.CONTADO_A_LA_EMISION,CP.CONTADO_AL_RECIBIR from DJR_CONDICIONES_PAGOS CP " +
                                 "INNER JOIN DJR_DETALLE_CONTRATOS DC ON DC.ID = CP.FK_ID_DETALLE_CONTRATOS " +
                                 "INNER JOIN DJR_CONTRATOS CONTRATO ON CONTRATO.ID = CP.FK_ID_CONTRATO " +
                                 "JOIN DJR_PRODUCTORES PROD ON PROD.ID = CP.FK_ID_PRODUCTOR AND PROD.ID = CP.FK_ID_PRODUCTORDETALLEC " +
                                 "INNER JOIN DJR_CLIENTES CLI ON CLI.ID = CP.FK_ID_CLIENTE " +
                                 "WHERE DC.ID = @IDDETALLE ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDDETALLE", condicionPagoModelo.FK_ID_DETALLE_CONTRATOS);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CondicionPagoModelo condicionPagoModelo = new CondicionPagoModelo();
                                condicionPagoModelo.FK_ID_DETALLE_CONTRATOS = "" + reader.GetInt32(0);
                                condicionPagoModelo.ID = "" + reader.GetInt32(1);
                                condicionPagoModelo.NOMBREPRODUCTOR = reader.GetString(2);
                                condicionPagoModelo.NOMBRECLIENTE = reader.GetString(3);
                                condicionPagoModelo.TIPO = (reader.IsDBNull(4) != true) ? reader.GetString(4):null;
                                condicionPagoModelo.CANTIDADCUOTA = (reader.IsDBNull(5) != true) ? "" + reader.GetInt32(5):null;
                                condicionPagoModelo.PORCENTAJE_POR_CUOTA = (reader.IsDBNull(6) != true) ? "" + reader.GetInt32(6):null;
                                condicionPagoModelo.CONTADO_A_LA_EMISION = (reader.IsDBNull(7) != true) ? "" + reader.GetInt32(7):null;
                                condicionPagoModelo.CONTADO_AL_RECIBIR = (reader.IsDBNull(8) != true) ? "" + reader.GetInt32(8):null;
                                listacp.Add(condicionPagoModelo);
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
