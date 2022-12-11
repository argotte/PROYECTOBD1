using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Individuales
{
    public class IndexModel : PageModel
    {
        public List<ProveedorModelo> listaProveedor = new List<ProveedorModelo>();
        public List<ProductorModelo> listaProductor = new List<ProductorModelo>();        string error = "";
        public List<Prod_ProvModel> listapp = new List<Prod_ProvModel>();
        string productor = "";
        public string idregion = "";
        String connectionString = "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
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
                OnGet();
            }
        }
        public void OnPost() 
        {
            productor = Request.Form["idproductor"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT PP.FK_ID_PRODUCTOR,PP.FK_ID_PROVEEDOR,PROV.NOMBRE,PROD.NOMBRE FROM DJR_PROD_PROV AS PP "+
                                 "INNER JOIN DJR_PROVEEDORES AS PROV ON PROV.ID = PP.FK_ID_PROVEEDOR "+
                                 "INNER JOIN DJR_PRODUCTORES AS PROD ON PROD.ID = PP.FK_ID_PRODUCTOR " +
                                 "WHERE PP.FK_ID_PRODUCTOR=@IDPRODUCTOR";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@idproductor", productor);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Prod_ProvModel model = new Prod_ProvModel();
                                model.FK_ID_PRODUCTOR=""+reader.GetInt32(0);
                                model.FK_ID_PROVEEDOR=""+reader.GetInt32(1);
                                model.NOMBREPROVEEDOR = reader.GetString(2);
                                model.NOMBREPRODUCTOR=reader.GetString(3);
                                listapp.Add(model);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                error = ex.Message;
                OnGet();
            }
            OnGet();
        }
    }
}
