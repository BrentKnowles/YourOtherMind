using System;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using CoreUtilities;

namespace Layout
{
	public class RichTextExtended : RichTextBox
	{
//		private iMarkupLanguage markuplanguage;

//		/// <summary>
//		/// Gets or sets the markuplanguage.
//		/// Dicates how the screen draws in Paint
//		/// </summary>
//		/// <value>
//		/// The markuplanguage.
//		/// </value>
//		public iMarkupLanguage Markuplanguage {
//			get {
//				return markuplanguage;
//			}
//			set {
//				markuplanguage = value;
//			}
//		}


		private iMarkupLanguage markupOverride=null;

		public iMarkupLanguage MarkupOverride {
			get {

				if (null == markupOverride)
				{
					markupOverride = LayoutDetails.Instance.GetCurrentMarkup();
				}
				return markupOverride;
			}
			set {
				markupOverride = value;
			}
		}

		public RichTextExtended ()
		{
//			if (_Markup == null) {
//				throw new Exception("A markup language is required");
//			}
//			Markuplanguage = _Markup;
		}

		private bool inhibitPaint = false;
		
		public bool InhibitPaint
		{
			set { inhibitPaint = value; }
		}

		private const int WM_PAINT = 15;
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == WM_PAINT && !inhibitPaint)
			{
				// raise the paint event
				using (Graphics graphic = base.CreateGraphics())
					OnPaint(new PaintEventArgs(graphic,
					                           base.ClientRectangle));
			}
			
		}

		bool showMarkup = true;
		// Replaces the AutoFormatForWriters flag
		// I may not actually this with YOM2013 directly (i.e., store it in RichText)
		// becaues I am already storing the Markup
		public bool ShowMarkup {
			get {
				return showMarkup;
			}
			set {
				showMarkup = value;
			}
		}

		bool showPartsOfSpeechMode = false;

		public bool ShowPartsOfSpeechMode {
			get {
				return showPartsOfSpeechMode;
			}
			set {
				showPartsOfSpeechMode = value;
			}
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			base.OnPaint(e);

			if (ShowMarkup == false) return;

			Point zeroPoint = new Point(0, 0);
			int start = GetCharIndexFromPosition(zeroPoint);
			int end = GetCharIndexFromPosition(new Point(this.ClientSize.Width, this.ClientSize.Height));
			
			if (start >= end) return; // don't paint if there's no size here




			if (showPartsOfSpeechMode == true)
			{
				// paint the parts of speech if in his mode (slow)
				try
				{
					
					string currentword = "";
					Point startofcurrentword = zeroPoint;
					Graphics g;
					
					g = CreateGraphics();
					System.Drawing.Font drawFont = new System.Drawing.Font("Courier", 8);
					System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
					
					
					for (int i = start; i <= (end - start); i++)
					{
						// iterate/
						// when we find a word break we stop building that word?
						if (Text[i] != ' ' && Text[i] != '-' && Text[i] != '.' && Text[i] != '?' && Text[i] != '!'
						    && Text[i] != ';' && Text[i] != ':' && Text[i] != ',')
						{
							currentword = currentword + Text[i];
						}
						else
						{
							// try to avoid blank words clogging things
							if (currentword != null && currentword != " " && currentword != "")
							{
								//   Graphics g;
								
								//  g = CreateGraphics();
								if (g != null)
								{
									
									startofcurrentword = CoreUtilities.General.BuildLocationForOverlayText(startofcurrentword, DockStyle.Fill, currentword);
									int x_modifier = 5;
									if (Char.IsUpper(currentword[0]) == false)
									{
										x_modifier = 15; // middle words need more spacing 
									}
									
									startofcurrentword = new Point(startofcurrentword.X + x_modifier, startofcurrentword.Y + 10);
									g.DrawString("FINISH PORTING THIS", drawFont, drawBrush, startofcurrentword);
									//g.DrawString(GetPartOfSpeech(currentword.Trim(), Speller), drawFont, drawBrush, startofcurrentword);
									
								}
								// clear
								currentword = "";
								startofcurrentword = GetPositionFromCharIndex(i);
								
							}
						}
					}
					drawFont.Dispose();
					drawBrush.Dispose();
					g.Dispose();
				}
				catch (Exception ex)
				{
					NewMessage.Show(String.Format("Failed in parts of speech Start {0} End {1}", start, end) + ex.ToString());
				}
			}



			if (null != markupOverride) {
				markupOverride.DoPaint(e, start, end, this);
			}
			else
				LayoutDetails.Instance.GetCurrentMarkup().DoPaint(e, start, end, this);
		}

	}
}

