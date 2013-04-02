using System;
using NUnit.Framework;
using CoreUtilities;
namespace Testing
{
	[TestFixture]
	public class CoreUtilities_File
	{
		public CoreUtilities_File ()
		{
		}

		[Test]
		public void TestForBlankFile()
		{
			Assert.True (CoreUtilities.FileUtils.DoesThisFileHaveErrors(_TestSingleTon.BlankFileTest1));
		}

		[Test]
		public void TestForBlankFileThatIsNotBlank()
		{
			Assert.False (CoreUtilities.FileUtils.DoesThisFileHaveErrors(_TestSingleTon.BlankFileTest2));
		}

		[Test]
		public void TestForHdriveFailure()
		{
			Assert.False (CoreUtilities.FileUtils.CheckForFileError());
		}


		[Test]
		public void TestValidFilename ()
		{
			string invalidfilename = @"hello?there.txt";
			string converted = FileUtils.MakeValidFilename(invalidfilename);
			Assert.AreEqual (@"hello_there.txt",converted);
		}

		[Test]
		[Ignore]
		public void TestEveryThingInCoreUtilities ()
		{
			// see commented out stuff, below
			Assert.True (false);
		}
	}
}

/*
 * using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CoreUtilities;
using namespaceLogs;
using System.IO;
using System.Windows.Forms;


namespace TesterAppFrame2010
{
    ///////////////////////////////////////////////////////////////
    //
    // TEST CASES  - CoreUtilities
    //
    ///////////////////////////////////////////////////////////////
    [TestFixture]
    public class testCoreUtilities
    {
        // some routine need to try to access a drive that does not exist and then fail appropriately
        const string INVALID_DRIVE = "w:\\";

        [SetUp]
        public void Init()
        {
            Logs.NewLog(Logs.DEFAULT_LOUDNESS);
        }

        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullTextWidthForIMage()
        {
            General.ReformatStringForWidth(null, 10);
        }

        [Test]
        public static void testFixLink()
        {
            Assert.True(General.FixLink("yourothermind.com") == "http://www.yourothermind.com", "yourothermind.com");
            Assert.True(General.FixLink("www.yourothermind.com") == "http://www.yourothermind.com", "yourothermind.com");
            Assert.True(General.FixLink("http://yourothermind.com") == "http://www.yourothermind.com", "yourothermind.com");
            Assert.True(General.FixLink("http://www.yourothermind.com") == "http://www.yourothermind.com", "yourothermind.com");
        }

        [Test]
        public static void testNotAGraphicFile()
        {
            Assert.False(General.IsGraphicFile("micky.txt"));
            Assert.False(General.IsGraphicFile("micky.tga"));
        }
        [Test]
        public static void testGraphicFiles()
        {
            Assert.True(General.IsGraphicFile("micky.jpg"));
            Assert.True(General.IsGraphicFile("micky.JPg"));
            Assert.True(General.IsGraphicFile("micky.JPeg"));
            Assert.True(General.IsGraphicFile("micky.bmp"));
            Assert.True(General.IsGraphicFile("micky.gif"));
            Assert.True(General.IsGraphicFile("micky.png"));

        }
        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullGraphicsFile()
        {
            General.IsGraphicFile(null);
        }
        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullGetFieldValue()
        {
            General.GetFieldValue(null, null);
        }

        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullSetFieldValue()
        {
            General.SetFieldValue(null, null, null);
        }
        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullCreateMissingDirectories()
        {
            General.CreateMissingDirectories(null);
        }

        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullOpenDocument()
        {
            General.OpenDocument(null, null);
        }
        [Test]
        public static void testNotepadOpenDocument()
        {
            System.Diagnostics.Process process = General.OpenDocument("notepad.exe", "", true);
            Assert.IsFalse(process == null, "notepad not found!?");
            if (process != null)
            {
                Logs.Line("");
                Logs.Line(String.Format("process {0} closing", process.ProcessName));
                process.CloseMainWindow();
                process.Dispose();

                process = null;
            }
        }
        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullStrToInt()
        {
            General.StrToInt(null);
        }

        [Test]
        public static void testInvalidDataStrToInt()
        {
            Assert.IsTrue(General.StrToInt("") == 0);
            Assert.IsTrue(General.StrToInt("****") == 0);
            Assert.IsTrue(General.StrToInt(".") == 0);

        }
        [Test]
        public static void testValidDataStrToInt()
        {
            Assert.IsTrue(General.StrToInt("77") == 77);
            Assert.IsTrue(General.StrToInt("-1") == -1);
            Assert.IsTrue(General.StrToInt("200900") == 200900);


        }

        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullStrToDouble()
        {
            General.StrToDouble(null);
        }

        [Test]
        public static void testInvalidDataStrToDouble()
        {
            Assert.IsTrue(General.StrToDouble("") == 0);
            Assert.IsTrue(General.StrToDouble("****") == 0);
            Assert.IsTrue(General.StrToDouble(".") == 0);

        }
        [Test]
        public static void testValidDataStrToDouble()
        {
            Assert.IsTrue(General.StrToDouble("77") == 77);
            Assert.IsTrue(General.StrToDouble("-1") == -1);
            Assert.IsTrue(General.StrToDouble("200900") == 200900);


        }
        [Test]
        public static void testValidStr()
        {
            Assert.IsTrue(General.ValidStr(null) == false);
            Assert.IsTrue(General.ValidStr("") == false);
            Assert.IsTrue(General.ValidStr(" ") == true);
            Assert.IsTrue(General.ValidStr("franke") == true);
        }


        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullRemoveExtension()
        {
            General.RemoveExtension(null);
        }

        [Test]
        public static void testRemoveExtension()
        {
            Assert.IsTrue(General.RemoveExtension("jobs.txt") == "jobs");
            Assert.IsTrue(General.RemoveExtension("monikers.frank.txt") == "monikers.frank");
        }

        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullSerializeConverter()
        {
            General.SerializeConverter(null);
        }
        [Test]
        [ExpectedException("System.Exception")]
        public static void testNullSerialize()
        {
            General.Serialize(null, null);
        }

        [Test]
        public static void testSerializeAndDeserializeObject()
        {

            General.Serialize("string object", "strobj.xml");
            Assert.True(File.Exists("strobj.xml"));
            string s = (string)General.DeSerialize("strobj.xml", typeof(string));
            Assert.True(s == "string object");
        }

        [Test]
        public static void testNullFormatRichText()
        {
            General.FormatRichText(null, System.Drawing.FontStyle.Bold);
        }

        [Test]
        [ExpectedException("System.ArgumentException")]
        public static void testNullSourceCopy()
        {
            General.Copy(null, null, null, null, false, 0, null);
        }
        [Test]
        [ExpectedException("System.ArgumentException")]
        public static void testNullDestinationCopy()
        {
            General.Copy(null, null, null, null, false, 0, null);
        }
        [Test]
        [ExpectedException("System.IO.IOException")]
        public static void testSourceNotFoundCopy()
        {
            General.Copy(new DirectoryInfo(INVALID_DRIVE), new DirectoryInfo("c:\\temp\\"), null, null, false, 1, null);
        }
        [Test]
        [ExpectedException("System.ArgumentException")]
        public static void testFolderLimitLessThan1Copy()
        {
            General.Copy(new DirectoryInfo(TestDirectory.Directory), new DirectoryInfo("c:\\temp\\"), "*.bak", "*", false, 0, null);
        }
        [Test]
        [ExpectedException("System.ArgumentException")]
        public static void testNullProgressCopy()
        {
            General.Copy(new DirectoryInfo(TestDirectory.Directory), new DirectoryInfo("c:\\temp\\"), "*.bak", "*", false, 0, null);
        }
        [Test]
        [ExpectedException("System.ArgumentException")]
        public static void testNullSourceGetListOfFiles()
        {
            General.GetListOfFiles(null, null, null,  0);
        }
        [Test]
        [ExpectedException("System.IO.IOException")]
        public static void testSourceNotFoundGetListOfFiles()
        {
            General.GetListOfFiles(new DirectoryInfo(INVALID_DRIVE), null, null,  1);
        }
        [Test]
        [ExpectedException("System.ArgumentException")]
        public static void testFolderLimitLessThan1GetListOfFiles()
        {
            General.GetListOfFiles(new DirectoryInfo(TestDirectory.Directory), "*.bak", "*",  0);
        }
        [Test]
        [ExpectedException("System.ArgumentException")]
        public static void testNullProgressGetListOfFiles()
        {
            General.GetListOfFiles(new DirectoryInfo(TestDirectory.Directory), "*.bak", "*",  0);
        }

[Test]
public static void testNullMakeUniqueFileName()
{
	Assert.True(General.MakeUniqueFileNameForDirectory(null, null, null, 0, 0) == null);
	Assert.True(General.MakeUniqueFileNameForDirectory("c:\\", null, null, 0, 0) == null);
	Assert.True(General.MakeUniqueFileNameForDirectory("c:\\", "test", null, 0, 0) == null);
	
}
[Test]
[ExpectedException("System.ArgumentException")]
public static void test0CharactersMakeUNiqueFileNameForDirectory()
{
	Assert.True(General.MakeUniqueFileNameForDirectory("c:\\", "test", "tst", 0, 1) == null);
}
[Test]
[ExpectedException("System.ArgumentException")]
public static void testLessThan0Count2MakeUNiqueFileNameForDirectory()
{
	Assert.True(General.MakeUniqueFileNameForDirectory("c:\\", "test", "tst", 1, -1) == null);
}

[Test]
public static void ParseStringBetweenValid()
{
	string sResult = CoreUtilities.General.SubStringBetween("Alias: Frank, Mary, Jane \\par", "Alias: ", "\\par");
	Assert.True(sResult == "Frank, Mary, Jane", "Error " + sResult);
}
[Test]
public static void ParseStringBetweenMissingEnd()
{
	string sSource = "Gender: Male\\par\nPriority: 10\\par\nAlias: Harrison";
	string sResult = CoreUtilities.General.SubStringBetween(sSource, "Alias: ", "\\par");
	Assert.True(sResult == "", "ERror " + sResult);
	
}

}//-testCoreUtilities

}
*/