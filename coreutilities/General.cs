// General.cs
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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace CoreUtilities
{
	public partial class General
	{

		/// <summary>
		/// Default parameters make it check for file existence
		/// 
		/// This does not work for Directories, you need to supply bIgnoreFileExists=true
		/// </summary>
		/// <param name="sFileName"></param>
		/// <param name="sArgs"></param>
		/// <returns></returns>
		public static Process OpenDocument(string sFileName, string sArgs)
		{
			return OpenDocument(sFileName, sArgs, false);
		}
		/// <summary>
		/// Open sFileName in its default viewer
		/// October 2009 - put a "does file exist" check in before attempting to load
		///           This broke URLS (added IsUrl)
		///           This also brok eDirectory opens, added an bIgnoreFileExists
		/// </summary>
		/// <param name="sFileName"></param>
		/// <returns></returns>
		public static Process OpenDocument(string sFileName, string sArgs, bool bIgnoreFileExists)
		{
			if (sFileName == null || sArgs == null)
			{
				throw new Exception("OpenDocument passed in null sFileName or sArgs");
			}
			
			try
			{
				if (File.Exists(sFileName) == true || IsUrl(sFileName) == true || bIgnoreFileExists == true)
				{
					System.Diagnostics.Process myBatchFile = new System.Diagnostics.Process();
					myBatchFile.StartInfo.FileName = sFileName;
					myBatchFile.StartInfo.Verb = "Open";
					myBatchFile.StartInfo.CreateNoWindow = true;
					//  myBatchFile.StartInfo.UseShellExecute = false;
					//myBatchFile.StartInfo.Arguments = sArgs;
					myBatchFile.Start();
					return myBatchFile;
				}
				else
				{
					lg.Instance.Line("General.OpenDocument", ProblemType.WARNING,"not found. Returning null. " + sFileName);
					return null;
				}
			}
			catch (Exception)
			{
				lg.Instance.Line("General.OpenDocument", ProblemType.WARNING,"not found. Returning null. " + sFileName);
				return null;
			}
			//myBatchFile.WaitForExit();
		}
		/// <summary>
		/// returns true if this is an url. Presence of http
		/// </summary>
		/// <param name="sUrl"></param>
		/// <returns></returns>
		public static bool IsUrl(string sUrl)
		{
			if (sUrl.IndexOf("http") > -1)
			{
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// had to add null check because sometimes the selection font would come
		/// in null
		/// </summary>
		/// <param name="rich"></param>
		/// <param name="fs"></param>
		public static void FormatRichText(RichTextBox rich, FontStyle fs)
		{
			if (rich != null)
			{
				if (rich.SelectionFont != null)
				{
					rich.SelectionFont = new Font(rich.SelectionFont,
					                              rich.SelectionFont.Style ^ fs);
				}
			}
			
		}
		/// <summary>
		/// takes the specified string and converts it to the font
		/// if blank will default to Times 12
		/// </summary>
		/// <param name="sString"></param>
		/// <returns></returns>
		static public Font StringToFont(string sString)
		{
			if (sString == null || sString == "")
			{
				throw new Exception("StringToFont sString not defined");
			}
			FontConverter fc = new FontConverter();
			string sFontToUse = sString;
			if (sFontToUse == null || sFontToUse == "")
			{
				return new Font("Times New Roman", 12);
			}
			return (Font)fc.ConvertFromString(sFontToUse);
			
		}
		/// <summary>
		/// returns sSource = enough spaces to make final string nLenght (nLength-Length.Source)
		/// For formating text reports
		/// </summary>
		/// <param name="sSource"></param>
		/// <param name="nLength"></param>
		/// <returns></returns>
		public static string properspaces(string sSource, int nLength)
		{
			int nSpaces = nLength - sSource.Length;
			for (int i = 1; i <= nSpaces; i++)
			{
				sSource = sSource + " ";
			}
			return sSource;
			
		}
		/// <summary>
		/// grabs the substring between two pairs such as 
		/// 
		/// string sSource = "Alias: Frank, Mary, Jane \\par"
		/// 
		/// SubStringBetween(sSource, "Alias: ", "\\par")
		/// returns Frank, Mary, Jane
		/// </summary>
		/// <param name="sSource"></param>
		/// <param name="sStart"></param>
		/// <param name="sEnd"></param>
		/// <returns></returns>
		static public string SubStringBetween(string sSource, string sStart, string sEnd)
		{
			
			string sPriority = "";
			try
			{
				int nPriorityPos = sSource.IndexOf(sStart);
				if (nPriorityPos > -1)
				{
					
					int nLF = sSource.IndexOf(sEnd, nPriorityPos);
					if (nLF > -1)
					{
						nPriorityPos = nPriorityPos + sStart.Length;
						
						sPriority = sSource.Substring(nPriorityPos, nLF - nPriorityPos);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString() + "\n " + sSource);
			}
			return sPriority.Trim();
		}


		[DllImport("user32.dll")]
		static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
		
		[StructLayout(LayoutKind.Sequential)]
		public struct LASTINPUTINFO
		{
			public uint cbSize;
			public uint dwTime;
		}
		/// <summary>
		/// How many seconds since last user input
		/// Source: http://blog.abodit.com/2011/08/stop-writing-rude-software-use-lastinputinfo-instead/
		/// </summary>
		public static double SecondsSinceLastInput()
		{
			LASTINPUTINFO lastInPut = new LASTINPUTINFO();
			lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
			GetLastInputInfo(ref lastInPut);
			
			uint idle = (uint)Environment.TickCount - lastInPut.dwTime;
			return idle/1000.0;
		}
		/// <summary>
		/// Using reflection I return the tag of an object. This was written to make my localization
		/// routine run a lot faster (in the old versin, not used with YOM2013, though this routine is.
		/// <param name="obj"> the object to get the field value from</param>
		/// <param name="sField">the field to test, i.e., "Tag"</param>
		/// </summary>
		/// <returns> blank if no value, or no valid value for this filed</returns>
		static public string GetFieldValue(string sField, object obj)
		{
			string sValue = "";
			if (sField == null || obj == null)
			{
				throw new Exception("GetFieldValue passed in null sField or obj");
			}
			
			try
			{
				
				System.Type senderType = obj.GetType();
				System.Reflection.PropertyInfo property = senderType.GetProperty(sField);
				if (property != null)
				{
					sValue = property.GetValue(obj, null).ToString();
				}
				else
				{
					lg.Instance.Line("General.GetFieldValue",CoreUtilities.ProblemType.MESSAGE, "property for was null " +sField);
					
				}
			}
			catch (Exception)
			{
				sValue = "";
				lg.Instance.Line("General.GetFieldValue", ProblemType.MESSAGE, "GetFieldValue Exception on  passing blank string out " + sField);
			}
			return sValue;
		}

	}
}

