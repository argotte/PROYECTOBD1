using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Apadrinamiento
{
    public class IndexModel : PageModel
    {
        Connection connection2 = new Connection();
        // connectionString = connection2.ConnectionString;
        String connectionString = "";
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public List<PadrinosModelo> listaPadrinos = new List<PadrinosModelo>();
        public List<ProgramaApadrinamientoModelo> listaPam = new List<ProgramaApadrinamientoModelo>();
        string error = "";
        string productor = "";
        string padrino = "";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
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
        }

        public void OnPost() 
        {
            connectionString = connection2.ConnectionString;
            try
            {
                productor = Request.Form["productor"];
           //     padrino = Request.Form["padrino"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select PR.NOMBRE, PADRINOS.NOMBRE,PADRINOS.SEGUNDO_NOMBRE,PADRINOS.APELLIDO,PADRINOS.SEGUNDO_APELLIDO,PA.FECHA_INICIAL,PA.FECHA_FINAL,PA.APORTE_ANUAL,PADRINOS.DOC_IDENTIDAD, V.NOMBRE from DJR_PROGRAMAS_APADRINAMIENTO PA " +
                                 "inner join DJR_PRODUCTORES PR ON PR.ID = PA.FK_ID_PRODUCTOR " +
                                 "inner join DJR_PADRINOS PADRINOS ON PADRINOS.DOC_IDENTIDAD = PA.FK_DOC_IDENTIDAD_PADRINO " +
                                 "inner join DJR_VARIEDADES V ON V.ID=PA.FK_ID_VARIEDAD "+
                                 "WHERE PR.ID = @IDPRODUCTOR";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                         command.Parameters.AddWithValue("@IDPRODUCTOR", productor);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProgramaApadrinamientoModelo pam = new ProgramaApadrinamientoModelo();
                                pam.NOMBREPRODUCTOR = reader.GetString(0);
                                pam.NOMBREPADRINO = reader.GetString(1);
                                pam.NOMBREPADRINO = (reader.IsDBNull(2) != true) ? pam.NOMBREPADRINO + " " + reader.GetString(2) : pam.NOMBREPADRINO;
                                pam.NOMBREPADRINO = pam.NOMBREPADRINO+" "+reader.GetString(3);
                                pam.NOMBREPADRINO = (reader.IsDBNull(4) != true) ? pam.NOMBREPADRINO + " " + reader.GetString(4) : pam.NOMBREPADRINO;
                                pam.FECHAINICIO= (reader.IsDBNull(5) != true) ? reader.GetDateTime(5).ToString("yyyy-MM-dd") : "";
                                pam.FECHAFINAL= (reader.IsDBNull(6) != true) ? reader.GetDateTime(6).ToString("yyyy-MM-dd") : "";
                                pam.APORTEANUAL= (reader.IsDBNull(7) != true) ?"" +reader.GetInt32(7) : "";
                                pam.CEDULAPADRINO = "" + reader.GetInt32(8);
                                pam.NOMBREVARIEDAD = reader.GetString(9);
                                listaPam.Add(pam);
                            }
                        }
                    }
                }
                // return Page();  
            }
            // Redirect("Productores/Index");
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                Redirect("/Apadrinamiento/Index");
            }
            OnGet();
        }
    }
}
