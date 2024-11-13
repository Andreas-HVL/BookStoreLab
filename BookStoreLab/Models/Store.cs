using System;
using System.Collections.Generic;

namespace BookStoreLab.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string? StreetName { get; set; }

    public string? StreetNumber { get; set; }

    public string? PostCode { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<StockStatus> StockStatuses { get; set; } = new List<StockStatus>();
}
