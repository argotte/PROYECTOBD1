using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion.Formulas
{
    public class MostrarModel : PageModel
    {
        public List<FormulaModelo> listaFor = new List<FormulaModelo>();
        String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT F.ID,F.TIPO,F.PORCENTAJEIMPORTANCIA,F.PORCENTAJEACEPTACION,F.FK_ID_CLIENTE,F.FK_ID_CRITERIO_VAR FROM DJR_FORMULAS F";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FormulaModelo formula=new FormulaModelo();
                                formula.ID = (reader.IsDBNull(0) != true) ? "" + reader.GetInt32(0) : "";
                                formula.TIPO = (reader.IsDBNull(1) != true) ? "" + reader.GetString(1) : "";
                                formula.PORCENTAJE_IMPORTANCIA = (reader.IsDBNull(2) != true) ? "" + reader.GetInt32(2) : "";
                                formula.PORCENTAJE_ACEPTACION = (reader.IsDBNull(3) != true) ? "" + reader.GetInt32(3) : "";
                                formula.FK_ID_CLIENTE = (reader.IsDBNull(4) != true) ? "" + reader.GetInt32(4) : "";
                                formula.FK_ID_CRITERIO = (reader.IsDBNull(5) != true) ? "" + reader.GetInt32(5) : "";
                                //       paisModelo.Ciudades= new List<CiudadModelo>();
                                listaFor.Add(formula);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
