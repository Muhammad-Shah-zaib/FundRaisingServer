using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class CauseLog
{
    public int CauseLogId { get; set; }

    public string LogType { get; set; } = null!;

    public DateTime LogTimestamp { get; set; }

    public string CauseTitle { get; set; } = null!;

    public decimal CollectedAmount { get; set; }

    public int CauseId { get; set; }

    public int UserCnic { get; set; }

    public string Description { get; set; } = null!;

    public virtual Cause Cause { get; set; } = null!;

    public virtual User UserCnicNavigation { get; set; } = null!;
}
