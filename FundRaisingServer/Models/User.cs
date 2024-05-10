using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class User
{
    public int UserCnic { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<CaseLog> CaseLogs { get; set; } = new List<CaseLog>();

    public virtual ICollection<CauseLog> CauseLogs { get; set; } = new List<CauseLog>();

    public virtual Donator? Donator { get; set; }

    public virtual ICollection<Password> Passwords { get; set; } = new List<Password>();

    public virtual ICollection<UserAuthLog> UserAuthLogs { get; set; } = new List<UserAuthLog>();

    public virtual ICollection<UserType> UserTypes { get; set; } = new List<UserType>();
}
