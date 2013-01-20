using System;
using System.Collections;
using CoreUtilities;
using System.Windows.Forms;
namespace Layout
{
	/// <summary>
	/// Wiill hold info about NoteTypes, About the active layoutpanel
	/// </summary>
	public class LayoutDetails
	{
		#region variables
		protected static volatile LayoutDetails instance;
		protected static object syncRoot = new Object();

		// used when needing random numbers
		public Random RandomNumbers;


		public LayoutPanelBase SystemLayout = null;
		public LayoutPanelBase TableLayout = null;

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

		public Action<string> LoadLayoutRef= null;
		public Action<string> UpdateTitle = null;

		// in rare case of harddrive failure this variable can be set and checked when the application closes to prevent saving empty files (December 2012)
		// for YOM this is Set ONLY IN MainformBase
		public bool ForceShutdown = false; 
		// added this for testing purposes
		public string OverridePath=Constants.BLANK;


		public string Path {
			get {


				string path = "";

#if(DEBUG)
				path = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "YOMDEBUG");
#else
				path =System.IO.Path.Combine ( Environment.CurrentDirectory, "YOMRELEASE");
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
		public LayoutDetails ()
		{


			RandomNumbers = new Random();

			//TypeList = new Type[4] {typeof(NoteDataXML), typeof(NoteDataXML_RichText), typeof(NoteDataXML_NoteList), typeof(NoteDataXML_SystemOnly)};
			TypeList = new ArrayList();
			NameList = new ArrayList();

			AddToList(typeof(NoteDataXML),new NoteDataXML().RegisterType());
			AddToList(typeof(NoteDataXML_RichText),new NoteDataXML_RichText().RegisterType ());
			AddToList (typeof(NoteDataXML_NoteList),new NoteDataXML_NoteList().RegisterType());
			AddToList (typeof(NoteDataXML_Table),new NoteDataXML_Table().RegisterType());
		//	AddToList (typeof(NoteDataXML_SystemOnly),new NoteDataXML_SystemOnly().RegisterType());


		}
		public void DoForceShutDown(bool ShutDown)
		{
			Layout.LayoutDetails.Instance.ForceShutdown = ShutDown;
		}
		/// <summary>
		/// This is how we decide to load a new layout. This is a link set in the MainForm
		/// that calls the main LoadLayout routine therein.
		/// </summary>
		/// <param name='guid'>
		/// GUID.
		/// </param>
		public void LoadLayout (string guid)
		{
			if (null != LoadLayoutRef) {
				LoadLayoutRef (guid);
			} else {
				throw new Exception("A reference needs to be set in the MainForm to the method to use when loading a new method. That did not happen today.");
			}
		}
		// other PLACES will modify this list, when registering new types
		private ArrayList TypeList = null;
		private ArrayList NameList = null;

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
			if (TypeList.IndexOf (newType) == -1) {
				TypeList.Add (newType);
				NameList.Add (name);
			}

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
			if (NameList.Count != TypeList.Count) {
				throw new Exception("Must be same number of type names as types");
			}
			int index = TypeList.IndexOf (lookupType);
			if (index >= 0) {
				return NameList[index].ToString ();
			}
			return Constants.ERROR;
		}
		public Type[] ListOfTypesToStoreInXML ()
		{
			Type[] ArrayOfTypes = new Type[TypeList.Count];
			TypeList.CopyTo(ArrayOfTypes);
			return ArrayOfTypes;
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
		public static ToolStripMenuItem BuildMenuPropertyEdit (string Title, string ToolTip, KeyEventHandler action)
		{
			ToolStripMenuItem TableCaptionLabel = new ToolStripMenuItem ();
			TableCaptionLabel.Text = Title;
			TableCaptionLabel.ToolTipText = ToolTip;
			ContextMenuStrip TableCaptionMenu = new ContextMenuStrip ();
			ToolStripTextBox TableCaptionText = new ToolStripTextBox ();
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
					((sender as ToolStripTextBox).Tag as ToolStripMenuItem).Text = Caption;
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
	}
}




