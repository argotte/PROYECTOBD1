using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion.Evaluaciones
{
    public class MostrarModel : PageModel
    {
        Connection connection2 = new Connection();
       // connectionString = connection2.ConnectionString;
        String connectionString = "";
        public List<EvaluacionModelo> listaEva = new List<EvaluacionModelo>();
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
                    String sql = "SELECT C.ID,C.NOMBRE FROM DJR_CLIENTES AS C";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteModelo cliente = new ClienteModelo();
                                cliente.ID  =   (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                cliente.NOMBRE  = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                listaClientes.Add(cliente);
                                /* EvaluacionModelo evaluacion = new EvaluacionModelo();
                                 evaluacion.ANIO= (reader.IsDBNull(0) != true) ? "" + reader.GetString(0) : "";
                                 evaluacion.FECHAEVALUACION = (reader.IsDBNull(1) != true) ? "" + reader.GetDateTime(1) : "";
                                 evaluacion.DECISIONFINAL = (reader.IsDBNull(2) != true) ? "" + reader.GetString(2) : "";
                                 evaluacion.RESULTADO = (reader.IsDBNull(3) != true) ? "" + reader.GetString(3) : "";
                                 evaluacion.PORCENTAJE_RESULTADO = (reader.IsDBNull(4) != true) ? "" + reader.GetDecimal(4) : "";
                                 evaluacion.FK_ID_PRODUCTOR = (reader.IsDBNull(5) != true) ? "" + reader.GetInt32(5) : "";
                                 evaluacion.FK_ID_CLIENTE = (reader.IsDBNull(6) != true) ? "" + reader.GetInt32(6) : "";
                                 listaEva.Add(evaluacion);*/
                            }
                        }
                    }
                }
                /*using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.NOMBRE,P.NOMBRE FROM DJR_CLIENTES C, DJR_PRODUCTORES P WHERE P.ID= @PRODUCTOR AND C.ID= @CLIENTE";
                    foreach (var item in listaEva)
                    {


                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@CLIENTE", item.FK_ID_CLIENTE);
                            command.Parameters.AddWithValue("@PRODUCTOR", item.FK_ID_PRODUCTOR);
                            // command.ExecuteNonQuery();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    item.NOMBRECLIENTE = "" + reader.GetString(0);
                                    item.NOMBREPRODUCTOR = "" + reader.GetString(1);
                                }
                            }
                        }
                    }
                }*/
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
                    String sql = "SELECT E.ANIO,E.FECHAEVALUACION,E.DECISIONFINAL,E.RESULTADO,E.PORCENTAJE_RESULTADO, E.FK_ID_CLIENTE, E.FK_ID_PRODUCTOR, C.NOMBRE,P.NOMBRE " +
                                 "FROM DJR_EVALUACIONES AS E,DJR_CLIENTES AS C, DJR_PRODUCTORES AS P " +
                                 //"INNER JOIN DJR_CLIENTES AS C ON E.FK_ID_CLIENTE=C.ID" + 
                                 //"INNER JOIN DJR_PRODUCTORES AS P ON E.FK_ID_PRODUCTOR=P.ID" +
                                 "WHERE E.FK_ID_CLIENTE=C.ID AND E.FK_ID_PRODUCTOR=P.ID AND C.NOMBRE=@CLIENTE ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CLIENTE", cliente);
                        //Console.WriteLine(cliente);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //      command.Parameters.AddWithValue("@PAIS", pais);

                            while (reader.Read())
                            {
                                EvaluacionModelo evaluacion = new EvaluacionModelo();
                                evaluacion.ANIO = (reader.IsDBNull(0) != true) ? "" + reader.GetString(0) : "";
                                evaluacion.FECHAEVALUACION = (reader.IsDBNull(1) != true) ? "" + reader.GetDateTime(1) : "";
                                evaluacion.DECISIONFINAL = (reader.IsDBNull(2) != true) ? "" + reader.GetString(2) : "";
                                evaluacion.RESULTADO = (reader.IsDBNull(3) != true) ? "" + reader.GetString(3) : "";
                                evaluacion.PORCENTAJE_RESULTADO = (reader.IsDBNull(4) != true) ? "" + reader.GetDecimal(4) : "";
                                evaluacion.FK_ID_PRODUCTOR = (reader.IsDBNull(5) != true) ? "" + reader.GetInt32(5) : "";
                                evaluacion.FK_ID_CLIENTE = (reader.IsDBNull(6) != true) ? "" + reader.GetInt32(6) : "";
                                evaluacion.NOMBRECLIENTE = (reader.IsDBNull(7) != true) ? "" + reader.GetString(7) : "";
                                evaluacion.NOMBREPRODUCTOR = (reader.IsDBNull(8) != true) ? "" + reader.GetString(8) : "";
                                listaEva.Add(evaluacion);
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
