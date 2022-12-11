namespace PROYECTOBD1.Modelos
{
    public class FormulaModelo
    {
        public string FK_ID_CRITERIO { get; set; }
        public string FK_ID_CLIENTE { get; set; }
        public string ID { get; set; }
        public string TIPO { get; set; }
        public string PORCENTAJE_IMPORTANCIA { get; set; }
        public string PORCENTAJE_ACEPTACION { get; set; }

        //////////////////////////////
        public string NOMBRECRITERIO { get; set; }
        public string NOMBRECLIENTE { get; set; }
    }
}
