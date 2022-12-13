using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Recetas.Ingredientes
{
    public class CrearIModel : PageModel
    {
        public IngredienteModelo ingredienteMIInfo = new IngredienteModelo();
        public IngredienteRealModelo ingredienteInfo = new IngredienteRealModelo();
        public MedidaModelo medidaInfo = new MedidaModelo();
        public List<IngredienteModelo> listIngrediente = new List<IngredienteModelo>();
        public string errorMessage = "";
        public string successMessage = "";
        public Connection connection2 = new Connection();
        String connectionString = "";
        public string medida1 = "PESO";
        public string medida2 = "VOLUMEN";
        public string idReceta = "";
        IngredienteRealModelo ingredienteIn = new IngredienteRealModelo();
        public void OnGet()
        {
            
        }
        public void OnPost() 
        {
            idReceta = Request.Query["id"];
            connectionString = connection2.ConnectionString;
            ingredienteInfo.NOMBRE = Request.Form["NOMBRE"];
            medidaInfo.NOMBRE = Request.Form["MEDIDA"];
            int resultado;
            if((resultado = string.Compare(medidaInfo.NOMBRE,"GRAMOS"))==0 || (resultado = string.Compare(medidaInfo.NOMBRE, "CUCHARADAS")) == 0 || (resultado = string.Compare(medidaInfo.NOMBRE, "CUCHARADITAS")) == 0 || (resultado = string.Compare(medidaInfo.NOMBRE, "AL GUSTO")) == 0)
            {
                medidaInfo.TIPO = "PESO";
            }
            else
            {
                medidaInfo.TIPO = "VOLUMEN";
            }
            ingredienteMIInfo.CANTIDAD = Request.Form["CANTIDAD"];

            if (ingredienteInfo.NOMBRE == null || medidaInfo.NOMBRE == null || ingredienteMIInfo.CANTIDAD == null)
            {
                errorMessage = "Todos los cambos son requeridos";
                return;
            }

            if (ingredienteInfo.NOMBRE.Length == 0 || medidaInfo.NOMBRE.Length == 0  || ingredienteMIInfo.CANTIDAD.Length == 0)
            {
                errorMessage = "Todos los cambos son requeridos";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_INGREDIENTES" +
                                 "(NOMBRE) VALUES " +
                                 "(@NOMBRE);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", ingredienteInfo.NOMBRE);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_MEDIDAS" +
                                 "(NOMBRE, TIPO) VALUES " +
                                 "(@NOMBRE, @TIPO);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", medidaInfo.NOMBRE);
                        command.Parameters.AddWithValue("@TIPO", medidaInfo.TIPO);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT I.ID FROM DJR_INGREDIENTES AS I " +
                                 "WHERE I.NOMBRE = @NOMBRE";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@NOMBRE", ingredienteInfo.NOMBRE);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                             //   IngredienteRealModelo ingredienteIn = new IngredienteRealModelo();
                                ingredienteIn.ID = "" + reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT M.ID FROM DJR_MEDIDAS AS M " +
                                 "WHERE M.NOMBRE = @NOMBRE";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@NOMBRE", medidaInfo.NOMBRE);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //  MedidaModelo medidaI = new MedidaModelo();
                                medidaInfo.ID = "" + reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_M_I" +
                                 "(FK_ID_RECETA, FK_ID_INGREDIENTE, FK_ID_MEDIDA, CANTIDAD) VALUES " +
                                 "(@IDRECETA, @IDINGREDIENTE, @IDMEDIDA, @CANTIDAD);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDRECETA",idReceta);
                        command.Parameters.AddWithValue("@IDINGREDIENTE", ingredienteIn.ID);
                        command.Parameters.AddWithValue("@IDMEDIDA", medidaInfo.ID);
                        command.Parameters.AddWithValue("@CANTIDAD", ingredienteMIInfo.CANTIDAD);
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                OnGet();
            }

            ingredienteInfo.NOMBRE = "";
            medidaInfo.NOMBRE = "";
            ingredienteMIInfo.CANTIDAD = "";

            successMessage = "Nuevo ingrediente añadido.";

            OnGet();

        }
    }
}
