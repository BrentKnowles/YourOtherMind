// LayoutDetails.cs
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
using System.Collections;
using System.Collections.Generic;
using CoreUtilities;
using System.Windows.Forms;
using System.Drawing;
using Transactions;
using System.IO;

namespace Layout
{
	/// <summary>
	/// Wiill hold info about NoteTypes, About the active layoutpanel
	/// </summary>
	public class LayoutDetails
	{
		public static  string  SIDEDOCK = "system_sidedock";
		public static string TABLEGUID = "tables"; // name of panel containing the tables on the system note

		// the list of events (deleting, submissions, worklogs)
		private List<Type> transactionsLIST = new List<Type>();

		// this list TEMPORARILY stores, during a LayoutPanel Load, the list of notes requesting an Update-after-load-finishes. LayoutPanel does this form this. TODO: This is a poor implementation
		public List<NoteDataInterface> UpdateAfterLoadList = new List<NoteDataInterface>();
		public void AddTo_TransactionsLIST (Type newType)
		{
			try {
				transactionsLIST.Add (newType);
			} catch (Exception ex) {
				NewMessage.Show (ex.ToString());
			}
		//	NewMessage.Show (((TransactionBase)Activator.CreateInstance (newType)).Display);
		}
			

		#region constants

		// for testing and want certain minor warnings not to trigger. use sparingly
		public bool SuppressWarnings = false;

		public static string SYSTEM_QUERIES ="list_queries";
		public const string SYSTEM_RANDOM_TABLES = "list_randomtables";
		public const string SYSTEM_NOTEBOOKS = "list_notebooks";
		public const string SYSTEM_STATUS = "list_status";
		public const string SYSTEM_SUBTYPE = "list_subtypes";
		public const string SYSTEM_KEYWORDS = "list_keywords";

		// March 2013
		// these two tables need to be in System because of the way I'm storing and loading them from the serialized market table
		//public const  string SYSTEM_PUBLISHTYPES ="list_publishtypes";
		//public const  string SYSTEM_MARKETTYPES ="list_markettypes";

		

		//public const string SYSTEM_WORKLOGCATEGORY="list_worklogcategory";
		//public const string SYSTEM_GRAMMAR="list_grammar";
#endregion
		#region variables


		public NoteDataInterface currentCopiedNote=null;
		/// <summary>
		/// Gets or sets the current copied note.
		/// </summary>
		/// <value>
		/// The current copied note.
		/// </value>
		public NoteDataInterface CurrentCopiedNote {
			get {
				return currentCopiedNote;
			}
			set {
				currentCopiedNote = value;
			}
		}

		protected static volatile LayoutDetails instance;
		protected static object syncRoot = new Object();
	
		// Used when forms have an OK and Cancel button; this the height of the panel
		public const int ButtonHeight = 50;
	

		// set in main constructor
		public Icon MainFormIcon = null;
		public FormUtils.FontSize MainFormFontSize = FormUtils.FontSize.Normal;

		// used when needing random numbers
		public Random RandomNumbers;
		public List<Color> HighlightColorList = new List<Color>();

		public LayoutPanelBase SystemLayout = null;
		public LayoutPanelBase TableLayout = null;

		ToolStripProgressBar progress=null;

		public ToolStripProgressBar Progress {
			get {

				return progress;
			}
			set {
				progress = value;
			}
		}

		private WordsSystem wordSystemInUse = null;

		public WordsSystem WordSystemInUse {
			get {

				if (null == wordSystemInUse)
				{
					// instead of setting in main form
					// we just create a default if none was established
					wordSystemInUse = new WordsSystem();
				}

				return wordSystemInUse;
			}
			set {
				wordSystemInUse = value;
			}
		}

		private LayoutPanelBase currentLayout;
		public LayoutPanelBase CurrentLayout {
			get { return currentLayout;}
			set {
				currentLayout = value;
				if (null != value)
				{
				if (UpdateTitle != null )
				{
						//currentLayout
					UpdateTitle(currentLayout.Caption);
				}
				lg.Instance.Line ("LayoutDetails->CurrentLayout", ProblemType.MESSAGE, "Setting Layout to " + currentLayout.GUID);
				}
				else
				{
					// if null, blank the title
					UpdateTitle("");
				}
			}
		}



		public Action<string, string> LoadLayoutRef= null;
		public Action<string> UpdateTitle = null;
		private bool titleUpdateSuspended = false;

		/// <summary>
		/// Gets a value indicating whether this instance is title suspended.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is title suspended; otherwise, <c>false</c>.
		/// </value>
		public bool IsTitleSuspended {
			get {
				return titleUpdateSuspended;
			}

		}
		/// <summary>
		/// Suspends the title update.
		/// </summary>
		/// <param name='Suspend'>
		/// If set to <c>true</c> suspend.
		/// </param>
		public void SuspendTitleUpdate (bool Suspend)
		{
			titleUpdateSuspended = Suspend;
		}

		private appframe.MainFormBase MainForm = null;

		public void SetMainForm (appframe.MainFormBase mainForm)
		{
			MainForm = mainForm;
		}
		/// <summary>
		/// Wrapper to run a hot-key from SOMEWHERE other than a key press
		/// </summary>
		/// <param name='code'>
		/// Code.
		/// </param>
		public void ManualRunHotkeyOperation (string code)
		{
			if (MainForm != null) {
				MainForm.ManualRunHotkeyOperation(code);
			}
		}


		// in rare case of harddrive failure this variable can be set and checked when the application closes to prevent saving empty files (December 2012)
		// for YOM this is Set ONLY IN MainformBase
		public bool ForceShutdown = false; 
		// added this for testing purposes
		public string OverridePath=Constants.BLANK;

		public void DoUpdateTitle (string newTitle)
		{
			if (UpdateTitle != null) {
				UpdateTitle(newTitle);
			}
		}

		public string Path {
			get {


				string path = "";

#if(DEBUG)
				path = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "YOMDEBUG");
#else
				path =System.IO.Path.Combine ( Environment.CurrentDirectory, "YourOtherMind");
#endif

				if (Constants.BLANK != OverridePath)
				{
					path = OverridePath;
				}

				if (!System.IO.Directory.Exists (path)) {
					System.IO.Directory.CreateDirectory(path);
				}
				return path;
			}
		}
		private string yom_database= Constants.BLANK;
		// Moved from LayoutDatabase into ths so it can more easily be overridden by unit tests
		// We return the standard name for database, with the path UNLESS it has been overridden by a set WHICH should only happen during unit testing
		public virtual string YOM_DATABASE {
			get { 
				string path = "";
				if (yom_database == Constants.BLANK) {
					 path =  System.IO.Path.Combine (LayoutDetails.Instance.Path, "yomdata.s3db");

				} else
					 path=yom_database;

				return path;
			}
			set {
				yom_database = value;
			}
		}

		#endregion;
		#region callbacks
		public delegate AppearanceClass GetAppearanceFromStorageTYPE(string Key);
		public GetAppearanceFromStorageTYPE GetAppearanceFromStorage;

		public Func<System.Collections.Generic.List<string>> GetListOfAppearancesDelegate=null;
		#endregion
		// other PLACES will modify this list, when registering new types
		//private ArrayList TypeList = null;
		//private ArrayList NameList = null;
		
		private List<NoteTypeDetails> ListOfNoteTypes = new List<NoteTypeDetails>();
		public List<NoteTypeDetails> GetListOfNoteTypeDetails ()
		{
			return ListOfNoteTypes;
		}

		public void WaitCursor (bool b)
		{
			if (LayoutDetails.Instance.CurrentLayout != null) {
				if (true == b)
				{
				LayoutDetails.Instance.CurrentLayout.Cursor = Cursors.WaitCursor;
				}
				else{
					LayoutDetails.Instance.CurrentLayout.Cursor = Cursors.Default;
				}
			}
		}

		/// <summary>
		/// Clears the dragging of notes on A layout. If stuck in dragmode
		/// </summary>
		public void ClearDraggingOfNotesOnALayout ()
		{
			if (CurrentLayout != null) {
				CurrentLayout.ClearDrag();
			}
		}
		public LayoutDetails ()
		{


			RandomNumbers = new Random();

			//TypeList = new Type[4] {typeof(NoteDataXML), typeof(NoteDataXML_RichText), typeof(NoteDataXML_NoteList), typeof(NoteDataXML_SystemOnly)};
//			TypeList = new ArrayList();
//			NameList = new ArrayList();

			AddToList(typeof(NoteDataXML),new NoteDataXML().RegisterType());
			AddToList(typeof(NoteDataXML_RichText),new NoteDataXML_RichText().RegisterType ());
			AddToList (typeof(NoteDataXML_NoteList),new NoteDataXML_NoteList().RegisterType());
			AddToList (typeof(NoteDataXML_Table),new NoteDataXML_Table().RegisterType());
			AddToList(typeof(NoteDataXML_LinkNote), new NoteDataXML_LinkNote().RegisterType());
			AddToList(typeof(NoteDataXML_Timeline), new NoteDataXML_Timeline().RegisterType());
			AddToList(typeof(NoteDataXML_GroupEm), new NoteDataXML_GroupEm().RegisterType());
		//	AddToList (typeof(NoteDataXML_SystemOnly),new NoteDataXML_SystemOnly().RegisterType());

			Markups = new List<iMarkupLanguage> ();
			Markups.Add (new MarkupLanguageNone ());
		}
		public void DoForceShutDown(bool ShutDown)
		{
			Layout.LayoutDetails.Instance.ForceShutdown = ShutDown;
		}
		/// <summary>
		/// This is how we decide to load a new layout. This is a link set in the MainForm
		/// that calls the main LoadLayout routine therein.
		/// 
		/// If opened already, it will 'bring it to front'
		/// 
		/// </summary>
		/// <param name='guid'>
		/// GUID.
		/// </param>
		public void LoadLayout (string guid, string childGuid)
		{
			if (null != LoadLayoutRef) {
				LoadLayoutRef (guid, childGuid);
			} else {
				throw new Exception("A reference needs to be set in the MainForm to the method to use when loading a new method. That did not happen today.");
			}
		}

		/// <summary>
		/// Loads the layout.
		///  30/06/2014
		///  - can pass text that the layout attempts to find on the child note
		/// </summary>
		/// <param name='guid'>
		/// GUID.
		/// </param>
		/// <param name='childGuid'>
		/// Child GUID.
		/// </param>
		/// <param name='TextToSearchForOnChild'>
		/// Text to search for on child.
		/// </param>
		public void LoadLayout (string guid, string childGuid, string TextToSearchForOnChild)
		{
			if (null != LoadLayoutRef) {
				LoadLayoutRef (guid, childGuid);



				if (CurrentLayout != null && CurrentLayout.CurrentTextNote != null)
				{
					FindBarStatusStrip find = CurrentLayout.GetFindbar();
					if (find != null)
					{
						CurrentLayout.FocusOnFindBar();

						find.DoFind (TextToSearchForOnChild, false, CurrentLayout.CurrentTextNote.GetRichTextBox() ,0);
					}
				}
			} else {
				throw new Exception("A reference needs to be set in the MainForm to the method to use when loading a new method. That did not happen today.");
			}
		}

		public void LoadLayout (string guid)
		{
			
			if (null != LoadLayoutRef) {
				LoadLayoutRef (guid, Constants.BLANK);
			} else {
				throw new Exception("A reference needs to be set in the MainForm to the method to use when loading a new method. That did not happen today.");
			}
		}


		/// <summary>
		/// Adds to both lists.
		/// Will not add it if it already esxists
		/// </summary>
		/// <param name='newType'>
		/// New type.
		/// </param>
		/// <param name='newAssembly'>
		/// New assembly.
		/// </param>
		public void AddToList (Type newType, string name)
		{
			AddToList (newType, name, Constants.BLANK);
		}

		public void AddToList (Type newType, string name, string folder)
		{
//			if (TypeList.IndexOf (newType) == -1) {
//				TypeList.Add (newType);
//				NameList.Add (name);
//
//			}
		
			if (Constants.BLANK == folder) {
				// probably do nothing
			}
			if (ListOfNoteTypes.Find (NoteTypeDetails=>NoteTypeDetails.TypeOfNote == newType) != null)
			{
			}
			else
			{
				// A new one. Let us add it.
				NoteTypeDetails newNoteType = new NoteTypeDetails();
				newNoteType.TypeOfNote = newType;
				newNoteType.NameOfNote = name;


				newNoteType.Folder = folder;
				ListOfNoteTypes.Add(newNoteType);
			}
			 
			
		}
		public void RemoveFromList (Type newType)
		{
			NoteTypeDetails newNoteType = ListOfNoteTypes.Find (NoteTypeDetails => NoteTypeDetails.TypeOfNote == newType);
			if (null != newNoteType) {
				ListOfNoteTypes.Remove (newNoteType);
			}

//			if (TypeList.IndexOf (newType) > -1) {
//				TypeList.Remove(newType);
//			}
		}
		/// <summary>
		/// Gets the type of the name from.
		/// Assumes namelist and typelist are equal at all times
		/// </summary>
		/// <returns>
		/// The name from type.
		/// </returns>
		/// <param name='lookupType'>
		/// Lookup type.
		/// </param>
		public string GetNameFromType (Type lookupType)
		{

			NoteTypeDetails newNoteType = ListOfNoteTypes.Find (NoteTypeDetails => NoteTypeDetails.TypeOfNote == lookupType);
			if (newNoteType != null) {
				return newNoteType.NameOfNote;
			}

			return Constants.ERROR;

//			if (NameList.Count != TypeList.Count) {
//				throw new Exception("Must be same number of type names as types");
//			}
//			int index = TypeList.IndexOf (lookupType);
//			if (index >= 0) {
//				return NameList[index].ToString ();
//			}
//			return Constants.ERROR;
		}
		public Type[] ListOfTypesToStoreInXML ()
		{
			ArrayList TypesA = new ArrayList ();
			foreach (NoteTypeDetails nd in ListOfNoteTypes) {
				TypesA.Add (nd.TypeOfNote);
			}

			Type[] ArrayOfTypes = new Type[ListOfNoteTypes.Count];
			TypesA.CopyTo(ArrayOfTypes);
			return ArrayOfTypes;
		}
		public static ToolStripMenuItem BuildMenuPropertyEdit (string Label, string Title, string ToolTip, KeyEventHandler action)
		{
			return BuildMenuPropertyEdit(Label, Title, ToolTip, action, -1);
		}
	/// <summary>
	/// Convenience function to make it easier to build Menu option that show the 'Field' and then allow a sideclick to edit them.
	/// </summary>
	/// <returns>
	/// The menu property edit.
	/// </returns>
	/// <param name='Title'>
	/// Title.
	/// </param>
	/// <param name='ToolTip'>
	/// Tool tip.
	/// </param>
	/// <param name='action'>
	/// Action.
	/// </param>
		public static ToolStripMenuItem BuildMenuPropertyEdit (string Label, string Title, string ToolTip, KeyEventHandler action, int truncate)
		{

		

			ToolStripMenuItem TableCaptionLabel = new ToolStripMenuItem ();
			TableCaptionLabel.Text = String.Format (Label, Title);
			// 
			// Truncate text if specified
			//
			if (truncate > 0 && truncate < TableCaptionLabel.Text.Length) {
				TableCaptionLabel.Text = TableCaptionLabel.Text.Substring(0, truncate) + "...";
			}

			TableCaptionLabel.ToolTipText = ToolTip;
			ContextMenuStrip TableCaptionMenu = new ContextMenuStrip ();
			ToolStripTextBox TableCaptionText = new ToolStripTextBox ();

			TableCaptionLabel.Tag = Label; // this is the format label 	// label is a format string like "Field: {0}"

			TableCaptionText.Tag = TableCaptionLabel;
			TableCaptionText.Text = Title; 
			TableCaptionText.KeyDown += action;
			TableCaptionMenu.Items.Add (TableCaptionText);
			
			TableCaptionLabel.DropDown = TableCaptionMenu;
			return TableCaptionLabel;
		}

		public static void HandleMenuLabelEdit (object sender, KeyEventArgs e, ref string Caption, Action<bool>SetSaveRequired)
		{
			if (e.KeyData == Keys.Enter) {
				// the header is not updated unti enter pressed but the NAME is being updated
				Caption = (sender as ToolStripTextBox).Text;
				if ((sender as ToolStripTextBox).Tag != null && ((sender as ToolStripTextBox).Tag is ToolStripMenuItem)) {
					string formatstring = ((sender as ToolStripTextBox).Tag as ToolStripMenuItem).Tag.ToString();
					((sender as ToolStripTextBox).Tag as ToolStripMenuItem).Text = String.Format (formatstring, Caption);
				}
				// silenece beep
				e.SuppressKeyPress = true;
				SetSaveRequired (true);
			}
		}
		public static LayoutDetails Instance
		{
			get
			{
				if (null == instance)
				{
					// only one instance is created and when needed
					lock (syncRoot)
					{
						if (null == instance)
						{
							instance = new LayoutDetails();
							instance.HighlightColorList.Add (Color.BurlyWood);
							instance.HighlightColorList.Add (Color.Yellow);
							instance.HighlightColorList.Add (Color.CornflowerBlue);
							
							instance.HighlightColorList.Add (Color.Red);
							instance.HighlightColorList.Add (Color.Green);
							instance.HighlightColorList.Add (Color.White);
							instance.HighlightColorList.Add (Color.Black);
							instance.HighlightColorList.Add (Color.MediumSpringGreen);
							instance.HighlightColorList.Add (Color.Aquamarine);
							instance.HighlightColorList.Add (Color.GreenYellow);
							instance.HighlightColorList.Add (Color.LightSalmon);
							instance.HighlightColorList.Add (Color.Blue);
							instance.HighlightColorList.Add (Color.Sienna);
							instance.HighlightColorList.Add (Color.PeachPuff);
						
						}
					}
				}
				return (LayoutDetails)instance;
			}
		}


		// The Data Wrappers -- Here is where the specific subclass/impelmentation of interfaces are used
		public static LayoutInterface DATA_Layout(string GUID)
		{
			return new LayoutDatabase(GUID);
		}

		string layoutlink = Constants.BLANK;
		public void PushLink(string identifier)
		{
			layoutlink = identifier;
		}

		/// <summary>
		/// Grabs the existing link from the list and clears it
		/// </summary>
		/// <returns>
		/// The link.
		/// </returns>
		public string PopLink()
		{
			string result = layoutlink;
			layoutlink = Constants.BLANK;
			return result;
		}


		private TransactionsTable transactionsList=null;

		List<Type> GetListOfTransacitonTypesAddedByAddIns ()
		{
			return transactionsLIST;
		}

		/// <summary>
		/// Access to the event table
		/// </summary>
		/// <value>
		/// The events list.
		/// </value>
		public TransactionsTable TransactionsList {
			get {
				if (transactionsList == null) throw new Exception ("Tran list has not been setup.");
				return transactionsList;
			}
			set {

				// when we set the transaction list we set a callback to get the list of types

				transactionsList = value;
				transactionsList.TransactionTypesAddedThroughAddIns += GetListOfTransacitonTypesAddedByAddIns;
			}
		}

		/// <summary>
		/// Focuses the on find bar. Ctrl +F
		/// </summary>
//		public void FocusOnFindBar ()
//		{
//			if (CurrentLayout != null) {
//				CurrentLayout.FocusOnFindBar();
//			}
//		}

		#region markup
		// List of available markup languages
		List<iMarkupLanguage> Markups=null;
		iMarkupLanguage CurrentMarkup=null;
		public void AddMarkupToList (iMarkupLanguage newMarkup)
		{
		
			Markups.Add (newMarkup);
		}
		
		/// <summary>
		/// Gets the markup match. Used in the Interfact options panel
		/// to figure out which type was saved as the default
		/// the value is the type stored in the database
		/// </summary>
		/// <param name='value'>
		/// Value.
		/// </param>
		public iMarkupLanguage GetMarkupMatch (string value)
		{
			foreach (iMarkupLanguage mark in Markups) {
				if (mark.GetType().AssemblyQualifiedName == value)
				{
					return mark;
				}
			}
			return null;
		}
		
		public void RemoveMarkupFromList (Type markup)
		{
			iMarkupLanguage removeMe = null;
			foreach (iMarkupLanguage Mark in Markups) {
				if (Mark.GetType () == markup) {
					removeMe = Mark;
				}
			}
			if (null != removeMe) {
				Markups.Remove(removeMe);
			}
		}
		
		public iMarkupLanguage GetCurrentMarkup ()
		{
			if (null == CurrentMarkup) {
				CurrentMarkup = new MarkupLanguageNone();
			}
			return CurrentMarkup;
		}
		public void SetCurrentMarkup (iMarkupLanguage newMarkup)
		{
			// set when options menu closes and when MainForm opens
			if (null != newMarkup) {
				CurrentMarkup = newMarkup;
			}
		}
		public void BuildMarkupComboBox (ComboBox markupCombo)
		{
	
			foreach (iMarkupLanguage language in Markups) {
				markupCombo.Items.Add (language);
			}
		}
		public void BuildMarkupComboBox (ToolStripComboBox markupCombo)
		{
			
			foreach (iMarkupLanguage language in Markups) {
				markupCombo.Items.Add (language);
			}
		}

		/// <summary>
		/// Returns a simple text form to use for Building Reports
		/// </summary>
		/// <returns>
		/// The text form to use.
		/// </returns>
		public GenericTextForm GetTextFormToUse()
		{
			return new GenericTextForm();
		}



		public System.Collections.Generic.List<string> GetListOfAppearances ()
		{
			if (GetListOfAppearancesDelegate != null) {
				return GetListOfAppearancesDelegate();
			}
			return null;
		}

		public void PurgeAppearanceCache ()
		{
			AppearanceCache = new Hashtable();
		}
		Hashtable  AppearanceCache = new Hashtable();
	//	System.Collections.SortedList  AppearanceCache = new SortedList();
		//List<Appearance> AppearanceCache = new List<Appearance>();
		public AppearanceClass GetAppearanceByName (string classic)
		{
			if (null == GetAppearanceFromStorage) {
				throw new Exception("A callback to GetAppearanceFromStorage needs to be defined when application initalizes");
			}


			//Hashtable attempt 6.7 seconds to 6.4 on second attempt

			AppearanceClass app = (AppearanceClass)AppearanceCache [classic];
			if (app != null) {
			} else {


				app = GetAppearanceFromStorage(classic);

				if (app == null)
				{
					NewMessage.Show (Loc.Instance.GetStringFmt("The appearance {0} no longer exists, reverting to default.", classic));
					// somehow we have lost an appearance then we just use clasic
					app = AppearanceClass.SetAsClassic();

				}
				//app = new Appearance();
				//app.SetAsClassic();
				
				AppearanceCache.Add (classic, app);
			}

			//6.4 seconds to 6.9
//			Appearance app = (Appearance)AppearanceCache [classic];
//			if (app != null) {
//			} else {
//				NewMessage.Show ("Hack : 'grabbing app from database. Should see this only once a session");
//							app = new Appearance();
//							app.SetAsClassic();
//				
//								AppearanceCache.Add (classic, app);
//			}
//




			// 1. Look to see if it already exist. If so, retrieve it. about 6.6 seconds on zombie
			//    IF NOT, load from database

			/* This seemed faster than Sorted list

			Appearance app = AppearanceCache.Find (Appearance => Appearance.Name == classic);
			if (app != null) {

			}
			else
			{
				NewMessage.Show ("Hack : 'grabbing app from database. Should see this only once a session");
			app = new Appearance();
			app.SetAsClassic();

				AppearanceCache.Add (app);
			}
*/
			return app;
		}
		#endregion

		public static void SupressBeep (KeyPressEventArgs e)
		{
			//e.SuppressKeyPress = true;
			e.Handled = true;
		}

		/// <summary>
		/// called from SaveTextLineToFile
		/// </summary>
		/// <param name="NoteToOpen">if present and we encounter [[title]] we replace [[title]] with NoteToOpen   ///  </param>
		/// <param name="sText"></param>
		void SaveTextLineByLine (StreamWriter writer, string[] linesOfText, string empty)
		{
			foreach (string s in linesOfText)
			{
				writer.WriteLine(s);
			}
		}

		int WriteANote (NoteDataInterface note, bool bGetWords, ref string sWordInformation, StreamWriter writer)
		{
			int words = 0;
			if (note != null && (note is NoteDataXML_RichText))
			{
				RichTextBox tempBox = new RichTextBox();
				tempBox.Rtf = note.Data1;
				SaveTextLineByLine(writer, tempBox.Lines, note.Caption);
				
				if (true == bGetWords)
				{
					int Words = 
						LayoutDetails.Instance.WordSystemInUse.CountWords(tempBox.Text);
					words = Words;//TotalWords = TotalWords + Words;

					
					
					
					sWordInformation = sWordInformation + String.Format("{0}: {1}{2}", note.Caption, Words.ToString(), Environment.NewLine);
				}
				
				tempBox.Dispose();
			}
			return words;
		}
		
		/// <summary>
		/// Goes through rich edit line by line saving to a plain text file
		/// 
		/// December 2009
		///  Here we need to do a redesign.
		/// 
		/// If we encounter [[index]] on the first line we know that we have an index page. So instead we need to do the following:
		/// 
		/// Each line is either a NOTE NAME to add to the text file (which will be parsed line by line and converted to plain text)
		/// 
		/// OR
		/// 
		/// It returns a list of names that are then parsed line by line as above, in the order of the list
		///   
		/// An example index would be
		/// [[index]]
		/// _Header [[words]]
		/// [[Group,Storyboard,Chapter*,words]         !- This returns an array of note names that match the criteria (i.e., Chapter 01, Chapter 02)
		/// _Footer
		/// 
		/// 
		/// 
		/// * Note: Choose not to let the groups handl
		/// </summary>
		/// <param name="sFile"></param>
		public void SaveTextLineToFile (string[] LinesOfText, string sFilepath)
		{
			string sWordInformation = "";
			int TotalWords = 0;
			bool bGetWords = false;
			
			// certain Addins like spellchecking won't both writing a file out
			if (sFilepath != Constants.BLANK) {
				
				
				try {
					StreamWriter writer = new StreamWriter (sFilepath);
					
					//
					//					if (LayoutDetails.Instance.CurrentLayout != null && LayoutDetails.Instance.CurrentLayout.CurrentTextNote != null)
					//					{
					//						NewMessage.Show (Loc.Instance.GetString ("The current markup does not support Sending-Away files correctly. Did you forget to set the current markup in the Options menu?"));
					//												if (LayoutDetails.Instance.CurrentLayout.CurrentTextNote is NoteData
					//					}
					ArrayList ListOfParsePages = new ArrayList ();
					if (LayoutDetails.Instance.GetCurrentMarkup().IsIndex(LinesOfText [0].ToLower ()) == true)
					{

						//if (LinesOfText [0].ToLower () == "[[index]]") {
						// we are actually an index note
						// which will instead list a bunch of other pages to use
						// we now iterate through LinesOfText[1] to end and parse those instead
						for (int i = 1; i < LinesOfText.Length; i++) {
							string sLine = LinesOfText [i];
						

							

							bool WordCountRequested = false;
							if (LayoutDetails.Instance.GetCurrentMarkup().IsOver(sLine))
							{
								// if we hit a manual terminate then we exit adding pages
								// this is used if we want notes on an index page but
								// don't want to waste time trying to parse them
								break;
							}
							if (LayoutDetails.Instance.GetCurrentMarkup().IsWordRequest(sLine))
							 {

								// if we have the words keyword we know we want to display some word info at the end
								sLine = LayoutDetails.Instance.GetCurrentMarkup().CleanWordRequest(sLine);
								
								bGetWords = true;
								WordCountRequested  = true;
							}

							if (LayoutDetails.Instance.GetCurrentMarkup().IsGroupRequest(sLine)) {
								
								ArrayList tmp   = LayoutDetails.Instance.GetCurrentMarkup().GetListOfPages(sLine, ref bGetWords, LayoutDetails.Instance.CurrentLayout);
								ListOfParsePages.AddRange(tmp);
								// we have a group
								
							} else {
								if (false == WordCountRequested)
								{
									// at any point we encounter [[words]] we switch to counting words
								ListOfParsePages.Add (sLine);
								}
							}
							

							//                            panel.Dispose(); Don't think I can do this becauseit would dlette hte note, benig an ojbect
							// may 2013 - moving the iteration outside generation loopListOfParsePages = null;
							
						}
					} else {
						// not an index
						SaveTextLineByLine (writer, LinesOfText, "");
					}


					if (ListOfParsePages != null)
					{
						// Now we go through the pages and write them into the text file
						// feb 19 2010 - added because chapter notes were not coming out in alphaetical
						ListOfParsePages.Sort ();

						foreach (string notetoopen in ListOfParsePages) {
							//	DrawingTest.NotePanel panel = ((mdi)_CORE_GetActiveChild()).page_Visual.GetPanelByName(notetoopen);
							NoteDataInterface note = LayoutDetails.Instance.CurrentLayout.FindNoteByName(notetoopen);	
							
							if (note != null)
							{
								
								// may 2013
								// if a panel is specified then we open each note on that panel?
								if (note.ListOfSubnotes() != null)
								{
									// we don't know about panels directly at this level
									// so we query if there are subnotes
									// IF SO: then we parse them

									// may 27 2013 - also need to sort subpages coming from a panel

									List<string> subpages = note.ListOfSubnotes();
									subpages.Sort ();
									foreach (string s in subpages)
									{
										NoteDataInterface subnote = LayoutDetails.Instance.CurrentLayout.FindNoteByName(s);	
										//	subnote = LayoutDetails.Instance.CurrentLayout.GoToNote(subnote);
										if (null != subnote)
										{
											TotalWords = TotalWords + WriteANote(subnote, bGetWords, ref sWordInformation, writer);
										}
									}
								}
								else
								{
									// just a normal note
									TotalWords = TotalWords + WriteANote(note, bGetWords, ref sWordInformation, writer);
								}
								
							}
						} //open each note list
					}//list not nulls

					writer.Close ();
					writer.Dispose ();




					if (sWordInformation != "") {
						string sResult = Loc.Instance.GetStringFmt ("Total Words:{0}\n{1}", TotalWords.ToString (), sWordInformation);
						Clipboard.SetText (sResult);
						NewMessage.Show (Loc.Instance.GetString ("Your Text Has Been Sent! Press Ctrl + V to paste word count information into current note."));
						//NewMessage.Show(sResult);
					}
				} catch (Exception) {
					NewMessage.Show (Loc.Instance.GetStringFmt ("Unable to write to {0} please shut down and try again", sFilepath));
				}
			}
			LinesOfText = null;
			
			
		}
		/// <summary>
		/// Filters the by keyword.
		/// 
		/// Will *attempt* to find a Note List on the System Page and ask it to filter by the specified keyword
		/// </summary>
		/// <param name='text'>
		/// Text.
		/// </param>
		public void FilterByKeyword (string text)
		{
			if (LayoutDetails.Instance.SystemLayout != null) {
				LayoutDetails.Instance.SystemLayout.FilterByKeyword (text);
			}
		}
		// used as an accessor to get the default font set in options
		public Func<Font> GetDefaultFont = null;


	}
}




