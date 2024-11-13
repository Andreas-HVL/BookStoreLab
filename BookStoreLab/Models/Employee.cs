using System;
using System.Collections.Generic;

namespace BookStoreLab.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public int EmployedStoreId { get; set; }

    public virtual Store EmployedStore { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
