using eshoponline.Models;

namespace eshoponline.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        string GetCurrentUserEmail();
        User GetCurrentUser();
    }
}
