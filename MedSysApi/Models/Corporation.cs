﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysApi.Models;

public partial class Corporation
{
    public int TaxId { get; set; }

    public string Corporation1 { get; set; }

    public double? Discount { get; set; }

    public int Contactnumber { get; set; }

    public string Address { get; set; }

    public string Middleman { get; set; }

    public string Password { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}