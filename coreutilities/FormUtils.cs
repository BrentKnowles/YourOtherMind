using System;
using System.Windows.Forms;
using System.Drawing;


namespace CoreUtilities
{
	public static class FormUtils
	{
		public enum FontSize {Normal, Plus1, Plus2};
	

		public static void SizeFormsForAccessibility (Form form, FontSize fontSize)
		{
			float newSize = 1.0f;
			switch (fontSize) {
			case FontSize.Normal:
				newSize = 12.0f;
				break;
			case FontSize.Plus1:
				newSize = 14.0f;
				break;
			case FontSize.Plus2:
				newSize = 16.0f;
				break;
			}
			
			
			form.Font = new Font (form.Font.FontFamily, newSize);
			foreach (Control control in form.Controls) {
				if (control is ToolStrip)
				{
					((ToolStrip)control).Font= new Font(((ToolStrip)control).Font.FontFamily, newSize);
				}
			}
		}
	}
}

