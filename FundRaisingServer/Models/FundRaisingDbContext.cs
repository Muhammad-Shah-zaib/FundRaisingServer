using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Models;

public partial class FundRaisingDbContext : DbContext
{
    public FundRaisingDbContext()
    {
    }

    public FundRaisingDbContext(DbContextOptions<FundRaisingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Password> Passwords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAuthLog> UserAuthLogs { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=FundRaisingDb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Password>(entity =>
        {
            entity.HasKey(e => e.PasswordId).HasName("PK__Password__850E247ABB43354C");

            entity.Property(e => e.PasswordId).HasColumnName("Password_ID");
            entity.Property(e => e.HashKey)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Hash_Key");
            entity.Property(e => e.HashedPassword)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Hashed_Password");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Passwords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Passwords__User___5165187F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206D9190DE5E73EB");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A1CD5705").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("User_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
        });

        modelBuilder.Entity<UserAuthLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__User_Aut__2D26E7AE0A99153A");

            entity.ToTable("User_Auth_Log");

            entity.Property(e => e.LogId).HasColumnName("Log_ID");
            entity.Property(e => e.EventTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Event_Timestamp");
            entity.Property(e => e.EventType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Event_Type");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.UserAuthLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__User_Auth__User___5629CD9C");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__User_Typ__D3A592DC22127C26");

            entity.ToTable("User_Type");

            entity.Property(e => e.UserTypeId).HasColumnName("User_Type_ID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.UserTypes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__User_Type__User___59063A47");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
