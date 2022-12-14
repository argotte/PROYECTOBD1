using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        public string pais;
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<CiudadModelo> listaCiudades = new List<CiudadModelo>();
        public List<ClienteModelo> listaClientes = new List<ClienteModelo>();
        public Connection connection2 = new Connection();
        String connectionString = "";
      //  String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";

        public void OnGet() 
        {
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT P.ID,P.NOMBRE,P.CONTINENTE FROM DJR_PAISES AS P";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PaisModelo paisModelo = new PaisModelo();
                                paisModelo.ID = "" + reader.GetInt32(0);
                                paisModelo.NOMBRE = reader.GetString(1);
                                paisModelo.CONTINENTE = reader.GetString(2);
                                //       paisModelo.Ciudades= new List<PadrinosModelo>();
                                listaPaises.Add(paisModelo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            foreach (var item in listaPaises)
            {

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "SELECT C.ID, C.FK_ID_PAIS, C.NOMBRE FROM DJR_CIUDADES AS C " +
                                     "INNER JOIN DJR_PAISES P " +
                                     "ON C.FK_ID_PAIS=P.ID " +
                                     "WHERE P.ID = @IDPAIS";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@IDPAIS", Int32.Parse(item.ID));
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                item.Ciudades = new List<CiudadModelo>();
                                while (reader.Read())
                                {
                                    CiudadModelo ciudadModelo = new CiudadModelo();
                                    ciudadModelo.ID = "" + reader.GetInt32(0);
                                    ciudadModelo.FK_ID_PAIS = "" + reader.GetInt32(1);
                                    ciudadModelo.NOMBRE = reader.GetString(2);
                                    item.Ciudades.Add(ciudadModelo);
                                }
                            }
                        }
                    }
                    //PadrinosModelo ciudadModelo = new PadrinosModelo();

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                pais = Request.Form["pais"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID,C.NOMBRE , C.MISION, C.RANGO_ESCALA,C.PORCENTAJE_ACEPTACION,C.FK_ID_CIUDAD, C.FK_ID_PAIS, " +
                                 "CI.NOMBRE, PA.NOMBRE " +
                                 "FROM DJR_CLIENTES AS C " +
                                 "INNER JOIN DJR_CIUDADES AS CI ON C.FK_ID_CIUDAD=CI.ID " +
                                 "INNER JOIN DJR_PAISES AS PA ON CI.FK_ID_PAIS=PA.ID " +
                                 "WHERE PA.NOMBRE=@PAIS";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PAIS", pais);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                      //      command.Parameters.AddWithValue("@PAIS", pais);
                            
                            while (reader.Read())
                            { 
                                ClienteModelo clienteModelo = new ClienteModelo();
                                clienteModelo.ID            = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0):"";
                                clienteModelo.NOMBRE        = (reader.IsDBNull(1) != true) ? reader.GetString(1):"";
                                clienteModelo.MISION        = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                clienteModelo.RANGO_ESCALA = (reader.IsDBNull(3) != true) ? "" + reader.GetInt32(3) : "";
                                clienteModelo.PORCENTAJE_ACEPTACION = (reader.IsDBNull(4) != true) ? ""+reader.GetInt32(4) : "";
                                clienteModelo.FK_ID_CIUDAD  = (reader.IsDBNull(5) != true) ? ""+reader.GetInt32(5):"";
                                clienteModelo.FK_ID_PAIS    = (reader.IsDBNull(6) != true) ? ""+reader.GetInt32(6):"";
                                clienteModelo.NOMBRECIUDAD  = (reader.IsDBNull(7) != true) ? reader.GetString(7) : "";
                                clienteModelo.NOMBREPAIS    = (reader.IsDBNull(8) != true) ? reader.GetString(8) : "";
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
            OnGet();
        }
    }



}
