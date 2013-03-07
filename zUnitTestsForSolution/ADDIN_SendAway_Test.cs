using System;
using NUnit.Framework;
using CoreUtilities;
using SendTextAway;
using System.IO;
using System.Collections.Generic;
namespace Testing
{
	[TestFixture]
	public class ADDIN_SendAway_Test
	{
		string PathToSendAwayUnitTestingFIles = Constants.BLANK;
		string PathToSendTextAwayFiles = Constants.BLANK;
		string PathToOutput =Constants.BLANK;
		string PathToProper =Constants.BLANK;
		public ADDIN_SendAway_Test ()
		{
		}

		// each test should hapepn in its own otuput directory
		public ControlFile Setup (string uniquetestdirectory)
		{
			// deletes contents of the output folder
			string path = Environment.CurrentDirectory;
			 PathToSendAwayUnitTestingFIles = Path.Combine (path, "SendAwayUnitTestingFiles");
			 PathToSendTextAwayFiles = _TestSingleTon.PathToSendAwayEPUBTemplates;
			 PathToOutput = Path.Combine (PathToSendAwayUnitTestingFIles, "output");

			string code = uniquetestdirectory;
			PathToOutput = Path.Combine (PathToOutput,code);
			 PathToProper = Path.Combine (PathToSendAwayUnitTestingFIles, "proper");

			if (Directory.Exists (PathToOutput))
			    {
			Directory.Delete (PathToOutput, true);
			}

			Directory.CreateDirectory (PathToOutput);

			if (Directory.Exists (PathToSendAwayUnitTestingFIles) == false || Directory.Exists (PathToSendTextAwayFiles) == false || Directory.Exists (PathToOutput) == false) {
				throw new Exception ("not all paths defined");
			}
			//tests for existence of needed debug folders

			// Sets up a proper epub control file
			ControlFile result =  ControlFile.Default;
			result.ConverterType = ControlFile.convertertype.epub;
			result.OutputDirectory = PathToOutput;
			result.Zipper =_TestSingleTon.Zipper;
			result.TemplateDirectory = PathToSendTextAwayFiles;
			
			return result;

		}


		private List<string> ConvertFileToLIstOfStrings(string file)
		{
			List<string> result = new List<string>();
			string line = "";
			System.IO.StreamReader fileR = 				new System.IO.StreamReader(file);

				while((line = fileR.ReadLine()) != null)
				{
				//String line = fileR.ReadToEnd();
				result.Add (line);
			}

			return result;
		}

		public int CompareTwoFiles (string file_1, string file_2)
		{
			int differences = 0;


			string file1Copy =  file_1; // we don't copy originalfile_1+"original.txt";
			string file2Copy = file_2 +"copy.txt";

			//File.Copy (file_1, file1Copy);
			File.Copy (file_2,file2Copy);

			List<string> FILE1 = ConvertFileToLIstOfStrings (file1Copy);
			List<string> FILE2 = ConvertFileToLIstOfStrings (file2Copy);

			differences = FILE1.Count - FILE2.Count;
			int linescounted = 0;
			if (differences == 0) {
				// now do line by line compare
				for (int i = 0; i < FILE1.Count; i++)
				{
					linescounted++;
					if (FILE1[i] != FILE2[i])
					{
						differences++;
					}
				}
			}
			_w.output("Lines read " + linescounted.ToString());
			return differences;
		}

		// keep each test small so its easier to track down where the failur ehappened

		[Test]
		public void TestHeadings()
		{
			PerformTest("headings.txt");
		}

		[Test]
		public void TestBullets()
		{
			PerformTest("bullet.txt");
		}
		[Test]
		public void TestBrokenBullets()
		{
			// We accept this will break  but test to make sure it is never invisibly fixed. (see FAQ:  http://yourothermind.com/yourothermind-2013/using-yourothermind-2013/addin_sendtextaway/)
			PerformTest("bulletbroken.txt");
		}

		[Test]
		public void TestNumberBullets()
		{
			PerformTest("numberbullet.txt");
		}

		[Test]
		public void SectionFormat()
		{
			// this is <past> </past> across multiple lines
		}

		void PerformTest (string simpletxt)
		{
			ControlFile Controller = Setup (simpletxt);
			string Incoming = simpletxt;
			string FileToTest = Path.Combine (PathToSendAwayUnitTestingFIles, Incoming);
			
			sendePub2 SendAwayIt = new sendePub2 ();
			SendAwayIt.SuppressMessages = true;
			SendAwayIt.WriteText (FileToTest, Controller, -1);
			
			
			string FileOutput = Path.Combine (PathToOutput, sendePub2.GetDateDirectory);
			FileOutput = Path.Combine (FileOutput, "oebps");
			FileOutput = Path.Combine (FileOutput, "preface.xhtml");
			
			Assert.True (File.Exists (FileOutput), "NOt found " + FileOutput);
			
			// now test the file for identicality
			string FileToCompareItTo = Path.Combine (PathToProper, Incoming);
			if (File.Exists (FileToCompareItTo) == false) {
				throw new Exception ("The file does not exist : " + FileToCompareItTo);
			}
			int differences = CompareTwoFiles(FileToCompareItTo, FileOutput);
			
			Assert.AreEqual(0, differences, "Differences Found " + differences);
		}



		[Test]
		/// <summary>
		/// Simples the test.
		/// </summary>
		public void SimpleTest ()
		{
			PerformTest("simple.txt");

		}
	}
}

