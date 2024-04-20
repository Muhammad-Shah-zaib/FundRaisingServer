using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class CasesFund
{
    public int CaseFundId { get; set; }

    public decimal? CollectedAmount { get; set; }

    public decimal RequiredAmount { get; set; }

    public decimal? RemainingAmount { get; set; }

    public int CaseId { get; set; }

    public virtual Case Case { get; set; } = null!;
}
