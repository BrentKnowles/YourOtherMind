using System;
using System.Windows.Forms;
using Layout;
using CoreUtilities;

namespace LayoutPanels
{
	public class HeaderBar : IDisposable
	{
		#region variables
		LayoutPanel Layout;
		LayoutInterface Notes;
		ToolStrip headerBar = null;

		#endregion
		public HeaderBar (LayoutPanel layout, LayoutInterface notes)
		{
			if (null == layout || null == notes) {
				throw new Exception("Must pass in a valid layout");
			}
			Layout = layout;
			Notes = notes;
			HeaderToolbar();
			UpdateHeader ();
			headerBar.SendToBack();
		}
		private void HeaderToolbar ()
		{
			headerBar = new ToolStrip();
			headerBar.Parent = (Control)Layout;
			headerBar.Dock = DockStyle.Top;
			headerBar.Visible = true;
			
			
		}
		/// <summary>
		/// Releases all resource used by the <see cref="LayoutPanels.HeaderBar"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="LayoutPanels.HeaderBar"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="LayoutPanels.HeaderBar"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="LayoutPanels.HeaderBar"/> so the garbage
		/// collector can reclaim the memory that the <see cref="LayoutPanels.HeaderBar"/> was occupying.
		/// </remarks>
		public void Dispose()
		{
			Layout.Controls.Remove (headerBar);
			headerBar = null;
		}
		void HandlePropertiesDropDownOpening (object sender, EventArgs e)
		{
			(sender as ToolStripDropDownButton).DropDownItems.Clear ();
			ToolStripTextBox changeName = new ToolStripTextBox();
			changeName.Text = Notes.Name;
			changeName.KeyDown += HandleChangeNameKeyDown;
			changeName.TextChanged+= HandleChangeNameClick;



			//tabMenu.DropDownItems.Remove (tabMenu.DropDownItems.Add ("empty"));

			(sender as ToolStripDropDownButton).DropDownItems.Add (changeName);
			//(sender as ToolStripDropDownButton).DropDownItems.Add (tabMenu);
		}


	
		void HandleChangeNameClick (object sender, EventArgs e)
		{
			Notes.Name = (sender as ToolStripTextBox).Text;
			Layout.SetSaveRequired(true);
			
		}
		void HandleChangeNameKeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter) {
				// the header is not updated unti enter pressed but the NAME is being updated
				UpdateHeader ();
			}
		}
		
		public void UpdateHeader ()
		{
			if (false == Layout.GetIsChild && false == Layout.GetIsSystemLayout ) {
				headerBar.Items.Clear ();
				ToolStripLabel bold = new ToolStripLabel ();
				bold.Text = Notes.Name;
				headerBar.Items.Add (bold);
				
				ToolStripDropDownButton properties = new ToolStripDropDownButton();
				properties.Text = Loc.Instance.GetString("Properties");
				properties.DropDownOpening+= HandlePropertiesDropDownOpening;
				
				
				
				headerBar.Items.Add (properties);

				lg.Instance.Line("HeaderBar.UpdateHeader", ProblemType.MESSAGE, "Header should be visible");
				
			}
		}
		

	}
}

