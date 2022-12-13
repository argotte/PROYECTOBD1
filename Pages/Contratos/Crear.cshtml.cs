using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;
using System.Xml.Schema;

namespace PROYECTOBD1.Pages.Contratos
{
    public class CrearModel : PageModel
    {
        Connection connection2 = new Connection();
        //  connectionString = connection2.ConnectionString;
        String connectionString = "";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<VariedadesModelo> listaVariedades = new List<VariedadesModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public List<CultivosModelo> listaCultivos = new List<CultivosModelo>();
        public List<RegionModelo> listaRegion = new List<RegionModelo>();
        public List<ClienteModelo> listaCliente = new List<ClienteModelo>();
        public RegionModelo regionModelo = new RegionModelo();
        public AsociacionesModelo asociacionesModelo = new AsociacionesModelo();
        public CultivosModelo cultivosModelo = new CultivosModelo();
        public string error = "";
        public string correcto = "";
        public string idpais = "";
        public string idproductor, idcliente = "";
        public string porcentajedescuento,total,fechaemision = "";
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
            idproductor = Request.Form["IDPRODUCTOR"];
            idcliente = Request.Form["IDCLIENTE"];
            porcentajedescuento = Request.Form["PORCENTAJEDESCUENTO"];
            fechaemision = Request.Form["FECHAEMISION"];
            total = Request.Form["TOTAL"];
          //  regionModelo.ID = Request.Form["IDREGION"];

            if (porcentajedescuento==null|| fechaemision ==null|| total==null || porcentajedescuento.Length == 0 || fechaemision.Length == 0 || total.Length==-0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                OnGet();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT R.FK_ID_PAISES FROM DJR_REGIONES R WHERE R.ID=@IDREGION";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDREGION", regionModelo.ID);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idpais = "" + reader.GetInt32(0);
                            }
                        }
                    }
                }//idpais, regionModelo.ID, asociacionesModelo.NOMBRE
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_ASOCIACIONES(NOMBRE,FK_ID_REGION,FK_ID_PAIS) VALUES(@NOMBRE,@IDREGION,@IDPAIS);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", asociacionesModelo.NOMBRE);
                        command.Parameters.AddWithValue("@IDREGION", regionModelo.ID);
                        command.Parameters.AddWithValue("@IDPAIS", idpais);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                OnGet();

            }
            //ahora salva la info en la bd
            //  productorModelo.NOMBRE = ""; productorModelo.FK_ID_CIUDAD = ""; productorModelo.FK_ID_PAIS = ""; productorModelo.NOMBREPAIS = ""; productorModelo.NOMBRECIUDAD = ""; productorModelo.DIRECCION = "";
            //   productorModelo.ENVASE_ESTANDAR = "";

            correcto = "PRODUCTOR AGREGADO CORRECTAMENTE";
            asociacionesModelo.NOMBRE = "";
            OnGet();
        }
    }
}
