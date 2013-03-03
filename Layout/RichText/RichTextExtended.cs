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
		public enum LineSpaceTypes { Single, OneAndHalf, Double };
		#region PARAFORMAT MASK VALUES
		// PARAFORMAT mask values
		private const uint PFM_STARTINDENT = 0x00000001;
		private const uint PFM_RIGHTINDENT = 0x00000002;
		private const uint PFM_OFFSET = 0x00000004;
		private const uint PFM_ALIGNMENT = 0x00000008;
		private const uint PFM_TABSTOPS = 0x00000010;
		private const uint PFM_NUMBERING = 0x00000020;
		private const uint PFM_OFFSETINDENT = 0x80000000;
		
		// PARAFORMAT 2.0 masks and effects
		private const uint PFM_SPACEBEFORE = 0x00000040;
		private const uint PFM_SPACEAFTER = 0x00000080;
		private const uint PFM_LINESPACING = 0x00000100;
		private const uint PFM_STYLE = 0x00000400;
		private const uint PFM_BORDER = 0x00000800; // (*)
		private const uint PFM_SHADING = 0x00001000; // (*)
		private const uint PFM_NUMBERINGSTYLE = 0x00002000; // RE 3.0
		private const uint PFM_NUMBERINGTAB = 0x00004000; // RE 3.0
		private const uint PFM_NUMBERINGSTART = 0x00008000; // RE 3.0
		
		private const uint PFM_RTLPARA = 0x00010000;
		private const uint PFM_KEEP = 0x00020000; // (*)
		private const uint PFM_KEEPNEXT = 0x00040000; // (*)
		private const uint PFM_PAGEBREAKBEFORE = 0x00080000; // (*)
		private const uint PFM_NOLINENUMBER = 0x00100000; // (*)
		private const uint PFM_NOWIDOWCONTROL = 0x00200000; // (*)
		private const uint PFM_DONOTHYPHEN = 0x00400000; // (*)
		private const uint PFM_SIDEBYSIDE = 0x00800000; // (*)
		private const uint PFM_TABLE = 0x40000000; // RE 3.0
		private const uint PFM_TEXTWRAPPINGBREAK = 0x20000000; // RE 3.0
		private const uint PFM_TABLEROWDELIMITER = 0x10000000; // RE 4.0
		
		// The following three properties are read only
		private const uint PFM_COLLAPSED = 0x01000000; // RE 3.0
		private const uint PFM_OUTLINELEVEL = 0x02000000; // RE 3.0
		private const uint PFM_BOX = 0x04000000; // RE 3.0
		private const uint PFM_RESERVED2 = 0x08000000; // RE 4.0
#endregion
		#region variables
		
		private const int WM_USER = 0x0400;
		private const int EM_GETCHARFORMAT = WM_USER + 58;
		private const int EM_SETCHARFORMAT = WM_USER + 68;
		public const int EM_GETPARAFORMAT = WM_USER + 61;
		public const int EM_SETPARAFORMAT = WM_USER + 71;// 0x447;

		[StructLayout(LayoutKind.Sequential)]
		public class PARAFORMAT2
		{
			public int cbSize;
			public int dwMask;
			public short wNumbering;
			public short wReserved;
			public int dxStartIndent;
			public int dxRightIndent;
			public int dxOffset;
			public short wAlignment;
			public short cTabCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
			public int[] rgxTabs;
			
			public int dySpaceBefore; // Vertical spacing before para
			public int dySpaceAfter; // Vertical spacing after para
			public int dyLineSpacing; // Line spacing depending on Rule
			public short sStyle; // Style handle
			public byte bLineSpacingRule; // Rule for line spacing (see tom.doc)
			public byte bOutlineLevel; // Outline Level
			public short wShadingWeight; // Shading in hundredths of a per cent
			public short wShadingStyle; // Byte 0: style, nib 2: cfpat, 3: cbpat
			public short wNumberingStart; // Starting value for numbering
			public short wNumberingStyle; // Alignment, Roman/Arabic, (), ), ., etc.
			public short wNumberingTab; // Space bet 1st indent and 1st-line text
			public short wBorderSpace; // Border-text spaces (nbl/bdr in pts)
			public short wBorderWidth; // Pen widths (nbl/bdr in half twips)
			public short wBorders; // Border styles (nibble/border)
			
			public PARAFORMAT2()
			{
				this.cbSize = Marshal.SizeOf(typeof(PARAFORMAT2));
			}
		}
		#endregion
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
			this.AcceptsTab = true;
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


			//TODO: Move this into a custom richedit used in proofing system instead??

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
									//g.DrawString("FINISH PORTING THIS", drawFont, drawBrush, startofcurrentword);
									g.DrawString(LayoutDetails.Instance.WordSystemInUse.GetPartOfSpeech(currentword.Trim()), drawFont, drawBrush, startofcurrentword);
									
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
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		public static extern IntPtr SendMessage(System.Runtime.InteropServices.HandleRef hWnd, int msg, int wParam, [In, Out, MarshalAs(UnmanagedType.LPStruct)] PARAFORMAT2 lParam);
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
		/// <summary>
		/// default linespace is to select all
		/// </summary>
		/// <param name="types"></param>
		public void LineSpace(LineSpaceTypes types)
		{
			LineSpace(types, true);
		}
		
		//http://msdn.microsoft.com/en-us/library/bb787942(VS.85).aspx
		/// <summary>
		/// just testing, this is quite complicated
		/// http://msdn2.microsoft.com/en-us/library/aa140277(office.10).aspx
		/// </summary>
		public void LineSpace(LineSpaceTypes types, bool bSelectAll)
		{
			PARAFORMAT2 paraformat1 = new PARAFORMAT2();
			paraformat1.dwMask = (int)PFM_LINESPACING;
			paraformat1.cbSize = (int)Marshal.SizeOf(paraformat1);//(UInt32)Marshal.SizeOf(paraformat1);
			paraformat1.bLineSpacingRule = (byte)(((int)types));
			//paraformat1.wReserved = 0;
			
			switch (types)
			{
			case LineSpaceTypes.Single: paraformat1.dyLineSpacing = 20; break;
			case LineSpaceTypes.OneAndHalf: paraformat1.dyLineSpacing = 30; break;
			case LineSpaceTypes.Double: paraformat1.dyLineSpacing = 40; break;
			}
			
			
			//  paraformat1.dyLineSpacing = ((int)types)40; // the above commented lie. This does need to be set
			if (bSelectAll == true) this.SelectAll();
			
			SendMessage(new System.Runtime.InteropServices.HandleRef(this, this.Handle), 0x447, 0, paraformat1);
			this.SelectionLength = 0;
			
		}
	}
}

