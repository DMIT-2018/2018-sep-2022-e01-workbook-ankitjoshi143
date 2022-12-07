<Query Kind="Program">
  <Connection>
    <ID>b7c59cab-a635-45d3-9814-97405317895d</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>.\SQLEXPRESS</Server>
    <Database>WorkSchedule</Database>
    <DisplayName>WorkSchedule-Enitity</DisplayName>
    <DriverData>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	
}

public class EmpSkill
{
	public bool SelectedSkill { get; set; }
	public int SkillID { get; set; }
	public int Level { get; set; }
	public int? YOE { get; set; }
	public decimal HourlyWage { get; set; }
}

public class SkillList
{
	public int SkillID { get; set; }
	public string Description { get; set; }
}

public List<SkillList> Skills_OrderedList()
{
	IEnumerable<SkillList> results = Skills
										.OrderBy(x => x.Description)
										.Select(x => new SkillList()
										{
											SkillID = x.SkillID,
											Description = x.Description										});
	
	return results.ToList();
}


void EmployeeSkills_RegisterTRX(string firstname, string lastname, string homephone, List<EmpSkill> skillslist)
{
	Employees employeeexists = null;
	Skills skillexists = null;
	EmployeeSkills employeeskillexists = null;
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
			skillexists = Skills
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
			if(skill.YOE != null)
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

			employeeskillexists = EmployeeSkills
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

	employeeexists = Employees
							.Where(e => e.FirstName.ToUpper().Equals(firstname)
							&& e.LastName.ToUpper().Equals(lastname)
							&& e.HomePhone.Equals(homephone))
							.FirstOrDefault();


	if (employeeexists == null)
	{
		employeeexists = new Employees()
		{
			FirstName = firstname,
			LastName = lastname,
			HomePhone = homephone,
			Active = true
		};
		Employees.Add(employeeexists);
	}
	
	foreach (EmpSkill skill in skillslist)
	{
		if (skill.SelectedSkill)
		{
			employeeskillexists = new EmployeeSkills();
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
		SaveChanges();
	}

}

// You can define other methods, fields, classes and namespaces here