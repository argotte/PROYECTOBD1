using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace PROYECTOBD1.Pages.Productores
{
    public class IndexModel : PageModel
    {
        //  public IEnumerable<PaisModelo> paisdisplay { get; set; }
        public string pais = "";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<CiudadModelo> listaCiudades = new List<CiudadModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public void OnGet() 
        {
            
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
                                listaPaises.Add(paisModelo);
                            }
                        }
                    }
                }
                // CiudadModelo ciudadModelo = new CiudadModelo();
              
                //foreach (var item in listaPaises)
                //{
                //    int IDpais = Int32.Parse(item.ID);
                //    using (SqlConnection connection = new SqlConnection(connectionString))
                //    {
                    
                //        connection.Open();
                        
                //        String sql = "SELECT C.ID,C.FK_ID_PAIS,C.NOMBRE FROM DJR_CIUDADES AS C " +
                //                 "INNER JOIN DJR_PAISES P " +
                //                 "ON P.ID=C.FK_ID_PAIS" +
                //                 "WHERE P.ID=@ID";
                    
                //        using (SqlCommand command = new SqlCommand(sql, connection))
                //        {
                //            command.Parameters.AddWithValue("@ID", IDpais);
                //            using (SqlDataReader reader = command.ExecuteReader())
                //            {
                //                while (reader.Read())
                //                {
                //                    CiudadModelo ciudadModelo = new CiudadModelo();
                //                    ciudadModelo.ID = "" + reader.GetInt32(0);
                //                    ciudadModelo.FK_ID_PAIS = "" + reader.GetInt32(1);
                //                    ciudadModelo.NOMBRE= reader.GetString(2);
                //                    listaCiudades.Add(ciudadModelo);
                //                    item.Ciudades.Add(ciudadModelo);
                                    
                              
                //                }
                //            }
                //        }
                //    }

                //}
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult OnPost()
        {
            OnGet();
            try
            {
                pais = Request.Form["pais"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PR.ID,PR.NOMBRE AS PRODUCTOR,PR.ENVASE_ESTANDAR AS ENVASE,PR.FK_COOPERATIVA AS COOPERATIVA, PR.DIRECCION AS DIRECCION, " +
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
                                productorModelo.ID               = (reader.IsDBNull(0) != null) ? "" + reader.GetInt32(0):"";
                                productorModelo.NOMBRE           = (reader.IsDBNull(1) != true) ? reader.GetString(1):"";
                                productorModelo.ENVASE_ESTANDAR  = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                productorModelo.FK_COOPERATIVA   = (reader.IsDBNull(3) != true) ? reader.GetString(3):"NINGUNA";
                                productorModelo.DIRECCION        = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                productorModelo.FK_ID_CIUDAD     = (reader.IsDBNull(5) != true) ? reader.GetString(5) : "";
                                productorModelo.FK_ID_PAIS       = (reader.IsDBNull(6) != true) ? reader.GetString(6) : "";
                                
                                listaProductor.Add(productorModelo);
                            }
                        }
                    }
                }
                return Page();  
            }
           // Redirect("Productores/Index");
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                Redirect("Productores/Index");
            }
            //RedirectToAction("OnGet()");
            return Page();

        }
    }



}
