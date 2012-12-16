using System;

namespace Layout
{
	/// <summary>
	/// List of layouts interface.
	/// 
	/// This is the Master List of All Layouts (Pages) in the Program, much like the Data Class from the previous version
	/// </summary>
	public interface iMasterOfLayouts : IDisposable
	{

		System.Collections.Generic.List<NameAndGuid> GetListOfLayouts (string filter);
	
	}
}

