using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Identity;
using WalletAPI.Models;

namespace WalletAPI.Context;

public class WalletDbContext(DbContextOptions<WalletDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    private const string currentTimestamp = "CURRENT_TIMESTAMP";
    private const string uuidV7 = "uuidv7()";
    private const string timestampWithoutTimeZone = "timestamp without time zone";

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<Movement> Movements { get; set; }
    public DbSet<User> WalletUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasPostgresEnum("MovementType", ["Income", "Outcome"]);

        builder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id).HasName("PK_Accounts_Id");

            entity.ToTable("Accounts");

            entity.Property(a => a.Id)
                    .HasDefaultValueSql(uuidV7);

            entity.Property(a => a.Balance)
                    .HasPrecision(15, 4)
                    .HasDefaultValue(0m);

            entity.Property(a => a.CreatedAt)
                    .HasDefaultValueSql(currentTimestamp)
                    .HasColumnType(timestampWithoutTimeZone);

            entity.Property(a => a.UpdatedAt)
                    .HasColumnType(timestampWithoutTimeZone);

            entity.HasOne(a => a.User)
                    .WithMany(u => u.Accounts)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Accounts_User");
        });

        builder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(al => al.Id).HasName("PK_AuditLogs_Id");

            entity.ToTable("AuditLogs");

            entity.HasIndex(al => al.CreatedAt, "Idx_AuditLogs_CreatedAt");
            
            entity.HasIndex(al => new { al.TableName, al.RecordId }, "Idx_AuditLogs_TableName_RecordId");

            entity.Property(al => al.Id)
                    .HasDefaultValueSql(uuidV7);

            entity.Property(al => al.Action)
                    .HasMaxLength(10)
                    .HasComment("INSERT, UPDATE, DELETE");

            entity.Property(al => al.CreatedAt)
                    .HasDefaultValueSql(currentTimestamp)
                    .HasColumnType(timestampWithoutTimeZone);

            entity.Property(al => al.NewValues)
                    .HasComment("Datos después del cambio (NULL si es DELETE)")
                    .HasColumnType("jsonb");

            entity.Property(al => al.OldValues)
                    .HasComment("Datos antes del cambio (NULL si es INSERT)")
                    .HasColumnType("jsonb");

            entity.Property(al => al.RecordId)
                    .HasMaxLength(50)
                    .HasComment("Id del registro afectado. Varchar para aceptar UUID e INT");

            entity.Property(al => al.TableName)
                    .HasMaxLength(50)
                    .HasComment("Ej: Accounts, WalletUsers, etc.");
        });

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.HasOne(au => au.User)
                    .WithOne(u => u.ApplicationUser)
                    .HasForeignKey<ApplicationUser>(au => au.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ApplicationUser_User");
        });

        builder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(dt => dt.Id).HasName("PK_DocumentTypes_Id");

            entity.ToTable("DocumentTypes");

            entity.Property(dt => dt.DocumentName)
                    .HasMaxLength(100);

            entity.Property(dt => dt.CreatedAt)
                    .HasDefaultValueSql(currentTimestamp)
                    .HasColumnType(timestampWithoutTimeZone);
        });

        builder.Entity<Movement>(entity =>
        {
            entity.HasKey(m => m.Id).HasName("PK_Movements_Id");

            entity.ToTable("Movements");

            entity.Property(m => m.Id)
                    .HasDefaultValueSql(uuidV7);

            entity.Property(m => m.Amount)
                    .HasPrecision(15, 4);

            entity.Property(m => m.Type)
                    .HasColumnType("MovementType")
                    .HasConversion<string>();
            
            entity.Property(m => m.CreatedAt)
                    .HasDefaultValueSql(currentTimestamp)
                    .HasColumnType(timestampWithoutTimeZone);

            entity.HasOne(m => m.Account)
                    .WithMany(a => a.Movements)
                    .HasForeignKey(m => m.AccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Movement_Account");
        });

        builder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id).HasName("PK_Users_Id");

            entity.ToTable("WalletUsers");

            entity.HasIndex(u => u.DocumentNumber, "Idx_WalletUsers_DocumentNumber")
                    .IsUnique();

            entity.Property(u => u.DocumentNumber)
                    .HasMaxLength(20);

            entity.Property(u => u.FirstName)
                    .HasMaxLength(150);
            
            entity.Property(u => u.FirstSurname)
                    .HasMaxLength(150);

            entity.Property(u => u.MiddleName)
                    .HasMaxLength(150);

            entity.Property(u => u.SecondSurname)
                    .HasMaxLength(150);

            entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql(currentTimestamp)
                    .HasColumnType(timestampWithoutTimeZone);

            entity.Property(u => u.UpdatedAt)
                    .HasDefaultValueSql(currentTimestamp)
                    .HasColumnType(timestampWithoutTimeZone);

            entity.HasOne(u => u.DocumentType)
                    .WithMany(dt => dt.Users)
                    .HasForeignKey(u => u.DocumentTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_DocumentType");
        });
    }
}
