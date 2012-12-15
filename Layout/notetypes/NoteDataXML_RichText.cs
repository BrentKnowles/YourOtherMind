using System;
using System.Windows.Forms;
using CoreUtilities;

namespace Layout
{
	public class NoteDataXML_RichText : NoteDataXML
	{
		#region formcontrols
		RichTextBox richBox ;
		#endregion

		public NoteDataXML_RichText () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Text Note");
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
			richBox = new RichTextBox();
			richBox.Parent = Parent;
			richBox.Dock = DockStyle.Fill;
			richBox.BringToFront();
			richBox.Rtf = this.Data1;
			richBox.TextChanged+= HandleTextChanged;
		
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
	}
}

