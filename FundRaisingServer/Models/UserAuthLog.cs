using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class UserAuthLog
{
    public int LogId { get; set; }

    public string? EventType { get; set; }

    public DateTime? EventTimestamp { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
