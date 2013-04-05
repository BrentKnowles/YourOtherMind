// newGenericDate.cs
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
using CoreUtilities;

namespace Timeline
{
	public struct newGenericDate
	{
		public static bool WarnedAlready;
		
		public int Year;
		public int Month;
		public int Day;
		public newGenericDate(int nYear, int nMonth, int nDay)
		{
		
			Year = nYear;
			Month = nMonth;
			Day = nDay;
		}
		public newGenericDate(DateTime date)
		{
		
			Year = date.Year;
			Month = date.Month;
			Day = date.Day;
		}

		public DateTime AsDate
		{
			get {
				DateTime DateToReturn = DateTime.Now;
				try
				{
					DateToReturn = new DateTime(Year, Month, Day);
				}
				catch (Exception)
				{
					DateToReturn = DateTime.Now;
					if (false == WarnedAlready)
					{
						NewMessage.Show (Loc.Instance.GetStringFmt ("The date with day {0} and month {0} and year {0} was invalid. Setting to today.", Day, Month, Year));
						WarnedAlready = true;
					}
				}
				return DateToReturn;}
		}
		public string AsString
		{
			get { return String.Format("{0}/{1}/{2}", Day.ToString(), Month.ToString(), Year.ToString()); }
		}
		
		/// <summary>
		/// creates it from a datetime object, but after moment of creation
		/// </summary>
		/// <param name="date"></param>
		public void FromDate(DateTime date)
		{
			Year = date.Year;
			Month = date.Month;
			Day = date.Day;
		}
		/// <summary>
		/// makes this struct populated with info from the date string loaded in
		/// expecting a format of
		///  DAY/MONTH/YEAR like 20/9/2008
		/// </summary>
		/// <param name="sDateString"></param>
		public void FromString(string sDateString)
		{
			try
			{
				if (sDateString != null && sDateString != "")
				{
					int nIdx = sDateString.IndexOf("/");
					if (nIdx > -1)
					{
						// found a line, grab day
						string sDay= sDateString.Substring(0, nIdx);
						sDay = sDay.Trim();
						Int32.TryParse(sDay, out Day);
						sDateString = sDateString.Substring(nIdx+1, sDateString.Length - nIdx-1);
					}
					nIdx = sDateString.IndexOf("/");
					if (nIdx > -1)
					{
						// found a line, grab month
						string sMonth = sDateString.Substring(0, nIdx);
						sMonth = sMonth.Trim();
						Int32.TryParse(sMonth, out Month);
						sDateString = sDateString.Substring(nIdx + 1, sDateString.Length - nIdx - 1);
					}
					// there is no / after date
					nIdx = sDateString.IndexOf(" ");
					Year = -1; // set this to test against it
					if (nIdx > -1)
					{
						// found a line, grab month
						string sYear = sDateString.Substring(0, nIdx);
						sYear = sYear.Trim();
						Int32.TryParse(sYear, out Year);
						sDateString = sDateString.Substring(nIdx + 1, sDateString.Length - nIdx - 1);
					}
					// if we did not find a year, in the case of Newer daes that just go day/month/year with not 
					// time stuff at the end we simply have to take the reaminder of the date string
					if (Year == -1)
					{
						string sYear = sDateString;
						sYear = sYear.Trim();
						Int32.TryParse(sYear, out Year);
						//Year = Int32.Parse(sYear);
					}
				}
			}
			catch (Exception ex)
			{
				// October 2012 - supressing this cause it is really annoying
				lg.Instance.Line("newGenericDate->FromString", ProblemType.EXCEPTION,"NotePanelTimeline" + "timeline date did not work " + ex.ToString());
				// NewMessage.Show(ex.ToString());
			}
		}

		/// <summary>
		/// October 19 2008
		/// - changed this to a generic date, so its always a string
		/// and a generic date class that loads it expecting day/month/year
		/// and THEN it converts it to a DateTime
		/// 
		/// 
		/// October 17
		/// 
		/// I was getting crashes on other machines with the timeline.
		/// I assumed that this was because Machine A used DateFormat A
		/// Machine B used DateFormat B
		/// And parsing didn't work (the core DateTime.Parse) 
		/// 
		/// I don't know if that  is the issue but will try it.
		/// 
		/// Basically we take the datestring and somehow convert it?
		/// </summary>
		/// <param name="sDateString"></param>
		/// <returns></returns>
		static public DateTime SafeDateParse(string sDateString)
		{
			DateTime newDate = DateTime.Now;
			newGenericDate genericDate = new newGenericDate();
			genericDate.FromString(sDateString);
			newDate = genericDate.AsDate;
			
			
			return newDate;
			/*
            try
            {
                newDate = DateTime.Parse(sDateString);
            }
            catch (Exception )
            {
                // if fails we then try to do an exact parse
                try
                { 
                    newDate = DateTime.ParseExact(sDateString, "dd-MM-yyyy HH:mm:ss tt", null);
                }
                catch (Exception ex)
                {
                    Messag eBox.Show(String.Format("SafeDateParse error for {0} with this exception {1}", sDateString, ex.ToString()));
                }
            }
             */
			
			
			
		}
		
	}
}

