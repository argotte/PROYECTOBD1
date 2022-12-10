using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Productores.Cultivos
{
    public class CrearModel : PageModel
    {
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<VariedadesModelo> listaVariedades = new List<VariedadesModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public List<CultivosModelo> listaCultivos = new List<CultivosModelo>();
        public CultivosModelo cultivosModelo = new CultivosModelo();
        public string error = "";
        public string correcto = "";
        public string pais, ciudad;

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PR.ID,PR.NOMBRE AS PRODUCTOR,PR.ENVASE_ESTANDAR AS ENVASE,PR.FK_COOPERATIVA AS COOPERATIVA, PR.DIRECCION AS DIRECCION, " +
                        "PR.FK_ID_CIUDAD, PR.FK_ID_PAIS " +
                        "FROM DJR_PRODUCTORES AS PR ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductorModelo productorModelo = new ProductorModelo();
                                productorModelo.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                productorModelo.NOMBRE = (reader.IsDBNull(1) != true) ? reader.GetString(1) : "";
                                productorModelo.ENVASE_ESTANDAR = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                productorModelo.FK_COOPERATIVA = (reader.IsDBNull(3) != true) ? "" + reader.GetInt32(3) : "NINGUNA";
                                productorModelo.DIRECCION = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                productorModelo.FK_ID_CIUDAD = (reader.IsDBNull(5) != true) ? "" + reader.GetInt32(5) : "";
                                productorModelo.FK_ID_PAIS = (reader.IsDBNull(6) != true) ? "" + reader.GetInt32(6) : "";

                                listaProductor.Add(productorModelo);
                            }
                        }
                    }
                }
                //CiudadModelo ciudadModelo = new CiudadModelo();

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
                    String sql = "SELECT P.ID,P.PRECOCIDAD,P.NOMBRE,P.FIRMEZA,P.COLOR,P.ESPECIE,P.DESCRIPCION FROM DJR_VARIEDADES AS P";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VariedadesModelo variedadesModelo = new VariedadesModelo();
                                variedadesModelo.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                variedadesModelo.PRECOCIDAD = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                variedadesModelo.NOMBRE = (reader.IsDBNull(2) != true) ? reader.GetString(2) : "";
                                variedadesModelo.FIRMEZA = (reader.IsDBNull(3) != true) ? reader.GetString(3) : "";
                                variedadesModelo.COLOR = (reader.IsDBNull(4) != true) ? reader.GetString(4) : "";
                                variedadesModelo.ESPECIE = (reader.IsDBNull(5) != true) ? reader.GetString(5) : "";
                                variedadesModelo.DESCRIPCION = (reader.IsDBNull(6) != true) ? reader.GetString(6) : "";
                                listaVariedades.Add(variedadesModelo);

                                
                            }
                        }
                    }
                }
                //CiudadModelo ciudadModelo = new CiudadModelo();

            }
            catch (Exception ex)
            {

                error = ex.Message;
            }
        }
        public void OnPost()
        {
            cultivosModelo.NOMBREPRODUCTOR = Request.Form["NOMBREPRODUCTOR"];
            cultivosModelo.NOMBREVARIEDAD = Request.Form["NOMBREVARIEDAD"];
            cultivosModelo.CALIBRE = Request.Form["CALIBRE"];
            cultivosModelo.PRODUCCION_ESTIMADA_KG = Request.Form["PRODUCCIONESTIMADA"];
            cultivosModelo.PERIODO_INICIO_DISPONIBILIDAD = Request.Form["INICIODISPONIBILIDAD"];
            cultivosModelo.PERIODO_FIN_DISPONIBILIDAD = Request.Form["FINDISPONIBILIDAD"];
            cultivosModelo.MAX_DESTINADO_EXPORTACION = Request.Form["MAXDESTINADO"];


            if (cultivosModelo.NOMBREPRODUCTOR.Length == 0 || cultivosModelo.NOMBREVARIEDAD.Length == 0 || cultivosModelo.CALIBRE.Length == 0 || cultivosModelo.PRODUCCION_ESTIMADA_KG.Length == 0 
                || cultivosModelo.PERIODO_INICIO_DISPONIBILIDAD.Length==0 || cultivosModelo.MAX_DESTINADO_EXPORTACION.Length==0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                { 
                    connection.Open();
                    String sql = "select P.ID FROM DJR_PRODUCTORES AS P WHERE P.NOMBRE=@productor";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {            
                        command.Parameters.AddWithValue("@productor", cultivosModelo.NOMBREPRODUCTOR);
                       // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                cultivosModelo.FK_ID_PRODUCTOR = "" + reader.GetInt32(0);
                            }
                        }
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select V.ID FROM DJR_VARIEDADES AS V WHERE V.NOMBRE=@variedad";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@variedad", cultivosModelo.NOMBREVARIEDAD);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cultivosModelo.FK_ID_VARIEDAD = "" + reader.GetInt32(0);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_CULTIVOS" +
                                "(FK_ID_VARIEDAD,FK_ID_PRODUCTOR,CALIBRE,PRODUCCION_ESTIMADA,PERIODO_INICIO_DISPONIBILIDAD,PERIODO_FIN_DISPONIBILIDAD,MAX_DESTINADO_EXPORTACION) VALUES" +
                                "(@IDVARIEDAD,@IDPRODUCTOR,@CALIBRE,@PRODUCCIONESTIMADA,@PERIODOINICIO,@PERIODOFIN,@MAXEXPORTACION);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDVARIEDAD", cultivosModelo.FK_ID_VARIEDAD);
                        command.Parameters.AddWithValue("@IDPRODUCTOR", cultivosModelo.FK_ID_PRODUCTOR);
                        command.Parameters.AddWithValue("@CALIBRE", cultivosModelo.CALIBRE);
                        command.Parameters.AddWithValue("@PRODUCCIONESTIMADA", cultivosModelo.PRODUCCION_ESTIMADA_KG);
                        command.Parameters.AddWithValue("@PERIODOINICIO", cultivosModelo.PERIODO_INICIO_DISPONIBILIDAD);
                        command.Parameters.AddWithValue("@PERIODOFIN",cultivosModelo.PERIODO_FIN_DISPONIBILIDAD);
                        command.Parameters.AddWithValue("@MAXEXPORTACION", cultivosModelo.MAX_DESTINADO_EXPORTACION);
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
          //  productorModelo.NOMBRE = ""; productorModelo.FK_ID_CIUDAD = ""; productorModelo.FK_ID_PAIS = ""; productorModelo.NOMBREPAIS = ""; productorModelo.NOMBRECIUDAD = ""; productorModelo.DIRECCION = "";
         //   productorModelo.ENVASE_ESTANDAR = "";

            correcto = "PRODUCTOR AGREGADO CORRECTAMENTE";
            Response.Redirect("/Mantenimiento/Productores/Index");
        }
    }
}
