using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using EmployeeRegistrationSystem.DAL;
using EmployeeRegistrationSystem.ViewModels;
#endregion

namespace EmployeeRegistrationSystem.BLL
{
    public class FetchEmployeeSkillServices
    {
        #region Constructor for Context Dependency
        private readonly WorkScheduleContext _context;

        internal FetchEmployeeSkillServices(WorkScheduleContext context)
        {
            _context = context;
        }
        #endregion

        #region Queries
        public List<FetchEmployeeSkill> Skills_FetchSkillBy(string phonenumber)
        {
            if (string.IsNullOrWhiteSpace(phonenumber))
            {
                throw new ArgumentNullException("No search value submitted");
            }
            IEnumerable<FetchEmployeeSkill> results = _context.EmployeeSkills
                                                .Where(x => x.Employee.HomePhone == phonenumber)
                                                .Select(x => new FetchEmployeeSkill()
                                                {
                                                    Employee = x.Employee.FirstName + " " + x.Employee.LastName,
                                                    Skill = x.Skill.Description,
                                                    Level= x.Level,
                                                    YOE = x.YearsOfExperience,
                                                    HourlyWage= x.HourlyWage
                                                });

            return results.ToList();
        }
        #endregion
    }
}
