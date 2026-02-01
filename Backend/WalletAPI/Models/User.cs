namespace WalletAPI.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string FirstSurname { get; set; } = null!;
    public string? SecondSurname { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
