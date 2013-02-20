using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using System.IO;

namespace DiffEngine
{
    public partial class DiffControl : UserControl
    {
        public DiffControl()
        {
            InitializeComponent();
        }

        private void DoCompare(DiffList_TextFile source, DiffList_TextFile destination, ArrayList DiffLines, double seconds)
        {
          
          //  label1.Text = string.Format("Results: {0} secs.", seconds.ToString("#0.00"));
            lvSource.Items.Clear();
            lvDestination.Items.Clear();

            int nChanges = 0;

            ListViewItem lviS;
            ListViewItem lviD;
            int cnt = 1;
            int i;

            foreach (DiffResultSpan drs in DiffLines)
            {
                switch (drs.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(cnt.ToString("00000"));
                            lviD = new ListViewItem(cnt.ToString("00000"));
                            lviS.BackColor = Color.Red;
                            lviS.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD.BackColor = Color.LightGray;
                            lviD.SubItems.Add("");

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;

                            nChanges++;

                        }

                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(cnt.ToString("00000"));
                            lviD = new ListViewItem(cnt.ToString("00000"));
                            lviS.BackColor = Color.White;
                            lviS.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD.BackColor = Color.White;
                            lviD.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;

                           
                        }

                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(cnt.ToString("00000"));
                            lviD = new ListViewItem(cnt.ToString("00000"));
                            lviS.BackColor = Color.LightGray;
                            lviS.SubItems.Add("");
                            lviD.BackColor = Color.LightGreen;
                            lviD.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;

                            nChanges++;

                        }

                        break;
                    case DiffResultSpanStatus.Replace:
                        for (i = 0; i < drs.Length; i++)
                        {
                            lviS = new ListViewItem(cnt.ToString("00000"));
                            lviD = new ListViewItem(cnt.ToString("00000"));
                            lviS.BackColor = Color.Red;
                            lviS.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                            lviD.BackColor = Color.LightGreen;
                            lviD.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);

                            lvSource.Items.Add(lviS);
                            lvDestination.Items.Add(lviD);
                            cnt++;

                            nChanges++;
                        }

                        break;
                }

            }
            statusLabel.Text = String.Format("Changes {0}", nChanges.ToString());
            labelWordsNew.Text = String.Format("Words: {0}", destination.words.ToString());
            labelWordsOld.Text =  String.Format("Words: {0}", source.words.ToString());
            
        }

        private void lvSource_Resize(object sender, EventArgs e)
        {
            if (lvSource.Width > 100)
            {
                lvSource.Columns[1].Width = -2;
            }
        }

        private int boxupdatecount;
        // Whenever we equal two (we have updated both boxes)
        // we update the comparision text AND THEN set to 0
        private int BoxUpdateCount
        {
            get { return boxupdatecount; }
            set
            {
                boxupdatecount = value;

                if (boxupdatecount == 2)
                {
                    UpdateComparisonBox();
                    boxupdatecount = 0;
                }
            }
        }

        private void lvSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSource.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvDestination.Items[lvSource.SelectedItems[0].Index];
                lvi.Selected = true;
                lvi.EnsureVisible();
                string sText = lvSource.SelectedItems[0].SubItems[1].Text;
                textBoxDest.Text = sText;
                BoxUpdateCount++;
                
            }
        }

        private void lvDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDestination.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvSource.Items[lvDestination.SelectedItems[0].Index];
                lvi.Selected = true;
                lvi.EnsureVisible();
                string sText = lvDestination.SelectedItems[0].SubItems[1].Text;
                textBoxSource.Text = sText;
              //  MessgeBox.Show(lvi.SubItems[0].Text + "jjj" + lvi.SubItems[1].Text + " lll " + lvi.SubItems[0].ToString() + " ;; " + lvi.Text + " aaa " + lvi.ToString());
                BoxUpdateCount++; 
            }
        }
        /// <summary>
        ///  now we highlight any changes between the actual textboxes
        /// </summary>
        private void UpdateComparisonBox()
        {
            // now we highlight any changes between the actual textboxes
            
          

            DiffList_TextFile sLF = new DiffList_TextFile(textBoxSource.Text, true);
            DiffList_TextFile dLF = new DiffList_TextFile(textBoxDest.Text, true);


                    double time = 0;
                    DiffEngine de = new DiffEngine();
                    time = de.ProcessDiff(sLF, dLF, _level);



                    ArrayList rep = de.DiffReport();
                   int  nChanges = rep.Count;
                    //Results dlg = new Results(sLF, dLF, rep, time);
                   // this.DoCompare(sLF, dLF, rep, time);
                 //   textBoxSource.Text = textBoxSource.Text + " >>>> " + nChanges.ToString();


             //   textBoxSource.Text = "";
          //  textBoxDest.Text = "";

                   int i = 0;
                    foreach (DiffResultSpan drs in rep)
                    {
                        switch (drs.Status)
                        {
                            case DiffResultSpanStatus.DeleteSource:
                                for (i = 0; i < drs.Length; i++)
                                {



                                    string sSource = ((TextLine)sLF.GetByIndex(drs.SourceIndex + i)).Line;
                                    textBoxSource.Find(sSource);
                                    textBoxSource.SelectionBackColor = Color.Red;
                                  //  lviD.BackColor = Color.LightGray;
                          
                                   // cnt++;

                                    nChanges++;

                                }

                                break;
                            case DiffResultSpanStatus.NoChange:
                                for (i = 0; i < drs.Length; i++)
                                {
                                  /*  lviS = new ListViewItem(cnt.ToString("00000"));
                                    lviD = new ListViewItem(cnt.ToString("00000"));
                                    lviS.BackColor = Color.White;
                                    lviS.SubItems.Add(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line);
                                    lviD.BackColor = Color.White;
                                    lviD.SubItems.Add(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line);

                                    lvSource.Items.Add(lviS);
                                    lvDestination.Items.Add(lviD);
                                  */ // cnt++;
                                    

                                }

                                break;
                            case DiffResultSpanStatus.AddDestination:
                                for (i = 0; i < drs.Length; i++)
                                {


                                    string sDest = ((TextLine)dLF.GetByIndex(drs.DestIndex + i)).Line;
                                    textBoxDest.Find(sDest);
                                    textBoxDest.SelectionBackColor = Color.LightGreen;
                                   // cnt++;

                                    nChanges++;

                                }

                                break;
                            case DiffResultSpanStatus.Replace:
                                for (i = 0; i < drs.Length; i++)
                                {


                                    string sSource = (((TextLine)sLF.GetByIndex(drs.SourceIndex + i)).Line);
                                     textBoxSource.Find(sSource);
                                    textBoxSource.SelectionBackColor = Color.LightGreen;

                                    string sDest = (((TextLine)dLF.GetByIndex(drs.DestIndex + i)).Line);
                                    textBoxDest.Find(sDest);
                                     textBoxDest.SelectionBackColor = Color.Red;
                                 
                                  //  cnt++;

                                    nChanges++;
                                }

                                break;
                        }

                    }


                
        }
        private void lvDestination_Resize(object sender, EventArgs e)
        {
            if (lvDestination.Width > 100)
            {
                lvDestination.Columns[1].Width = -2;
            }
        }

        /// <summary>
        /// Keeping them equal width??
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiffControl_Resize(object sender, EventArgs e)
        {
           /* int w = this.ClientRectangle.Width / 2;
            lvSource.Location = new Point(0, 0);
            lvSource.Width = w;
            lvSource.Height = this.ClientRectangle.Height;

            lvDestination.Location = new Point(w + 1, 0);
            lvDestination.Width = this.ClientRectangle.Width - (w + 1);
            lvDestination.Height = this.ClientRectangle.Height;*/
        }

        private void DiffControl_Load(object sender, EventArgs e)
        {
            DiffControl_Resize(sender, e);
            textBoxSource.Width = textBoxDest.Width;
        } // DoCompare

        private bool ValidFile(string fname)
        {
            if (fname != string.Empty)
            {
                if (File.Exists(fname))
                {
                    return true;
                }
            }
            return false;
        }

        private DiffEngineLevel _level = DiffEngineLevel.Medium;

        /// <summary>
        /// returns the number of differences
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="dFile"></param>
        /// <returns></returns>
        private int TextDiff(string sFile, string dFile)
        {
            this.Cursor = Cursors.WaitCursor;

            int nChanges = 0;

            DiffList_TextFile sLF = null;
            DiffList_TextFile dLF = null;
            try
            {
                sLF = new DiffList_TextFile(sFile);
                dLF = new DiffList_TextFile(dFile);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                CoreUtilities.NewMessage.Show(ex.Message, "File Error");
                return -1;
            }

            try
            {
                double time = 0;
                DiffEngine de = new DiffEngine();
                time = de.ProcessDiff(sLF, dLF, _level);



                ArrayList rep = de.DiffReport();
                 nChanges = rep.Count;
                //Results dlg = new Results(sLF, dLF, rep, time);
                this.DoCompare(sLF, dLF, rep, time);
                //dlg.ShowDialog();
                //dlg.Dispose();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
                    ex.Message,
                    Environment.NewLine,
                    ex.StackTrace);
                CoreUtilities.NewMessage.Show(tmp, "Compare Error");
                return -1;
            }
            this.Cursor = Cursors.Default;
            return nChanges;
        }

        /// <summary>
        /// Compare two text files and will show them 
        /// in this usre control
        /// 
        /// returns the number of differneces
        /// </summary>
        /// <param name="sOldFile"></param>
        /// <param name="sCurrentFile"></param>
        /// <returns></returns>
        public int ShowTheDifference(string sOldFile, string sCurrentFile)
        {
            string sFile = sOldFile;
            string dFile = sCurrentFile;

            if (!ValidFile(sFile))
            {

                throw new Exception("ShowTheDifference: You did not pass a valid file in");
            }

            if (!ValidFile(dFile))
            {

                throw new Exception("ShowTheDifference: You did not pass a valid file in");
            }

            /*   if (rbFast.Checked)
               {
                   _level = DiffEngineLevel.FastImperfect;
               }
               else
               {
                   if (rbMedium.Checked)
                   {
                       _level = DiffEngineLevel.Medium;
                   }
                   else
                   {
                       _level = DiffEngineLevel.SlowPerfect;
                   }
               }*/

            /*if (chkBinary.Checked)
            {
                BinaryDiff(sFile, dFile);
            }
            else
            {*/
            return TextDiff(sFile, dFile);

        }
        /// <summary>
        /// used to blank the findings
        /// </summary>
        public void Clear()
        {
            lvSource.Items.Clear();
            lvDestination.Items.Clear();
        }
    }
}
