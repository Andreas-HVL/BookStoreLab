﻿using System;
using System.Collections.Generic;

namespace BookStoreLab.Models;

public partial class StockStatus
{
    public int StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int CurrentStock { get; set; }

    public virtual Book Isbn13Navigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
