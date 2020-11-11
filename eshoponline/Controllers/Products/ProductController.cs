using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eshoponline.Controllers.Products
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.ProductsListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<ProductDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{productId}")]
        public async Task Delete(int productId)
        {
            await _mediator.Send(new Delete.Command() { ProductId = productId });
        }
    }
}