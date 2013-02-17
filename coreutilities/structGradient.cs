using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace CoreUtilities
{
	public  struct structGradient
	{
		int _Color1Int;
		int _Color2Int;
		
		private Color mColor1;
		private Color mColor2;
		private LinearGradientMode mGradientMode;
		
		
		/*	public structGradient(Color c1, Color c2, LinearGradientMode gradient)
				{
					mColor1 = c1;
					mColor2 = c2;
					mGradientMode = gradient;
				}
				public structGradient(structGradient gradient)
				{
					mColor1 = gradient.Color1;
					mColor2 = gradient.Color2;
					mGradientMode = gradient.GradientMode;
				}*/
		
		public int Color1Int
		{
			set { _Color1Int = value;  Color1 = Color.FromArgb(value); }
			get { return _Color1Int; }
		}
		
		[XmlIgnore]
		public Color Color1
		{
			get {return mColor1;}
			set 
			{
				mColor1 = value;;
				_Color1Int = value.ToArgb();
			}
		}
		
		public int Color2Int
		{
			set { _Color2Int = value;  Color2 = Color.FromArgb(value); }
			get { return _Color2Int; }
		}
		[XmlIgnore]
		public Color Color2
		{
			get {return mColor2;}
			set {mColor2 = value; _Color2Int = value.ToArgb();}
		}
		public LinearGradientMode GradientMode
		{
			get {return mGradientMode;}
			set {mGradientMode = value;}
		}
		
		/*public static Complex operator -(Complex c)
			{
				Complex temp = new Complex();
				temp.x = -c.x;
				temp.y = -c.y;
				return temp;
			}*/
		
	}

}

