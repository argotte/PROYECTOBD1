using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Productores
{
    public class CrearModel : PageModel
    {
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<CiudadModelo> listaCiudades = new List<CiudadModelo>();
        public ProductorModelo productorModelo = new ProductorModelo();
        public string error = "";
        public string correcto = "";
        public string pais, ciudad;

        public void OnGet()
        {
        }
        public void OnPost()
        {
            productorModelo.NOMBRE = Request.Form["NOMBRE"];
            productorModelo.ENVASE_ESTANDAR = Request.Form["ENVASE_ESTANDAR"];
            productorModelo.DIRECCION = Request.Form["DIRECCION"];
            productorModelo.NOMBREPAIS = Request.Form["NOMBREPAIS"];
            productorModelo.NOMBRECIUDAD = Request.Form["NOMBRECIUDAD"];
            

            if (productorModelo.NOMBREPAIS.Length == 0 || productorModelo.NOMBRECIUDAD.Length == 0 || productorModelo.NOMBRE.Length == 0 || productorModelo.DIRECCION.Length == 0)
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
                        command.Parameters.AddWithValue("@PAIS", productorModelo.NOMBREPAIS);
                        command.Parameters.AddWithValue("@CIUDAD", productorModelo.NOMBRECIUDAD);
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
                productorModelo.FK_ID_PAIS = pais;
                productorModelo.FK_ID_CIUDAD = ciudad;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_PRODUCTORES" +
                                "(NOMBRE,FK_ID_PAIS,FK_ID_CIUDAD,ENVASE_ESTANDAR,DIRECCION,FK_COOPERATIVA) VALUES" +
                                "(@NOMBRE,@FK_ID_PAIS,@FK_ID_CIUDAD,@ENVASE_ESTANDAR,@DIRECCION,@COOPERATIVA);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", productorModelo.NOMBRE);
                        command.Parameters.AddWithValue("@FK_ID_PAIS", pais);
                        command.Parameters.AddWithValue("@FK_ID_CIUDAD", ciudad);
                        command.Parameters.AddWithValue("@ENVASE_ESTANDAR", productorModelo.ENVASE_ESTANDAR);
                        command.Parameters.AddWithValue("@DIRECCION", productorModelo.DIRECCION);
                        command.Parameters.AddWithValue("COOPERATIVA",1);
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
            productorModelo.NOMBRE = ""; productorModelo.FK_ID_CIUDAD = ""; productorModelo.FK_ID_PAIS = ""; productorModelo.NOMBREPAIS = ""; productorModelo.NOMBRECIUDAD = ""; productorModelo.DIRECCION = "";
            productorModelo.ENVASE_ESTANDAR = "";

            correcto = "PRODUCTOR AGREGADO CORRECTAMENTE";
            Response.Redirect("/Mantenimiento/PRODUCTORES/Index");
        }
    }
}
