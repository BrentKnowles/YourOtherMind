// DefaultLayouts.cs
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
			sidedockpanel.GuidForNote = LayoutDetails.SIDEDOCK; //"system_sidedock";
			SystemLayout.AddNote(sidedockpanel);
			sidedockpanel.CreateParent(SystemLayout);



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

		

			NoteDataXML_Table randomTables = new NoteDataXML_Table(100, 100,new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), new appframe.ColumnDetails("tables",100)} );
			randomTables.Caption = LayoutDetails.SYSTEM_RANDOM_TABLES;
		//	randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), new appframe.ColumnDetails("tables",100)};
		


			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout());


			randomTables.AddRow(new object[2]{"1", "example|colors"});
			randomTables.AddRow(new object[2]{"2", "example|colorPROMPTS"});
			SystemLayout.SaveLayout ();

			randomTables = new NoteDataXML_Table(100, 100, new appframe.ColumnDetails[3]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("notebooks",100),
				new appframe.ColumnDetails("sections",200)});
			randomTables.Caption = LayoutDetails.SYSTEM_NOTEBOOKS;
//			randomTables.Columns = new appframe.ColumnDetails[3]{new appframe.ColumnDetails("id",100), 
//				new appframe.ColumnDetails("notebooks",100),
//				new appframe.ColumnDetails("sections",200)};

		
			subpanel.AddNote(randomTables);
			randomTables.CreateParent (subpanel.GetPanelsLayout ());

			randomTables.AddRow(new object[3]{"1", Loc.Instance.GetString("Writing"), Loc.Instance.GetString("All|Advice|Characters|Markets|Projects|Scenes")});
			randomTables.AddRow(new object[3]{"2", Loc.Instance.GetString("Research"), Loc.Instance.GetString("All|Historical|Science")});
			randomTables.AddRow(new object[3]{"3", Loc.Instance.GetString("All"), Loc.Instance.GetString("All")});

			///
			/// -- STATUS
			/// 
			randomTables = new NoteDataXML_Table(100, 100, new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("status",100)});
			randomTables.Caption = LayoutDetails.SYSTEM_STATUS;
		//	randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
		//		new appframe.ColumnDetails("status",100)};

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
			randomTables = new NoteDataXML_Table(100, 100, new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("subtype",100)});
			randomTables.Caption = LayoutDetails.SYSTEM_SUBTYPE;
	//		randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
		//		new appframe.ColumnDetails("subtype",100)};
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
			randomTables = new NoteDataXML_Table(100, 100,  new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("keyword",100)});
			randomTables.Caption = LayoutDetails.SYSTEM_KEYWORDS;
		//	randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
		//		new appframe.ColumnDetails("keyword",100)};
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
			randomTables = new NoteDataXML_Table(100, 100, new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("category",100)});
			randomTables.Caption = LayoutDetails.SYSTEM_WORKLOGCATEGORY;
		//	randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
		//		new appframe.ColumnDetails("category",100)};
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
			randomTables = new NoteDataXML_Table(100, 100 , new appframe.ColumnDetails[4]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("pattern",100), new appframe.ColumnDetails("advice",100), new appframe.ColumnDetails("overused",100)});
			randomTables.Caption = LayoutDetails.SYSTEM_GRAMMAR;
		//	randomTables.Columns = new appframe.ColumnDetails[4]{new appframe.ColumnDetails("id",100), 
		//		new appframe.ColumnDetails("pattern",100), new appframe.ColumnDetails("advice",100), new appframe.ColumnDetails("overused",100)};
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

			
			///
			/// -- Queries
			/// 
			/// 
			/// 
			randomTables = new NoteDataXML_Table(100, 100, new appframe.ColumnDetails[3]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("name",100), new appframe.ColumnDetails("query",100)});
			randomTables.GuidForNote ="systemqueries";
			randomTables.Caption = LayoutDetails.SYSTEM_QUERIES;
	//		randomTables.Columns = new appframe.ColumnDetails[3]{new appframe.ColumnDetails("id",100), 
	//			new appframe.ColumnDetails("name",100), new appframe.ColumnDetails("query",100)};
			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout ());


			randomTables.AddRow(new object[3]{
				"1", "All", @""}
			);
			randomTables.AddRow(new object[3]{
				"2", "WritingProjects", @"notebook='Writing' and section='Projects'"}
			);

		
			///
			/// -- SUBMISSION -- most submission stuff needs to be in Submission AddIn but these two (for load reasons) are here. Sorry.
			/// 
			/// 
			/// 
			/// 1. Publish Types (electronic or print)
			/// 
			/// 
			randomTables = new NoteDataXML_Table(100, 100, new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("category",100)});
			randomTables.Caption = LayoutDetails.SYSTEM_PUBLISHTYPES;
			//	randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
			//		new appframe.ColumnDetails("category",100)};
			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout ());
			
			randomTables.AddRow(new object[2]{"1", Loc.Instance.GetString("Both")});
			randomTables.AddRow(new object[2]{"2", Loc.Instance.GetString("Electronic")});
			randomTables.AddRow(new object[2]{"3", Loc.Instance.GetString("None")});
			randomTables.AddRow(new object[2]{"4", Loc.Instance.GetString("Print")});

			///
			/// 2. Market Types (pay category)
			/// 
			/// 
			randomTables = new NoteDataXML_Table(100, 100, new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("category",100)});
			randomTables.Caption = LayoutDetails.SYSTEM_MARKETTYPES;
			//	randomTables.Columns = new appframe.ColumnDetails[2]{new appframe.ColumnDetails("id",100), 
			//		new appframe.ColumnDetails("category",100)};
			subpanel.AddNote(randomTables);
			randomTables.CreateParent(subpanel.GetPanelsLayout ());
			
			randomTables.AddRow(new object[2]{"1", Loc.Instance.GetString("Non Paying")});
			randomTables.AddRow(new object[2]{"2", Loc.Instance.GetString("None")});
			randomTables.AddRow(new object[2]{"3", Loc.Instance.GetString("Semi-Pro")});
			randomTables.AddRow(new object[2]{"4", Loc.Instance.GetString("Small Press (Token)")});
			randomTables.AddRow(new object[2]{"5", Loc.Instance.GetString("Pro Market")});

			SystemLayout.SaveLayout ();
			// note list needs to be at the end March 2013 but creatio happens earlier so it is the defautl viewed note
		    // so we build it later


			NoteDataXML_NoteList list = new NoteDataXML_NoteList();
			list.GuidForNote = "notelist";
			sidedockpanel.AddNote(list);

			list.CreateParent(sidedockpanel.GetPanelsLayout());
			list.Mode = NoteDataXML_NoteList.Modes.LAYOUTS;
			
			
			list.Dock = DockStyle.Fill;


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

