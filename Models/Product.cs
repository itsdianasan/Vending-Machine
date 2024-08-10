namespace VendingMachineApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int ID { get; internal set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
