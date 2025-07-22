namespace Domain.Entities
{
    public class ShippingOption
    {
        public required string Method { get; set; }
        public decimal Cost { get; set; }
    }
}
