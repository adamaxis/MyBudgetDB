using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBudgetDB.Authorization
{
    public class Claims
    {
        public const string IsAdmin = "IsAdmin";
        public const string Friend = "Friend";
        public const string IsFriend = "IsFriend";
        public const string IsActive = "IsActive";
    }
}
