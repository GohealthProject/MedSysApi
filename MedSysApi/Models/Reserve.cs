﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysApi.Models;

public partial class Reserve
{
    public int ReserveId { get; set; }

    public int? MemberId { get; set; }

    public int? PlanId { get; set; }

    public string ReserveDate { get; set; }

    public string ReserveState { get; set; }

    public virtual ICollection<HealthReport> HealthReports { get; set; } = new List<HealthReport>();

    public virtual Member Member { get; set; }

    public virtual Plan Plan { get; set; }

    public virtual ICollection<ReservedSub> ReservedSubs { get; set; } = new List<ReservedSub>();
}