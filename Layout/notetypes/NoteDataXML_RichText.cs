using System;
using System.Windows.Forms;
using CoreUtilities;
using System.Xml.Serialization;
using System.Drawing;
namespace Layout
{
	public class NoteDataXML_RichText : NoteDataXML
	{
		public enum FormatText {BOLD, STRIKETHRU, ZOOM, LINE, BULLET, BULLETNUMBER, DEFAULTFONT};
		// if this is the value set on the dropdown then we use the defined default in OPTIONS
		const string defaultmarkup = "Default"; 
		#region XML
		string markuplanguage="Default";

		public string Markuplanguage {
			get {
				return markuplanguage;
			}
			set {
				markuplanguage = value;
			}
		}

		#endregion

		#region formcontrols
		protected RichTextExtended richBox ;
		protected ToolStripComboBox MarkupCombo;
		#endregion

		// mutex to prevent markup list from calling update when loading
		private bool loadingcombo = false;

		public override bool IsLinkable { get { return true; }}
		protected override void CommonConstructorBehavior ()
		{

			base.CommonConstructorBehavior ();
			Caption = Loc.Instance.Cat.GetString("Text Note");
		}
		public NoteDataXML_RichText () : base()
		{

		}

		private iMarkupLanguage SelectedMarkup {
			get {
				iMarkupLanguage returnvalue = null;
				if (Markuplanguage != defaultmarkup) {
					// we need to create this once but LayoutDetails will store it inot until it changes again so 
					// this won't be a slow operatin, just done on load and if changed
					
					
					returnvalue = LayoutDetails.Instance.GetMarkupMatch (Markuplanguage);
					
					
				} else {
					// we revert to none if plugin has been removed and we store this value too
					returnvalue = new MarkupLanguageNone ();
				

				}
				return returnvalue;
			}
		}

		public NoteDataXML_RichText(int height, int width) : base(height, width)
		{
			//Caption = Loc.Instance.Cat.GetString("Text Note");
		}
		private void UpdateMarkupSelection ()
		{

			if (MarkupCombo.SelectedItem != null ){
				if (MarkupCombo.Text == defaultmarkup) 
					Markuplanguage = defaultmarkup;
				else
				{

					Markuplanguage = MarkupCombo.SelectedItem.GetType ().AssemblyQualifiedName.ToString ();
					richBox.MarkupOverride = (iMarkupLanguage)MarkupCombo.SelectedItem;
				}
			}
		}

		public override void Save ()
		{
			base.Save ();
			// I made the decision to suppress Errors (hence not required CreateParent, for the purpose of MOVING notes
			// This violated an earlier decision I had made and I had to disable the TryToSaveWithoutCreating a Parent Exception
			if (richBox != null) {
				this.Data1 = richBox.Rtf;
			}

			//MarkupTag is used to determine whether user has changed the data or not to avoid unnecessary slowness in save
			if (((bool)(MarkupCombo.Tag)) == true)
			{
			UpdateMarkupSelection();
			}

		}


		public RichTextExtended GetRichTextBox ()
		{
			return richBox;
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

		protected override void DoChildAppearance (AppearanceClass app)
		{
			base.DoChildAppearance(app);


			richBox.BackColor = app.mainBackground;
			richBox.ForeColor = app.captionForeground;
		}
		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);
			CaptionLabel.Dock = DockStyle.Top;
			richBox = new RichTextExtended ();
			richBox.ContextMenuStrip = Layout.GetLayoutTextEditContextStrip ();
			richBox.Enter += HandleRichBoxEnter;
			richBox.Parent = ParentNotePanel;
			richBox.Dock = DockStyle.Fill;
			richBox.BringToFront ();
			richBox.Rtf = this.Data1;
			richBox.SelectionChanged+= HandleRichTextSelectionChanged;
			richBox.TextChanged += HandleTextChanged;
			richBox.ReadOnly = this.ReadOnly;
			richBox.HideSelection = false; // must be able to see focus form other controls

			MarkupCombo = new ToolStripComboBox ();
			MarkupCombo.ToolTipText = Loc.Instance.GetString ("AddIns allow text notes to format text. A global option controls the default markup to use on notes but this may be overridden here.");
		//	LayoutDetails.Instance.BuildMarkupComboBox (MarkupCombo);

			BuildDropDownList();

			properties.DropDownItems.Add (MarkupCombo);
			MarkupCombo.SelectedIndexChanged += HandleSelectedIndexChanged;
			MarkupCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			MarkupCombo.DropDown += HandleDropDown;
			loadingcombo = true;
			// just show default if we have not overridden this
			if (Markuplanguage == defaultmarkup) {
				MarkupCombo.Text = defaultmarkup;
			}
			else
			if (SelectedMarkup != null) {
				for (int i = 0; i < MarkupCombo.Items.Count; i++) {
					if (MarkupCombo.Items [i].GetType () == SelectedMarkup.GetType ()) {
						MarkupCombo.SelectedIndex = i;
						break;
					}
				}
				richBox.MarkupOverride = (iMarkupLanguage)MarkupCombo.SelectedItem;
			}

			// we use markup tag to indicate whether the data on the markup combo has changed to avoid slowness in save
			MarkupCombo.Tag = false;
			loadingcombo = false;
		}

		void HandleRichTextSelectionChanged (object sender, EventArgs e)
		{
			if (Layout.GetFindbar () != null) {
				Layout.GetFindbar ().UpdateSelection (richBox.SelectedText, false);
			}
		}

		void HandleSelectedIndexChanged (object sender, EventArgs e)
		{
			if (false == loadingcombo) {
				// if we ever have the default then we nerf the override
				richBox.MarkupOverride = null;

				// made a change. Need to save.
				MarkupCombo.Tag = true;
				UpdateMarkupSelection ();
				richBox.Invalidate();


			}
		}

		void BuildDropDownList()
		{
			// always rebuild lsit if expanded in case AddIn added new
			MarkupCombo.Items.Clear ();
			LayoutDetails.Instance.BuildMarkupComboBox (MarkupCombo);
			// we add default as an option
			MarkupCombo.Items.Add(defaultmarkup);
		}
		void HandleDropDown (object sender, EventArgs e)
		{
			BuildDropDownList();

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
			BringToFrontAndShow();
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
		//	richBox.SelectionFont = new System.Drawing.Font(richBox.SelectionFont.FontFamily, richBox.SelectionFont.Size, System.Drawing.FontStyle.Bold);
			General.FormatRichText(richBox, FontStyle.Bold);
		}

		public void Strike()
		{
			General.FormatRichText(richBox, FontStyle.Strikeout);
		}
		/// <summary>
		/// Will adjust the zoom of a richtext
		/// </summary>
		/// <param name="nCurrent"></param>
		public void ZoomToggle()
		{
			float nCurrent = richBox.ZoomFactor;
			
			if (nCurrent == 1.0f)
			{
				richBox.ZoomFactor = 1.25f;
			}
			if (nCurrent == 1.25f)
			{
				richBox.ZoomFactor = 1.50f;
			}
			if (nCurrent == 1.50f)
			{
				richBox.ZoomFactor = 1.75f;
			}
			if (nCurrent == 1.75f)
			{
				richBox.ZoomFactor = 2.0f;
			}
			if (nCurrent == 2.0f)
			{
				richBox.ZoomFactor = 1.0f;
			}
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
		// For link notes
		public override string GetLinkData ()
		{
			return this.Data1;
		}

		public void SaveAsExternalFile (string sFile)
		{
			richBox.SaveFile (sFile, RichTextBoxStreamType.RichText);
		}
	}
}

