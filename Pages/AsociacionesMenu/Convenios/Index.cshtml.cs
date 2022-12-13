using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace PROYECTOBD1.Pages.AsociacionesMenu.Convenios
{
    public class Index : PageModel
    {
        Connection connection2 = new Connection();
        // connectionString = connection2.ConnectionString;
        String connectionString = "";
        public List<ConveniosModel> listaConvenios = new List<ConveniosModel>();
        public string error = "";
        public string combobox = "";
        string padrino = "";
        public bool boolproductor=false;

        public void OnGet()
        {
            connectionString = connection2.ConnectionString;
        }
        public void OnPost()
        {
            connectionString = connection2.ConnectionString;
            try
            {
                combobox = Request.Form["combobox"];
                if (String.Equals(combobox, "PRODUCTOR"))
                {
                    combobox = "select C.ID, PROD.NOMBRE, PROV.NOMBRE,C.FECHAINICIO,C.ESTADO_VIGENCIA from djr_convenios as C inner join DJR_PROD_PROV PP on C.FK_ID_PRODUCTOR_PROD_PROV = PP.FK_ID_PRODUCTOR and C.FK_ID_PROVEEDOR_PROD_PROV = PP.FK_ID_PROVEEDOR INNER JOIN DJR_PRODUCTORES PROD on PROD.ID = PP.FK_ID_PRODUCTOR INNER JOIN DJR_PROVEEDORES PROV ON PROV.ID = PP.FK_ID_PROVEEDOR";
                    boolproductor = true;
                }
                else
                {
                    combobox = "select C.ID, ASO.NOMBRE,PROV.NOMBRE,C.FECHAINICIO,C.ESTADO_VIGENCIA  from djr_convenios as C inner join DJR_ASOCIACIONES AS ASO on C.FK_ID_ASOCIACION=ASO.ID inner join DJR_PROVEEDORES AS PROV ON C.FK_ID_PROVEEDOR = PROV.ID";
                    boolproductor=false;
                }
                //     padrino = Request.Form["padrino"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = combobox;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                       // command.Parameters.AddWithValue("@", combobox);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (boolproductor==true)
                            {
                                while (reader.Read())
                                {
                                    ConveniosModel conveniosModel = new ConveniosModel();
                                    conveniosModel.ID = "" + reader.GetInt32(0);
                                    conveniosModel.NOMBREPRODUCTORPP = reader.GetString(1);
                                    conveniosModel.NOMBREPROVEEDORPP = reader.GetString(2);
                                    conveniosModel.FECHAINICIO = reader.GetDateTime(3).ToString("yyyy-MM-dd");
                                    conveniosModel.ESTADO_VIGENCIA = reader.GetString(4);
                                    listaConvenios.Add(conveniosModel);
                                }
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    ConveniosModel conveniosModel = new ConveniosModel();
                                    conveniosModel.ID = "" + reader.GetInt32(0);
                                    conveniosModel.NOMBREASOCIACION = reader.GetString(1);
                                    conveniosModel.NOMBREPROVEEDORPP = reader.GetString(2);
                                    conveniosModel.FECHAINICIO = reader.GetDateTime(3).ToString("yyyy-MM-dd");
                                    conveniosModel.ESTADO_VIGENCIA = reader.GetString(4);
                                    listaConvenios.Add(conveniosModel);

                                }
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
                OnGet();
            }
            OnGet();
        }
    }
}
