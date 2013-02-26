using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
//-Using
using System.Collections;
using System.IO;
using CoreUtilities;
using CoreUtilities.Links;


namespace Storyboards
{
    /// <summary>
    /// A way of grouping information and storing it, internally in an ArrayList
    /// but with methods to easily add to an XML serializable object.
    /// 
    /// This class uses the LinkSystem2 class, so all links are relative.
    /// 
    /// It handles the entire link management, the only thing the other application needs
    /// to do is to store the arraylist and restore it.
    /// 
    /// The other application does not need to know anything about a groupem, other than store the data.
    /// 
    /// VIEW
    /// - This is uses the listview and should expose the views to the user
    ///   so they can use the ones they want
    /// 
    /// STORAGE
    /// - All the data needs to be stored in the LinkSystem, nothing extra.
    /// - Exception: There should be on additional field "DefaultView" that can be stored
    ///  in case the user has modified their view
    /// 
    /// POSITION
    /// - this shoudl use an used field in LinkTable (i.e., be an explicity I am position 9)
    /// 
    /// GOALS
    /// - try to keep the data interface as simple as possible.
    /// - Can the arraylist (listview) behind the scenes simply be an arraylist of LinkRecords? A
    ///   generic, embedded icon is used for notes
    /// 
    /// LINKTABLERECORD
    ///  What is being used and how
    /// 
    ///   sFilename - URL to location
    ///   sText - text label
    ///   nBookmarkKey -  0 = Note, 1 =  Picture
    ///   sExtra - group information
    /// 
    /// NOTES
    /// - there will be methods for the group em to interact with the parent
    /// 
    ///   1. Double-clicking a note will send a message to the parent (in case they want to display the note)
    /// </summary>
    public partial class Storyboard : UserControl
    {
        public Storyboard()
        {
            InitializeComponent();
        }

#region privatevariables
       // private ArrayList items; choose not to use this, Just use the listview list
        private ListViewColumnSorter lvwColumnSorter;
        private string sSource; // GUID of current page
#endregion privatevariables

        #region publicvariables


        private int mFirstColumnWidth = 100;
        /// <summary>
        /// Defaults to 100; is the width of the first column
        /// </summary>
        public int FirstColumnWidth
        {
            get { return mFirstColumnWidth; }
            set { mFirstColumnWidth = value; }
        }


        private bool bShowToolbar = true;
        public bool ShowToolbar
        {
            get { return bShowToolbar; }
            set
            {
                toolStrip1.Visible = value;
                
                bShowToolbar = value;
            }
        }

        private int mNumberOfColumns = 3;

        /// <summary>
        /// Can only be one or two. If one, won't show second column
        /// </summary>
        public int NumberOfColumns
        {
            get { return mNumberOfColumns; }
            set { mNumberOfColumns = value; }
        }

        /// <summary>
        /// The listview groups
        /// </summary>
        public ListViewGroupCollection Groups
        {
            get { return listView.Groups; }
        }


        bool mSortGroups = true;
        /// <summary>
        /// set this to false if you don't care whether groups sort
        /// will make things run faster
        /// </summary>
        public bool SortGroup
        {
            get { return mSortGroups; }
            set { mSortGroups = value; }
                 
        }

        private int mDefaultView = 2; // defaults to small icon
        /// <summary>
        /// Must be set before SOURCE is set
        /// determines the default view to use
        /// </summary>
        public int DefaultView
        {
            get { return mDefaultView; }
            set { mDefaultView = value; }
        }

        /// <summary>
        /// View style for the groupbox used for storing settings
        /// </summary>
        public View ViewStyle
        {
            get { return listView.View; }
            set { listView.View = value; viewLabel.Text = value.ToString(); }
        }

        /// <summary>
        /// the GUID of the current page, needs to be set when the "note" is created
        /// 
        /// When source is set, a filter is tested and used to populate the items
        /// </summary>
        public string Source
        {
            get { return sSource; }
            set
            {
                sSource = value;

                // putting this here as a hook for later when I add DefaultView to cose
                // and to help make it refresh properly (I'm thinking the mode needs to be 
                // set before code loaded??
                switch (mDefaultView)
                {
                    case 0: listView.ShowGroups = true; listView.View = View.Details; break;
                    case 1: listView.ShowGroups = true; listView.View = View.LargeIcon; break;
                    case 2: listView.View = View.SmallIcon; listView.ShowGroups = true; break;
                }

                //January 2012 - removing this because we cannot load the group ems until the page is loaded now
                // with the new DataTable system
                //LoadGroupEm();
              //  listView.Refresh(); Nope, did not help
               

                /*
            LinkTableRecord[] records = linkTable.SetArray();

            int nCount = 0;

            foreach (LinkTableRecord record in records)
            {
                if (record.sSource == sSource)
                {

                }
            }

                int[] items = new int[groupEmLink.Count()];
                for (int i = 0; i < groupEmLink.Count(); i++)
                {
                    LinkTableRecord record = groupEmLink.GetLinkTableRecord(i.ToString());
                    if (record != null)
                    {
                        items[i] = Int32.Parse(record.sKey);
                    }
                }
            */
            } //set source

        }
        /// <summary>
        /// This will be public interface that the xml uses to grab
        /// the list of ids
        /// 
        /// When set, it will need to build the ArrayList
        /// </summary>
     /*   private int[] Items
        {
            get { return null; }
            set {
              Items = 
            }//set items
        
        
        }*/

        // This is the linktable that must be associated with the panel
        // before the panel is displayed
     //  public LinkTable linkTable; 

        /*January 2012 - Starting to REplace linktable*/
		public LinkTable StickyTable = null;

        #endregion publicvariables


        ////////////////////////////////////////////////////
        //
        // Interface
        //
        /////////////////////////////////////////////////////

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupEms_Load(object sender, EventArgs e)
        {
           
        }

        private void addNewGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddToGroup();
        }

        /// <summary>
        /// cycle through views
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            View currentView = listView.View;
            switch (currentView)
            {
                case View.Details: currentView = View.LargeIcon; listView.ShowGroups = true;
                    // if in details view we know we have to reload for some reason
                    LoadGroupEm();

                    
                    break;
                case View.LargeIcon: currentView = View.SmallIcon; listView.ShowGroups = true; break;
              //   case View.List: currentView = View.SmallIcon; break;
                case View.SmallIcon: currentView = View.Details; listView.ShowGroups = false; break;
                //   case View.Tile: currentView = View.Details; break;
            }
            listView.View = currentView;
            viewLabel.Text = currentView.ToString();

            // test removing all refreshes because they didn't help and I think they're screwing with fast update
            //listView.Refresh();

        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView.Sort();

        }

        /// <summary>
        /// true if in preview view
        /// </summary>
        public bool IsPreview
        {
            get { return !splitContainer1.Panel2Collapsed; }
        }

        /// <summary>
        /// called on reload on StickIt page if ShowPreview=true
        /// </summary>
        public void ShowPreview()
        {
            pictureBox.Visible = true;

            //listView.Dock = DockStyle.Top;
            // listView.Height = this.Height / 4;
            pictureBox.Dock = DockStyle.Fill;
            splitContainer1.Panel2Collapsed = false;
            pictureBox.BringToFront();

            // display current selected item 
            UpdatePicture();
        }

        /// <summary>
        /// splitter distance -- where the splitter bar is, stored in StickItPage
        /// </summary>
        public int SplitterPosition
        {
            get { return splitContainer1.SplitterDistance; }
            set { splitContainer1.SplitterDistance = value; }
        }

        /// <summary>
        /// toggles the presence of the picture box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bToggle_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel2Collapsed == false)
            {
                pictureBox.Visible = false;
                listView.Dock = DockStyle.Fill;
                splitContainer1.Panel2Collapsed = true;
            }
            else
            {
                ShowPreview();
            }
        }
        /// <summary>
        /// Converts the LinKTable to the StickyTable system
        /// Only called if the stickit page needed to create a new table (i.e., had not converted before)
        /// </summary>
//        public void Convert()
//        {
//            if (null != StickyTable)
//            {
//                NewMessage.Show("Converting Storyboard. Press OK to convert.");
//                LinkTableRecord[] records = linkTable.SetArray();
//                foreach (LinkTableRecord record in records)
//                {
//                    // we only want the records for this page
//                    if (record.sSource == sSource)
//                    {
//                        StickyTable.Convert(record);
//                    }
//                }
//            }
//        }
        ////////////////////////////////////////////////////
        //
        // Behavior
        //
        /////////////////////////////////////////////////////


        /// <summary>
        /// called at the start of most important functions.
        /// 
        /// Will throw an exception if linktable has not been set
        /// </summary>
        private void _Ready()
        {
            if (StickyTable == null)
            {
                throw new Exception("GroupEm. Linktable has not been set.");
            }
    
           
        }


        private bool bOneSafeRefresh = false; // set to true once the list has been refreshed once artificially
                                         // to get rid of drwaing errors

        /// <summary>
        /// Primarily this will be called from drag/drop activities
        /// but could also be called in response to a button press.
        /// <param name="sTitle">Title will be displayed in list</param>
        /// <param name="sLinkURL">Link will be used to retrieve contents.</param>
		/// <param name="AlternativeLink">Some things, like images, supply the image in sLinkURL and the GUID in AlternativeLInk, which is stored in ExtraField</param>
        /// </summary>
        public ListViewItem AddItem(string sTitle, string sLinkURL, int nType, string AlternativeLink)
        {
            _Ready();
            if (sTitle == null || sLinkURL == null)
            {
                throw new Exception("GroupEm.AddItem sTitle or sLinkURL null");
            }
            LinkTableRecord record = new LinkTableRecord();
            
            record.sText = sTitle;
            // Jan 2010: If you ahve a title of text%link the link will be the preview used
            record.sFileName = sLinkURL;
            record.sSource = sSource;
        
			record.ExtraField = AlternativeLink;
            

            record.nBookmarkKey = nType;

            //record.sExtra is USED FOR GROUPS, we cannot use it

            record.linkType = LinkType.FILE;  // FILE = Stretch POPUP = Center

            
            
                                    // NOPE, did not work, we already use sExtra Jan 2010 I am piggybacking sExtra -1 or "" = Stretch; -2 = No Stretch; if there is a valid file here
                                 // we use that for the thumbnail instead.


           // linkTable.AddLinkTableRecord(record);
           // linkTable.Save();

            if (null != StickyTable)
            {
                record = StickyTable.Add(record);
            }

            ListViewItem item = AddRecord(record);
            OnNeedSave(this); // January 2012. Added because Adds did not appear to register as needing saves

            record = null;
            
            // APril 2009: forcing a refresh to minimize the 
            // number of times the list gets out of sync
            // will only call this ONE time
            if (bOneSafeRefresh == false)
            {
                LoadGroupEm();
                bOneSafeRefresh = true;
            }
            return item;

        }
        /// <summary>
        /// Adds the record to the listview (it has already bbeen adding to the linktable)
        /// 
        /// Called both from AddItem and when Items are set
        /// </summary>
        /// <param name="record"></param>
        private ListViewItem AddRecord(LinkTableRecord record)
        {
            // by default using image index 0
            // a picture will override this
            int nIdx = 0;
            string sSubfolder = "Note";
            bool bFakePic = false;

            string sThumbnailImage = record.sFileName;
            string sTitle = record.sText;

            try
            {
                if (record.sText.IndexOf("%") > -1)
                {
                    string[] strings  = record.sText.Split('%');
                    sTitle = strings[0];

                    if (strings[1] != "")
                    {
                        sThumbnailImage = strings[1];
                        //record.nBookmarkKey = 1; // temp set to picture so it goes into next part
                        bFakePic = true;
                    }
                }
            }
            catch (Exception)
            {
            }


            if (record.nBookmarkKey == 1 || true == bFakePic) // 1 = picutre
            {
                
                // do special code, assigning 
                // the image indexes
                // NO need to store image indexes

             

                // Jan 2010 if the title is parseable with text%link we use link instead
                // of the otherimage (for character page thumbnails)
                
               


                if (File.Exists(sThumbnailImage) == true)
                {
                    Image image = Image.FromFile(sThumbnailImage);
                    imageList.Images.Add(image);
                    nIdx = imageList.Images.Count - 1;
                    imageListLarge.Images.Add(image);

                }

                sSubfolder = "Picture";

            }
            // add record to arraylist
            ListViewItem item = listView.Items.Add(record.sFileName, sTitle, nIdx) ;
            item.Tag =  record;


            

            //this will only happen on Load
            if (record.sExtra != null && record.sExtra != " " && record.sExtra != "")
            {
                
                AddItemToGroup(item, record.sExtra);
            }
            // May 2012 -- add group to the Details view (the subitems)
            if (item.Group != null && item.Group.Name != "")
            {
                item.SubItems.Add(item.Group.Name);
            }
            else
            {
                item.SubItems.Add("_default");
            }
            item.SubItems.Add(sSubfolder);
            // may 2012 - wanted to return the listview item so got rid of the item = null line that was here
            return item;
        } 
        /// <summary>
        /// edits the item in the linktable
        /// </summary>
        /// <param name="record"></param>
        public void EditItem(LinkTableRecord record)
        {
            _Ready();

            // replaces the existing record with Tag with a new record

           // linkTable.SetLinkTableRecord(record.sKey, record);
           // linkTable.Save();
            

            StickyTable.Edit(record);
            

        }

        /// <summary>
        /// Deletes the indicated record
        /// </summary>
        /// <param name="record"></param>
        public void DeleteItem(LinkTableRecord record)
        {
            _Ready();
           // linkTable.DeleteLinkTableRecord(record.sKey);
            /*January 2012*/
            /*We delete it from the database too*/
            if (null != StickyTable)
            {
                StickyTable.Delete(record);
            }
        }

      

       

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        /// <summary>
        /// if picture box present then updat ethe picture show in it
        /// to the selected index
        /// of the list view, unless its a note
        /// </summary>
        private void UpdatePicture()
        {
            //if (pictureBox.Visible == true)
            if (splitContainer1.Panel2Collapsed == false)
            {
                // how to retrieve linktable record info?
                if (listView.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView.SelectedItems[0];
                    if (item != null)
                    {
                        LinkTableRecord record = (LinkTableRecord)item.Tag;
                        if (record != null && record.nBookmarkKey == 1)
                        {

                            string sFileName = record.sFileName;
                            if (File.Exists(sFileName) == false) return; // we had no file here
                          
                            Image i = Image.FromFile(sFileName);
                            pictureBox.Visible = true;
                            if (i != null)
                            {
                                //pictureBox.Image = i;
                                pictureBox.BackgroundImage = i;
                                pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
                                i = null;
                            }
                            else
                            {
                                pictureBox.BackgroundImage = null;
                            //    pictureBox.Refresh();
                            }

                            if (LinkType.FILE == record.linkType)
                            {
                                pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
                            }
                            else
                                if (LinkType.POPUP == record.linkType)
                                {
                                    pictureBox.BackgroundImageLayout = ImageLayout.Center;
                                }
                                else
                                    if (LinkType.PAGE == record.linkType)
                                    {
                                        pictureBox.BackgroundImageLayout = ImageLayout.None;
                                    }


                        }
                        else
                        {
                            lg.Instance.Line("groupEms->UpdatePicture", ProblemType.MESSAGE, "UpdatePicture Tag was nullor not a picture");
                            pictureBox.BackgroundImage = null;
                            // try to laod text

                            pictureBox.Visible = false;
                            textBoxPreview.Visible = true;
                            textBoxPreview.Dock = DockStyle.Fill;
                            textBoxPreview.Rtf = OnGetNoteForGroupEmPreview(record.sFileName);

                            // force black for text color
                            textBoxPreview.SelectAll();
                            textBoxPreview.SelectionColor = Color.Black;
                            textBoxPreview.SelectionLength = 0;

                          //  pictureBox.Refresh();
                        }
                        record = null;
                    }
                    item = null;
                }
                else
                {
                    lg.Instance.Line("groupsems->UpdatePicture", ProblemType.MESSAGE,"UpdatePicture selected index was null");
                }
            }
        }




        /// <summary>
        /// Returns the currently selected items
        /// </summary>
        public ListView.SelectedListViewItemCollection SelectedItems
        {
            get { return listView.SelectedItems; }
        }

        /// <summary>
        /// can multiple items be interacted with?
        /// </summary>
        public bool MultiSelect
        {
            get { return listView.MultiSelect; }
            set { listView.MultiSelect = value; }
        }



        /// <summary>
        /// Adds the item then finds it and assigns it to a group [So far this seems used only when adding Facts and SearchItems through Addin_yourOthermindmarkup
        /// </summary>
        /// <param name="sTitle"></param>
        /// <param name="sLinkURL"></param>
        /// <param name="nType"></param>
        /// <param name="sGroup"></param>
        public void AddItemAndToGroup(string sTitle, string sLinkURL, int nType, string sGroup)
       {
            ListViewItem item = AddItem(sTitle, sLinkURL, nType,"");
            
            // Potential issue, might not find the one you just added
            // if things have the same name

           // ListViewItem[] items = listView.Items.Find(sLinkURL, true);
           // if (items != null && items.Length > 0)
            {
                AddItemToGroup(item, sGroup);
                // may 2012 -- adding subitem group information too
                item.SubItems[1].Text = sGroup; // set 2nd details column to Group
                
            }
        }

        private bool mShowImage = true;
        /// <summary>
        /// if set to false, images won't be shown
        /// </summary>
        public bool ShowImages
        {
            get { return mShowImage; }
            set
            {
                mShowImage = value;
                if (mShowImage == true)
                {
                    listView.SmallImageList = imageList;
                    listView.LargeImageList = imageListLarge;
                }
                else
                {
                    listView.SmallImageList = null;
                    listView.LargeImageList = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public  ListView.ListViewItemCollection Items
        {
            get { return listView.Items ;}
        }
        

        /// <summary>
        /// Adds item to sGroup, creating the group if necessary
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sGroup"></param>
        private void AddItemToGroup(ListViewItem item, string sGroup)
        {
            ListViewGroup group = null;


        

            // look for existing group
            if (listView.Groups[sGroup] != null)
            {
                group = listView.Groups[sGroup];
            }
            else
            {
                group = new ListViewGroup(sGroup);
                group.Name = sGroup;
            }



            listView.Items.Remove(item);
            item.Group = group;

            /// If name too long then truncate it to last 30 characters
            if (group.Name.Length > groupNameTruncateLength)
            {
                int nIndexLastSlash = group.Name.LastIndexOf("\\");
                int nLengthToCopy = 10;

                // if no directory slash then just last 10 digits
                if (nIndexLastSlash <= -1)
                {
                    nIndexLastSlash = group.Name.Length - groupNameTruncateLength;
                }
                else
                {
                    nIndexLastSlash++;
                    // we found a directory
                    nLengthToCopy = group.Name.Length - nIndexLastSlash ;
                }


                group.Header = group.Name.Substring(nIndexLastSlash, nLengthToCopy);
            }
            listView.Items.Add(item);
            listView.Groups.Add(group);
            


            // To  - Do : This is where we require editing of existing
            // records to do the group assignment
            LinkTableRecord record = (LinkTableRecord)item.Tag;
            if (record != null)
            {
                record.sExtra = sGroup;
                EditItem(record);
            }
            else
            {
                lg.Instance.Line("groupEms->AddItemToGroup", ProblemType.MESSAGE,"Add new group record tag was null");
            }

            
            group = null;
            
            OnNeedSave(this);

            
        }

        /// <summary>
        /// Prompts the user to add a new group to the existing, selected item
        /// </summary>
        private void AddToGroup()
        {
            if (listView.SelectedItems.Count > 0)
            {
                // add the group

                // add the group to the context menu too (this will be rebuilt on Load)

                //ListViewItem item = listView.SelectedItems[0];
                
                // to -do popup GUI
                //NewMessage?
                

                string sGroup = NewMessage.Show("Create Group", "Enter name for new group", null, true, null);
                foreach (ListViewItem item in listView.SelectedItems)
                {
                    if (sGroup != "")
                    {
                        AddItemToGroup(item, sGroup);

                        // April 10 2009 - forcing a full refresh to get rid of redraw problems
                        LoadGroupEm();
                    }
                }
                
                
                

            }
            else
            {
                NewMessage.Show("Please select an item first");
            }
        }

      /// <summary>
      /// hooking up both clicks to different event
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
//        private void listView_Click(object sender, EventArgs e)
//        {
//            if (listView.SelectedItems.Count > 0 && listView.SelectedItems[0] != null)
//            {
//                OnSingleClickItem((LinkTableRecord)listView.SelectedItems[0].Tag);
//            }
//        }

        /// <summary>
        ///  on double click we will generaly open the item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_DoubleClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0 && listView.SelectedItems[0] != null)
            {
                OnClickItem((LinkTableRecord)listView.SelectedItems[0].Tag);
            }
        }

        /// <summary>
        /// When opening we will fill in the list of groups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                // Delete items with Tag = Temp
                for (int i = contextMenuStrip1.Items.Count - 1; i > -1; i--)
                {
                    if (contextMenuStrip1.Items[i] != null)
                    {
                        if (contextMenuStrip1.Items[i].Tag != null)
                        {
                            if (contextMenuStrip1.Items[i].Tag.ToString() == "Temp")
                            {
                                contextMenuStrip1.Items.Remove(contextMenuStrip1.Items[i]);

                            }
                        }
                    }

                }




                // Part 1 Add any custom groups being used so far
                ArrayList items = new ArrayList();

                foreach (ListViewGroup group in listView.Groups)
                {
                    items.Add(group.Name);
                }

                // Part 2 (NOT DONE) Refresh list of Groups from the file specified when the item created


                // Part 3
                // GO through the array of strings made above and add them all, after sorting it

                items.Sort();

                foreach (string s in items)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(s);
                    item.Tag = "Temp";
                    item.Click += new EventHandler(item_Click);
                    contextMenuStrip1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                NewMessage.Show(ex.ToString());
            }
        }

        /// <summary>
        /// This is what happens when you click on a "group"
        /// 
        /// WHen you select this the currently highlighted item is assigned to that group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {

                foreach (ListViewItem item in listView.SelectedItems)
                {
                    //ListViewItem item = listView.SelectedItems[0];



                    {
                        AddItemToGroup(item, (sender as ToolStripMenuItem).Text);
                    }
                }//foreach




            }
            else
            {
                NewMessage.Show("Please select an item first");
            }
        }

        private void editCaptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoEditItem();
        }

        /// <summary>
        /// Edits the currently selected item
        /// </summary>
        private void DoEditItem()
        {
            if (listView.SelectedItems.Count > 0)
            {
                // add the group

                // add the group to the context menu too (this will be rebuilt on Load)

                ListViewItem item = listView.SelectedItems[0];

                // to -do popup GUI
                //NewMessage?
                if (item != null)
                {
                    LinkTableRecord record = (LinkTableRecord)item.Tag;
                    string sGroup = NewMessage.Show("Edit Caption", String.Format("Enter new caption"), null, true, record.sText);
                    if (sGroup != "")
                    {
                        // To  - Do : This is where we require editing of existing
                        // records to do the group assignment
                      
                        if (record != null)
                        {
                            record.sText = sGroup;
                            item.Text = sGroup;

                            
                            EditItem(record);

                            // reload to force  afull alphabetical resort
                            LoadGroupEm();
                            listView.Refresh();
                        }
                        else
                        {
                            lg.Instance.Line("groupEms->DoEditItem", ProblemType.MESSAGE,"Edit title record tag was null");

                        }
                    }
                } // item not null




            }
            else
            {
                NewMessage.Show("Please select an item first");
            }
        }
        /// <summary>
        /// deletes all items in this storyboard
        /// </summary>
        public void DeleteRecordsForStoryboard()
        {
            for (int i = listView.Items.Count - 1; i >= 0; i--)
            {
                ListViewItem item = listView.Items[i];
                LinkTableRecord record = (LinkTableRecord)item.Tag;
                DeleteItem(record);
                item.Remove();
                OnItemDeleted(record);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOn"></param>
        public void FactMode(bool isOn)
        {
            toolStrip1.Visible = !isOn;
            
        }

        /// <summary>
        /// Deletes the selected item
        /// </summary>
        private void DeleteItemInList()
        {

            if (listView.SelectedItems.Count > 0)
            {

                // delete each one
                for (int i = listView.SelectedItems.Count-1; i >= 0; i--)
                {
                    ListViewItem item = listView.SelectedItems[i];
                    if (NewMessage.Show("Delete?", String.Format("Are you sure you want to delete {0}", item.Text), MessageBoxButtons.YesNo, null) == DialogResult.Yes)
                    {


                        LinkTableRecord record = (LinkTableRecord)item.Tag;
                        DeleteItem(record);
                        item.Remove();
                        OnItemDeleted(record);
                    }
                }



                

            }
        }

        private int groupNameTruncateLength = 10;
        /// <summary>
        /// sets the length of how long a groupname is allowed to be
        /// Note: If the groupname is a path it will be truncated to
        ///   the last directory name regardless of length
        ///  (i.e., MyStuff\FolderA would be FolderA
        /// </summary>
        public int GroupNameTruncateLength
        {
            get { return groupNameTruncateLength; }
            set { groupNameTruncateLength = value; }
        }

        /// <summary>
        /// deletes the selected item from the link table and the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDeleteItem_Click(object sender, EventArgs e)
        {
            DeleteItemInList();
        }

        /// <summary>
        /// allows dragging of files directly onto it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_DragEnter(object sender, DragEventArgs e)
        {
         
            lg.Instance.Line("groupEms->ListView_DragEnter", ProblemType.MESSAGE,"group em drag enter entered");
            ///allowing drop of listbox items
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true /*|| e.Data.GetDataPresent(DataFormats.StringFormat)*/)
            {
               lg.Instance.Line("groupEms->ListView_DragEnter", ProblemType.MESSAGE,"proper data found");
                // allow them to continue
                // (without this, the cursor stays a "NO" symbol
                //if (activePicture != null)
                {
                    e.Effect = DragDropEffects.All;
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        CoreUtilities.NewMessage.Show(ex.ToString());
                    }
                }
            }
            else if (e.Data.GetDataPresent("DrawingTest.Appearance") == true)
            {
                // DrawingTest.Appearance App = (DrawingTest.Appearance)e.Data.GetData("DrawingTest.Appearance");
                //MessaeBox.Show("entering with data" + App.Caption);
                e.Effect = DragDropEffects.Copy;
            }
            else
                if (e.Data.GetDataPresent("CoreUtilities.General+Listitem") == true)
                {
                    e.Effect = DragDropEffects.All;
                }
        }

        /// <summary>
        /// a file has been dropped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_DragDrop(object sender, DragEventArgs e)
        {
           lg.Instance.Line("groupEms->listview_dragdrop", ProblemType.MESSAGE, "group em drag DROP entered");
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    this.Cursor = Cursors.WaitCursor;

                    // loop through the string array, adding each filename to the ListBox
                    foreach (string file in files)
                    {


                        //  Logs.LineF("a picture panel is setting NEEDS save to true for file {0}", file);
                        //needssave

                        if (General.IsGraphicFile(file) == true)
                        {

                            // add the picture to the groupem
                            AddItem(new FileInfo(file).Name, file, 1,"");



                        }
                        else
                        {
                            // add text files too
                            AddItem(new FileInfo(file).Name, file, 0,"");
                        }
                    }
                    this.Cursor = Cursors.Default;
                }// more than 0 files
            }//dragdrop of a PICTURE file
            else if (e.Data.GetDataPresent("DrawingTest.Appearance") == true)
            {
                //e.Effect = DragDropEffects.All;
                // problem I don't know anything about this dataformat in this class
                // can I do a callback?
                //Yes.
                OnDragNote(e.Data.GetData("DrawingTest.Appearance"), this);
            }
            else

               /* if (e.Data.GetDataPresent(DataFormats.StringFormat) == true)
                {
                    // we are dropping a listbox item
                    MessagBox.Show(e.Data.GetData(DataFormats.StringFormat).ToString());
                }
                else*/
                if (e.Data.GetDataPresent("CoreUtilities.General+Listitem") == true)
                {
                    Listitem App = (Listitem)e.Data.GetData("CoreUtilities.General+Listitem");
                    if (App != null)
                    {
                        AddItem(App.FancyCaption, App.sTag, 0,"");
                    }
                }
        }

        /// <summary>
        /// go the previous selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bPrevious_Click(object sender, EventArgs e)
        {
            if (listView.Items == null || listView.Items.Count == 0) return;
            
            if (listView.SelectedItems.Count <= 0)
            {
                // go to first if none selected
                listView.SelectedIndices.Add(0);
            }
            else
            {
                if (listView.SelectedIndices[0] > 0)
                {
                    int nPosition = listView.SelectedIndices[0];
                    listView.SelectedItems.Clear();
                    listView.SelectedIndices.Add(nPosition - 1);
                    listView.Refresh();
                }
            }
        }

        /// <summary>
        /// Go to next item in list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bNext_Click(object sender, EventArgs e)
        {
            if (listView.Items == null || listView.Items.Count == 0) return;

            if ( listView.SelectedItems.Count <= 0)
            {
                // go to first if none selected
                listView.SelectedIndices.Add(0);
            }
            else
            {
                if (listView.SelectedIndices[0] < listView.Items.Count-1)
                {
                    int nPosition = listView.SelectedIndices[0];
                    listView.SelectedItems.Clear();
                    listView.SelectedIndices.Add(nPosition + 1);
                    listView.Refresh();
                }
            }
        }

        

        /// <summary>
        /// calls the handler 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addItemToListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //sending the actual group em
            OnAddItemFromMenu(this);
        }

        private void printSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // go through and create an array of GUID
            string[] sGUID = new string[listView.SelectedItems.Count];
            int nCount= -1;
            foreach (ListViewItem item in listView.SelectedItems)
            {
                nCount++;
                if (item != null)
                {
                    sGUID[nCount] = ((LinkTableRecord)item.Tag).sFileName;
                }
            }
            OnPrintSelected(sGUID);
        }

        private void exportSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            // go through and create an array of GUID
            string[] sGUID = new string[listView.SelectedItems.Count];
            int nCount = -1;
            foreach (ListViewItem item in listView.SelectedItems)
            {
                nCount++;
                if (item != null)
                {
                    sGUID[nCount] = ((LinkTableRecord)item.Tag).sFileName;
                }
            }
            OnExportSelected(sGUID);
        }

        ////////////////////////////////////////////////////////////////////////////
        //
        // CUSTOM EVENTS
        //
        ////////////////////////////////////////////////////////////////////////////
        public delegate string CustomEventHandler(object sender);
        public delegate string CustomEventHandler2(object oAppearance, object oSender);

        // custom events
        public virtual event CustomEventHandler ClickItem;
      //  public virtual event CustomEventHandler SingleClickItem;
        public virtual event CustomEventHandler NeedSave;
        public virtual event CustomEventHandler AddItemFromMenu;
        public virtual event CustomEventHandler PrintSelected;
        public virtual event CustomEventHandler ExportSelected;
        public virtual event CustomEventHandler2 DragNote;
        public virtual event CustomEventHandler ItemDeleted;

        public virtual event CustomEventHandler GetNoteForGroupEmPreview;
        /// <summary>
        /// Added ability to display notes and note just pictures
        /// 
        /// This gets the Note Text in palin-text format
        /// </summary>
        /// <param name="oSender"></param>
        /// <returns></returns>
        public string OnGetNoteForGroupEmPreview(object oSender)
        {
            if (GetNoteForGroupEmPreview != null)
            {
                return GetNoteForGroupEmPreview(oSender);
            }

            return null;
        }

        /// <summary>
        /// this clicker reutrns the filename
        /// </summary>
        /// <param name="oSender"></param>
        /// <returns></returns>
        public string OnClickItem(object oSender)
        {
            if (ClickItem != null)
            {
                return ClickItem(oSender);
            }

            return null;
        }

        /// <summary>
        /// this clicker reutrns the filename
        /// </summary>
        /// <param name="oSender"></param>
        /// <returns></returns>
//        public string OnSingleClickItem(object oSender)
//        {
//            if (SingleClickItem != null)
//            {
//                return SingleClickItem(oSender);
//            }
//
//            return null;
//        }

        /// <summary>
        /// Clean up the interface after the delete
        /// </summary>
        /// <param name="oSender"></param>
        /// <returns></returns>
        public string OnItemDeleted(object oSender)
        {
            if (ItemDeleted != null)
            {

               

                return ItemDeleted(oSender);
            }

            return null;
        }


        /// <summary>
        /// Called when a note is dragged from a Drawing.Appearance object
        /// oSender is the object
        /// </summary>
        /// <param name="oSender"></param>
        /// <returns></returns>
        public string OnDragNote(object oAppearance, object oSender)
        {
            if (DragNote != null)
            {
                return DragNote(oAppearance, oSender);
            }

            return null;
        }

        /// <summary>
        /// setup to register a save event
        /// </summary>
        /// <param name="oSender"></param>
        /// <returns></returns>
        public string OnNeedSave(object oSender)
        {
            if (NeedSave != null)
            {
                return NeedSave(oSender);
            }

            return null;
        }
        /// <summary>
        /// when multipel entries selected for PRINTING
        /// </summary>
        /// <param name="oSender"></param>
        /// <returns></returns>
        public string OnPrintSelected(object oSender)
        {
            if (PrintSelected != null)
            {
                return PrintSelected(oSender);
            }
            return null;
        }

        public string OnExportSelected(object oSender)
        {
            if (ExportSelected != null)
            {
                return ExportSelected(oSender);
            }
            return null;
        }
        /// <summary>
        /// setup to register a save event
        /// </summary>
        /// <param name="oSender"></param>
        /// <returns></returns>
        public string OnAddItemFromMenu(object oSender)
        {
            if (AddItemFromMenu != null)
            {
                return AddItemFromMenu(oSender);
            }

            return null;
        }

        /// <summary>
        /// manual refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bRefresh_Click(object sender, EventArgs e)
        {
            LoadGroupEm();
        }

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteItemInList();
        }
        bool bAutoRefresh = false;
        private void listView_Enter(object sender, EventArgs e)
        {
         
        }

        private void groupEms_Enter(object sender, EventArgs e)
        {
          
        }

        private void groupEms_MouseEnter(object sender, EventArgs e)
        {
            // march 2010 trying to fix the 'scroll-jump' bug in groupems
            //listView.Focus();
        }

        private void listView_MouseEnter(object sender, EventArgs e)
        {
            // jan 2010 - if I don't do this then when you scroll it gets out of sync withthe actual list
            //this.Focus();
            // jan 8 2010 - this did not work

            
            // one time only refresh to get rid of 'missing icons'
            if (bAutoRefresh == false)
            {
                bAutoRefresh = true;

                // January 27 2011 - REMOVED THIS because it was not helping.
               // LoadGroupEm();


               // listView.Refresh(); FAILED
               // listView.Invalidate(true); FAILED
                //listView.RedrawItems(0, listView.Items.Count - 1, false); FAILED
               // listView.RedrawItems(0, listView.Items.Count - 1, true); FAILED


                // now trying pairs
               
                /* FAILED
                listView.Invalidate(true); 
               listView.RedrawItems(0, listView.Items.Count - 1, false); 
                listView.RedrawItems(0, listView.Items.Count - 1, true);
                listView.Refresh(); 
                 */

            }
        }
        /// <summary>
        /// sets new iamge size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stretchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                // add the group

                // add the group to the context menu too (this will be rebuilt on Load)

                ListViewItem item = listView.SelectedItems[0];

                // to -do popup GUI
                //NewMessage?
                if (item != null)
                {

                    // To  - Do : This is where we require editing of existing
                    // records to do the group assignment
                    LinkTableRecord record = (LinkTableRecord)item.Tag;
                    if (record != null)
                    {
                        // jan 2010 - adding image stretching

                        if (record.linkType == LinkType.FILE)
                        {
                            record.linkType = LinkType.POPUP;
                        }
                        else if (record.linkType == LinkType.POPUP)
                        {
                            record.linkType = LinkType.PAGE;

                        }
                        else if (record.linkType == LinkType.PAGE)
                        {
                            record.linkType = LinkType.FILE;

                        }

                        EditItem(record);
                        UpdatePicture();
                        // reload to force  afull alphabetical resort
                        // LoadGroupEm();
                        // listView.Refresh();
                    }
                    else
                    {
                       lg.Instance.Line("groupEms->stretchToolStripMenuItemClick", ProblemType.MESSAGE, "Edit picturesize record tag was null");
                    }
                }

            } // item not null
        }

        //bool bSetParentSelected = false; // this is used by VirtualDesktop selection; we only do this once per 'use'. Use is cleared when 
                                         // the user moves from the control (March 2010 - moved all this into VirtualDesktop; more appropriate there
        private void listView_onScroll(object sender, ScrollEventArgs e)
        {
          /*  if (OnMouseDown != null && false == bSetParentSelected)
            {
                // this should intiiate a 'set selected' back in the VirtualDesktop.
                OnMouseDown(;
                bSetParentSelected = true;
            }

            //if (listView.Focused == false)
            {
               // NewMessage.Show("yep");
             //   listView.Focus();
            }
           * */
        }

        private void listView_Leave(object sender, EventArgs e)
        {
         //   bSetParentSelected = false;
        }

		/// <summary>
		/// I made thjis a warpper so I can watch for external influences
		/// </summary>
		public void InitializeGroupEmFromExternal()
		{
			LoadGroupEm();
		}
		
		/// <summary>
		/// called when source is set or when an item is added ind etials view (to work around a bug in that)
		/// </summary>
		private void LoadGroupEm()
		{
			
			/* Optimization Section
            1. Look at ways to optimize this for LARGE values (hold the linktable in memory?) (Add also needs optimization) 
            Answer: I am already doing this
             */
			
			
			// _Ready();
			if (StickyTable != null)
			{
				this.Cursor = Cursors.WaitCursor;

				listView.Groups.Clear();
				listView.Items.Clear();
				
				//erase items to start
				
				
				
				
				// add the columns if not already added
				if (listView.Columns.Count == 0)
				{
					ColumnHeader columnheader = new ColumnHeader();
					columnheader.Text = "Title";
					columnheader.Width = mFirstColumnWidth;
					
					// setup sorting
					if (lvwColumnSorter == null)
					{
						lvwColumnSorter = new ListViewColumnSorter();
						this.listView.ListViewItemSorter = lvwColumnSorter;
						lvwColumnSorter.Order = SortOrder.Ascending;
						lvwColumnSorter.SortColumn = 0;
					}
					
					listView.Columns.Add(columnheader);
					if (NumberOfColumns > 1)
					{
						listView.Columns.Add("Group");
						listView.Columns.Add("Type");
					}
					
				}
				
				
				/* May 2009 
                 * POtential bug?
                 * I did not change this but is it possible that SetARray() is
                 * potentially going to work on an out of date linktable??
                 * 
                 * - should we force a reload of it?
                 */
				
				
				//  LinkTableRecord[] records = linkTable.SetArray();
				
				LinkTableRecord[] records = null;
				if (StickyTable != null)
				{
					records = StickyTable.GetRecords();
				}
				
				if (records == null)
				{
					throw new Exception("records in LoadGroupEm were null. I believe this is the cause of lost data, the link table somehow becoming changed or missing. Press abort, restart now to prevent further data loss.");
				}
				
				
				// How to sort groups (Not implemented yet)
				// I could have an arraylist, and keep all the found entries
				// and another arraylist for groups
				// Then create teh groups, after sorting them?
				// Then go througha rraylist and add the items
				ArrayList groups = new ArrayList();
				ArrayList matches = new ArrayList();
				
				foreach (LinkTableRecord record in records)
				{
					// locate in link table
					// load from link table
					//  LinkTableRecord record = linkTable.GetLinkTableRecord(nKey.ToString());
					
					if (record != null && record.sSource == sSource)
					{
						// add record to arraylist
						// ListViewItem item = listView.Items.Add(record.sFileName, record.sText, record.nBookmarkKey);
						// item.SubItems.Add(record.sExtra);
						// item = null;
						
						if (groups.IndexOf(record.sExtra) == -1)
						{
							// we found a new group
							// add it
							groups.Add(record.sExtra);
						}
						matches.Add(record);
						//AddRecord(record);
						
					}
					
					
					// add to item view
				} // for each
				
				
				ListViewGroup group;
				
				groups.Sort();
				
				//clear groups
				
				
				if (SortGroup == true)
				{
					
					// now we add all the groups
					foreach (string s in groups)
					{
						if (s != null && s != "")
						{
							group = new ListViewGroup(s);
							group.Name = s;
							listView.Groups.Add(group);
							
						}
					}
				}
				/* Why does the listview update itself even with begin/end update?
                 * Because of the clearing of the groups, above
                 * 
                 * If we accepted unsorted groups, we would not have this problem.
                 */
				
				listView.BeginUpdate();
				
				// now we add the records we found
				foreach (LinkTableRecord record in matches)
				{
					AddRecord(record);
				}
				
				listView.EndUpdate();
				
				group = null;
				groups = null;
				matches = null;
				
				
				
			} // linktable not null
			
			this.Cursor = Cursors.Default;
		}

    }// class
}// namespace
