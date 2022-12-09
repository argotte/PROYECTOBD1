namespace PROYECTOBD1.Modelos
{
    public class ProveedorModelo
    {
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string FK_ID_TIPO_NEGOCIO { get; set; }
        public string FK_ID_CIUDAD { get; set; }
        public string FK_ID_PAIS { get; set; }
        ///////
        ///
        public string NOMBRECIUDAD {get; set;}
        public string NOMBREPAIS { get; set; }
        public string TIPONEGOCIO { get; set; }
    }
}
