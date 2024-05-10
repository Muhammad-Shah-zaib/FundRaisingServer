using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Cause
{
    public int CauseId { get; set; }

    public string CauseTitle { get; set; } = null!;

    public decimal CollectedAmount { get; set; }

    public string? Description { get; set; }

    public bool ClosedStatus { get; set; }

    public virtual ICollection<CauseLog> CauseLogs { get; set; } = new List<CauseLog>();

    public virtual ICollection<CauseTransaction> CauseTransactions { get; set; } = new List<CauseTransaction>();
}
