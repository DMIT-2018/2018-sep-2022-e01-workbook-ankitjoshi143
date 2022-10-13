<Query Kind="Program">
  <Connection>
    <ID>46d7fdee-eedd-4956-8560-762e22306fd2</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>.\SQLEXPRESS</Server>
    <Database>Chinook</Database>
    <DisplayName>Chinook-Entity</DisplayName>
    <DriverData>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	// MAin is going to represent the web page post method
	string searcharg = "Deep";
	string searchby = "Artist";
	List<TrackSelection> tracklist = Track_FetchTrackBy(searcharg, searchby);
	tracklist.Dump();
}



// You can define other methods, fields, classes and namespaces here

#region QRS Queries
public class TrackSelection
{
	public int TrackId { get; set; }
	public string SongName { get; set; }
	public string AlbumTitle { get; set; }
	public string ArtistName { get; set; }
	public int MilliSeconds { get; set; }
	public decimal Price { get; set; }
}

public class PlaylistTrackInfo
{
	public int TrackId { get; set; }
	public int TrackNumber { get; set; }
	public string SongName { get; set; }
	public int MilliSeconds { get; set; }
}

#endregion

// pretend to be the class library project
#region TrackServices class
public List<TrackSelection> Track_FetchTrackBy(string searcharg, string searchby)
{
	if (string.IsNullOrWhiteSpace(searcharg))
	{
		throw new ArgumentNullException("No search value submitted");
	}
	if (string.IsNullOrWhiteSpace(searchby))
	{
		throw new ArgumentNullException("No search style submitted");
	}
	IEnumerable<TrackSelection> results = Tracks
											.Where(x => (x.Album.Artist.Name.Contains(searcharg) &&
															searchby.Equals("Artist")) ||
													(x.Album.Title.Contains(searcharg) &&
															searchby.Equals("Album")))
											.Select(x => new TrackSelection
												{
													TrackId = x.TrackId,
													SongName = x.Name,
													AlbumTitle = x.Album.Title,
													ArtistName = x.Album.Artist.Name,
													MilliSeconds = x.Milliseconds,
													Price = x.UnitPrice
												});
	return results.ToList();											
}
#endregion