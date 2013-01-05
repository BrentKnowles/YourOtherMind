using System;
using CoreUtilities;
using appframe;

using System.Windows.Forms;

namespace YOM2013
{
	public class Options_InterfaceElements : iConfig, IDisposable
	{

		#region variables_private
		
		
		string DatabaseName;
		Panel configPanel;
#endregion
		
		const string columnID="id";
		const string columnKey="key";
		const string columnValue="value";
		const string TableName = "generalsettings";
		
		#region variables
		
		#endregion
		
		
		// if true on save we know it is safe to try to save (because interface exists)
		bool PanelWasMade = false;
		public Options_InterfaceElements (string _DatabaseName)
		{

			this.DatabaseName = _DatabaseName;
			
			
			
		}
		public void Dispose ()
		{
		}

		public string ConfigName {
			get { return Loc.Instance.GetString ("Interface");}
		}
		public Panel GetConfigPanel ()
		{
			// if panel made then leave it alone
			if (PanelWasMade == true) return configPanel;
			
			PanelWasMade = true;
			
			configPanel = new Panel ();
			
			
			Label 	label = new Label();
			label.Text ="Should maybe inherit some methods from Options.cs. Will have option to Reset System Layout";
			configPanel.Controls.Add (label);
			label.Dock = DockStyle.Top;

			
			
			
			return configPanel;
			
		}
		public void SaveRequested ()
		{
			
			
			if (configPanel == null) {
				throw new Exception ("no config panel defined");
			}
			
			if (false == PanelWasMade)
				return;

		}
	}
}

