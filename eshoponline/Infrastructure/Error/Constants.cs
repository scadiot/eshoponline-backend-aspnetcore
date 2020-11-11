using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Infrastructure.Error
{
    public class Constants
    {
        public const string NOT_FOUND = "not found";
        public const string IN_USE = "in use";
        public const string USER_NOT_FOUND = "user not found";
        public const string OPERATION_NOT_PERMITTED = "operation not permitted";
        
        public const string InternalServerError = nameof(InternalServerError);
    }
}
