using MyBudgetDB.Data;

namespace MyBudgetDB.Models
{
    public class CreateUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public ApplicationUser ToUser()
        {
            return new ApplicationUser()
            {
                UserName = UserName
            };
        }
    }
}
