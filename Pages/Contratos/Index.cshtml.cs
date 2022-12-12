using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace PROYECTOBD1.Pages.Contratos
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        //  connectionString = connection2.ConnectionString;
        String connectionString = "";
        public List<ClienteModelo> listaCliente = new List<ClienteModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>(); string error = "";
        public List<ContratosModelo> listaContrato = new List<ContratosModelo>();
        public string idproductor = "";
        public string idcliente = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT P.ID, P.NOMBRE FROM DJR_PRODUCTORES AS P";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductorModelo productorModelo = new ProductorModelo();
                                productorModelo.ID = "" + reader.GetInt32(0);
                                productorModelo.NOMBRE = reader.GetString(1);
                                listaProductor.Add(productorModelo);
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
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID, C.NOMBRE FROM DJR_CLIENTES AS C";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteModelo clienteModelo = new ClienteModelo();
                                clienteModelo.ID = "" + reader.GetInt32(0);
                                clienteModelo.NOMBRE = reader.GetString(1);
                                listaCliente.Add(clienteModelo);
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

        public void OnPost() 
        {
            connectionString = connection2.ConnectionString;
            idproductor = Request.Form["idproductor"];
            idcliente = Request.Form["idcliente"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select CONTRATO.ID,C.NOMBRE,P.NOMBRE,CONTRATO.FECHA_EMITIDO,CONTRATO.PORCENTAJEDESCUENTO,CONTRATO.TOTAL from DJR_CONTRATOS as CONTRATO " +
                                 "INNER JOIN DJR_PRODUCTORES AS P ON P.ID = CONTRATO.FK_ID_PRODUCTOR " +
                                 "INNER JOIN DJR_CLIENTES AS C ON C.ID = CONTRATO.FK_ID_CLIENTE " +
                                 "WHERE CONTRATO.FK_ID_PRODUCTOR =@IDPRODUCTOR AND CONTRATO.FK_ID_CLIENTE=@IDCLIENTE";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDPRODUCTOR", idproductor);
                        command.Parameters.AddWithValue("@IDCLIENTE", idcliente);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ContratosModelo contratosModelo = new ContratosModelo();
                                contratosModelo.ID = "" + reader.GetInt32(0);
                                contratosModelo.NOMBRECLIENTE= reader.GetString(1);
                                contratosModelo.NOMBREPRODUCTOR = reader.GetString(2);
                                contratosModelo.FECHA_EMITIDO = reader.GetDateTime(3).ToString("yyyy-MM-dd");
                                contratosModelo.PORCENTAJEDESCUENTO = "" + reader.GetInt32(4);
                                contratosModelo.TOTAL = "" + reader.GetDecimal(5);
                                listaContrato.Add(contratosModelo);
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
            OnGet();
        }
    }
}
