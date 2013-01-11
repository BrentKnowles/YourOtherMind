using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CoreUtilities;


namespace CoreUtilities.Tables
{
	
	/// <summary>
	/// this structure contains a string and a modifier value
	/// that is returned from GenerateResult
	/// </summary>
	public struct resultReturn
	{
		public int nModifier;
		public string sResult;
	}

	
	/// <summary>
	/// This structi s used by Generate result to hold the min/maxes of each row
	/// </summary>
	public struct myRange
	{
		public int nMin;
		public int nMax;
	};
	
	/// <summary>
	/// September 2011
	/// A static wrapper for manipulating tables for use with classPageTable and the Table I ported into VisualDesk
	/// </summary>
	public static class TableWrapper
	{
		public const string TablePageTableName= "Table";
		public const string Result="Result";
		public const string  Roll="Roll";
		public const string InvalidRow = "InvalidRow";
		public const string NextTable = "Next Table";	
		public const string Modifier = "Modifier";
		public static string ImportError ()
		{
			return Loc.Instance.GetString("Error in importing table data");
		}
		public static string ColumnDoesNotExist {
			get { return Loc.Instance.GetString ("Column {0} does not exist");}
		}
		/// <summary>
		/// returns an array of Min/Max values representing
		/// the rows in the current table
		/// </summary>
		/// <returns></returns>
		public static myRange[] BuildRangeArray(myRange[] ranges, DataTable currentTable)
		{
			int nCount = -1;
			
			
			// if there's not a valid Roll columnt hen don't bother
			// just return out of here
			try
			{
				if (currentTable.Columns.IndexOf(TableWrapper.Roll) > -1)
				{
					if (currentTable.Rows[0][TableWrapper.Roll] == null)
					{
						return null;
					}
					else
					{
						try
						{
							// returner.nModifier = (int)Int32.Parse(currentTable.Rows[nFoundRowNumber][Header.Roll].ToString());
						}
						catch (Exception)
						{
							return null;
						}
					}
				}
				else
				{
					return null;
				}
				
			}
			catch (Exception)
			{
				lg.Instance.Line("classPageTable BuildRange Array", ProblemType.EXCEPTION, "jumped to non table page?");
				return null;
			}
			foreach (DataRow dr in currentTable.Rows)
			{
				nCount++;
				bool bError = false;
				
				
				
				string sValue = dr[TableWrapper.Roll].ToString();
				try
				{
					int nValue; // used for int test
					if (sValue != null && sValue != Constants.BLANK && sValue.IndexOf("-") > 0)
					{
						string[] results = sValue.Split(new char[1] {'-'});
						for (int i = 0; i < results.Length; i++)
						{
							results[i] = results[i].Trim();
						}
						ranges[nCount].nMin = Int32.Parse(results[0]);
						ranges[nCount].nMax = Int32.Parse(results[1]);
					}
					else
						// if this is just a number than it goes with the same number of min and max
						if (sValue != null && sValue != Constants.BLANK && Int32.TryParse(sValue, out nValue) == true)
							
					{
						ranges[nCount].nMin = nValue;
						ranges[nCount].nMax = nValue;
						
					}
					else
					{
						bError = true;
					}
				}
				catch (Exception)
				{
					bError = true;
				}
				if (bError == true)
				{
					lg.Instance.Line("TablePanel->BuildRangeArray", 
					                 ProblemType.ERROR, String.Format("{0} {1} {2}", Loc.Instance.GetString("Invalid Row"), nCount.ToString(), sValue));
					
					
				}
				
			}
			return ranges;
		}
		
		
		
		
		
		
		
		
		
		/// <summary>
		/// imports the list into the Result oclumn and autonumbers the roll column
		/// will not do anything without botha  roll and result column (not true; and that's good because this makes it more flexible)
		/// 
		/// If # of columns do not match, dumps everything into second column
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// 
		/// <param name="list"></param>
		public static void ImportList(string[] list, DataTable currentTable)
		{
			
			//if (currentTable.Columns.IndexOf(Header.Result) > -1
			//    && currentTable.Columns.IndexOf(Header.Roll) > -1)
			if (currentTable.Columns.Count > 0)
			{
				
				try
				{
					
					
					myRange[] ranges = new myRange[currentTable.Rows.Count];
					int nMax = 0;
					try
					{
						ranges = BuildRangeArray(ranges, currentTable);
						
						// find maximum value in last row
						nMax = ranges[ranges.Length - 1].nMax;
					}
					catch (Exception)
					{
						nMax = 0;
					}
					foreach (string s in list)
					{
						string sResult = s;
						nMax++;
						string sRow = nMax.ToString(); // currentTable.Rows.Count.ToString();
						
						if (s != null && s != Constants.BLANK)
						{
							// this bit is complicated because I don't know what
							// columns you have.
							string[] sResultArray = new string[currentTable.Columns.Count];
							for (int ij = 0; ij < currentTable.Columns.Count; ij++)
							{
								sResultArray[ij] = "";
							}
							//{ sRow, sResult, sNextTable, sModifier };
							
							// build array to size of columns
							// If Roll Column is Present
							//        add Roll Data
							//        add text to next column after it, unless last then do first
							
							/* If you have a row column it will only parse the CSV first column
                             * to keep things simple*/
							/* September 2011 - NO NO NO
                             * Changing this. If we do a split and the count does not match the COLUMNS
                             * then we use the 'simple system'; else we use the complicated one
                             */
							
							string[] sCSV = sResult.Split(',');
							
							//
							//if (nRowColumn > -1)
							if (sCSV.Length != currentTable.Columns.Count)
							{
								int nRowColumn = currentTable.Columns.IndexOf(TableWrapper.Roll);
								sResultArray[nRowColumn] = sRow;
								if (nRowColumn < currentTable.Columns.Count - 1)
								{
									sResultArray[nRowColumn + 1] = sResult;
								}
								else
								{
									sResultArray[nRowColumn - 1] = sResult;
								}
							}
							else
							{
								//* We don't have a roll column
								// * now we check and try to parse CSV format text
								
								
								for (int kk = 0; kk < sCSV.Length; kk++)
								{
									// if we have not exceeded length of Result array, fill it in
									if (kk < sResultArray.Length)
									{
										sResultArray[kk] = sCSV[kk];
									}
								}
								
								//sResultArray[0] = sResult;
							}
							
							// if Roll Column is NOT present
							//        DO NOT add Roll Data
							//        add text to first column
							currentTable.Rows.Add(sResultArray)
								;
						}
					}
				}
				catch (Exception)
				{
					NewMessage.Show(TableWrapper.ImportError());
				}
			}
			else
			{
				NewMessage.Show(TableWrapper.ImportError());
			}
		}
		
		

		

		
		private static bool mDebug;
		/// <summary>
		/// a testing only variable. if true then we can see the debug results of a roll
		/// </summary>
		
		public static bool DebugMode
		{
			get { return mDebug; }
			set { mDebug = value; }
		}
	}// class
	
	
}
