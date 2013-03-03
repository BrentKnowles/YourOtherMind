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
		public static void CreateASystemLayout(Control parent, ContextMenuStrip textEditorContextStrip)
		{
			LayoutPanel SystemLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK, true);
			SystemLayout.NewLayout ("system" ,false, textEditorContextStrip);
			SystemLayout.BackColor = System.Drawing.Color.Wheat;
			SystemLayout.SetName (Loc.Instance.GetString("system"));
			SystemLayout.Parent = parent;
			SystemLayout.Visible = true;

		

			NoteDataXML_Panel sidedockpanel = new NoteDataXML_Panel(800,300);
			sidedockpanel.GuidForNote = "system_sidedock";
			SystemLayout.AddNote(sidedockpanel);
			sidedockpanel.CreateParent(SystemLayout);


			NoteDataXML_NoteList list = new NoteDataXML_NoteList();
			sidedockpanel.AddNote(list);
			list.CreateParent(sidedockpanel.GetPanelsLayout());
			list.Mode = NoteDataXML_NoteList.Modes.LAYOUTS;


			list.Dock = DockStyle.Fill;

			sidedockpanel.BringToFrontAndShow();
			sidedockpanel.Dock = DockStyle.Left;

			//
			//------Subpanel
			//
			NoteDataXML_Panel subpanel = new NoteDataXML_Panel(100,100);

			//subpanel.Save();
			//sidedockpanel.Save();
			//* important
			subpanel.GuidForNote = "tables";
			subpanel.Caption = Loc.Instance.GetString("Tables");


			sidedockpanel.AddNote(subpanel);
			subpanel.CreateParent(sidedockpanel.GetPanelsLayout());

		

			NoteDataXML_Table randomTables = new NoteDataXML_Table();
			randomTables.Caption = LayoutDetails.SYSTEM_RANDOM_TABLES;
			randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), new appframe.ColumnDetails("tables",100)};
		


			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout());


			randomTables.AddRow(new object[2]{"1", "example|colors"});
			randomTables.AddRow(new object[2]{"2", "example|colorPROMPTS"});
			SystemLayout.SaveLayout ();

			randomTables = new NoteDataXML_Table();
			randomTables.Caption = LayoutDetails.SYSTEM_NOTEBOOKS;
			randomTables.Columns = new appframe.ColumnDetails[3]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("notebooks",100),
				new appframe.ColumnDetails("sections",200)};

		
			subpanel.AddNote(randomTables);
			randomTables.CreateParent (subpanel.GetPanelsLayout ());

			randomTables.AddRow(new object[3]{"1", Loc.Instance.GetString("Writing"), Loc.Instance.GetString("All|Advice|Characters|Markets|Projects|Scenes")});
			randomTables.AddRow(new object[3]{"2", Loc.Instance.GetString("Research"), Loc.Instance.GetString("All|Historical|Science")});
			randomTables.AddRow(new object[3]{"3", Loc.Instance.GetString("All"), Loc.Instance.GetString("All")});

			///
			/// -- STATUS
			/// 
			randomTables = new NoteDataXML_Table();
			randomTables.Caption = LayoutDetails.SYSTEM_STATUS;
			randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("status",100)};

			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout());

			randomTables.AddRow(new object[2]{"1", Loc.Instance.GetString("0 Not Started")});
			randomTables.AddRow(new object[2]{"2", Loc.Instance.GetString("1 Planning")});
			randomTables.AddRow(new object[2]{"3", Loc.Instance.GetString("2 Writing")});
			randomTables.AddRow(new object[2]{"4", Loc.Instance.GetString("3 Rewriting")});
			randomTables.AddRow(new object[2]{"5", Loc.Instance.GetString("4 Complete")});
			randomTables.AddRow(new object[2]{"6", Loc.Instance.GetString("5 Accepted")});
			randomTables.AddRow(new object[2]{"7", Loc.Instance.GetString("6 Published")});
			randomTables.AddRow(new object[2]{"8", Loc.Instance.GetString("7 Republished on personal site")});
			randomTables.AddRow(new object[2]{"9", Loc.Instance.GetString("8 Selfpublished")});
		//	NewMessage.Show (randomTables.RowCount().ToString ());
			randomTables.AddRow(new object[2]{"10", Loc.Instance.GetString("9 Retired")});

		//	NewMessage.Show (randomTables.RowCount().ToString ());


			///
			/// -- SUBTYPES
			/// 
			/// 
			/// 
			randomTables = new NoteDataXML_Table();
			randomTables.Caption = LayoutDetails.SYSTEM_SUBTYPE;
			randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("subtype",100)};
			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout());

			randomTables.AddRow(new object[2]{"1", Loc.Instance.GetString("Article")});
			randomTables.AddRow(new object[2]{"2", Loc.Instance.GetString("Idea")});
			randomTables.AddRow(new object[2]{"3", Loc.Instance.GetString("Novel")});
			randomTables.AddRow(new object[2]{"4", Loc.Instance.GetString("Play")});
			randomTables.AddRow(new object[2]{"5", Loc.Instance.GetString("Story")});

			///
			/// -- KEYWORDS
			/// 
			/// 
			/// 
			randomTables = new NoteDataXML_Table();
			randomTables.Caption = LayoutDetails.SYSTEM_KEYWORDS;
			randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("keyword",100)};
			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout ());

			randomTables.AddRow(new object[2]{"1", Loc.Instance.GetString("Brainstorm")});
			randomTables.AddRow(new object[2]{"2", Loc.Instance.GetString("Horror")});
			randomTables.AddRow(new object[2]{"3", Loc.Instance.GetString("Fantasy")});
			randomTables.AddRow(new object[2]{"4", Loc.Instance.GetString("SciFi")});
			randomTables.AddRow(new object[2]{"5", Loc.Instance.GetString("WhatIf?")});


			//HACK: Realistically these next tables are required by AddIns, and hence should be
			//  be created by those AddIns.

			///
			/// -- WORKLOG
			/// 
			/// 
			/// 
			randomTables = new NoteDataXML_Table();
			randomTables.Caption = LayoutDetails.SYSTEM_WORKLOGCATEGORY;
			randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("category",100)};
			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout ());
			
			randomTables.AddRow(new object[2]{"1", Loc.Instance.GetString("Writing")});
			randomTables.AddRow(new object[2]{"2", Loc.Instance.GetString("Editing")});
			randomTables.AddRow(new object[2]{"3", Loc.Instance.GetString("Planning")});


			///
			/// -- Grammar
			/// 
			/// 
			/// 
			randomTables = new NoteDataXML_Table();
			randomTables.Caption = LayoutDetails.SYSTEM_GRAMMAR;
			randomTables.Columns = new appframe.ColumnDetails[4]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("pattern",100), new appframe.ColumnDetails("advice",100), new appframe.ColumnDetails("overused",100)};
			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout ());

			randomTables.AddRow(new object[4]{
				"1", "1.0", @"The first row of this table is a version number. Feel free to edit it when major changes are made to this list. On each Layout you can record the last grammar version you have checked it against.", "0"}
			);

			randomTables.AddRow(new object[4]{
				"2", "Among", @"When more than two things or persons are involved, among is usually called for.", "0"}
			);
			randomTables.AddRow(new object[4]{
				"3", "As to whether", @"Whether is sufficient.", "1"}
			);



			SystemLayout.SaveLayout ();
			list.Refresh();
			SystemLayout.Dispose ();
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
		public static void CreateExampleLayout (Control parent, ContextMenuStrip TextEditMenuStrip)
		{
			
			LayoutPanel exampleLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK, true);
			exampleLayout.NewLayout ("example" ,false, TextEditMenuStrip);

			exampleLayout.Parent = parent;
			exampleLayout.Visible = true;


			exampleLayout.SetName (Loc.Instance.GetString("example"));
		


			const int columns = 3;
			
			NoteDataXML_Table randomTables = new NoteDataXML_Table();
			randomTables.Columns = new appframe.ColumnDetails[columns]{new appframe.ColumnDetails(TableWrapper.Roll,100), new appframe.ColumnDetails(TableWrapper.Result,100)
			,new appframe.ColumnDetails(TableWrapper.NextTable, 100)};
			
			exampleLayout.SaveLayout ();
		
			exampleLayout.AddNote(randomTables);
			randomTables.CreateParent(exampleLayout);

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
			randomTables.CreateParent(exampleLayout);
			

			exampleLayout.SaveLayout ();

			exampleLayout.Dispose();
		}
	}
}

