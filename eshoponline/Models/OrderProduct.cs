namespace eshoponline.Models
{
    public class OrderProduct
    {
        public int OrderProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // n-1 relationships
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
