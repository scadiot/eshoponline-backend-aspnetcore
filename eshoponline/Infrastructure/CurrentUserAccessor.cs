using eshoponline.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eshoponline.Infrastructure
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EshoponlineContext _context;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, EshoponlineContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public string GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public User GetCurrentUser()
        {
            return _context.Users.FirstOrDefault(u => u.Email == this.GetCurrentUserEmail());
        }
    }
}
