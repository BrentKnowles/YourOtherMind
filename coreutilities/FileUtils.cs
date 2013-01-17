using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CoreUtilities
{
	public static class FileUtils
	{

		/// <summary>
		/// Doeses the this file have errors.
		/// 
		/// If the file is blank will return true.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if this file hae errors was doesed, <c>false</c> otherwise.
		/// </returns>
		/// <param name='file'>
		/// File.
		/// </param>
		public static bool DoesThisFileHaveErrors (string file)
		{
			bool foundRealText = false;
			//System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9]*$");
			using (StreamReader sr = new StreamReader(file))
			{
				String line = sr.ReadToEnd();
				//Console.WriteLine (line);
				//if (string.IsNullOrWhiteSpace(line) == false)
			//	if (r.IsMatch(line))
				if (line.IndexOfAny(new char[5]{'a','i','o','u','e'}) > -1)
				{
					//Console.WriteLine ("found text");
					foundRealText = true;

				}

			}
			return !foundRealText;
		}

		/// <summary>
		/// Finds the file, starting at rootDirectory.
		/// 
		/// Returns the full path to the found file or BLANK, if not found
		/// </summary>
		/// <returns>
		/// The file.
		/// </returns>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='rootDirectory'>
		/// Root directory.
		/// </param>
		public static string FindFile (string fileName, string rootDirectory)
		{
			string[] results = Directory.GetFiles (rootDirectory, fileName, SearchOption.AllDirectories);
			lg.Instance.Line("FileUtils->FindFile", ProblemType.MESSAGE, String.Format ("Searching for file {0} in Directory {1}", fileName, rootDirectory));
			string returnresult = Constants.BLANK;
			if (results.Length > 0) {
				returnresult = results[0];
			}
			return returnresult;
		}

		/// <summary>
		/// Checks for file error.
		/// 
		/// Several times I have encounterd a harddrive failure (maybe) that all files that are saved after the point
		/// in which the invisible error happens.
		/// 
		/// This affected the previous version of YOM (.NET 2.0) as well as PDF files that were open in Adobe. All the files
		/// would have the same filelength as previous but every character was replaced with a blank.
		/// 
		/// Very bizarre. This is my attempt to create a test that is intended to be ran when an Application is closing.
		/// <returns>If it encounters the problem it returns TRUE  meaning a file error has occurred. At this point
		/// it is advised to terminate the application without trying to save the files.
		/// </returns>
		/// 
		/// 
		/// </summary>
		public static bool CheckForFileError ()
		{
			string oldfile = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(oldfile)) {
				sw.Write ("This is a test file from coreutilities in YOM to make sure harddrive failure is not happening.");
			}
		
			return DoesThisFileHaveErrors(oldfile);
		}





		public static Icon GetIcon (string identifier)
		{
			return new Icon (System.Reflection.Assembly.GetCallingAssembly ().GetManifestResourceStream (identifier));
		}

		public static Bitmap GetImage_ForDLL (string identifier)
		{
			return new Bitmap (System.Reflection.Assembly.GetCallingAssembly ().GetManifestResourceStream (identifier));
		}
		public static Bitmap GetImage_ForEXE (string identifier)
		{
			return new Bitmap (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (identifier));
		}

		/// <summary>
		/// Defaults to 7 characters
		/// </summary>
		/// <param name="sDirectory"></param>
		/// <param name="sPrefix"></param>
		/// <param name="sExtension"></param>
		/// <returns></returns>
		public static string MakeUniqueFileNameForDirectory(string sDirectory, string sPrefix, string sExtension)
		{
			return MakeUniqueFileNameForDirectory(sDirectory, sPrefix, sExtension, 7, 0);
		}
		
		/// <summary>
		/// Creates a unique filename. Used for adding pages and submission files
		/// </summary>
		/// <param name="sDirectory"></param>
		/// <param name="sPrefix"></param>
		/// <param name="sExtension"></param>
		/// <param name="nCharacters">how many characters in the name. Throws exception if 0</param>
		/// <param name="nCount2">minimum number, for examplke Keeper can't create pages under 100. Throws exception if 0</param>
		/// <returns>null if unable to build the filename</returns>
		public static string MakeUniqueFileNameForDirectory(string sDirectory, string sPrefix, string sExtension, int nCharacters, int nCount2)
		{
			if (sDirectory == null || sPrefix == null || sExtension == null)
			{
				
				return null;
			}
			if (nCount2 < 0 || nCharacters == 0)
			{
				throw new ArgumentException("MakeUniqueFilenameForDirectory Count < 0 or nCharacters == 0");
			}
			
			string sFileName = "";
			int nCount = nCount2;
			// will loop to guarantee the uniqueness of the file
			do
			{
				// count number of files and add a Number
				string[] sFiles = Directory.GetFiles(sDirectory);
				sFileName = (sFiles.Length + 1 + nCount).ToString();
				// add zeroes
				while (sFileName.Length < nCharacters)
				{
					sFileName = "0" + sFileName;
					
				}
				
				sFileName = sPrefix + sFileName + "." + sExtension;
				
				nCount++;
			} while (File.Exists(sDirectory + "\\" + sFileName) == true);
			return sFileName;
			
		}
	
	}
}

