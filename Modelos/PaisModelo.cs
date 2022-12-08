using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PROYECTOBD1.Modelos
{
    [Table("Paises")]
    public class PaisModelo
    {
        [Key]
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string CONTINENTE { get; set; }

        public ICollection<CiudadModelo> Ciudades { get; set; }
       
        public int NumeroCiudades => Ciudades == null ? 0 : Ciudades.Count;
    }
}
