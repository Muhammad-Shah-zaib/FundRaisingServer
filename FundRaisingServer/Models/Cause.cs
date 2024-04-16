using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Cause
{
    public int CauseId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
}
