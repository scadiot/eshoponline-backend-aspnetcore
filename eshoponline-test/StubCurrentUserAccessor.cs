using eshoponline.Infrastructure;
using eshoponline.Models;
using System.Collections.Generic;

namespace eshoponline_test
{
    public class StubCurrentUserAccessor : ICurrentUserAccessor
    {
        private User _currentUser;

        public StubCurrentUserAccessor(User user)
        {
            _currentUser = user;
        }

        public void setUser(User user)
        {
            _currentUser = user;
        }

        public string GetCurrentUserEmail()
        {
            return _currentUser.Email;
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }
    }
}
