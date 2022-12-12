using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Contratos.DetalleContratos.Pagos
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString = "";
        PagosModelo pagosModelo = new PagosModelo();
        public List<PagosModelo> listaPago = new List<PagosModelo>();

        public string error = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            pagosModelo.FK_ID_DETALLE_CONTRATO = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PAGO.FK_ID_DETALLE_CONTRATO,PAGO.ID,PROD.NOMBRE,CLI.NOMBRE,PAGO.FECHA,PAGO.MONTO_DOLARES FROM DJR_PAGOS PAGO " +
                                 "INNER JOIN DJR_DETALLE_CONTRATOS DC ON DC.ID = PAGO.FK_ID_DETALLE_CONTRATO " +
                                 "INNER JOIN DJR_CONTRATOS CONTRATO ON CONTRATO.ID = PAGO.FK_ID_CONTRATO " +
                                 "INNER JOIN DJR_PRODUCTORES PROD ON PROD.ID = PAGO.FK_ID_PRODUCTOR " +
                                 "INNER JOIN DJR_CLIENTES CLI ON CLI.ID = PAGO.FK_ID_CLIENTE " +
                                 "where DC.ID = @IDDETALLE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDDETALLE", pagosModelo.FK_ID_DETALLE_CONTRATO);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PagosModelo pagosModelo = new PagosModelo();
                                pagosModelo.FK_ID_DETALLE_CONTRATO = "" + reader.GetInt32(0);
                                pagosModelo.ID = "" + reader.GetInt32(1);
                                pagosModelo.NOMBREPRODUCTOR = reader.GetString(2);
                                pagosModelo.NOMBRECLIENTE = reader.GetString(3);
                                pagosModelo.FECHA= reader.GetDateTime(4).ToString("yyyy-MM-dd");
                                pagosModelo.MONTO_DOLARES = "" + reader.GetDecimal(5);
                                listaPago.Add(pagosModelo);
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
