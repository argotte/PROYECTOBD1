using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Nodes;

namespace PROYECTOBD1.Pages.Productores
{
    public class IndexModel : PageModel 
    {
        //  public IEnumerable<PaisModelo> paisdisplay { get; set; }
        public string pais, error;
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<CiudadModelo> listaCiudades = new List<CiudadModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        string connectionString = "";
        Connection connection2 = new Connection();
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
                                paisModelo.ID = ""+reader.GetInt32(0);
                                paisModelo.NOMBRE = reader.GetString(1);
                                paisModelo.CONTINENTE =reader.GetString(2);
                         //       paisModelo.Ciudades= new List<PadrinosModelo>();
                                listaPaises.Add(paisModelo);
                            }
                        }
                    }
                }
                //PadrinosModelo ciudadModelo = new PadrinosModelo();

            }
            catch (Exception ex)
            {

                error=ex.Message;
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
                                    ciudadModelo.FK_ID_PAIS= "" + reader.GetInt32(1);
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
                    String sql = "SELECT PR.ID,PR.NOMBRE AS PRODUCTOR,PR.ENVASE_ESTANDAR AS ENVASE,PR.FK_COOPERATIVA AS COOPERATIVA, PR.DIRECCION AS DIRECCION, " +
                        "PR.FK_ID_CIUDAD, PR.FK_ID_PAIS, " +
                        "CI.NOMBRE AS CIUDAD, PA.NOMBRE AS PAIS " +
                        "FROM DJR_PRODUCTORES AS PR " +
                        "INNER JOIN DJR_CIUDADES CI ON PR.FK_ID_CIUDAD=CI.ID " +
                        "INNER JOIN DJR_PAISES PA ON CI.FK_ID_PAIS=PA.ID " +
                        "WHERE PA.NOMBRE=@PAIS";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PAIS", pais);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                ProductorModelo productorModelo  = new ProductorModelo();
                                productorModelo.ID               = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0):"";
                                productorModelo.NOMBRE           = (reader.IsDBNull(1) != true) ? reader.GetString(1):"";
                                productorModelo.ENVASE_ESTANDAR  = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                productorModelo.FK_COOPERATIVA   = (reader.IsDBNull(3) != true) ? ""+reader.GetInt32(3):"NINGUNA";
                                productorModelo.DIRECCION        = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                productorModelo.FK_ID_CIUDAD     = (reader.IsDBNull(5) != true) ? ""+reader.GetInt32(5) : "";
                                productorModelo.FK_ID_PAIS       = (reader.IsDBNull(6) != true) ? ""+reader.GetInt32(6) : "";
                                productorModelo.NOMBRECIUDAD     = (reader.IsDBNull(7) != true) ? reader.GetString(7) : "";
                                productorModelo.NOMBREPAIS       = (reader.IsDBNull(8) != true) ? reader.GetString(8) : "";

                                listaProductor.Add(productorModelo);
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
                Redirect("Productores/Index");
            }
            //RedirectToAction("OnGet()");
            OnGet();
            //return Page();

        }
    }



}

