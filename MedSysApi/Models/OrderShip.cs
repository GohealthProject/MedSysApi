﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysApi.Models;

public partial class OrderShip
{
    public int ShipId { get; set; }

    public string ShipName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}