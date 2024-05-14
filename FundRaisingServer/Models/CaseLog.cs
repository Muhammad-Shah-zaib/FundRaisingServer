using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class CaseLog
{
    public int CaseLogId { get; set; }

    public string LogType { get; set; } = null!;

    public DateTime LogTimestamp { get; set; }

    public int CaseId { get; set; }

    public int UserCnic { get; set; }

    public string Title { get; set; } = null!;

    public string CauseName { get; set; } = null!;

    public decimal RequiredAmount { get; set; }

    public decimal CollectedAmount { get; set; }

    public decimal? RemainingAmount { get; set; }

    public bool VerifiedStatus { get; set; }

    public bool ResolvedStatus { get; set; }

    public string Description { get; set; } = null!;

    public virtual Case Case { get; set; } = null!;

    public virtual User UserCnicNavigation { get; set; } = null!;
}
