using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PROYECTOBD1.Modelos
{
    public class PaisModelo
    {
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string CONTINENTE { get; set; }

        public ICollection<CiudadModelo> Ciudades { get; set; }

       
        public int NumeroCiudades => Ciudades == null ? 0 : Ciudades.Count;
    }
}
