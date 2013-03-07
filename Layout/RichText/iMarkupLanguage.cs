using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;

namespace Layout
{
	public interface iMarkupLanguage
	{
		string NameAndIdentifier { get; set; }
	
		bool IsIndex(string incoming);

		ArrayList GetListOfPages(string sLine, ref bool bGetWords);

		void DoPaint(PaintEventArgs e, int Start, int End, RichTextBox RichText);
	}
}

