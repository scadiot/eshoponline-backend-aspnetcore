using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eshoponline.Models
{
    public enum OrderStatus
    {
        WaitProcess,
        Processing,
        WaitQualityCheck,
        QualityChecking,
        WaitDispatching,
        Dispatched,
        Canceled,
        Delayed,
        Delivered
    }

    public enum PaymentMethod
    {
        Check,
        BankTransfer,
        Cash,
        Paypal,
        Payoneer
    }

    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Key { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        [DataType("decimal(16 ,3")]
        public decimal ProductsSumPrice { get; set; }

        [DataType("decimal(16 ,3")]
        public decimal ExpeditionPrice { get; set; }

        [DataType("decimal(16 ,3")]
        public decimal TotalPrice { get; set; }

        public int BillingAddressId { get; set; }

        public Address BillingAddress { get; set; }

        public int ShippingAddressId { get; set; }

        public Address ShippingAddress { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; }
    }
}
