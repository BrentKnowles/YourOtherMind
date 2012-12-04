using System;
using System.Xml;
using System.Xml.Serialization;
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
	public class NoteDataXML : NoteDataInterface
	{
		#region constants
		const string BLANK = "";
		#endregion
		#region variables_frominterface
		public NoteDataXML () 
		{
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
	
		private string caption = BLANK;
		public string Caption {
			get {return caption;}
			set{ caption = value;}


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

		public void CreateParent()
		{
		}
		[XmlIgnore]
		public NotePanelInterface Parent {
			get{ return null;}
			set{}
		}
	}
}

