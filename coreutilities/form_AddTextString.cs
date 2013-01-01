using System;
using System.Windows.Forms;

namespace CoreUtilities
{
	public class form_AddTextString : Form
	{


		public form_AddTextString(System.Drawing.Icon formIcon)
		{
			this.Icon = formIcon;
			InitializeComponent();
		}

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
			this.bOK = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Location = new System.Drawing.Point(160, 38);
			this.bOK.Name = "bOK";
			this.bOK.Size = new System.Drawing.Size(75, 23);
			this.bOK.TabIndex = 2;
			this.bOK.Text = "OK";
			this.bOK.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(241, 38);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// textBox
			// 
			this.textBox.AcceptsReturn = true;
			this.textBox.Location = new System.Drawing.Point(6, 12);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(310, 20);
			this.textBox.TabIndex = 0;
			// 
			// fAddTextString
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(318, 67);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.bOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "fAddTextString";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Text";
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		
#endregion
		
		private System.Windows.Forms.Button bOK;
		private System.Windows.Forms.Button button2;
		public System.Windows.Forms.TextBox textBox;
	}
}

