namespace PROYECTOBD1.Modelos
{
    public class ConveniosModel
    {
        public string FK_ID_ASOCIACION { get; set; }
        public string FK_ID_PROVEEDOR_PROD_PROV { get; set; }
        public string FK_ID_PRODUCTOR_PROD_PROV { get; set; }
        public string FK_ID_PROVEEDOR { get; set; }
        public string ID { get; set; }
        public string FECHAINICIO { get; set; }
        public string ESTADO_VIGENCIA { get; set; }

        public string NOMBREASOCIACION { get; set; }
        public string NOMBREPROVEEDORPP { get; set; }
        public string NOMBREPRODUCTORPP { get; set; }
    }
}
