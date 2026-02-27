namespace MarketLocalShirts3.Models
{
    public class CarritoItem
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = "";
        public string Marca { get; set; } = "";
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string Imagen { get; set; } = "";
        public decimal Subtotal => Precio * Cantidad;
    }
}