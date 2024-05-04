using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class CaseLog
{
    public int CaseLogId { get; set; }

    public string LogType { get; set; } = null!;

    public DateTime LogTimestamp { get; set; }

    public int CaseId { get; set; }

    public virtual Case Case { get; set; } = null!;
}
