﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysApi.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; }

    public int EmployeeClassId { get; set; }

    public DateTime? EmployeeBirthDate { get; set; }

    public string EmployeePhoneNum { get; set; }

    public string EmployeeEmail { get; set; }

    public string EmployeePassWord { get; set; }

    public byte[] EmployeePhoto { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual EmployeeClass EmployeeClass { get; set; }
}