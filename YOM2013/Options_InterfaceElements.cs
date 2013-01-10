using System;
using CoreUtilities;
using appframe;
using Layout;
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

			Button buttonResetSystem = new Button();
			buttonResetSystem.Text = Loc.Instance.GetString("Reset System Layout");
			buttonResetSystem.Click+= HandleResetSysteClick;

			buttonResetSystem.Dock = DockStyle.Top;
			configPanel.Controls.Add (label);
			configPanel.Controls.Add (buttonResetSystem);


			label.Dock = DockStyle.Top;

			
			
			
			return configPanel;
			
		}

		void HandleResetSysteClick (object sender, EventArgs e)
		{
			
			if (NewMessage.Show (Loc.Instance.GetString ("Caution!"), Loc.Instance.GetString ("Reset the system layout to restore default settings. Doing so will lose any customization to the sytem layout AND require a restart of the application. Proceed?"),
			                    MessageBoxButtons.YesNo, null) == DialogResult.Yes) {
				// do reset
				// 1. Delete existing system note
				MasterOfLayouts.DeleteLayout("system");
				// 2. Restore note
				// because we reboot it does not matter that I do not pass a valid contextmenu into here
				DefaultLayouts.CreateASystemLayout(null);
				// 3. Restart
				Application.Exit ();
			}

		}
		public void SaveRequested ()
		{
			
			if (false == PanelWasMade)
				return;


			if (configPanel == null) {
				throw new Exception ("no config panel defined");
			}
			
		

		}
	}
}

