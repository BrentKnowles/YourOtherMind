using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using CoreUtilities;


namespace Layout
{
	// This class is serialized into a column in the Database. Users may add/edit and then select them to modify the appearance of notes
	[XmlRootAttribute("Appearance", Namespace = "", IsNullable = false)]
	public class Appearance
	{
		public Appearance ()
		{
		}


		string name = Constants.BLANK;
		// this is the key into the db
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}

		private int mnHeaderHeight;
		public int nHeaderHeight 
		{
			get {return mnHeaderHeight;}
			set {mnHeaderHeight = value;}
		}
		
		private BorderStyle mHeaderBorderStyle;
		public	BorderStyle HeaderBorderStyle
		{
			get
			{
				return mHeaderBorderStyle;
			}
			set
			{
				mHeaderBorderStyle = value;
			}
		}
		private	BorderStyle mmainPanelBorderStyle;
		public	BorderStyle mainPanelBorderStyle
		{
			get
			{
				return mmainPanelBorderStyle;
			}
			set
			{
				mmainPanelBorderStyle = value;
			}
		}
		
		
		
		
		
		public string mcaptionFont;
		
		
		// don't output the heading to XML because it won't serialize
		// instead the string behidn the scenes is output
		[XmlIgnoreAttribute()]
		public Font captionFont
		{
			get
			{
				return General.StringToFont(mcaptionFont);
			}
			set
			{
				FontConverter fc = new FontConverter();
				mcaptionFont = (string)fc.ConvertTo(value, typeof(string));
			}
		}
		
		
//		public  string mtextFont;
//		
//		[XmlIgnoreAttribute()]
//		public Font textFont
//		{
//			get
//			{
//				return General.StringToFont(mtextFont);
//			}
//			set
//			{
//				FontConverter fc = new FontConverter();
//				mtextFont = (string)fc.ConvertTo(value, typeof(string));
//			}
//		}
		public string mdayFont;
		
		[XmlIgnoreAttribute()]
		public Font dayFont
		{
			get
			{
				return General.StringToFont(mdayFont);
			}
			set
			{
				FontConverter fc = new FontConverter();
				mdayFont = (string)fc.ConvertTo(value, typeof(string));
			}
		}
		
		
		
		
		[XmlIgnore]
		public Color captionBackground; // what is reade
		
		
		public  int mcaptionBackground // what is stored
		{
			get
			{
				return captionBackground.ToArgb();
			}
			set
			{
				captionBackground = Color.FromArgb(value);
				
			}
		}
		
		[XmlIgnore]
		public Color captionForeground;
		
		public int mcaptionForeground
		{
			get
			{
				return captionForeground.ToArgb(); ;
			}
			set
			{
				captionForeground = Color.FromArgb(value);
			}
		}
		
		[XmlIgnore]
		public Color mainBackground;
		public int mmainBackground
		{
			get
			{
				return mainBackground.ToArgb(); ;
			}
			set
			{
				mainBackground = Color.FromArgb(value); ;
			}
		}
		
//		[XmlIgnore]
//		public  Color textColor;
//		public int mtextColor
//		{
//			get
//			{
//				return textColor.ToArgb(); ; ;
//			}
//			set
//			{
//				textColor = Color.FromArgb(value); ; ;
//			}
//		}
//		
//		[XmlIgnore]
//		public Color timelineZoomOutPanelBackground;
//		public int mtimelineZoomOutPanelBackground
//		{
//			get
//			{
//				return timelineZoomOutPanelBackground.ToArgb(); ;
//			}
//			set
//			{
//				timelineZoomOutPanelBackground = Color.FromArgb(value); ;
//			}
//		}
//		private BorderStyle mrichTextBorderStyle;
//		public BorderStyle richTextBorderStyle
//		{
//			get
//			{
//				return mrichTextBorderStyle;
//			}
//			set
//			{
//				mrichTextBorderStyle = value;
//			}
//		}
//		
//		[XmlIgnore]
//		public Color monthFontColor;
//		public int mmonthFontColor
//		{
//			get
//			{
//				return monthFontColor.ToArgb();
//			}
//			set
//			{
//				monthFontColor = Color.FromArgb(value);
//			}
//		}
//		
//		public string mmonthFont;
//		
//		[XmlIgnoreAttribute()]
//		public Font monthFont
//		{
//			get
//			{
//				return General.StringToFont(mmonthFont);
//			}
//			set
//			{
//				FontConverter fc = new FontConverter();
//				mmonthFont = (string)fc.ConvertTo(value, typeof(string));
//			}
//		}
//		private  structGradient mtimelineGradient;
//		public structGradient timelineGradient
//		{
//			get
//			{
//				return mtimelineGradient;
//			}
//			set
//			{
//				mtimelineGradient = value;
//			}
//		}
		
		
		private bool mUseBackgroundColor = true;
		/// <summary>
		/// If true shape inherits the background color
		/// Otherwise the shape will keep the colors from the art
		/// </summary>
		public bool UseBackgroundColor
		{
			get {return mUseBackgroundColor;}
			set {mUseBackgroundColor = value;}
		}
		private string mImage = "";
		/// <summary>
		/// is the filename (no directory) of the image to use
		/// looks for it in the ShipIcons directory
		/// </summary>
		public string Image
		{
			get { return mImage;}
			set {mImage = value;}
		}

		/// <summary>
		/// Serializes the XML into a string
		/// </summary>
		/// <returns>
		/// The XM.
		/// </returns>
		public string GetAppearanceXML ()
		{
			System.Xml.Serialization.XmlSerializer x3 = null;
			System.IO.StringWriter sw = null;
			sw = new System.IO.StringWriter ();
			//sw.Encoding = "";
			x3 = new System.Xml.Serialization.XmlSerializer (typeof(Appearance)); 
			x3.Serialize (sw, this);
			x3 = null;
			//x3.Serialize (sw, ListAsDataObjectsOfType,ns, "utf-8");
			string result = sw.ToString ();
			sw.Close ();
			return result;
		}
		
		
		public void SetAsFantasy ()
		{
			Name="fantasy";
			mcaptionFont = "Georgia, 12pt";   //1
			
			mnHeaderHeight = 28; //2
			HeaderBorderStyle = BorderStyle.FixedSingle; //3
			mainPanelBorderStyle = BorderStyle.None; //4
			mcaptionBackground =-663885; //5
			mcaptionForeground = -16777216; //6
			mmainBackground = Color.Gray.ToArgb(); //7
			// TODO: What is this for?
			UseBackgroundColor = true;
		}
		public void SetAsClassic()
		{
			Name = "classic";
			// doing this inside the class to have access to private members to set them to numbers
			mcaptionFont = "Times New Roman, 10pt, style=Bold";

			mnHeaderHeight = 16;
			HeaderBorderStyle = BorderStyle.FixedSingle;
			mainPanelBorderStyle = BorderStyle.FixedSingle;
			mcaptionBackground = -2987746;
			mcaptionForeground = -16777216;
			mmainBackground = Color.Gray.ToArgb();
			// TODO: What is this for?
			UseBackgroundColor = true;


			// February 2013

			// I removed the timeline colors from this because it does not make sense
			// what should be happening with timelines is that they instead take the base
			// colors and apply them to the timeline in some way

//			monthFont = new Font("Georgia", 20.0);
//			mtimelineZoomOutPanelBackground = -5383962;
//			mmonthFontColor = -65536;
//			timelineGradient.Color1 = -7876885;
//			timelineGradient .Color2 = -657931;
//			timelineGradient.GradientMode =System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
		


		}
	}
}
