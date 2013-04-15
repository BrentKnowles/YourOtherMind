// ADDIN_SendAway_Test.cs
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
		public ControlFile Setup (string uniquetestdirectory, ControlFile overrideControl)
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



			result.ListOfTags=new string[1]{"game|''"};
			result.ConverterType = ControlFile.convertertype.epub;
			result.MultiLineFormats= new string[1]{"past"};
			result.MultiLineFormatsValues = new string[1]{"blockquote2"};
			result.Zipper =_TestSingleTon.Zipper;

			if (overrideControl != null) result = overrideControl;
			result.OutputDirectory = PathToOutput;
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
		public void Fancy()
		{
			ControlFile result =  ControlFile.Default;
			
			
			
			result.ListOfTags=new string[1]{"game|''"};
			result.ConverterType = ControlFile.convertertype.epub;
			result.MultiLineFormats= new string[1]{"past"};
			result.MultiLineFormatsValues = new string[1]{"blockquote2"};
			result.Zipper =_TestSingleTon.Zipper;
			result.FancyCharacters = true;
			PerformTest("fancycharacters.txt", result);
		}
		[Test]
		public void Bug001()
		{
			

			PerformTest("bug001.txt");
		}

		[Test]
		public void Emdash()
		{
			ControlFile result =  ControlFile.Default;
			
			
			
			result.ListOfTags=new string[1]{"game|''"};
			result.ConverterType = ControlFile.convertertype.epub;
			result.MultiLineFormats= new string[1]{"past"};
			result.MultiLineFormatsValues = new string[1]{"blockquote2"};
			result.Zipper =_TestSingleTon.Zipper;
			result.FancyCharacters = false;
			result.ConvertToEmDash = true;
			PerformTest("emdash.txt", result);
		}
		[Test]
		public void Bug002()
		{
			// could not actually replicate the bug but figured I'd leave this in as a baseline.
			PerformTest("bug002.txt");
		}

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
		public void TestInlineFormat()
		{
			// <game</game> on an individual line
			PerformTest("inlineformat.txt");
		}
		[Test]
		public void TestStrikeAndSuper()
		{
			PerformTest("strikeandsuper.txt");
		}
		[Test]
		public void TestAnchorLink ()
		{
			PerformTest("anchor.txt");
		}

		[Test]
		public void TestVariable()
		{
			PerformTest ("variable.txt");
		}

		[Test]
		public void SectionFormat()
		{
			// this is <past> </past> across multiple lines
			PerformTest ("sectionformat.txt");
		}


		void PerformTest (string simpletxt)
		{
			PerformTest (simpletxt, null);
		}
		void PerformTest (string simpletxt, ControlFile overrideControl)
		{
			ControlFile Controller = Setup (simpletxt, overrideControl);
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

