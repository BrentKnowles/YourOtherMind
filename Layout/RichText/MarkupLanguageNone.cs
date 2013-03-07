using System;
using CoreUtilities;
using System.Windows.Forms;
using System.Collections;

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
		public bool IsIndex (string incoming)
		{
			return false;
		}
		public ArrayList GetListOfPages (string s, ref bool b)
		{
			b = false;
			return null;
		}
		public void DoPaint(PaintEventArgs e, int Start, int End, RichTextBox RichText)
		{
			// do nothing
		}
	}
}

