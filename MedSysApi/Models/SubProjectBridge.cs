﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysApi.Models;

public partial class SubProjectBridge
{
    public int SubProjectBridgeId { get; set; }

    public int? ProjectId { get; set; }

    public int? ItemId { get; set; }

    public virtual Item Item { get; set; }

    public virtual Project Project { get; set; }
}