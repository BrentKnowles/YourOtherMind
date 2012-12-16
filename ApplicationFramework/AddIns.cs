using System;
using System.AddIn;
using System.AddIn.Hosting;
using System.AddIn.Contract;
using System.Collections;
using System.Collections.Generic;
using Layout;
using core;
using System.Collections.ObjectModel;

namespace appframe
{
	public class AddIns
	{
		public AddIns ()
		{
		}
		public void ScanForAddIns ()
		{
			// Set the add-ins discovery root directory to be the current directory
			string addinRoot = Environment.CurrentDirectory;
			// Rebuild the add-ins cache and pipeline components cache.
			//The required folder "C:\Users\Brent\Documents\Projects\Utilities\yom2013B\yom2013B\bin\Debug\HostSideAdapters" does not exist.
			AddInStore.Rebuild (addinRoot);
			// Get registerd add-ins of type SimpleAddInHostView
			Collection<AddInToken> addins = AddInStore.FindAddIns (typeof(core.SimpleAddInHostView), addinRoot);
		
		
			foreach (AddInToken addinToken in addins) {
				// Activate the add-in
				SimpleAddInHostView addinInstance =
				addinToken.Activate<SimpleAddInHostView> (AddInSecurityLevel.Internet);
			
				// Use the add-in
				Console.WriteLine (String.Format ("Add-in {0} Version {1}",
			                                addinToken.Name, addinToken.Version));
				//Console.WriteLine(addinInstance.SayHello("Guy"));
				Console.WriteLine (addinInstance.SayHello ("Guy"));
			}
		}
	}
}

