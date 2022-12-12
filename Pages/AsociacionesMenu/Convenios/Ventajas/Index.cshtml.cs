using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Convenios.Ventajas
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        string connectionString="";
        public List<VentajaModelo> listaVentaja = new List<VentajaModelo>();
        public VentajaModelo ventajaModelo = new VentajaModelo();

        public string error = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            ventajaModelo.FK_ID_CONVENIOS= Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT V.ID, V.NOMBRE,V.PORCENTAJEDESCUENTO FROM DJR_VENTAJAS V " +
                                 "INNER JOIN DJR_CONVENIOS C ON C.ID = V.FK_ID_CONVENIOS " +
                                 "WHERE V.FK_ID_CONVENIOS = @IDCONVENIO";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDCONVENIO", ventajaModelo.FK_ID_CONVENIOS);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VentajaModelo ventajaModelo = new VentajaModelo();
                                ventajaModelo.ID = "" + reader.GetInt32(0);
                                ventajaModelo.NOMBRE = reader.GetString(1);
                                ventajaModelo.PORCENTAJEDESCUENTO = ""+reader.GetInt32(2);
                                listaVentaja.Add(ventajaModelo);
                             
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
