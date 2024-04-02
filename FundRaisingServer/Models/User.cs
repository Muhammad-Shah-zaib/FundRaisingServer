﻿using System;
using System.Collections.Generic;

namespace FundRaisingServer.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Password> Passwords { get; set; } = new List<Password>();

    public virtual ICollection<UserAuthLog> UserAuthLogs { get; set; } = new List<UserAuthLog>();

    public virtual ICollection<UserType> UserTypes { get; set; } = new List<UserType>();
}
