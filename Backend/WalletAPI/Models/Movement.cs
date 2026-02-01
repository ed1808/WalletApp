using WalletAPI.Types.Enums;

namespace WalletAPI.Models;

public class Movement
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public MovementType Type { get; set; }
    public Guid AccountRefId { get; set; }
    public DateTime CreatedAt { get; set; }
}
