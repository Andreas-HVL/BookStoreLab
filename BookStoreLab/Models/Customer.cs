﻿using System;
using System.Collections.Generic;

namespace BookStoreLab.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public int? CustomerGroup { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
