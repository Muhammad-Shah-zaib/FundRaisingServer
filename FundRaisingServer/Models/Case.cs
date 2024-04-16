using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Case
{
    public int CaseId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly CreatedDate { get; set; }

    public int CauseId { get; set; }

    public virtual Cause Cause { get; set; } = null!;
}
