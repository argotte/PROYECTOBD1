namespace PROYECTOBD1.Modelos
{
    public class PrecioVariedadesModelo
    {
        public string ID { get; set; }
        public string FECHAINICIO { get; set; }
        public string FECHAFIN { get; set; }
        public string PRECIO { get; set; }
        public string CALIBRE { get; set; }
        public string FK_ID_VARIEDADES { get; set; }
        public string FK_ID_PAISES { get; set; }

        ///////
        ///
        public string VARIEDAD { get; set; }
        public string PAIS { get; set; }
    }
}
