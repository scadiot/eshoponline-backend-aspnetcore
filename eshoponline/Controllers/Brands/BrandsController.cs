using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Brands
{
    [Authorize(Roles = "administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List.BrandsListDto> Get([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<BrandDto> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{brandId}")]
        public async Task<BrandDto> Edit(int brandId, [FromBody]Edit.Command command)
        {
            command.BrandId = brandId;
            return await _mediator.Send(command);
        }

        [HttpDelete("{brandId}")]
        public async Task Delete(int brandId)
        {
            await _mediator.Send(new Delete.Command() { BrandId = brandId });
        }
    }
}
