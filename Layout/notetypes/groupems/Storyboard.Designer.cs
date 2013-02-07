namespace Storyboards
{
    partial class Storyboard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Storyboard));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addItemToListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCaptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
     //       this.printSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
     //       this.exportSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.stretchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addNewGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bPrevious = new System.Windows.Forms.ToolStripButton();
            this.bNext = new System.Windows.Forms.ToolStripButton();
            this.bDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bTogglePictureBox = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bToggleView = new System.Windows.Forms.ToolStripButton();
            this.viewLabel = new System.Windows.Forms.ToolStripLabel();
            this.bRefresh = new System.Windows.Forms.ToolStripButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxPreview = new System.Windows.Forms.RichTextBox();
            this.listView = new groupem_scrollmaster();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addItemToListToolStripMenuItem,
            this.editCaptionToolStripMenuItem,
         //   this.printSelectedToolStripMenuItem,
         //   this.exportSelectedToolStripMenuItem,
            this.deleteItemToolStripMenuItem,
            this.toolStripSeparator3,
            this.stretchToolStripMenuItem,
            this.toolStripSeparator2,
            this.addNewGroupToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(171, 170);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addItemToListToolStripMenuItem
            // 
            this.addItemToListToolStripMenuItem.Name = "addItemToListToolStripMenuItem";
            this.addItemToListToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.addItemToListToolStripMenuItem.Text = "Add Item To List";
            this.addItemToListToolStripMenuItem.Click += new System.EventHandler(this.addItemToListToolStripMenuItem_Click);
            // 
            // editCaptionToolStripMenuItem
            // 
            this.editCaptionToolStripMenuItem.Name = "editCaptionToolStripMenuItem";
            this.editCaptionToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.editCaptionToolStripMenuItem.Text = "Edit Caption";
            this.editCaptionToolStripMenuItem.ToolTipText = "Rename entries... this can be used to adjust their display order";
            this.editCaptionToolStripMenuItem.Click += new System.EventHandler(this.editCaptionToolStripMenuItem_Click);
            // 
            // printSelectedToolStripMenuItem
            // 
//            this.printSelectedToolStripMenuItem.Name = "printSelectedToolStripMenuItem";
//            this.printSelectedToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
//            this.printSelectedToolStripMenuItem.Text = "Print Selected";
//            this.printSelectedToolStripMenuItem.Click += new System.EventHandler(this.printSelectedToolStripMenuItem_Click);
//            // 
//            // exportSelectedToolStripMenuItem
//            // 
//            this.exportSelectedToolStripMenuItem.Name = "exportSelectedToolStripMenuItem";
//            this.exportSelectedToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
//            this.exportSelectedToolStripMenuItem.Text = "Export Selected";
//            this.exportSelectedToolStripMenuItem.Click += new System.EventHandler(this.exportSelectedToolStripMenuItem_Click);
            // 
            // deleteItemToolStripMenuItem
            // 
            this.deleteItemToolStripMenuItem.Name = "deleteItemToolStripMenuItem";
            this.deleteItemToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.deleteItemToolStripMenuItem.Text = "Delete Item";
            this.deleteItemToolStripMenuItem.Click += new System.EventHandler(this.deleteItemToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(167, 6);
            // 
            // stretchToolStripMenuItem
            // 
            this.stretchToolStripMenuItem.Name = "stretchToolStripMenuItem";
            this.stretchToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.stretchToolStripMenuItem.Text = "Toggle Image Size";
            this.stretchToolStripMenuItem.Click += new System.EventHandler(this.stretchToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
            // 
            // addNewGroupToolStripMenuItem
            // 
            this.addNewGroupToolStripMenuItem.Name = "addNewGroupToolStripMenuItem";
            this.addNewGroupToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.addNewGroupToolStripMenuItem.Text = "Add New Group...";
            this.addNewGroupToolStripMenuItem.Click += new System.EventHandler(this.addNewGroupToolStripMenuItem_Click);
            // 
            // imageListLarge FEB 2013 - As part of convesrion process I do not actually Think we need these default imagelists setup.
            // 

			this.imageListLarge.Images.Add (CoreUtilities.FileUtils.GetImage_ForDLL ("photo.png"));
			this.imageListLarge.Images.SetKeyName(0, "photo.png");
//            this.imageListLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLarge.ImageStream")));
//            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
//            this.imageListLarge.Images.SetKeyName(0, "text_align_center.png");
//            // 
//            // imageList
//            // 
			this.imageList.Images.Add(CoreUtilities.FileUtils.GetImage_ForDLL ("photo.png"));
			this.imageList.Images.SetKeyName(0,"photo.png");
//            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
//            this.imageList.Images.SetKeyName(0, "text_align_center.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bPrevious,
            this.bNext,
            this.bDeleteItem,
            this.bTogglePictureBox,
            this.toolStripSeparator1,
            this.bToggleView,
            this.viewLabel,
            this.bRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(346, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // bPrevious
            // 
            this.bPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bPrevious.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("arrow_left.png");
            this.bPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bPrevious.Name = "bPrevious";
            this.bPrevious.Size = new System.Drawing.Size(23, 22);
            this.bPrevious.ToolTipText = "Previous Image";
            this.bPrevious.Click += new System.EventHandler(this.bPrevious_Click);
            // 
            // bNext
            // 
            this.bNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bNext.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("arrow_right.png");
            this.bNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bNext.Name = "bNext";
            this.bNext.Size = new System.Drawing.Size(23, 22);
            this.bNext.Text = "toolStripButton3";
            this.bNext.ToolTipText = "Next Image";
            this.bNext.Click += new System.EventHandler(this.bNext_Click);
            // 
            // bDeleteItem
            // 
            this.bDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bDeleteItem.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("delete.png");
            this.bDeleteItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bDeleteItem.Name = "bDeleteItem";
            this.bDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bDeleteItem.Text = "toolStripButton1";
            this.bDeleteItem.ToolTipText = "Delete Current Item... this will only remove the reference, not the actual file o" +
                "r linked item";
            this.bDeleteItem.Click += new System.EventHandler(this.bDeleteItem_Click);
            // 
            // bTogglePictureBox
            // 
            this.bTogglePictureBox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bTogglePictureBox.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("photo.png");
            this.bTogglePictureBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bTogglePictureBox.Name = "bTogglePictureBox";
            this.bTogglePictureBox.Size = new System.Drawing.Size(23, 22);
            this.bTogglePictureBox.Text = "toolStripButton2";
            this.bTogglePictureBox.ToolTipText = "Toggle Preview";
            this.bTogglePictureBox.Click += new System.EventHandler(this.bToggle_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bToggleView
            // 
            this.bToggleView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bToggleView.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("picture_go.png");
            this.bToggleView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bToggleView.Name = "bToggleView";
            this.bToggleView.Size = new System.Drawing.Size(23, 22);
            this.bToggleView.Text = "toolStripButton1";
            this.bToggleView.ToolTipText = "Toggle View";
            this.bToggleView.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // viewLabel
            // 
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(59, 22);
            this.viewLabel.Text = "SmallIcon";
            // 
            // bRefresh
            // 
            this.bRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bRefresh.Image =  CoreUtilities.FileUtils.GetImage_ForDLL ("arrow_refresh.png");
            this.bRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(23, 22);
            this.bRefresh.Text = "Refresh";
            this.bRefresh.ToolTipText = "If entries are missing press this to refresh the display";
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 46);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            this.pictureBox.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxPreview);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(346, 373);
            this.splitContainer1.SplitterDistance = 186;
            this.splitContainer1.TabIndex = 3;
            // 
            // textBoxPreview
            // 
            this.textBoxPreview.Location = new System.Drawing.Point(56, 50);
            this.textBoxPreview.Name = "textBoxPreview";
            this.textBoxPreview.ReadOnly = true;
            this.textBoxPreview.Size = new System.Drawing.Size(100, 96);
            this.textBoxPreview.TabIndex = 3;
            this.textBoxPreview.Text = "";
            // 
            // listView
            // 
            this.listView.AllowDrop = false;
            this.listView.ContextMenuStrip = this.contextMenuStrip1;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.LargeImageList = this.imageListLarge;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.ScrollPosition = 0;
            this.listView.Size = new System.Drawing.Size(346, 373);
            this.listView.SmallImageList = this.imageList;
            this.listView.TabIndex = 0;
            this.listView.TileSize = new System.Drawing.Size(48, 48);
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.SmallIcon;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
            this.listView.Leave += new System.EventHandler(this.listView_Leave);
            this.listView.Enter += new System.EventHandler(this.listView_Enter);
            this.listView.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_DragDrop);
            this.listView.MouseEnter += new System.EventHandler(this.listView_MouseEnter);
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
            this.listView.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_DragEnter);
            this.listView.onScroll += new System.Windows.Forms.ScrollEventHandler(this.listView_onScroll);
         //   this.listView.Click += new System.EventHandler(this.listView_Click);
            // 
            // groupEms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "groupEms";
            this.Size = new System.Drawing.Size(346, 398);
            this.Load += new System.EventHandler(this.groupEms_Load);
            this.Enter += new System.EventHandler(this.groupEms_Enter);
            this.MouseEnter += new System.EventHandler(this.groupEms_MouseEnter);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton bToggleView;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ToolStripLabel viewLabel;
        private System.Windows.Forms.ToolStripButton bPrevious;
        private System.Windows.Forms.ToolStripButton bNext;
        private System.Windows.Forms.ToolStripButton bTogglePictureBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addNewGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem editCaptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton bDeleteItem;
      //  private System.Windows.Forms.ToolStripMenuItem printSelectedToolStripMenuItem;
       // private System.Windows.Forms.ToolStripMenuItem exportSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addItemToListToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton bRefresh;
        private System.Windows.Forms.ToolStripMenuItem deleteItemToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox textBoxPreview;
        private System.Windows.Forms.ToolStripMenuItem stretchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public groupem_scrollmaster listView;
    }
}
