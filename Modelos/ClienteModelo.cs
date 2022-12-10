namespace PROYECTOBD1.Modelos
{
    public class ClienteModelo
    {
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string MISION { get; set; }
        public string RANGO_ESCALA { get; set; }
        public string PORCENTAJE_ACEPTACION {  get; set; }
        public string FK_ID_CIUDAD { get; set; }
        public string FK_ID_PAIS { get; set; }

        /////////////////////
        ///
        public string NOMBRECIUDAD { get; set; }
        public string NOMBREPAIS { get; set; }
    }
}
