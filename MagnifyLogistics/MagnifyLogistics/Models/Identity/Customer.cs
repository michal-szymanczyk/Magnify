using MagnifyLogistics.Persistence;

namespace MagnifyLogistics.Models.Identity
{
    public class Customer : BaseModel
    {
        // for website or API we can use .NET Identity package and derive from IdentityUser
        // it will allow to use predefined functionality of roles/claims/authorization policies/etc.
        
        public string Name { get; set; }
        public IList<CustomerRoles> Roles { get; set; } = new List<CustomerRoles>();
    }
}
