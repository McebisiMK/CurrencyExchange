using System;
using System.Collections.Generic;

namespace CurrencyExchange.Domain.Models.Entities;

public partial class Currencyrate
{
    public int Id { get; set; }

    public string Base { get; set; } = null!;

    public string Results { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
