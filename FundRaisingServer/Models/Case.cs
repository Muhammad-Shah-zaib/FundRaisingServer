using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Case
{
    public int CaseId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string CauseName { get; set; } = null!;

    public bool VerifiedStatus { get; set; }

    public virtual CasesFund? CasesFund { get; set; }
}
