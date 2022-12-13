namespace PROYECTOBD1.Modelos
{
    public class Recetas_Info
    {
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string TIPO { get; set; }
        public string DESCRIPCION { get; set; }
        public string TIEMPOPREPARACION { get; set; }
        public string RACIONES { get; set; }

        public string FK_ID_PRODUCTOR { get; set; }
        public string FK_ID_CLIENTE { get; set; }

    }
}