// TablePanel.cs
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
//using System.ComponentModel;
using System.Data;
using CoreUtilities;
using System.Drawing;
using CoreUtilities.Tables;
namespace appframe
{
	
	public class DataGridNoKeyPressIrritation : DataGridView
	{
		// replace with real code
		public bool SafeEditMode;

	}





	/// <summary>
	/// Table panel.
	/// </summary>
	public class TablePanel : Panel
	{
	

		#region constant

		public const int defaultwidth = 100;

#endregion
		#region variables
		public bool SuppressWarning = false;// able to turn off the no columns warning when it is not a valid warning (beacuse table was not visible)
		// delegate: called whenever the table changes
		Func<int> TableChanged = null;
		Func<string> GenerateResults = null;
		Action<string>GoToNote=null;
		
		//used to load default settings
		ColumnDetails[] IncomingColumns= null;
		string TableName=Constants.BLANK;


		public bool ReadOnly {
			get {return dataGrid1.ReadOnly;}
			set {dataGrid1.ReadOnly = value;}
		}

#endregion

		void Row_Changed (object sender, DataRowChangeEventArgs e)
		{
			TableChanged();
		}

		public TablePanel (DataTable _dataSource, Func<int> _tableChanged, ColumnDetails[] incomingColumns, Action<string>_GoToNote, string _TableName, Func<string> _GenerateResults)
		{
			InitializeComponent ();
			if (null != _dataSource) {

				dataGrid1.DataSource = _dataSource;
				dataGrid1.DataBindingComplete += HandleDataBindingComplete;
				dataGrid1.CellBeginEdit += HandleCellBeginEdit;


				_dataSource.RowChanged+=Row_Changed;
				dataGrid1.DataSourceChanged += HandleDataSourceChanged;
//				NewMessage.Show (((DataTable)dataGrid1.DataSource).Columns.Count.ToString());
//				NewMessage.Show (Columns.Length.ToString ());
//				NewMessage.Show (dataGrid1.Rows.Count.ToString ());
			} else {
				NewMessage.Show ("passed a null datasource in!");
			}

			TableChanged = _tableChanged;
			IncomingColumns = incomingColumns;
			GoToNote = _GoToNote;
			// this would be the Note.Caption and is used in GenerateAllResults
			TableName = _TableName;
			GenerateResults = _GenerateResults;

			panel1.Visible = false;


		}
		/// <summary>
		/// Sets the save needed.
		/// 
		/// Called when need to alert parents we made a change warranting a save
		/// </summary>
		/// <param name='needsave'>
		/// If set to <c>true</c> needsave.
		/// </param>
		private void SetSaveNeeded (bool needsave)
		{
			if (true == needsave) {
				if (null != TableChanged) {
					TableChanged ();
				}
			}
		}

		void HandleDataSourceChanged (object sender, EventArgs e)
		{
			// should set NeedsSave flag if updating columng
			SetSaveNeeded(true);
		}
		/// <summary>
		/// With data complete load the stored widths
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleDataBindingComplete (object sender, DataGridViewBindingCompleteEventArgs e)
		{

			if (dataGrid1.DataSource != null && IncomingColumns != null) {
				// set column widths
				for (int i = 0 ; i < IncomingColumns.Length ; i++) {
					if (i >= dataGrid1.Columns.Count)
					{
						Console.Beep ();
						// this message was annoying during tests
					//		NewMessage.Show (Loc.Instance.GetStringFmt("(Jan 16 2013) Import Only Error? There are more IncomingColumns than columns in the datagrid? Datagrid has {0} columns, Incoming has {1}. They can be ignored. Just means non standard tables",
					//	                                           dataGrid1.Columns.Count, IncomingColumns.Length));

					}
					else
					dataGrid1.Columns[i].Width = IncomingColumns[i].ColumnWidth;
				}
				IncomingColumns = null;
				
			}
		}



		void HandleCellBeginEdit (object sender, DataGridViewCellCancelEventArgs e)
		{
			if (null != TableChanged) {
				TableChanged();
			}
		}





		#region Component Designer generated code

		private System.Windows.Forms.ToolStrip runStrip;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripButton buttonPreview;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripButton buttonEditColumns;
		private System.Windows.Forms.ToolStripButton buttonJumpLink;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private DataGridNoKeyPressIrritation dataGrid1;
		private System.Windows.Forms.ToolStripButton toolStripButtonEditMode;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButtonLoadTableFile;
		private System.Windows.Forms.ToolStripButton toolStripButtonSaveTableToXML;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripButtonInsert;
		private System.Windows.Forms.ToolStripButton toolStripButtonNumber;


		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.runStrip = new System.Windows.Forms.ToolStrip();
			this.buttonPreview = new System.Windows.Forms.ToolStripButton();
			this.buttonEditColumns = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.buttonJumpLink = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonEditMode = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonLoadTableFile = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSaveTableToXML = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonInsert = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonNumber = new System.Windows.Forms.ToolStripButton();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.dataGrid1 = new DataGridNoKeyPressIrritation();
			this.runStrip.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// runStrip
			// 
			this.runStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.buttonPreview,
				this.buttonEditColumns,
				this.toolStripButton1,
				this.toolStripButton2,
				this.toolStripSeparator1,
				this.buttonJumpLink,
				this.toolStripButtonEditMode,
				this.toolStripSeparator2,
				this.toolStripButtonLoadTableFile,
				this.toolStripButtonSaveTableToXML,
				this.toolStripSeparator3,
				this.toolStripButtonInsert,
				this.toolStripButtonNumber});
			this.runStrip.Location = new System.Drawing.Point(0, 0);
			this.runStrip.Name = "runStrip";
			this.runStrip.Size = new System.Drawing.Size(530, 25);
			this.runStrip.TabIndex = 2;
			this.runStrip.Text = "toolStrip1";
			// 
			// buttonPreview
			// 

			this.buttonPreview.Image =   FileUtils.GetImage_ForDLL("zoom.png");
			this.buttonPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonPreview.Name = "buttonPreview";
			this.buttonPreview.Size = new System.Drawing.Size(23, 22);
			this.buttonPreview.ToolTipText = Loc.Instance.GetString ("Preview");
			this.buttonPreview.Click += new System.EventHandler(this.previewclick);
			// 
			// buttonEditColumns
			// 
			this.buttonEditColumns.Image =  FileUtils.GetImage_ForDLL("table_edit.png");
			this.buttonEditColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonEditColumns.Name = "buttonEditColumns";
			this.buttonEditColumns.Size = new System.Drawing.Size(23, 22);
			this.buttonEditColumns.ToolTipText = Loc.Instance.GetString ("Edit Columns");
			this.buttonEditColumns.Click += new System.EventHandler(this.buttonEditColumns_Click);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.Image =FileUtils.GetImage_ForDLL("script_edit.png");
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.ToolTipText = Loc.Instance.GetString ("Import List");
			this.toolStripButton1.Click += new System.EventHandler(this.ImportListClick);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.Image =FileUtils.GetImage_ForDLL("page_copy.png");
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.ToolTipText =Loc.Instance.GetString ( "Copy To Clipboard");
			this.toolStripButton2.Click += new System.EventHandler(this.CopyToClipboardClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// buttonJumpLink
			// 
			this.buttonJumpLink.Image = FileUtils.GetImage_ForDLL("link_go.png");
			this.buttonJumpLink.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonJumpLink.Name = "buttonJumpLink";
			this.buttonJumpLink.Size = new System.Drawing.Size(23, 22);
			this.buttonJumpLink.ToolTipText = Loc.Instance.GetString ("Follow Link");
			this.buttonJumpLink.Click += new System.EventHandler(this.buttonJumpLink_Click);
			// 
			// toolStripButtonEditMode
			// 
			this.toolStripButtonEditMode.Checked = true;
			this.toolStripButtonEditMode.CheckOnClick = true;
			this.toolStripButtonEditMode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.toolStripButtonEditMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;

			this.toolStripButtonEditMode.Image = FileUtils.GetImage_ForDLL("tag_blue.png");
			this.toolStripButtonEditMode.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonEditMode.Name = "toolStripButtonEditMode";
			this.toolStripButtonEditMode.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonEditMode.Text = Loc.Instance.GetString ("Toggle Safe Edit Mode ** REMOVE? **");
			this.toolStripButtonEditMode.Click += new System.EventHandler(this.toggleSafeEditModeClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonLoadTableFile
			// 
			this.toolStripButtonLoadTableFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;

			this.toolStripButtonLoadTableFile.Image = FileUtils.GetImage_ForDLL("table_add.png"); 
			this.toolStripButtonLoadTableFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonLoadTableFile.Name = "toolStripButtonLoadTableFile";
			this.toolStripButtonLoadTableFile.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonLoadTableFile.Text = "toolStripButton3";
			this.toolStripButtonLoadTableFile.ToolTipText = Loc.Instance.GetString ("Load a xml based datatable");
			this.toolStripButtonLoadTableFile.Click += new System.EventHandler(this.toolStripButtonLoadTableFile_Click);
			// 
			// toolStripButtonSaveTableToXML
			// 
			this.toolStripButtonSaveTableToXML.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSaveTableToXML.Image = FileUtils.GetImage_ForDLL("table_save.png"); 
			this.toolStripButtonSaveTableToXML.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSaveTableToXML.Name = "toolStripButtonSaveTableToXML";
			this.toolStripButtonSaveTableToXML.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonSaveTableToXML.Text = "toolStripButton3";
			this.toolStripButtonSaveTableToXML.ToolTipText = Loc.Instance.GetString ("Save table to an XML file");
			this.toolStripButtonSaveTableToXML.Click += new System.EventHandler(this.toolStripButtonSaveTableToXML_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonInsert
			// 
			this.toolStripButtonInsert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonInsert.Image = FileUtils.GetImage_ForDLL("basket_add.png"); 
			this.toolStripButtonInsert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonInsert.Name = "toolStripButtonInsert";
			this.toolStripButtonInsert.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonInsert.Text = "toolStripButton3";
			this.toolStripButtonInsert.ToolTipText = Loc.Instance.GetString ("Insert row");
			this.toolStripButtonInsert.Click += new System.EventHandler(this.toolStripButtonInsert_Click);
			// 
			// toolStripButtonNumber
			// 
			this.toolStripButtonNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonNumber.Image = FileUtils.GetImage_ForDLL("tag.png");
			this.toolStripButtonNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonNumber.Name = "toolStripButtonNumber";
			this.toolStripButtonNumber.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonNumber.Text = "Auto number";
			this.toolStripButtonNumber.ToolTipText = Loc.Instance.GetString ("Auto number rows");
			this.toolStripButtonNumber.Click += new System.EventHandler(this.AutoNumberClick);
			// 
			// richTextBox1
			// 
			this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Location = new System.Drawing.Point(0, 13);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(163, 263);
			this.richTextBox1.TabIndex = 3;
			this.richTextBox1.Text = "a";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Result Preview";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.richTextBox1);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(163, 276);
			this.panel1.TabIndex = 5;
			// 
			// dataGrid1
			// 
			//this.dataGrid1.AlternatingBackColor = System.Drawing.Color.WhiteSmoke;
			this.dataGrid1.BackColor = System.Drawing.Color.Gainsboro;
			this.dataGrid1.BackgroundColor = System.Drawing.Color.DarkGray;
			this.dataGrid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			//this.dataGrid1.CaptionBackColor = System.Drawing.Color.DarkKhaki;
			//this.dataGrid1.CaptionFont = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
			//this.dataGrid1.CaptionForeColor = System.Drawing.Color.Black;
			//this.dataGrid1.CaptionVisible = false;
			//this.dataGrid1.DataMember = global::Worgan2006.Header.Blank;
			this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			//this.dataGrid1.FlatMode = true;
			this.dataGrid1.Font = new System.Drawing.Font("Times New Roman", 9F);
			this.dataGrid1.ForeColor = System.Drawing.Color.Black;
		//	this.dataGrid1.GridLineColor = System.Drawing.Color.Silver;
		//	this.dataGrid1.HeaderBackColor = System.Drawing.Color.Black;
		//	this.dataGrid1.HeaderFont = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
		//	this.dataGrid1.HeaderForeColor = System.Drawing.Color.White;
		//	this.dataGrid1.LinkColor = System.Drawing.Color.DarkSlateBlue;
			this.dataGrid1.Location = new System.Drawing.Point(163, 25);
			this.dataGrid1.Name = "dataGrid1";
		//	this.dataGrid1.ParentRowsBackColor = System.Drawing.Color.LightGray;
		//	this.dataGrid1.ParentRowsForeColor = System.Drawing.Color.Black;
		//	this.dataGrid1.PreferredColumnWidth = 150;
			this.dataGrid1.SafeEditMode = true;
		//	this.dataGrid1.SelectionBackColor = System.Drawing.Color.Firebrick;
		//	this.dataGrid1.SelectionForeColor = System.Drawing.Color.White;
			this.dataGrid1.Size = new System.Drawing.Size(367, 276);
			this.dataGrid1.TabIndex = 0;
			this.dataGrid1.CurrentCellChanged += new System.EventHandler(this.dataGrid1_CurrentCellChanged);
			// 
			// tablePanel
			// 
			//this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			//this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.dataGrid1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.runStrip);
			this.Name = "tablePanel";
			this.Size = new System.Drawing.Size(530, 301);
			this.runStrip.ResumeLayout(false);
			this.runStrip.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		
#endregion

	


		/// <summary>
		/// used to hide the toolbar when used for a culture or similiar type of page
		/// 
		/// does not actually hide, instead disables all the buttons Except jump link and Clipboard
		/// </summary>
		private bool mHideToolbar;
		public bool HideToolbar
		{
			get { return mHideToolbar; }
			set
			{
				mHideToolbar = value;
				if (mHideToolbar == true)
				{
					toolStripButton1.Enabled = false;
					buttonEditColumns.Enabled = false;
					buttonPreview.Enabled = false;
				}
				else
				{
					toolStripButton1.Enabled = true;
					buttonEditColumns.Enabled = true;
					buttonPreview.Enabled = true;
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripButton1_Click(object sender, EventArgs e)
		{

			// FEB 2013 -- This is just old code. Not used. Table save/load does work.

			// temp: create and save a new classPageTable (this will include save the .xtt file
			// that is the table
//			panelTable = new classPageTable();
//			
//			// pick columns
//			fAddColumn addColumn = new fAddColumn();
//			addColumn.ShowDialog();
//			
//			Cols = addColumn.tree_StringControl1.Strings;
//			// now hook up to datagrid
//			dataGrid1.DataSource = null;
//			dataGrid1.DataSource = panelTable.dataSource;
//			dataGrid1.NavigateTo(0, "Table1");
			//propertyGrid1.SelectedObject = panelTable;
			
		}
		
		/// <summary>
		/// save the table
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonSave_Click(object sender, EventArgs e)
		{
			
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				//(DataTable)dataGrid1.DataSource

				//if (panelTable != null)
				//	panelTable.Save(saveFileDialog1.FileName);

			}
		}



		/// <summary>
		///  because I made the panel persistent I've been running into a lot of weird bugs
		/// This resets the panel
		/// </summary>
		public void RefreshAll()
		{
			richTextBox1.Visible = false;
			label1.Visible = false;
			dataGrid1.DataSource = null;
			panel1.Visible = false;
			
		}
		/// <summary>
		/// Open file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripButton1_Click_1(object sender, EventArgs e)
		{
//			if (openFileDialog1.ShowDialog() == DialogResult.OK)
//			{
//				panelTable = (classPageTable)CoreUtilities.General.DeSerialize(openFileDialog1.FileName, typeof(classPageTable));
//				if (panelTable != null)
//				{
//					dataGrid1.DataSource = panelTable.dataSource;
//					dataGrid1.NavigateTo(0, "Table1");
//					//  propertyGrid1.SelectedObject = panelTable;
//				}
//			}
		}
		
		private void previewclick (object sender, EventArgs e)
		{

			string sResult = "";
			
			if (GenerateResults != null) {
				sResult = GenerateResults();

			}


//		
			if (sResult != Constants.BLANK)
			{
				richTextBox1.Visible = true;
				label1.Visible = true;
				panel1.Visible = true;
				richTextBox1.Text = "";
				sResult = sResult.Trim();
				if (sResult != null && sResult != "")
				{
					sResult = sResult + "\n"; // the trim removes the \n
					richTextBox1.AppendText(sResult);
					
					
				}
			}
		}
		
		private int last_row = 0;
		/// <summary>
		/// whenever data changes set page saved to false
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGrid1_CurrentCellChanged (object sender, EventArgs e)
		{
			// october 2012 -- find I need to keep track of the index myself?
			if (dataGrid1.CurrentCell != null ) {
				last_row = dataGrid1.CurrentCell.RowIndex; 
			
				SetSaveNeeded (true);
			} else {
				last_row = 0;
			}
		}

	//	public Appearance appearance = null;
		/// <summary>
		/// September 2011
		/// A wrapper for column rquestss in case I am using this from the appearance system
		/// </summary>
		private ColumnDetails[] Columns {
			get {

				ColumnDetails[] columns = new ColumnDetails[dataGrid1.Columns.Count];

				if (0 == dataGrid1.Columns.Count)
				{
					string message = Loc.Instance.GetStringFmt("This table {0} has no columns! This usually happens if loading the Table without the Layout it contains being on a form! And form has to call its Show method",this.TableName);
					lg.Instance.Line("TablePanel->Columns->Get", ProblemType.WARNING, "This happens when saving a NON VISIBILE table. Suppressing the Popup in this situation but leaving the log in case it causes errors " +message);
					if (SuppressWarning == false) NewMessage.Show(message);
					return null;
				}
				for (int i = 0; i <  dataGrid1.Columns.Count; i++) {
					columns [i] = new ColumnDetails (dataGrid1.Columns[i].Name, dataGrid1.Columns[i].Width);
				}

//					if (appearance != null)
//					{
//						return appearance.GenerateColums();
//					}
				return columns;
			}
			
		}
			
			
			
			

		/// <summary>
		/// Gets the table.
		/// 
		/// Called when the note calls Save()
		/// </summary>
		/// <returns>
		/// The table.
		/// </returns>
		public DataTable GetTable()
		{
			return(DataTable) dataGrid1.DataSource;
		}
		public ColumnDetails[] GetColumns ()
		{
			return GetColumns(false);
		}
		/// <summary>
		/// Gets the columns.
		/// 
		/// Called when the note calls Save()
		/// </summary>
		/// <returns>
		/// The columns.
		/// </returns>
		public ColumnDetails[] GetColumns(bool DoISuppressWarning)
		{
			SuppressWarning = DoISuppressWarning;
			return Columns;
		}
		/// <summary>
		/// Builds the table from columns.
		/// </summary>
		/// <returns>
		/// The table from columns.
		/// </returns>
		public static DataTable BuildTableFromColumns (ColumnDetails[] value)
		{
			DataTable table = new DataTable();
			table.TableName = TableWrapper.TablePageTableName;
			//  table.Columns.Add("Roll", typeof(string));
			// add default column
			
			foreach (ColumnDetails column in value)
			{
				try
				{
					table.Columns.Add(column.ColumnName, typeof(string));
				}
				catch (Exception)
				{
					// added when adding blank column names for faiths
					
				}
				
				
			}
			return table;
		}

		private string[] GetJustColumnNames ()
		{
			// trims column detail array just to the name
			string[] columns = new string[Columns.Length];
							for(int i = 0 ; i <  Columns.Length; i++)
							{
								columns[i] = Columns[i].ColumnName;
							}
			return columns;
		}

//		private DataSet DATA_SOURCE()
//		{
//			if (panelTable == null)
//			{
//				return appearance.dataSource;
//			}
//			else
//			{
//				return panelTable.dataSource;
//			}
		//}


		protected void EditColumns()
		{
			// changing columns can destroy data
			// pick columns
			form_StringControl addColumn = new form_StringControl(MainFormBase.MainFormIcon);
			
			addColumn.Strings = GetJustColumnNames ();
			int beforelength = addColumn.Strings.Length;
			int afterlength = 0;
			if (addColumn.ShowDialog() == DialogResult.OK)
			{
				// February 2012 - copying to clipboard to try to save data
				
				/*                
1. Don't bother copying, just build and store a string in memory
2. If *FEWER* Columns then we do not bother copying (too hard)
3. If *MORE* Columns then we need to add extra commas at end
4. We need to remove the Table1 and column name lines
 */
				
				
				
				afterlength = addColumn.Strings.Length;
				string sOldData = "";
				
				// we do not import table if we have removed columns
				if (afterlength >= beforelength)
				{
					
					try
					{
						
						sOldData = TableToString((DataTable)dataGrid1.DataSource, true, (afterlength - beforelength));
						lg.Instance.Line("TablePanel->ButtonEditCOlumns", ProblemType.MESSAGE, sOldData, Loud.CTRIVIAL);
					}
					catch (Exception ex)
					{
						NewMessage.Show(ex.ToString());
					}
				}
				else
				{
					lg.Instance.Line("TablePanel->EditColumns", ProblemType.MESSAGE, "we do not import table if we have removed columns");
				}
				ColumnDetails[] newColumns = new ColumnDetails[addColumn.Strings.Length];
				for (int i = 0 ;  i < addColumn.Strings.Length; i++)
				{
					newColumns[i] = new ColumnDetails(addColumn.Strings[i], defaultwidth);
				}
				
				
				//Cols = addColumn.Strings;
				dataGrid1.DataSource = null;
				dataGrid1.DataSource = BuildTableFromColumns(newColumns);
				
				
				if ("" != sOldData)
				{
					string[] list = sOldData.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
					try
					{
						// panelTable.ImportList(list);
						TableWrapper.ImportList(list, (DataTable)dataGrid1.DataSource);
					}
					catch (Exception ex)
					{
						NewMessage.Show(ex.ToString());
					}
				}
				
			}
		}

		/// <summary>
		/// allows editing of columns
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonEditColumns_Click(object sender, EventArgs e)
		{
			EditColumns ();
			

		}
		
		private void buttonJumpLink_Click (object sender, EventArgs e)
		{
			string notename = dataGrid1 [dataGrid1.CurrentCell.ColumnIndex, dataGrid1.CurrentCell.RowIndex].Value.ToString ();
			if (GoToNote != null) {
				GoToNote(notename);
			}
//			if (panelTable != null)
//			{
//				FollowLinkEvent eNew = new FollowLinkEvent(
//					
//					// (dataGrid1.DataSource as DataSet).Tables[0].Rows[dataGrid1.CurrentCell.RowNumber]
//					//  [dataGrid1.CurrentCell.ColumnNumber].ToString());
//					dataGrid1[dataGrid1.CurrentCell.RowNumber, dataGrid1.CurrentCell.ColumnNumber].ToString()
//					, dataGrid1.CurrentCell.RowNumber.ToString(), dataGrid1.CurrentCell.ColumnNumber.ToString());
//				
//				panelTable.OnFollowLink(eNew);
//			}
//			else if (appearance != null)
//			{
//				string page = dataGrid1[dataGrid1.CurrentCell.RowNumber, dataGrid1.CurrentCell.ColumnNumber].ToString();
//				appearance.GoToNoteOnPage(page);
//			}
		}
		
		
		
		/// <summary>
		/// will import a listof entries into the table
		/// 
		/// December 2008.
		/// 
		/// Changing this to be csv format
		///    r1c1, r1c2, r1c3
		///    r2c1, r2c2, r2c3
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImportListClick(object sender, EventArgs e)
		{
			TablePanel_Form_ImportList fList = new TablePanel_Form_ImportList();
			if (fList.ShowDialog() == DialogResult.OK)
			{


				TableWrapper.ImportList(fList.textList.Lines, (DataTable)dataGrid1.DataSource);

			}
			
		}

		public void Copy()
		{
			CopyToClipboard(dataGrid1, TableWrapper.TablePageTableName);

		}

		private void CopyToClipboardClick(object sender, EventArgs e)
		{
			Copy ();
			NewMessage.Show("Data copied in CSV format to clipboard.");
		}
		/// <summary>
		/// returns the appropriate table
		/// </summary>
		/// <param name="dg"></param>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public DataTable GetTable(DataGridView dg, string tableName)
		{

			return (DataTable)dg.DataSource;

			// Dec 31 2012 -- do we need all this complexity?
//			if (dg.DataSource != null)
//			{
//				DataTable dt = null;
//				if (dg.DataSource.GetType() == typeof(DataSet))
//				{
//					DataSet ds = (DataSet)dg.DataSource;
//					// need to use tableName when DataSet contains more than
//					// one table
//					if (ds.Tables.Contains(tableName))
//						dt = ds.Tables[tableName];
//				}
//				else
//					if (dg.DataSource.GetType() == typeof(DataTable))
//				{
//					dt = (DataTable)dg.DataSource;
//					if (dt.TableName != tableName)
//					{
//						dt.Clear();
//						dt = null;
//					}
//				}
//				return dt;
//			}
//			return null;
		}
		
		
		/// <summary>
		/// copies the datagrid to the clipboard
		/// </summary>
		/// <param name="dg"></param>
		/// <param name="tableName"></param>
		private void CopyToClipboard(DataGridView dg, string tableName)
		{
			DataTable dt = GetTable(dg, tableName);
			if (dt != null)
				Clipboard.SetDataObject(TableToString(dt), true);
			
			
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="JustData"></param>
		/// <param name="ExtraColumns"></param>
		/// <returns></returns>
		private string TableToString(DataTable dt)
		{
			return TableToString(dt, false, 0);
		}
		/// <summary>
		/// This method returns a string containing the data in a DataTable object. The first line contains the name of
		/// the table, the second line contains the name of each column separated by a tab character and the 
		/// remaining lines, one for each row in the table, contains the corresponding column data separated 
		/// by a tab character.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="JustData">if true will only return ROW Data</param>
		/// <param name="ExtraColumns">This integer value determines how many extra columsn to add... useful when adding columns February 2012</param>
		/// <returns></returns>
		private string TableToString(DataTable dt, bool JustData, int ExtraColumns)
		{
			string strData = "";
			if (false == JustData)
			{
				strData = dt.TableName + "\r\n";
			}
			
			string sep = string.Empty;
			if (dt.Rows.Count > 0)
			{
				if (false == JustData)
				{
					foreach (DataColumn c in dt.Columns)
					{
						if (c.DataType != typeof(System.Guid) &&
						    c.DataType != typeof(System.Byte[]))
						{
							strData += sep + c.ColumnName;
							sep = "\t";
						}
					}
				}
				strData += "\r\n";
				foreach (DataRow r in dt.Rows)
				{
					sep = string.Empty;
					
					int nColumnIdx = -1;
					foreach (DataColumn c in dt.Columns)
					{
						nColumnIdx++;
						if (c.DataType != typeof(System.Guid) &&
						    c.DataType != typeof(System.Byte[]))
						{
							if (!Convert.IsDBNull(r[c.ColumnName]))
							{
								//    if (nColumnIdx == dt.Columns.Count)
								//    {
								//    }
								//     else
								{
									if (r[c.ColumnName].ToString() != "")
									{
										strData += sep +
											r[c.ColumnName].ToString();
									}
									else
									{
										strData += sep + "";
									}
								}
							}
							else
							{
								
								strData += sep + "";
							}
							// changed this to comma seperated
							sep = ",";//"\t";
						}
					}
					// February 2012
					// Now add extra columns
					for (int i = 1; i <= ExtraColumns; i++)
					{
						strData += sep;
					}
					strData += Environment.NewLine;// "\r\n";
				}
			}
			else
				strData += "\r\n---> Table was empty!";
			return strData;
		}
		
		private void toggleSafeEditModeClick(object sender, EventArgs e)
		{
			dataGrid1.SafeEditMode = !dataGrid1.SafeEditMode;
		}
		
		private void toolStripButtonLoadTableFile_Click(object sender, EventArgs e)
		{
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					if (NewMessage.Show(Loc.Instance.GetString("Caution!"), Loc.Instance.GetString("If you continue you will lose any existing data."),
					                    MessageBoxButtons.YesNo, null) == DialogResult.Yes)
					{
						DataSet temp = new DataSet();
						temp.ReadXml(openFileDialog1.FileName);
						
						
						try
						{
							
							// trying somethign more complicated because I can't get deletions to work
							//                            note.appearance.dataSource = new DataSet("Table");
							//                          note.appearance.Columns = new string[4] { Header.Roll, Header.Result, Header.NextTable, Header.Modifier }; // setting columns creates the table
						dataGrid1.DataSource = null;
						dataGrid1.DataSource = temp.Tables[0];

							
							
						}
						catch (Exception ex)
						{
							NewMessage.Show(ex.ToString());
						}
				
						
					}
				}

		}
		
		private void toolStripButtonSaveTableToXML_Click(object sender, EventArgs e)
		{
			if (dataGrid1.DataSource != null)
			{
				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					DataSet temp = new DataSet();
					temp.Tables.Add((DataTable)dataGrid1.DataSource);
					temp.WriteXml(saveFileDialog1.FileName, XmlWriteMode.WriteSchema);
				}
			}
		}

		public void InsertRow()
		{
			//	int row = last_row;
			//((DataSet)dataGrid1.DataSource).Tables[0].DefaultView(
			if (dataGrid1.DataSource != null) {
				DataRow row = ((DataTable)dataGrid1.DataSource).NewRow ();
				lg.Instance.Line ("TablePanel.InsertRow", ProblemType.MESSAGE, String.Format ("Inserting at row = {0}", last_row));
				((DataTable)dataGrid1.DataSource).Rows.InsertAt (row, last_row);
				SetSaveNeeded (true);
			}
		}

		/// <summary>
		/// insert a row
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripButtonInsert_Click (object sender, EventArgs e)
		{
			InsertRow ();
		}
		
		/// <summary>
		/// will autonumber the current row, replacing contents
		/// </summary>
		private void DoAutoNumber ()
		{
			if (dataGrid1.DataSource != null) {
				// get current column
				int column = dataGrid1.CurrentCell.ColumnIndex;
				string sColumn = ((DataTable)dataGrid1.DataSource).Columns [column].ToString ();
			
				// prompt
				if (NewMessage.Show ("Caution!", String.Format ("Do you want to overwrite the values in column: {0}.", sColumn),
			                    MessageBoxButtons.YesNo, null) == DialogResult.Yes) {
					for (int i = 0; i < ((DataTable)dataGrid1.DataSource).Rows.Count; i++) {
						((DataTable)dataGrid1.DataSource).Rows [i] [column] = i + 1;
					}
				}
			}
			SetSaveNeeded(true);
		}
		
		private void AutoNumberClick(object sender, EventArgs e)
		{
			DoAutoNumber();
		}
	


	}
}

