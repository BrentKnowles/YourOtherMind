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

