// EventTable.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using System.Windows.Forms;
//-Using
using CoreUtilities;
using Transactions;

namespace Incentives
{
	/// <summary>
	/// THis is a static class that retrieves event table information
	/// </summary>
	public static class EventTable
	{
		
		/* This tableis made up of events
         *  - Dates are NOT unique
         *  -   Date  Type Data
        */
		// FIelds
		static public string F_DATE = "Date";
	

 // always in format day/month/year
		static public string F_TYPE = "Type"; // int
		static public string F_DATA = "Data";    // Page Name
		static public string F_DATA2 = "Data2";  // Page Type
		static public string F_DATA3 = "Data3";  // 
		static public string F_DATA4 = "Data4";  // 
		static public string YOM_GUID = "YOMGUID";
		static public string YOM_SUBMISSIONS = "YOMSUBS";
		
		
		public const int T_ADDED = 1;
		public const int T_DELETED = 2;
		public const int T_RETIRED = 3;
		public const int T_FINISHED = 4;
		public const int T_USER = 5;
		public const int T_ADDSUB = 6;
		public const int T_REGISTER = 7;
		public const int T_WEB = 8;
		public const int T_LINK = 9;
		public const int T_TENWINDOWS = 10;
		public const int T_BRAIN = 11;
		public const int T_WORDCOUNT = 12;
		public const int T_NAGSTARTED = 13;
		public const int T_NAGINTERRUPTED = 14;
		
		
		const string pathandfile = "C:\\temppicdirectory\\EventTable.xml";
		// builds the table if it doesn't exist
		//
		public static DataSet _LoadTable()
		{

			//Logs.Line(String.Format("Incentives table looking for {0} path", path.DataDirectory + "\\EventTable.xml"));
			if (File.Exists(pathandfile) == false)
			{
			//	Logs.Line("Creating new event table");
				DataTable pageIndex = new DataTable();
				pageIndex.Columns.Add(F_DATE, typeof(DateTime));
				pageIndex.Columns.Add(F_TYPE, typeof(int));
				
				pageIndex.Columns.Add(F_DATA, typeof(string));
				pageIndex.Columns.Add(F_DATA2, typeof(string));
				pageIndex.Columns.Add(F_DATA3, typeof(int));
				pageIndex.Columns.Add(F_DATA4, typeof(int));
				
				pageIndex.Columns.Add(YOM_GUID, typeof(string));
				pageIndex.Columns.Add(YOM_SUBMISSIONS, typeof(string));
				
				DataSet ds = new DataSet();
				ds.Tables.Add(pageIndex);
				return ds;
			}
			else
			{
				DataSet ds = new DataSet();
				try
				{
					ds.ReadXml(pathandfile);
					
					//Feb 2012
					// as part of the conversion process I am just trying to loads this TABLE into a NoteTable.
					// ASSUMPTION: Everybody has an EventTable.
					// TO DO:
					//       1. Rename this file
					//       2. Don't rebuild this file (see above, the 'if')
					// CONSIDERATIO: Just leave this but add a 'manual' conversion to Table? I.e., you can load any DataTable.
					
				}
				
				catch (Exception ex)
				{
				//	namespaceLogs.Logs.Line("EventTable_LoadTable", "file was blank - rare problem", "Solution: Told User and aborted");
					NewMessage.Show(String.Format("Sorry! There was a fatal error with {0}. You need to restore a backup of that file and reload YourOtherMind. Now closing application.", ex.ToString()));
					Application.Exit();
					
				}
				
				return ds;
			}
			
		}
		
		
		/// <summary>
		/// created a wrapper that allow sspecial codes. This just passes nSpecial= 0 (nothing)
		/// </summary>
		/// <param name="row"></param>
		/// <param name="sData1"></param>
		/// <param name="sData2"></param>
		/// <param name="nData3"></param>
		/// <param name="nData4"></param>
		/// <param name="nType"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public static bool UpdateRow(string sData1, string sData2, int nData3, int nData4, int nType, DateTime date)
		{
			return UpdateRow(sData1, sData2, nData3, nData4, nType, date, 0, null);
		}
		
		/// <summary>
		/// Updates an existing row.
		/// 
		/// This can only be used for USER enters rows becuase they
		/// are meant to be unique.
		/// 
		/// It can't work for systems rows like T_WORD, et cetera
		/// unless you are using nSpecial = 1 (which means to also look for a match for sData1)
		/// </summary>
		/// <param name="date"></param>
		/// <param name="nData3"></param>
		/// <param name="nData4"></param>
		/// <param name="row"></param>
		/// <param name="nType"></param>
		/// <param name="sData1"></param>
		/// <param name="sData2"></param>
		/// <param name="nSpecial">0 - nothing, 1 - means to treat sData1 as a look up too</param>
		/// <returns>True, if an existing row was found</returns>
		public static bool UpdateRow(string sData1, string sData2, int nData3, int nData4, int nType, DateTime date, int nSpecial, DataSet source)
		{
			// have to repeat the GetRow code her
			// not ideal
			
			DataSet ds = null;
			if (source == null)
			{
				ds = _LoadTable();
			}
			else
				ds = source;
			
			
			// DataSet ds = _LoadTable();
			bool bRowFound = false;
			
			if (ds != null)
			{
				
				
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					string sDateString = GetDateString(date);
					if (nType == T_BRAIN)
					{
						date = new DateTime(1950, 01, 01);
					}
					
					bool bSpecialTest = true;
					if (nSpecial == 1)
					{
						if (sData1 != ds.Tables[0].Rows[i][F_DATA].ToString())
						{
							bSpecialTest = false;
						}
					}
					
					
					if ((int)ds.Tables[0].Rows[i][F_TYPE] == nType && (DateTime)ds.Tables[0].Rows[i][F_DATE] == date && bSpecialTest == true)
					{
						// found a match.
						// return this row
						ds.Tables[0].Rows[i].BeginEdit();
						ds.Tables[0].Rows[i][F_DATA] = sData1;
						ds.Tables[0].Rows[i][F_DATA2] = sData2;
						ds.Tables[0].Rows[i][F_DATA3] = nData3;
						ds.Tables[0].Rows[i][F_DATA4] = nData4;
						
						
						ds.Tables[0].Rows[i].EndEdit();
						bRowFound = true;
						break;
					}
				}
				
			}
			// now edit
			
			
			if (bRowFound == true)
			{
				if (source == null)
					ds.WriteXml(pathandfile, XmlWriteMode.WriteSchema);
			}
			return bRowFound;
		}
		
		
		/// <summary>
		/// created a wrapper to ensure consistentcy
		/// </summary>
		public static DateTime Today
		{
			get { return DateTime.Today; }
		}
		
		/// <summary>
		/// Adds a row to the Event Table
		/// 
		/// Opens (or creates) the table
		/// Creates the row
		/// Writes the row
		/// Saves the table
		/// 
		/// </summary>
		/// <param name="sData1"></param>
		/// <param name="sData2"></param>
		/// <param name="nType"></param>
		
		public static void Add(string sData1, string sData2, int nData3, int nData4, int nType)
		{
			Add(sData1, sData2, nData3, nData4, nType, Today, null);
			
		}
		
		/// <summary>
		/// Add but with a date override
		/// 
		/// 
		/// T_WORD 
		///    sData1 = sFileName
		///    sData2 = ""
		///    nData3 = Word Count
		/// 
		/// </summary>
		/// <param name="sData1"></param>
		/// <param name="sData2"></param>
		/// <param name="sData3"></param>
		/// <param name="nType">This shouldb e the constant like T_BRAIN</param>
		/// <param name="source">if null, grabs a new source. You can pass in an existing source for a speed improvement
		/// but you become responsible for saving the file yourself</param>
		/// <param name="date"></param>
		public static void Add(string sData1, string sData2, int nData3, int nData4, int nType, DateTime date, DataSet source)
		{
			
			DataSet ds = null;
			if (source == null)
			{
				ds = _LoadTable();
			}
			else
				ds = source;
			if (ds != null)
			{
				DataTable myTable = ds.Tables[0];
				
				string sDateString = GetDateString(date);
				if (nType == T_BRAIN)
				{
					date = new DateTime(1950, 01, 01);
				}
				
				myTable.Rows.Add(new object[6] { date, nType, sData1, sData2, nData3, nData4 });
				
				// we only save the file if we have not passed in a source
				if (source == null)
				{
					Save(ds);//ds.WriteXml(path.DataDirectory + "\\EventTable.xml", XmlWriteMode.WriteSchema);
				}
			}
		}
		
		public static void Save(DataSet ds)
		{
			ds.WriteXml(pathandfile, XmlWriteMode.WriteSchema);
		}
		
		/// <summary>
		/// Used for finding User-entered data
		/// Assumes there is only one USER Entry + Date
		/// 
		/// This can't be used to look for system added entries like
		/// Page additions, because these are not UNIQUE.
		/// </summary>
		/// <param name="date"></param>
		/// <param name="nType"></param>
		/// <returns></returns>
		public static DataRow getRow(DateTime date, int nType)
		{
			DataSet ds = _LoadTable();
			
			
			if (ds != null)
			{
				// load a DataROwFIlter
				DataView dv = new DataView(ds.Tables[0]);
				if (nType != T_BRAIN)
				{
					string sFilter = "Date='" + (date) + "'";
					dv.RowFilter = sFilter;
				}
				
				///row.BeginEdit();
				
				for (int i = 0; i < dv.ToTable().Rows.Count; i++)
				{
					if ((int)dv.ToTable().Rows[i][F_TYPE] == nType)
					{
						// found a match.
						// return this row
						return dv.ToTable().Rows[i];
					}
				}
				
			}
			return null;
			
		}
		
		/// <summary>
		/// breaks strings into a custom date string so I don't need to worry about them
		/// changing acros platofrms
		/// </summary>
		/// <param name="date"></param>
		private static string GetDateString(DateTime date)
		{
			// REMOVED CUT DEBUG used a custom date format but it killed my ability to do reports... removed
			int nMonth = date.Month;
			int nDay = date.Day;
			int nYear = date.Year;
			string sDate = String.Format("{0}/{1}/{2}", nMonth.ToString(), nDay.ToString(), nYear.ToString());
			return sDate;
		}
		
		/// <summary>
		/// returns the word count of the entry that is
		/// closest in date to dateMatch but
		/// DOES NOT equal dateMatch
		/// </summary>
		/// <param name="sData1Key"></param>
		/// <returns></returns>
		static public int GetLastWordCount(string sData1Key, DateTime dateMatch)
		{
			DataSet ds = _LoadTable();
			DataTable table = ds.Tables[0];
			
			int nClosestWords = 0;
			int nClosestDistance = 0;
			
			foreach (DataRow r in table.Rows)
			{
				if (r[F_DATA].ToString() == sData1Key && (int)r[F_TYPE] == T_WORDCOUNT)
				{
					TimeSpan totalDays = dateMatch - (DateTime)r[F_DATE];
					int nDistance = (int) totalDays.TotalDays;
					if (nDistance < nClosestDistance || nClosestDistance == 0)
					{
						if (nDistance != 0)
						{
							nClosestWords = (int)r[F_DATA3];
							nClosestDistance = nDistance;
							
						}
					}
				}
			}
			
			table = null; 
			ds = null;
			
			return nClosestWords;
			
		}
		
		
		
		/// <summary>
		/// returns an array of the year in which subs happened
		/// </summary>
		/// <returns></returns>
		static public int[] GetWorkHistoryYears()
		{
			DataSet ds = _LoadTable();
			DataTable table = ds.Tables[0];
			ArrayList newList = new ArrayList();
			foreach (DataRow r in table.Rows)
			{
				
				DateTime date = (DateTime)r[F_DATE];//(DateTime.ParseExact(r[F_DATE].ToString(),"MM/DD/YYYY"));
				
				// 1950 was used as a code year for Brainstorm counting
				if (date.Year != 1950 && date.Year != 1)
				{
					if (newList.IndexOf(date.Year) == -1)
					{
						// year does not exist, add it
						newList.Add(date.Year);
					}
				}
				
			}
			newList.Sort();
			
			int[] list = new int[newList.Count];
			newList.CopyTo(list);
			return list;
			
			
		}
		
		/// <summary>
		/// goes through the table looking pu the user- manually entered fields
		/// where word count is put into place and looking for matches where
		/// Data2 = any string in alias
		/// 
		/// ** To Get DateStart just return Date Created
		/// </summary>
		/// <param name="Alias"></param>
		/// <param name="Hours"></param>
		/// <param name="lastFinished">returns tomorrow if no date found</param>
		static public void GetAliasInformation(ArrayList Alias, out int Hours,  out DateTime lastFinished)
		{
			DataSet ds = _LoadTable();
			DataTable table = ds.Tables[0];
			
			lastFinished = DateTime.Today.AddDays(1);
			Hours = 0;
			
			foreach (DataRow r in table.Rows)
			{
				if ( (int)r[F_TYPE] == T_USER || (int)r[F_TYPE] == T_FINISHED)
				{
					foreach (string s in Alias)
					{
						if ((int) r[F_TYPE] == T_USER && s == r[F_DATA2].ToString())
						{
							// match for Hours
							try
							{
								Hours = Hours + (int)r[F_DATA3];
							}
							catch (Exception)
							{
							}
						}
						else
							if ((int)r[F_TYPE] == T_FINISHED && s == r[F_DATA].ToString())
						{
							// we have a finished date
							lastFinished = (DateTime) r[F_DATE];
						}
					}
				}
				
			}
			ds = null;
			table = null;
			
		}
		
		/// <summary>
		/// Counts using a date
		/// </summary>
		/// <param name="nYear"></param>
		/// <param name="sExtraFilter"></param>
		/// <returns></returns>
		static public string QueryCount(int nYear, string sExtraFilter)
		{
			return QueryCount(nYear, sExtraFilter, false);
		}
		/// <summary>
		/// counts the number of occurences of a type
		/// </summary>
		/// <param name="nYear"></param>
		/// <param name="sExtraFilter"></param>
		/// <returns>-1 if no event table found</returns>
		static public string QueryCount(int nYear, string sExtraFilter, bool bAllYears)
		{
			string sValue = "error";
			try
			{
				DataSet ds = _LoadTable();
				if (ds == null)
				{
					return "-1";
				}
				DataTable table = ds.Tables[0];
				
				string sCalcString1 = String.Format("Count({0})", F_DATA);
				
				//newDate
				string sCalcString2 = "";
				if (bAllYears == false)
				{
					DateTime newDateStart = new DateTime(nYear, 1, 1).Date;
					DateTime newDateEnd = new DateTime(nYear + 1, 1, 1).Date;
					
					sCalcString2 = String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", F_DATE, newDateStart, newDateEnd);
				}
				else if (bAllYears == true)
				{
					sCalcString2 = sExtraFilter;
					sExtraFilter = "";
				}
				
				if (sExtraFilter != "")
				{
					// add an extra query to query
					sCalcString2 = sCalcString2 + String.Format(" AND ({0})", sExtraFilter);
				} 
				//  Messa geBox.Show(String.Format("{0} -- {1}", sCalcString1, sCalcString2));
				//Messa geBox.Show(table.Compute(sCalcString1, sCalcString2).GetType().ToString());
				sValue = ((int)(table.Compute(sCalcString1, sCalcString2))).ToString();
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
			return sValue;
			
		}
		
		/// <summary>
		/// counts the number of occurences of a type
		/// </summary>
		/// <param name="nYear"></param>
		/// <param name="sExtraFilter"></param>
		/// <returns>-1 if no event table found</returns>
		static public string QueryMax(int nYear, string sExtraFilter, bool bAllYears, string maxcol)
		{
			string sValue = "error";
			try
			{
				DataSet ds = _LoadTable();
				if (ds == null)
				{
					return "-1";
				}
				DataTable table = ds.Tables[0];
				
				string sCalcString1 = String.Format("Max({0})", maxcol);
				
				//newDate
				string sCalcString2 = "";
				if (bAllYears == false)
				{
					DateTime newDateStart = new DateTime(nYear, 1, 1).Date;
					DateTime newDateEnd = new DateTime(nYear + 1, 1, 1).Date;
					
					sCalcString2 = String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", F_DATE, newDateStart, newDateEnd);
				}
				else if (bAllYears == true)
				{
					sCalcString2 = sExtraFilter;
					sExtraFilter = "";
				}
				
				if (sExtraFilter != "")
				{
					// add an extra query to query
					sCalcString2 = sCalcString2 + String.Format(" AND ({0})", sExtraFilter);
				}
				//  Messa geBox.Show(String.Format("{0} -- {1}", sCalcString1, sCalcString2));
				//Messa geBox.Show(table.Compute(sCalcString1, sCalcString2).GetType().ToString());
				sValue = ((int)(table.Compute(sCalcString1, sCalcString2))).ToString();
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
			return sValue;
			
		}
		/// <summary>
		/// retrieves a  SUM value based on criteria
		///  
		/// HOURS is 3
		/// 
		/// Example usage
		/// </summary>
		/// <param name="nYear"></param>
		/// <returns></returns>
		static public string Query(string sSumColumn, int nYear, string sExtraFilter)
		{
			string sValue = "error";
			try
			{
				DataSet ds = _LoadTable();
				DataTable table = ds.Tables[0];
				
				string sCalcString1 = String.Format("SUM({0})", sSumColumn);
				DateTime newDateStart = new DateTime(nYear, 1, 1).Date;
				DateTime newDateEnd = new DateTime(nYear + 1, 1, 1).Date;
				
				//newDate
				string sCalcString2 = String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", F_DATE, newDateStart, newDateEnd);
				if (sExtraFilter != "")
				{
					// add an extra query to query
					sCalcString2 = sCalcString2 + String.Format(" AND ({0})", sExtraFilter);
				}
				//  Messa geBox.Show(String.Format("{0} -- {1}", sCalcString1, sCalcString2));
				sValue = ((Int64)(table.Compute(sCalcString1, sCalcString2))).ToString();
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
			return sValue;
			
		}
		
		
		static public string QueryValueForTimePeriod(DataTable table, string sValue, string sSumColumn, string sExtraFilter, DateTime newDateStart, DateTime newDateEnd)
		{
			return QueryValueForTimePeriod(table, sValue, sSumColumn, sExtraFilter, newDateStart, newDateEnd, 0);
		}
		/// <summary>
		/// January 8 2010
		/// Breaking apart QueryMonthInYear so the date look up is a bit moduldar
		/// </summary>
		/// <param name="sSumColumn"></param>
		/// <param name="sExtraFilter"></param>
		/// <param name="newDateStart"></param>
		/// <param name="newDateEnd"></param>
		/// <param name="nTypeOfQuery">0 - Normal, 1 - Max</param>
		/// <returns></returns>
		static public string QueryValueForTimePeriod(DataTable table, string sValue, string sSumColumn, string sExtraFilter, DateTime newDateStart, DateTime newDateEnd, int nTypeOfQuery)
		{
			/* HUGE ISSUE
              * ew DateTime(2008,10,12) works because it is encoding it as YEAR/DAY/MONTH which is what the database wants
              * I need a way to force the database to work with the same date system
              * as the core*/
			
			string sCalcString1 = "";
			/// if not an integer column do a count instead
			if (1 == nTypeOfQuery)
			{
				sCalcString1 = String.Format("MAX({0})", sSumColumn);
			}
			else
				if (sSumColumn == F_DATA)
			{
				sCalcString1 = String.Format("COUNT({0})", sSumColumn);
			}
			else
				sCalcString1 = String.Format("SUM({0})", sSumColumn);
			
			
			string sCalcString2 = String.Format("({0} >= #{1}#) AND ({0} <#{2}#)", F_DATE, newDateStart.ToString("u"), newDateEnd.ToString("u"));// String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", F_DATE, newDateStart, newDateEnd);
			if (sExtraFilter != "")
			{
				// add an extra query to query
				sCalcString2 = sCalcString2 + String.Format(" AND ({0})", sExtraFilter);
			}
			//  Mess ageBox.Show(String.Format("{0} -- {1}", sCalcString1, sCalcString2));
			object compute = table.Compute(sCalcString1, sCalcString2);
			if (compute == null || compute.GetType() == typeof(DBNull))
			{
				sValue = "null compute";
			}
			else
				sValue = compute.ToString();
			
			return sValue;
		}
		
		/// <summary>
		/// Takes current date.
		/// Finds most recent monday.
		/// Then tabulates up until this date.
		/// 
		/// So, basically ifyou did this on a Tuesday you wuld get the Monday and the Tuesday result.
		/// </summary>
		/// <param name="sSumColumn"></param>
		/// <param name="?"></param>
		/// <param name="sExtraFilter"></param>
		/// <returns></returns>
		static public string QueryLastWeek(DateTime daytouse, string sSumColumn, string sExtraFilter)
		{
			string sValue = "error";
			try
			{
				DataSet ds = _LoadTable();
				DataTable table = ds.Tables[0];
				
				
				
				
				DateTime newDateEnd = daytouse.Date; // was DateTime.Today.Date
				int nDays = 0; // how many days to compare
				
				
				
				DayOfWeek dayofweek = newDateEnd.DayOfWeek;
				switch (dayofweek)
				{
				case DayOfWeek.Monday: nDays = 0; break;
				case DayOfWeek.Tuesday: nDays = 1; break;
				case DayOfWeek.Wednesday: nDays = 2; break;
				case DayOfWeek.Thursday: nDays = 3; break;
				case DayOfWeek.Friday: nDays = 4; break;
				case DayOfWeek.Saturday: nDays = 5; break;
				case DayOfWeek.Sunday : nDays = 6; break;
				}
				
				// we have to add 1 to dateend
				newDateEnd = newDateEnd.AddDays(1).Date;
				DateTime newDateStart = daytouse.AddDays(-1 * nDays).Date;
				
				//newDate
				
				
				sValue = QueryValueForTimePeriod(table, sValue, sSumColumn, sExtraFilter, newDateStart, newDateEnd);
				
				
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
			return sValue;
		}
		/// <summary>
		/// general purpose get Sumo f a particular column, for a specific month in year
		/// 
		/// Lots of datetime problems
		/// http://ca.wrs.yahoo.com/_ylt=A0oGk5h9ulVJ4FYB_QHrFAx.;_ylu=X3oDMTEzczU3a2phBHNlYwNzcgRwb3MDNQRjb2xvA3NrMQR2dGlkA0NBQzAwMV8x/SIG=12p0m13ok/EXP=1230441469/**http%3a//www.c-sharpcorner.com/Forums/ShowMessages.aspx%3fThreadID=6078
		/// Partially solved. datetime.ToString("u") works independently of system date format. 
		/// 
		/// Also to consider
		/// Try using:
		
		//DateTime.ParseExact(_VardtFromDb, "MM/dd/yyyy",null).ToString(_VarDateFormat)
		static public string QueryMonthInYear(string sSumColumn, int nYear, string sExtraFilter, int nMonth)
		{
			return QueryMonthInYear(sSumColumn, nYear, sExtraFilter, nMonth, 0);
		}
		/// </summary>
		/// <param name="sSumColumn"></param>
		/// <param name="nYear"></param>
		/// <param name="sExtraFilter"></param>
		/// <param name="nMonth"></param>
		/// <returns></returns>
		static public string QueryMonthInYear(string sSumColumn, int nYear, string sExtraFilter, int nMonth, int nTypeOfQuery)
		{
			string sValue = "error";
			try
			{
				DataSet ds = _LoadTable();
				DataTable table = ds.Tables[0];
				
				
				
				int nNumberOfDaysInMonth = DateTime.DaysInMonth(nYear, nMonth);
				
				
				DateTime newDateStart = new DateTime(nYear, nMonth, 1).Date;
				DateTime newDateEnd = new DateTime(nYear, nMonth, nNumberOfDaysInMonth).Date;
				
				//newDate
				
				
				sValue = QueryValueForTimePeriod(table, sValue, sSumColumn, sExtraFilter, newDateStart, newDateEnd, nTypeOfQuery);
				
				
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
			return sValue;
			
		}
		
		/// <summary>
		/// returns a month by month breakdown of progress
		/// 
		/// will include both Hours, Words, FInished and retired, formatted like
		/// 
		///   January 2008
		///     Minutes: 203  (F_DATA3)
		///     Words: 1200 (F_DATA4)
		///     Finished: 0
		///     Retired: 1
		///  
		/// Example usage
		/// </summary>
		/// <param name="nYear"></param>
		/// <returns></returns>
		static public string QueryMonthsInYearReport(int nYear, int nMonth)
		{
			//
			string nMinutes = QueryMonthInYear(F_DATA3, nYear, String.Format("{0}='{1}'", F_TYPE, T_USER), nMonth);
			string Words = "0";
			string Added = "0";
			string AddedSub = "0";
			string Finished = "0";
			string Retired = "0";
			string Nag = "0";
			string MaxWords = "0";
			int minutes = 0;
			int hours = 0;
			
			try
			{
				minutes = Int32.Parse(nMinutes);
				hours = (int)(minutes / 60);
				nMinutes = String.Format("{0} (~{1} hours)", nMinutes, hours.ToString());
				
				
				
				Words = QueryMonthInYear(F_DATA4, nYear, String.Format("{0}='{1}'", F_TYPE, T_USER), nMonth);
				Added = QueryMonthInYear(F_DATA, nYear, String.Format("{0}={1}", F_TYPE, T_ADDED), nMonth);
				
				AddedSub = QueryMonthInYear(F_DATA, nYear, String.Format("{0}={1}", F_TYPE, T_ADDSUB), nMonth);
				
				Finished = QueryMonthInYear(F_DATA, nYear, String.Format("{0}={1}", F_TYPE, T_FINISHED), nMonth);
				
				Retired = QueryMonthInYear(F_DATA, nYear, String.Format("{0}={1}", F_TYPE, T_RETIRED), nMonth);
				Nag = QueryMonthInYear(F_DATA, nYear, String.Format("{0}={1}", F_TYPE, T_NAGINTERRUPTED), nMonth);
				MaxWords = QueryMonthInYear(F_DATA4, nYear, String.Format("{0}={1}", F_TYPE, T_USER), nMonth, 1);
			}
			catch (Exception)
			{
				minutes = 0;
				hours = 0;
				nMinutes = "0";
			}
			
			
			DateTime date = new DateTime(nYear, nMonth, 1);
			
			
			string sValue =
				String.Format("{0}\r\nMinutes: {1} \r\nWords: {2}\r\nFinished: {3}\r\nRetired: {4}\r\nAdded: {5} \r\nSubmissions: {6} \r\nMax Words in One Day: {7} \r\nDistracted: {8}",
				              date.ToString("MMMM"), nMinutes, Words, Finished, Retired, Added, AddedSub, MaxWords, Nag);
			return sValue;
			
		}
		
		/// <summary>
		/// for web form feedback
		/// on crashing or bugs
		/// </summary>
		/// <returns></returns>
		public static string GetDataAsTextString(DateTime datefilter)
		{
			string sLineFeed = "%OA";
			string sText = "";
			
			DataSet ds = _LoadTable();
			
			
			if (ds != null)
			{
				// load a DataROwFIlter
				DataView dv = new DataView(ds.Tables[0]);
				
				
				///row.BeginEdit();
				
				for (int i = 0; i < dv.ToTable().Rows.Count; i++)
				{
					if ((DateTime)dv.ToTable().Rows[i][F_DATE] == datefilter)
					{
						sText = String.Format("{0} {1} {2} {3} {4}", sText, sLineFeed, dv.ToTable().Rows[i][F_DATE],
						                      dv.ToTable().Rows[i][F_DATA].ToString(), dv.ToTable().Rows[i][F_DATA2].ToString());
					}
					
				}
				
			}
			return sText;
		}
		/// <summary>
		///  get week states for current week
		/// </summary>
		/// <param name="daytouse">Will build the week based off of this date</param>
		/// <returns></returns>
		public static string GetWeekStats(DateTime daytouse)
		{
			//DateTime todaysDate = DateTime.Today();
			
			string nMinutes = QueryLastWeek(daytouse, F_DATA3, String.Format("{0}='{1}'", F_TYPE, T_USER));
			
			int minutes = 0;
			Int32.TryParse(nMinutes, out minutes);
			int hours = (int)(minutes / 60);
			nMinutes = String.Format("{0} (~{1} hours)", minutes.ToString(), hours.ToString());
			
			string Words = QueryLastWeek(daytouse, F_DATA4, String.Format("{0}='{1}'", F_TYPE, T_USER));
			if (Words.IndexOf("null") > -1) Words = "0"; // sometimes they show null compute
			
			string sResult = String.Format("Minutes This Week: {0} \nWords This Week: {1}", nMinutes, Words);
			return sResult;
			
		}
		public static void ConvertToYom2013_FromYom2013 (TransactionsTable TransactionT)
		{

			string DELIM = "@@@@"; // could not be semi because semis are used in RTF text
			DataSet ds = EventTable._LoadTable ();
			DataTable table = ds.Tables [0];

			int Added =0;
			// go through each record and based on type add an appropriate transaction
			foreach (DataRow r in table.Rows) {

				DateTime date =(DateTime)r[EventTable.F_DATE];

				int type = (int)r[F_TYPE];
				// extract the GUID which we stored as {blah blah}; {GUID} in old table
				string fireline = "";
				if (type ==EventTable.T_USER) 
				{
					fireline = r[EventTable.F_DATA2].ToString();

				}
				else
				{
					fireline = r[EventTable.F_DATA].ToString();
				}
				string[] asparam = null;
				const string error = "Layout Not Found In Conversion";
					string LayoutGUID = error;
				if (fireline != "")
				{
					asparam = fireline.Split (new string[1]{DELIM},StringSplitOptions.None);
					if (asparam != null && asparam.Length >=2)
					{
						//fireline = asparam[0]; we never use this first line again
						LayoutGUID = asparam[1];
					}
				}

				if (error == LayoutGUID || "" == LayoutGUID)
				{
					// if we cannot find an appropriate Layout to assign this to, we will use (noT SYSTEM, A NEW NOTE)
					LayoutGUID = "example";
				}

				switch (type)
				{
				case T_ADDED:
					TransactionT.AddEvent(new TransactionNewLayout(date,LayoutGUID));  
					Added++;
					break;
				case T_DELETED: 
					TransactionT.AddEvent (new TransactionDeleteLayout(date, LayoutGUID));
					Added++;
					break;
				case T_RETIRED: 
					TransactionT.AddEvent(new TransactionRetired(date, LayoutGUID));
					Added++;
					break;

				case T_FINISHED: 
					TransactionT.AddEvent(new TransactionFinishedLayout(date, LayoutGUID));
					Added++;
					break;
				case T_NAGSTARTED: 
					TransactionT.AddEvent(new TransactionNagStarted(date, LayoutGUID));
					Added++;
					break;
				case T_NAGINTERRUPTED: 
					TransactionT.AddEvent(new TransactionNagInterrupted(date, LayoutGUID));
					Added++;
					break;
				case T_USER: 
					string note = r[EventTable.F_DATA].ToString();
					int  words = (int)r[EventTable.F_DATA4];
					int minutes = (int)r[EventTable.F_DATA3];
					TransactionT.AddEvent (new TransactionWorkLog(date, LayoutGUID, note, words, minutes, "Writing"));
					Added++;
					break;
				case 1001: 
					Added++;
					if (asparam.Length == 14)
					{
					//	string dataToAdd = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}", date, GuidOfLayout, GuidOfMarket, Priority, 
					//	                                 Expenses, Earned, dataReplied, notes, rights, draftversionused, replytype, replyfeedback, submissiontype);

						// we already have date. 

						// we override GUID
						LayoutGUID = asparam[1];
					
						if ("" == asparam[1])
						{
							LayoutGUID = "example";
						}

						DateTime DateSubbed;
						DateTime.TryParse (asparam[0], out DateSubbed);
					// new sub
						string MarketGUID=  asparam[2];
						int Priority = 0;
						int.TryParse (asparam[3].ToString (), out Priority);

						//NewMessage.Show ("confirm that money1 is expenses!");
						float Money1 = 0.0f;
						float.TryParse (asparam[4].ToString(), out  Money1);
					
						float Money2 = 0.0f;
						float.TryParse (asparam[5].ToString (), out Money2);



						DateTime DataReplied;
						DateTime.TryParse (asparam[6], out DataReplied);

						string Notes =  asparam[7];

						string Rights= asparam[8];
						string Version= asparam[9];
						string ReplyType= asparam[10];
					string ReplyFeedback=asparam[11]; 
					string SubmissionType=asparam[12];
						string MarketName = asparam[13];

						// for this we do not USE the date
						// we use the parsed date instead


						TransactionT.AddEvent(new TransactionSubmission(DateSubbed, LayoutGUID, MarketGUID,  Priority,
						                                                Money1,  Money2,  DataReplied,
						                                                Notes,  Rights,  Version,
						                                                ReplyType, ReplyFeedback,  SubmissionType, MarketName));
					}
					else
					{
						string details = "";
						foreach (string s in asparam)
						{
							details = details + Environment.NewLine + s;
						}
						NewMessage.Show (String.Format ("A submission did not have right length. Length was {0} should be {1}. Details: {2}", asparam.Length, 14, details));
					}
					break;
				case 1005: 
					Added++;
					if (asparam.Length == 14)
					{
						//	string dataToAdd = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}", date, GuidOfLayout, GuidOfMarket, Priority, 
						//	                                 Expenses, Earned, dataReplied, notes, rights, draftversionused, replytype, replyfeedback, submissiontype);
						
						// we already have date. 
						
						// we override GUID
						LayoutGUID = asparam[1];
						
						if ("" == asparam[1])
						{
							LayoutGUID = "example";
						}
						
						DateTime DateSubbed;
						DateTime.TryParse (asparam[0], out DateSubbed);
						// new sub
						string MarketGUID=  asparam[2];
						int Priority = 0;
						int.TryParse (asparam[3].ToString (), out Priority);
						
						float Money1 = 0.0f;
						float.TryParse (asparam[4].ToString(), out  Money1);
						
						float Money2 = 0.0f;
						float.TryParse (asparam[5].ToString (), out Money2);
						DateTime DataReplied;
						DateTime.TryParse (asparam[6], out DataReplied);
						
						string Notes =  asparam[7];
						
						string Rights= asparam[8];
						string Version= asparam[9];
						string ReplyType= asparam[10];
						string ReplyFeedback=asparam[11]; 
						string SubmissionType=asparam[12];
						string MarketName = asparam[13];
						// for this we do not USE the date
						// we use the parsed date instead
						
						
						TransactionT.AddEvent(new TransactionSubmissionDestination(DateSubbed, LayoutGUID, MarketGUID,  Priority,
						                                                Money1,  Money2,  DataReplied,
						                                                Notes,  Rights,  Version,
						                                                ReplyType, ReplyFeedback,  SubmissionType, MarketName));
					}
					else
					{
						string details = "";
						foreach (string s in asparam)
						{
							details = details + Environment.NewLine + s;
						}
						NewMessage.Show (String.Format ("A submission did not have right length. Length was {0} should be {1}. Details: {2}", asparam.Length, 14, details));
					}
					break;
				}

			}


			NewMessage.Show (String.Format("Added {0} entries", Added));
		}

		/// <summary>
		/// returns a plain text string of all of the day's stats
		/// 
		/// retrieved each time date set on Today panel
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetDatesStats(DateTime date)
		{
			DataSet ds = _LoadTable();
			string sOutput = "";
			
			if (ds != null)
			{
				// load a DataROwFIlter
				DataView dv = new DataView(ds.Tables[0]);
				string sFilter = "Date='" + (date) + "'";
				dv.RowFilter = sFilter;
				try
				{
					foreach (DataRow dr in dv.ToTable().Rows)
					{
						int nAction = (int)dr[F_TYPE];
						if (nAction != T_USER)
						{
							string sAction = "";
							switch (nAction)
							{
							case T_ADDED: sAction = String.Format("Added the page - {0} ({1} type)", dr[F_DATA].ToString(), dr[F_DATA2].ToString()); break;
							case T_DELETED: sAction = String.Format("Deleted the page - {0} ({1} type)", dr[F_DATA].ToString(), dr[F_DATA2].ToString()); break;
							case T_RETIRED: sAction = String.Format("Retired the page - {0} ({1} type)", dr[F_DATA].ToString(), dr[F_DATA2].ToString()); break;
							case T_FINISHED: sAction = String.Format("Finished the story - {0} ({1} type)", dr[F_DATA].ToString(), dr[F_DATA2].ToString()); break;
							case T_ADDSUB: sAction = String.Format("Submitted {0}  to {1} ", dr[F_DATA2].ToString(), dr[F_DATA].ToString()); break;
							case T_LINK: sAction = String.Format("Added a link on the page {0}", dr[F_DATA].ToString()); break;
							case T_REGISTER: sAction = String.Format("Registered Keeper"); break;
							case T_TENWINDOWS: sAction = String.Format("Multitasked! Had ten windows open!"); break;
							case T_WEB: sAction = String.Format("Visited the Keeper website"); break;
							case T_NAGSTARTED: sAction = "Distraction mode activated"; break;
							case T_NAGINTERRUPTED: sAction = "Distracted!"; break;
								
							}
							try
							{
								string sNewLine = sAction;
								sOutput = sOutput + sNewLine + " \r\n ";
							}
							catch (Exception)
							{
							}
						}
					}// foreach
				}
				catch (Exception ex)
				{
					NewMessage.Show(ex.ToString());
				}
				return sOutput;
			}
			else
				
				return "NO STATS MAN!";
		}
		
		
	}//eventtableclass
	
	public static class path
	{
		// Sep 2009 - rearchitetcturing this so it actually loads the Path variable from fMain
		// This old way was dumb
		private static string sPath = "";
		
		public static string DataDirectory
		{
			get { return sPath; }
			set { sPath = value; }
		}
		/*
        public static string DataDirectory
        {
            get
            {
                string sPath = class_Paths.MyDocuments() +"\\" + class_Paths.GetApplicationName() + "\\Data\\";

#if DEBUG
                sPath = Environment.CurrentDirectory + "\\Data\\";
                
#endif
                // when porting into incentive class I need to figure out the MyDocuments+Directory myself
                return sPath;
            }
        }
         */
	}
	
}
