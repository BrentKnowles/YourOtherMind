using System;
using System.Windows.Forms;

namespace Layout
{
	public class RichTextExtended : RichTextBox
	{
		private iMarkupLanguage markuplanguage;

		/// <summary>
		/// Gets or sets the markuplanguage.
		/// Dicates how the screen draws in Paint
		/// </summary>
		/// <value>
		/// The markuplanguage.
		/// </value>
		public iMarkupLanguage Markuplanguage {
			get {
				return markuplanguage;
			}
			set {
				markuplanguage = value;
			}
		}

		public RichTextExtended (iMarkupLanguage _Markup)
		{
			if (_Markup == null) {
				throw new Exception("A markup language is required");
			}
			Markuplanguage = _Markup;
		}

	}
}

