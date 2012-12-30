using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace CoreUtilities
{
	public static class File
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

		public static Bitmap GetImage_ForDLL (string identifier)
		{
			return new Bitmap (System.Reflection.Assembly.GetCallingAssembly ().GetManifestResourceStream (identifier));
		}
		public static Bitmap GetImage_ForEXE (string identifier)
		{
			return new Bitmap (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (identifier));
		}
	}
}

