// Stars.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace CoreUtilities
{
	public partial class Stars : ToolStripControlHost
	{
		public Stars():base(new FlowLayoutPanel())
		{
			InitializeComponent();
			controlPanel = (FlowLayoutPanel)base.Control;
			controlPanel.BackColor = Color.Transparent;

			Panel panel = new Panel();
			panel.Width=125;
			panel.Height=controlPanel.Height;
			controlPanel.Controls.Add (panel);

			panel.Controls.Add(this.star5);
			panel.Controls.Add(this.star4);
			panel.Controls.Add(this.star3);
			panel.Controls.Add(this.star2);
			panel.Controls.Add(this.star1);








		}
		
		private FlowLayoutPanel controlPanel;

		static Bitmap starimage = FileUtils.GetImage_ForDLL("star");
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sTag">send "1" to turn stars on</param>
		/// <param name="nPos"># of stars</param>
		public void SetStars(string sTag, int nPos)
		{
			//this might be tricky
			int nStars = 0;
			
			// click to enable
			if (sTag == "1")
			{
				//(sender as ToolStripLabel).Tag = "2";
				// turn off all of them
				star1.Image = star2.Image = star3.Image = star4.Image = star5.Image =FileUtils.GetImage_ForDLL("stargrey");
				star1.Tag = star2.Tag = star3.Tag = star4.Tag = star5.Tag = "1";
				//(sender as ToolStripLabel).Image = English.star;
				if (nPos >= 1)
				{
					for (int i = 1; i <= nPos; i++)
					{
						switch (i)
						{
						case 1: star1.Image = starimage; star1.Tag = "2"; nStars = 1; break;
						case 2: star2.Image = starimage;star2.Tag = "2"; nStars = 2; break;
						case 3: star3.Image =starimage; star3.Tag = "2"; nStars = 3; break;
						case 4: star4.Image = starimage; star4.Tag = "2"; nStars = 4; break;
						case 5: star5.Image = starimage; star5.Tag = "2"; nStars = 5; break;
							
						}
					} // for
					
				}
			}// enable ends
			else // disable
			{
				// turn off all of them
				star1.Image = star2.Image = star3.Image = star4.Image = star5.Image = FileUtils.GetImage_ForDLL("stargrey");
				//(sender as ToolStripLabel).Tag = "1";
				star1.Tag = star2.Tag = star3.Tag = star4.Tag = star5.Tag = "1";
				// turn onto MINUS one to me (i.e., if I turn 3 position off 1 and 2 should be on
				for (int i = 1; i < nPos; i++)
				{
					switch (i)
					{
					case 1: star1.Image = starimage; star1.Tag = "2"; nStars = 1; break;
					case 2: star2.Image = starimage;star2.Tag = "2"; nStars = 2; break;
					case 3: star3.Image = starimage; star3.Tag = "2"; nStars = 3; break;
					case 4: star4.Image = starimage; star4.Tag = "2"; nStars = 4; break;
					case 5: star5.Image =starimage; star5.Tag = "2"; nStars = 5; break;
						
					}
				} // for
			}
			starCount = (nStars);
		}
		private int starCount;
		
		public int GetStars()
		{
			return starCount;
		}
		public void UpdateStars()
		{
			star1.Image = star2.Image = star3.Image = star4.Image = star5.Image = FileUtils.GetImage_ForDLL("stargrey");
			star1.Tag = star2.Tag = star3.Tag = star4.Tag = star5.Tag = "1";
			
		}  
		private bool mReadOnly=false;
		public bool ReadOnly
		{
			get { return mReadOnly; }
			set { mReadOnly = value; }
		}
//Z		private void Stars_Load(object sender, EventArgs e)
//		{
//
//		}
		
		private void star1_Click(object sender, EventArgs e)
		{
			
			if (ReadOnly == false)
			{
				// had to use tag ast eh toggle (1 = grey, 2 = bright)
				// Position is the name's last character
				string sName = (sender as Label).Name;
				int nPos = Int32.Parse(sName.Substring(sName.Length - 1));
				SetStars((sender as Label).Tag.ToString(), nPos);
				OnRatingChange();
			}
			
		}
		
		
		// w need to setup callbacks to perform a save when rating changes
		public delegate string CustomEventHandler();
		public virtual event CustomEventHandler RatingChanged;
		
		public void OnRatingChange()
		{
			
			if (RatingChanged != null)
			{
				
				// M essageBox.Show("note link clicked");
				RatingChanged();
			}
		}
	}
	partial class Stars
	{
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
		
		#region Component Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			//System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stars));
			this.star2 = new System.Windows.Forms.Label();
			this.star3 = new System.Windows.Forms.Label();
			this.star4 = new System.Windows.Forms.Label();
			this.star5 = new System.Windows.Forms.Label();
			this.star1 = new System.Windows.Forms.Label();
		//	this.SuspendLayout();
			// 
			// star2
			// 
			this.star2.BackColor = System.Drawing.Color.Transparent;
			this.star2.Dock = System.Windows.Forms.DockStyle.Left;
			this.star2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.star2.Image =starimage;
			this.star2.Location = new System.Drawing.Point(21, 0);
			this.star2.Margin = new System.Windows.Forms.Padding(0);
			this.star2.Name = "star2";
			this.star2.Size = new System.Drawing.Size(21, 28);
			this.star2.TabIndex = 2;
			this.star2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.star2.Click += new System.EventHandler(this.star1_Click);
			// 
			// star3
			// 
			this.star3.BackColor = System.Drawing.Color.Transparent;
			this.star3.Dock = System.Windows.Forms.DockStyle.Left;
			this.star3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.star3.Image = starimage;
			this.star3.Location = new System.Drawing.Point(42, 0);
			this.star3.Margin = new System.Windows.Forms.Padding(0);
			this.star3.Name = "star3";
			this.star3.Size = new System.Drawing.Size(21, 28);
			this.star3.TabIndex = 3;
			this.star3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.star3.Click += new System.EventHandler(this.star1_Click);
			// 
			// star4
			// 
			this.star4.BackColor = System.Drawing.Color.Transparent;
			this.star4.Dock = System.Windows.Forms.DockStyle.Left;
			this.star4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.star4.Image = starimage;
			this.star4.Location = new System.Drawing.Point(63, 0);
			this.star4.Margin = new System.Windows.Forms.Padding(0);
			this.star4.Name = "star4";
			this.star4.Size = new System.Drawing.Size(21, 28);
			this.star4.TabIndex = 4;
			this.star4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.star4.Click += new System.EventHandler(this.star1_Click);
			// 
			// star5
			// 
			this.star5.BackColor = System.Drawing.Color.Transparent;
			this.star5.Dock = System.Windows.Forms.DockStyle.Left;
			this.star5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.star5.Image = starimage;
			this.star5.Location = new System.Drawing.Point(84, 0);
			this.star5.Margin = new System.Windows.Forms.Padding(0);
			this.star5.Name = "star5";
			this.star5.Size = new System.Drawing.Size(21, 28);
			this.star5.TabIndex = 5;
			this.star5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.star5.Click += new System.EventHandler(this.star1_Click);
			// 
			// star1
			// 
			this.star1.BackColor = System.Drawing.Color.Transparent;
			this.star1.Dock = System.Windows.Forms.DockStyle.Left;
			this.star1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.star1.Image = FileUtils.GetImage_ForDLL("bullet_black");
			this.star1.Location = new System.Drawing.Point(0, 0);
			this.star1.Margin = new System.Windows.Forms.Padding(0);
			this.star1.Name = "star1";
			this.star1.Size = new System.Drawing.Size(21, 28);
			this.star1.TabIndex = 1;
			this.star1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.star1.Click += new System.EventHandler(this.star1_Click);
			// 
			// Stars
			// 



			//this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			//this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			//this.BackColor = System.Drawing.SystemColors.Control;


			this.Name = "Stars";
			this.Size = new System.Drawing.Size(108, 28);
			//this.Load += new System.EventHandler(this.Stars_Load);
			//this.ResumeLayout(false);
			
		}
		
#endregion
		
		private System.Windows.Forms.Label star2;
		private System.Windows.Forms.Label star3;
		private System.Windows.Forms.Label star4;
		private System.Windows.Forms.Label star5;
		private System.Windows.Forms.Label star1;
		
	}
}
