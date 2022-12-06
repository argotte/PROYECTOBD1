namespace PROYECTOBD1.Modelos
{
    public class ProductorModelo
    {
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string ENVASE_ESTANDAR { get; set; }
        public string FK_ID_CIUDAD { get; set; }
        public string FK_ID_PAIS { get; set; }
        public string FK_COOPERATIVA { get; set; }
    }
}
