using Microsoft.AspNetCore.Identity;

namespace ShopApplcationBackEndApi.Entities
{
    public class AppUser:IdentityUser
    {
        public string FullName  { get; set; }

    }
}
