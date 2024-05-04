using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class UserType
{
    public int UserTypeId { get; set; }

    public string? Type { get; set; }

    public int? UserCnic { get; set; }

    public virtual User? UserCnicNavigation { get; set; }
}
