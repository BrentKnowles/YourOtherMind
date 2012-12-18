using System.ComponentModel.Composition;
using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
namespace appframe
{
	public class AddIns
	{
		string dataPath = CoreUtilities.Constants.BLANK;
		public AddIns (string path)
		{
			dataPath = path;
			if (Directory.Exists (dataPath) == false) {
				Directory.CreateDirectory (dataPath);
			}

		}
		public void TEstMEFPlug ()
		{
			if (CoreUtilities.Constants.BLANK == dataPath) {
				throw new Exception ("No path defined for AddIns");
			}

			var bootStrapper = new MefAddIns.Terminal.Bootstrapper ();
			//An aggregate catalog that combines multiple catalogs
			var catalog = new AggregateCatalog ();
			//Adds all the parts found in same directory where the application is running!
			//var currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(MainForm)).Location);
			catalog.Catalogs.Add (new DirectoryCatalog (dataPath));
			
			//Create the CompositionContainer with the parts in the catalog
			var _container = new CompositionContainer (catalog);
			
			//Fill the imports of this object
			try {
				_container.ComposeParts (bootStrapper);
			} catch (CompositionException compositionException) {
				Console.WriteLine (compositionException.ToString ());
			}
			
			//Prints all the languages that were found into the application directory
			var i = 0;
			foreach (var language in bootStrapper.Base) {
				Console.WriteLine ("[{0}] {1} by {2}.\n\t{3}\n", language.Version, language.Name, language.Author, language.Description);
				//language.Boom ();


				//string result = language.Tester ("this is the string I passed in");
				//Console.WriteLine ("RESULT = " + result);

				i++;
			}
			Console.WriteLine("It has been found {0} supported languages",i);


			foreach (var form in bootStrapper.FormBasic) {
				Console.WriteLine ("[{0}] {1} by {2}.\n\t{3}\n", form.Version, form.Name, form.Author, form.Description);
				System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess ();
				long size = proc.PrivateMemorySize64;
				Console.WriteLine ("Memory " + size);
				form.ShowWindow();
			}

			foreach (var note in bootStrapper.Notes) {
				Console.WriteLine ("[{0}] {1} by {2}.\n\t{3}\n", note.Version, note.Name, note.Author, note.Description);
				//language.Boom ();
				
				
				//string result = language.Tester ("this is the string I passed in");
				//Console.WriteLine ("RESULT = " + result);
				
				note.RegisterType();
			}

		}
		public void BuildListOfAddins()
		{
			TEstMEFPlug();
		}
		public void RunAddIn()
		{
		}


	}
}

