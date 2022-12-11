using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion.Evaluaciones
{
    public class CrearModel : PageModel
    {
        String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public List<ClienteModelo> listaClientes = new List<ClienteModelo>();
        public EvaluacionModelo evaluacion = new EvaluacionModelo();
        public string error = "";
        public string correcto = "";
        public string cliente,prod;
        public void OnGet()
        {
        }

        public void OnPost()
        {
            evaluacion.ANIO = Request.Form["ANIO"];
            evaluacion.FECHAEVALUACION = Request.Form["FECHAEVALUACION"];
            evaluacion.DECISIONFINAL = Request.Form["DECISIONFINAL"];
            evaluacion.RESULTADO = Request.Form["RESULTADO"];
            evaluacion.PORCENTAJE_RESULTADO = Request.Form["PORCENTAJE_RESULTADO"];
            evaluacion.NOMBRECLIENTE = Request.Form["NOMBRECLIENTE"];
            evaluacion.NOMBREPRODUCTOR = Request.Form["NOMBREPRODUCTOR"];
            if (evaluacion.NOMBRECLIENTE.Length == 0 || evaluacion.NOMBREPRODUCTOR.Length == 0 || evaluacion.ANIO.Length == 0 || evaluacion.FECHAEVALUACION.Length == 0 || evaluacion.DECISIONFINAL.Length == 0 || evaluacion.RESULTADO.Length == 0 || evaluacion.PORCENTAJE_RESULTADO.Length == 0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID,P.ID FROM DJR_CLIENTES C, DJR_PRODUCTORES P WHERE P.NOMBRE= @PRODUCTOR AND C.NOMBRE= @CLIENTE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CLIENTE", evaluacion.NOMBRECLIENTE);
                        command.Parameters.AddWithValue("@PRODUCTOR", evaluacion.NOMBREPRODUCTOR);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cliente = "" + reader.GetInt32(0);
                                prod = "" + reader.GetInt32(1);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_EVALUACIONES" +
                                "(ANIO,FECHAEVALUACION,DECISIONFINAL,RESULTADO,PORCENTAJE_RESULTADO,FK_ID_CLIENTE,FK_ID_PRODUCTOR) VALUES" +
                                "(@ANIO,@FECHAEVALUACION,@DECISIONFINAL,@RESULTADO,@PORCENTAJE_RESULTADO,@FK_ID_CLIENTE,@FK_ID_PRODUCTOR);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ANIO", evaluacion.ANIO);
                        command.Parameters.AddWithValue("@FECHAEVALUACION", evaluacion.FECHAEVALUACION);
                        command.Parameters.AddWithValue("@DECISIONFINAL", evaluacion.DECISIONFINAL);
                        command.Parameters.AddWithValue("@RESULTADO", evaluacion.RESULTADO);
                        command.Parameters.AddWithValue("@PORCENTAJE_RESULTADO", evaluacion.PORCENTAJE_RESULTADO);
                        command.Parameters.AddWithValue("@FK_ID_CLIENTE", cliente);
                        command.Parameters.AddWithValue("@FK_ID_PRODUCTOR", prod);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;

            }
            //ahora salva la info en la bd
            evaluacion.ANIO = ""; evaluacion.FECHAEVALUACION = ""; evaluacion.DECISIONFINAL = ""; evaluacion.RESULTADO = ""; evaluacion.PORCENTAJE_RESULTADO = ""; evaluacion.NOMBRECLIENTE = ""; evaluacion.NOMBREPRODUCTOR = "";
            correcto = "EVALUACION AGREGADA CORRECTAMENTE";
            Response.Redirect("/Evaluacion/Evaluaciones/Mostrar");
        }
    }
}
