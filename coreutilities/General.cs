using System;
using System.Diagnostics;
using System.IO;

namespace CoreUtilities
{
	public class General
	{
		public General ()
		{
		}

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
	}
}

