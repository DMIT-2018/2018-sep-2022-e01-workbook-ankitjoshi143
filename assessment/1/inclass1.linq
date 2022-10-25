<Query Kind="Statements">
  <Connection>
    <ID>d6b147c9-7fe9-47bd-8cbc-c8a35198e53b</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>FSIS_2018</Database>
  </Connection>
</Query>

// q1 List all the Guardians with more than one child in the league. Sort the output by the number of guardian children, guardians with the most children first. Sort the listed children by Age.
var q1 = Guardians
			.Where(g => g.Players.Count > 1)
			.Select(x => new
			 {
				 Name = x.FirstName + " " + x.LastName,
				Children = x.Players
								.Select(x => new
								 {
									 Name = x.FirstName + " " + x.LastName,
									 Age = x.Age,
									 Gender = x.Gender,
									 Team  = x.Team.TeamName
								 }).OrderBy(x => x.Age)
			}).OrderByDescending(x => x.Children.Count())
			.Dump()
			;
			
// q2 Display the number of male and female players in the league.

var q2 = Players
			.GroupBy(p => p.Gender)
				.Select(sg => new
				{
					Gender = sg.Key == "F" ? "Female" : "Male" ,
					Count = sg.Count()
				}
						)
				.Dump()
				;
				
				
// q3

var q3 = Teams
			.OrderBy(t => t.TeamName)
			.Select(t => new
				{
					Team = t.TeamName,
					Coach = t.Coach,
				Players = t.Players
								.Select(x => new
								 {
									 LastName = x.LastName,
									 FirstName = x.FirstName,
									 Gender = x.Gender == "F" ? "Female" : "Male" ,
									 Age = x.Age
								 }).OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
			}
			)
			.Dump()
			;
			
			
// q4
var q4 = Teams
			.Select(e => new
									{
										TeamName = e.TeamName,
										Wins = e.Wins
									});

var mostwins = q4
					.Where(x => x.Wins ==
									q4.Max(x => x.Wins))
					.Dump()
					;
					
					
// q5
var q5 = PlayerStats
			.GroupBy(ps => ps.Player)
			.Select(ps => new 
				{
					name = ps.Key.FirstName + " " +ps.Key.LastName,
					teamname = ps.Key.Team.TeamName,
					goals = ps.Sum(p => p.Goals),
					assists = ps.Sum(p => p.Assists),
					redcards = ps.Count(p => p.RedCard ),
					yellowcards = ps.Count(p => p.YellowCard )
							
				}
			).OrderBy(ps => ps.name)
		
			.Dump()
			;