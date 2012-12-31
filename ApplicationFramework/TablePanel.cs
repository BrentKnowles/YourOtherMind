using System;
using System.Windows.Forms;
//using System.ComponentModel;
using System.Data;
using CoreUtilities;
using System.Drawing;

namespace appframe
{

	public class DataGridNoKeyPressIrritation : DataGridView
	{
		// replace with real code
		public bool SafeEditMode;

	}
	/// <summary>
	/// Column details.
	/// This is stored in the NoteDataXML_Table class. We use it here to build tables when columsn reorganized
	/// </summary>
	public class ColumnDetails
	{
		private string columnName;
		private int columnWidth;
		public string ColumnName {
			get { return columnName; }
			set { columnName = value;}
		}
		public int ColumnWidth {
			get { return columnWidth;}
			set { columnWidth = value;}
		}
		public ColumnDetails(string name, int width)
		{
			columnName = name;
			columnWidth = width;
		}
		// need this for serializing
		public ColumnDetails()
		{
		}
	}
	public class classPageTable
	{
		public DataSet dataSource;
	}

	/// <summary>
	/// Table panel.
	/// </summary>
	public class TablePanel : Panel
	{

		public TablePanel (DataTable _dataSource, Func<int> _tableChanged, ColumnDetails[] incomingColumns)
		{
			InitializeComponent ();
			if (null != _dataSource) {

				dataGrid1.DataSource = _dataSource;
				dataGrid1.DataBindingComplete+= HandleDataBindingComplete;
				dataGrid1.CellBeginEdit += HandleCellBeginEdit;
			}
			TableChanged = _tableChanged;
			IncomingColumns = incomingColumns;



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
			if (dataGrid1.DataSource != null) {
				// set column widths
				for (int i = 0 ; i < IncomingColumns.Length ; i++) {
					dataGrid1.Columns[i].Width = IncomingColumns[i].ColumnWidth;
				}
				
			}
		}



		void HandleCellBeginEdit (object sender, DataGridViewCellCancelEventArgs e)
		{
			if (null != TableChanged) {
				TableChanged();
			}
		}



		#region constant
		public const string TablePageTableName= "Table";
		public const int defaultwidth = 100;
		#endregion
		#region variables
		// this is the object holding all the table details
		public classPageTable panelTable;

		// delegate: called whenever the table changes
		Func<int> TableChanged = null;

		//used to load default settings
		ColumnDetails[] IncomingColumns= null;

		#endregion

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
			this.buttonPreview.ToolTipText = "Preview";
			this.buttonPreview.Click += new System.EventHandler(this.previewclick);
			// 
			// buttonEditColumns
			// 
			this.buttonEditColumns.Image =  FileUtils.GetImage_ForDLL("table_edit.png");
			this.buttonEditColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonEditColumns.Name = "buttonEditColumns";
			this.buttonEditColumns.Size = new System.Drawing.Size(23, 22);
			this.buttonEditColumns.ToolTipText = "Edit Columns";
			this.buttonEditColumns.Click += new System.EventHandler(this.buttonEditColumns_Click);
			// 
			// toolStripButton1
			// 
			//this.toolStripButton1.Image = global::Worgan2006.Header.script_edit;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.ToolTipText = "Import List";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_3);
			// 
			// toolStripButton2
			// 
			//this.toolStripButton2.Image = global::Worgan2006.Header.page_copy;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.ToolTipText = "Copy To Clipboard";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// buttonJumpLink
			// 
			//this.buttonJumpLink.Image = global::Worgan2006.Header.link_go;
			this.buttonJumpLink.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonJumpLink.Name = "buttonJumpLink";
			this.buttonJumpLink.Size = new System.Drawing.Size(23, 22);
			this.buttonJumpLink.ToolTipText = "Follow Link";
			this.buttonJumpLink.Click += new System.EventHandler(this.buttonJumpLink_Click);
			// 
			// toolStripButtonEditMode
			// 
			this.toolStripButtonEditMode.Checked = true;
			this.toolStripButtonEditMode.CheckOnClick = true;
			this.toolStripButtonEditMode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.toolStripButtonEditMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			//this.toolStripButtonEditMode.Image = global::Worgan2006.Properties.Resources.tag_blue;
			this.toolStripButtonEditMode.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonEditMode.Name = "toolStripButtonEditMode";
			this.toolStripButtonEditMode.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonEditMode.Text = "Toggle Safe Edit Mode";
			this.toolStripButtonEditMode.Click += new System.EventHandler(this.toolStripButtonEditMode_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonLoadTableFile
			// 
			this.toolStripButtonLoadTableFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			//this.toolStripButtonLoadTableFile.Image = global::Worgan2006.Properties.Resources.table_add;
			this.toolStripButtonLoadTableFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonLoadTableFile.Name = "toolStripButtonLoadTableFile";
			this.toolStripButtonLoadTableFile.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonLoadTableFile.Text = "toolStripButton3";
			this.toolStripButtonLoadTableFile.ToolTipText = "Load a xml based datatable";
			this.toolStripButtonLoadTableFile.Click += new System.EventHandler(this.toolStripButtonLoadTableFile_Click);
			// 
			// toolStripButtonSaveTableToXML
			// 
			this.toolStripButtonSaveTableToXML.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			//this.toolStripButtonSaveTableToXML.Image = global::Worgan2006.Properties.Resources.table_save;
			this.toolStripButtonSaveTableToXML.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSaveTableToXML.Name = "toolStripButtonSaveTableToXML";
			this.toolStripButtonSaveTableToXML.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonSaveTableToXML.Text = "toolStripButton3";
			this.toolStripButtonSaveTableToXML.ToolTipText = "Save table to an XML file";
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
			//this.toolStripButtonInsert.Image = global::Worgan2006.Properties.Resources.basket_add;
			this.toolStripButtonInsert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonInsert.Name = "toolStripButtonInsert";
			this.toolStripButtonInsert.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonInsert.Text = "toolStripButton3";
			this.toolStripButtonInsert.ToolTipText = "Insert row";
			this.toolStripButtonInsert.Click += new System.EventHandler(this.toolStripButtonInsert_Click);
			// 
			// toolStripButtonNumber
			// 
			this.toolStripButtonNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			//this.toolStripButtonNumber.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNumber.Image")));
			this.toolStripButtonNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonNumber.Name = "toolStripButtonNumber";
			this.toolStripButtonNumber.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonNumber.Text = "Auto number";
			this.toolStripButtonNumber.ToolTipText = "Auto number rows?";
			this.toolStripButtonNumber.Click += new System.EventHandler(this.toolStripButton3_Click);
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
				/*TODO: Hook back up
				if (panelTable != null)
					panelTable.Save(saveFileDialog1.FileName);
*/
			}
		}

		/// <summary>
		/// when a table is passed in, it is displayed
		/// </summary>
		/// <param name="newTable"></param>
		public void OpenTable(classPageTable newTable)
		{
//			if (newTable != null)
//			{
//				dataGrid1.DataSource = newTable.dataSource;
//				dataGrid1.NavigateTo(0, "Table1"); // same as Table1 referenced by column styles
//				//  propertyGrid1.SelectedObject = newTable;
//				panelTable = newTable;
//				panelTable.PageSaved = false;
//			}
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
		
		private void previewclick(object sender, EventArgs e)
		{
			NewMessage.Show ("implement me");
//			string sResult = "";
//			
//			if (panelTable != null)
//			{
//				sResult = panelTable.GenerateAllResults();
//			}
//			else if (appearance != null && appearance.virtualDesktop != null && appearance.virtualDesktop.Tag != null)
//			{
//				string sNextTable = "";
//				sResult = TableWrapper.GenerateAllResults(ref sNextTable, ((DataSet)dataGrid1.DataSource).Tables[0], ((DataSet)dataGrid1.DataSource), "Current", (classPageVisual)appearance.virtualDesktop.Tag, appearance.Caption);
//			}
//			
//			if (sResult != Header.Blank)
//			{
//				richTextBox1.Visible = true;
//				label1.Visible = true;
//				panel1.Visible = true;
//				richTextBox1.Text = "";
//				sResult = sResult.Trim();
//				if (sResult != null && sResult != "")
//				{
//					sResult = sResult + "\n"; // the trim removes the \n
//					richTextBox1.AppendText(sResult);
//					
//					
//				}
//			}
		}
		
		private int last_row = 0;
		/// <summary>
		/// whenever data changes set page saved to false
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
		{
//			last_row = dataGrid1.CurrentCell.RowNumber; // october 2012 -- find I need to keep track of the index myself?
//			
//			
//			if (panelTable != null)
//			{
//				panelTable.PageSaved = false;
//			}
//			else if (appearance != null)
//			{
//				appearance.NeedsSave();
//			}
		}

	//	public Appearance appearance = null;
		/// <summary>
		/// September 2011
		/// A wrapper for column rquestss in case I am using this from the appearance system
		/// </summary>
		private ColumnDetails[] Columns {
			get {

				ColumnDetails[] columns = new ColumnDetails[dataGrid1.Columns.Count];
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
		/// <summary>
		/// Gets the columns.
		/// 
		/// Called when the note calls Save()
		/// </summary>
		/// <returns>
		/// The columns.
		/// </returns>
		public ColumnDetails[] GetColumns()
		{
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
			table.TableName = "Table";
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
		
		/// <summary>
		/// allows editing of columns
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonEditColumns_Click(object sender, EventArgs e)
		{

			
			// changing columns can destroy data
			// pick columns
			form_StringControl addColumn = new form_StringControl();
			
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
						//DataTable dt = GetTable(dataGrid1, TablePageTableName);
					//	sOldData = TableToString(dt, true, (afterlength - beforelength));
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
//				Columns = new ColumnDetails[addColumn.Strings.Length];
//				for (int i = 0 ;  i < addColumn.Strings.Length; i++)
//				{
//					Columns[i] = new ColumnDetails(addColumn.Strings[i], defaultwidth);
//				}


				//Cols = addColumn.Strings;
				dataGrid1.DataSource = null;
				dataGrid1.DataSource = BuildTableFromColumns(Columns);
				//dataGrid1.NavigateTo(0, Header.TablePageTableName);
				//dataGrid1.Refresh();
//				if (panelTable != null)
//				{
//					panelTable.OnEditColumns(new CustomEventArgs(""));
//				}
//				if ("" != sOldData)
//				{
//					string[] list = sOldData.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
//					try
//					{
//						// panelTable.ImportList(list);
//						TableWrapper.ImportList(list, appearance.dataSource.Tables[0], appearance.dataSource);
//					}
//					catch (Exception ex)
//					{
//						NewMessage.Show(ex.ToString());
//					}
//				}
				
			}
		}
		
		private void buttonJumpLink_Click(object sender, EventArgs e)
		{
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
		private void toolStripButton1_Click_3(object sender, EventArgs e)
		{
//			fImportList fList = new fImportList();
//			if (fList.ShowDialog() == DialogResult.OK)
//			{
//				// go through each text entry and add a new row to the result column
//				if (panelTable != null)
//				{
//					panelTable.ImportList(fList.textList.Lines);
//				}
//				else if (appearance != null)
//				{
//					TableWrapper.ImportList(fList.textList.Lines, appearance.dataSource.Tables[0], appearance.dataSource);
//				}
//			}
			
		}
		
		private void toolStripButton2_Click(object sender, EventArgs e)
		{
//			CopyToClipboard(dataGrid1, Header.TablePageTableName);
//			NewMessage.Show("Data copied in CSV format to clipboard.");
		}
		/// <summary>
		/// returns the appropriate table
		/// </summary>
		/// <param name="dg"></param>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public DataTable GetTable(DataGridView dg, string tableName)
		{
			if (dg.DataSource != null)
			{
				DataTable dt = null;
				if (dg.DataSource.GetType() == typeof(DataSet))
				{
					DataSet ds = (DataSet)dg.DataSource;
					// need to use tableName when DataSet contains more than
					// one table
					if (ds.Tables.Contains(tableName))
						dt = ds.Tables[tableName];
				}
				else
					if (dg.DataSource.GetType() == typeof(DataTable))
				{
					dt = (DataTable)dg.DataSource;
					if (dt.TableName != tableName)
					{
						dt.Clear();
						dt = null;
					}
				}
				return dt;
			}
			return null;
		}
		
		
		/// <summary>
		/// copies the datagrid to the clipboard
		/// </summary>
		/// <param name="dg"></param>
		/// <param name="tableName"></param>
		public void CopyToClipboard(DataGridView dg, string tableName)
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
		
		private void toolStripButtonEditMode_Click(object sender, EventArgs e)
		{
			dataGrid1.SafeEditMode = !dataGrid1.SafeEditMode;
		}
		
		private void toolStripButtonLoadTableFile_Click(object sender, EventArgs e)
		{
//			if (appearance != null)
//			{
//				if (openFileDialog1.ShowDialog() == DialogResult.OK)
//				{
//					if (NewMessage.Show("Caution!", "If you continue you will lose any existing data.",
//					                    MessageBoxButtons.YesNo, null) == DialogResult.Yes)
//					{
//						DataSet temp = new DataSet();
//						temp.ReadXml(openFileDialog1.FileName);
//						//((DataSet)dataGrid1.DataSource).Tables[0].TableName = temp.Tables[0].TableName;
//						// ((DataSet)dataGrid1.DataSource).Tables[0].Columns = temp.Tables[0].Columns;
//						// ((DataSet)dataGrid1.DataSource).Tables[0].Rows = temp.Tables[0].Rows; ((DataSet)dataGrid1.DataSource).Tables.
//						
//						//                    ((DataSet)dataGrid1.DataSource).Tables.Clear();
//						//((DataSet)dataGrid1.DataSource).Tables.Remove(((DataSet)dataGrid1.DataSource).Tables[0]);
//						//((DataSet)dataGrid1.DataSource).Tables[0].TableName = "Table1";
//						
//						try
//						{
//							
//							// trying somethign more complicated because I can't get deletions to work
//							//                            note.appearance.dataSource = new DataSet("Table");
//							//                          note.appearance.Columns = new string[4] { Header.Roll, Header.Result, Header.NextTable, Header.Modifier }; // setting columns creates the table
//							appearance.dataSource = new DataSet("Table");
//							appearance.dataSource.Tables.Add(temp.Tables[0].Copy());
//							// not tried yet
//							// ** Feb 11 2012 - none of the deletions worked
//							//lookfortablebyname andrename
//							/* while ((((DataSet)dataGrid1.DataSource).Tables).Count > 0)
//                            {
//                                DataTable table = ((DataSet)dataGrid1.DataSource).Tables[0];
//                                if (((DataSet)dataGrid1.DataSource).Tables.CanRemove(table))
//                                {
//                                    ((DataSet)dataGrid1.DataSource).Tables.Remove(table);
//                                }
//                            }
//                            */
//							// *1 appearance.dataSource.Tables[0].Clear();
//							// *1 appearance.dataSource.Tables[0].TableName = "tableold" + appearance.dataSource.Tables.Count.ToString();
//							//appearance.dataSource.Clear();
//							//appearance.dataSource.Tables.Remove(appearance.dataSource.Tables[0]); // this wokred but required a reload
//							// appearance.dataSource.Tables.Remove("Table1");
//							//appearance.dataSource.Tables.Clear();
//							
//						}
//						catch (Exception ex)
//						{
//							MessageBox.Show(ex.ToString());
//						}
//						//  appearance.dataSource.Tables.Add(temp.Tables[0].Copy());
//						// this kind of worked but not really
//						//dataGrid1.DataSource = new DataSet();
//						//((DataSet)dataGrid1.DataSource).Tables.Add(temp.Tables[0].Copy());
//						
//					}
//				}
//			}
		}
		
		private void toolStripButtonSaveTableToXML_Click(object sender, EventArgs e)
		{
//			if (appearance.dataSource != null)
//			{
//				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
//				{
//					((DataSet)appearance.dataSource).WriteXml(saveFileDialog1.FileName, XmlWriteMode.WriteSchema);
//				}
//			}
		}
		/// <summary>
		/// insert a row
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripButtonInsert_Click(object sender, EventArgs e)
		{
			int row = last_row;
			//((DataSet)dataGrid1.DataSource).Tables[0].DefaultView(
			((DataSet)dataGrid1.DataSource).Tables[0].Rows.InsertAt(((DataSet)dataGrid1.DataSource).Tables[0].NewRow(), last_row);
		}
		
		/// <summary>
		/// will autonumber the current row, replacing contents
		/// </summary>
		private void DoAutoNumber()
		{
			
			// get current column
//			int column = dataGrid1.CurrentCell.ColumnNumber;
//			string sColumn = ((DataSet)dataGrid1.DataSource).Tables[0].Columns[column].ToString();
//			
//			// prompt
//			if (NewMessage.Show("Caution!", String.Format("Do you want to overwrite the values in column: {0}.", sColumn),
//			                    MessageBoxButtons.YesNo, null) == DialogResult.Yes)
//			{
//				for (int i = 0; i < ((DataSet)dataGrid1.DataSource).Tables[0].Rows.Count; i++)
//				{
//					((DataSet)dataGrid1.DataSource).Tables[0].Rows[i][column] = i + 1;
//				}
//			}
			
		}
		
		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			DoAutoNumber();
		}

	}
}

