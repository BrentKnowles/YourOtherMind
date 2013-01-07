using System;
using CoreUtilities;
using Layout;
using LayoutPanels;
using CoreUtilities.Tables;
using System.Windows.Forms;
namespace YOM2013
{
	/// <summary>
	/// Static class is able to create standard Layouts.
	/// 
	/// Primary purpose is to resupply the System Layout (essential) if damaged or user requests a reset. 
	/// -- Creates all the necessary user-setting tables too.
	/// </summary>
	public static class DefaultLayouts
	{
		public static LayoutPanelBase CreateASystemLayout()
		{
			LayoutPanel SystemLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK, true);
			SystemLayout.NewLayout ("system" ,false);

			//SystemLayout.Parent = parent;
			SystemLayout.Visible = true;

		

			NoteDataXML_Panel sidedockpanel = new NoteDataXML_Panel(800,300);
			SystemLayout.AddNote(sidedockpanel);
			sidedockpanel.CreateParent(SystemLayout);


			NoteDataXML_NoteList list = new NoteDataXML_NoteList();
			sidedockpanel.AddNote(list);
			list.Mode = NoteDataXML_NoteList.Modes.LAYOUTS;


			list.Dock = DockStyle.Fill;

			sidedockpanel.BringToFront();
			sidedockpanel.Dock = DockStyle.Left;



			NoteDataXML_Table randomTables = new NoteDataXML_Table();
			randomTables.Caption = LayoutPanel.SYSTEM_RANDOM_TABLES;
			randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), new appframe.ColumnDetails("tables",100)};
		
			SystemLayout.SaveLayout ();

			sidedockpanel.AddNote(randomTables);
			randomTables.AddRow(new object[2]{"1", "example|colors"});
			randomTables.AddRow(new object[2]{"2", "example|colorPROMPTS"});

			SystemLayout.SetName (Loc.Instance.GetString("system"));
			SystemLayout.SaveLayout ();
			list.Refresh();
			return SystemLayout;
		}
		/// <summary>
		/// Creates the example layout. For new users.
		/// </summary>
		/// <returns>
		/// The example layout.
		/// </returns>
		/// <param name='parent'>
		/// Parent.
		/// </param>
		public static LayoutPanelBase CreateExampleLayout (Control parent)
		{
			
			LayoutPanel exampleLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK, true);
			exampleLayout.NewLayout ("example" ,false);

			exampleLayout.Parent = parent;
			exampleLayout.Visible = true;
			
			const int columns = 3;
			
			NoteDataXML_Table randomTables = new NoteDataXML_Table();
			randomTables.Columns = new appframe.ColumnDetails[columns]{new appframe.ColumnDetails(TableWrapper.Roll,100), new appframe.ColumnDetails(TableWrapper.Result,100)
			,new appframe.ColumnDetails(TableWrapper.NextTable, 100)};
			
			exampleLayout.SaveLayout ();
		
			exampleLayout.AddNote(randomTables);
			randomTables.Caption = "colors";
			randomTables.TableCaption = "COLORS: ";
			randomTables.AddRow(new object[columns]{"1", "red",""});
			randomTables.AddRow(new object[columns]{"2", "blue",""});
			randomTables.AddRow(new object[columns]{"3", "yellow",""});

			// 2nd table
			randomTables = new NoteDataXML_Table();
			randomTables.TableCaption = "COLORS AS PROMPT: ";
			randomTables.Columns = new appframe.ColumnDetails[columns]{new appframe.ColumnDetails(TableWrapper.Roll,100), 
				new appframe.ColumnDetails(TableWrapper.Result,100),new appframe.ColumnDetails(TableWrapper.NextTable, 100)};
			randomTables.Caption = "colorprompts";
			randomTables.AddRow(new object[columns]{"1", "red",""});
			randomTables.AddRow(new object[columns]{"2", "blue",""});
			randomTables.AddRow(new object[columns]{"3", "yellow",""});
			exampleLayout.AddNote(randomTables);

			
			exampleLayout.SetName (Loc.Instance.GetString("example"));
			exampleLayout.SaveLayout ();

			return exampleLayout;
		}
	}
}
