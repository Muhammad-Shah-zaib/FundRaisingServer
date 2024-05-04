using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class CaseTransaction
{
    public int CaseTransactionId { get; set; }

    public DateTime TransactionLog { get; set; }

    public decimal TransactionAmount { get; set; }

    public int? CaseId { get; set; }

    public int? DonorCnic { get; set; }

    public virtual Case? Case { get; set; }

    public virtual Donator? DonorCnicNavigation { get; set; }
}
