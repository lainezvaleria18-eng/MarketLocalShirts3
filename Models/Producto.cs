namespace MarketLocalShirts3.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string Imagen { get; set; } = "";


        public int MarcaId { get; set; }

        public Marca? Marca { get; set; }
    }
