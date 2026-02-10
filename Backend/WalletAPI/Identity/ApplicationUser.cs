using Microsoft.AspNetCore.Identity;
using WalletAPI.Models;

namespace WalletAPI.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
