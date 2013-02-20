namespace DiffEngine
{
    partial class DiffControl
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
            this.lvDestination = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.lvSource = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxSource = new System.Windows.Forms.RichTextBox();
            this.textBoxDest = new System.Windows.Forms.RichTextBox();
            this.panelExtra = new System.Windows.Forms.Panel();
            this.labelWordsNew = new System.Windows.Forms.Label();
            this.labelWordsOld = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelExtra.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvDestination
            // 
            this.lvDestination.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lvDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDestination.FullRowSelect = true;
            this.lvDestination.HideSelection = false;
            this.lvDestination.Location = new System.Drawing.Point(580, 108);
            this.lvDestination.MultiSelect = false;
            this.lvDestination.Name = "lvDestination";
            this.lvDestination.Size = new System.Drawing.Size(220, 218);
            this.lvDestination.TabIndex = 4;
            this.lvDestination.UseCompatibleStateImageBehavior = false;
            this.lvDestination.View = System.Windows.Forms.View.Details;
            this.lvDestination.Resize += new System.EventHandler(this.lvDestination_Resize);
            this.lvDestination.SelectedIndexChanged += new System.EventHandler(this.lvDestination_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Line";
            this.columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Text (Current)";
            this.columnHeader4.Width = 198;
            // 
            // lvSource
            // 
            this.lvSource.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvSource.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvSource.FullRowSelect = true;
            this.lvSource.HideSelection = false;
            this.lvSource.Location = new System.Drawing.Point(0, 108);
            this.lvSource.MultiSelect = false;
            this.lvSource.Name = "lvSource";
            this.lvSource.Size = new System.Drawing.Size(570, 218);
            this.lvSource.TabIndex = 3;
            this.lvSource.UseCompatibleStateImageBehavior = false;
            this.lvSource.View = System.Windows.Forms.View.Details;
            this.lvSource.Resize += new System.EventHandler(this.lvSource_Resize);
            this.lvSource.SelectedIndexChanged += new System.EventHandler(this.lvSource_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Line";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Text (Archive)";
            this.columnHeader2.Width = 147;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(570, 108);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 218);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(0, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(69, 16);
            this.statusLabel.TabIndex = 6;
            this.statusLabel.Text = "Changes";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxSource);
            this.panel1.Controls.Add(this.textBoxDest);
            this.panel1.Controls.Add(this.panelExtra);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 92);
            this.panel1.TabIndex = 7;
            // 
            // textBoxSource
            // 
            this.textBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSource.Location = new System.Drawing.Point(574, 20);
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.Size = new System.Drawing.Size(226, 72);
            this.textBoxSource.TabIndex = 1;
            this.textBoxSource.Text = "";
            // 
            // textBoxDest
            // 
            this.textBoxDest.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxDest.Location = new System.Drawing.Point(0, 20);
            this.textBoxDest.Name = "textBoxDest";
            this.textBoxDest.Size = new System.Drawing.Size(574, 72);
            this.textBoxDest.TabIndex = 0;
            this.textBoxDest.Text = "";
            // 
            // panelExtra
            // 
            this.panelExtra.Controls.Add(this.labelWordsNew);
            this.panelExtra.Controls.Add(this.labelWordsOld);
            this.panelExtra.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExtra.Location = new System.Drawing.Point(0, 0);
            this.panelExtra.Name = "panelExtra";
            this.panelExtra.Size = new System.Drawing.Size(800, 20);
            this.panelExtra.TabIndex = 2;
            // 
            // labelWordsNew
            // 
            this.labelWordsNew.AutoSize = true;
            this.labelWordsNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelWordsNew.Location = new System.Drawing.Point(787, 0);
            this.labelWordsNew.Name = "labelWordsNew";
            this.labelWordsNew.Size = new System.Drawing.Size(13, 13);
            this.labelWordsNew.TabIndex = 1;
            this.labelWordsNew.Text = "0";
            // 
            // labelWordsOld
            // 
            this.labelWordsOld.AutoSize = true;
            this.labelWordsOld.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelWordsOld.Location = new System.Drawing.Point(0, 0);
            this.labelWordsOld.Name = "labelWordsOld";
            this.labelWordsOld.Size = new System.Drawing.Size(13, 13);
            this.labelWordsOld.TabIndex = 0;
            this.labelWordsOld.Text = "0";
            // 
            // DiffControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvDestination);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.lvSource);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusLabel);
            this.Name = "DiffControl";
            this.Size = new System.Drawing.Size(800, 326);
            this.Load += new System.EventHandler(this.DiffControl_Load);
            this.Resize += new System.EventHandler(this.DiffControl_Resize);
            this.panel1.ResumeLayout(false);
            this.panelExtra.ResumeLayout(false);
            this.panelExtra.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvDestination;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ListView lvSource;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox textBoxSource;
        private System.Windows.Forms.RichTextBox textBoxDest;
        private System.Windows.Forms.Panel panelExtra;
        private System.Windows.Forms.Label labelWordsNew;
        private System.Windows.Forms.Label labelWordsOld;
    }
}
