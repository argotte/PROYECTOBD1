namespace PROYECTOBD1.Modelos
{
    public class DetalleContratoModelo
    {
        public string FK_ID_PRODUCTOR { get; set; }
        public string FK_ID_CLIENTE { get; set; }
        public string FK_ID_CONTRATO { get; set; }
        public string ID { get; set; }
        public string FECHAENVIO { get; set; }
        public string FECHAENVIOMAX { get; set; }
        public string CANTIDAD_KG { get; set; }
        public string PORCENTAJEDESCUENTO { get; set; }

        ////
        ////
        ///
        public string NOMBREPRODUCTOR { get; set; }
        public string NOMBRECLIENTE { get; set; }
    ////    public string NOMBRECONTRATO { }
    }
}
