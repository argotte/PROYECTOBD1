namespace PROYECTOBD1.Modelos
{
    public class PagosModelo
    {
        public string FK_ID_DETALLE_CONTRATO { get; set; }
        public string FK_ID_PRODUCTOR { get; set; }
        public string FK_ID_CLIENTE { get; set; }
        public string FK_ID_CONTRATO { get; set; }
        public string ID { get; set; }
        public string FECHA { get; set; }
        public string MONTO_DOLARES { get; set; }

        /////
        ///
        public string NOMBREPRODUCTOR { get; set; }
        public string NOMBRECLIENTE { get; set; }
    }
}
