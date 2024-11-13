using System;
using System.Collections.Generic;

namespace BookStoreLab.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public int StoreId { get; set; }

    public int? EmployeeId { get; set; }

    public string? Isbn13 { get; set; }

    public int Quantity { get; set; }

    public DateOnly OrderDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual Book? Isbn13Navigation { get; set; }

    public virtual Store Store { get; set; } = null!;
}
