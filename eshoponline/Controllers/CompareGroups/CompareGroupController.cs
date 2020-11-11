using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshoponline.Controllers.CompareGroups
{
    [Authorize(Roles = "administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompareGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompareGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.CompareGroupsListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<CompareGroupDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{compareGroupId}")]
        public async Task<CompareGroupDto> Edit(int compareGroupId, [FromBody]Edit.Command command)
        {
            command.CompareGroupId = compareGroupId;
            return await _mediator.Send(command);
        }

        [HttpDelete("{compareGroupId}")]
        public async Task Delete(int compareGroupId)
        {
            await _mediator.Send(new Delete.Command() { CompareGroupId = compareGroupId });
        }
    }
}
