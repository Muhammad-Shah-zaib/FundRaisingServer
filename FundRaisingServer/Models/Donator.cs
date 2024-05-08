using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Donator
{
    public int Cnic { get; set; }

    public decimal TotalDonation { get; set; }

    public virtual ICollection<CaseTransaction> CaseTransactions { get; set; } = new List<CaseTransaction>();

<<<<<<< HEAD
    public virtual ICollection<CauseTransaction> CauseTransactions { get; set; } = new List<CauseTransaction>();

=======
>>>>>>> 4ab63298207977f13ed64d920967c696394def9a
    public virtual User CnicNavigation { get; set; } = null!;
}
