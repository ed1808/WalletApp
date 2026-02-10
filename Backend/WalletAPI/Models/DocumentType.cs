namespace WalletAPI.Models;

public class DocumentType
{
    public int Id { get; set; }
    public string DocumentName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public ICollection<User> Users { get; set; } = [];
}
