<Query Kind="Program">
  <Connection>
    <ID>d8951735-46de-49b1-a766-1949a481f84d</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Server>.\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	// Conversions
	// collection we will look at are Iqueryable, IEnumerable and List
	
	// Display all albums and their tracks. Display the album title 
	// artist name and album tracks. For each track show the song name
	// and play time. Show only albums with 25 or more tracks.
	
	List<AlbumTracks> albumlist = Albums
						.Where(a => a.Tracks.Count >= 25)
						.Select(a => new AlbumTracks
							{
								Title = a.Title,
								Artist = a.Artist.Name,
								Songs = a.Tracks
											.Select(tr => new SongItem
												{
													Song = tr.Name,
													Playtime = tr.Milliseconds / 1000.0
												})
												.ToList()
								
							})
							.ToList()
							//.Dump()
							;
							
		// Using .FirstOrDeafult()
		// first saw in 1517 when check to see if a record existed in a BLL service method
		
		// Find the first album by Deep Purple
		var artistparam = "Deep Purrple";
		var resultsFOD = Albums
							.Where(a => a.Artist.Name.Equals(artistparam))
							.Select(a => a)		// take the whole records
							.OrderBy(a => a.ReleaseYear)
							.FirstOrDefault()
							//.Dump()
							;
		if (resultsFOD != null)
		{
			resultsFOD.Dump();
		}
		else
		{
			Console.WriteLine($"No albums found for artist {artistparam}");
		}
}


public class SongItem
{
	public string Song{get;set;}
	public double Playtime{get;set;}
}

public class AlbumTracks
{
	public string Title{get;set;}
	public string Artist{get;set;}
	public List<SongItem> Songs{get;set;}
}


// You can define other methods, fields, classes and namespaces here