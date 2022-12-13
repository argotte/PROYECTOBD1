using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Recetas.Productor
{
    public class IndexPModel : PageModel
    {
        public Recetas_Info recetaInfo = new Recetas_Info();
        public List<ProductorModelo> listaProductores = new List<ProductorModelo>();
        public List<ClienteModelo> listaClientes = new List<ClienteModelo>();
        public string errorMessage = "";
        public string successMessage = "";
        //String connectionString = "Data Source=CHUBE;Initial Catalog=ProyectoCereza;Integrated Security=True";
        public Connection connection2 = new Connection();
        String connectionString = "";
        //    connectionString=connection2.ConnectionString;
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
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
                            listaProductores.Add(productorModelo);
                        }
                    }
                }
            }
        }

        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            recetaInfo.NOMBRE = Request.Form["NOMBRE"];
            recetaInfo.TIPO = Request.Form["TIPO"];
            recetaInfo.DESCRIPCION = Request.Form["DESCRIPCION"];
            recetaInfo.TIEMPOPREPARACION = Request.Form["TIEMPOPREPARACION"];
            recetaInfo.RACIONES = Request.Form["RACIONES"];
            recetaInfo.FK_ID_PRODUCTOR = Request.Form["PRODUCTOR"];

            if (recetaInfo.NOMBRE.Length == 0 || recetaInfo.TIPO.Length == 0 || recetaInfo.DESCRIPCION.Length == 0 || recetaInfo.TIEMPOPREPARACION.Length == 0 || recetaInfo.RACIONES.Length == 0)
            {
                errorMessage = "Todos los cambos son requeridos";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_RECETAS" +
                                 "(NOMBRE, TIPO, DESCRIPCION, TIEMPOPREPARACION, RACIONES, FK_ID_PRODUCTOR) VALUES " +
                                 "(@NOMBRE, @TIPO, @DESCRIPCION, @TIEMPOPREPARACION, @RACIONES, @PRODUCTOR);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", recetaInfo.NOMBRE);
                        command.Parameters.AddWithValue("@TIPO", recetaInfo.TIPO);
                        command.Parameters.AddWithValue("@DESCRIPCION", recetaInfo.DESCRIPCION);
                        command.Parameters.AddWithValue("@TIEMPOPREPARACION", recetaInfo.TIEMPOPREPARACION);
                        command.Parameters.AddWithValue("@RACIONES", recetaInfo.RACIONES);
                        command.Parameters.AddWithValue("@PRODUCTOR", recetaInfo.FK_ID_PRODUCTOR);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
            }
            

            recetaInfo.NOMBRE = "";
            recetaInfo.TIPO = "";
            recetaInfo.DESCRIPCION = "";
            recetaInfo.TIEMPOPREPARACION = "";
            recetaInfo.RACIONES = "";

            successMessage = "Nueva receta añadida correctamente";

            OnGet();

        }
    }
}
