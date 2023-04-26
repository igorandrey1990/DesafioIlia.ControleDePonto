namespace DesafioIlha.ControleDePonto.Models
{
    public class Registro
    {
        public Registro()
        {
            this.horarios = new List<string>();
        }

        public string dia { get; set; }
        public List<string> horarios { get; set; }
    }
}
