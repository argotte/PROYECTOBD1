using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Convenios
{
    public class CrearAsoProvModel : PageModel
    {
        public Connection connection2 = new Connection();
        String connectionString = "";
        //   String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<ProveedorModelo> listaProveedor = new List<ProveedorModelo>();
        public AsociacionesModelo asociacionesModelo = new AsociacionesModelo();
        public List<AsociacionesModelo> listaAsociaciones = new List<AsociacionesModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public ProductorModelo productorModelo = new ProductorModelo();
        public string error = "";
        public string correcto = "";
        public string fechainicio, vigencia,idproveedor = "";
        public string idproductor, idpaisproductor, idasociacion, idpaisasociacion,idpaisproveedor = "";
        bool lo_anado = false;
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT A.ID, A.NOMBRE FROM DJR_ASOCIACIONES AS A";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AsociacionesModelo asociacionesModelo = new AsociacionesModelo();
                                asociacionesModelo.ID = "" + reader.GetInt32(0);
                                asociacionesModelo.NOMBRE = reader.GetString(1);
                                listaAsociaciones.Add(asociacionesModelo);
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
            idasociacion = Request.Form["idasociacion"];
            idproveedor = Request.Form["idproveedor"];
            fechainicio = Request.Form["fechainicio"];
            vigencia = Request.Form["vigencia"];

            if (idasociacion==null||idproveedor==null || vigencia==null|| fechainicio == null || idasociacion.Length == 0 || idproveedor.Length == 0 || fechainicio.Length==0 || vigencia.Length==0)
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
                    String sql = "select c.fk_id_proveedor,c.fk_id_asociacion from djr_convenios c where  c.fk_id_proveedor=@IDPROVEEDOR and c.fk_id_asociacion=@IDASOCIACION ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        lo_anado = true;
                        command.Parameters.AddWithValue("@IDPROVEEDOR", idasociacion);
                        command.Parameters.AddWithValue("@IDASOCIACiON", idproveedor);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idproveedor = "" + reader.GetInt32(0);
                                idasociacion = "" + reader.GetInt32(1);
                                lo_anado = false;
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = " select p.fk_id_pais from DJR_PROVEEDORES p WHERE p.ID=@idproveedor";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@idproveedor", idproveedor);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idpaisproveedor = "" + reader.GetInt32(0);
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "  select a.fk_id_pais from DJR_ASOCIACIONES a where a.id=@idasociacion";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@idasociacion", idasociacion);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idpaisasociacion = "" + reader.GetInt32(0);
                            }
                        }
                    }
                }
                //idpais, regionModelo.ID, asociacionesModelo.NOMBRE

                if (lo_anado==true && idpaisasociacion!=idpaisproveedor)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "INSERT INTO DJR_CONVENIOS(FK_ID_ASOCIACION,FK_ID_PROVEEDOR,FECHAINICIO,ESTADO_VIGENCIA) VALUES " +
                                     "(@IDASOCIACION,@IDPROVEEDOR,@FECHAINICIO,@VIGENCIA);";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@IDASOCIACION",idasociacion);
                            command.Parameters.AddWithValue("@IDPROVEEDOR", idproveedor);
                            command.Parameters.AddWithValue("@FECHAINICIO", fechainicio);

                            command.Parameters.AddWithValue("@VIGENCIA", vigencia);
                            command.ExecuteNonQuery();
                        }
                        correcto = "CONVENIO AGREGADO CORRECTAMENTE";
                    }
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
