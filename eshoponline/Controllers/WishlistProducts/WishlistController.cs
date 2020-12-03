using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eshoponline.Controllers.WishlistProducts
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WishlistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.WishlistProductsListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<WishlistProductDto> Add([FromBody]Add.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{wishlistProductId}")]
        public async Task Remove(int wishlistProductId)
        {
            await _mediator.Send(new Remove.Command() { WishlistProductId = wishlistProductId });
        }
    }
}
