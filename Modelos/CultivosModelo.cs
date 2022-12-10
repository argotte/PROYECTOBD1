namespace PROYECTOBD1.Modelos
{
    public class CultivosModelo
    {
        public string ID { get; set; }
        public string FK_ID_VARIEDAD { get; set; }
        public string FK_ID_PRODUCTOR { get; set; }
        public string CALIBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public string PRODUCCION_ESTIMADA_KG { get; set; }
        public string PERIODO_INICIO_DISPONIBILIDAD { get; set; }
        public string PERIODO_FIN_DISPONIBILIDAD { get; set; }
        public string MAX_DESTINADO_EXPORTACION { get; set; }
        /// <summary>
        /// //////////////////
        /// </summary>
        public string NOMBREVARIEDAD { get; set; }
        public string NOMBREPRODUCTOR { get; set; }
    }
}
