namespace WalletAPI.Models;

public class Account
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Movement> Movements { get; set; } = [];
    public User User { get; set; } = null!;
}
