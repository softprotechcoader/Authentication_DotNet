namespace LoginRegistrationAPI.Repositories;
using Microsoft.AspNetCore.Identity;


public interface ItokenRepository
{
    string CreateJWTToken(IdentityUser user, List<string> roles);
}