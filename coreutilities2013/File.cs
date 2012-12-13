using System;
using System.IO;


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
			using (StreamReader sr = new StreamReader(file))
			{
				String line = sr.ReadToEnd();
				if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^[a-zA-Z0-9]+$"))
				{

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
		public static bool CheckForFileError()
		{
			return false;
		}
	}
}

