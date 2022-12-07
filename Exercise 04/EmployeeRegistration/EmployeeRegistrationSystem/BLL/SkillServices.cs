#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using EmployeeRegistrationSystem.DAL;
using EmployeeRegistrationSystem.Entities;
using EmployeeRegistrationSystem.ViewModels;
#endregion

namespace EmployeeRegistrationSystem.BLL
{
    public class SkillServices
    {
        #region Constructor for Context Dependency
        private readonly WorkScheduleContext _context;

        internal SkillServices(WorkScheduleContext context)
        {
            _context = context;
        }
        #endregion

        #region Queries
        public List<SkillList> Skills_OrderedList()
        {
            IEnumerable<SkillList> results = _context.Skills
                                                .OrderBy(x => x.Description)
                                                .Select(x => new SkillList()
                                                {
                                                    SkillID = x.SkillID,
                                                    Description = x.Description
                                                });

            return results.ToList();
        }
        #endregion

        #region Command TRX Methods
        public void EmployeeSkills_RegisterTRX(string firstname, string lastname, string homephone, List<EmpSkill> skillslist)
        {
            Employee employeeexists = null;
            Skill skillexists = null;
            EmployeeSkill employeeskillexists = null;
            List<Exception> errors = new List<Exception>();

            if (string.IsNullOrWhiteSpace(firstname))
            {
                throw new ArgumentNullException("No First Name submitted");
            }
            if (string.IsNullOrWhiteSpace(lastname))
            {
                throw new ArgumentNullException("No Last Name submitted");
            }
            if (string.IsNullOrWhiteSpace(homephone))
            {
                throw new ArgumentNullException("No Home Phone submitted");
            }
            if (skillslist == null)
            {
                throw new ArgumentNullException("No skill list submitted");
            }

            int skillselected = skillslist
                                    .Where(s => s.SelectedSkill)
                                    .Count();

            if (skillselected == 0)
            {
                errors.Add(new Exception("No skill list submitted"));
            }

            string skilldescription = null;
            foreach (EmpSkill skill in skillslist)
            {
                if (skill.SelectedSkill)
                {
                    skillexists = _context.Skills
                                    .Where(x => x.SkillID == skill.SkillID)
                                    .FirstOrDefault();
                    if (skillexists == null)
                    {
                        errors.Add(new Exception("Selected skill does not exist"));
                        skilldescription = "invalid skill identifier";
                    }
                    else
                    {
                        skilldescription = skillexists.Description;
                    }
                    if (skill.Level < 1 || skill.Level > 3)
                    {
                        errors.Add(new Exception($" {skilldescription} proper Level must be selected for selected skill"));
                    }
                    if (skill.YOE != null)
                    {
                        if (skill.YOE > 50 || skill.YOE < 1)
                        {
                            errors.Add(new Exception($" {skilldescription} YOE must be positive non zero integer and between 1 & 50"));
                        }
                    }
                    if (skill.HourlyWage < 15 || skill.HourlyWage > 100)
                    {
                        errors.Add(new Exception($" {skilldescription} wage should be between 15 & 100"));
                    }

                    employeeskillexists = _context.EmployeeSkills
                                .Where(e => e.Employee.FirstName.ToUpper().Equals(firstname)
                                    && e.Employee.LastName.ToUpper().Equals(lastname)
                                    && e.Employee.HomePhone.Equals(homephone)
                                    && e.SkillID == skill.SkillID)
                                .FirstOrDefault();

                    if (employeeskillexists != null)
                    {
                        errors.Add(new Exception($"Employee already has the {skilldescription} registered"));
                    }
                }
            }

            employeeexists = _context.Employees
                                    .Where(e => e.FirstName.ToUpper().Equals(firstname)
                                    && e.LastName.ToUpper().Equals(lastname)
                                    && e.HomePhone.Equals(homephone))
                                    .FirstOrDefault();


            if (employeeexists == null)
            {
                employeeexists = new Employee()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    HomePhone = homephone,
                    Active = true
                };
                _context.Employees.Add(employeeexists);
            }

            foreach (EmpSkill skill in skillslist)
            {
                if (skill.SelectedSkill)
                {
                    employeeskillexists = new EmployeeSkill();
                    employeeskillexists.SkillID = skill.SkillID;
                    employeeskillexists.Level = skill.Level;
                    employeeskillexists.YearsOfExperience = skill.YOE;
                    employeeskillexists.HourlyWage = skill.HourlyWage;
                    employeeexists.EmployeeSkills.Add(employeeskillexists);
                }
            }

            if (errors.Count > 0)
            {
                throw new AggregateException("Unable to register. Check concerns", errors);
            }
            else
            {
                _context.SaveChanges();
            }

        }

        #endregion
    }
}
