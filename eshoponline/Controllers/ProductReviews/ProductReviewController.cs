using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eshoponline.Controllers.ProductReviews
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.ProductReviewsListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<ProductReviewDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{productReviewId}")]
        public async Task<ProductReviewDto> Edit(int productReviewId, [FromBody]Edit.Command command)
        {
            command.ProductReviewId = productReviewId;
            return await _mediator.Send(command);
        }

        [HttpDelete("{productReviewId}")]
        public async Task Delete(int productReviewId)
        {
            await _mediator.Send(new Delete.Command() { ProductReviewId = productReviewId });
        }
    }
}
