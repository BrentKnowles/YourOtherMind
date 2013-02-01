using System;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
using CoreUtilities;

namespace Layout
{
	// TODO:
	// We might consider using this as Layout.XML (and LayoutXML too would share this) to encourage me NOT to 
	//   call any methods here directly.
	// I need to be using the interface instead


	/// <summary>
	/// The XML version of a Note.
	/// 
	/// Because I intend to store the entire Note data inside an XML that is inside a table this would work with either XML or database representations.
	/// </summary>
	[Serializable]
	public partial class NoteDataXML : NoteDataInterface, IComparable, IDisposable
	{
		#region constants

		const string BLANK = "";
		protected int defaultheight = 200;
		protected int defaultwidth = 200;
		[XmlIgnore]
		public virtual int defaultHeight { get { return defaultheight; } }
		[XmlIgnore]
		public virtual int defaultWidth { get { return defaultwidth; } }

		#endregion

		#region variables_frominterface
		//TODO was trying to avoid storing a reference to actual Layout but the need to update the list (and handle the Property grid) made it so)
		protected LayoutPanelBase Layout;

		public virtual bool IsSystemNote{
			get {return false;}
		}
		protected virtual void CommonConstructorBehavior ()
		{
			Caption = Loc.Instance.Cat.GetString("Blank Note");
			GuidForNote = System.Guid.NewGuid().ToString();
		}
		public NoteDataXML () 
		{
			CommonConstructorBehavior ();
		
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Layout.NoteDataXML"/> class.
		/// 
		/// If -1 passed in for a parameter it will use the default height or width.
		/// This constructor is called only when creating a note (NOT when loading)
		/// </summary>
		/// <param name='_height'>
		/// _height.
		/// </param>
		/// <param name='_width'>
		/// _width.
		/// </param>
		public NoteDataXML (int _height, int _width)
		{
			CommonConstructorBehavior ();
			if (-1 != _height) {
				height = _height;
			} else {
				height = defaultHeight;
			}


			if (-1 != _width) {
				width = _width;
			}
			else {
				width =  defaultWidth;
			}
		}
		private bool visible = true;

		public bool Visible {
			get {
				return visible;
			}
			set {
				visible = value;
			}
		}

		private NotePanel parent;
		[XmlIgnore]
		public NotePanel ParentNotePanel {
			get{ return parent;}
			set{ parent = value;}
		}


		protected bool readOnly=false;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Layout.NoteDataXML"/> read only.
		/// </summary>
		/// <value>
		/// <c>true</c> if read only; otherwise, <c>false</c>.
		/// </value>
		public virtual bool ReadOnly {
			get {
				return readOnly;
			}
			set {
				readOnly = value;
			}
		}

		#endregion

		#region xml_save
		private DockStyle dock = DockStyle.None;
		public DockStyle Dock {
			get { return dock;}
			set { dock = value;}
		}

		private string _GuidForNote=Constants.BLANK;
		/// <summary>
		/// Unique identifier specifying this particular note within a layout.
		/// </summary>
		/// <value>
		/// The GUID for note.
		/// </value>
		public string GuidForNote
		{
			get { return _GuidForNote; } set { _GuidForNote = value; } 
		}
	
		private string caption = CoreUtilities.Constants.BLANK;
		public string Caption {
			get {return caption;}
			set{ caption = value;}
					}

		protected int height = 200;
		protected int width = 200;
		public int Height { get { return height; } set { height = value; }}
		public int Width { get { return width; } set { width = value; }}




		private Point location = new Point(0,0);
		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		/// <value>
		/// The location.
		/// </value>
		public Point Location {
			get { return location;}
			set { 

				location = value;

			// TODO Can never have Interface actions here. They must occur elsewhere.
			}
		}
		private string rtf  = CoreUtilities.Constants.BLANK ;
		public string Data1 {
			get { return rtf;}
			set { rtf = value;}
		}

	
		#endregion
		#region variables_new
//		/// <summary>
//		/// THIS works but is bad design. The interface is not useful if we add too many custom, public methods
//		/// </summary>
//		/// <value>
//		/// The just XMLONLY test.
//		/// </value>
//		public string JustXMLONLYTest {
//			get { return "boo";}
//		}
		#endregion;

		#region control_panel_settings
		public virtual bool IsPanel {
			get { return false;}
		}
		public virtual bool IsLinkable { get { return false; }}
#endregion;

		#region methods
		public override string ToString ()
		{
			return string.Format ("[NoteDataXML: defaultHeight={0}, defaultWidth={1}, IsSystemNote={2}, Visible={3}, ParentNotePanel={4}, ReadOnly={5}, Dock={6}, GuidForNote={7}, Caption={8}, Height={9}, Width={10}, Location={11}, Data1={12}, IsPanel={13}, IsLinkable={14}, SetSaveRequired={15}]", defaultHeight, defaultWidth, IsSystemNote, Visible, ParentNotePanel, ReadOnly, Dock, GuidForNote, Caption, Height, Width, Location, Data1, IsPanel, IsLinkable, SetSaveRequired);
		}
		public virtual System.Collections.ArrayList GetChildNotes()
		{
			return null;
		}

		public virtual void Save()
		{
			// save additional UI elements to XML fiellds, *if* necessary
		}

		/// <summary>
		/// Recreates the interface, usually called when something like Caption is changed. Use UpdateLocation for position only changes
		/// 
		/// DO NOT CALL THIS FROM EDITING PROPERTIES!
		/// </summary>
		/// <param name='Layout'>
		/// Layout.
		/// </param>
		public virtual void Update (LayoutPanelBase Layout)
		{
			Save ();
			ParentNotePanel.Dispose();
			//Parent = null;
			CreateParent(Layout);

		}
		/// <summary>
		/// Destroy this instance.
		/// 
		/// Called during a move (and probably a delete when they get there
		/// </summary>
		public void Destroy()
		{
			//NewMessage.Show ("DO NOT CALL THIS JAN 20 2013");
			ParentNotePanel.Dispose ();
		}


		/// <summary>
		/// Updates the location, called when location is set outside actual movement.
		/// TODO: Generalize this to UpdateAppearance?
		/// Also update sizea
		/// </summary>
		public void UpdateLocation ()
		{
			if (ParentNotePanel == null) {
				throw new Exception("Parent must not be null");
			}
			ParentNotePanel.Location = Location;
			ParentNotePanel.Height = Height;
			ParentNotePanel.Width = Width;
			ParentNotePanel.Dock = this.Dock;
			ParentNotePanel.Visible = this.Visible;
			SetSaveRequired(true);


		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public virtual string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Label");
		}
		public void Minimize()
		{
			// perform an actual minimize
			this.Visible = false;
			UpdateLocation ();
		}
/// <summary>
/// Maximize the specified Maximize.
/// </summary>
/// <param name='Maximize'>
/// If set to <c>true</c> maximize.
/// </param>
		public void Maximize (bool Maximize)
		{
			lg.Instance.Line("Maximize", ProblemType.MESSAGE, String.Format ("Calling Maximize for note with GUID = {0} and Parent LayoutPanel GUID Of {1}",this.GuidForNote, Layout.GUID ));
			// is this actually dock=none/bringtofront, full width?
			if (true == Maximize) {
				ParentNotePanel.Dock = DockStyle.None;
//				Dock = System.Windows.Forms.DockStyle.None;
				// temporary size change (change the form, not the XML)
				ParentNotePanel.Location = new Point (0, 0);
				ParentNotePanel.Height = Layout.Height - 15;
				ParentNotePanel.Width = Layout.Width - 15;
				ParentNotePanel.BringToFront ();
			} else {
				// restore defaults
				this.Dock = DockStyle.None;
			//	ParentNotePanel.Dock = DockStyle.Fill;
				UpdateLocation ();
			}

			//UpdateLocation ();

		}
		/// <summary>
		/// Compares to.
		/// Implements the interface
		/// </summary>
		/// <returns>
		/// The to.
		/// </returns>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public int CompareTo (object obj)
		{
			if (obj == null)
				return 1;
			NoteDataXML otherNote = obj as NoteDataXML;
			if (otherNote != null) {
				return this.Caption.CompareTo (otherNote.Caption);
			} else {
				throw new Exception("Object is a NoteDataXML");
			}
		}

		public string GetAbsoluteParent()
		{
			return Layout.GetAbsoluteParent().GUID;
		}

	

#endregion;

		
	}
}

