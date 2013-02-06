using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;


namespace CoreUtilities
{
	/// <summary>
	/// Replacement class for MessageBox that will allow custom text
	/// </summary>
	public  static class NewMessage
	{
		private static Image image = null;
		private static Icon icon = null;
		
		private static bool bSetupRan = false;
		static ImageLayout newLayout;
		static Color transKey;
		static Font captionFont; static Font textFont; static Color buttonColor;
		static Color captionColor; static Color textColor; static Color formBackColor;
		static Color captionBackColor;
		static Color textBackColor;

		
		/// <summary>
		/// because this is static we can set some varibles that
		/// become true for entire application
		/// 
		/// This should be called before first use but not essential
		/// </summary>
		/// <param name="image"></param>
		/// <param name="newIcon">STRING to a file, leave NULL if none</param>
		/// <param name="_buttonColor"></param>
		public static void SetupBoxFirstTime(Image newImage, Icon newIcon,
		                                     ImageLayout _newLayout, Color _transKey,
		                                     Font _captionFont, Font _textFont, Color _buttonColor,
		                                     Color _captionColor, Color _textColor, Color _formBackColor,
		                                     Color _captionBackColor,
		                                     Color _textBackColor)
		{
			image = newImage;
			newLayout = _newLayout;
			transKey = _transKey;
			captionFont = _captionFont;
			
			textFont = _textFont;
			buttonColor = _buttonColor;
			captionColor = _captionColor;
			
			textColor = _textColor;
			formBackColor = _formBackColor;
			captionBackColor = _captionBackColor;
			textBackColor = _textBackColor;
			
			icon = newIcon;
			
			bSetupRan = true;
		}
		
		/// <summary>
		/// A wrapper for a plain Just text messagebox
		/// </summary>
		/// <param name="sCaption"></param>
		/// <returns></returns>
		public static DialogResult Show(string sText)
		{
			return Show("", sText, MessageBoxButtons.OK, null);
		}
		
		/// <summary>
		/// A wrapper for a messagebox with caption and text
		/// </summary>
		/// <param name="sCaption"></param>
		/// <param name="sText"></param>
		/// <returns></returns>
		public static DialogResult Show(string sCaption, string sText)
		{
			return Show(sCaption, sText, MessageBoxButtons.OK, null);
		}
		
		
		/// <summary>
		/// For displays a messagebox with an edit box
		/// 
		/// </summary>
		/// <param name="sCaption"></param>
		/// <param name="sText"></param>
		/// <param name="_picImage"></param>
		/// <returns>"" if no text or cancel</returns>
		public static string Show(string sCaption, string sText, Image _picImage, bool bOkEnabled, string sDefault)
		{
			string sInput="";
			
			if (Show(sCaption, sText, MessageBoxButtons.OKCancel, _picImage, true, out sInput, bOkEnabled, sDefault) ==
			    DialogResult.OK)
			{
				
				return sInput;
			}
			return "";
			
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sCaption"></param>
		/// <param name="sText"></param>
		/// <param name="buttons"></param>
		/// <param name="_picImage"></param>
		/// <param name="bEdit"></param>
		/// <param name="InputText"></param>
		/// <param name="bOkDefault"></param>
		/// <param name="sEditText"></param>
		/// <param name="sWebLink"></param>
		/// <param name="sHelpLink"></param>
		/// <param name="sHelpFile"></param>
		/// <param name="AlwaysOnTop"></param>
		/// <param name="editsize"></param>
		/// <returns></returns>
		public static string Show(string sCaption, string sText,
		                          MessageBoxButtons buttons, Image _picImage, bool bEdit, out string InputText, bool bOkDefault, string sEditText,
		                          string sWebLink, string sHelpLink, string sHelpFile, int editsize)
		{
			string sInput = "";
			InputText = "";
			if (Show(sCaption, sText, MessageBoxButtons.OKCancel, _picImage, true, out sInput, bOkDefault, sEditText, sWebLink, sHelpFile, sHelpFile, false, editsize) ==
			    DialogResult.OK)
			{
				
				return sInput;
			}
			return "";
		}
		/// <summary>
		/// wrapper for a standard message
		/// </summary>
		/// <param name="sCaption"></param>
		/// <param name="sText"></param>
		/// <param name="buttons"></param>
		/// <param name="_picImage"></param>
		/// <returns></returns>
		public static DialogResult Show(string sCaption, string sText,
		                                MessageBoxButtons buttons, Image _picImage)
		{
			string s = "";
			
			return Show(sCaption, sText, buttons, _picImage, false, out s, false, null);
		}
		public static DialogResult Show(string sCaption, string sText,
		                                MessageBoxButtons buttons, Image _picImage, bool bEdit, out string InputText, bool bOkDefault, string sEditText)
		{
			// string s = "";
			return Show(sCaption, sText, buttons, _picImage, bEdit, out InputText, bOkDefault, sEditText,"","","");
		}
		
		/// <summary>
		/// for help
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>
		public static DialogResult Show(string sCaption, string sText, string sWebLink, string sHelpLink, string sHelpFile)
		{
			string s = "";
			return Show(sCaption, sText, MessageBoxButtons.OK, null, false, out s, true, "", sWebLink, sHelpLink, sHelpFile);
		}
		/// <summary>
		///  Always On Top, exposed
		/// </summary>
		/// <param name="sCaption"></param>
		/// <param name="sText"></param>
		/// <returns></returns>
		public static DialogResult Show(string sCaption, string sText, bool bAlwaysOnTop)
		{
			string s = "";
			return Show(sCaption, sText, MessageBoxButtons.OK, null, false, out s, false, null,"", "", "", bAlwaysOnTop,-1);
		}
		
		
		public static DialogResult Show(string sCaption, string sText,
		                                MessageBoxButtons buttons, Image _picImage, bool bEdit, out string InputText, bool bOkDefault, string sEditText,
		                                string sWebLink, string sHelpLink, string sHelpFile)
		{
			return Show(sCaption, sText, buttons, _picImage, bEdit, out InputText, bOkDefault, sEditText, sWebLink, sHelpFile, sHelpFile, false, -1);
		}
		
		/// <summary>
		///  
		/// </summary>
		/// <param name="sCaption"></param>
		/// <param name="sText"></param>
		/// <param name="buttons"></param>
		/// <param name="_picImage">if not null this image will show up on the form</param>
		/// <param name="bEdit">if true there's an edit field</param>
		/// <param name="bOkDefault">if set to true the OK button will be default, generally not the case (Cancel is default)</param>
		/// <param name="InputText">the variable to returnt he modified text if usign this as a textbox entry</param>
		/// <param name="sEditText">default value for a textbox</param>
		/// <returns></returns>
		public static DialogResult Show(string sCaption, string sText,
		                                MessageBoxButtons buttons, Image _picImage, bool bEdit, out string InputText, bool bOkDefault, string sEditText, 
		                                string sWebLink, string sHelpLink, string sHelpFile, bool AlwaysOnTop, int editsize)
			
		{
			if (bSetupRan == false)
			{
				lg.Instance.Line("NewMessage.Show", ProblemType.ERROR, "MessageBox invoked without a call to SetupBoxFirstTime");
			}
			
			form_NewMessage form = new form_NewMessage();
			if (true == AlwaysOnTop)
			{
				form.TopMost = true;
			}


			if (sEditText != null)
			{
				form.userinput.Text = sEditText;
			}
			
			if (bEdit == true)
			{
				form.userinput.Visible = true;
				form.userinput.Dock = DockStyle.Top;
				form.userinput.TabIndex = 0;
				if (editsize > -1)
				{
					form.userinput.MaxLength = editsize;
				}
			}

			form.SetupForButtons(buttons, buttonColor, bOkDefault);
			form.SetupColors(formBackColor, captionColor, textColor, captionBackColor, textBackColor);
			form.SetupFonts(captionFont, textFont);
			form.SetupStrings(sCaption, sText);
		
			
			// adjust image
			if (image != null)
			{
				form.BackgroundImage = image;
				form.BackgroundImageLayout = newLayout;
				form.TransparencyKey = transKey;
			}
			if (icon != null /*&& File.Exists(icon)*/)
			{
				try
				{
					form.Icon = icon;//new Icon(icon, 24, 24);
				}
				catch (Exception)
				{
					lg.Instance.Line("NewMessage.Show", ProblemType.EXCEPTION,"not a valid icon for NewMessage" +icon);
				}
			}
			
			if (_picImage != null)
			{
				form.picImage.BackgroundImage = _picImage;
				form.picImage.Width = 48;
				form.picImage.Height = 48;
				form.picImage.BackgroundImageLayout = ImageLayout.Stretch;
				form.picImage.Visible = true;
			}
			
			if (sWebLink != "" || sHelpLink != "")
			{
				form.SetupForHelpMode(sWebLink, sHelpLink, sHelpFile);
			}
			DialogResult result =  form.ShowDialog();
			
			
			InputText = form.userinput.Text;
			
			
			form = null;
			lg.Instance.Line("NewMessage.Show", ProblemType.TEMPORARY, "NewMessage ShowMessage button response" + 
			                        result.ToString());
			
			return result;
			
			
		}
	}
}
