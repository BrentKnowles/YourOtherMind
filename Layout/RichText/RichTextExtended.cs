using System;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using CoreUtilities;
using System.Runtime.InteropServices;
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

		// if we are spell checking a single word 
		// we set this to know the reange we are checking
		// it will be cleaered in the Mispelled Word function
		public int _CurrentSpellSelectionStart = -1;
		public int _CurrentSpellSelectionLength = -1;


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
			this.MouseDown+= RichTextBoxEx_MouseDown;
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
		// for passing into GetNearestSpacer
		//  delegate int IndexOf(char value, int pos); 
		private int GetNearestSpacer(int Position, string text, bool LastLookup)
		{
			// look for nearest seperator
			char[] seps = new char[10] { ' ', '.', ',', '!', '?', '-', ';', ':', '/', '\n' };
			
			int best = int.MaxValue;
			int value = -1;
			// looking for seperator closest to the position (less different between Position and sep.position)
			foreach (char seperator in seps)
			{
				int current = 0;
				if (false == LastLookup)
				{
					current = text.IndexOf(seperator, Position);
				}
				else
				{
					current = text.LastIndexOf(seperator, Position);
				}
				int diff = Math.Abs(current - Position);
				if (diff < best)
				{
					best = diff;
					value = current;
				}
			}
			
			
			
			return value;
		}
		
		/// <summary>
		/// tries to select a word from the psoition indicated
		/// </summary>
		/// <param name="fromPosition"></param>
		public void SelectAsWord(int fromPosition)
		{
			//look for first non-alpha character to left and right?
			int cursorPosition = fromPosition;
			int nextSpace = GetNearestSpacer(cursorPosition, Text, false);//Text.IndexOf(' ', cursorPosition);
			int selectionStart = 0;
			string trimmedString = string.Empty;
			// Strip everything after the next space...
			if (nextSpace != -1)
			{
				trimmedString = Text.Substring(0, nextSpace);
			}
			else
			{
				trimmedString = Text;
			}
			
			// determining the start
			int lastspacer = GetNearestSpacer(trimmedString.Length - 1, trimmedString, true);
			if (  /*trimmedString.LastIndexOf(' ', 0)*/ lastspacer != -1)
			{
				selectionStart = 1 + lastspacer;
				trimmedString = trimmedString.Substring(1 + lastspacer);
			}
			
			SelectionStart = selectionStart;
			SelectionLength = trimmedString.Length;
			
		}
		
		/// <summary>
		/// may 2010 - trying to select word on right-click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void RichTextBoxEx_MouseDown(object sender, MouseEventArgs e)
		{
			// may 16 - don't try selecting if we already ahve a selection
			if (e.Button == MouseButtons.Right && SelectedText == "")
			{
				SelectAsWord(GetCharIndexFromPosition(new Point(e.X, e.Y)));
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

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
		/// <summary>
		/// Flicker resistance
		/// http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/a6abf4e1-e502-4988-a239-a082afedf4a7
		/// </summary>
		public void BeginUpdate()
		{
			SendMessage(this.Handle, 0xb, (IntPtr)0, IntPtr.Zero);
			
		}
		/// <summary>
		/// Flicker resistance
		/// http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/a6abf4e1-e502-4988-a239-a082afedf4a7
		/// </summary>
		public void EndUpdate()
		{
			SendMessage(this.Handle, 0xb, (IntPtr)1, IntPtr.Zero);
			this.Invalidate();
			
		}
		
		public bool bSuspendUpdateSelection = false;
		/// <summary>
		/// during certain bulk actions you don't want to be updated selections
		/// </summary>
		public void SuspendUpdateSelection()
		{
			bSuspendUpdateSelection = true;
		}
		
		public void ResumeUpdateSelection()
		{
			bSuspendUpdateSelection = false;
		}

	}
}

