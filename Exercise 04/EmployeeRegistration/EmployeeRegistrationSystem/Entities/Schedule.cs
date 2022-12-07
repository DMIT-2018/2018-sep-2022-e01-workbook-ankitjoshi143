﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistrationSystem.Entities
{
    internal partial class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }
        [Column(TypeName = "date")]
        public DateTime Day { get; set; }
        public int ShiftID { get; set; }
        public int EmployeeID { get; set; }
        [Column(TypeName = "money")]
        public decimal HourlyWage { get; set; }
        public bool OverTime { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("Schedules")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ShiftID")]
        [InverseProperty("Schedules")]
        public virtual Shift Shift { get; set; }
    }
}