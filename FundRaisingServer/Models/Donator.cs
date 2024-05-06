﻿using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Donator
{
    public int Cnic { get; set; }

    public decimal TotalDonation { get; set; }

    public virtual ICollection<CaseTransaction> CaseTransactions { get; set; } = new List<CaseTransaction>();

    public virtual User CnicNavigation { get; set; } = null!;
}
