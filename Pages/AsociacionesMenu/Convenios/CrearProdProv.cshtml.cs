using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Convenios
{
    public class CrearProdProvModel : PageModel
    {
        public Connection connection2 = new Connection();
        String connectionString = "";
        //   String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<ProveedorModelo> listaProveedor = new List<ProveedorModelo>();
        public AsociacionesModelo asociacionesModelo = new AsociacionesModelo();
        public List<AsociacionesModelo> listaAsociaciones = new List<AsociacionesModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
     //   public ProductorModelo productorModelo = new ProductorModelo();
        public string error = "";
        public string correcto = "";
        public string fechainicio, vigencia, idproveedor = "";
        public string idproductor, idpaisproductor, idasociacion, idpaisasociacion, idpaisproveedor = "";
        bool lo_anado = false;
        bool lo_anado1 = false;
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
                                ProductorModelo productoresModelo = new ProductorModelo();
                                productoresModelo.ID = "" + reader.GetInt32(0);
                                productoresModelo.NOMBRE = reader.GetString(1);
                                listaProductor.Add(productoresModelo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                error = "ASOCIACION NO PUDO SER AGREGADO";
                //  OnGet();
            }
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT A.ID, A.NOMBRE FROM DJR_PROVEEDORES AS A";
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
                //  OnGet();
            }
        }

        public void OnPost() 
        {
            connectionString = connection2.ConnectionString;
            idproductor = Request.Form["idproductor"];
            idproveedor = Request.Form["idproveedor"];
            fechainicio = Request.Form["fechainicio"];
            vigencia = Request.Form["vigencia"];

            if (idproductor == null || idproveedor == null || vigencia == null || fechainicio == null ||  idproveedor.Length == 0 || fechainicio.Length == 0 || vigencia.Length == 0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                OnGet();
            }
            // select c.fk_id_proveedor,c.fk_id_asociacion from djr_convenios c where  c.fk_id_proveedor=4 and c.fk_id_asociacion=5            ///                     

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select c.fk_id_proveedor,C.FK_ID_PRODUCTOR_PROD_PROV FROM DJR_CONVENIOS C WHERE C.FK_ID_PROVEEDOR = @IDPROVEEDOR AND C.FK_ID_PRODUCTOR_PROD_PROV=@IDPRODUCTOR ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        lo_anado = true;
                        command.Parameters.AddWithValue("@IDPROVEEDOR", idproveedor);
                        command.Parameters.AddWithValue("@IDPRODUCTOR", idproductor);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idproveedor = "" + reader.GetInt32(0);
                                idproductor = "" + reader.GetInt32(1);
                                lo_anado = false;
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select pp.FK_ID_PRODUCTOR, pp.FK_ID_PROVEEDOR from DJR_PROD_PROV pp WHERE PP.fk_id_productor=@IDPRODUCTOR and pp.fk_id_proveedor=@IDPROVEEDOR";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        lo_anado1 = false;
                        command.Parameters.AddWithValue("@IDPRODUCTOR", idproductor);
                        command.Parameters.AddWithValue("@IDPROVEEDOR", idproveedor);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idproveedor = "" + reader.GetInt32(0);
                                idproductor = "" + reader.GetInt32(1);
                                lo_anado1 = true;
                            }
                        }
                    }
                }

                if (lo_anado == true && lo_anado1 == true)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "INSERT INTO DJR_CONVENIOS(FK_ID_PRODUCTOR_PROD_PROV,FK_ID_PROVEEDOR_PROD_PROV,FK_ID_PROVEEDOR,FECHAINICIO,ESTADO_VIGENCIA) VALUES " +
                                     "(@IDPRODUCTOR,@IDPROVEEDOR,@IDPROVEEDOR1,@FECHAINICIO,@VIGENCIA);";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@IDPRODUCTOR", idproductor);
                            command.Parameters.AddWithValue("@IDPROVEEDOR", idproveedor);
                            command.Parameters.AddWithValue("@IDPROVEEDOR1", idproveedor);
                            command.Parameters.AddWithValue("@FECHAINICIO", fechainicio);
                            command.Parameters.AddWithValue("@VIGENCIA", vigencia);
                            command.ExecuteNonQuery();
                        }
                        correcto = "CONVENIO AGREGADO CORRECTAMENTE";
                    }

                }
                else
                {
                    error = "NO SE PUDO AGREGAR CONVENIO";
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                OnGet();

            }
            fechainicio = "";

            OnGet();
        }
    }
}
