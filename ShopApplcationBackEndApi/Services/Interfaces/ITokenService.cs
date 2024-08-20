using ShopApplcationBackEndApi.Entities;

namespace ShopApplcationBackEndApi.Services.Interfaces
{
    public interface ITokenService
    {
        string GetToken(string SecretKey, string Audience, string Issuer, AppUser existUser, IList<string> roles);
    }
}
