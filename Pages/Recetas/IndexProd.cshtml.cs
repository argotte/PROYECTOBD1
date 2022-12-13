using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Recetas
{
    public class IndexProdModel : PageModel
    {
        public List<Modelos.Recetas_Info> listRecetas = new List<Modelos.Recetas_Info>();
        public Connection connection2 = new Connection();
        String connectionString = "";
        string productor = "";
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
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

                Console.WriteLine("Exception: " + ex.ToString());
            }

    
        }

        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            productor = Request.Form["productor"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT R.ID,R.NOMBRE,R.TIPO,R.DESCRIPCION,R.TIEMPOPREPARACION,R.RACIONES FROM DJR_RECETAS R INNER JOIN DJR_PRODUCTORES P ON P.ID=R.FK_ID_PRODUCTOR WHERE P.ID=@IDPRODUCTOR";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDPRODUCTOR", productor);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Modelos.Recetas_Info receta_info = new Modelos.Recetas_Info();
                                receta_info.ID = "" + reader.GetInt32(0);
                                receta_info.NOMBRE = reader.GetString(1);
                                receta_info.TIPO = reader.GetString(2);
                                receta_info.DESCRIPCION = reader.GetString(3);
                                receta_info.TIEMPOPREPARACION = "" + reader.GetInt32(4);
                                receta_info.RACIONES = "" + reader.GetInt32(5);
                                listRecetas.Add(receta_info);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            OnGet();
        }
    }
}
