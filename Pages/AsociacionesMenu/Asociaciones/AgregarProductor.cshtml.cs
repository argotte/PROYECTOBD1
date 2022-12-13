using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Asociaciones
{
    public class AgregarProductorModel : PageModel
    {
        public Connection connection2 = new Connection();
        String connectionString = "";
        //   String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public AsociacionesModelo asociacionesModelo = new AsociacionesModelo();
        public List<AsociacionesModelo> listaAsociaciones = new List<AsociacionesModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public ProductorModelo productorModelo = new ProductorModelo();
        public string error = "";
        public string correcto = "";
        public string idproductor, idpaisproductor,idasociacion,idpaisasociacion = "";
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
              //  OnGet();
            }
        }
        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            asociacionesModelo.ID = Request.Query["id"];
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
                    String sql = "select PROD.ID,PA.ID from DJR_PRODUCTORES PROD INNER JOIN DJR_PAISES PA ON PROD.FK_ID_PAIS=PA.ID where PROD.ID=@IDPRODUCTOR";
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
                    String sql = "select A.ID,P.ID FROM DJR_ASOCIACIONES A " +
                                 "INNER JOIN DJR_REGIONES R ON A.FK_ID_REGION = R.ID " +
                                 "INNER JOIN DJR_PAISES P ON R.FK_ID_PAISES = P.ID " +
                                 "WHERE A.ID = @IDASOCIACION ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDASOCIACION", asociacionesModelo.ID);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idasociacion = "" + reader.GetInt32(0);
                                idpaisasociacion = "" + reader.GetInt32(1);
                            }
                        }
                    }
                }


                int result = string.Compare(idpaisproductor, idpaisasociacion);
                if (result==0)//efectivamente estan en el mismo pais. Ahora a revisar si ya esta repetido o no.
                {
                    lo_anado = true;
                    try //AHORA REVISAREMOS SI YA ESTA EN LA ASOCIACION PARA QUE NO SE REPITA JUEPUTA
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            String sql = "select pa.FK_ID_PRODUCTOR from DJR_P_A PA " +
                                         "inner join DJR_PRODUCTORES PROD on PROD.ID=PA.FK_ID_PRODUCTOR " +
                                         "WHERE PROD.ID=@IDPRODUCTOR";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@IDPRODUCTOR", idproductor);
                                // command.ExecuteNonQuery();
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        idproductor = "" + reader.GetInt32(0);
                                        lo_anado=false;
                                    }
                                }
                            }
                        }

                        
                    }
                    catch (Exception EX)
                    {
                        lo_anado=false;
                        error = EX.Message;
                        
                    }
                    
                }
                if (lo_anado==true)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            String sql = "INSERT INTO DJR_P_A" +
                                        "(FK_ID_ASOCIACION,FK_ID_PRODUCTOR) VALUES" +
                                        "(@IDASOCIACION,@IDPRODUCTOR);";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@IDASOCIACION", idasociacion);
                                command.Parameters.AddWithValue("@IDPRODUCTOR", idproductor);
                                command.ExecuteNonQuery();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        error = "EL PRODUCTOR NO PUDO SER AGREGADO";
                        OnGet();
                    }
                    correcto = "PRODUCTOR AGREGADO CORRECTAMENTE A LA ASOCIACION";        //
                }
                if (lo_anado==false)
                {
                    error = "EL PRODUCTOR NO PUDO SER AGREGADO";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                OnGet();

            }
            //ahora salva la info en la bd
         //   correcto = "CLIENTE AGREGADO CORRECTAMENTE";
            // Response.Redirect("/Mantenimiento/Clientes/Index");
            OnGet();
        }
    }
}
