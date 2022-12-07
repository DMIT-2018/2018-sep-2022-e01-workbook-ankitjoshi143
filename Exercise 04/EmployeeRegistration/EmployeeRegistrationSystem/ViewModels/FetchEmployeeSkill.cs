using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRegistrationSystem.ViewModels
{
    public class FetchEmployeeSkill
    {
        public string Employee { get; set; }
        public string Skill { get; set; }
        public int Level { get; set; }
        public int? YOE { get; set;}
        public decimal HourlyWage { get; set; }
    }
}
