using System;
using System.Data.SQLite;
/* TODO: REMOVE but a reminder for now
 * How do I install System.Data.SQLite on a development machine?

Strictly speaking, there is no need to install System.Data.SQLite on any development machine (e.g. via the setup). The recommended way to use the assemblies is:

Download the precompiled binary package for your target framework and processor architecture (e.g. 32-bit x86, .NET Framework 2.0).
Extract the package to a directory named "Externals" inside your project directory.
Add a reference to the "System.Data.SQLite" assembly from the "Externals" directory.
If necessary (i.e. you require LINQ support), also add a reference to the "System.Data.SQLite.Linq" assembly from the "Externals" directory.*/


//HACKS:  had to manually copy the sqlite interop dll??
  //(2) had to use the 32 bit version instead
namespace database
{
	public class DataTest
	{
		public DataTest ()
		{
		}

		private string connection_string = @"Data Source=sample.s3db;Version=3"; // needed to add Version=3 for it to work

		public void CreateDatabase ()
		{

			//TODO: Tutorial used http://www.techcoil.com/blog/my-experience-with-system-data-sqlite-in-c/


			try {
				SQLiteConnection sqliteCon = new SQLiteConnection (connection_string);
			
			sqliteCon.Open();

			// Define the SQL Create table statement
				string createAppUserTableSQL = "CREATE TABLE if not exists [AppUser] (" +
				"[name] TEXT NULL," +
					"[username] TEXT  NULL" +
					") ";


			using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction())
			{
				// Create the table
				SQLiteCommand createCommand = new SQLiteCommand(createAppUserTableSQL
				                                                , sqliteCon);
				createCommand.ExecuteNonQuery();
				createCommand.Dispose();
				
				// Commit the changes into the database
				sqlTransaction.Commit();
			} // end using
			
			// Close the database connection
			sqliteCon.Close();
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}
		public void InsertTest ()
		{
			// Performs an insert, change contents of sqlStatement to perform
			// update or delete.
			string sqlStatement = "INSERT INTO AppUser(name, username) VALUES('Tommy', 'Tommy_83')";

			SQLiteConnection sqliteCon = new SQLiteConnection (connection_string);
			sqliteCon.Open ();
			using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction()) {
				SQLiteCommand command = new SQLiteCommand (sqlStatement, sqliteCon);
				command .ExecuteNonQuery ();
				sqlTransaction.Commit ();
			}
			sqliteCon.Close();
		}
		public void GetData ()
		{
			// Connect to database

			SQLiteConnection sqliteCon = new SQLiteConnection (connection_string);
			sqliteCon.Open ();
			try {
				// Execute query on database
				//string selectSQL = "SELECT name, username FROM AppUser";
				string selectSQL = "SELECT * FROM AppUser";
				SQLiteCommand selectCommand = new SQLiteCommand (selectSQL, sqliteCon);
				SQLiteDataReader dataReader = selectCommand.ExecuteReader ();
			
				// Iterate every record in the AppUser table
				while (dataReader.Read()) {
					Console.WriteLine ("Name: " + dataReader.GetString (0)
						+ " Username: " + dataReader.GetString (1));
				}
				dataReader.Close ();
				sqliteCon.Close ();
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}
	}
}

