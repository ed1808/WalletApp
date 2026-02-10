using System;

namespace WalletAPI.Models;

public class AuditLog
{
    public Guid Id { get; set; }
    public string TableName { get; set; } = null!;
    public string RecordId { get; set; } = null!;
    public string Action { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? NewValues { get; set; }
    public string? OldValues { get; set; }
}
