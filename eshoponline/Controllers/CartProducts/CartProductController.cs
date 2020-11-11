using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CartProducts
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.CartProductsListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<CartProductDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{cartProductId}")]
        public async Task<CartProductDto> Edit(int cartProductId, [FromBody]Edit.Command command)
        {
            command.CartProductId = cartProductId;
            return await _mediator.Send(command);
        }

        [HttpDelete("{cartProductId}")]
        public async Task Delete(int cartProductId)
        {
            await _mediator.Send(new Delete.Command() { CartProductId = cartProductId });
        }
    }
}
