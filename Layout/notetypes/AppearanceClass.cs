using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using CoreUtilities;


namespace Layout
{
	// This class is serialized into a column in the Database. Users may add/edit and then select them to modify the appearance of notes
	[XmlRootAttribute("Appearance", Namespace = "", IsNullable = false)]
	public class AppearanceClass
	{
		public AppearanceClass ()
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
			x3 = new System.Xml.Serialization.XmlSerializer (typeof(AppearanceClass)); 
			x3.Serialize (sw, this);
			x3 = null;
			//x3.Serialize (sw, ListAsDataObjectsOfType,ns, "utf-8");
			string result = sw.ToString ();
			sw.Close ();
			return result;
		}
		
		
		public static AppearanceClass SetAsFantasy ()
		{
			AppearanceClass newApp = new AppearanceClass();
			newApp.Name="fantasy";
			newApp.mcaptionFont = "Georgia, 12pt";   //1
			
			newApp.mnHeaderHeight = 28; //2
			newApp.HeaderBorderStyle = BorderStyle.FixedSingle; //3
			newApp.mainPanelBorderStyle = BorderStyle.None; //4
			newApp.mcaptionBackground =-663885; //5
			newApp.mcaptionForeground = -16777216; //6
			newApp.mmainBackground =-663885;  //7
			// TODO: What is this for?
			newApp.UseBackgroundColor = true;
			return newApp;
		}

		public static AppearanceClass SetAsSciFI ()
		{
			
			AppearanceClass newApp = new AppearanceClass();
			newApp.Name = "scifi";
			// doing this inside the class to have access to private members to set them to numbers
			newApp.mcaptionFont = "Verdana, 18pt, style=Bold";
			
			newApp.mnHeaderHeight = 28;
			newApp.HeaderBorderStyle = BorderStyle.None;
			newApp.mainPanelBorderStyle = BorderStyle.None;
			newApp.mcaptionBackground = -12156236;
			//newApp.mcaptionForeground = -2894893;
			newApp.mcaptionForeground = Color.DarkGray.ToArgb();//-2894893;
			newApp.mmainBackground = -2894893;
			// TODO: What is this for?
			newApp.UseBackgroundColor = true;
			return newApp;
		}
		public static AppearanceClass SetAsResearch()
		{
			AppearanceClass newApp = new AppearanceClass();
			newApp.Name = "research";
			// doing this inside the class to have access to private members to set them to numbers
			newApp.mcaptionFont = "Times New Roman, 10pt, style=Bold";
			
			newApp.mnHeaderHeight = 20;
			newApp.HeaderBorderStyle = BorderStyle.FixedSingle;
			newApp.mainPanelBorderStyle = BorderStyle.FixedSingle;
			newApp.mcaptionBackground = -12490271;
			newApp.mcaptionForeground = -16777216;
			newApp.mmainBackground =-984833;
			// TODO: What is this for?
			newApp.UseBackgroundColor = true;
			return newApp;
		}
		public static AppearanceClass SetAsClassic()
		{
			AppearanceClass newApp = new AppearanceClass();
			newApp.Name = "classic";
			// doing this inside the class to have access to private members to set them to numbers
			newApp.mcaptionFont = "Times New Roman, 10pt, style=Bold";

			newApp.mnHeaderHeight = 20;
			newApp.HeaderBorderStyle = BorderStyle.FixedSingle;
			newApp.mainPanelBorderStyle = BorderStyle.FixedSingle;
			newApp.mcaptionBackground = -2987746;
			newApp.mcaptionForeground = -16777216;
			newApp.mmainBackground = Color.White.ToArgb();
			// TODO: What is this for?
			newApp.UseBackgroundColor = true;
			return newApp;

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

		public static AppearanceClass SetAsProgrammer()
		{
			AppearanceClass newApp = new AppearanceClass();
			newApp.Name = "programmer";
			// doing this inside the class to have access to private members to set them to numbers
			newApp.mcaptionFont = "Verdana, 12pt, style=Bold";
			
			newApp.mnHeaderHeight = 20;
			newApp.HeaderBorderStyle = BorderStyle.FixedSingle;
			newApp.mainPanelBorderStyle = BorderStyle.FixedSingle;
newApp.mcaptionBackground =-16777216;
			newApp.mcaptionForeground = -256;
			newApp.mmainBackground =-16777216;
			// TODO: What is this for?
			newApp.UseBackgroundColor = true;
			return newApp;
		}
		public static AppearanceClass SetAsNote()
		{
			AppearanceClass newApp = new AppearanceClass();
			newApp.Name = "note";
			// doing this inside the class to have access to private members to set them to numbers
			newApp.mcaptionFont = "Verdana, 10pt, style=Bold";
			
			newApp.mnHeaderHeight = 20;
			newApp.HeaderBorderStyle = BorderStyle.None;
			newApp.mainPanelBorderStyle = BorderStyle.None;
			newApp.mcaptionBackground =-128;
			newApp.mcaptionForeground = -16777216;
			newApp.mmainBackground =-128;
			// TODO: What is this for?
			newApp.UseBackgroundColor = true;
			return newApp;
		}
		public static AppearanceClass SetAsModern()
		{
			AppearanceClass newApp = new AppearanceClass();
			newApp.Name = "modern";
			// doing this inside the class to have access to private members to set them to numbers
			newApp.mcaptionFont = "Arial Black, 12pt, style=Bold";
			
			newApp.mnHeaderHeight = 28;
			newApp.HeaderBorderStyle = BorderStyle.None;
			newApp.mainPanelBorderStyle = BorderStyle.None;
			newApp.mcaptionBackground =-6972;
			newApp.mcaptionForeground = -16777216;
			newApp.mmainBackground =-1468806;
			// TODO: What is this for?
			newApp.UseBackgroundColor = true;
			return newApp;
		}
		public static AppearanceClass SetAsBlue()
		{
			AppearanceClass newApp = new AppearanceClass();
			newApp.Name = "blue";
			// doing this inside the class to have access to private members to set them to numbers
			newApp.mcaptionFont = "Verdana, 12pt, style=Bold";
			
			newApp.mnHeaderHeight = 26;
			newApp.HeaderBorderStyle = BorderStyle.None;
			newApp.mainPanelBorderStyle = BorderStyle.None;
			newApp.mcaptionBackground =-12490271;
			newApp.mcaptionForeground = -657931;
			newApp.mmainBackground =-12490271;
			// TODO: What is this for?
			newApp.UseBackgroundColor = true;
			return newApp;
		}
	}
}
