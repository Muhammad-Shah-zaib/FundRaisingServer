using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Case
{
    public int CaseId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CauseName { get; set; }

    public virtual CasesFund? CasesFund { get; set; }
}
