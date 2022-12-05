using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        public List<ClienteModelo> listaClientes = new List<ClienteModelo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select C.ID,C.NOMBRE AS CLIENTE,CI.NOMBRE AS CIUDAD,PA.NOMBRE AS PAIS FROM DJR_CLIENTES AS C " +
                        "INNER JOIN DJR_CIUDADES CI  ON C.FK_ID_CIUDAD = CI.ID " +
                        "INNER JOIN DJR_PAISES   PA  ON CI.FK_ID_PAIS = PA.ID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteModelo clienteModelo = new ClienteModelo();
                                clienteModelo.ID = "" + reader.GetInt32(0);
                                clienteModelo.NOMBRE = reader.GetString(1);
                                clienteModelo.FK_ID_CIUDAD = reader.GetString(2);
                                clienteModelo.FK_ID_PAIS = reader.GetString(3);
                                listaClientes.Add(clienteModelo);
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
