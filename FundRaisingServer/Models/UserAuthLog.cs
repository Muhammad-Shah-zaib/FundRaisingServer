using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class UserAuthLog
{
    public int LogId { get; set; }

    public string EventType { get; set; } = null!;

    public DateTime EventTimestamp { get; set; }

    public int UserCnic { get; set; }

    public virtual User UserCnicNavigation { get; set; } = null!;
}
