namespace WalletAPI.Models;

public class Account
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
