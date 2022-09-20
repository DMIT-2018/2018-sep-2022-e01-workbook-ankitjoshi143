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

var nonqualifiedemployee = Skills
							.Where(s => s.EmployeeSkills.Count == 0)
							.Select(s => new 
								{
									Skill = s.Description		
								})
							.Dump();