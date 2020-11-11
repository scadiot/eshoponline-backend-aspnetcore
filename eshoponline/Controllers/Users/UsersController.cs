using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Users
{
    [Authorize(Roles = "administrator")]
    [Route("users")]
    public class UserController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Login.LoginDto> Login([FromBody] Login.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}
