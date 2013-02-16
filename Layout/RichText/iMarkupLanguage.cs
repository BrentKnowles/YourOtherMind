using System;
using System.Windows.Forms;

namespace Layout
{
	public interface iMarkupLanguage
	{
		string NameAndIdentifier { get; set; }
	

		void DoPaint(PaintEventArgs e, int Start, int End, RichTextBox RichText);
	}
}

