using System;
using CoreUtilities;
using System.Windows.Forms;
namespace Layout
{
	public class MarkupLanguageNone : iMarkupLanguage
	{
		public MarkupLanguageNone ()
		{
		}

		public override string ToString ()
		{
			return nameAndIdentifier;
		}
		private string nameAndIdentifier= Loc.Instance.GetString("None");

		public string NameAndIdentifier {
			get {
				return nameAndIdentifier;
			}
			set {
				nameAndIdentifier = value;
			}
		}
		public void DoPaint(PaintEventArgs e, int Start, int End, RichTextBox RichText)
		{
			// do nothing
		}
	}
}

