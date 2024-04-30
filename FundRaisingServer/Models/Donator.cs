using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Donator
{
    public int Cnic { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string PhoneNo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<CaseTransaction> CaseTransactions { get; set; } = new List<CaseTransaction>();
}
