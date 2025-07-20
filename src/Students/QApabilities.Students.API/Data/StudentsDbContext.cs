using Microsoft.EntityFrameworkCore;
using QApabilities.Students.API.Entities;

namespace QApabilities.Students.API.Data;

public class StudentsDbContext : DbContext
{
    public StudentsDbContext(DbContextOptions<StudentsDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasIndex(e => e.Email)
                .IsUnique();
            
            entity.Property(e => e.Cpf)
                .IsRequired()
                .HasMaxLength(11);
            
            entity.HasIndex(e => e.Cpf)
                .IsUnique();
            
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(15);
            
            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);
        });
    }
} 