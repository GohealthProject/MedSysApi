﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysApi.Models;

public partial class TrackingList
{
    public int TrackingListId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductId { get; set; }

    public virtual Member Member { get; set; }

    public virtual Product Product { get; set; }
}