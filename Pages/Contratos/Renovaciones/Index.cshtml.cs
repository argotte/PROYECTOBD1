using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Contratos.Renovaciones
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString = "";
        RenovacionModelo renovacionModelo = new RenovacionModelo();
        public List<RenovacionModelo> listaRenovacion = new List<RenovacionModelo>();

        public string error = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            renovacionModelo.FK_ID_CONTRATO = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select RC.ID,P.NOMBRE,C.NOMBRE,RC.FECHARENOVACION,RC.TOTADOLARES from DJR_RENOVACION_CONTRATOS RC " +
                                 "INNER JOIN DJR_CONTRATOS CT ON CT.ID = RC.FK_ID_CONTRATO " +
                                 "INNER JOIN DJR_PRODUCTORES P ON P.ID = RC.FK_ID_PRODUCTOR " +
                                 "INNER JOIN DJR_CLIENTES C ON C.ID = RC.FK_ID_CLIENTE " +
                                 "WHERE RC.FK_ID_CONTRATO = @IDCONTRATO ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDCONTRATO", renovacionModelo.FK_ID_CONTRATO);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RenovacionModelo renovacionModelo = new RenovacionModelo();
                                renovacionModelo.ID = "" + reader.GetInt32(0);
                                renovacionModelo.NOMBREPRODUCTOR = reader.GetString(1);
                                renovacionModelo.NOMBRECLIENTE = reader.GetString(2);
                                renovacionModelo.FECHARENOVACION= (reader.IsDBNull(3) != true) ? reader.GetDateTime(3).ToString("yyyy-MM-dd") : "";
                                renovacionModelo.TOTALDOLARES = ""+reader.GetDecimal(4);
                                listaRenovacion.Add(renovacionModelo);

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
