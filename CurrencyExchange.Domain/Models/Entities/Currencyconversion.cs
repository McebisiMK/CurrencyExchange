using System;
using System.Collections.Generic;

namespace CurrencyExchange.Domain.Models.Entities;

public partial class Currencyconversion
{
    public int Id { get; set; }

    public string Base { get; set; } = null!;

    public string Result { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }
}
