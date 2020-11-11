using eshoponline.Infrastructure;
using eshoponline.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshoponline_test.Controllers.Users
{
    class UserHelper
    {
        public static async Task<ICurrentUserAccessor> CreateCurrentUserAccessor(SliceFixture fixture, bool administrator, bool manager)
        {
            var dbContext = fixture.GetDbContext();

            var user = new User()
            {
                Email = "Elijah.Pritchard@gmail.com",
                Pseudonym = "MrElijah",
                FirstName = "Elijah",
                LastName = "Pritchard",
                Roles = new List<Role>()
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            if (administrator)
            {
                dbContext.Roles.Add(new Role() { Label = RoleLabel.administrator, UserId = user.UserId });
            }
            if (manager)
            {
                dbContext.Roles.Add(new Role() { Label = RoleLabel.manager, UserId = user.UserId });
            }
            await dbContext.SaveChangesAsync();

            return new StubCurrentUserAccessor(user);
        }
    }
}
