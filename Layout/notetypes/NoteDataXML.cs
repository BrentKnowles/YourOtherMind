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
		public NoteDataXML () 
		{
			Caption = Loc.Instance.Cat.GetString("Blank Note");
			GuidForNote = System.Guid.NewGuid().ToString();
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

		int height = 100;
		int width = 100;
		public int Height { get { return height; } set { height = value; }}
		public int Width { get { return width; } set { width = value; }}

		private Point location = new Point(200,200);
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

		private NotePanel parent;
		[XmlIgnore]
		public NotePanel Parent {
			get{ return parent;}
			set{ parent = value;}
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
		public virtual bool IsPanel {
			get { return false;}
		}
		public virtual System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> GetChildNotes()
		{
			return null;
		}

		public virtual void Save()
		{
			// save UI elements to XML fiellds, *if* necessary
		}

		/// <summary>
		/// Recreates the interface, usually called when something like Caption is changed. Use UpdateLocation for position only changes
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
		}


	

	

	}
}

