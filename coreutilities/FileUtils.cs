using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Reflection;
namespace CoreUtilities
{
	public static class FileUtils
	{
		/// <summary>
		/// Serialize any serializable C# object into an xml file.
		/// </summary>
		/// <param name="oObject">The object to serialize</param>
		/// <param name="backup">backup directory</param>
		/// <param name="sName">Path and name of the output file, i.e. myobject.xml</param>
		
		public static void Serialize(object oObject, string sName, string backup)
		{
			bool bDidBackup = false;
			
			
			if (oObject == null || sName == null)
			{
				throw new Exception("Serialize null passed in for oObject or sName");
			}
			
			// copy backup to safe place 
			if (null != backup && "" != backup && File.Exists(sName))
			{
				try
				{
					FileInfo f = new FileInfo(sName);
					backup = Path.Combine(backup,"backup");
					File.Copy(sName, backup , true);
					f = null;
					bDidBackup = true;
				}
				catch (Exception)
				{
					
				}
			}
			
			try
			{
				System.Type t = oObject.GetType();
				if (t != null)
				{
					XmlSerializer s = new XmlSerializer(t);
					TextWriter w = new StreamWriter(@sName);
					s.Serialize(w, oObject);
					w.Close();
					t = null;
					w = null;
					s = null;
				}
				
			}
			catch (Exception ex)
			{
				if (true == bDidBackup)
				{
					// copy the backed up file
					if (File.Exists(backup) == true)
					{
						File.Copy(backup, sName, true);
					}
					
				}
				// February 2010 
				// I lost part of Zombieworld and I figured maybe it might make sense to copy a backup 
				// of the filee being saved AND RESTORE it if there is an exception
				CoreUtilities.NewMessage.Show(sName + " did not save correctly. You should copy your data (copy/paste) shut down and retry. Your last saved version will still be valid." + ex.ToString());
			}
			
		}
		/// <summary>
		/// Deserialize any object from xml. 
		/// </summary>
		/// <param name="sName">File name</param>
		/// <param name="t">Object type (i.e. GetType(o))</param>
		/// <returns>Deserialized object. Cast back to type before using</returns>
		public static object DeSerialize(string sName, System.Type t)
		{
			FileInfo f = new FileInfo(sName);
			
			/*February 2009
             * I had originally prevent any non xml files from entering here
             * but that was just a bandaid.
             * 
             * I needed to pass bst (brainstorm) files into this and to do that I needed this to be open
             */
			
			
			// if (f.Extension.ToLower() == ".xml")
			{
				
				
				TextReader r = null;
				try
				{
					XmlSerializer s = new XmlSerializer(t);
					object oRet;
					r = new StreamReader(@sName);
					oRet = s.Deserialize(r);
					r.Close();
					
					return oRet;
				}
				catch (Exception ex)
				{
					if (r != null)
						r.Close();
					// MessgeBox.Show(sName + " " + ex.ToString());
					throw (new Exception(sName.ToUpper() + " " + ex.ToString()));
				}
			}
			/*   else
            {
                MessgeBox.Show(String.Format("For some reason an invalid file ({0}) was passed into the xml deserializer. Skipping.", sName));
                return null;
            }*/
			// return null;
		}
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

		/// <summary>
		/// pulls resource of resourceid
		/// 
		/// (Copies said file from embedded resources into the diretory said
		/// called by PrepareDictionary
		/// </summary>
		/// <param name="sResourceID"></param>
		public static void PreparePullResource(Assembly _assembly, string sResourceID, string sFile)
		{
			if (sResourceID == null || sFile == null)
			{
				throw new Exception("PreparePullResource - no file or resource specified");
			}
			StreamReader _imageStream;
			_imageStream = new StreamReader(_assembly.GetManifestResourceStream(sResourceID));
			if (_imageStream != null)
			{
				StreamWriter sw = new StreamWriter(sFile);
				string s = _imageStream.ReadLine();
				while (s != null)
				{
					sw.WriteLine(s);
					s = _imageStream.ReadLine();
				}
				
				_imageStream.Close();
				sw.Close();
				sw = null;
				
				_imageStream = null;
				
			}
		}
	
	}
}

