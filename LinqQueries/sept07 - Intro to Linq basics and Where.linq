<Query Kind="Expression">
  <Connection>
    <ID>54bf9502-9daf-4093-88e8-7177c12aaaaa</ID>
    <NamingService>2</NamingService>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\ChinookDemoDb.sqlite</AttachFileName>
    <DisplayName>Demo database (SQLite)</DisplayName>
    <DriverData>
      <PreserveNumeric1>true</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.Sqlite</EFProvider>
      <MapSQLiteDateTimes>true</MapSQLiteDateTimes>
      <MapSQLiteBooleans>true</MapSQLiteBooleans>
    </DriverData>
  </Connection>
</Query>

Albums

// query syntax to list all records in an entity (table, collection)
from arowoncollection in Albums
select arowoncollection

//method  syntax to list all records in an entity (lamda expression from lamda symbol)
Albums
	.Select (arowoncollection => arowoncollection)

//Where
//filter method
//the conditions are setup as you would in C#
//beware that LinqPad may NOT like some C# syntax (DateTime)
//beware that Linq is converted to SQL which may not 
//like certain C# syntax because Sql count not convert


//syntax 
//notice that the method syntax makes use of the Lambda expression
//Lambdas are common when performing Linq with the method syntax
//.Where (lamda expression)
//.Where(x => condition [logical operator condition2 ...])
//.Where(x => x.Bytes > 350000)

Tracks
	.Where(x => x.Bytes > 1000000000)

//in query syntax format
from x in Tracks
where x.Bytes > 1000000000
select x

//Find all the albums of the artist Queen.
//Concerns: the artist name is in another table
//          in an sql Select you would be using inner  Join
//			in Linq you DO NOT nned to specific your inner join
//			instead usethe " navigational properties" of your entity 
// 			to generate the relationship

Albums 
	.Where(a => a.Artist.Name.Contains("Queen"))
	// could use == or .Equals 
	// .Where(a => a.Artist.Name.Equals("Queen"))
	// .Where(a => a.Artist.Name == "Queen")