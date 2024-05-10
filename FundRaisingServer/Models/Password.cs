using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class Password
{
    public int PasswordId { get; set; }

    public string? HashedPassword { get; set; }

    public string? HashKey { get; set; }

    public int? UserCnic { get; set; }

    public virtual User? UserCnicNavigation { get; set; }
}
