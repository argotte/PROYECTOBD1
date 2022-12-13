using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Contratos.DetalleContratos
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString = "";
        public List<DetalleContratoModelo> listaDetalleContrato = new List<DetalleContratoModelo>();
        DetalleContratoModelo detalleContrato = new DetalleContratoModelo();

        public string error = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            detalleContrato.FK_ID_CONTRATO = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select c.ID,p.ID,c.NOMBRE,p.NOMBRE,dc.FECHAENVIO,dc.FECHAENVIOMAX,dc.CANTIDAD_KG,dc.PORCENTAJEDESCUENTO,dc.ID from DJR_DETALLE_CONTRATOS DC "+
                                 "INNER JOIN DJR_PRODUCTORES P ON P.ID = DC.FK_ID_PRODUCTOR "+
                                 "INNER JOIN DJR_CLIENTES C ON C.ID = DC.FK_ID_CLIENTE "+
                                 "where dc.ID = @IDCLIENTE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDCLIENTE",detalleContrato.FK_ID_CONTRATO);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DetalleContratoModelo detalleContrato = new DetalleContratoModelo();
                                detalleContrato.FK_ID_CLIENTE = "" + reader.GetInt32(0);
                                detalleContrato.FK_ID_PRODUCTOR=""+reader.GetInt32(1);
                                detalleContrato.NOMBRECLIENTE=reader.GetString(2);
                                detalleContrato.NOMBREPRODUCTOR = reader.GetString(3);
                                detalleContrato.FECHAENVIO=reader.GetDateTime(4).ToString("yyyy-MM-dd");
                                detalleContrato.FECHAENVIOMAX = reader.GetDateTime(5).ToString("yyyy-MM-dd");
                                detalleContrato.CANTIDAD_KG = ""+reader.GetInt32(6);
                                detalleContrato.PORCENTAJEDESCUENTO = (reader.IsDBNull(7)!=true)?""+reader.GetInt32(7):"";
                                detalleContrato.ID = "" + reader.GetInt32(8);
                                listaDetalleContrato.Add(detalleContrato);

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
