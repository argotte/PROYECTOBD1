using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Individuales
{
    public class CrearModel : PageModel
    {
        public Connection connection2 = new Connection();
        String connectionString = "";
        //   String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public AsociacionesModelo asociacionesModelo = new AsociacionesModelo();
        public List<AsociacionesModelo> listaAsociaciones = new List<AsociacionesModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public ProductorModelo productorModelo = new ProductorModelo();
        public List<ProveedorModelo> listaProveedor = new List<ProveedorModelo>();
        public ProveedorModelo proveedorModelo = new ProveedorModelo();
        public string error = "";
        public string correcto = "";
        public string idproductor, idpaisproductor, idproveedor, idpaisproveedor = "";
        bool lo_anado = false;
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT A.ID, A.NOMBRE FROM DJR_PRODUCTORES AS A";
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

                error = "EL PRODUCTOR NO PUDO SER AGREGADO";
                OnGet();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PROV.ID, PROV.NOMBRE FROM DJR_PROVEEDORES AS PROV";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProveedorModelo proveedorModelo = new ProveedorModelo();
                                proveedorModelo.ID = "" + reader.GetInt32(0);
                                proveedorModelo.NOMBRE = reader.GetString(1);
                                listaProveedor.Add(proveedorModelo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                error = "EL PRODUCTOR NO PUDO SER AGREGADO";
                OnGet();
            }
        }

        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            proveedorModelo.ID = Request.Form["idproveedor"];
            productorModelo.ID = Request.Form["idproductor"];

            //if (clienteModelo.FK_ID_PAIS.Length == 0 || clienteModelo.FK_ID_PAIS.Length == 0 || clienteModelo.NOMBRE.Length == 0)
            //{
            //    error = "TODOS LOS CAMPOS SON REQUERIDOS";
            //    return;
            //}
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PROD.ID, PROD.FK_ID_PAIS FROM DJR_PRODUCTORES PROD WHERE PROD.ID=@IDPRODUCTOR";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDPRODUCTOR", productorModelo.ID);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idproductor = "" + reader.GetInt32(0);
                                idpaisproductor = "" + reader.GetInt32(1);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PROV.ID,PROV.FK_ID_PAIS FROM DJR_PROVEEDORES PROV WHERE PROV.ID=@IDPROVEEDOR";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDPROVEEDOR", proveedorModelo.ID);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idproveedor = "" + reader.GetInt32(0);
                                idpaisproveedor = "" + reader.GetInt32(1);
                            }
                        }
                    }
                }

                int result = string.Compare(idpaisproductor, idpaisproveedor);
                if (result != 0)//efectivamente estan en el mismo pais. Ahora a revisar si ya esta repetido o no.
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            String sql = "INSERT INTO DJR_PROD_PROV(FK_ID_PRODUCTOR,FK_ID_PROVEEDOR) VALUES (@IDPRODUCTOR,@IDPROVEEDOR)";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@IDPRODUCTOR", idproductor);
                                command.Parameters.AddWithValue("@IDPROVEEDOR", idproveedor);
                                command.ExecuteNonQuery();
                            }
                            correcto = "RELACION AGREGADA CORRECTAMENTE";

                        }
                    }
                    catch (Exception ex)
                    {
                        error = "NO PUDO AGREGARSE LA RELACION";
                        OnGet();
                    }
                }
                else { error = "NO PUDO AGREGARSE LA RELACION"; }
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
