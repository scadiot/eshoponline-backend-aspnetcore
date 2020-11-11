using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eshoponline.Controllers.SpecificationGroups
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpecificationGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.SpecificationGroupsListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<SpecificationGroupDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{specificationGroupId}")]
        public async Task<SpecificationGroupDto> Edit(int specificationGroupId, [FromBody]Edit.Command command)
        {
            command.SpecificationGroupId = specificationGroupId;
            return await _mediator.Send(command);
        }

        [HttpDelete("{specificationGroupId}")]
        public async Task Delete(int specificationGroupId)
        {
            await _mediator.Send(new Delete.Command() { SpecificationGroupId = specificationGroupId });
        }
    }
}
