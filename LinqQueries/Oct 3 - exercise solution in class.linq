<Query Kind="Statements">
  <Connection>
    <ID>91084132-7491-4d5b-9d0c-cdee156efeb9</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>WorkSchedule</Database>
  </Connection>
</Query>

//Exercise 01
var nonqualifiedemployee = Skills
							.Where(s => s.EmployeeSkills.Count == 0)
							.Select(s => new 
								{
									Skill = s.Description		
								})
							.Dump()
							;
							
							
// 
var q1 = Skills
			.Where(s => s.EmployeeSkills.Count() == 0)
			.Select(x => x.Description)
			.Dump()
			;



// Exercise 02
var skillswithticket = Skills
						.Where(s => s.RequiresTicket == true)
						.Select(s => new 
								{
									Description = s.Description,	
									Employees = s.EmployeeSkills
													.Select(e => new 
														{
															Name = e.Employee.FirstName + " " + e.Employee.LastName,
															Level = e.Level == 1 ? "Novice" : 
																			 e.Level == 2 ? "Proficient":
																			 e.Level >= 3 ? "Expert": "Novice",
															YearsExperience = e.YearsOfExperience
														})
													.OrderByDescending(x => x.YearsExperience)
								})
							
							.Dump()
							;
	
//q2 
var q2 = Skills
						.Where(s => s.RequiresTicket == true)
						.Select(s => new
							{
								Description = s.Description,
								Employees = s.EmployeeSkills
												.OrderByDescending(x => x.YearsOfExperience)
												.Select(e => new 
														{
															Name = e.Employee.FirstName + " " + e.Employee.LastName,
															Level = e.Level == 1 ? "Novice" : 
																			 e.Level == 2 ? "Proficient": "Expert",
															YearsExperience = e.YearsOfExperience
														})
												
							})
						.Dump();
							
							
// Exercise 03
var allemployee = Employees
					.Where(e => e.EmployeeSkills.Count > 1)
					.Select(e => new 
						{
							Name = e.FirstName + " " + e.LastName,
							Skills = e.EmployeeSkills
												.Select(es => new
													{
													Description = es.Skill.Description,
													Level = es.Level == 1 ? "Novice" :
																			 es.Level == 2 ? "Proficient" :
																			 es.Level >= 3 ? "Expert" : "Novice",
													YearsExperience = es.YearsOfExperience
													}
												)
						})
					.Dump()
					;
					

//q3
var q3 = Employees
					.Where(e => e.EmployeeSkills.Count() > 1)
					.Select(e => new 
						{
							Name = e.FirstName + " " + e.LastName,
							Skills = e.EmployeeSkills
												.Select(es => new
													{
													Description = es.Skill.Description,
													Level = es.Level == 1 ? "Novice" :
																			 es.Level == 2 ? "Proficient" :"Expert",
													YearsExperience = es.YearsOfExperience
													}
												)
						})
					.Dump()
					;
					
					
// Exercise 04
var employeeseachday = Shifts
							.Where(s => s.PlacementContract.Location.Name.Contains("NAIT"))
							.GroupBy(s => s.DayOfWeek)			
							.Select(sg => new
									{
										DayOfWeek = sg.Key == 0 ? "Sun" :
																		sg.Key == 1 ? "Mon" :
																		sg.Key == 2 ? "Tue" :
																		sg.Key == 3 ? "Wed" :
																		sg.Key == 4 ? "Thu" :
																		sg.Key == 5 ? "Fri" : "Sat",
										EmployessNeeded = sg.Sum(x=>x.NumberOfEmployees)
									}
									)
							.Dump()
							;

//q4
var q4 =  Shifts
			.Where(s => s.PlacementContract.Location.Name.Contains("NAIT"))
			.GroupBy(s => s.DayOfWeek)			// cant use order by because navigational property isnt available because of same table
			.Select(sg => new
					{
						DayOfWeek = sg.Key == 0 ? "Sun" :
														sg.Key == 1 ? "Mon" :
														sg.Key == 2 ? "Tue" :
														sg.Key == 3 ? "Wed" :
														sg.Key == 4 ? "Thu" :
														sg.Key == 5 ? "Fri" : "Sat",
						EmployessNeeded = sg.Sum(x=>x.NumberOfEmployees) //,  
						//shifts = sg 										// how to select 
						//			.Select(y => new
						//				{
						//					id = y.ShiftID,
						//					Start = y.StartTime
						//				})
					}
					);
			

q4.Dump();

// Exercise 05
var employeelist = Employees
						.Select(e => new 
							{
								Name = e.FirstName + " " + e.LastName,
								YOE = e.EmployeeSkills.Sum(x => x.YearsOfExperience)
							});

var mostexperienced = employeelist
					.Where(x => x.YOE ==
									employeelist.Max(x => x.YOE))
					.Dump()
					;

//q5
var parta = Employees
				.Select(e => new 
					{
						Name = e.FirstName + " " + e.LastName,
						YOE = e.EmployeeSkills.Sum(x => x.YearsOfExperience)
					});

var maxYears = parta
	.Max(p => p.YOE);
var q5 = parta
				.Where(x => x.YOE ==
								maxYears)
				;


q5.Dump();

//using one query
var q5onequery = Employees
				.Select(e => new 
					{
						Name = e.FirstName + " " + e.LastName,
						YOE = e.EmployeeSkills.Sum(x => x.YearsOfExperience)
					})
					.GroupBy(x => x.YOE)
					.OrderByDescending(x => x.Key)
					.First()
					.Dump();


// Exercise 06			
var totalearnings = Schedules
						.Where(a => a.Day.Month == 3)
						.GroupBy(a => a.Employee)
						.ToList()
						//.Select(a => a)
						.Select(a => new
							{
								Name = a.Key.FirstName + " " + a.Key.LastName,
								RegularEarnings = a.Sum(a => (a.Shift.EndTime - a.Shift.StartTime).Hours * a.HourlyWage).ToString("0.00"),
								//RegularEarnings = (a.Where(a => !a.OverTime).Sum(a => (a.Shift.EndTime - a.Shift.StartTime).Hours * a.HourlyWage)).ToString("0.00"),
								OverTimeEarnings = (a.Where(a => a.OverTime).Sum(a => (a.Shift.EndTime - a.Shift.StartTime).Hours * a.HourlyWage * (decimal)1.5)).ToString("0.00"),
								NumberOfShifts = a.Count( )
							}
						).ToList()
						//.GroupBy(a => a.Name)
						//.Distinct()
						.Dump()
						;
						
						
// q6
var q6 = Schedules
			.Where(a => a.Day.Month == 3)
			.ToList()
			.GroupBy(a => a.Employee)  
			.Select( x => new
			{
				Name = x.Key.FirstName + " " + x.Key.LastName,
				RegularEarnings = x.Where(y => !y.OverTime).Sum(y => y.HourlyWage * 
																			(y.Shift.EndTime -y.Shift.StartTime).Hours).ToString("0.00"),
				OTEarnings = x.Where(y => !y.OverTime).Sum(y => y.HourlyWage * 
																			(y.Shift.EndTime -y.Shift.StartTime).Hours * 1.5m).ToString("0.00"),
				NOS = x.Count()															
			});
q6.Dump();
						

