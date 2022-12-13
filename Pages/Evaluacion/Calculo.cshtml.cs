using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion
{
    public class CalculoModel : PageModel
    {
        public Connection connection2 = new Connection();
        String connectionString = "";
        public List<CriterioModelo> listaCri = new List<CriterioModelo>();
        public List<CriterioModelo> criteriosAgregados = new List<CriterioModelo>();
        public List<ClienteModelo> listaClientes = new List<ClienteModelo>();
        ClienteModelo clienteCri = new ClienteModelo();
        ProductorModelo prodCri = new ProductorModelo();
        CriterioModelo crit = new CriterioModelo();
        //public List<ClienteModelo> clientesAgregados = new List<ClienteModelo>();
        public List<ProductorModelo> listaProd = new List<ProductorModelo>();
       // public List<string> prodAgregados = new List<string>();
        public string productorAux="";
        public string criterio="";
        public string cliente="";
        public string num;
        public string importancia;
        public string aceptacion;
        public double res;
        public double aux;
        public string evaAceptacion;
        public string decision;

        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID,C.NOMBRE FROM DJR_CRITERIOS_VAR C";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                CriterioModelo criterio=new CriterioModelo(); 
                                criterio.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                criterio.NOMBRE= (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                /* criterio.DESCRIPCION = (reader.IsDBNull(2) != true) ? "" + reader.GetString(2) : "";
                                criterio.TIPO = (reader.IsDBNull(3) != true) ? "" + reader.GetString(3) : "";*/
                                listaCri.Add(criterio);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID,C.NOMBRE FROM DJR_CLIENTES C";
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
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID,C.NOMBRE FROM DJR_PRODUCTORES C";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                ProductorModelo prod = new ProductorModelo();
                                prod.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                prod.NOMBRE = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                listaProd.Add(prod);
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
                num= Request.Form["num"];
                criterio = Request.Form["criterio"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                   // Console.WriteLine(cliente);
                    connection.Open();
                    String sql = "SELECT C.ID, C.NOMBRE " +
                                "FROM  DJR_CRITERIOS_VAR C " +
                                // "DJR_CLIENTES AS C, DJR_FORMULAS AS F " +
                                "WHERE C.NOMBRE=@CRITERIO";
                    // "AND C.ID=F.FK_ID_CLIENTE AND C.NOMBRE=@CLIENTE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CRITERIO", criterio);
                        //Console.WriteLine(cliente);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //      command.Parameters.AddWithValue("@PAIS", pais);

                            while (reader.Read())
                            {
                                CriterioModelo criteri = new CriterioModelo();
                                criteri.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                criteri.NOMBRE = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                crit = criteri;
                                criterio = crit.ID;
                            }
                        }
                    }
                }
                //criteriosAgregados.Add(criterio);
                cliente = Request.Form["cliente"];
               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID, C.NOMBRE " +
                                "FROM  DJR_CLIENTES C " +
                                // "DJR_CLIENTES AS C, DJR_FORMULAS AS F " +
                                "WHERE C.NOMBRE=@CLIENTE ";
                                // "AND C.ID=F.FK_ID_CLIENTE AND C.NOMBRE=@CLIENTE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CLIENTE", cliente);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //      command.Parameters.AddWithValue("@PAIS", pais);

                            while (reader.Read())
                            {
                                ClienteModelo cliente2 = new ClienteModelo();
                                cliente2.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                cliente2.NOMBRE = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                clienteCri = cliente2;
                                cliente = clienteCri.ID;
                            }
                        }
                    }
                }

                productorAux = Request.Form["productor"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID, C.NOMBRE " +
                                "FROM  DJR_PRODUCTORES C " +
                                // "DJR_CLIENTES AS C, DJR_FORMULAS AS F " +
                                "WHERE C.NOMBRE=@PRODUCTOR ";
                    // "AND C.ID=F.FK_ID_CLIENTE AND C.NOMBRE=@CLIENTE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PRODUCTOR", productorAux);
                        //Console.WriteLine(cliente);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //      command.Parameters.AddWithValue("@PAIS", pais);

                            while (reader.Read())
                            {
                                ProductorModelo prod = new ProductorModelo();
                                prod.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                prod.NOMBRE = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                prodCri = prod;
                                productorAux = prodCri.ID;
                            }
                        }
                    }
                }

                /**/

                if (prodCri != null||clienteCri!=null)
                {
                    
                   // foreach (var item in criteriosAgregados)
                    //{
                        
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            String sql = "SELECT F.PORCENTAJEIMPORTANCIA, C.PORCENTAJE_ACEPTACION " +
                                        "FROM DJR_FORMULAS AS F " +
                                        "INNER JOIN DJR_CRITERIOS_VAR CR ON F.FK_ID_CRITERIO_VAR=CR.ID " +
                                        "INNER JOIN DJR_CLIENTES AS C ON F.FK_ID_CLIENTE=C.ID " +
                                        "WHERE CR.ID=@CRITERIO AND C.ID=@CLIENTE";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {

                                command.Parameters.AddWithValue("@CRITERIO", criterio);
                                command.Parameters.AddWithValue("@CLIENTE", cliente);
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        importancia = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                        aceptacion = (reader.IsDBNull(1) != true) ? "" + reader.GetInt32(1) : "";
                                    }
                                }
                            }
                        }
                    
                    aux = int.Parse(importancia) * int.Parse(num);
                        res += (aux/100);
                    Console.WriteLine(num);
                    Console.WriteLine(aux);
                    Console.WriteLine(res);

                    //}
                    if (res < int.Parse(aceptacion))
                    {
                        evaAceptacion = "REPROBADO";
                        decision = "NO RENOVAR";
                    }
                    else
                    {
                        evaAceptacion = "APROBADO";
                        decision = "RENOVAR";
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        
                        String sql="UPDATE DJR_EVALUACIONES "+
                                    "SET PORCENTAJE_RESULTADO=@PORCENTAJE_RESULTADO, RESULTADO=@RESULTADO,DECISIONFINAL=@DECISIONFINAL " +
                                    "WHERE FK_ID_PRODUCTOR=@PRODUCTOR AND FK_ID_CLIENTE=@CLIENTE";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@PRODUCTOR", productorAux);
                            command.Parameters.AddWithValue("@CLIENTE", cliente);
                            command.Parameters.AddWithValue("@RESULTADO", evaAceptacion);
                            command.Parameters.AddWithValue("@PORCENTAJE_RESULTADO", res);
                            command.Parameters.AddWithValue("@DECISIONFINAL", decision);
                            command.ExecuteNonQuery();
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
