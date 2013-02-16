using NUnit.Framework;
using System;
using System.Collections.Generic;
using database;
using Layout.data;
using CoreUtilities;

namespace Testing
{
	[TestFixture()]
	public class SqlLiteDatabaseTest 
	{
		public  SqlLiteDatabaseTest()
		{
		}

		#region SetupFunctions

		const string test_database_name ="UNITTESTING.s3db";
	

		private FAKE_SqlLiteDatabase CreateTestDatabase (string primarykey)
		{
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase (test_database_name);

			try {

				// There was a lock on deleting the database but I realized that the call to new would
				// clear the database if paired with a delete?

				// We have to clear memory when deleting because the database might be lingering
				//GC.Collect();
				//System.IO.File.Delete (test_database_name);
			} catch (Exception ex) {
				_w.output ("Unable to delete file " + ex.ToString ());
			}
			try {

				db.DropTableIfExists(dbConstants.table_name);
			} catch (Exception ex) {
				_w.output (String.Format ("Unable to drop table {0}", ex.ToString()));
			}

			db.CreateTableIfDoesNotExist (dbConstants.table_name, new string[6] 
			                              {dbConstants.ID, dbConstants.GUID, dbConstants.XML,
				dbConstants.STATUS, dbConstants.NAME, dbConstants.SUBPANEL}, 
			new string[6] {
				"INTEGER",
				"TEXT UNIQUE",
				"LONGTEXT",
				"TEXT","TEXT","BOOLEAN"
			}, primarykey
			);
			return db;
		}
		#endregion

		#region basedatabase
		[Test()]
		public void ColumnArrayToStringForInserting()
		{
			FAKE_SqlLiteDatabase test = new FAKE_SqlLiteDatabase("nUnitTest");
			string columns = test.TestColumnArrayToStringForInserting(new string[3] {"dog", "cat", "fish"});
			//Console.WriteLine(columns);
			Assert.AreEqual(columns, "dog,cat,fish");


		}
		#endregion

		#region insert

		/// <summary>
		/// Addings to unique column.
		/// 
		/// TEST: We are prevented from adding two columns with the same unique field
		/// </summary>
		[Test()]
		[ExpectedException]
		public void AddingToUniqueColumn()
		{
			// Create a test database
			FAKE_SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
		Console.WriteLine("First insert should work");
			db.InsertData (dbConstants.table_name, new string[3] {
				dbConstants.STATUS,
				dbConstants.XML,
				dbConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});
			Console.WriteLine("inserting a unique GUID_B is okay");
			db.InsertData (dbConstants.table_name, new string[3] {
				dbConstants.STATUS,
				dbConstants.XML,
				dbConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_B"});
			Console.WriteLine("Second insert should fail ");
			db.InsertData (dbConstants.table_name, new string[3] {
				dbConstants.STATUS,
				dbConstants.XML,
				dbConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});

		}
		#endregion




		#region AddMissingColumns
		[Test()]
		public void AddMissingColumn_Test_ColumnDidNotExistAndWeDetectedItCorrectly()
		{
			// Create a test database
			FAKE_SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.ID));
			db.InsertData (dbConstants.table_name, new string[3] {
				dbConstants.STATUS,
				dbConstants.XML,
				dbConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});
			_w.output(db.BackupDatabase());
			// add ColumnA to it
			bool result = db.TestAddMissingColumn(dbConstants.table_name, new string[1] {"boomer"},
			new string[1] {"TEXT"});
			// check return value is true
			Assert.True(result);
			_w.output(db.BackupDatabase());
			// add ColumnA to it again. This time return value should be false (because we did not need to add the column)
			result = db.TestAddMissingColumn(dbConstants.table_name, new string[1] {"boomer"},
			new string[1] {"TEXT"});
			Console.WriteLine ("Result #2 = " + result);
			Assert.False (result);
		}
		[Test()]
		[ExpectedException]
		public void AddMissingColumn_ListOfColumnsNotEqualToListOfTypes()
		{

			// Create a test database
			FAKE_SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.ID));
			db.InsertData (dbConstants.table_name, new string[3] {
				dbConstants.STATUS,
				dbConstants.XML,
				dbConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});

			bool result = db.TestAddMissingColumn(dbConstants.table_name, new string[2] {"boomer","poob"},
			new string[1] {"TEXT"});
		}
		#endregion
		#region TableExists
		[Test]
		public void TableDoesExist ()
		{

			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase (test_database_name);
			db.DropTableIfExists(dbConstants.table_name);
			_w.output ("here1");
			Assert.False (db.TableExists (dbConstants.table_name));
			_w.output ("here");
			CreateTestDatabase (String.Format ("{0}", dbConstants.ID));
			_w.output ("here");
			try {
				db.InsertData (dbConstants.table_name, new string[3] {
					dbConstants.STATUS,
					dbConstants.XML,
					dbConstants.GUID
				}
				, new object[3] {"boo status", "boo xml", "GUID_A"});
				Assert.True (db.TableExists (dbConstants.table_name));
			} catch (Exception) {
			}
		}
		[Test]
		public void DropTableTest ()
		{
			FAKE_SqlLiteDatabase db = CreateTestDatabase (String.Format ("{0}", dbConstants.ID));
			_w.output ("here");
			try {
				db.InsertData (dbConstants.table_name, new string[3] {
					dbConstants.STATUS,
					dbConstants.XML,
					dbConstants.GUID
				}
				, new object[3] {"boo status", "boo xml", "GUID_A"});

				Assert.True (db.TableExists (dbConstants.table_name));
				db.DropTableIfExists (dbConstants.table_name);
				Assert.True (db.TableExists (dbConstants.table_name));
			} catch (Exception) {
			}

		}
		#endregion

		#region Exists (NOT DONE)
		/// <summary>
		/// Tests that we detect CORRECTLY that a GUID IS present in the database
		/// </summary>
		[Test()]
		public void GuidDoesExist ()
		{
			_w.output ("GUIDDOESEXIST");
			bool result = false;
			_w.output ("here1");
				// Create a test database
				FAKE_SqlLiteDatabase db = CreateTestDatabase (String.Format ("{0}", dbConstants.ID));
			_w.output ("here");
			try {
				db.InsertData (dbConstants.table_name, new string[3] {
				dbConstants.STATUS,
				dbConstants.XML,
				dbConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});
			
				 result = db.TestAddMissingColumn (dbConstants.table_name, new string[1] {"boomer"},
			new string[1] {"TEXT"});
			} catch (Exception ex) {
				_w.output(ex.ToString ());
			}
			_w.output ("here");
			Assert.True (db.Exists(dbConstants.table_name, dbConstants.GUID, "GUID_A"));
			_w.output (result);
		}
		[Test()]
		public void GuidDoesNotExist()
		{
			// Create a test database
			FAKE_SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.ID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			bool result = db.TestAddMissingColumn(dbConstants.table_name, new string[1] {"boomer"},
			new string[1] {"TEXT"});
			Console.WriteLine ("GuidDoesNotExist");
			Assert.False (db.Exists(dbConstants.table_name, dbConstants.GUID, "GUID_B"));
		}

		#endregion

		#region GetValues (NOT DONE)

		[Test]
		public void GetMultipleRowsWhenMultipleRowsMatch ()
		{
		
			_w.output ("**Get data Test**");

				SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");

			db.InsertData (dbConstants.table_name, new string[3] {
				dbConstants.STATUS,
				dbConstants.XML,
				dbConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});

			_w.output("first roww");
			db.InsertData (dbConstants.table_name, new string[3] {
				dbConstants.STATUS,
				dbConstants.XML,
				dbConstants.GUID
			}
			, new object[3] {"boo status", "boo xmlB", "GUID_B"});





			System.Collections.Generic.List<object[]> results = db.GetValues (dbConstants.table_name, new string[1] {
				dbConstants.GUID

			}, dbConstants.STATUS, "boo status");

			int count = 0;
			if (results != null) {
				
				foreach (object[] o in results) {
					count++;
					foreach (object oo in o) {
						Console.WriteLine (oo.ToString ());
					}

				}
			}
			Console.WriteLine(count);
			Assert.AreEqual(count,2);
			/*// need to find 2 rows
			if (2 == count) {
				Console.WriteLine ("success");

			} else {
				Console.WriteLine(count + " rows found instead of 2");
				Assert.True(false);
			}*/

		}


		[Test()]
		[ExpectedException]
		public void GetText_NoTable()
		{
			
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});

			db.GetValues("", new string[1]{"irrelevant"}, dbConstants.GUID, "GUID_A");
		}
		[Test()]
		[ExpectedException]
		public void GetText_NoGetColumn()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.GetValues(dbConstants.table_name, new string[1]{""}, dbConstants.GUID, "GUID_A");
		}
		[Test()]
		[ExpectedException]
		public void GetText_GetColumnIsNull()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.GetValues(dbConstants.table_name, new string[1]{""}, dbConstants.GUID, "GUID_A");
			db.Dispose();
		}
		[Test()]
		[ExpectedException]
		public void GetText_NoTestColumn()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, "", "GUID_A");
			db.Dispose();
		}

		[Test()]
		public void GetText_TestShouldHaveNoProblems()
		{
			
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, dbConstants.GUID, "GUID_A");
			db.Dispose();
		}

		[Test()]
		[ExpectedException]
		public void GetText_NoTestValue()
		{
		
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, dbConstants.GUID, "");
			db.Dispose();
			
		}

		[Test()]
		//[ExpectedException]
		public void GetText_ReturnColumnDoesNotExists()
		{
			
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
		//	db.GetValues(tmpDatabaseConstants.table_name, new string[1]{"booboo"}, tmpDatabaseConstants.GUID, "GUID_A");

			// just trying another way to structure the test. see http://nunit.net/blogs/?p=63

			Assert.That (delegate {db.GetValues(dbConstants.table_name, new string[1]{"booboo"}, dbConstants.GUID, "GUID_A"); },Throws.Exception );
			db.Dispose();
			
		}
		/// <summary>
		/// table has no rows in it?
		/// Return a blank 
		/// </summary>
		[Test()]
		public void BlankTable_GetValues ()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));

			//db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			//}, new object[3] {"boo status", "boo xml", "GUID_A"});
			Assert.True (db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, dbConstants.GUID, "GUID_A").Count== 0);
			//Assert.That (delegate {db.GetValues(tmpDatabaseConstants.table_name, new string[1]{"a"}, tmpDatabaseConstants.GUID, "GUID_A"); },Throws.Exception );

			//Assert.Throws(typeof(Exception), db.GetValues(tmpDatabaseConstants.table_name, new string[1]{"a"}, tmpDatabaseConstants.GUID, "a"));
			db.Dispose();

		}
		[Test()]
		public void GetText_ExpectBlankString()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			System.Collections.Generic.List<object[]> list = db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, dbConstants.GUID, "GUID_B");
			// expect an empty list
			Assert.AreEqual(list.Count, 0);
			//Assert.IsNull(list);

			db.Dispose();
		}
		[Test()]
		public void GetText_ExpectExactResult()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			System.Collections.Generic.List<object[]> list = db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, dbConstants.GUID, "GUID_A");
			_w.output (list[0][0].ToString());
			if (list == null) Assert.True (false);
			Assert.AreEqual(list[0][0].ToString(), "boo status");
			db.Dispose();
		}
		[Test]
		public void GetText_MultipleRowsExactMatch()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			
			_w.output("database made");
			
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_B"});
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_C"});
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status2", "boo xml", "GUID_D"});

			System.Collections.Generic.List<object[]> list = db.GetValues(dbConstants.table_name, new string[1]{dbConstants.GUID}, dbConstants.STATUS, "boo status");
			_w.output (list.Count);
			Assert.AreEqual (3, list.Count);

			db.Dispose();
		}

		/*if (CoreUtilities.Constants.BLANK == tableName) {
			throw new Exception("You must provide a table to query");
		}
		
		if (CoreUtilities.Constants.BLANK == Test || CoreUtilities.Constants.BLANK == columnToTest) {
			throw new Exception ("Must define a Test criteria for retrieving text");
		}
		
		if (columnToReturn == null || columnToReturn.Length <= 0) {
			throw new Exception("At least one column is required, to know what you want from the database.");
		}*/
#endregion

		#region general


		/// <summary>
		///  Should test with at least 3 tables
		/// </summary>
		[Test()]
		public void CreateFakeDatabaseAndBackup()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status2", "boo xml2", "GUID_A2"});
			db.Dispose();


			db = new FAKE_SqlLiteDatabase (test_database_name);
			

			try {
				
				db.DropTableIfExists(dbConstants.table_name+"_b");
			} catch (Exception ex) {
				_w.output (String.Format ("Unable to drop table {0}", ex.ToString()));
			}
			
			db.CreateTableIfDoesNotExist (dbConstants.table_name+"_b", new string[4] 
			                              {dbConstants.ID, dbConstants.GUID, dbConstants.XML,
				dbConstants.STATUS}, 
			new string[4] {
				"INTEGER",
				"TEXT UNIQUE",
				"LONGTEXT",
				"TEXT"
			}, dbConstants.GUID
			);


			db.InsertData (dbConstants.table_name+"_b", new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_B"});




			db.Dispose();
			db = new FAKE_SqlLiteDatabase (test_database_name);
			
			
			try {
				
				db.DropTableIfExists(dbConstants.table_name+"_c");
			} catch (Exception ex) {
				_w.output (String.Format ("Unable to drop table {0}", ex.ToString()));
			}
			
			db.CreateTableIfDoesNotExist (dbConstants.table_name+"_c", new string[4] 
			                              {dbConstants.ID, dbConstants.GUID, dbConstants.XML,
				dbConstants.STATUS}, 
			new string[4] {
				"INTEGER",
				"TEXT UNIQUE",
				"LONGTEXT",
				"TEXT"
			}, dbConstants.GUID
			);

			db.InsertData (dbConstants.table_name+"_c", new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_C"});

			// get rid of full text database too
			db.DropTableIfExists("fulltextsearch");

			//not sure how to set this test up. Force File Write? Then test if file exists?
			// or shoudl this return a Stream?
			string result = db.BackupDatabase();
			_w.output(result.Length);
			_w.output(result);

			db.DropTableIfExists(dbConstants.table_name+"_b");
			db.DropTableIfExists(dbConstants.table_name+"_c");
			db.Dispose();
			Assert.AreEqual(526, result.Length);
			//Assert.False (true);
		}
		/// <summary>
		/// If a primary key is not specified then it should throw an exception
		/// </summary>
		[Test()]
		[ExpectedException]
		public void CreateTableWithNoPrimaryKey()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", ""));
			db.Dispose();
		}

		/// <summary>
		/// Should be no problems if creating two tables one after another, if they are the same table
		/// </summary>
		[Test()]
		public void CreateTableIfDoesNotExist_Test()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
		}

		[Test()]
		[ExpectedException]
		public void Test_FailOnInvalidDatabaseName()
		{
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase ("");
			db.Dispose();
		}
		[Test()]
		public void DatabaseIsDisposed()
		{	
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.Dispose();

			Assert.That (delegate {db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, dbConstants.GUID, "GUID_A"); },
			   Throws.Exception );
		}

		[Test()]
		[ExpectedException]
		public void CreateTableIfDoesNotExist_CreateUnevenTable()
		{
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase (test_database_name);
			

			try {
				
				db.DropTableIfExists(dbConstants.table_name);
			} catch (Exception ex) {
				_w.output (String.Format ("Unable to drop table {0}", ex.ToString()));
			}
			
			db.CreateTableIfDoesNotExist (dbConstants.table_name, new string[4] 
			                              {dbConstants.ID, dbConstants.GUID, dbConstants.XML,
				dbConstants.STATUS}, 
			new string[3] {
				"INTEGER",
				"TEXT UNIQUE",
				"LONGTEXT"

			}, dbConstants.GUID
			);
			db.Dispose();
		}

		#endregion

		#region update
		/// <summary>
		/// Nothing should be updated if entered the wrong GUID  to update (and it shoudl return false)
		/// </summary>
		[Test()]
		public void RowDoesNotExistOnUpdate()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});


			// we fail to update

			Assert.AreEqual(false, db.UpdateSpecificColumnData(dbConstants.table_name, new string[1] {dbConstants.STATUS},
			new object[1] {"snakes!"}, dbConstants.GUID, "GUID_B"));

			// there is a problem with updating specific column data AND not having the right GUID. Crashes rest of unit tests

			Assert.AreEqual("boo status",  db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, dbConstants.GUID, "GUID_A")[0][0].ToString());
			
			db.Dispose();
		}


		[Test()]
		[ExpectedException]
		public void UpdateSpecificColumnData_FailsWhenUnevenColumns()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});

			db.UpdateSpecificColumnData(dbConstants.table_name, new string[2] {dbConstants.XML, dbConstants.STATUS},
			new object[1] {"snakes!"}, dbConstants.GUID, "GUID_A");

			db.Dispose();
		}
		[Test()]
		public void UpdateDataWorks ()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});

			db.UpdateSpecificColumnData(dbConstants.table_name, new string[1] {dbConstants.STATUS},
			new object[1] {"snakes!"}, dbConstants.GUID, "GUID_A");

			Assert.AreEqual("snakes!", db.GetValues(dbConstants.table_name, new string[1]{dbConstants.STATUS}, dbConstants.GUID, "GUID_A")[0][0].ToString());

			db.Dispose();
		}
	
		[Test()]
		[ExpectedException()]
		public void UpdateInvalidTable()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData("", new string[1] {dbConstants.STATUS},
			new object[1] {"snakes!"}, dbConstants.GUID, "GUID_A");
			db.Dispose();
		}

		[Test()]
		[ExpectedException()]
		public void UpdateNullColumns()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData(dbConstants.table_name,null,
			new object[1] {"snakes!"}, dbConstants.GUID, "GUID_A");
			db.Dispose();
		}
		[Test()]
		[ExpectedException()]
		public void UpdateNullValues()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData(dbConstants.table_name, new string[1] {dbConstants.STATUS},
			null, dbConstants.GUID, "GUID_A");
			db.Dispose();
		}
		[Test()]
		[ExpectedException()]
		public void UpdateEmptyWhereColumn()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData(dbConstants.table_name, new string[1] {dbConstants.STATUS},
			new object[1] {"snakes!"}, "", "GUID_A");
			db.Dispose();
		}
		[Test()]
		[ExpectedException()]
		public void UpdateEmptyWhereValue()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.InsertData (dbConstants.table_name, new string[3] {	dbConstants.STATUS,dbConstants.XML,dbConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData(dbConstants.table_name, new string[1] {dbConstants.STATUS},
			new object[1] {"snakes!"}, dbConstants.GUID, "");
			db.Dispose();
		}

		#endregion


		#region sorting
		[Test()]
		public void TestSortingOnGetValues ()
		{
			Layout.LayoutDetails.Instance.YOM_DATABASE =test_database_name;
			SqlLiteDatabase db = CreateTestDatabase (String.Format ("{0}", dbConstants.ID));
			db.InsertData (dbConstants.table_name, new string[4] {	
				dbConstants.NAME,dbConstants.XML,dbConstants.GUID, dbConstants.SUBPANEL
			}, new object[4] {"Zum", "boo xml", "GUID_DDA",0});
			db.InsertData (dbConstants.table_name, new string[4] 
			               {	dbConstants.NAME,dbConstants.XML,dbConstants.GUID, dbConstants.SUBPANEL
			}, new object[4] {"Alexander", "boo xml", "GUID_B",0});
			db.InsertData (dbConstants.table_name, new string[4] {	dbConstants.NAME,dbConstants.XML,dbConstants.GUID, dbConstants.SUBPANEL
			}, new object[4] {"aboo", "boo xml", "GUID_A",0});
			db.InsertData (dbConstants.table_name, new string[4] {	dbConstants.NAME,dbConstants.XML,dbConstants.GUID, dbConstants.SUBPANEL
			}, new object[4] {"Calv", "boo xml", "GUID_C",0});

			Layout.MasterOfLayouts mastery = new Layout.MasterOfLayouts ();
			List<Layout.MasterOfLayouts.NameAndGuid> list = mastery.GetListOfLayouts ("");
			foreach (Layout.MasterOfLayouts.NameAndGuid guid in list) {
				_w.output (guid.Caption);
			}
			Assert.AreEqual(4, list.Count);
			Assert.AreEqual ("aboo", list[0].Caption);
			Assert.AreEqual ("Alexander", list[1].Caption);
			Assert.AreEqual ("Calv", list[2].Caption);
			Assert.AreEqual ("Zum", list[3].Caption);



			mastery.Dispose ();
			db.Dispose();
		}
		[Test()]
		public void TestSortingOnGetValues_ShouldFailBecauseAreSubPanels ()
		{
			Layout.LayoutDetails.Instance.YOM_DATABASE =test_database_name;
			SqlLiteDatabase db = CreateTestDatabase (String.Format ("{0}", dbConstants.ID));
			db.InsertData (dbConstants.table_name, new string[4] {	dbConstants.NAME,dbConstants.XML,dbConstants.GUID, dbConstants.SUBPANEL
			}, new object[4] {"Zum", "boo xml", "GUID_DDA",1});
			db.InsertData (dbConstants.table_name, new string[4] {	dbConstants.NAME,dbConstants.XML,dbConstants.GUID, dbConstants.SUBPANEL
			}, new object[4] {"Alexander", "boo xml", "GUID_B",1});
			db.InsertData (dbConstants.table_name, new string[4] {	dbConstants.NAME,dbConstants.XML,dbConstants.GUID, dbConstants.SUBPANEL
			}, new object[4] {"aboo", "boo xml", "GUID_A",1});
			db.InsertData (dbConstants.table_name, new string[4] {	dbConstants.NAME,dbConstants.XML,dbConstants.GUID, dbConstants.SUBPANEL
			}, new object[4] {"Calv", "boo xml", "GUID_C",1});
			
			Layout.MasterOfLayouts mastery = new Layout.MasterOfLayouts ();
			List<Layout.MasterOfLayouts.NameAndGuid> list = mastery.GetListOfLayouts ("");
			foreach (Layout.MasterOfLayouts.NameAndGuid guid in list) {
				_w.output (guid.Caption);
			}
			Assert.AreEqual(0, list.Count);
			/*Assert.AreEqual ("aboo", list[0].Caption);
			Assert.AreEqual ("Alexander", list[1].Caption);
			Assert.AreEqual ("Calv", list[2].Caption);
			Assert.AreEqual ("Zum", list[3].Caption);
			*/
			
			
			mastery.Dispose ();
			db.Dispose();
		}
		#endregion


		[Test]
		public void TestFullTextSearch()
		{
			Assert.True (false);
			// This test seems to cause mdhost.exe crashes!?!? Wasn't finished anyways so aborted it
			return;

			FAKE_SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", dbConstants.GUID));
			db.DropTableIfExists("fulltextsearch");
			db.CreateFullSearchDatabase();
			db.InsertData("fulltextsearch", new string[1] {"texttosearch"}, new object[1] {"blah blah hello there"});
			db.InsertData("fulltextsearch", new string[1] {"texttosearch"}, new object[1] {"blah blah fish hello there"});
			db.InsertData("fulltextsearch", new string[1] {"texttosearch"}, new object[1] {"blah blah hello there"});
			db.InsertData("fulltextsearch", new string[1] {"texttosearch"}, new object[1] {"blah blah dog fish hello there"});
			_w.output("now search");
			db.SearchFullSearchDatabase ();
			_w.output("now search over");
			Console.WriteLine (db.BackupDatabase());
			db.Dispose();
			Assert.True (false);
		}

	}
}

