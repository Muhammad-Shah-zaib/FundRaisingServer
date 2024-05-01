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

    public virtual DbSet<CaseLog> CaseLogs { get; set; }

    public virtual DbSet<CaseTransaction> CaseTransactions { get; set; }

    public virtual DbSet<Donator> Donators { get; set; }

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
            entity.Property(e => e.CollectedAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Collected_Amount");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RemainingAmount)
                .HasComputedColumnSql("([Collected_Amount]-[Required_Amount])", false)
                .HasColumnType("decimal(11, 2)")
                .HasColumnName("Remaining_amount");
            entity.Property(e => e.RequiredAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Required_Amount");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerifiedStatus).HasColumnName("Verified_Status");
        });

        modelBuilder.Entity<CaseLog>(entity =>
        {
            entity.HasKey(e => e.CaseLogId).HasName("PK__Case_Log__D5158BB1B3202808");

            entity.ToTable("Case_Log");

            entity.Property(e => e.CaseLogId).HasColumnName("Case_Log_ID");
            entity.Property(e => e.CaseId).HasColumnName("Case_ID");
            entity.Property(e => e.LogTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Log_Timestamp");
            entity.Property(e => e.LogType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Log_Type");

            entity.HasOne(d => d.Case).WithMany(p => p.CaseLogs)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Case_Log__Case_I__3B40CD36");
        });

        modelBuilder.Entity<CaseTransaction>(entity =>
        {
            entity.HasKey(e => e.CaseTransactionId).HasName("PK__Case_Tra__1D0AE26140EDC83D");

            entity.ToTable("Case_Transactions");

            entity.Property(e => e.CaseTransactionId).HasColumnName("Case_Transaction_ID");
            entity.Property(e => e.CaseId).HasColumnName("Case_ID");
            entity.Property(e => e.TransactionLog)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Transaction_Log");
            entity.Property(e => e.TrasactionAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Trasaction_Amount");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.Case).WithMany(p => p.CaseTransactions)
                .HasForeignKey(d => d.CaseId)
                .HasConstraintName("FK__Case_Tran__Case___367C1819");

            entity.HasOne(d => d.User).WithMany(p => p.CaseTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Case_Tran__User___3587F3E0");
        });

        modelBuilder.Entity<Donator>(entity =>
        {
            entity.HasKey(e => e.Cnic).HasName("PK__Donators__AA570FD50742B62B");

            entity.Property(e => e.Cnic)
                .ValueGeneratedNever()
                .HasColumnName("CNIC");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("Phone_No");
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
            entity.HasKey(e => e.UserCnic).HasName("PK__Users__206D91903BFBC3E9");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345BEACED2").IsUnique();

            entity.Property(e => e.UserCnic).HasColumnName("User_CNIC");
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
                .OnDelete(DeleteBehavior.ClientSetNull)
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
