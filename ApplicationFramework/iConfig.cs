using System;
using System.Windows.Forms;

namespace appframe
{
	/// <summary>
	/// For building components that are meant to be displayed and modified in the Config panel.
	/// 
	/// </summary>
	public interface iConfig
	{
		string ConfigName {get;}

		Panel GetConfigPanel();
		void SaveRequested();
	}
}

