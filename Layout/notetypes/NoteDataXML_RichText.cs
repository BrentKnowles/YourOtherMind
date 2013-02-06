using System;
using System.Windows.Forms;
using CoreUtilities;
using System.Xml.Serialization;
namespace Layout
{
	public class NoteDataXML_RichText : NoteDataXML
	{
		#region formcontrols
		protected RichTextExtended richBox ;
		#endregion
		public override bool IsLinkable { get { return true; }}
		protected override void CommonConstructorBehavior ()
		{

			base.CommonConstructorBehavior ();
			Caption = Loc.Instance.Cat.GetString("Text Note");
		}
		public NoteDataXML_RichText () : base()
		{

		}
		public NoteDataXML_RichText(int height, int width) : base(height, width)
		{
			//Caption = Loc.Instance.Cat.GetString("Text Note");
		}

		public override void Save ()
		{
			base.Save ();
			// I made the decision to suppress Errors (hence not required CreateParent, for the purpose of MOVING notes
			// This violated an earlier decision I had made and I had to disable the TryToSaveWithoutCreating a Parent Exception
			if (richBox != null) {
				this.Data1 = richBox.Rtf;
			}
		}
		/// <summary>
		/// returns the text as text
		/// </summary>
		/// <returns>
		/// The as text.
		/// </returns>
		public string GetAsText()
		{
			return richBox.Text;
		}
		public string[] Lines()
		{
			return richBox.Lines;
		}
		/// <summary>
		/// Overwrites the with RTF file.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		public void DoOverwriteWithRTFFile (string file)
		{
			richBox.LoadFile(file);
		}
		/// <summary>
		/// Checks to see if the richtext is empty
		/// </summary>
		/// <returns>
		/// <c>true</c>, if text blank was riched, <c>false</c> otherwise.
		/// </returns>
		public bool GetIsRichTextBlank ()
		{
			if (richBox.Text == Constants.BLANK) {
				return true;
			}
			return false;
		}
		public override void CreateParent (LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;
			richBox = new RichTextExtended(new MarkupLanguageNone());
			richBox.ContextMenuStrip = Layout.GetLayoutTextEditContextStrip();
			richBox.Enter += HandleRichBoxEnter;
			richBox.Parent = ParentNotePanel;
			richBox.Dock = DockStyle.Fill;
			richBox.BringToFront();
			richBox.Rtf = this.Data1;
			richBox.TextChanged+= HandleTextChanged;
			richBox.ReadOnly = this.ReadOnly;
		}

		protected void SetThisTextNoteAsActive()
		{
			Layout.CurrentTextNote = (NoteDataXML_RichText)this;
		}
		/// <summary>
		/// Handles the rich box enter. Sets the active richtext box
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleRichBoxEnter (object sender, EventArgs e)
		{
			lg.Instance.Line("NoteDataXML_RichText", ProblemType.MESSAGE, String.Format ("{0} has been set as CurrentTextNote for Layout {1}", this.Caption, Layout.GUID));
			SetThisTextNoteAsActive();
		}

		void HandleTextChanged (object sender, EventArgs e)
		{
			this.SetSaveRequired(true);
		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Text");
		}
		protected override void RespondToReadOnlyChange()
		{
			this.richBox.ReadOnly = this.ReadOnly;
		}

		/// <summary>
		/// RichText Accessors
		/// </summary>
		/// <value>
		/// The selected text.
		/// </value>
		[XmlIgnore]
		public string SelectedText {
			get { return richBox.SelectedText;}
			set { richBox.SelectedText = value;}
		}
		[XmlIgnore]
		public int SelectionStart {
			get { return richBox.SelectionStart;}
			set { richBox.SelectionStart = value;}
		}
		[XmlIgnore]
		public int SelectionLength {
			get { return richBox.SelectionLength;}
			set { richBox.SelectionLength = value;}
		}
		public void Bold()
		{
			richBox.SelectionFont = new System.Drawing.Font(richBox.SelectionFont.FontFamily, richBox.SelectionFont.Size, System.Drawing.FontStyle.Bold);

		}
		public override string ToString ()
		{
			return this.Data1;
//			if (richBox != null) {
//				return richBox.Text;
//			} else {
//				return "no rich text box load";
//			}
		}
		public override string GetStoryboardPreview {
			get {
				return this.Data1;
			}

		}
	}
}

