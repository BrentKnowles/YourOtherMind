using System;
using CoreUtilities;
using System.Windows.Forms;
using appframe;
using System.Xml.Serialization;
using System.Data;
using System.ComponentModel;
namespace Layout
{
	public class NoteDataXML_Table : NoteDataXML
	{
		#region constants
		public override int defaultHeight { get { return 400; } }
		public override int defaultWidth { get { return 700; } }


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
			public ColumnDetails()
			{
			}
		}

		#endregion

		#region formcontrols
		TablePanel Table= null;
		#endregion

		#region variables
		protected DataTable mdataSource=null;
		[Browsable(false)]
		public DataTable dataSource {
			get {
				return mdataSource;
				
			}
			set {
				mdataSource = value;
				// The CALLER will also have to make sure to call the Update routine so note is recreated 
				// with the new datasource
			}
		}
		// September 2011 - Data Source for DataTable
		protected  ColumnDetails[] mColumns;
		/// <summary>
		/// This is the string list of columns
		/// Remember there is always one "automatic" column
		/// for handling the row numbers
		/// 
		/// When columns are changed we need to build the datatable
		/// </summary>
		[Browsable(false)]
		public ColumnDetails[] Columns
		{
			get { return mColumns; }
			set
			{
				if (value == null) return;
				mColumns = value;
				
				// if we keep this without the condition, it blanks the table
				// I honestly do not understand how this worked in previous version of YOM
				if (dataSource == null)
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

					dataSource = table;//new DataTable();

//					dataSource = new DataSet("Table");
//					
//					if (dataSource.Tables != null)
//					{
//						try
//						{
//							dataSource.Tables.Clear();
//						}
//						catch (Exception)
//						{
//							
//						}
//					}
//					dataSource.Tables.Add(table);

				}

				
			}
		}

		#endregion

		public NoteDataXML_Table () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Table");
		}

		public NoteDataXML_Table(int height, int width) : base(height, width)
		{
			Caption = Loc.Instance.Cat.GetString("Table");

			// This constructor is called ONLY when the note is first being created (not loaded)
			// so we create a default datasource here and now.
			//dataSource = new DataSet("Table");
			//dataSource = new DataTable(); // A new datasource is already being created when column default is made.
			Columns = new ColumnDetails[4] { 
				new ColumnDetails("Roll",100), new ColumnDetails("Result",100), new ColumnDetails("NextTable", 100),
				new ColumnDetails("Modifier", 100)};

		}

		public override void Save ()
		{
			base.Save ();
			if (Table != null) {

				// Does not appear we need to store anythign extra

				//dataSource = (DataTable)Table.dataGrid1.DataSource;
				// TODO: Save table stuff
//				DataSet ds = new DataSet();
//				ds.Tables.Add((DataTable)Table.dataGrid1.DataSource);
//				dataSource = ds;

			}
		}
		public override void CreateParent (LayoutPanelBase Layout)
		{



			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;

			Table = new TablePanel ();
			Table.Parent = Parent;
			Table.Dock = DockStyle.Fill;
			Table.BringToFront ();
			// TODO: Load table data richBox.Rtf = this.Data1;
			if (null != dataSource) {
				Table.dataGrid1.DataSource = dataSource;
				Table.dataGrid1.CellBeginEdit+= HandleCellBeginEdit;
				//Table.dataGrid1.DataSource = dataSource;// This works but then we lose the hook.Tables [0];
				//Table.dataGrid1.DataMember = "Table";

			} else {
				NewMessage.Show (Loc.Instance.GetString("You have a null table which should not happen"));
			}

			//Table.dataGrid1.NavigateTo(0, "Table1"); // same as Table1 referenced by column styles
			//Table.appearance = this;
				
		

			// TODO: Set up table handlersrichBox.TextChanged+= HandleTextChanged;
			
		}

		void HandleCellBeginEdit (object sender, DataGridViewCellCancelEventArgs e)
		{
			SetSaveRequired(true);
		}
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Table");
		}
		

		public void RandomTable()
		{
			//TODO: figure out if new random table system will function like this
			/*1. Called from own page
			 *2. Called from other page
			 *3. Called from the TablePanel itself, the 'preview function'
			 */
		}
		
	}
}

