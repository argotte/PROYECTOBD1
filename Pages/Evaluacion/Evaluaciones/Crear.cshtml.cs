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
        public string cliente;
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
            evaluacion.FK_ID_CLIENTE = Request.Form["FK_ID_CLIENTE"];
            evaluacion.FK_ID_PRODUCTOR = Request.Form["FK_ID_PRODUCTOR"];
            if (evaluacion.FK_ID_CLIENTE.Length == 0 || evaluacion.FK_ID_PRODUCTOR.Length == 0 || evaluacion.ANIO.Length == 0 || evaluacion.FECHAEVALUACION.Length == 0 || evaluacion.DECISIONFINAL.Length == 0 || evaluacion.RESULTADO.Length == 0 || evaluacion.PORCENTAJE_RESULTADO.Length == 0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT P.ID,C.ID FROM DJR_PAISES AS P INNER JOIN DJR_CIUDADES C ON P.ID=C.FK_ID_PAIS WHERE P.NOMBRE= @PAIS AND C.NOMBRE= @CIUDAD";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PAIS", clienteModelo.FK_ID_PAIS);
                        command.Parameters.AddWithValue("@CIUDAD", clienteModelo.FK_ID_CIUDAD);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pais = "" + reader.GetInt32(0);
                                ciudad = "" + reader.GetInt32(1);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_CLIENTES" +
                                "(NOMBRE,FK_ID_PAIS,FK_ID_CIUDAD) VALUES" +
                                "(@NOMBRE,@FK_ID_PAIS,@FK_ID_CIUDAD);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", clienteModelo.NOMBRE);
                        command.Parameters.AddWithValue("@FK_ID_PAIS", pais);
                        command.Parameters.AddWithValue("@FK_ID_CIUDAD", ciudad);
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
            clienteModelo.NOMBRE = ""; clienteModelo.FK_ID_CIUDAD = ""; clienteModelo.FK_ID_PAIS = "";
            correcto = "CLIENTE AGREGADO CORRECTAMENTE";
            Response.Redirect("/Mantenimiento/Clientes/Index");
        }
    }
}
