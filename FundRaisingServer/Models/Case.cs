using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Case
{
    public int CaseId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string CauseName { get; set; } = null!;

    public bool VerifiedStatus { get; set; }

    public decimal CollectedAmount { get; set; }

    public decimal RequiredAmount { get; set; }

    public decimal? RemainingAmount { get; set; }

    public bool ResolveStatus { get; set; }

    public virtual ICollection<CaseLog> CaseLogs { get; set; } = new List<CaseLog>();

    public virtual ICollection<CaseTransaction> CaseTransactions { get; set; } = new List<CaseTransaction>();
}
