using System;
using System.Collections.Generic;
using CurrencyExchange.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Domain.Models;

public partial class CurrencyExchangeContext : DbContext
{
    public CurrencyExchangeContext()
    {
    }

    public CurrencyExchangeContext(DbContextOptions<CurrencyExchangeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Currencyconversion> Currencyconversions { get; set; }

    public virtual DbSet<Currencyrate> Currencyrates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Currencyconversion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("currencyconversion");

            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Base).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Result).HasMaxLength(200);
        });

        modelBuilder.Entity<Currencyrate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("currencyrate");

            entity.Property(e => e.Base).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Results).HasMaxLength(2000);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
