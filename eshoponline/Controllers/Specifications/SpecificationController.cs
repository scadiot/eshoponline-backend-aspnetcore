using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Specifications
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpecificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.SpecificationsListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<SpecificationDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{specificationId}")]
        public async Task<SpecificationDto> Edit(int specificationId, [FromBody]Edit.Command command)
        {
            command.SpecificationId = specificationId;
            return await _mediator.Send(command);
        }

        [HttpDelete("{specificationId}")]
        public async Task Delete(int specificationId)
        {
            await _mediator.Send(new Delete.Command() { SpecificationId = specificationId });
        }
    }
}
