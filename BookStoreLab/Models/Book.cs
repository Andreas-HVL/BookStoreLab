using System;
using System.Collections.Generic;

namespace BookStoreLab.Models;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public decimal? Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public string? Genre { get; set; }

    public int? Pages { get; set; }

    public int AuthorId { get; set; }

    public int PublisherId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<StockStatus> StockStatuses { get; set; } = new List<StockStatus>();
}
