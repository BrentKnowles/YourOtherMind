using NUnit.Framework;
using System;
using database;
using Layout.data;

namespace Testing
{
	[TestFixture()]
	public class UnitTest_SqlLiteDatabase 
	{
		public  UnitTest_SqlLiteDatabase()
		{
		}

		#region SetupFunctions

		const string test_database_name ="UNITTESTING.s3db";
		/// <summary>
		/// just a wrapper for console out
		/// </summary>
		/// <param name='s'>
		/// S.
		/// </param>
		private void output(object s)
		{
			Console.WriteLine(s);
		}

		private UnitTest_Class_Database CreateTestDatabase (string primarykey)
		{
			UnitTest_Class_Database db = new UnitTest_Class_Database (test_database_name);

			try {

				// There was a lock on deleting the database but I realized that the call to new would
				// clear the database if paired with a delete?

				// We have to clear memory when deleting because the database might be lingering
				//GC.Collect();
				//System.IO.File.Delete (test_database_name);
			} catch (Exception ex) {
				output ("Unable to delete file " + ex.ToString ());
			}
			try {

				db.DropTableIfExists(tmpDatabaseConstants.table_name);
			} catch (Exception ex) {
				output (String.Format ("Unable to drop table {0}", ex.ToString()));
			}

			db.CreateTableIfDoesNotExist (tmpDatabaseConstants.table_name, new string[4] 
			                              {tmpDatabaseConstants.ID, tmpDatabaseConstants.GUID, tmpDatabaseConstants.XML,
				tmpDatabaseConstants.STATUS}, 
			new string[4] {
				"INTEGER",
				"TEXT UNIQUE",
				"LONGTEXT",
				"TEXT"
			}, primarykey
			);
			return db;
		}
		#endregion

		#region basedatabase
		[Test()]
		public void ColumnArrayToStringForInserting()
		{
			UnitTest_Class_Database test = new UnitTest_Class_Database("nUnitTest");
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
			UnitTest_Class_Database db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
		Console.WriteLine("First insert should work");
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML,
				tmpDatabaseConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});
			Console.WriteLine("inserting a unique GUID_B is okay");
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML,
				tmpDatabaseConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_B"});
			Console.WriteLine("Second insert should fail ");
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML,
				tmpDatabaseConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});

		}
		#endregion




		#region AddMissingColumns
		[Test()]
		public void AddMissingColumn_Test_ColumnDidNotExistAndWeDetectedItCorrectly()
		{
			// Create a test database
			UnitTest_Class_Database db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.ID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML,
				tmpDatabaseConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});
			db.BackupDatabase("");
			// add ColumnA to it
			bool result = db.TestAddMissingColumn(tmpDatabaseConstants.table_name, new string[1] {"boomer"},
			new string[1] {"TEXT"});
			// check return value is true
			Assert.True(result);
			db.BackupDatabase("");
			// add ColumnA to it again. This time return value should be false (because we did not need to add the column)
			result = db.TestAddMissingColumn(tmpDatabaseConstants.table_name, new string[1] {"boomer"},
			new string[1] {"TEXT"});
			Console.WriteLine ("Result #2 = " + result);
			Assert.False (result);
		}
		[Test()]
		[ExpectedException]
		public void AddMissingColumn_ListOfColumnsNotEqualToListOfTypes()
		{

			// Create a test database
			UnitTest_Class_Database db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.ID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML,
				tmpDatabaseConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});

			bool result = db.TestAddMissingColumn(tmpDatabaseConstants.table_name, new string[2] {"boomer","poob"},
			new string[1] {"TEXT"});
		}
		#endregion
		#region TableExists
		[Test]
		public void TableDoesExist ()
		{

			UnitTest_Class_Database db = new UnitTest_Class_Database (test_database_name);
			db.DropTableIfExists(tmpDatabaseConstants.table_name);
			output ("here1");
			Assert.False (db.TableExists (tmpDatabaseConstants.table_name));
			output ("here");
			CreateTestDatabase (String.Format ("{0}", tmpDatabaseConstants.ID));
			output ("here");
			try {
				db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
					tmpDatabaseConstants.STATUS,
					tmpDatabaseConstants.XML,
					tmpDatabaseConstants.GUID
				}
				, new object[3] {"boo status", "boo xml", "GUID_A"});
				Assert.True (db.TableExists (tmpDatabaseConstants.table_name));
			} catch (Exception) {
			}
		}
		[Test]
		public void DropTableTest ()
		{
			UnitTest_Class_Database db = CreateTestDatabase (String.Format ("{0}", tmpDatabaseConstants.ID));
			output ("here");
			try {
				db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
					tmpDatabaseConstants.STATUS,
					tmpDatabaseConstants.XML,
					tmpDatabaseConstants.GUID
				}
				, new object[3] {"boo status", "boo xml", "GUID_A"});

				Assert.True (db.TableExists (tmpDatabaseConstants.table_name));
				db.DropTableIfExists (tmpDatabaseConstants.table_name);
				Assert.True (db.TableExists (tmpDatabaseConstants.table_name));
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
			output ("GUIDDOESEXIST");
			bool result = false;
			output ("here1");
				// Create a test database
				UnitTest_Class_Database db = CreateTestDatabase (String.Format ("{0}", tmpDatabaseConstants.ID));
			output ("here");
			try {
				db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML,
				tmpDatabaseConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});
			
				 result = db.TestAddMissingColumn (tmpDatabaseConstants.table_name, new string[1] {"boomer"},
			new string[1] {"TEXT"});
			} catch (Exception ex) {
				output(ex.ToString ());
			}
			output ("here");
			Assert.True (db.Exists(tmpDatabaseConstants.table_name, tmpDatabaseConstants.GUID, "GUID_A"));
			output (result);
		}
		[Test()]
		public void GuidDoesNotExist()
		{
			// Create a test database
			UnitTest_Class_Database db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.ID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			bool result = db.TestAddMissingColumn(tmpDatabaseConstants.table_name, new string[1] {"boomer"},
			new string[1] {"TEXT"});
			Console.WriteLine ("GuidDoesNotExist");
			Assert.False (db.Exists(tmpDatabaseConstants.table_name, tmpDatabaseConstants.GUID, "GUID_B"));
		}

		#endregion

		#region GetValues (NOT DONE)

		[Test]
		public void GetMultipleRowsWhenMultipleRowsMatch ()
		{
		
			output ("**Get data Test**");

				SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");

			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML,
				tmpDatabaseConstants.GUID
			}
			, new object[3] {"boo status", "boo xml", "GUID_A"});

			output("first roww");
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML,
				tmpDatabaseConstants.GUID
			}
			, new object[3] {"boo status", "boo xmlB", "GUID_B"});





			System.Collections.Generic.List<object[]> results = db.GetValues (tmpDatabaseConstants.table_name, new string[1] {
				tmpDatabaseConstants.GUID

			}, tmpDatabaseConstants.STATUS, "boo status");

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
			
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});

			db.GetValues("", new string[1]{"irrelevant"}, tmpDatabaseConstants.GUID, "GUID_A");
		}
		[Test()]
		[ExpectedException]
		public void GetText_NoGetColumn()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.GetValues(tmpDatabaseConstants.table_name, new string[1]{""}, tmpDatabaseConstants.GUID, "GUID_A");
		}
		[Test()]
		[ExpectedException]
		public void GetText_GetColumnIsNull()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.GetValues(tmpDatabaseConstants.table_name, new string[1]{""}, tmpDatabaseConstants.GUID, "GUID_A");
			db.Dispose();
		}
		[Test()]
		[ExpectedException]
		public void GetText_NoTestColumn()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, "", "GUID_A");
			db.Dispose();
		}

		[Test()]
		public void GetText_TestShouldHaveNoProblems()
		{
			
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, tmpDatabaseConstants.GUID, "GUID_A");
			db.Dispose();
		}

		[Test()]
		[ExpectedException]
		public void GetText_NoTestValue()
		{
		
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, tmpDatabaseConstants.GUID, "");
			db.Dispose();
			
		}

		[Test()]
		//[ExpectedException]
		public void GetText_ReturnColumnDoesNotExists()
		{
			
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
		//	db.GetValues(tmpDatabaseConstants.table_name, new string[1]{"booboo"}, tmpDatabaseConstants.GUID, "GUID_A");

			// just trying another way to structure the test. see http://nunit.net/blogs/?p=63

			Assert.That (delegate {db.GetValues(tmpDatabaseConstants.table_name, new string[1]{"booboo"}, tmpDatabaseConstants.GUID, "GUID_A"); },Throws.Exception );
			db.Dispose();
			
		}
		/// <summary>
		/// table has no rows in it?
		/// Return a blank 
		/// </summary>
		[Test()]
		public void BlankTable_GetValues ()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));

			//db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			//}, new object[3] {"boo status", "boo xml", "GUID_A"});
			Assert.True (db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, tmpDatabaseConstants.GUID, "GUID_A").Count== 0);
			//Assert.That (delegate {db.GetValues(tmpDatabaseConstants.table_name, new string[1]{"a"}, tmpDatabaseConstants.GUID, "GUID_A"); },Throws.Exception );

			//Assert.Throws(typeof(Exception), db.GetValues(tmpDatabaseConstants.table_name, new string[1]{"a"}, tmpDatabaseConstants.GUID, "a"));
			db.Dispose();

		}
		[Test()]
		public void GetText_ExpectBlankString()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			System.Collections.Generic.List<object[]> list = db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, tmpDatabaseConstants.GUID, "GUID_B");
			// expect an empty list
			Assert.AreEqual(list.Count, 0);
			//Assert.IsNull(list);

			db.Dispose();
		}
		[Test()]
		public void GetText_ExpectExactResult()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			System.Collections.Generic.List<object[]> list = db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, tmpDatabaseConstants.GUID, "GUID_A");
			output (list[0][0].ToString());
			if (list == null) Assert.True (false);
			Assert.AreEqual(list[0][0].ToString(), "boo status");
			db.Dispose();
		}
		[Test]
		public void GetText_MultipleRowsExactMatch()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			
			output("database made");
			
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_B"});
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_C"});
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status2", "boo xml", "GUID_D"});

			System.Collections.Generic.List<object[]> list = db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.GUID}, tmpDatabaseConstants.STATUS, "boo status");
			output (list.Count);
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


		
		[Test()]
		public void CreateFakeDatabaseAndBackup()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});

			//not sure how to set this test up. Force File Write? Then test if file exists?
			// or shoudl this return a Stream?
			

			db.BackupDatabase("");
			Assert.False (true);
		}
		/// <summary>
		/// If a primary key is not specified then it should throw an exception
		/// </summary>
		[Test()]
		[ExpectedException]
		public void CreateTableWithNoPrimaryKey()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", ""));
		}

		/// <summary>
		/// Should be no problems if creating two tables one after another, if they are the same table
		/// </summary>
		[Test()]
		public void CreateTableIfDoesNotExist_Test()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
		}

		[Test()]
		[ExpectedException]
		public void Test_FailOnInvalidDatabaseName()
		{
			UnitTest_Class_Database db = new UnitTest_Class_Database ("");

		}
		[Test()]
		public void DatabaseIsDisposed()
		{	
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.Dispose();

			Assert.That (delegate {db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, tmpDatabaseConstants.GUID, "GUID_A"); },
			   Throws.Exception );
		}

		[Test()]
		[ExpectedException]
		public void CreateTableIfDoesNotExist_CreateUnevenTable()
		{
			UnitTest_Class_Database db = new UnitTest_Class_Database (test_database_name);
			

			try {
				
				db.DropTableIfExists(tmpDatabaseConstants.table_name);
			} catch (Exception ex) {
				output (String.Format ("Unable to drop table {0}", ex.ToString()));
			}
			
			db.CreateTableIfDoesNotExist (tmpDatabaseConstants.table_name, new string[4] 
			                              {tmpDatabaseConstants.ID, tmpDatabaseConstants.GUID, tmpDatabaseConstants.XML,
				tmpDatabaseConstants.STATUS}, 
			new string[3] {
				"INTEGER",
				"TEXT UNIQUE",
				"LONGTEXT"

			}, tmpDatabaseConstants.GUID
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
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});


			// we fail to update

			Assert.AreEqual(false, db.UpdateSpecificColumnData(tmpDatabaseConstants.table_name, new string[1] {tmpDatabaseConstants.STATUS},
			new object[1] {"snakes!"}, tmpDatabaseConstants.GUID, "GUID_B"));

			// there is a problem with updating specific column data AND not having the right GUID. Crashes rest of unit tests

			Assert.AreEqual("boo status",  db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, tmpDatabaseConstants.GUID, "GUID_A")[0][0].ToString());
			
			db.Dispose();
		}


		[Test()]
		[ExpectedException]
		public void UpdateSpecificColumnData_FailsWhenUnevenColumns()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});

			db.UpdateSpecificColumnData(tmpDatabaseConstants.table_name, new string[2] {tmpDatabaseConstants.XML, tmpDatabaseConstants.STATUS},
			new object[1] {"snakes!"}, tmpDatabaseConstants.GUID, "GUID_A");

			db.Dispose();
		}
		[Test()]
		public void UpdateDataWorks ()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});

			db.UpdateSpecificColumnData(tmpDatabaseConstants.table_name, new string[1] {tmpDatabaseConstants.STATUS},
			new object[1] {"snakes!"}, tmpDatabaseConstants.GUID, "GUID_A");

			Assert.AreEqual("snakes!", db.GetValues(tmpDatabaseConstants.table_name, new string[1]{tmpDatabaseConstants.STATUS}, tmpDatabaseConstants.GUID, "GUID_A")[0][0].ToString());

			db.Dispose();
		}
	
		[Test()]
		[ExpectedException()]
		public void UpdateInvalidTable()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData("", new string[1] {tmpDatabaseConstants.STATUS},
			new object[1] {"snakes!"}, tmpDatabaseConstants.GUID, "GUID_A");
			db.Dispose();
		}

		[Test()]
		[ExpectedException()]
		public void UpdateNullColumns()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData(tmpDatabaseConstants.table_name,null,
			new object[1] {"snakes!"}, tmpDatabaseConstants.GUID, "GUID_A");
			db.Dispose();
		}
		[Test()]
		[ExpectedException()]
		public void UpdateNullValues()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData(tmpDatabaseConstants.table_name, new string[1] {tmpDatabaseConstants.STATUS},
			null, tmpDatabaseConstants.GUID, "GUID_A");
			db.Dispose();
		}
		[Test()]
		[ExpectedException()]
		public void UpdateEmptyWhereColumn()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData(tmpDatabaseConstants.table_name, new string[1] {tmpDatabaseConstants.STATUS},
			new object[1] {"snakes!"}, "", "GUID_A");
		}
		[Test()]
		[ExpectedException()]
		public void UpdateEmptyWhereValue()
		{
			SqlLiteDatabase db =CreateTestDatabase(String.Format ("{0}", tmpDatabaseConstants.GUID));
			db.InsertData (tmpDatabaseConstants.table_name, new string[3] {	tmpDatabaseConstants.STATUS,tmpDatabaseConstants.XML,tmpDatabaseConstants.GUID
			}, new object[3] {"boo status", "boo xml", "GUID_A"});
			
			db.UpdateSpecificColumnData(tmpDatabaseConstants.table_name, new string[1] {tmpDatabaseConstants.STATUS},
			new object[1] {"snakes!"}, tmpDatabaseConstants.GUID, "");
		}

		#endregion





	}
}
