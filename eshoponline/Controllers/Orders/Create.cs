using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Orders
{
    public class Create
    {
        public class Command : IRequest<OrderDto>
        {
            public CommandAddress ShippingAddress { get; set; }
            public CommandAddress BillingAddress { get; set; }
        }

        public class CommandAddress
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Street { get; set; }
            public string Street2 { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ShippingAddress).NotNull();
                RuleFor(x => x.ShippingAddress.FirstName).NotNull().NotEmpty();
                RuleFor(x => x.ShippingAddress.LastName).NotNull().NotEmpty();
                RuleFor(x => x.ShippingAddress.Street).NotNull().NotEmpty();
                RuleFor(x => x.ShippingAddress.PostalCode).NotNull().NotEmpty();
                RuleFor(x => x.ShippingAddress.City).NotNull().NotEmpty();
                RuleFor(x => x.ShippingAddress.Country).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, OrderDto>
        {
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(EshoponlineContext context, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _mapper = mapper;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<OrderDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = _currentUserAccessor.GetCurrentUser();

                //Add address
                var shippingAddress = _mapper.Map<Models.Address>(request.ShippingAddress);
                await _context.Address.AddAsync(shippingAddress);
                await _context.SaveChangesAsync(cancellationToken);

                var billingAddress = shippingAddress;
                if (request.BillingAddress != null)
                {
                    billingAddress = _mapper.Map<Models.Address>(request.BillingAddress);
                    await _context.Address.AddAsync(billingAddress);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                //Add order
                var order = _mapper.Map<Models.Order>(request);
                order.UserId = _currentUserAccessor.GetCurrentUser().UserId;
                order.Date = DateTime.Now;
                order.Key = StringHelper.RandomString(10);
                order.Status = Models.OrderStatus.WaitProcess;
                order.BillingAddressId = billingAddress.AddressId;
                order.ShippingAddressId = shippingAddress.AddressId;
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync(cancellationToken);

                //Add products
                var cartProducts = await _context.CartProducts
                    .Where(cp => cp.User.UserId == user.UserId)
                    .Include(cp => cp.Product)
                    .ToListAsync();

                decimal productsSumPrice = 0m;
                foreach (var cartProduct in cartProducts)
                {
                    productsSumPrice += cartProduct.Product.UnitPrice;
                    var orderProduct = new Models.OrderProduct()
                    {
                        OrderId = order.OrderId,
                        ProductId = cartProduct.ProductId,
                        Quantity = cartProduct.Quantity,
                        UnitPrice = cartProduct.Product.UnitPrice
                    };
                    _context.OrderProducts.Add(orderProduct);
                    _context.CartProducts.Remove(cartProduct);
                }
                order.ProductsSumPrice = productsSumPrice;
                order.ExpeditionPrice = 0;
                order.ProductsSumPrice = productsSumPrice;
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<OrderDto>(order);
            }
        }
    }
}
