namespace PROYECTOBD1.Modelos
{
    public class CondicionPagoModelo
    {
        public string FK_ID_DETALLE_CONTRATOS { get; set; }
        public string FK_ID_PRODUCTORDETALLE { get; set; }
        public string FK_ID_CLIENTE { get; set; }
        public string FK_ID_CONTRATO { get; set; }
        public string FK_ID_PRODUCTOR { get; set; }
        public string ID { get; set; }
        public string TIPO { get; set; }
        public string CANTIDADCUOTA { get; set; }
        public string PORCENTAJE_POR_CUOTA { get; set; }
        public string CONTADO_A_LA_EMISION { get; set; }
        public string CONTADO_AL_RECIBIR { get; set; }
        /////
        ///
        public string NOMBRECLIENTE { get; set; }
        public string NOMBREPRODUCTOR { get; set; }
    }
}
