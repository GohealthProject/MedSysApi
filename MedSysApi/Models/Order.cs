﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysApi.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public int? MemberId { get; set; }

    public int? ShipId { get; set; }

    public int? PayId { get; set; }

    public int? StateId { get; set; }

    public DateTime? ShipDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string MerchantTradeNo { get; set; }

    public virtual Member Member { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderPay Pay { get; set; }

    public virtual OrderShip Ship { get; set; }

    public virtual OrderState State { get; set; }
}