namespace PROYECTOBD1.Modelos
{
    public class RenovacionModelo
    {
        public string FK_ID_PRODUCTOR { get; set; }
        public string FK_ID_CLIENTE { get; set; }
        public string FK_ID_CONTRATO { get; set; }
        public string ID { get; set; }
        public string FECHARENOVACION { get; set; }
        public string TOTALDOLARES { get;set; }

        //////////
        ///
        public string NOMBREPRODUCTOR { get;set;}
        public string NOMBRECLIENTE { get; set; }

    }
}
