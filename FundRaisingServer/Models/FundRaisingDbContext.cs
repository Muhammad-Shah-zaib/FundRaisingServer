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

    public virtual DbSet<Case> Cases { get; set; }

    public virtual DbSet<CasesFund> CasesFunds { get; set; }

    public virtual DbSet<Password> Passwords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAuthLog> UserAuthLogs { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:FundRaisingDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Case>(entity =>
        {
            entity.HasKey(e => e.CaseId).HasName("PK__Cases__D062FC05A83F1357");

            entity.Property(e => e.CaseId).HasColumnName("Case_ID");
            entity.Property(e => e.CauseName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Cause_Name");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("Created_Date");
            entity.Property(e => e.Description)
                .HasMaxLength(560)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerifiedCases).HasColumnName("Verified_Cases");
        });

        modelBuilder.Entity<CasesFund>(entity =>
        {
            entity.HasKey(e => e.CaseFundId).HasName("PK__Cases_Fu__A919421DA7E03F01");

            entity.ToTable("Cases_Funds");

            entity.HasIndex(e => e.CaseId, "UQ__Cases_Fu__A8FC9C6FB368CE6F").IsUnique();

            entity.Property(e => e.CaseFundId).HasColumnName("caseFund_id");
            entity.Property(e => e.CaseId).HasColumnName("case_Id");
            entity.Property(e => e.CollectedAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("collected_amount");
            entity.Property(e => e.RemainingAmount)
                .HasComputedColumnSql("([Required_amount]-[collected_amount])", true)
                .HasColumnType("decimal(11, 2)")
                .HasColumnName("Remaining_amount");
            entity.Property(e => e.RequiredAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Required_amount");

            entity.HasOne(d => d.Case).WithOne(p => p.CasesFund)
                .HasForeignKey<CasesFund>(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cases_Fun__case___1332DBDC");
        });

        modelBuilder.Entity<Password>(entity =>
        {
            entity.HasKey(e => e.PasswordId).HasName("PK__Password__850E247A37CBD892");

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
                .HasConstraintName("FK__Passwords__User___66603565");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206D91903BFBC3E9");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345BEACED2").IsUnique();

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
            entity.HasKey(e => e.LogId).HasName("PK__User_Aut__2D26E7AE60A7A898");

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
                .HasConstraintName("FK__User_Auth__User___6C190EBB");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__User_Typ__D3A592DC14136B60");

            entity.ToTable("User_Type");

            entity.Property(e => e.UserTypeId).HasColumnName("User_Type_ID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.UserTypes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__User_Type__User___6383C8BA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
