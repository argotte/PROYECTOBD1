using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion.Evaluaciones
{
    public class MostrarModel : PageModel
    {
        public List<EvaluacionModelo> listaEva = new List<EvaluacionModelo>();
        String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT E.ANIO,E.FECHAEVALUACION, E.DECISIONFINAL,E.RESULTADO,E.PORCENTAJE_RESULTADO,E.FK_ID_PRODUCTOR, E.FK_ID_CLIENTE FROM DJR_EVALUACIONES E";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EvaluacionModelo evaluacion = new EvaluacionModelo();
                                evaluacion.ANIO= (reader.IsDBNull(0) != true) ? "" + reader.GetString(0) : "";
                                evaluacion.FECHAEVALUACION = (reader.IsDBNull(1) != true) ? "" + reader.GetDateTime(1) : "";
                                evaluacion.DECISIONFINAL = (reader.IsDBNull(2) != true) ? "" + reader.GetString(2) : "";
                                evaluacion.RESULTADO = (reader.IsDBNull(3) != true) ? "" + reader.GetString(3) : "";
                                evaluacion.PORCENTAJE_RESULTADO = (reader.IsDBNull(4) != true) ? "" + reader.GetDecimal(4) : "";
                                evaluacion.FK_ID_PRODUCTOR = (reader.IsDBNull(5) != true) ? "" + reader.GetInt32(5) : "";
                                evaluacion.FK_ID_CLIENTE = (reader.IsDBNull(6) != true) ? "" + reader.GetInt32(6) : "";
                                //       paisModelo.Ciudades= new List<CiudadModelo>();
                                listaEva.Add(evaluacion);
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
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
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
