using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Contratos.DetalleContratos.ProduccionAnual
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString = "";
        public List<ProduccionAnualModelo> pam = new List<ProduccionAnualModelo>();
        ProduccionAnualModelo produccionAnualModelo = new ProduccionAnualModelo();

        public string error = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            produccionAnualModelo.FK_ID_DETALLE_CONTRATOS = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select PA.FK_ID_DETALLE_CONTRATOS,PA.FK_ID_CONTRATO,PROD.NOMBRE,CLI.NOMBRE,V.NOMBRE,PA.ANIO,PA.TOTALKG from DJR_PRODUCCIONES_ANUALES AS PA " +
                                 "INNER JOIN DJR_DETALLE_CONTRATOS DC ON DC.ID = PA.FK_ID_DETALLE_CONTRATOS " +
                                 "INNER JOIN DJR_CONTRATOS CONTRATO ON CONTRATO.ID = PA.FK_ID_CONTRATO " +
                                 "INNER JOIN DJR_PRODUCTORES PROD ON PROD.ID = PA.FK_ID_PRODUCTOR " +
                                 "INNER JOIN DJR_CLIENTES CLI ON CLI.ID = PA.FK_ID_CLIENTE " +
                                 "INNER JOIN DJR_VARIEDADES V ON V.ID = PA.FK_ID_VARIEDADES " +
                                 "WHERE DC.ID = @IDDETALLE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDDETALLE", produccionAnualModelo.FK_ID_DETALLE_CONTRATOS);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProduccionAnualModelo produccionAnualModelo = new ProduccionAnualModelo();

                                produccionAnualModelo.FK_ID_DETALLE_CONTRATOS = "" + reader.GetInt32(0);
                                produccionAnualModelo.FK_ID_CONTRATO = "" + reader.GetInt32(1);
                                produccionAnualModelo.NOMBREPRODUCTOR = reader.GetString(2);
                                produccionAnualModelo.NOMBRECLIENTE = reader.GetString(3);
                                produccionAnualModelo.NOMBREVARIEDAD = reader.GetString(4);
                                produccionAnualModelo.ANO = reader.GetDateTime(5).ToString("yyyy-MM-dd");
                                produccionAnualModelo.TOTALKG =""+ reader.GetInt32(6);
                                pam.Add(produccionAnualModelo);
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
