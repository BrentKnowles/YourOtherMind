using System;
using System.Windows.Forms;
using System.Drawing;

namespace CoreUtilities
{
	public class form_NewMessage : Form
	{

		public form_NewMessage()
		{
			InitializeComponent();
			labelMessage.Dock = DockStyle.Top;
			labelMessage.BringToFront();
		}
		int dwidth = 400;
		int dheight = 200;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		
		#region Windows Form Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.bOk = new System.Windows.Forms.Button();
			this.labelMessage = new System.Windows.Forms.Label();
			this.footer = new System.Windows.Forms.Panel();
			this.bNo = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.header = new System.Windows.Forms.Panel();
			this.captionLabel = new System.Windows.Forms.Label();
			this.picImage = new System.Windows.Forms.PictureBox();
			this.userinput = new System.Windows.Forms.TextBox();
			this.panelWebHelp = new System.Windows.Forms.Panel();
			this.linkLabelWebHelp = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.panelHelpHelp = new System.Windows.Forms.Panel();
			this.linkLabelHelpFile = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.footer.SuspendLayout();
			this.header.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
			this.panelWebHelp.SuspendLayout();
			this.panelHelpHelp.SuspendLayout();
			this.SuspendLayout();
			// 
			// bOk
			// 
			this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOk.Location = new System.Drawing.Point(240, 6);
			this.bOk.Name = "bOk";
			this.bOk.Size = new System.Drawing.Size(75, 23);
			this.bOk.TabIndex = 0;
			this.bOk.Text = "OK";
			this.bOk.UseVisualStyleBackColor = true;
			this.bOk.Visible = false;
			this.bOk.Dock = DockStyle.Left;
			// 
			// labelMessage
			// 
			this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                                 | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMessage.BackColor = System.Drawing.Color.Transparent;
			this.labelMessage.Location = new System.Drawing.Point(66, 34);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(220, 50);
			this.labelMessage.TabIndex = 1;
			this.labelMessage.Text = "label1";
			this.labelMessage.MouseLeave += new System.EventHandler(this.labelMessage_MouseLeave);
			this.labelMessage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseMove);
			this.labelMessage.Click += new System.EventHandler(this.labelMessage_Click);
			this.labelMessage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseDown);
			this.labelMessage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseUp);
			// 
			// footer
			// 
			this.footer.BackColor = System.Drawing.Color.Transparent;
			this.footer.Controls.Add(this.bNo);
			this.footer.Controls.Add(this.bCancel);
			this.footer.Controls.Add(this.bOk);
			this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.footer.Location = new System.Drawing.Point(0, 86);
			this.footer.Name = "footer";
			this.footer.Size = new System.Drawing.Size(327, 32);
			this.footer.TabIndex = 2;
			this.footer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseUp);
			this.footer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseDown);
			this.footer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseUp);
			// 
			// bNo
			// 
			this.bNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.bNo.Location = new System.Drawing.Point(76, 6);
			this.bNo.Name = "bNo";
			this.bNo.Size = new System.Drawing.Size(75, 23);
			this.bNo.TabIndex = 2;
			this.bNo.Text = "No";
			this.bNo.UseVisualStyleBackColor = true;
			this.bNo.Visible = false;
			this.bNo.Dock = DockStyle.Right;
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(157, 6);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(75, 23);
			this.bCancel.TabIndex = 1;
			this.bCancel.Text = "Cancel";
			this.bCancel.UseVisualStyleBackColor = true;
			this.bCancel.Visible = false;
		
			// 
			// header
			// 
			this.header.BackColor = System.Drawing.Color.Transparent;
			this.header.Controls.Add(this.captionLabel);
			this.header.Dock = System.Windows.Forms.DockStyle.Top;
			this.header.Location = new System.Drawing.Point(0, 0);
			this.header.Name = "header";
			this.header.Size = new System.Drawing.Size(327, 31);
			this.header.TabIndex = 3;
			this.header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseMove);
			this.header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseDown);
			this.header.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelMessage_MouseUp);
			// 
			// captionLabel
			// 
			this.captionLabel.AutoSize = true;
			this.captionLabel.Location = new System.Drawing.Point(10, 5);
			this.captionLabel.Name = "captionLabel";
			this.captionLabel.Size = new System.Drawing.Size(35, 13);
			this.captionLabel.TabIndex = 0;
			this.captionLabel.Text = "label1";
			// 
			// picImage
			// 
			this.picImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                             | System.Windows.Forms.AnchorStyles.Left)));
			this.picImage.Location = new System.Drawing.Point(12, 34);
			this.picImage.Name = "picImage";
			this.picImage.Size = new System.Drawing.Size(48, 48);
			this.picImage.TabIndex = 4;
			this.picImage.TabStop = false;
			this.picImage.Visible = false;
			// 
			// userinput
			// 
			this.userinput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                               | System.Windows.Forms.AnchorStyles.Left)
			                                                              | System.Windows.Forms.AnchorStyles.Right)));
			this.userinput.Location = new System.Drawing.Point(69, 54);
			this.userinput.Name = "userinput";
			this.userinput.Size = new System.Drawing.Size(246, 20);
			this.userinput.TabIndex = 5;
			this.userinput.Visible = false;
			// 
			// panelWebHelp
			// 
			this.panelWebHelp.Controls.Add(this.linkLabelWebHelp);
			this.panelWebHelp.Controls.Add(this.label1);
			this.panelWebHelp.Location = new System.Drawing.Point(69, 49);
			this.panelWebHelp.Name = "panelWebHelp";
			this.panelWebHelp.Size = new System.Drawing.Size(246, 25);
			this.panelWebHelp.TabIndex = 6;
			this.panelWebHelp.Visible = false;
			// 
			// linkLabelWebHelp
			// 
			this.linkLabelWebHelp.AutoSize = true;
			this.linkLabelWebHelp.Location = new System.Drawing.Point(68, 5);
			this.linkLabelWebHelp.Name = "linkLabelWebHelp";
			this.linkLabelWebHelp.Size = new System.Drawing.Size(55, 13);
			this.linkLabelWebHelp.TabIndex = 1;
			this.linkLabelWebHelp.TabStop = true;
			this.linkLabelWebHelp.Text = "linkLabel1";
			this.linkLabelWebHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWebHelp_LinkClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Web Help:";
			// 
			// panelHelpHelp
			// 
			this.panelHelpHelp.Controls.Add(this.linkLabelHelpFile);
			this.panelHelpHelp.Controls.Add(this.label2);
			this.panelHelpHelp.Location = new System.Drawing.Point(69, 49);
			this.panelHelpHelp.Name = "panelHelpHelp";
			this.panelHelpHelp.Size = new System.Drawing.Size(283, 22);
			this.panelHelpHelp.TabIndex = 7;
			this.panelHelpHelp.Visible = false;
			// 
			// linkLabelHelpFile
			// 
			this.linkLabelHelpFile.AutoSize = true;
			this.linkLabelHelpFile.Location = new System.Drawing.Point(68, 5);
			this.linkLabelHelpFile.Name = "linkLabelHelpFile";
			this.linkLabelHelpFile.Size = new System.Drawing.Size(55, 13);
			this.linkLabelHelpFile.TabIndex = 1;
			this.linkLabelHelpFile.TabStop = true;
			this.linkLabelHelpFile.Text = "linkLabel1";
			this.linkLabelHelpFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHelpFile_LinkClicked);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Help File:";
			// 
			// NewMessageForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(dwidth, dheight);
			this.ControlBox = false;
			this.Controls.Add(this.panelHelpHelp);
			this.Controls.Add(this.panelWebHelp);
			this.Controls.Add(this.userinput);
			this.Controls.Add(this.picImage);
			this.Controls.Add(this.header);
			this.Controls.Add(this.footer);
			this.Controls.Add(this.labelMessage);
			this.Name = "NewMessageForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NewMessage";
			this.footer.ResumeLayout(false);
			this.header.ResumeLayout(false);
			this.header.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
			this.panelWebHelp.ResumeLayout(false);
			this.panelWebHelp.PerformLayout();
			this.panelHelpHelp.ResumeLayout(false);
			this.panelHelpHelp.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		

			this.AutoScroll = true;
		}
		
#endregion
		
		private System.Windows.Forms.Button bOk;
		private System.Windows.Forms.Label labelMessage;
		private System.Windows.Forms.Panel footer;
		private System.Windows.Forms.Panel header;
		private System.Windows.Forms.Label captionLabel;
		public System.Windows.Forms.PictureBox picImage;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Button bNo;
		public System.Windows.Forms.TextBox userinput;
		private System.Windows.Forms.Panel panelWebHelp;
		private System.Windows.Forms.LinkLabel linkLabelWebHelp;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panelHelpHelp;
		private System.Windows.Forms.LinkLabel linkLabelHelpFile;
		private System.Windows.Forms.Label label2;
		//////////////////////////////////////////////////////////////////////
		//
		// Methods
		//
		//////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="captionFont"></param>
		/// <param name="textFont"></param>
		public void SetupFonts(Font captionFont, Font textFont)
		{
			if (captionFont != null && textFont != null)
			{
				labelMessage.Font = new Font(textFont, FontStyle.Regular);
				captionLabel.Font = new Font(captionFont, FontStyle.Bold);
				label1.Font =new Font(textFont, FontStyle.Regular);
				label2.Font = new Font(textFont, FontStyle.Regular);
				linkLabelHelpFile.Font = new Font(textFont, FontStyle.Regular);
				linkLabelWebHelp.Font = new Font(textFont, FontStyle.Regular);
			}
			
		}
		/// <summary>
		/// sets the form to display help information
		/// </summary>
		public void SetupForHelpMode(string sWeblink, string sHelplink, string sHelpfile)
		{
			if (sWeblink != "")
			{
				panelWebHelp.Visible = true;
				
				panelWebHelp.Dock = DockStyle.Bottom;
				linkLabelWebHelp.Text = sWeblink;
			}
			/* Ju7ly 2010 - couldn't get this to work proerply so I removed
            if (sHelplink != "" && sHelplink != null && sHelplink != "")
            {
                panelHelpHelp.Visible = true;
                panelHelpHelp.Dock = DockStyle.Bottom;
                linkLabelHelpFile.Text = sHelplink;
                linkLabelHelpFile.Tag = sHelpfile;
                
            }*/
			labelMessage.Dock = DockStyle.Top;
			labelMessage.BringToFront();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="backColor">Note: If there is an image this will be useless</param>
		/// <param name="captionColor">Set to transparent if there is a background</param>
		/// <param name="textColor">Set to transprent if there is a background</param>
		public void SetupColors(Color backColor, Color captionColor, Color textColor,
		                        Color captionBackColor, Color textBackColor)
		{
			try
			{
				//labelMessage.BackColor = ;//Color.Transparent;
				panelWebHelp.BackColor = textBackColor;
				panelHelpHelp.BackColor = textBackColor;
				labelMessage.BackColor = textBackColor;
				captionLabel.BackColor = captionBackColor;
				
			}
			catch (Exception)
			{
				lg.Instance.Line("NewMessage.SetupColors",CoreUtilities.ProblemType.EXCEPTION,"unable to set label back to transparent");
				labelMessage.BackColor = this.TransparencyKey;
				captionLabel.BackColor = captionBackColor;
			}
			
			// if we choose not to be transparent we probably want the 
			// panel to be an actual heading panel like a normal
			// message box
			if (captionBackColor != Color.Transparent)
			{
				int nHeight = (int) (captionLabel.Font.Height * 1.5);
				captionLabel.AutoSize = false;
				captionLabel.Dock = DockStyle.Top;
				captionLabel.Height = nHeight;
				
				
			}
			
			this.BackColor = backColor;
			captionLabel.ForeColor = captionColor;
			labelMessage.ForeColor = textColor;
			
			
			
			
			
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sCaption"></param>
		/// <param name="sText"></param>
		public void SetupStrings(string sCaption, string sText)
		{
			captionLabel.Text = sCaption;
			labelMessage.Text = sText;
		}
		
		/// <summary>
		/// Called for each button to set the parameters correctly
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		private Button setupButton(Button button, Color buttonColor, DockStyle docking)
		{
			button.Visible = true;
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderSize = 0;
			
			button.BackColor = buttonColor;
			button.Dock = docking;
			
			//    button.Margin = new Padding(20);
			//   button.Padding = new Padding(10); Nope
			
			footer.Padding = new Padding(5);
			//     footer.Margin = new Padding(50);
			return button;
		}
		
		/// <summary>
		/// 
		/// </summary>
		private void setupYesNo(Color buttonColor)
		{
			bOk = setupButton(bOk, buttonColor, DockStyle.None);
			bNo = setupButton(bNo, buttonColor, DockStyle.Right);
			bNo.SendToBack(); // to make it align to the right
			
			bOk.Text = "Yes";
			//  bCancel.Text = "No";
			bOk.DialogResult = DialogResult.Yes;
			//  bCancel.DialogResult = DialogResult.No;
			
			
			// manually ensure that bOK is spaced far enough away from bCancel
			bOk.Dock=DockStyle.Left;
			//bOk.Left = bNo.Left - 5;// -(bOk.Width); OK, Feb 2013 - we commented this out, using the Docking + Margin to accomplish spacing
			bOk.Height = bNo.Height;
			bOk.Top = bNo.Top;
			
			bNo.TabIndex = 0;
			bOk.TabIndex = 1;
			this.AcceptButton = bNo;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buttonColor"></param>
		public void setupYesNoCancel(Color buttonColor)
		{
			bCancel = setupButton(bCancel, buttonColor, DockStyle.Right);
			bOk = setupButton(bOk, buttonColor, DockStyle.None);
			bNo = setupButton(bNo, buttonColor, DockStyle.None);
			
			bOk.Text = "Yes";
			bOk.Dock = DockStyle.Left;
			bOk.DialogResult = DialogResult.Yes;
			
			bCancel.SendToBack(); // to make it align to the right
			
			bNo.Dock = DockStyle.None;
			bNo.Left = bCancel.Left - 5;// -(bOk.Width);
			bNo.Height = bCancel.Height;
			bNo.Top = bCancel.Top;
			
			
			bOk.Left = bNo.Left - bNo.Width - 2;// -(bOk.Width);
			bOk.Height = bNo.Height;
			bOk.Top = bNo.Top;
			
			bCancel.TabIndex = 0;
			bNo.TabIndex = 1;
			bOk.TabIndex = 2;
			
			this.AcceptButton = bCancel;
			
		}
		
		
		/// <summary>
		/// Sets
		/// </summary>
		/// <param name="buttons"></param>
		/// <param name="bOkDefault">if true the OK button will be the default button</param>
		public void SetupForButtons(MessageBoxButtons buttons, Color buttonColor, bool bOkDefault)
		{
			switch (buttons)
			{
			case MessageBoxButtons.OK:
			{
				this.AcceptButton = bOk;
				bOk  = setupButton(bOk, buttonColor, DockStyle.Right);
			}
				break;
			case MessageBoxButtons.OKCancel:
			{
				bOk = setupButton(bOk, buttonColor, DockStyle.Left);
				bCancel = setupButton(bCancel, buttonColor, DockStyle.Right);
				bCancel.SendToBack(); // to make it align to the right
				
				// manually ensure that bOK is spaced far enough away from bCancel
				bOk.Left = bCancel.Left - 5;// -(bOk.Width);
				bOk.Height = bCancel.Height;
				bOk.Top = bCancel.Top;
				
				bCancel.TabIndex = 0;
				bOk.TabIndex = 1;
				this.AcceptButton = bCancel;
			}
				break;
			case MessageBoxButtons.YesNo:
			{
				setupYesNo(buttonColor);
				
				
				
				
			} break;
			case MessageBoxButtons.YesNoCancel:
			{
				setupYesNoCancel(buttonColor);   
				
			} break;
			case MessageBoxButtons.RetryCancel:
			{
				setupYesNo(buttonColor);
				bOk.Text = "Retry";
				bOk.DialogResult = DialogResult.Retry;
				bNo.Text = "Cancel";
				bNo.DialogResult = DialogResult.Cancel;
			} break;
			case MessageBoxButtons.AbortRetryIgnore:
			{
				setupYesNoCancel(buttonColor);
				bOk.Text = "Abort";
				bOk.DialogResult = DialogResult.Abort;
				
				bNo.Text = "Retry";
				bNo.DialogResult = DialogResult.Retry;
				
				bCancel.Text = "Ignore";
				bCancel.DialogResult = DialogResult.Cancel;
				
			} break;
				
			default: MessageBox.Show(buttons.ToString() + " is unhandled in NewMessage"); break;
			}
			
			if (bOkDefault == true)
			{
				this.AcceptButton = bOk;
			}
			
		}
		
		private void labelMessage_Click(object sender, EventArgs e)
		{
			
		}
		
		/// <summary>
		/// we hook up all components to the mouse move/mouse down so that moving works naturally
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void labelMessage_MouseDown(object sender, MouseEventArgs e)
		{
			base.OnMouseDown(e);
		}
		
		private void labelMessage_MouseLeave(object sender, EventArgs e)
		{
			
		}
		
		private void labelMessage_MouseMove(object sender, MouseEventArgs e)
		{
			base.OnMouseMove(e);
		}
		
		private void labelMessage_MouseUp(object sender, MouseEventArgs e)
		{
			base.OnMouseUp(e);
		}
		
		private void linkLabelWebHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			General.OpenDocument(linkLabelWebHelp.Text, "");
		}
		
		private void linkLabelHelpFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (linkLabelHelpFile.Tag != null)
			{
				string sHelpFile = linkLabelHelpFile.Tag.ToString();
				
				Help.ShowHelp(this, sHelpFile, HelpNavigator.Topic, linkLabelHelpFile.Text);
			}
			
		}
	}
}