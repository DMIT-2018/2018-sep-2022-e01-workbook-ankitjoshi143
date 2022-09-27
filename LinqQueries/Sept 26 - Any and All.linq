<Query Kind="Statements">
  <Connection>
    <ID>d8951735-46de-49b1-a766-1949a481f84d</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

// Any and All
// these filter tests return a true or false condition
// they work at the complete collection level

// Genres.Count().Dump();
// 25

// Show genres that have tracks which are not on any playlist
Genres
	.Where (g => g.Tracks.Any(tr => tr.PlaylistTracks.Count() == 0))
	.Select(g => g)
	//.Dump()
	;

// Show Genres that have all their tracks appearing at least once on a playlist
Genres
	.Where (g => g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0))
	.Select(g => g)
	//.Dump()
	;
	
	
// there maybe times that using a !Any() -> All(!relationship)
// 			and !All -> Any (!relationship)
// All != 0 for Any == 0 .Where (g => g.Tracks.All(tr => tr.PlaylistTracks.Count() != 0))
// Any == 0 for All > 0

// Using All and Any in comparin g2 collections
// if your collection is NOT a complex record there is a Linq method
//	called .Except that can be used to solve your query

// Compare the track collection of 2 people using All and Any

// roberto almedia and Michelle Brooks

var almeida = PlaylistTracks
					.Where( x => x.Playlist.UserName.Contains("AlmeidaR"))
					.Select(x => new 
						{
							song = x.Track.Name,
							genre = x.Track.Genre.Name,
							id = x.TrackId,
							artist = x.Track.Album.Artist.Name
						})
						.Distinct()
						.OrderBy(x => x.song)
						//.Dump() //110
						;


var brooks = PlaylistTracks
					.Where( x => x.Playlist.UserName.Contains("BrooksM"))
					.Select(x => new 
						{
							song = x.Track.Name,
							genre = x.Track.Genre.Name,
							id = x.TrackId,
							artist = x.Track.Album.Artist.Name
						})
						.Distinct()
						.OrderBy(x => x.song)
						//.Dump() //88
						;

// List the tracks that BOTH Roberto and Michelle like.
// Compare 2 datasets togther, data in listA that is also in listB
// Assume listA is Roberto and listB is Michelle
// listA is what you wish to report from 
// listB is what you wish to compare to 

// What songs does Roberto like but not Michelle
var c1 = almeida
			.Where(rob => !brooks.Any(mic => mic.id == rob.id)) //rob as record entity //outside collection vs inside collection // not any of michelle record matching rob
			.OrderBy(rob => rob.song)
			//.Dump()
			;


var c2 = almeida  //another way of doing it 
			.Where(rob => brooks.All(mic => mic.id != rob.id)) 
			.OrderBy(rob => rob.song)
			//.Dump()
			;
			
var c3 = brooks 
			.Where(mic => almeida.All(rob => rob.id != mic.id))
			.OrderBy(mic => mic.song)
			//.Dump()
			;
			
// What songs does both michelle and roberto like 
var c4 = brooks //lets do with brook this time
			.Where(mic => almeida.Any(rob => rob.id == mic.id))
			.OrderBy(mic => mic.song)
			.Dump()
			;






