using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROYECTOBD1.Modelos
{
    [Table("Ciudades")]
    public class CiudadModelo
    {
        [Key]
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        [Key]
        public string FK_ID_PAIS { get; set; }
        public PaisModelo Pais { get; set; }

     
    }
}
