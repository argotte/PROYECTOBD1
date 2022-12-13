using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Recetas.Ingredientes
{
    public class IngredienteModel : PageModel
    {
        public IngredienteModelo ingrediente = new IngredienteModelo();
        public List<IngredienteModelo> listIngredientes = new List<IngredienteModelo>();
        public string errorMessage = "";
        public string successMessage = "";
        public string idReceta = "";
        //    String connectionString = "Data Source=CHUBE;Initial Catalog=ProyectoCereza;Integrated Security=True";
        public Connection connection2 = new Connection();
        String connectionString = "";
        //    connectionString=connection2.ConnectionString;
        public void OnGet()
        {
            idReceta = Request.Query["id"];
            connectionString = connection2.ConnectionString;
            string idR = Request.Query["ID"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT ING.NOMBRE,  ME.NOMBRE, MI.CANTIDAD FROM DJR_M_I AS MI " +
                                 "INNER JOIN DJR_INGREDIENTES AS ING ON ING.ID = MI.FK_ID_INGREDIENTE " +
                                 "INNER JOIN DJR_MEDIDAS AS ME ON ME.ID = MI.FK_ID_MEDIDA " +
                                 "INNER JOIN DJR_RECETAS AS R ON R.ID = MI.FK_ID_RECETA " +
                                 "WHERE MI.FK_ID_RECETA = @IDR";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDR", idR);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IngredienteModelo ingredienteIn = new IngredienteModelo();
                                ingredienteIn.NOMBREINGREDIENTE = reader.GetString(0);
                                ingredienteIn.NOMBREMEDIDA = reader.GetString(1);
                                ingredienteIn.CANTIDAD = "" + reader.GetInt32(2);
                                listIngredientes.Add(ingredienteIn);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            string idRV = Request.Query["ID"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT VA.NOMBRE,  ME.NOMBRE, RV.CANTIDAD FROM DJR_R_V AS RV " +
                                 "INNER JOIN DJR_VARIEDADES AS VA ON VA.ID = RV.FK_ID_VARIEDAD " +
                                 "INNER JOIN DJR_MEDIDAS AS ME ON ME.ID = 2 " +
                                 "INNER JOIN DJR_RECETAS AS R ON R.ID = RV.FK_ID_RECETA " +
                                 "WHERE RV.FK_ID_RECETA = @IDRV";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDRV", idRV);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IngredienteModelo ingredienteIn = new IngredienteModelo();
                                ingredienteIn.NOMBREINGREDIENTE = reader.GetString(0);
                                ingredienteIn.NOMBREMEDIDA = reader.GetString(1);
                                ingredienteIn.CANTIDAD = "" + reader.GetInt32(2);
                                listIngredientes.Add(ingredienteIn);
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
    }
}
