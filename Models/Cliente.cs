namespace MarketLocalShirts3.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Direccion { get; set; } = "";

        public string Telefono { get; set; } = "";

        public string Ciudad { get; set; } = "";

        public Usuario? Usuario { get; set; }
    }
}