using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.PaisOrigen
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString = "";
        public List<PaisOrigenModelo> listapaisorigen = new List<PaisOrigenModelo>();
        PaisOrigenModelo paisorigen = new PaisOrigenModelo();

        public string error = "";

        public void OnGet() 
        {
            connectionString = connection2.ConnectionString;
            paisorigen.FK_ID_VARIEDAD = Request.Query["id"];
            //     produccionAnualModelo.FK_ID_DETALLE_CONTRATOS = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PO.FK_ID_PAIS,PO.FK_ID_VARIEDAD,P.NOMBRE,V.NOMBRE FROM DJR_PAISES_ORIGEN PO "+
                                 "INNER JOIN DJR_VARIEDADES V ON V.ID = PO.FK_ID_VARIEDAD "+
                                 "INNER JOIN DJR_PAISES P ON P.ID = PO.FK_ID_PAIS "+
                                 "WHERE V.ID = @IDVARIEDAD";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDVARIEDAD", paisorigen.FK_ID_VARIEDAD);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PaisOrigenModelo paisOrigenModelo = new PaisOrigenModelo();
                                paisOrigenModelo.FK_ID_PAIS = "" + reader.GetInt32(0);
                                paisOrigenModelo.FK_ID_VARIEDAD = "" + reader.GetInt32(1);
                                paisOrigenModelo.PAIS = reader.GetString(2);
                                paisOrigenModelo.VARIEDAD = reader.GetString(3);
                                listapaisorigen.Add(paisOrigenModelo);
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
