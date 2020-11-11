namespace eshoponline.Models
{
    public class CartProduct
    {
        public int CartProductId { get; set; }

        public int Quantity { get; set; }

        // n-1 relationships
        public int UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
