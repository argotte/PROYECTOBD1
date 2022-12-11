using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Variedades
{
    public class CrearModel : PageModel
    {
        public Connection connection2 = new Connection();
        String connectionString = "";
        //connectionString = connection2.ConnectionString;
       // String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<VariedadesModelo> variedades = new List<VariedadesModelo>();
        public VariedadesModelo variedadesModelo = new VariedadesModelo();
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public string error = "";
        public string correcto = "";
        public string pais, ciudad;

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
                //PadrinosModelo ciudadModelo = new PadrinosModelo();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            variedadesModelo.PRECOCIDAD = Request.Form["PRECOCIDAD"];
            variedadesModelo.NOMBRE = Request.Form["NOMBRE"];
            variedadesModelo.FIRMEZA = Request.Form["FIRMEZA"];
            variedadesModelo.COLOR = Request.Form["COLOR"];
            variedadesModelo.ESPECIE = Request.Form["ESPECIE"];
            variedadesModelo.DESCRIPCION = Request.Form["DESCRIPCION"];
            variedadesModelo.PAISNOMBRE = Request.Form["pais"];
            

            if (variedadesModelo.PRECOCIDAD.Length == 0 || variedadesModelo.NOMBRE.Length == 0 || variedadesModelo.FIRMEZA.Length == 0 || variedadesModelo.COLOR.Length == 0 || variedadesModelo.ESPECIE.Length==0 || variedadesModelo.DESCRIPCION.Length==0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_VARIEDADES " +
                                "(PRECOCIDAD,NOMBRE,FIRMEZA,COLOR,ESPECIE,DESCRIPCION) VALUES" +
                                "(@PRECOCIDAD,@NOMBRE,@FIRMEZA,@COLOR,@ESPECIE,@DESCRIPCION);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PRECOCIDAD", variedadesModelo.PRECOCIDAD);
                        command.Parameters.AddWithValue("@NOMBRE", variedadesModelo.NOMBRE);
                        command.Parameters.AddWithValue("@FIRMEZA", variedadesModelo.FIRMEZA);
                        command.Parameters.AddWithValue("@COLOR", variedadesModelo.COLOR);
                        command.Parameters.AddWithValue("@ESPECIE", variedadesModelo.ESPECIE);
                        command.Parameters.AddWithValue("@DESCRIPCION", variedadesModelo.DESCRIPCION);
                        command.ExecuteNonQuery();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;
               
            }
            string idVariedad = "";
            try
            {  
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT V.ID FROM DJR_VARIEDADES AS V WHERE V.NOMBRE=@NOMBRE AND V.FIRMEZA = @FIRMEZA AND V.COLOR=@COLOR AND V.ESPECIE=@ESPECIE AND V.DESCRIPCION=@DESCRIPCION";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PRECOCIDAD", variedadesModelo.PRECOCIDAD);
                        command.Parameters.AddWithValue("@NOMBRE", variedadesModelo.NOMBRE);
                        command.Parameters.AddWithValue("@FIRMEZA", variedadesModelo.FIRMEZA);
                        command.Parameters.AddWithValue("@COLOR", variedadesModelo.COLOR);
                        command.Parameters.AddWithValue("@ESPECIE", variedadesModelo.ESPECIE);
                        command.Parameters.AddWithValue("@DESCRIPCION", variedadesModelo.DESCRIPCION);
                        command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idVariedad = "" + reader.GetInt32(0);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;

            }
            string idPais = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT P.ID FROM DJR_PAISES AS P WHERE P.NOMBRE=@PAIS ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PAIS", variedadesModelo.PAISNOMBRE);
                        command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idPais = "" + reader.GetInt32(0);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;

            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_PAISES_ORIGEN " +
                                "(FK_ID_PAIS,FK_ID_VARIEDAD) VALUES" +
                                "(@IDPAIS,@IDVARIEDAD);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDPAIS", idPais);
                        command.Parameters.AddWithValue("@IDVARIEDAD", idVariedad);
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
            variedadesModelo.PRECOCIDAD = ""; variedadesModelo.NOMBRE = ""; variedadesModelo.FIRMEZA = ""; variedadesModelo.COLOR = ""; variedadesModelo.ESPECIE = ""; variedadesModelo.DESCRIPCION = "";

            correcto = "VARIEDAD AGREGADA CORRECTAMENTE";
            Response.Redirect("/Variedades/Index");
        }
    }
}
