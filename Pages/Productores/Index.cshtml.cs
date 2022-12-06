using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Productores
{
    public class IndexModel : PageModel
    {
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PR.ID,PR.NOMBRE AS PRODUCTOR,PR.ENVASE_ESTANDAR AS ENVASE,PR.FK_COOPERATIVA AS COOPERATIVA, PR.DIRECCION AS DIRECCION,CI.NOMBRE AS CIUDAD, PA.NOMBRE AS PAIS FROM DJR_PRODUCTORES AS PR INNER JOIN DJR_CIUDADES CI ON PR.FK_ID_CIUDAD=CI.ID INNER JOIN DJR_PAISES PA ON CI.FK_ID_PAIS=PA.ID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                ProductorModelo productorModelo  = new ProductorModelo();
                                productorModelo.ID               = (reader.IsDBNull(0) != null) ? "" + reader.GetInt32(0):"";
                                productorModelo.NOMBRE           = (reader.IsDBNull(1) != true) ? reader.GetString(1):"";
                                productorModelo.ENVASE_ESTANDAR  = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                productorModelo.FK_COOPERATIVA   = (reader.IsDBNull(3) != true) ? reader.GetString(3):"NINGUNA";
                                productorModelo.DIRECCION        = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                productorModelo.FK_ID_CIUDAD     = (reader.IsDBNull(5) != true) ? reader.GetString(5) : "";
                                productorModelo.FK_ID_PAIS       = (reader.IsDBNull(6) != true) ? reader.GetString(6) : "";
                                
                                listaProductor.Add(productorModelo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
    }



}
