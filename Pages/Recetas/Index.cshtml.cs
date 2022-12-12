using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Recetas
{
    public class IndexModel : PageModel
    {
        public List<Modelos.Recetas_Info> listRecetas = new List<Modelos.Recetas_Info>();
        public Connection connection2 = new Connection();
        String connectionString = "";
        public string cliente = "";
        public List<ClienteModelo> listaClientes = new List<ClienteModelo>();
    //    connectionString=connection2.ConnectionString;

        //  String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID, C.NOMBRE FROM DJR_CLIENTES AS C";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteModelo clienteModelo= new ClienteModelo();
                                clienteModelo.ID = "" + reader.GetInt32(0);
                                clienteModelo.NOMBRE = reader.GetString(1);
                                listaClientes.Add(clienteModelo);
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
            cliente = Request.Form["cliente"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT R.ID,R.NOMBRE,R.TIPO,R.DESCRIPCION,R.TIEMPOPREPARACION,R.RACIONES FROM DJR_RECETAS R INNER JOIN DJR_CLIENTES C ON C.ID=R.FK_ID_CLIENTE WHERE C.ID=@IDCLIENTE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDCLIENTE", cliente);

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
