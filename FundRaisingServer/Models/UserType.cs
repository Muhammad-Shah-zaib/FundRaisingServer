using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class UserType
{
    public int UserTypeId { get; set; }

    public string? Type { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
