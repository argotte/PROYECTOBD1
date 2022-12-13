using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion.Criterios
{
    public class MostrarModel : PageModel
    {
        Connection connection2 = new Connection();
        // connectionString = connection2.ConnectionString;
        String connectionString = "";
        public List<CriterioModelo> listaCri=new List<CriterioModelo>();
        public List<ClienteModelo> listaClientes = new List<ClienteModelo>();
        public string cliente;
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID,C.NOMBRE FROM DJR_CLIENTES AS C"; //"SELECT C.ID,C.NOMBRE,C.DESCRIPCION,C.TIPO FROM DJR_CRITERIOS_VAR C";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteModelo cliente = new ClienteModelo();
                                cliente.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                cliente.NOMBRE = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                listaClientes.Add(cliente);
                                /*CriterioModelo criterio=new CriterioModelo(); 
                                criterio.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                criterio.NOMBRE= (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                criterio.DESCRIPCION = (reader.IsDBNull(2) != true) ? "" + reader.GetString(2) : "";
                                criterio.TIPO = (reader.IsDBNull(3) != true) ? "" + reader.GetString(3) : "";
                                listaCri.Add(criterio);*/
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                cliente = Request.Form["cliente"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT CR.ID, CR.NOMBRE, CR.DESCRIPCION, CR.TIPO " +
                                 "FROM  DJR_CRITERIOS_VAR CR, " +
                                 "DJR_CLIENTES AS C, DJR_FORMULAS AS F " +
                                 "WHERE CR.ID=F.FK_ID_CRITERIO_VAR " +
                                 "AND C.ID=F.FK_ID_CLIENTE AND C.NOMBRE=@CLIENTE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CLIENTE", cliente);
                        //Console.WriteLine(cliente);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //      command.Parameters.AddWithValue("@PAIS", pais);

                            while (reader.Read())
                            {
                                CriterioModelo criterio = new CriterioModelo();
                                criterio.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                criterio.NOMBRE = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                criterio.DESCRIPCION = (reader.IsDBNull(2) != true) ? "" + reader.GetString(2) : "";
                                criterio.TIPO = (reader.IsDBNull(3) != true) ? "" + reader.GetString(3) : "";
                                listaCri.Add(criterio);
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
