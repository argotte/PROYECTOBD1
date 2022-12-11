namespace PROYECTOBD1.Modelos
{
    public class AsociacionesModelo
    {
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string FK_ID_REGION { get; set; }
        public string FK_ID_PAIS { get; set; }

        ////
        ///
        public string NOMBREPAIS { get; set; }
        public string NOMBREREGION { get; set; }
    }
}
