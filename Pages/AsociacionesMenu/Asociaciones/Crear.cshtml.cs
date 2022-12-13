using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Asociaciones
{
    public class CrearModel : PageModel
    {
        Connection connection2 = new Connection();
        //  connectionString = connection2.ConnectionString;
        String connectionString = "";
        public List<PaisModelo> listaPaises = new List<PaisModelo>();
        public List<VariedadesModelo> listaVariedades = new List<VariedadesModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();
        public List<CultivosModelo> listaCultivos = new List<CultivosModelo>();
        public List<RegionModelo> listaRegion = new List<RegionModelo>();
        public RegionModelo regionModelo = new RegionModelo();
        public AsociacionesModelo asociacionesModelo = new AsociacionesModelo();
        public CultivosModelo cultivosModelo = new CultivosModelo();
        public string error = "";
        public string correcto = "";
        public string idpais="";
        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT R.ID,R.NOMBRE FROM DJR_REGIONES R";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegionModelo regionModelo = new RegionModelo();
                                regionModelo.ID = "" + reader.GetInt32(0);
                                regionModelo.NOMBRE = reader.GetString(1);
                                listaRegion.Add(regionModelo);
                            }
                        }
                    }
                }
                //PadrinosModelo ciudadModelo = new PadrinosModelo();

            }
            catch (Exception ex)
            {

                error = ex.Message;
            }
        }

        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            regionModelo.ID = Request.Form["IDREGION"];
            asociacionesModelo.NOMBRE = Request.Form["NOMBREASOCIACION"];

            if (regionModelo.ID.Length==0 || asociacionesModelo.NOMBRE.Length==0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                OnGet();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT R.FK_ID_PAISES FROM DJR_REGIONES R WHERE R.ID=@IDREGION";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IDREGION", regionModelo.ID);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idpais = "" + reader.GetInt32(0);
                            }
                        }
                    }
                }//idpais, regionModelo.ID, asociacionesModelo.NOMBRE
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_ASOCIACIONES(NOMBRE,FK_ID_REGION,FK_ID_PAIS) VALUES(@NOMBRE,@IDREGION,@IDPAIS);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NOMBRE", asociacionesModelo.NOMBRE);
                        command.Parameters.AddWithValue("@IDREGION", regionModelo.ID);
                        command.Parameters.AddWithValue("@IDPAIS", idpais);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                OnGet();

            }
            //ahora salva la info en la bd
            //  productorModelo.NOMBRE = ""; productorModelo.FK_ID_CIUDAD = ""; productorModelo.FK_ID_PAIS = ""; productorModelo.NOMBREPAIS = ""; productorModelo.NOMBRECIUDAD = ""; productorModelo.DIRECCION = "";
            //   productorModelo.ENVASE_ESTANDAR = "";

            correcto = "PRODUCTOR AGREGADO CORRECTAMENTE";
            asociacionesModelo.NOMBRE = "";
            OnGet();
        }
    }
}
