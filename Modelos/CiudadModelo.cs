using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROYECTOBD1.Modelos
{

    public class CiudadModelo
    {
        public string ID { get; set; }
        public string NOMBRE { get; set; }

        public string FK_ID_PAIS { get; set; }

     
    }
}
