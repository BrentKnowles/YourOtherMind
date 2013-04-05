// form_StringControl.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
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
using System.Drawing;

namespace CoreUtilities
{
	public class form_StringControl :Form
	{
		#region interfaceelements
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton buttonAdd;
		private System.Windows.Forms.ToolStripButton buttonRemove;
		private ListBox ListOfStrings;
		private System.Windows.Forms.Panel panel1;
		// set during constructor
		private Icon FormIcon;
		
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBox1;

		#endregion
		public form_StringControl (Icon formIcon)
		{
			FormIcon = formIcon;
			this.Icon = FormIcon;

			InitializeComponent();
		}

		#region Component Designer generated code
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.buttonAdd = new System.Windows.Forms.ToolStripButton();
			this.buttonRemove = new System.Windows.Forms.ToolStripButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ListOfStrings = new ListBox();

			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();

			this.toolStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.buttonAdd,
				this.buttonRemove});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(293, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// buttonAdd
			// 
			//this.buttonAdd.Image = global::CoreUtilities.UserControls.add;
			this.buttonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(95, 22);
			this.buttonAdd.Text = "Add New Item";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonRemove
			// 
			//this.buttonRemove.Image = global::CoreUtilities.UserControls.delete;
			this.buttonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(135, 22);
			this.buttonRemove.Text = "Remove Selected Item";
			this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.ListOfStrings);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(293, 293);
			this.panel1.TabIndex = 3;
			// 
			// treeView1
			// 

			this.ListOfStrings.AllowDrop = true;
			this.ListOfStrings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ListOfStrings.Location = new System.Drawing.Point(0, 0);
			this.ListOfStrings.Name = "ListOfStrings";
			this.ListOfStrings.Size = new System.Drawing.Size(291, 291);
			this.ListOfStrings.TabIndex = 2;
			this.ListOfStrings.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);


			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(211, 302);
			this.button2.Dock = DockStyle.Bottom;
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Dock = DockStyle.Bottom;
			this.button1.Location = new System.Drawing.Point(130, 302);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.textBox1.Location = new System.Drawing.Point(0, 263);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(298, 33);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = "(CONSTRUCTOR)If you press OK after modifying column definitions any data in the current table " +
				"will be deleted.";

		//	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400,400);
			this.Controls.Add (button1);
			this.Controls.Add (button2);
			this.Controls.Add (textBox1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip1);


			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "STRING BUIlDER";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		//	this.Text = "Add Text";

			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();

			this.ResumeLayout(false);
			this.PerformLayout();
		}
		
#endregion

		#region methods
		public string[] Strings
		{
			get
			{
				string[] s = new string[ListOfStrings.Items.Count];
				int nCount = -1;
				foreach (object o in ListOfStrings.Items)
				{
					nCount++;
					s[nCount] = o.ToString();
				}
				return s;
			}
			set
			{
				if (value != null)
				{
				// build the treeview
				ListOfStrings.Items.Clear ();

				foreach (string s in value)
				{
					ListOfStrings.Items.Add(s);
				}
				}
			}
		}
		private bool bSorted=false;
		
		/// <summary>
		///        // if IsSorted == true then try and place it in the correct place
		/// </summary>
		/// 
		public bool IsSorted
		{
			get { return bSorted; }
			set { bSorted = value; }
		}
		
		private void buttonAdd_Click(object sender, EventArgs e)
		{
			
			form_AddTextString addText = new form_AddTextString(FormIcon);
			if (addText.ShowDialog() == DialogResult.OK)
			{
				/* TreeNode[] found = treeView1.Nodes.Find(addText.textBox.Text,true);
                if ( found.Length > 0)
                {
                    Messag eBox.Show("Strings in list must be unique");
                    return;
                }*/
				
				if (addText.textBox.Text == " " || addText.textBox.Text == "")
				{
					NewMessage.Show("Cannot add blank strings");
					return;
				}
				if (IsSorted == false || ListOfStrings.Items.Count == 0)
				{
					ListOfStrings.Items.Add(addText.textBox.Text);
				}
				else
					if (IsSorted == true)
				{
					string sText = addText.textBox.Text;
					int i = 0;
					int nIndex = -1;
					for (i = 0; i < ListOfStrings.Items.Count; i++)
					{
						if (ListOfStrings.Items[i].ToString ().CompareTo(sText) >= 0 )
						{
							nIndex = i;
							break;
						}
						
					}
					if (nIndex != -1)
						ListOfStrings.Items.Insert(nIndex, sText);
					else
					{
						ListOfStrings.Items.Add(addText.textBox.Text);
					}
				}
				// if IsSorted == true then try and place it in the correct place
				
			}
		}
		
		/// <summary>
		/// delete selected node
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonRemove_Click(object sender, EventArgs e)
		{
			if (NewMessage.Show(Loc.Instance.GetString("Warning"), Loc.Instance.GetString("Remove this Item?"), MessageBoxButtons.YesNo, null)
			    == DialogResult.Yes)
			{
				if (ListOfStrings.SelectedItem != null)
				{
				ListOfStrings.Items.Remove (ListOfStrings.SelectedItem);
				}

			}
			
		}
		/// <summary>
		/// returns the number of tree nodes selected
		/// </summary>
		public int NumberOfSelected
		{
			get { if (ListOfStrings.SelectedItem != null) return 1; else return 0; }
		}
		/// <summary>
		/// returns the strings that are selected
		/// </summary>
		public string[] SelectedStrings
		{
			get { return new string[1] { ListOfStrings.SelectedItem.ToString ()}; }
		}
		
		private void treeView1_DoubleClick(object sender, EventArgs e)
		{
			// send event to calling form that double clicked occured
			// only applicable when being used for keywords
			if (DataChanged != null)
			{
				DataChanged(null, null);
			}
		}
		// Class that contains the data for 
		// the alarm event. Derives from System.EventArgs.
		// THIS CLASS CAN BE USED FOR MOST OF MY CUSTOM EVENTS
		public class CustomEventArgs : EventArgs
		{
			private string sMessage;
			
			//Constructor.
			//
			public CustomEventArgs(string sMessage)
			{
				this.sMessage = sMessage;
			}
			
			public string Message
			{
				get { return sMessage; }
			}
			
		}
		
		
		// Delegate declaration.
		// This delegate can be used for most of my custom events
		public delegate void CustomEventHandler(object sender, CustomEventArgs e);
		// A custom event
		public virtual event CustomEventHandler DataChanged;
		
		// The protected OnAlarm method raises the event by invoking 
		// the delegates. The sender is always this, the current instance 
		// of the class.
		//
		protected void OnDataChange(CustomEventArgs e)
		{
			if (DataChanged != null)
			{
				// Invokes the delegates. 
				DataChanged(this, e);
			}
		}
	}
		#endregion

}

