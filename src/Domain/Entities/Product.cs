namespace Domain.Entities
{
    public class Product
        : Entity
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required Dimensions Dimensions { get; set; }
        public List<ShippingOption> ShippingOptions { get; set; } = [];
        public List<Supplier> Suppliers { get; set; } = [];
        public Inventory Inventory { get; set; } = new Inventory();
    }
}
