using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Apadrinamiento
{
    public class CrearModel : PageModel
    {
       public string error = "";
        public string correcto = "";
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<VariedadesModelo> listaVariedades = new List<VariedadesModelo>();
        public List<PadrinosModelo> listaPadrinos = new List<PadrinosModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public string fechainicial, fechafinal, aporteanual,idpadrino,idproductor,idvariedad = "";
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT P.ID, P.NOMBRE FROM DJR_PRODUCTORES AS P";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductorModelo productorModelo = new ProductorModelo();
                                productorModelo.ID = "" + reader.GetInt32(0);
                                productorModelo.NOMBRE = reader.GetString(1);
                                listaProductor.Add(productorModelo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                error = ex.Message;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT V.ID, V.NOMBRE FROM DJR_VARIEDADES AS V";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VariedadesModelo variedadesModelo= new VariedadesModelo();
                                variedadesModelo.ID = "" + reader.GetInt32(0);
                                variedadesModelo.NOMBRE = reader.GetString(1);
                                listaVariedades.Add(variedadesModelo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                error = ex.Message;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PADRINO.DOC_IDENTIDAD, PADRINO.NOMBRE,PADRINO.SEGUNDO_NOMBRE,PADRINO.APELLIDO,PADRINO.SEGUNDO_APELLIDO FROM DJR_PADRINOS AS PADRINO";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PadrinosModelo pam = new PadrinosModelo();
                                pam.DOC_IDENTIDAD = ""+reader.GetInt32(0);
                                pam.NOMBRE = reader.GetString(1);
                                pam.SEGUNDO_NOMBRE = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                pam.APELLIDO = reader.GetString(3);
                                pam.SEGUNDO_APELLIDO = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                pam.NOMBRECOMPLETO = pam.NOMBRE + " " + pam.SEGUNDO_NOMBRE + " " + pam.APELLIDO + " " + pam.SEGUNDO_APELLIDO;
                                listaPadrinos.Add(pam);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                error = ex.Message;
            }
        }

        public void OnPost() 
        {
            idpadrino = Request.Form["idpadrino"];
            idproductor = Request.Form["idproductor"];
            idvariedad = Request.Form["idvariedad"];
            fechainicial = Request.Form["fechainicial"];
            fechafinal = Request.Form["fechafinal"];
            aporteanual = Request.Form["aporteanual"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_PROGRAMAS_APADRINAMIENTO " +
                                "(FK_DOC_IDENTIDAD_PADRINO,FK_ID_VARIEDAD,FK_ID_PRODUCTOR,FECHA_INICIAL,APORTE_ANUAL,FECHA_FINAL) VALUES" +
                                "(@IDPADRINO,@IDVARIEDAD,@IDPRODUCTOR,@FECHAINICIAL,@APORTEANUAL,@FECHAFINAL);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDPADRINO", idpadrino);
                        command.Parameters.AddWithValue("@IDVARIEDAD", idvariedad);
                        command.Parameters.AddWithValue("@IDPRODUCTOR", idproductor);
                        command.Parameters.AddWithValue("@FECHAINICIAL", fechainicial);
                        command.Parameters.AddWithValue("@APORTEANUAL", aporteanual);
                        command.Parameters.AddWithValue("@FECHAFINAL", fechafinal);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return;

            }
            OnGet();
        }
    }
}
