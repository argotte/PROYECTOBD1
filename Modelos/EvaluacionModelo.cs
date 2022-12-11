namespace PROYECTOBD1.Modelos
{
    public class EvaluacionModelo
    {
        public string ANIO { get; set; }
        public string FECHAEVALUACION { get; set; }
        public string DECISIONFINAL { get; set; }
        public string RESULTADO { get; set; }
        public string PORCENTAJE_RESULTADO { get; set; }
        public string FK_ID_CLIENTE { get; set; }
        public string FK_ID_PRODUCTOR { get; set; }

        /////////////////////
        public string NOMBRECLIENTE { get; set; }
        public string NOMBREPRODUCTOR { get; set; }

    }
}
