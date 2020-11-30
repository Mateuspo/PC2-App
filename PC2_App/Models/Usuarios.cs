namespace PC2_App.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string CPF { get; set; }
        public string SUS { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public string TelefoneFormatado
        {
            get
            {
                return string.Format("{0:(00) 0 0000-0000}", long.Parse(Telefone));
            }

        }
    }
}
