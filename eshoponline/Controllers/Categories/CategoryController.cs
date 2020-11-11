using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Categories
{
    [Authorize(Roles = "administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.CategoriesListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<CategoryDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{categoryId}")]
        public async Task<CategoryDto> Edit(int categoryId, [FromBody]Edit.Command command)
        {
            command.CategoryId = categoryId;
            return await _mediator.Send(command);
        }

        [HttpDelete("{categoryId}")]
        public async Task Delete(int categoryId)
        {
            await _mediator.Send(new Delete.Command() { CategoryId = categoryId });
        }
    }
}
