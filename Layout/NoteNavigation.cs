// NoteNavigation.cs
//
// Copyright (c) 2013 -2014 Brent Knowles (http://www.brentknowles.com)
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
using System.Windows.Forms;
using System.Collections.Generic;
using CoreUtilities;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Layout
{


	public class NoteNavigation : TreeView
	{
		NoteDataXML_RichText RichText= null;
		ContextMenu Menu = new System.Windows.Forms.ContextMenu();
		MenuItem goTo = null;
		MenuItem widen = null;
		MenuItem clipboard = null;

	     private int mAX_NAVIGATION_WIDTH = 300;

		public int MAX_NAVIGATION_WIDTH {
			get {
				return mAX_NAVIGATION_WIDTH;
			}
			set {
			
				mAX_NAVIGATION_WIDTH = value;
				this.Width = mAX_NAVIGATION_WIDTH;
			}
		}

		/// <summary>
		/// Handles the widen click.
		/// 
		/// Allows user to adjust size so fi more detail is on the individual nodes it can bee seen.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleWidenClick (object sender, EventArgs e)
		{
			MAX_NAVIGATION_WIDTH =MAX_NAVIGATION_WIDTH  + 100;
		}



		void HandleClipboardClick (object sender, EventArgs e)
		{
			string result = CallRecursive(this);
			Clipboard.SetText (result);
		}
		private string PrintRecursive(TreeNode treeNode)
		{
			// Print the node.
			//System.Diagnostics.Debug.WriteLine(treeNode.Text);
			//MessageBox.Show(treeNode.Text);
			string result = treeNode.Text;
			// Print each node recursively.
			foreach (TreeNode tn in treeNode.Nodes)
			{
				result = String.Format ("{0}\n\t{1}", result,PrintRecursive(tn));
			}
			return result;
		}
		
		// Call the procedure using the TreeView.
		private string CallRecursive(TreeView treeView)
		{
			// Print each node recursively.
			TreeNodeCollection nodes = treeView.Nodes;
			string result = "";
			foreach (TreeNode n in nodes)
			{
				result = String.Format ("{0}\n{1}", result, PrintRecursive(n));
			}
			return result;
		}
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

			widen = new MenuItem();
			widen.Text = Loc.Instance.GetString("Widen");
			widen.Enabled = true;
			widen.Click+= HandleWidenClick;


			clipboard = new MenuItem();
			clipboard.Text = Loc.Instance.GetString("Clipboard");
			clipboard.Enabled = true;
			clipboard.Click+= HandleClipboardClick;

			Menu.MenuItems.Add (refreshMenu);
			Menu.MenuItems.Add (goTo);
			Menu.MenuItems.Add (widen);
			Menu.MenuItems.Add (clipboard);
		//	this.NodeMouseClick += HandleNodeMouseClick;
			this.ContextMenu = Menu;
			Menu.Popup+= HandlePopup;
			this.ShowNodeToolTips = false;

			this.DrawMode = TreeViewDrawMode.OwnerDrawText;
			this.DrawNode+= HandleDrawNode;
		}

		/// <summary>
		/// Each semicolon will be removed and the words given a DIFFERENT COLOR
		/// to help review scene lists in a nicer format.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleDrawNode (object sender, DrawTreeNodeEventArgs e)
		{
			string[] texts = e.Node.Text.Split (new char[1]{';'}, 
			StringSplitOptions.RemoveEmptyEntries);
			const int MaxColors = 5;
			Color[] colors = new Color[MaxColors] {Color.Black, Color.Red, Color.Blue, Color.Magenta, Color.Green};
			SizeF s;
			s = new SizeF(0,0);
			for (int i = 0; i < texts.Length && i < MaxColors; i++) {

				using (Font font = new Font(this.Font, FontStyle.Regular)) {

					using (Brush brush = new SolidBrush(colors[i])) {

						if (i > 0)
						{
							SizeF newSize =  e.Graphics.MeasureString(texts[i-1], font);
							s.Height = newSize.Height + s.Height;
							s.Width = newSize.Width + s.Width;
						}

						e.Graphics.DrawString(texts[i], font, brush, e.Bounds.Left + (int)s.Width, e.Bounds.Top);
						//e.Graphics.DrawString (texts [0], font, brush, e.Bounds.Left, e.Bounds.Top);
					}
//					if (texts.Length >= 2)
//					{
//						// measaure first words and add us at the end.
//						SizeF s = e.Graphics.MeasureString(texts[0], font);
//						using (Brush brush = new SolidBrush(Color.Red)) {
//							e.Graphics.DrawString(texts[1], font, brush, e.Bounds.Left + (int)s.Width, e.Bounds.Top);
//						}
//					}
				}

			}

		
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
			if (items != null && this != null && tree != null && tree.Nodes != null) {
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
					try
					{
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
				if (item.Level == LastLevel && LastNode.Parent != null) {
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
							try
							{
							LastNode.Nodes [0].Nodes.Add (Loc.Instance.GetStringFmt ("Heading Mismatch {0}", item.Name));
							}
							catch (Exception)
							{
								LastNode.Nodes.Add (Loc.Instance.GetStringFmt ("Error building {0}. Perhaps a heading has spaces after the trailing delimeter?", item.Name));
							}
						}
					}

//
					LastNode = newItem;
					LastLevel = item.Level;
					}
					catch (System.Exception ex)
					{
						NewMessage.Show ("RichTextParent = " + RichText.Caption+ "item level = " + item.Level+ "item.name = "+item.Name +"item = " + item.ToString () + " --  " + ex.ToString ());
					}
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
			if (Disposing || IsDisposed) return;
			// don't waste time redrawing if not visible.08/07/2014
			if (this.Visible == false) return;

			try {
				if (null != RichText && this.Nodes != null && this != null) {
					this.Nodes.Clear ();
					//List<TreeNode> roots = new List<TreeNode>();

					// parses the text
					List<TreeItem> items = BuildList ();
					if (items != null) {
						// add current location
						items.Add (new TreeItem (Loc.Instance.GetString ("Last Position"), 0, RichText.SelectionStart));
						// builds the treeview
						PopulateTree (this, items);
					}

					this.ExpandAll ();
				} else {
					throw new Exception ("No richtext was passed into UpdateListOfBookmarks");
				}
			} catch (System.Exception ex) {
				NewMessage.Show (ex.ToString ());
			}

		}
		/// <summary>
		/// Retrieves the number of nodes. For testing.
		/// </summary>
		/// <returns>
		/// The of nodes.
		/// </returns>
		public int NumberOfNodes ()
		{
			return this.GetNodeCount(true);
		}
	}
}

