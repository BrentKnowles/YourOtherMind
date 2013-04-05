// Images.cs
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
using System.Collections.Generic;
using System.Text;
// - Using
using System.Collections;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Drawing;


using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;

using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
namespace CoreUtilities
{
	public static partial class General
	{
		////////////////////////////////////////////////////////////////////
		//
		// Image Handling
		//
		////////////////////////////////////////////////////////////////////
		
		
		
		
		/// <summary>
		/// returns true if the extension for sFile matches a list of possible file names
		/// </summary>
		/// <param name="sFile"></param>
		/// <returns></returns>
		static public bool IsGraphicFile(string sFile)
		{
			if (sFile == null)
			{
				throw new Exception("You are attempting to test IsGraphicFile but you passed null to it.");
			}
			
			sFile = sFile.ToLower();
			
			if (sFile.IndexOf(".png") > -1 || sFile.IndexOf(".jpg") > -1 || sFile.IndexOf(".jpeg") > -1
			    || sFile.IndexOf(".bmp") > -1 || sFile.IndexOf(".gif") > -1)
			{
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Return the available image encoders
		/// http://www.codeproject.com/KB/cs/BuildWatermarkUtility.aspx
		/// </summary>
		/// <param name="mimeType"></param>
		/// <returns></returns>
		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for (j = 0; j < encoders.Length; ++j)
			{
				if (encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}
		
		
		
		
		/// <summary>
		/// for use with IdeaRiver, a collection of smartpoints so
		/// we know where we are clicking
		/// </summary>
		public struct smartpoint
		{
			public int x; public int width;
			public int y; public int height;
			
			public string sLink;
			public bool bExtra; // extra info, like for usage in Idea Flow brainstorming
			public int nSpeed; //extra for idea flow
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sSource"></param>
		/// <param name="nWidth"></param>
		/// <param name="nLines"></param>
		/// <returns></returns>
		public static string ReformatStringForWidth(string sSource, int nWidth)
		{
			return ReformatStringForWidth(sSource, nWidth, -1);
		}
		
		/// <summary>
		/// Goes through sSource and inserts \r\n at the specified widht. Every nWidth
		/// characters. This iwll allow it to be passed into CreateBitMapImageFromText with nicer results
		/// </summary>
		/// <param name="sSource"></param>
		/// <param name="nLines">if not -1 will only parse this many lines</param>
		/// 
		/// <returns></returns>
		public static string ReformatStringForWidth(string sSource, int nWidth, int nLines)
		{
			string sNew = "";
			
			if (sSource == null)
			{
				throw new Exception("ReformatStringForWidth sSource was null");
			}
			
			int nCount = 0;
			for (int i = 0; i < sSource.Length; i++)
			{
				nCount++;
				if (nCount == nWidth)
				{
					sNew = sNew + "\r\n";
					nCount = 0;
				}
				
				sNew = sNew + sSource[i];
				if (nLines != -1 && nCount >= nLines)
				{
					break;
				}
			}
			return sNew;
		}
		
		
		/// <summary>
		/// Creates a bitmap from text
		/// http://chiragrdarji.wordpress.com/2008/05/09/generate-image-from-text-using-c-or-convert-text-in-to-image-using-c/
		/// </summary>
		/// <param name="sImageText"></param>
		/// <param name="sTitle"></param>
		/// <param name="style">What should it look like?</param>
		/// <param name="nLength">How much text to show, -1 means full length</param>
		/// <returns></returns>
		public static Bitmap CreateBitmapImageFromText(string sTitle, string sImageText, 
		                                               FromTextStyles style, int nLength, TextToImageAppearance appearance, Image backImage)
		{
			if (sTitle == null || sImageText == null)
			{
				throw new Exception("Create Bitmap IMage from text You must supply both text and a title");
			}
			
			// truncate text based on limit
			if (nLength != -1)
			{
				if (nLength > sImageText.Length)
				{
					nLength = sImageText.Length;
				}
				sImageText = sImageText.Substring(0, nLength);
			}
			
			Bitmap objBmpImage = new Bitmap(1, 1);
			int intWidth = 0;
			int intHeight = 0;
			
			Color background = Color.Black;
			Color textcolor = Color.Black;
			Font objFont = null;
			Font titleFont = null;
			
			// appearance is passed in
			if (appearance == null)
			{
				appearance = new TextToImageAppearance();
			}
			appearance.Set(style);       
			
			
			objFont = new Font(appearance.MainTextFont, appearance.MainTextFont.Style);
			titleFont = new Font(appearance.TitleFont, appearance.TitleFont.Style);
			background = appearance.BackgroundColor;
			textcolor = appearance.TextColor;
			
			if (objFont == null || titleFont == null)
			{
				//Logs.Line("GeneralImage.CreateBitmapImageFromText", "FromText font null","", Logs.CRITICAL);
				lg.Instance.Line("CreateBitMapImageFromText", ProblemType.WARNING, "FromText font null");
				return null;
			}
			
			// Create the Font object for the image text drawing.   
			
			// Create a graphics object to measure the text's width and height.  
			Graphics objGraphics = Graphics.FromImage(objBmpImage);
			// This is where the bitmap size is determined.  
			intWidth = Math.Max((int)objGraphics.MeasureString(sImageText, objFont).Width,
			                    ((int)objGraphics.MeasureString(sTitle, titleFont).Width));
			intHeight = ((int)objGraphics.MeasureString(sImageText, objFont).Height)
				+ ((int)objGraphics.MeasureString(sTitle, titleFont).Height);
			
			// Create the bmpImage again with the correct size for the text and font.  
			objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
			// Add the colors to the new bitmap.  
			objGraphics = Graphics.FromImage(objBmpImage);
			
			
			
			
			// Set Background color 
			objGraphics.Clear(background);
			
			/* Gradient Test */
			
			
			
			
			if (backImage != null)
			{
				objGraphics.DrawImage(backImage, 0, 0, intWidth, intHeight);
			}
			
			else //back image is mutually exclusive with gradient
				
				if (appearance.IsGradient == true)
			{    // Create a diagonal linear gradient with four stops.   
				Rectangle rect = new Rectangle(0, 0, intWidth, intHeight);
				Color gradientColorOne = appearance.GradientColor1;
				Color gradientColorTwo = appearance.GradientColor2;
				
				
				// Dispose of brush resources after use
				using (LinearGradientBrush lgb = new LinearGradientBrush(rect,
				                                                         gradientColorOne, gradientColorTwo, appearance.mLinearGradientMode))
					
					objGraphics.FillRectangle(lgb, rect);
				
				
			}
			/* end gradient test*/
			
			
			objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
			objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			
			objGraphics.DrawString(sTitle, titleFont, new SolidBrush(appearance.TitleColor), 0, 0);
			
			// put the body of the text below the title
			objGraphics.DrawString(sImageText, objFont, new SolidBrush(textcolor), 0,
			                       ((int)objGraphics.MeasureString(sTitle, titleFont).Height));
			
			
			
			
			
			
			
			objGraphics.Flush();
			return (objBmpImage);
		}
		/// <summary>
		/// simpler wrapper
		/// </summary>
		/// <param name="imageSource"></param>
		/// <param name="frameColor"></param>
		/// <param name="nFramewidth"></param>
		/// <returns></returns>
		public static Image AddFrameToImage(Image imageSource, Color frameColor, int nFramewidth)
		{
			return AddFrameToImage(imageSource, frameColor, nFramewidth, nFramewidth, nFramewidth, nFramewidth,
			                       false, Color.White);
		}
		/// <summary>
		/// Returns am odified image with the Frame specified around it
		/// </summary>
		/// <param name="imageSource"></param>
		/// <returns></returns>
		public static Image AddFrameToImage(Image imageSource, Color frameColor, int nTopWidth,
		                                    int nLeftWidth, int nRightWidth, int nBottomWidth, bool bGradient, Color gradientColor)
		{
			if (imageSource == null)
			{
				throw new Exception("AddFrame - imageSource was null");
			}
			Bitmap bmp = new Bitmap(imageSource);
			
			Graphics frame = Graphics.FromImage(bmp);
			
			
			
			try
			{
				LinearGradientBrush lgb = null;
				Pen pen = null;
				
				if (bGradient == true)
				{
					lgb = new LinearGradientBrush(
						new Rectangle(0, 0, nLeftWidth * 2, bmp.Height),
						frameColor, gradientColor, LinearGradientMode.ForwardDiagonal);
					pen = new Pen(lgb, nLeftWidth * 2);  
				}
				else
				{
					
					pen = new Pen(frameColor, nLeftWidth * 2);
				}
				
				
				
				frame.DrawLine(pen, 0, 0, 0, bmp.Height);   // LEFT 
				
				
				
				if (bGradient == true)
				{
					lgb = new LinearGradientBrush(
						new Rectangle(0, 0, bmp.Width, nTopWidth * 2),
						frameColor, gradientColor, LinearGradientMode.ForwardDiagonal);
					pen = new Pen(lgb, nTopWidth * 2);
				}
				else
				{
					
					pen = new Pen(frameColor, nTopWidth * 2);
				}
				
				
				frame.DrawLine(pen, 0, 0, bmp.Width, 0);    // BOTTOM
				
				
				if (bGradient == true)
				{
					lgb = new LinearGradientBrush(
						new Rectangle(bmp.Width - nRightWidth, 0, bmp.Width-nRightWidth, bmp.Height),
						frameColor, gradientColor, LinearGradientMode.ForwardDiagonal);
					pen = new Pen(lgb,nRightWidth * 2);
				}
				else
				{
					
					pen = new Pen(frameColor, nRightWidth * 2);
				}
				
				//pen = new Pen(frameColor, nRightWidth * 2);
				frame.DrawLine(pen, bmp.Width, 0, bmp.Width, bmp.Height); // RIGHT
				
				
				
				if (bGradient == true)
				{
					lgb = new LinearGradientBrush(
						new Rectangle(0, bmp.Height-nBottomWidth, bmp.Width, bmp.Height),
						frameColor, gradientColor, LinearGradientMode.ForwardDiagonal);
					pen = new Pen(lgb, nBottomWidth * 2);
				}
				else
				{
					
					pen = new Pen(frameColor, nBottomWidth * 2);
				}
				
				
				frame.DrawLine(pen, 0, bmp.Height, bmp.Width, bmp.Height); // TOP
				
			}
			catch (Exception ex)
			{
				CoreUtilities.NewMessage.Show(ex.ToString());
			}
			frame.Dispose();
			
			return bmp;
		}
		
		
		/// <summary>
		/// Resize with no padding
		/// </summary>
		/// <param name="img"></param>
		/// <param name="percentage"></param>
		/// <returns></returns>
		public static Image ResizeImage(Image img, float percentage)
		{
			// resize with no padding
			return ResizeImage(img, percentage,percentage,  0,0,0,0,Color.White);
		}
		/// <summary>
		/// method for resizing an image
		/// </summary>
		/// <param name="img">the image to resize</param>
		/// <param name="percentage">Percentage of change (i.e for 105% of the original provide 105)</param>
		/// <param name="nPadding">padding is for frames</param>
		/// <returns></returns>
		public static Image ResizeImage(Image img, float widthpercentage, float heighpercentage,
		                                /*int nPadding, */
		                                int nLeftPad, int nTopPad, int nBottomPad, int nRightPad,
		                                Color backColor)
		{
			//get the height and width of the image
			int originalW = img.Width;
			int originalH = img.Height;
			
			//get the new size based on the percentage change
			/*   int resizedW = (int)(originalW * percentage);// +(nPadding * 2);
            int resizedH = (int)(originalH * percentage);// +(nPadding * 2);
            */
			
			int resizedW = (int)(originalW * widthpercentage);// +(nPadding * 2);
			int resizedH = (int)(originalH * heighpercentage);// +(nPadding * 2);
			//create a new Bitmap the size of the new image
			Bitmap bmp = new Bitmap(resizedW, resizedH);
			//create a new graphic from the Bitmap
			Graphics graphic = Graphics.FromImage((Image)bmp);
			graphic.InterpolationMode = InterpolationMode.HighQualityBilinear;
			//draw the newly resized image
			
			// Brent added this to introduce padding for the frame
			graphic.Clear(Color.Pink);
			
			
			/*     graphic.DrawImage(img, nPadding / 2, nPadding / 2, 
                resizedW - nPadding, 
                resizedH - nPadding);*/
			
			/*  int nLeftPad = 100;
            int nTopPad = 10;
            int nRightPad = 20;
            int nBottomPad = 30;*/
			graphic.DrawImage(img,
			                  nLeftPad,
			                  nTopPad,
			                  originalW/* - ((nLeftPad + nRightPad)/2)*/,
			                  originalH /*- ((nTopPad + nBottomPad)/2*/
			                  );
			/* resizedW - nLeftPad - nRightPad,
                resizedH - nTopPad - nBottomPad);
*/
			
			//dispose and free up the resources
			graphic.Dispose();
			//return the image
			return (Image)bmp;
		}
		/// <summary>
		/// Creates a new Image containing the same image only rotated
		/// 
		/// http://www.opensource.org/licenses/bsd-license.php
		/// 
		/// </summary>
		/// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
		/// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
		/// <returns>A new <see cref="System.Drawing.Bitmap"/> that is just large enough
		/// to contain the rotated image without cutting any corners off.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
		public static Bitmap RotateImageImproved(Image image, float angle)
		{
			if (image == null)
				throw new ArgumentNullException("image");
			
			const double pi2 = Math.PI / 2.0;
			
			// Why can't C# allow these to be const, or at least readonly
			// *sigh*  I'm starting to talk like Christian Graus :omg:
			double oldWidth = (double)image.Width;
			double oldHeight = (double)image.Height;
			
			// Convert degrees to radians
			double theta = ((double)angle) * Math.PI / 180.0;
			double locked_theta = theta;
			
			// Ensure theta is now [0, 2pi)
			while (locked_theta < 0.0)
				locked_theta += 2 * Math.PI;
			
			double newWidth, newHeight;
			int nWidth, nHeight; // The newWidth/newHeight expressed as ints
			
			#region Explaination of the calculations
			/*
			 * The trig involved in calculating the new width and height
			 * is fairly simple; the hard part was remembering that when 
			 * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
			 * height are switched.
			 * 
			 * When you rotate a rectangle, r, the bounding box surrounding r
			 * contains for right-triangles of empty space.  Each of the 
			 * triangles hypotenuse's are a known length, either the width or
			 * the height of r.  Because we know the length of the hypotenuse
			 * and we have a known angle of rotation, we can use the trig
			 * function identities to find the length of the other two sides.
			 * 
			 * sine = opposite/hypotenuse
			 * cosine = adjacent/hypotenuse
			 * 
			 * solving for the unknown we get
			 * 
			 * opposite = sine * hypotenuse
			 * adjacent = cosine * hypotenuse
			 * 
			 * Another interesting point about these triangles is that there
			 * are only two different triangles. The proof for which is easy
			 * to see, but its been too long since I've written a proof that
			 * I can't explain it well enough to want to publish it.  
			 * 
			 * Just trust me when I say the triangles formed by the lengths 
			 * width are always the same (for a given theta) and the same 
			 * goes for the height of r.
			 * 
			 * Rather than associate the opposite/adjacent sides with the
			 * width and height of the original bitmap, I'll associate them
			 * based on their position.
			 * 
			 * adjacent/oppositeTop will refer to the triangles making up the 
			 * upper right and lower left corners
			 * 
			 * adjacent/oppositeBottom will refer to the triangles making up 
			 * the upper left and lower right corners
			 * 
			 * The names are based on the right side corners, because thats 
			 * where I did my work on paper (the right side).
			 * 
			 * Now if you draw this out, you will see that the width of the 
			 * bounding box is calculated by adding together adjacentTop and 
			 * oppositeBottom while the height is calculate by adding 
			 * together adjacentBottom and oppositeTop.
			 */
#endregion
			
			double adjacentTop, oppositeTop;
			double adjacentBottom, oppositeBottom;
			
			// We need to calculate the sides of the triangles based
			// on how much rotation is being done to the bitmap.
			//   Refer to the first paragraph in the explaination above for 
			//   reasons why.
			if ((locked_theta >= 0.0 && locked_theta < pi2) ||
			    (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
			{
				adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
				oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
				
				adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
				oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
			}
			else
			{
				adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
				oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
				
				adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
				oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
			}
			
			newWidth = adjacentTop + oppositeBottom;
			newHeight = adjacentBottom + oppositeTop;
			
			nWidth = (int)Math.Ceiling(newWidth);
			nHeight = (int)Math.Ceiling(newHeight);
			
			Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);
			
			using (Graphics g = Graphics.FromImage(rotatedBmp))
			{
				// This array will be used to pass in the three points that 
				// make up the rotated image
				Point[] points;
				
				/*
                 * The values of opposite/adjacentTop/Bottom are referring to 
                 * fixed locations instead of in relation to the
                 * rotating image so I need to change which values are used
                 * based on the how much the image is rotating.
                 * 
                 * For each point, one of the coordinates will always be 0, 
                 * nWidth, or nHeight.  This because the Bitmap we are drawing on
                 * is the bounding box for the rotated bitmap.  If both of the 
                 * corrdinates for any of the given points wasn't in the set above
                 * then the bitmap we are drawing on WOULDN'T be the bounding box
                 * as required.
                 */
				if (locked_theta >= 0.0 && locked_theta < pi2)
				{
					points = new Point[] { 
						new Point( (int) oppositeBottom, 0 ), 
						new Point( nWidth, (int) oppositeTop ),
						new Point( 0, (int) adjacentBottom )
					};
					
				}
				else if (locked_theta >= pi2 && locked_theta < Math.PI)
				{
					points = new Point[] { 
						new Point( nWidth, (int) oppositeTop ),
						new Point( (int) adjacentTop, nHeight ),
						new Point( (int) oppositeBottom, 0 )						 
					};
				}
				else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))
				{
					points = new Point[] { 
						new Point( (int) adjacentTop, nHeight ), 
						new Point( 0, (int) adjacentBottom ),
						new Point( nWidth, (int) oppositeTop )
					};
				}
				else
				{
					points = new Point[] { 
						new Point( 0, (int) adjacentBottom ), 
						new Point( (int) oppositeBottom, 0 ),
						new Point( (int) adjacentTop, nHeight )		
					};
				}
				
				g.DrawImage(image, points);
			}
			
			if (image.Tag != null)
			{
				rotatedBmp.Tag = image.Tag;
			}
			
			return rotatedBmp;
		}
		
		
		/// <summary>
		/// Retrieves an embedded resource
		/// </summary>
		/// <param name="sResourceID">i.e., "MyNamespace.Resources.MyImage.bmp" 
		/// General.GetImageFromResource("CoreUtilities.Resources.corksource5.jpg");</param>
		/// <returns></returns>
		public static Image GetImageFromResource(Assembly _assembly, string sResourceID)
		{
			if (sResourceID == null || sResourceID == "")
			{
				throw new Exception("GetImageFromResource sResourceID was null or blank.");
			}
			
			// Assembly _assembly;
			Stream _imageStream;
			
			lg.Instance.Line("GeneralImages.GetImageFromResource", ProblemType.MESSAGE,String.Format (" retrieve image {0}", sResourceID));
			try
			{
				//  _assembly = Assembly.GetExecutingAssembly();
				if (_assembly != null)
				{
					_imageStream = _assembly.GetManifestResourceStream(sResourceID);
					if (_imageStream != null)
					{
						Image i = new Bitmap(_imageStream);
						if (i != null)
						{
							return i;
						}
						else
						{
							lg.Instance.Line("GeneralImages.GetImageFromResource", ProblemType.WARNING,"image was null");
						}
						
					}
					else
					{
						lg.Instance.Line("GeneralImages.GetImageFromResource",ProblemType.WARNING, String.Format (" _imageStream was null for >< {0}", sResourceID));
					}
					
					
				}
			}
			catch (Exception ex)
			{
				lg.Instance.Line("GetImageFromResource", ProblemType.EXCEPTION, ex.ToString());
			}
			return null;
			
		}
		
		
		////////////////////////////////////////////////////////////////////////
		//
		// TextToImageAPpearance
		// 
		//
		////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Predefined styles. Each will correspond to an embedded Appearance Object (eventually)
		/// </summary>
		public enum FromTextStyles
		{
			NEWSPAPER = 0,
			NOTEPAD = 1,
			CUSTOM = 2,
			PAPER = 3,
			BLUETAG = 4,
			POLAROID =5,
			JUSTGRADIENT=6,
			CORK=7
		}
		
		
		
		/// <summary>
		/// this class defines the parameters that can be used to tweak the look
		/// for text that is converted into an image.
		/// 
		/// Original usage: IdeaRiver
		/// -
		/// </summary>
		public class TextToImageAppearance : Object
			
		{
			private Font titleFont;
			private Font mainTextFont; // main text font
			private Color backgroundColor;
			private Color textcolor;
			
			private int leftWidth, topWidth, bottomWidth, rightWidth, resizeWidth;
			private Color frameColor;
			
			
			private bool isGradient;
			private Color gradientColor1;
			private Color gradientColor2;
			private LinearGradientMode mlinearGradientMode;
			private Color frameGradient;
			private string backImageResourceName;
			
			
			
			
			/// <summary>
			/// sets up some default values
			/// </summary>
			public TextToImageAppearance()
			{
				titleFont = new Font("Times", 12);
				mainTextFont = new Font("Times", 10);
				backgroundColor = Color.Blue;
				textcolor = Color.BlanchedAlmond;
				
				frameColor = Color.DarkGray;
				leftWidth = 10;
				topWidth = 10;
				bottomWidth = 10;
				rightWidth = 10;
				resizeWidth = 10;
				isGradient = false;
				
				backImageResourceName = "";
				
				
			}
			/// <summary>
			/// If defined represents an internal ID to an embedded graphics resource
			/// </summary>
			public string BackImageResourceName
			{
				get { return backImageResourceName; }
				set { backImageResourceName = value; }
				
			}
			
			/// <summary>
			/// if gradient set to true frame can have gradient too
			/// </summary>
			public Color FrameGradient
			{
				get { return frameGradient; }
				set { frameGradient = value; }
			}
			
			/// <summary>
			/// if true will attempt to paint a linear gradient
			/// </summary>
			public bool IsGradient
			{
				get { return isGradient; }
				set { isGradient = value; }
			}
			public LinearGradientMode mLinearGradientMode
			{
				get { return mlinearGradientMode; }
				set { mlinearGradientMode = value; }
			}
			public Color GradientColor1
			{
				get { return gradientColor1; }
				set { gradientColor1 = value; }
			}
			public Color GradientColor2
			{
				get { return gradientColor2; }
				set { gradientColor2 = value; }
			}
			
			/// <summary>
			/// the color of the frame around the image
			/// </summary>
			public Color FrameColor
			{
				get { return frameColor; }
				set {frameColor = value;}
			}
			
			/// <summary>
			/// rectangle defining the thicnkness of a the frame
			/// </summary>
			public int LeftWidth
			{
				get {return leftWidth;}
				set { leftWidth = value; }
				
				
			}
			/// <summary>
			/// rectangle defining the thicnkness of a the frame
			/// </summary>
			public int RightWidth
			{
				get { return rightWidth; }
				set { rightWidth = value; }
				
				
			}
			/// <summary>
			/// rectangle defining the thicnkness of a the frame
			/// </summary>
			public int TopWidth
			{
				get { return topWidth; }
				set { topWidth = value; }
				
				
			}
			
			/// <summary>
			/// rectangle defining the thicnkness of a the frame
			/// </summary>
			public int BottomWidth
			{
				get { return bottomWidth; }
				set { bottomWidth = value; }
				
				
			}
			/// <summary>
			/// This is the width of the resizing to use when creating
			/// panning in the image before applying the frame.
			/// 
			/// To-Do: This may need seperate x,y,width,height params (CANT WE JUST USE OTHER DIMS?)
			/// </summary>
			public int ResizeWidth
			{
				get { return resizeWidth; }
				set { resizeWidth = value; }
				
				
			}
			
			public Font TitleFont
			{
				get { return titleFont; }
				set { titleFont = new Font(value, value.Style); }
			}
			
			public Font MainTextFont
			{
				get { return mainTextFont; }
				set { mainTextFont = new Font(value, value.Style); }
			}
			
			public Color BackgroundColor
			{
				get { return backgroundColor; }
				set { backgroundColor = (value); }
			}
			public Color TextColor
			{
				get { return textcolor; }
				set { textcolor = (value); }
			}
			
			private Color titleColor;
			public Color TitleColor
			{
				get { return titleColor; }
				set { titleColor = value; }
			}
			/// <summary>
			/// override ToString for this object type
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				string sOut =
					"this.TitleFont = new Font(\"{0}\", 24, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);\r\n"
						+ "this.BackgroundColor = Color.FromArgb({1});\r\n"
						+ "this.TextColor = Color.FromArgb({2});\r\n"
						+ "this.MainTextFont = new Font(\"{3}\", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);\r\n"
						+ "this.TitleColor = Color.FromArgb({4});\r\n"
						+ "this.FrameColor = Color.FromArgb({5});\r\n"
						+ "this.LeftWidth = {6};\r\n"
						+"this.TopWidth = {7};\r\n"
						+"this.BottomWidth = {8};\r\n"
						+"this.RightWidth = {9};\r\n"
						+"this.ResizeWidth = {10};\r\n"
						+ "this.IsGradient = {11};\r\n"
						+ "this.GradientColor1 = Color.FromArgb({12});\r\n"
						+ "this.GradientColor2 = Color.FromArgb({13});\r\n"
						+ "this.mLinearGradientMode = LinearGradientMode.{14};\r\n"
						+ "this.frameGradient = Color.FromArgb({15});\r\n"
						
						;
				sOut = String.Format(sOut, this.TitleFont.Name, backgroundColor.ToArgb().ToString(),
				                     textcolor.ToArgb().ToString(), this.MainTextFont.Name, titleColor.ToArgb(),
				                     FrameColor.ToArgb(),
				                     leftWidth, topWidth, bottomWidth,
				                     rightWidth, resizeWidth,
				                     IsGradient.ToString().ToLower(), GradientColor1.ToArgb(), GradientColor2.ToArgb(),
				                     mLinearGradientMode.ToString(),
				                     frameGradient.ToArgb());
				
				return sOut;
			}
			
			
			/// <summary>
			/// Creates a text image box, for IdeaRiver, and returns an image
			/// 
			/// Assumes that formatting setup has already occured
			/// i.e., app = new General.TextToImageAppearance(
			///    (General.TextToImageAppearance)propertyGrid1.SelectedObject);
			/// </summary>
			/// <param name="sTitle"></param>
			/// <param name="sText"></param>
			/// <param name="nWidth"></param>
			/// <param name="nHeight"></param>
			/// <param name="AffectText">if true it will try to format text pretty</param>
			/// <returns></returns>
			public Image CreateFancyImage(string sTitle, string sText, int nWidth, int nHeight, 
			                              Image backImage, bool AffectText)
			{
				if (true == AffectText)
				{
					sText = General.ReformatStringForWidth(sText, nWidth, nHeight);
				}
				
				Image i = null;
				
				if (backImage != null)
				{
					//overriding background image with user passed in
					i = backImage;
				}
				else
					if (BackImageResourceName != "")
				{
					// some appearance types like Cork pull an embedded resource
					// to display it
					i = General.GetImageFromResource(Assembly.GetExecutingAssembly(),  BackImageResourceName);
				}
				
				try
				{
					i =  CoreUtilities.General.CreateBitmapImageFromText(sTitle,
					                                                     sText, (CoreUtilities.General.FromTextStyles.CUSTOM), nHeight, this, i);
					// now resize to prepare for adding a frame
					
					// we need to calculate teh area difference between the old image and
					// the new image.
					/*    float OriginalArea = i.Height * i.Width;
                    float ModifiedArea = (i.Width + (this.LeftWidth + this.RightWidth))
                    * (i.Height + (this.TopWidth + this.BottomWidth));

                    float fPercentWidthDifference = (ModifiedArea / OriginalArea);
                *  Logs.LineF("CreateFancyImage {0} percent difference between new/old areas {1}/{2}",
                        fPercentDifference, ModifiedArea, OriginalArea);
                    */
					float fPercentWidthDifference = (float)(i.Width + this.LeftWidth + this.RightWidth)
						/ (float)i.Width;
					
					if (fPercentWidthDifference < 1.0f) fPercentWidthDifference = 1.0f;
					
					float fPercentHeighDifference = (float)(i.Height + this.TopWidth + this.BottomWidth)
						/ (float)i.Height;
					
					if (fPercentHeighDifference < 1.0f) fPercentHeighDifference = 1.0f;
					
					i = CoreUtilities.General.ResizeImage(i, fPercentWidthDifference,
					                                      fPercentHeighDifference/*1.10f*/, 
					                                      leftWidth, topWidth, bottomWidth, rightWidth,
					                                      frameColor);
					
					
					
					// Padding should match the padding of the frame, likewise the colors
					i = CoreUtilities.General.AddFrameToImage(i, frameColor, topWidth,
					                                          leftWidth, rightWidth, bottomWidth, IsGradient, frameGradient);
				}
				catch (Exception)
				{
					lg.Instance.Line("GeneralImages.CreateFancyImage", ProblemType.EXCEPTION, "CreateFancyImage ERROR with creating image");
				}
				
				return i;
			}
			
			/// <summary>
			/// sets the appearance to a predefined style
			/// </summary>
			/// <param name="style"></param>
			public TextToImageAppearance Set(FromTextStyles style)
			{
				try
				{
					switch (style)
					{
					case FromTextStyles.NEWSPAPER:
					{
						
						this.TitleFont = new Font("Arial", 24, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.BackgroundColor = Color.FromArgb(-2302756);
						this.TextColor = Color.FromArgb(-10066330);
						this.MainTextFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.TitleColor = Color.FromArgb(-10066330);
						this.FrameColor = Color.FromArgb(-657931);
						this.LeftWidth = 5;
						this.TopWidth = 5;
						this.BottomWidth = 5;
						this.RightWidth = 5;
						this.ResizeWidth = 5;
						this.IsGradient = true;
						this.GradientColor1 = Color.FromArgb(-657931);
						this.GradientColor2 = Color.FromArgb(-2302756);
						this.mLinearGradientMode = LinearGradientMode.ForwardDiagonal;
						this.frameGradient = Color.FromArgb(-2039584);
						
						
					}
						break;
					case FromTextStyles.NOTEPAD:
					{
						
						this.TitleFont = new Font("Arial", 24, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.BackgroundColor = Color.FromArgb(-256);
						this.TextColor = Color.FromArgb(-16777216);
						this.MainTextFont = new Font("Microsoft Sans Serif", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.TitleColor = Color.FromArgb(-16777216);
						this.FrameColor = Color.FromArgb(-256);
						this.LeftWidth = 1;
						this.TopWidth = 1;
						this.BottomWidth = 1;
						this.RightWidth = 1;
						this.ResizeWidth = 1;
						this.IsGradient = true;
						this.GradientColor1 = Color.FromArgb(-32);
						this.GradientColor2 = Color.FromArgb(-256);
						this.mLinearGradientMode = LinearGradientMode.ForwardDiagonal;
						
						
						
						
					} break;
						
						
					case FromTextStyles.PAPER:
					{
						BackImageResourceName = "CoreUtilities.Resources.lined4.jpg";
						
						this.TitleFont = new Font("Monotype Corsiva", 24, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
						this.BackgroundColor = Color.FromArgb(16777215);
						this.TextColor = Color.FromArgb(-12490271);
						this.MainTextFont = new Font("Monotype Corsiva", 20, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
						this.TitleColor = Color.FromArgb(-16777088);
						this.FrameColor = Color.FromArgb(-16777216);
						this.LeftWidth = 1;
						this.TopWidth = 1;
						this.BottomWidth = 1;
						this.RightWidth = 1;
						this.ResizeWidth = 10;
						this.IsGradient = false;
						this.GradientColor1 = Color.FromArgb(0);
						this.GradientColor2 = Color.FromArgb(0);
						this.mLinearGradientMode = LinearGradientMode.Horizontal;
						this.frameGradient = Color.FromArgb(0);
						
						
						
						
					} break;
					case FromTextStyles.BLUETAG:
					{
						this.TitleFont = new Font("Arial", 24, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.BackgroundColor = Color.FromArgb(-657931);
						this.TextColor = Color.FromArgb(-657931);
						this.MainTextFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.TitleColor = Color.FromArgb(-7876885);
						this.FrameColor = Color.FromArgb(-15132304);
						this.LeftWidth = 30;
						this.TopWidth = 1;
						this.BottomWidth = 1;
						this.RightWidth = 1;
						this.ResizeWidth = 1;
						this.IsGradient = true;
						this.GradientColor1 = Color.FromArgb(-16777216);
						this.GradientColor2 = Color.FromArgb(-15132304);
						this.mLinearGradientMode = LinearGradientMode.Horizontal;
						this.frameGradient = Color.FromArgb(-10185235);
						
						
						
					} break;
					case FromTextStyles.POLAROID:
					{
						this.TitleFont = new Font("Arial", 24, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.BackgroundColor = Color.FromArgb(-1);
						this.TextColor = Color.FromArgb(-657931);
						this.MainTextFont = new Font("Times New Roman", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.TitleColor = Color.FromArgb(-657931);
						this.FrameColor = Color.FromArgb(-657931);
						this.LeftWidth = 5;
						this.TopWidth = 10;
						this.BottomWidth = 50;
						this.RightWidth = 5;
						this.ResizeWidth = 10;
						this.IsGradient = true;
						this.GradientColor1 = Color.FromArgb(-16777216);
						this.GradientColor2 = Color.FromArgb(-9868951);
						this.mLinearGradientMode = LinearGradientMode.Horizontal;
						this.frameGradient = Color.FromArgb(-657931);
						
						
						
					} break;
					case FromTextStyles.JUSTGRADIENT:
					{
						this.TitleFont = new Font("Arial", 24, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.BackgroundColor = Color.FromArgb(-657931);
						this.TextColor = Color.FromArgb(-657931);
						this.MainTextFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.TitleColor = Color.FromArgb(-7876885);
						this.FrameColor = Color.FromArgb(-15132304);
						this.LeftWidth = 40;
						this.TopWidth = 10;
						this.BottomWidth = 10;
						this.RightWidth = 10;
						this.ResizeWidth = 1;
						this.IsGradient = true;
						this.GradientColor1 = Color.FromArgb(-16777216);
						this.GradientColor2 = Color.FromArgb(-15132304);
						this.mLinearGradientMode = LinearGradientMode.Horizontal;
						this.frameGradient = Color.FromArgb(-32640);
					} break;
					case FromTextStyles.CORK:
					{
						this.TitleFont = new Font("Times New Roman", 24, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.BackgroundColor = Color.FromArgb(-16776961);
						this.TextColor = Color.FromArgb(-5171);
						this.MainTextFont = new Font("Trebuchet MS", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
						this.TitleColor = Color.FromArgb(-2039584);
						this.FrameColor = Color.FromArgb(-16777216);
						this.LeftWidth = 1;
						this.TopWidth = 1;
						this.BottomWidth = 1;
						this.RightWidth = 1;
						this.ResizeWidth = 10;
						this.IsGradient = false;
						this.GradientColor1 = Color.FromArgb(0);
						this.GradientColor2 = Color.FromArgb(0);
						this.mLinearGradientMode = LinearGradientMode.Horizontal;
						this.frameGradient = Color.FromArgb(0);
						
						
						BackImageResourceName = "CoreUtilities.Resources.corksource5.jpg";
					}
						break;
					case FromTextStyles.CUSTOM:
					{
						// do nothing, will be assumed that the user has done something with this.
					}
						break;
					}
				}
				catch (Exception ex)
				{
					lg.Instance.Line("GeneralImages.Set", ProblemType.EXCEPTION, ex.ToString());
				}
				return this;
			}
			
		} //TextToImageAPpearance
		
		/// <summary>
		/// Generates an appropriate point based on the specified dockstyle. Used when drawing custom RICHTEXT controls
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="dockStyle"></param>
		public static Point BuildLocationForOverlayText(Point pos, DockStyle dockStyle, string Text)
		{
			switch (dockStyle)
			{
			case DockStyle.Fill: 
				// basicallY ONTOP of the text
				return new Point(pos.X, pos.Y + 10);

			case DockStyle.Right:
				return new Point(pos.X + (Text.Length * 15), pos.Y + 10);

			case DockStyle.Bottom:
				return new Point(pos.X, pos.Y + 25);

			case DockStyle.Top:
				return new Point(pos.X, pos.Y - 15);

				
			}
			return new Point(0, 0);
		}
	} //  - generalimages
	
	
	
}
