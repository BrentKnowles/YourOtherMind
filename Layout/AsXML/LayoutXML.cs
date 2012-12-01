using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Layout
{
	/// <summary>
	/// Effectively the FILE in memory for a particular Layout
	/// </summary>
	public class LayoutXML : LayoutInterface
	{
		//variables
		private List<NoteDataInterface> dataForThisLayout= null;
		// This is the GUID for the page. It comes from
		//  (a) Either set during the Load Method
		//  (b) When a New Layout is Created
		private string LayoutGUID = CoreUtilities.Constants.BLANK;
		public LayoutXML (string GUID)
		{
			dataForThisLayout = new List<NoteDataInterface> ();
			LayoutGUID = GUID;

		}
		/// <summary>
		/// Gets the notes. Gets the notes. (Read-only only so that ONLY this class is able to modify the contents of the list)
		/// </summary>
		/// <returns>
		/// The notes. READ ONLY
		/// </returns>
		public System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> GetNotes()
		{
		//	return (System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> )dataForThisLayout.AsReadOnly();

			//((List<NoteDataInterface>)(dataForThisLayout)).AsReadOnly();
		//	return dataForThisLayout.AsReadOnly();
			// I got XML to export by changing the data type to an actual data type
			// but now this method won't work
			//return null;
			return dataForThisLayout.AsReadOnly();
		}

		/// <summary>
		/// Creates the name of the file.
		/// </summary>
		/// <returns>
		/// The file name.
		/// </returns>
		private string CreateFileName()
		{
			return System.IO.Path.Combine ("",LayoutGUID+".xml");
		}

		public void LoadFrom (string GUID)
		{
			LayoutGUID = GUID;


		}
		/// <summary>
		/// Saves to the XML file.
		/// 
		/// Notice the GUID is NOT allowed to be passed in. It is stored in the document when Loaded or Created
		/// </summary>
		/// <returns>
		/// The to.
		/// </returns>
		public string SaveTo ()
		{
			if (LayoutGUID == CoreUtilities.Constants.BLANK) {
				throw new Exception ("GUID need to be set before calling SaveTo");
			}
			string XMLAsString = CoreUtilities.Constants.BLANK;

			// we are storing a LIST of NoteDataXML objects
			//	CoreUtilities.General.Serialize(dataForThisLayout, CreateFileName());
			try {

				NoteDataXML[] ListAsDataObjectsOfType = new NoteDataXML[dataForThisLayout.Count];
				dataForThisLayout.CopyTo (ListAsDataObjectsOfType);
				
				
				System.Xml.Serialization.XmlSerializer x3 = 
					new System.Xml.Serialization.XmlSerializer (typeof(NoteDataXML[]), 
					                                            new Type[1] {typeof(NoteDataXML)});


				// Okay: I gave up and accept utf-16 encoding
			//http://www.codeproject.com/Articles/58287/XML-Serialization-Tips-Tricks
			// Needed to pass Namespace because I wanted to override encoding, next parameter
				//XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); 
			//	ns.Add ("", "");


				System.IO.StringWriter sw = new System.IO.StringWriter();
				//sw.Encoding = "";
				x3.Serialize (sw, ListAsDataObjectsOfType);
				//x3.Serialize (sw, ListAsDataObjectsOfType,ns, "utf-8");
				XMLAsString =sw.ToString();
				sw.Close ();

				/* Various attempts, got them all to work as I narrowed the right approach down

				// this works (just individual note)
				System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer (typeof(NoteDataXML));
				x.Serialize (Console.Out, (NoteDataXML)dataForThisLayout[0]);


				// this current WORKS (After resolving to a True data type rather than Interface). Collection of notes
				System.Xml.Serialization.XmlSerializer x2 = 
					new System.Xml.Serialization.XmlSerializer (typeof(List<NoteDataXML>), 
					                                            new Type[1] {typeof(NoteDataXML)});
				x2.Serialize (Console.Out, dataForThisLayout);
				// Okay: Was able to serialize NoteDataXML now need to figure out list



				List<NoteDataInterface> test =  new List<NoteDataInterface>();

				// convert the list of interfaces to a list of notes
				// OKAY: This worked too. I can convert all this back to using an Interface 
				NoteDataXML boo = new NoteDataXML();
				boo.Caption = "snakes eat fish";
				test.Add (boo);
				NoteDataXML[] xml2 = new NoteDataXML[test.Count];
				 test.CopyTo (xml2);
				

				System.Xml.Serialization.XmlSerializer x3 = 
					new System.Xml.Serialization.XmlSerializer (typeof(NoteDataXML[]), 
					                                            new Type[1] {typeof(NoteDataXML)});
				x3.Serialize (Console.Out, xml2);
*/
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
			//testthis?
			return XMLAsString;
		}

		/*Not needed?
		private List<NoteDataInterface> DataForThisLayout {
			get { return dataForThisLayout;}
			set { dataForThisLayout = value;}
		}
		*/
		/// <summary>
		/// Add the specified note to the DataForThisLayout list
		/// </summary>
		/// <param name='note'>
		/// Note.
		/// </param>
		public void Add(NoteDataInterface note)
		{
			dataForThisLayout.Add((NoteDataXML)note);
		}
		//would this list actually be a NoteDataList BARTER type of class that stores the List
		//this class is actually what ANOTHER page might call (for Random Tables)
	}
}

