// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistrationSystem.Entities
{
    internal partial class Skill
    {
        public Skill()
        {
            EmployeeSkills = new HashSet<EmployeeSkill>();
            Contracts = new HashSet<PlacementContract>();
        }

        [Key]
        public int SkillID { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        /// <summary>
        /// True if some kind of Journeyman or other certification is required for this skill
        /// </summary>
        public bool RequiresTicket { get; set; }

        [InverseProperty("Skill")]
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }

        [ForeignKey("SkillID")]
        [InverseProperty("Skills")]
        public virtual ICollection<PlacementContract> Contracts { get; set; }
    }
}