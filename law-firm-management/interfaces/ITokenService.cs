using law_firm_management.Models;

namespace law_firm_management.interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
