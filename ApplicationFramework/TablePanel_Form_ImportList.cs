// TablePanel_Form_ImportList.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System;
using System.Windows.Forms;
using CoreUtilities;

namespace appframe
{
	public class TablePanel_Form_ImportList : Form
	{

		public TablePanel_Form_ImportList()
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TablePanel_Form_ImportList));
				this.textList = new System.Windows.Forms.TextBox();
				this.textBox2 = new System.Windows.Forms.TextBox();
				this.panel1 = new System.Windows.Forms.Panel();
				this.button2 = new System.Windows.Forms.Button();
				this.button1 = new System.Windows.Forms.Button();
				this.panel1.SuspendLayout();
				this.SuspendLayout();
				// 
				// textList
				// 
				this.textList.Dock = System.Windows.Forms.DockStyle.Fill;
				this.textList.Location = new System.Drawing.Point(0, 0);
				this.textList.Multiline = true;
				this.textList.Name = "textList";
				this.textList.Size = new System.Drawing.Size(292, 159);
				this.textList.TabIndex = 0;
				// 
				// textBox2
				// 
				this.textBox2.BackColor = System.Drawing.Color.BurlyWood;
				this.textBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
				this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				this.textBox2.Location = new System.Drawing.Point(0, 159);
				this.textBox2.Multiline = true;
				this.textBox2.Name = "textBox2";
				this.textBox2.Size = new System.Drawing.Size(292, 172);
				this.textBox2.TabIndex = 1;

			string warning = "Import the list of entries above into the Result column of your table. There are two types of imports:"+
"1) Comma seperated values such as Row1Column1, Row1Column2  Row2Column1, Row2Column2 or"+
					"2) A single column of data but the Roll column of your table will be autonumbered to match the entries. This is an easy way to create a randomized list of items."+
					Environment.NewLine+"NOTE: To work properly make sure you have CORRECT number of COLUMNS. They must match else simpler simpler will be used instead";
				this.textBox2.Text = Loc.Instance.GetString(warning);
				// 
				// panel1
				// 
				this.panel1.Controls.Add(this.button2);
				this.panel1.Controls.Add(this.button1);
				this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
				this.panel1.Location = new System.Drawing.Point(0, 331);
				this.panel1.Name = "panel1";
				this.panel1.Size = new System.Drawing.Size(292, 28);
				this.panel1.TabIndex = 2;
				// 
				// button2
				// 
				this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
				this.button2.Location = new System.Drawing.Point(133, 2);
				this.button2.Name = "button2";
				this.button2.Size = new System.Drawing.Size(75, 23);
				this.button2.TabIndex = 1;
				this.button2.Text = "OK";
				this.button2.UseVisualStyleBackColor = true;
				// 
				// button1
				// 
				this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
				this.button1.Location = new System.Drawing.Point(214, 3);
				this.button1.Name = "button1";
				this.button1.Size = new System.Drawing.Size(75, 23);
				this.button1.TabIndex = 0;
				this.button1.Text = "Cancel";
				this.button1.UseVisualStyleBackColor = true;
				// 
				// fImportList
				// 
				this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				this.ClientSize = new System.Drawing.Size(292, 359);
				this.ControlBox = false;
				this.Controls.Add(this.textList);
				this.Controls.Add(this.textBox2);
				this.Controls.Add(this.panel1);
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = MainFormBase.MainFormIcon;
			//	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
				this.Name = "fImportList";
				this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
				this.Text = Loc.Instance.GetString("Import List");
				this.panel1.ResumeLayout(false);
				this.ResumeLayout(false);
				this.PerformLayout();
				
			}
			
#endregion
			
			private System.Windows.Forms.TextBox textBox2;
			private System.Windows.Forms.Panel panel1;
			private System.Windows.Forms.Button button2;
			private System.Windows.Forms.Button button1;
			public System.Windows.Forms.TextBox textList;
		}
}


