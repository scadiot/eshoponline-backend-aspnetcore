using eshoponline.Models;
using System;

namespace eshoponline.Controllers.Orders
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderAddressDto
    {
        public int AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Key { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal ProductsSumPrice { get; set; }
        public decimal ExpeditionPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderProductDto[] Products { get; set; }
        public OrderAddressDto BillingAddress { get; set; }
        public OrderAddressDto ShippingAddress { get; set; }
    }
}
