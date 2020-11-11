using AutoMapper;
using eshoponline.Infrastructure;
using eshoponline.Infrastructure.Error;
using eshoponline.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Users
{
    public class Login
    {
        public class Command : IRequest<LoginDto>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UserDataValidator : AbstractValidator<Command>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class LoginDto
        {
            public UserDto User { get; set; }
            public string Token { get; set; }
        }

        public class Handler : IRequestHandler<Command, LoginDto>
        {
            private readonly IConfiguration _config;
            private readonly EshoponlineContext _context;
            private readonly IMapper _mapper;

            public Handler(IConfiguration config, EshoponlineContext context, IMapper mapper)
            {
                _config = config;
                _context = context;
                _mapper = mapper;
            }

            public async Task<LoginDto> Handle(Command message, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(x => x.Email == message.Email).SingleOrDefaultAsync(cancellationToken);
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }
                
                //if (!user.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, person.Salt)))
                //{
                //    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                //}

                var loginDto = new LoginDto()
                {
                    User = _mapper.Map<UserDto>(user),
                    Token = GenerateJSONWebToken(user)
                };
                return loginDto;
            }

            private string GenerateJSONWebToken(User user)
            {
                var secret = _config["Jwt:Key"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(ClaimTypes.Role, "admin")
                    
                };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
