using System;
using System.Collections.Generic;

namespace AppCar.MinhDA.DAL.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? RoleId { get; set; }

    public string? RoleName { get; set; }

    public bool? Status { get; set; }
}
