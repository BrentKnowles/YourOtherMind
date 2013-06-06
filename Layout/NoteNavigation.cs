using System;
using System.Windows.Forms;
using System.Collections.Generic;
using CoreUtilities;
using System.Text.RegularExpressions;

namespace Layout
{


	public class NoteNavigation : TreeView
	{
		NoteDataXML_RichText RichText= null;
		ContextMenu Menu = new System.Windows.Forms.ContextMenu();
		MenuItem goTo = null;

		public NoteNavigation (NoteDataXML_RichText richText)

		{
			RichText = richText;
			MenuItem refreshMenu = new MenuItem();
			refreshMenu.Text = Loc.Instance.GetString ("Refresh");
			refreshMenu.Click+= HandleRefreshMenuClick;

			goTo = new MenuItem();
			goTo.Text = Loc.Instance.GetString("Go here");
			goTo.Enabled = false;
			goTo.Click+= HandleGoToClick;

			Menu.MenuItems.Add (refreshMenu);
			Menu.MenuItems.Add (goTo);
		//	this.NodeMouseClick += HandleNodeMouseClick;
			this.ContextMenu = Menu;
			Menu.Popup+= HandlePopup;
			this.ShowNodeToolTips = false;
		}

		void HandleGoToClick (object sender, EventArgs e)
		{
			if (this.SelectedNode != null) {
				if (this.SelectedNode.Tag != null) {
					int Position = 0;
					Int32.TryParse (SelectedNode.Tag.ToString (), out Position);
					if (RichText == null) {
						throw new Exception ("RichText not defined in HandleNodeMouseClick of BookMarkView");
					}
					RichText.SelectionStart = Position;
					RichText.ScrollToNearPosition ();
					//		NewMessage.Show (Position.ToString ());
				}
			}
		}

		void HandlePopup (object sender, EventArgs e)
		{
			if (this.SelectedNode != null) {
				if (this.SelectedNode.Level > 0) {
					goTo.Enabled = true;
				} else {
					goTo.Enabled = false;
				}
			}
		}

	

		void HandleRefreshMenuClick (object sender, EventArgs e)
		{
			UpdateListOfBookmarks();
		}

		//testlist
//		private List<TreeItem> BuildList()
//		{
//			List<TreeItem> items = new List<TreeItem>();
//			items.Add (new TreeItem("boom", 0,100));
//			items.Add (new TreeItem("boom2", 1,115));
//			items.Add (new TreeItem("boom3", 1,125));
//			items.Add (new TreeItem("boom3", 2,125));
//			items.Add (new TreeItem("boom33", 3,125));
//			items.Add (new TreeItem("boom35", 4,125));
//			items.Add (new TreeItem("boom3 44", 3,125));
//			items.Add (new TreeItem("boom3 55", 2,125));
//			items.Add (new TreeItem("boom3 66", 1,125));
//			items.Add (new TreeItem("boom5", 0,125));
//			items.Add (new TreeItem("boom6", 1,125));
//			items.Add (new TreeItem("boom6", 2,125));
//			items.Add (new TreeItem("boom6", 3,125));
//			items.Add (new TreeItem("boom6", 4,125));
//
//			items.Add (new TreeItem("boom7", 0,125));
//			items.Add (new TreeItem("boom8", 1,125));
//			return items;
//
//		}
		private List<TreeItem> BuildList ()
		{
			return LayoutDetails.Instance.GetCurrentMarkup().BuildList(RichText);

		}
		void PopulateTree (TreeView tree, List<TreeItem> items)
		{
			if (items != null) {
				tree.Nodes.Clear ();
				List<TreeNode> roots = new List<TreeNode> ();
				tree.Nodes.Add (Loc.Instance.GetString ("Note"));
				int LastLevel = 100;
				TreeNode LastNode = null;

				foreach (TreeItem item in items) {
					// my requirements are simpler, we know they will be in order
					// so if MY_LEVEL is GREATER than LAST_LEVEL, we add to the LastNode
					// else if LESS_THAN, we add to previous??

					TreeNode newItem = new TreeNode (item.Name);
					newItem.Tag = item.Position;

					if (null == LastNode) {
						tree.Nodes [0].Nodes.Add (newItem);
					} else
				if (item.Level > LastLevel) {
						// if we skip, we put in "fake parents"
						// to alert the user that they have jumped too deep into a subheading
						for (int i = 1; i < (item.Level-LastLevel); i++) {
							TreeNode gap = new TreeNode (Loc.Instance.GetString ("Missing Parent Heading"));
							gap.Tag = item.Position;
							LastNode.Nodes.Add (gap);
							LastNode = gap;
						}


						LastNode.Nodes.Add (newItem);
				    
					} else
				if (item.Level == LastLevel) {
						// we add to the parent of the lastNode?
						LastNode.Parent.Nodes.Add (newItem);
					} else {
						// trickier
						// we need to find an appropriate parent
						// but the one closest to me.

						// Count backwards from LastLevel until it equals my level
						for (int i = 1; i <= (LastLevel - item.Level); i++) {
							if (LastNode.Parent == null) {
								throw new Exception ("Data was not formatted correct.");
							} else
								LastNode = LastNode.Parent;

						}
						try {
							LastNode.Parent.Nodes.Add (newItem);
						} catch (Exception) {
							// we had poorly formatted data
							LastNode.Nodes [0].Nodes.Add (Loc.Instance.GetStringFmt ("Heading Mismatch {0}", item.Name));
						}
					}
//
					LastNode = newItem;
					LastLevel = item.Level;
//				if (item.Level == roots.Count)
//					roots.Add (roots [roots.Count - 1].LastNode);
//				TreeNode newItem = new TreeNode(item.Name);
//				newItem.Tag = item.Position;
//				roots [item.Level].Nodes.Add (newItem);
				}
			}
		}
//		void PopulateTree (TreeView tree, List<TreeItem> items)
//		{
//			this.Nodes.Clear ();
//			List<TreeNode> roots = new List<TreeNode> ();
//			roots.Add (this.Nodes.Add ("Items"));
//			foreach (TreeItem item in items) {
//				if (item.Level == roots.Count)
//					roots.Add (roots [roots.Count - 1].LastNode);
//				TreeNode newItem = new TreeNode(item.Name);
//				newItem.Tag = item.Position;
//				roots [item.Level].Nodes.Add (newItem);
//			}
//		}
		/// <summary>
		/// Updates the list of bookmarks.
		/// 
		/// Parses the text of the attached RichEdit and generates a clickable view
		/// </summary>
		public void UpdateListOfBookmarks ()
		{

			if (null != RichText) {
				this.Nodes.Clear ();
				//List<TreeNode> roots = new List<TreeNode>();

				// parses the text
				List<TreeItem> items = BuildList ();
				// add current location
				items.Add (new TreeItem(Loc.Instance.GetString("Last Position"), 0, RichText.SelectionStart));
				// builds the treeview
				PopulateTree(this, items );

				this.ExpandAll();
			} else {
				throw new Exception("No richtext was passed into UpdateListOfBookmarks");
			}

		}
	}
}

