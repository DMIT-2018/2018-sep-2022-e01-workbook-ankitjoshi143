<Query Kind="Program">
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

void Main()
{
	// pretend that the Main() is the web page
	// find songs by partial song name.
	// display the album title, song, and artist name.
	// order by song .
	
	//var songCollection = Tracks
	//						.Where(t => t.Name.Contains("dance"))
	//						.OrderBy(t => t.Name)
	//						.Select(t => new SongList
	//							{
	//								Album = t.Album.Title,
	//								Song = t.Name,
	//								Artist = t.Album.Artist.Name
	//							}
	//						);
	//songCollection.Dump();
	
	// assume a value was entered into the web page
	// assume that a post button was pressed
	// assume Main() is the OnPost event
	
	string inputvalue = "dance";
	List<SongList> songCollection = SongsByPartialName(inputvalue);
	songCollection.Dump(); //assume is the web page display
	
	
}

// You can define other methods, fields, classes and namespaces here


// C# really enjoys strongly typed data fields
// 	whether these fields are primitive data types (int, double, ...
// 	or developer defined datatypes (class)

public class SongList
{
	public string Album{get;set;}  //developer defined datatypes (class)
	public string Song{get;set;}
	public string Artist{get;set;}
}


// imagine the following method exists in a service in you rBLL
// this method receives the web page parameter value for the query
// this method will need to return a collection

List<SongList> SongsByPartialName(string partialSongName)
{
	// IEnumerable<SongList> songCollection = Tracks //if it doesnt like var or you can also use IQueryable (data coming from sql)
	var songCollection = Tracks
							.Where(t => t.Name.Contains(partialSongName))
							.OrderBy(t => t.Name)
							.Select(t => new SongList
								{
									Album = t.Album.Title,
									Song = t.Name,
									Artist = t.Album.Artist.Name
								}
							);
	return songCollection.ToList();
	
	//List<SongList> songCollection = Tracks
	//						.Where(t => t.Name.Contains(partialSongName))
	//						.OrderBy(t => t.Name)
	//						.Select(t => new SongList
	//							{
	//								Album = t.Album.Title,
	//								Song = t.Name,
	//								Artist = t.Album.Artist.Name
	//							}
	//						).ToList();
	//return songCollection;
}








































