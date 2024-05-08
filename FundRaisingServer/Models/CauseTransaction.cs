using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class CauseTransaction
{
    public int CauseTransactionId { get; set; }

    public DateTime TransactionTimestamp { get; set; }

    public decimal TransactionAmount { get; set; }

    public int CauseId { get; set; }

    public int DonorCnic { get; set; }

    public virtual Cause Cause { get; set; } = null!;

    public virtual Donator DonorCnicNavigation { get; set; } = null!;
}
