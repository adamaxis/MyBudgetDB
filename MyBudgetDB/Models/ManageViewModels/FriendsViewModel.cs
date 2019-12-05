using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetDB.Models.ManageViewModels
{
    public class FriendsViewModel
    {
        public IEnumerable<Claim> Friends { get; set; }
        public bool[] FriendsConfirmed { get; set; }
        [EmailAddress]
        public string NewFriend { get; set; }
    }
}
