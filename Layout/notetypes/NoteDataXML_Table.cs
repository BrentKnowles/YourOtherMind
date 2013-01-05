using System;
using CoreUtilities;
using CoreUtilities.Tables;
using System.Windows.Forms;
using appframe;
using System.Xml.Serialization;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;

namespace Layout
{
	public class NoteDataXML_Table : NoteDataXML
	{
		#region constants
		public override int defaultHeight { get { return 400; } }
		public override int defaultWidth { get { return 700; } }




		#endregion

		#region formcontrols
		TablePanel Table= null;
		#endregion

		#region variables
		protected DataTable mdataSource=null;
		[Browsable(false)]
		public DataTable dataSource {
			get {
				return mdataSource;
				
			}
			set {
				mdataSource = value;
				// The CALLER will also have to make sure to call the Update routine so note is recreated 
				// with the new datasource
			}

		}
		string tableCaption = Loc.Instance.GetString("ATableCaption");
		/// <summary>
		/// Gets or sets the table caption.
		/// </summary>
		/// <value>
		/// The table caption.
		/// </value>
		public string TableCaption {
			get { return tableCaption;}
			set { tableCaption = value;}
		}


		string nextTable="";
		/// <summary>
		/// Gets or sets the next table. This is the table that the random system uses to 'follow'
		/// </summary>
		/// <value>
		/// The next table.
		/// </value>
		public string NextTable {
			get { return nextTable;}
			set { nextTable = value;}
		}

		// September 2011 - Data Source for DataTable
		protected  ColumnDetails[] mColumns;
		/// <summary>
		/// This is the string list of columns
		/// Remember there is always one "automatic" column
		/// for handling the row numbers
		/// 
		/// When columns are changed we need to build the datatable
		/// </summary>
		[Browsable(false)]
		public ColumnDetails[] Columns
		{
			get { return mColumns; }
			set
			{
				if (value == null) return;
				mColumns = value;
				
				// if we keep this without the condition, it blanks the table
				// I honestly do not understand how this worked in previous version of YOM
				if (dataSource == null)
				{
					dataSource = TablePanel.BuildTableFromColumns(value);



					//dataSource = table;//new DataTable();

//					dataSource = new DataSet("Table");
//					
//					if (dataSource.Tables != null)
//					{
//						try
//						{
//							dataSource.Tables.Clear();
//						}
//						catch (Exception)
//						{
//							
//						}
//					}
//					dataSource.Tables.Add(table);

				}

				
			}
		}

		#endregion

		public NoteDataXML_Table () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Table");
		}

		/// <summary>
		/// Builds the default columns.
		/// 
		/// Called from: NoteDataXML_Table(height, width) constructor -- ONLY when note created 
		/// AND
		/// from CreateParent *IF* the table is null which should only happend uring UNIT testing
		/// </summary>
		private void BuildDefaultColumns()
		{
			Columns = new ColumnDetails[4] { 
				new ColumnDetails(TableWrapper.Roll,TablePanel.defaultwidth), new ColumnDetails(TableWrapper.Result,TablePanel.defaultwidth), 
				new ColumnDetails(TableWrapper.NextTable, TablePanel.defaultwidth),
				new ColumnDetails(TableWrapper.Modifier,TablePanel.defaultwidth)};
		}

		public NoteDataXML_Table(int height, int width) : base(height, width)
		{
			Caption = Loc.Instance.Cat.GetString("Table");

			// This constructor is called ONLY when the note is first being created (not loaded)
			// so we create a default datasource here and now.
			//dataSource = new DataSet("Table");
			//dataSource = new DataTable(); // A new datasource is already being created when column default is made.
			BuildDefaultColumns();

			// the table is created when the columns are manifested

		}

		public override void Save ()
		{
			base.Save ();
			if (Table != null) {

				// Does not appear we need to store anythign extra

				dataSource = Table.GetTable();
				Columns=Table.GetColumns();
				// TODO: Save table stuff
//				DataSet ds = new DataSet();
//				ds.Tables.Add((DataTable)Table.dataGrid1.DataSource);
//				dataSource = ds;

			}
		}
		public override void CreateParent (LayoutPanelBase Layout)
		{



			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;

			Table = new TablePanel (dataSource, HandleCellBeginEdit, Columns, GoToNote, this.Caption, GetRandomTableResults);
			Table.Parent = ParentNotePanel;
			Table.Dock = DockStyle.Fill;
			Table.BringToFront ();
			// TODO: Load table data richBox.Rtf = this.Data1;
			if (null != dataSource) {
			//Table.dataGrid1.DataSource = dataSource;// This works but then we lose the hook.Tables [0];
				//Table.dataGrid1.DataMember = "Table";

			} else {
				//NewMessage.Show ();
				lg.Instance.Line("NoteDataXML_Table->CreateParent", ProblemType.WARNING, Loc.Instance.GetString("You have a null table which should not happen. This usually occurs only during UNIT TESTING because proper create constructor is not used"));
				BuildDefaultColumns();
			}

			//Table.dataGrid1.NavigateTo(0, "Table1"); // same as Table1 referenced by column styles
			//Table.appearance = this;
				
		

			// TODO: Set up table handlersrichBox.TextChanged+= HandleTextChanged;
			
		}

		int HandleCellBeginEdit ()
		{
			SetSaveRequired(true);
			return 1;
		}
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Table");
		}
		


		/// <summary>
		/// Gos to note.
		/// Called from TablePanel
		/// </summary>
		/// <param name='NoteName'>
		/// Note name.
		/// </param>
		public void GoToNote (string NoteName)
		{
			NoteDataInterface note = Layout.FindNoteByName (NoteName);
			if (note != null) {
				Layout.GoToNote(note);
			}
		}



		/// <summary>
		/// Gets the random table results.
		/// </summary>
		/// <returns>
		/// The random table results.
		/// </returns>
		public string GetRandomTableResults ()
		{

			//TODO: figure out if new random table system will function like this
			/*1. Called from own page
			 *2. Called from other page
			 *3. Called from the TablePanel itself, the 'preview function' [DONE]
			 */
			if (dataSource != null) {
				string _caption = "";
				if (TableCaption != "") {
					_caption = TableCaption;
				}
				string sNextTable = NextTable; // can't use the actual nexttable field
				string sResult = GenerateAllResults (ref sNextTable, dataSource, _caption,
							                                        Caption, Layout);
				return sResult;
			}
			return Loc.Instance.GetString ("No data found in random table");
		}

		/// <summary>
		/// will go through each "next" table from the source one
		/// the calling object
		/// and generateresults
		/// 
		/// 
		/// 
		/// 
		/// </summary>
		/// <returns></returns>
		public static string GenerateAllResults (ref string nextTable, DataTable currentTable, string Title, string Core_Table, LayoutPanelBase Layout)
		{
			
			// probably should save first too but not sure
			// how to integrate that
			
			string sOriginalNextTable = nextTable;
			if (null == Layout) {
				throw new Exception("You must have a valid Layout loaded when doing a random table traversal.");
			}
			// do first table
			System.Collections.ArrayList notelist = Layout.GetAllNotes();
			NoteDataInterface[] notes = new NoteDataInterface[notelist.Count];
			notelist.CopyTo (notes);

			resultReturn sResult = GenerateResult(0, currentTable, ref nextTable, Title,notes);
			
			
			
			// now, if there are more tables
			//classPageTable tables = this;
			
			
			
			if (sResult.sResult == " \n")
			{
				sResult.sResult = Constants.BLANK;
			}
			
			
			while (nextTable != null && nextTable != Constants.BLANK)
			{

				// looks for the note called start
				NoteDataXML_Table table = null;
				bool LINEAR_MODE = false;
				
				if (nextTable[0] == '-')
				{
					// Linear table system.
					// a -X (any number, with a minus preceding it) represents a row
					
					// basically we want to stay on the current table but go to the next row
					table = (NoteDataXML_Table)Layout.FindNoteByName(Core_Table);
					LINEAR_MODE = true;
				}
				else
				{
					// Grab the next table
					NoteDataInterface potentialTable = Layout.FindNoteByName(nextTable);
					if (potentialTable is NoteDataXML_Table)
					{
						table = (NoteDataXML_Table) potentialTable;
						// sep 28 2011
						// linear table system
						// if we encounter a 'real table' reference that means 
						// to set the Core Table to that
						Core_Table = nextTable;
					}
					

				}
				
				
				
				
				//NotePanelTable table = (NotePanelTable)desktop.GetPanelByName(nextTable);
				
				resultReturn newResult ;
				
				if (table != null)
				{
					string old_next_table = nextTable;
					string _caption = "";// table.Caption;
					if (table.TableCaption != "")
					{
						_caption = table.TableCaption;
					}
					
					// see note A below for why we have this check
					if (LINEAR_MODE == true && table.NextTable == "")
					{
						// leave nextable alone in Lienar mode
						// needed for when we call a linear table from another table (instead of calling the linear table directly)
					}
					else
					{
						nextTable = table.NextTable; // this is clearing the row field!
					}
					newResult = GenerateResult(sResult.nModifier, (DataTable)table.dataSource, ref nextTable, _caption, notes);
					
					
					
					/* NOTE A September 28 2011
                        * There is a WEIRD pointer thing happening and working in my favor
                        * Basically because AppearanceClass.NextTable is passed as a ref into GenerateAllResults it is modified directly 
                        * when we set Generate the next result
                        * basically we continue to setting this value (not sure why the breakpoint never succeeded though)
                        * 
                        * So... I don't want to mess everything, so if we are in LINEAR mode then we 
                        * manualy set the table.
                        */
					
					
					// if (nextTable == old_next_table)
					//{
					//     nextTable = Header.Blank;
					// }
					//  nextTable = table.NextTable;
					if (newResult.sResult == " \n")
					{
						newResult.sResult = Constants.BLANK;
					}
					sResult.sResult = sResult.sResult + newResult.sResult;
					sResult.nModifier = newResult.nModifier;
				}
				else
				{
					// if we hit a null then we hit user error
					// abort
					nextTable =Constants.BLANK;
				}
				
				
				
			}
			
			
			nextTable = sOriginalNextTable;
			return sResult.sResult;
		}
		
		
		
		/// <summary>
		/// Adds
		/// 
		/// ASSUMPTIONS
		///  1. Last row contains the Highest number (the die roll)
		/// </summary>
		/// <param name="nModifier">Some tables influence the value of the next table. Generally this should be 0</param>
		/// 
		public static resultReturn GenerateResult(int nModifier, DataTable currentTable, ref string nextTable, string Title, NoteDataInterface[] notes)
		{
			resultReturn returner = new resultReturn();
			returner.nModifier = 0;
			returner.sResult = Constants.BLANK;
			
			// error checking
			// if result column does not exist
			if (currentTable.Columns.IndexOf(TableWrapper.Result) == -1 ||
			    currentTable.Columns.IndexOf(TableWrapper.Roll) == -1)
			{
				NewMessage.Show(TableWrapper.ColumnDoesNotExist);
				return returner;
			}
			
			myRange[] ranges = new myRange[currentTable.Rows.Count];
			// this is complicated.
			// I need to break each range apart into its components numbers
			// and do validity checking on it.
			
			// make the validity tests micro enough to use elsewhere
			// in case I need an authenticator
			
			
			int nFoundRowNumber = 0;
			int nValue = 0;
			int nMin = 0;
			int nMax = 0;
			if (currentTable.Rows[0][0].ToString() == "-1" )
			{
				// This is a linear table
				// Linear tables go through a checklist, one table after another. 
				// RULES:
				// 
				// - A result is taken (generally flat values or lookup results) and then we go to the next row
				// - If a result results in a next_table that is TAKEN, breaking the linear table
				// - to navigate we set Nexttable to the next row number but with a MINUS preceding it
				
				if (nextTable.Length > 0 && nextTable[0] == '-')
				{
					nFoundRowNumber = Math.Abs(Int32.Parse(nextTable));
				}
				else
				{
					nFoundRowNumber = 0;
				}
			}
			else
			{
				// fill in ranges values
				ranges = TableWrapper.BuildRangeArray(ranges, currentTable);
				
				if (ranges == null)
					return returner;
				
				// find maximum value in last row
				nMin = ranges[0].nMin;
				nMax = ranges[ranges.Length - 1].nMax;
				// roll the die
				Random r = new Random();
				nValue = nModifier + r.Next(nMin, nMax + 1);
				
				// if the value, with modifier, is now greater than the max, then set it to max
				if (nValue > nMax)
					nValue = nMax;
				
				
				// Go through each row and test to see if the number fits within the range
				for (int j = 0; j < ranges.Length; j++)
				{
					// am I greater than or equal to min value?
					if (nValue >= ranges[j].nMin)
					{
						// am I less than or equal to max value for this row
						if (nValue <= ranges[j].nMax)
						{
							nFoundRowNumber = j;
						}
					}
				}
			} // random lookup
			
			// we override the NExtTable result if there is a valid valu
			try
			{
				
				if (!Convert.IsDBNull(currentTable.Rows[nFoundRowNumber][TableWrapper.NextTable]))
				{
					
					string sNext = currentTable.Rows[nFoundRowNumber][TableWrapper.NextTable].ToString();
					if (sNext != null && sNext != Constants.BLANK)
					{
						nextTable = sNext;
					}
				}
				else if (nextTable.Length > 1 && nextTable[0] == '-')
				{
					// a linear table
					// to keep from falling into an infinite loop we need to drop out
					nextTable = "";
				}
			}
			catch (Exception ex)
			{
				NewMessage.Show (ex.ToString());
			}
			
			if (currentTable.Columns.IndexOf(TableWrapper.Modifier) > -1)
			{
				if (currentTable.Rows[nFoundRowNumber][TableWrapper.Modifier] == null)
				{
					returner.nModifier = 0;
				}
				else
				{
					try
					{
						returner.nModifier = (int)Int32.Parse(currentTable.Rows[nFoundRowNumber][TableWrapper.Modifier].ToString());
					}
					catch (Exception)
					{
						returner.nModifier = 0;
					}
				}
			}
			else
			{
				returner.nModifier = 0;
			}
			string sDebugOnly = Constants.BLANK;
//			if (DebugMode == true)
//			{
//				sDebugOnly = "Modifier = " + nModifier.ToString() + " Roll =" + nValue.ToString() + "/" + nMin.ToString() + "-" + nMax.ToString() + " ::: ";
//			}
			
			
			string sResult = currentTable.Rows[nFoundRowNumber][TableWrapper.Result].ToString();
			
			
			if (sResult.ToLower().IndexOf("lookup") > -1)
			{
				
				// NewMessage.Show("lookup found");
				sResult = lookup(sResult, currentTable, notes);
				
			}
			
			string sTitle = sDebugOnly + Title + " ";
			if (sTitle == " ")
			{
				sTitle = ""; // get rid of space if there was no title
			}
			returner.sResult = sTitle + sResult + Environment.NewLine;
			
			return returner;
		}

		/// <summary>
		///  // PROPER FORMAT lookup(colum1, column1value, table, columns to get, )
		// ** Cut double column lookup. Instead we make sure we look in BOTH archetype columns **
		// i.e., lookup(archetype1|archetype2, Woman's Man, archetyperelations, clash|mesh)
		/*described.
         * In the example above we want to grab any row that has WOMAN'S MAN on it and return both the clash and mesh columns
         */
		/// </summary>
		/// <param name="sQuery"></param>
		/// <param name="dataSource"></param>
		/// <returns>
		/// Keep in mind that the result might be multiple Rows!
		/// </returns>
		private static string lookup(string sQuery, DataTable currentTable, NoteDataInterface[] notes)
		{
			
			sQuery = sQuery.Replace("(", "");
			sQuery = sQuery.Replace(")", "");
			string sResults = "";
			sQuery = sQuery.Substring(6, sQuery.Length - 6);
			string[] query_params = sQuery.Split(new char[1] { ',' });
			string tablename = query_params[2].Trim();
			string matchvalue = query_params[1].ToLower().Trim();
			// params should be 
			//[0] = COLUMN 
			//[1] = column value to match
			//[2] = table name
			//[3] = columsn to get [this is delimited with a slash /  which in turn will need to be broken into multiple values]
			//[4] (optional) IF specified then bool RandomResult is set to true
			bool RandomResult = false;
			if (query_params.Length == 5)
			{
				// this means there is a random result column
				// but we double check to make sure its an exclamation point
				if (query_params[4] == "!")
				{
					RandomResult = true;
				}
			}
			//  should multiple columns be handled the way I intend to handle multiple columns to get? Yes.
			
			char[] splitter = new char[1] { '|' };
			string[] ColumnsToMatch = query_params[0].Split(splitter);
			string[] ColumnsToGet = query_params[3].Split(splitter);
			string captionoverrideforrandomresultset = "";
			// grab the table
			DataTable table = null;
			foreach (NoteDataInterface note in notes)
			{
				if (note != null && note.GetType() == typeof(NoteDataXML_Table))
				{
					if (tablename == note.Caption)
					{
						// matched table
						table = (DataTable)((NoteDataXML_Table)note).dataSource;
						captionoverrideforrandomresultset = ((NoteDataXML_Table)note).TableCaption; // only for random result sets
						break;
					}
				}
			}
			
			if (null == table)
			{
				return "table not found";
			}
			
			sResults = sResults + Environment.NewLine;
			string[] randomizeresult = new string[0]; // Sep 2011 for returning a random result from a list of all results (used generally with Linear Tables)
			string[] randomizeresult_nexttable = new string[0]; // Bit of a hack but when we store the result we also store a lookup table... which can then be looked down if parsed
			
			foreach (string column in ColumnsToMatch)
			{
				
				// process each column
				//sResults = sResults + column;
				foreach (DataRow row in table.Rows)
				{
					string sValueFound =  row[column.Trim()].ToString();
					if (sValueFound  == matchvalue || "*" == matchvalue || "*" == sValueFound)
					{
						foreach (string columntoget in ColumnsToGet)
						{
							string found = row[columntoget.Trim()].ToString();
							if (false == RandomResult)
							{
								
								string columnlabel = columntoget.ToUpper() + Environment.NewLine;
								if (ColumnsToGet.Length == 1)
								{
									// if we only have one column we don't display the column label
									columnlabel = "";
								}
								
								sResults = String.Format("{0} {1} {2} {3}", sResults, columnlabel, found, Environment.NewLine);
							}
							else
							{
								// we want to grab all the values but then randomize them
								Array.Resize(ref randomizeresult, randomizeresult.Length + 1);
								randomizeresult[randomizeresult.Length - 1] = found;
								
								if (table.Columns.IndexOf("Next Table") > -1)
								{
									Array.Resize(ref randomizeresult_nexttable, randomizeresult_nexttable.Length + 1);
									randomizeresult_nexttable[randomizeresult_nexttable.Length - 1] = row["Next Table"].ToString();
								}
								
							}
						}
					}
				}
				
				
				
			}
			
			
			if (true == RandomResult)
			{
				int length = randomizeresult.Length;
				Random r = new Random();
				int nValue = r.Next(0, length);
				sResults = String.Format("{0}: {1}", captionoverrideforrandomresultset, randomizeresult[nValue]);

				if (randomizeresult_nexttable != null && randomizeresult_nexttable.Length >0 && randomizeresult_nexttable[nValue].IndexOf("lookup") > -1)
				{
					// we have a lookup value in the next table
					// go do that lookup
					string sResult = randomizeresult_nexttable[nValue];
					sResults = sResults + Environment.NewLine + lookup(sResult, currentTable, notes);
				}
				
			}
			
			return sResults;
		}
		/// <summary>
		/// Gets the values for column.
		/// returns a list of strings for that column. Used for system tables on the system layout
		/// </summary>
		/// <returns>
		/// The values for column.
		/// </returns>
		/// <param name='columnIndex'>
		/// Column index.
		/// </param>
		public List<string> GetValuesForColumn (int columnIndex)
		{
			List<string> result = new List<string>();
			if (dataSource != null) {
				if (dataSource is DataTable)
				{
					if (dataSource.Columns.Count > columnIndex)
					{
					foreach (DataRow row in dataSource.Rows)
					{
						result.Add ( row[columnIndex].ToString ());
					}
					}
				}
			}
			return result;
		}
	}
}

