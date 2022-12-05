using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        public List<ClienteInfo> listaClientes = new List<ClienteInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM CLIENTES";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteInfo clienteInfo = new ClienteInfo();
                                clienteInfo.ID = "" + reader.GetInt32(0);
                                clienteInfo.NOMBRE = reader.GetString(1);
                                clienteInfo.FK_ID_CIUDAD = "" + reader.GetInt32(2);
                                clienteInfo.FK_ID_PAIS = "" + reader.GetInt32(3);
                                listaClientes.Add(clienteInfo);
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

    public class ClienteInfo
    {
        public string? ID;
        public string? NOMBRE;
        public string? FK_ID_CIUDAD;
        public string? FK_ID_PAIS;

    }
}
