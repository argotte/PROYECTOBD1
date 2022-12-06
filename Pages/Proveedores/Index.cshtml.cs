using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Proveedores
{
    public class IndexModel : PageModel
    {
        public List<ProveedorModelo> listaProveedor = new List<ProveedorModelo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PROV.ID AS ID, PROV.NOMBRE AS NOMBRE, TIPONEGOCIOS.NOMBRE AS \"TIPO DE NEGOCIO\", C.NOMBRE AS CIUDAD, PA.NOMBRE AS PAIS " +
                        "FROM DJR_PROVEEDORES AS PROV " +
                        "INNER JOIN DJR_TIPO_NEGOCIOS AS TIPONEGOCIOS " +
                        "ON PROV.FK_ID_TIPO_NEGOCIOS=TIPONEGOCIOS.ID " +
                        "INNER JOIN DJR_CIUDADES AS C " +
                        "ON PROV.FK_ID_CIUDAD=C.ID " +
                        "INNER JOIN DJR_PAISES AS PA " +
                        "ON C.FK_ID_PAIS = PA.ID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                ProveedorModelo productorModelo  = new ProveedorModelo();
                                productorModelo.ID               = (reader.IsDBNull(0)!= true)      ? "" + reader.GetInt32(0):"";
                                productorModelo.NOMBRE           = (reader.IsDBNull(1) != true)     ? reader.GetString(1):"";
                                productorModelo.FK_ID_TIPO_NEGOCIO  = (reader.IsDBNull(2) != true)  ? reader.GetString(2) : "";
                                productorModelo.FK_ID_CIUDAD     = (reader.IsDBNull(3) != true)     ? reader.GetString(3) : "";
                                productorModelo.FK_ID_PAIS       = (reader.IsDBNull(4) != true)     ? reader.GetString(4) : "";

                                listaProveedor.Add(productorModelo);
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
