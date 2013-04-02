using System;
using System.Data;
using System.Text.RegularExpressions;

namespace CoreUtilities
{
	public class TextUtils
	{
		public TextUtils ()
		{
		}

		public delegate string MyDelegate(Match m);
		public static MyDelegate fieldValueDelegate;
		//string fieldPattern = @"\[(?<Field>[A-Z, 1, 2, 3]+)\]";
		private static string FieldPattern = @"\[(?<Field>[A-Z,a-z]+)\]";
		private static DataRow TheRow;
		private static object oObjectBeingUsed, oObjectBeingUsed2, oObjectBeingUsed3;
		

		
		/// <summary>
		/// Returns the "value" for the current row using private member TheRow
		/// which was set in the Generate routine
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		private static string GetFieldValue(Match m)
		{
			return fieldValueDelegate(m);
			
		}
		
		/// <summary>
		/// A "replacement" for this can be created to use different things as datasources
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		private static string DefaultFieldValue(Match m)
		{
			string fieldName = m.Groups["Field"].Value;
			string sValue = TheRow[fieldName].ToString();
			return sValue;
		}
		
		/// <summary>
		/// Will parse the sSnippet template using TheRow as data. TheRow was set in Generate
		/// </summary>
		/// <param name="sSnippet">This is the file to use for the template</param>
		/// <returns></returns>
		private static string RegRow(string sText)
		{
			
			/*  string sReadSnippet = "";

            // Must load in the correct snippet. If the requested snippet is not found then fire an user message
            string sFile = sSnippet;
            if (File.Exists(sFile) == false)
            {
                throw new Exception("The file " + sFile + " does not exist");

            }
            StreamReader objReader = new StreamReader(sFile);
            string sLine = "";

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    sReadSnippet = sReadSnippet + sLine + "\n";
                }
            }
            objReader.Close();
*/
			
			
			return  Regex.Replace(sText, FieldPattern, new MatchEvaluator(GetFieldValue));
			
			
			
		}
		/// <summary>
		/// Will parse S with the data in dr
		/// so "TheRow" needs to be set beforehand
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private static string RegStr(string s)
		{
			return Regex.Replace(s, FieldPattern, new MatchEvaluator(GetFieldValue));
		}
		
		/// <summary>
		/// Be wary. If sText is "richText" it cannot have formatting within the []
		/// </summary>
		/// <param name="sText"></param>
		/// <param name="dr"></param>
		/// <returns></returns>
		public static string RegExternalStr(string sText, DataRow dr)
		{
			fieldValueDelegate = new MyDelegate(DefaultFieldValue);
			TheRow = dr;
			return RegRow(sText);
		}
		
		/// <summary>
		/// We try to find the value for this in the various objects being used
		/// </summary>
		/// <returns>
		/// The field value.
		/// </returns>
		/// <param name='m'>
		/// M.
		/// </param>
		private static string ObjectFieldValue(Match m)
		{
			string fieldName = m.Groups["Field"].Value;
			string sValue = General.GetFieldValue(fieldName, oObjectBeingUsed);// TheRow[fieldName].ToString();
			if ((sValue == null || sValue == Constants.BLANK) && (oObjectBeingUsed2 != null))
			{
				sValue = General.GetFieldValue(fieldName, oObjectBeingUsed2);
			}
			if ((sValue == null || sValue == Constants.BLANK) && (oObjectBeingUsed3 != null))
			{
				sValue = General.GetFieldValue(fieldName, oObjectBeingUsed3);
			}
			return sValue;
		}
		
		public static string RegExternalStr_ObjectLookup(string sText, object o1, object o2, object o3)
		{
			fieldValueDelegate = new MyDelegate(ObjectFieldValue);
			oObjectBeingUsed = o1;
			oObjectBeingUsed2 = o2;
			oObjectBeingUsed3 = o3;
			return RegRow(sText);
		}
	}
}

