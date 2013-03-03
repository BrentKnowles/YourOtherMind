using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
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
	}
}

