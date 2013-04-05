// AddInTesting.cs
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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;





namespace Testing
{

	[TestFixture]
	public class AddInTesting
	{
		public AddInTesting ()
		{

		}
		[Test]
		[Ignore]
		public void TEstMEFPlug ()
		{

			string dataPath = Environment.CurrentDirectory;

			// just use example dlls I made for testing (I copied them into this TEST FOLDER
			if (CoreUtilities.Constants.BLANK == dataPath) {
				throw new Exception("No path defined for AddIns");
			}
			var bootStrapper = new MefAddIns.Terminal.Bootstrapper();
			//An aggregate catalog that combines multiple catalogs
			var catalog = new AggregateCatalog();
			//Adds all the parts found in same directory where the application is running!
			//var currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(MainForm)).Location);
			catalog.Catalogs.Add(new DirectoryCatalog(dataPath));
			
			//Create the CompositionContainer with the parts in the catalog
			var _container = new CompositionContainer(catalog);
			
			//Fill the imports of this object
			try
			{
				_container.ComposeParts(bootStrapper);
			}
			catch (CompositionException compositionException)
			{
				Console.WriteLine(compositionException.ToString());
			}
			
			//Prints all the languages that were found into the application directory
			var i = 0;
			foreach (var language in bootStrapper.Languages)
			{
				Console.WriteLine("[{0}] {1} by {2}.\n\t{3}\n", language.Version, language.Name, language.Author, language.Description);
				language.Boom();
				string result = language.Tester("this is the string I passed in");
				Console.WriteLine ("RESULT = " + result);
				i++;
			}
			Console.WriteLine("It has been found {0} supported languages",i);
			Assert.True (false);
		}

	}
}

