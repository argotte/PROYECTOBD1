using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Proveedores
{
    public class IndexModel : PageModel
    {
        string pais = "";
        public List<ProveedorModelo> listaProveedor = new List<ProveedorModelo>();
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
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
                                    ciudadModelo.FK_ID_PAIS = "" + reader.GetInt32(1);
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

            try
            {
                pais = Request.Form["pais"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PROV.ID,PROV.NOMBRE AS PROVEEDOR, " +
                        "PROV.FK_ID_CIUDAD, PROV.FK_ID_PAIS, " +
                        "CI.NOMBRE AS CIUDAD, PA.NOMBRE AS PAIS " +
                        "FROM DJR_PROVEEDORES AS PROV " +
                        "INNER JOIN DJR_CIUDADES CI ON PROV.FK_ID_CIUDAD=CI.ID " +
                        "INNER JOIN DJR_PAISES PA ON CI.FK_ID_PAIS=PA.ID " +
                        "WHERE PA.NOMBRE=@PAIS";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@PAIS", pais);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                ProveedorModelo proveedorModelo = new ProveedorModelo();
                                proveedorModelo.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                proveedorModelo.NOMBRE = (reader.IsDBNull(1) != true) ? reader.GetString(1) : "";
                                proveedorModelo.FK_ID_CIUDAD = (reader.IsDBNull(2) != true) ? "" + reader.GetInt32(2) : "";
                                proveedorModelo.FK_ID_PAIS = (reader.IsDBNull(3) != true) ? "" + reader.GetInt32(3) : "";
                                proveedorModelo.NOMBRECIUDAD = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                proveedorModelo.NOMBREPAIS = (reader.IsDBNull(5) != true) ? reader.GetString(5) : "";

                                listaProveedor.Add(proveedorModelo);
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
                Redirect("/Mantenimiento/Proveedores/Index");
            }
            //RedirectToAction("OnGet()");
            OnGet();
            //return Page();

        }



    }

    


}
