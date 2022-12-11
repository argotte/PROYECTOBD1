using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROYECTOBD1.Modelos;
using System.Data.SqlClient;

namespace PROYECTOBD1.Pages.Evaluacion.Formulas
{
    public class CrearModel : PageModel
    {
        String connectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
        public FormulaModelo formula = new FormulaModelo();
        public string error = "";
        public string correcto = "";
        public string cliente, crit;
        public void OnGet()
        {
        }

        public void OnPost()
        {
            formula.TIPO = Request.Form["TIPO"];
            formula.PORCENTAJEIMPORTANCIA = Request.Form["PORCENTAJEIMPORTANCIA"];
            formula.PORCENTAJEACEPTACION = Request.Form["PORCENTAJEACEPTACION"];
            formula.NOMBRECLIENTE = Request.Form["NOMBRECLIENTE"];
            formula.NOMBRECRITERIO = Request.Form["NOMBRECRITERIO"];
            if (formula.TIPO.Length == 0 || formula.PORCENTAJEIMPORTANCIA.Length == 0 || formula.PORCENTAJEACEPTACION.Length == 0 || formula.NOMBRECLIENTE.Length == 0 || formula.NOMBRECRITERIO.Length == 0)
            {
                error = "TODOS LOS CAMPOS SON REQUERIDOS";
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT C.ID,P.ID FROM DJR_CLIENTES C, DJR_CRITERIOS_VAR P WHERE P.NOMBRE= @CRITERIO AND C.NOMBRE= @CLIENTE";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CLIENTE", formula.NOMBRECLIENTE);
                        command.Parameters.AddWithValue("@CRITERIO", formula.NOMBRECRITERIO);
                        // command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cliente = "" + reader.GetInt32(0);
                                crit = "" + reader.GetInt32(1);
                            }
                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO DJR_FORMULAS" +
                                "(TIPO,PORCENTAJEIMPORTANCIA,PORCENTAJEACEPTACION,FK_ID_CLIENTE,FK_ID_CRITERIO_VAR) VALUES" +
                                "(@TIPO,@PORCENTAJEIMPORTANCIA,@PORCENTAJEACEPTACION,@FK_ID_CLIENTE,@FK_ID_CRITERIO_VAR);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TIPO", formula.TIPO);
                        command.Parameters.AddWithValue("@PORCENTAJEIMPORTANCIA", formula.PORCENTAJEIMPORTANCIA);
                        command.Parameters.AddWithValue("@PORCENTAJEACEPTACION", formula.PORCENTAJEACEPTACION);
                        command.Parameters.AddWithValue("@FK_ID_CLIENTE", cliente);
                        command.Parameters.AddWithValue("@FK_ID_CRITERIO_VAR", crit);
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
            formula.TIPO = ""; formula.PORCENTAJEIMPORTANCIA = ""; formula.PORCENTAJEACEPTACION = ""; formula.NOMBRECLIENTE = ""; formula.NOMBRECRITERIO = "";
            correcto = "FORMULA AGREGADA CORRECTAMENTE";
            Response.Redirect("/Evaluacion/Formulas/Mostrar");
        }
    }
}
