using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Proveedores
{
    public class CrearModel : PageModel
    {
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<CiudadModelo> listaCiudades = new List<CiudadModelo>();
        public List<TipoNegociosModelo> listaTipoNegocios = new List<TipoNegociosModelo>();
        public ProveedorModelo proveedorModelo = new ProveedorModelo();
        public string error = "";
        public string correcto = "";
        public string pais, ciudad, IDtiponegocio;

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT TN.ID, TN.NOMBRE FROM DJR_TIPO_NEGOCIOS AS TN";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TipoNegociosModelo tipoNegocios = new TipoNegociosModelo();
                                tipoNegocios.ID = "" + reader.GetInt32(0);
                                tipoNegocios.NOMBRE = reader.GetString(1);
                                listaTipoNegocios.Add(tipoNegocios);
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
            proveedorModelo.NOMBRE = Request.Form["NOMBRE"];
            proveedorModelo.TIPONEGOCIO = Request.Form["TIPONEGOCIO"];
            proveedorModelo.NOMBREPAIS = Request.Form["NOMBREPAIS"];
            proveedorModelo.NOMBRECIUDAD = Request.Form["NOMBRECIUDAD"];
            

            if (proveedorModelo.NOMBREPAIS.Length == 0 || proveedorModelo.NOMBRECIUDAD.Length == 0 || proveedorModelo.NOMBRE.Length == 0)
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
                        command.Parameters.AddWithValue("@PAIS", proveedorModelo.NOMBREPAIS);
                        command.Parameters.AddWithValue("@CIUDAD", proveedorModelo.NOMBRECIUDAD);
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
                proveedorModelo.FK_ID_PAIS = pais;
                proveedorModelo.FK_ID_CIUDAD = ciudad;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT TN.ID FROM DJR_TIPO_NEGOCIOS AS TN " +
                                 "WHERE TN.NOMBRE=@TIPONEGOCIO";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TIPONEGOCIO", proveedorModelo.TIPONEGOCIO);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               IDtiponegocio=""+reader.GetInt32(0);
                            }
                        }
                    }
                }
                proveedorModelo.FK_ID_TIPO_NEGOCIO = IDtiponegocio;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_PROVEEDORES" +
                                "(NOMBRE,FK_ID_PAIS,FK_ID_CIUDAD,FK_ID_TIPO_NEGOCIOS) VALUES" +
                                "(@NOMBRE,@FK_ID_PAIS,@FK_ID_CIUDAD,@TIPONEGOCIO);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", proveedorModelo.NOMBRE);
                        command.Parameters.AddWithValue("@FK_ID_PAIS", pais);
                        command.Parameters.AddWithValue("@FK_ID_CIUDAD", ciudad);
                        command.Parameters.AddWithValue("@TIPONEGOCIO", IDtiponegocio);
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
            proveedorModelo.NOMBRE = ""; proveedorModelo.FK_ID_CIUDAD = ""; proveedorModelo.FK_ID_PAIS = ""; proveedorModelo.NOMBREPAIS = ""; proveedorModelo.NOMBRECIUDAD = "";
            correcto = "PRODUCTOR AGREGADO CORRECTAMENTE";
            Response.Redirect("/Mantenimiento/Proveedores/Index");
        }
    }
}
