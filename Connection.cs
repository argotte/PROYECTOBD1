namespace PROYECTOBD1
{
    public class Connection
    {

        private string _ConnectionString;
        public string ConnectionString
        { 
            get 
            {
                ////return _ConnectionString= "Data Source=DIEGUITO;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=micontrasena";
                 return _ConnectionString = "Data Source=DESKTOP-P186VBB;Initial Catalog=ProyectoCereza;Persist Security Info=True;User ID=sa;Password=12345678";
                /////return _ConnectionString = "Data Source=CHUBE;Initial Catalog=ProyectoCereza;Integrated Security=True"
            }
            set 
            { 
               ;
            } 
        
        }





}
}
