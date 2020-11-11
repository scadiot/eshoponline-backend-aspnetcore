using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Orders
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.OrdersListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<OrderDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{orderId}")]
        public async Task Delete(int orderId)
        {
            await _mediator.Send(new Delete.Command() { OrderId = orderId });
        }
    }
}
