using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class CaseTransaction
{
    public int CaseTransactionId { get; set; }

    public DateTime TransactionLog { get; set; }

    public decimal TrasactionAmount { get; set; }

    public int? CaseId { get; set; }

    public int? UserId { get; set; }

    public virtual Case? Case { get; set; }

    public virtual Donator? User { get; set; }
}
