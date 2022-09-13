<Query Kind="Expression">
  <Connection>
    <ID>cb38c261-6d0b-4c83-bf18-f84220b0066c</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>WB320-05\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

// List allalbums by release label. Any album with no label
// Should be indicated as Unknown 
// List Title, Lable and Artist Name
// Order by ReleaseLabel

// understand the problem
// collection: albums
// selective data: anonymous data set 
// label (nullable): either Unknown or label name *****
// order by the release label field

//design
// Albums
// Select (new{})
// fields: title
//		   label        ????? //how we going to solve label naming issue //ternary operator (conditon(s) ? true value : false value)
//		   Artist.Name

//coding and testing
Albums
//	.OrderBy(x => x.ReleaseLabel)  //sorting it before will have null on top and then change it to unknown
	.Select(x => new
	{
		Title = x.Title,
		Label = x.ReleaseLabel == null ? "Unknown" : x.ReleaseLabel,
		Artist = x.Artist.Name
	}
	)
	.OrderBy(x => x.Label)
	
	
// List all albums showing the Title, Artist Name, Year and decade of 
//	 release using oldies, 70s, 80s, 90s or modern.
// Order by decade.

// Hint: can you have nested ternary operators? yes

// <1970 ------> oldies
// else  ------> ( <1980 then 70's
//					else
//						(<1990 then 80's
//						else
//							(<2000 then 90's 
//							else
//								modern)))

Albums
	.Select(x => new
	{
		Title = x.Title,
		Artist = x.Artist.Name,
		Year = x.ReleaseYear,
		Decade = x.ReleaseYear < 1970 ? "Oldies" : 
					 x.ReleaseYear < 1980 ? "70s":
					 x.ReleaseYear < 1990 ? "80s":
					 x.ReleaseYear < 2000 ? "90s": "Modern"
	}
	)
	.OrderBy(x => x.Year)

