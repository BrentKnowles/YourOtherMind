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
	public partial class NoteDataXML : NoteDataInterface
	{
		#region constants
		const string BLANK = "";
		#endregion

		#region variables_frominterface
		//TODO was trying to avoid storing a reference to actual Layout but the need to update the list (and handle the Property grid) made it so)
		protected LayoutPanelBase Layout;

		public virtual bool IsSystemNote{
			get {return false;}
		}
		private void CommonConstructorBehavior ()
		{
			Caption = Loc.Instance.Cat.GetString("Blank Note");
			GuidForNote = System.Guid.NewGuid().ToString();
		}
		public NoteDataXML () 
		{
			CommonConstructorBehavior ();
		
		}
		public NoteDataXML(int _height, int _width)
		{
			CommonConstructorBehavior ();
			height = _height;
			width = _width;
		}
		private NotePanel parent;
		[XmlIgnore]
		public NotePanel Parent {
			get{ return parent;}
			set{ parent = value;}
		}

		#endregion

		#region xml_save
		private DockStyle dock = DockStyle.None;
		public DockStyle Dock {
			get { return dock;}
			set { dock = value;}
		}

		private string _GuidForNote;
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
		/// <summary>
		/// THIS works but is bad design. The interface is not useful if we add too many custom, public methods
		/// </summary>
		/// <value>
		/// The just XMLONLY test.
		/// </value>
		public string JustXMLONLYTest {
			get { return "boo";}
		}
		#endregion;

		#region control_panel_settings
		public virtual bool IsPanel {
			get { return false;}
		}
#endregion;

		#region methods

		public virtual System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> GetChildNotes()
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
		public void Update (LayoutPanelBase Layout)
		{
			Save ();
			Parent.Dispose();
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
			Parent.Dispose ();
		}


		/// <summary>
		/// Updates the location, called when location is set outside actual movement.
		/// TODO: Generalize this to UpdateAppearance?
		/// Also update sizea
		/// </summary>
		public void UpdateLocation ()
		{
			if (Parent == null) {
				throw new Exception("Parent must not be null");
			}
			Parent.Location = Location;
			Parent.Height = Height;
			Parent.Width = Width;
			Parent.Dock = this.Dock;
			SetSaveRequired(true);


		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public virtual string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Label");
		}

/// <summary>
/// Maximize the specified Maximize.
/// </summary>
/// <param name='Maximize'>
/// If set to <c>true</c> maximize.
/// </param>
		public void Maximize (bool Maximize)
		{
			lg.Instance.Line("Maximize", ProblemType.WARNING, String.Format ("Calling Maximize for note with GUID = {0} and Parent LayoutPanel GUID Of {1}",this.GuidForNote, Layout.GUID ));
			// is this actually dock=none/bringtofront, full width?
			if (true == Maximize) {
				Parent.Dock = DockStyle.None;
//				Dock = System.Windows.Forms.DockStyle.None;
				// temporary size change (change the form, not the XML)
				Parent.Location = new Point (0, 0);
				Parent.Height = Layout.Height - 15;
				Parent.Width = Layout.Width - 15;
				Parent.BringToFront ();
			} else {
				// restore defaults
				Parent.Dock = DockStyle.Fill;
				UpdateLocation ();
			}

			//UpdateLocation ();

		}

#endregion;


	}
}

