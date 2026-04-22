using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using dynamic_form_system.Entities;

namespace dynamic_form_system.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Form> Forms { get; set; }

    public virtual DbSet<FormField> FormFields { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Trackierr;Database=testYoung2;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Form>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Forms__3214EC0790E4371B");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<FormField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FormFiel__3214EC07BF292D7D");

            entity.HasIndex(e => e.FormId, "IX_FormFields_FormId");

            entity.HasIndex(e => new { e.FormId, e.Name }, "UQ_FormFields_FormId_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Configuration).HasDefaultValue("{}");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.FieldType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsRequired).HasDefaultValue(false);
            entity.Property(e => e.Label).HasMaxLength(255);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Form).WithMany(p => p.FormFields)
                .HasForeignKey(d => d.FormId)
                .HasConstraintName("FK_FormFields_Forms");
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Submissi__3214EC0757A583EA");

            entity.HasIndex(e => e.ExtractedCity, "IX_Submissions_City");

            entity.HasIndex(e => e.FormId, "IX_Submissions_FormId");

            entity.HasIndex(e => e.SubmittedAt, "IX_Submissions_SubmittedAt").IsDescending();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Data).HasDefaultValue("{}");
            entity.Property(e => e.ExtractedCity)
                .HasMaxLength(100)
                .HasComputedColumnSql("(CONVERT([nvarchar](100),json_value([Data],'$.city')))", false)
                .HasColumnName("Extracted_City");
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Form).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.FormId)
                .HasConstraintName("FK_Submissions_Forms");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
