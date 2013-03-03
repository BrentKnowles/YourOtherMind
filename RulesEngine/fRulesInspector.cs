using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CoreUtilities;
using System.Reflection;


/* Serves two purposes
 *  1. View readonly a 'live' datatable (the table can be saved, not edited)
 *  2. Saving/loading involves converting from an XML datatable (easiest editing)
 *  Note: the core data format CAN be saved/loaded but having an intermediary format
 *    makes editing/importing rules from elsewhere a lot easier
 */


namespace RulesMess
{
    public partial class fRulesInspector : Form
    {
        public fRulesInspector()
        {
            InitializeComponent();
        }

        private bool isreadonly;
        /// <summary>
        /// if true just for viewing
        /// </summary>
        public bool ReadOnly
        {
            get { return isreadonly; }
            set { isreadonly = value; }
        }

        /// <summary>
        /// show the knowledge base in a table?
        /// </summary>
        /// <param name="knowledge"></param>
        public void formLoad(KnowledgeBase k)
        {

       //     DataSet ds = new DataSet();
       //    ds.ReadXml(Application.StartupPath + "\\knowledge.xml");
  //         listBoxRules.DataSource = k.ToArray();// ds.Tables[2];
            dataGridView1.DataSource = k.ToArray();          
            
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create a brand new table
            // building teh columns based on rules
            Relationship temp = new Relationship();

			//TODO: The commented out line worked in previous version but would not compile here so I had to 
			// change it, to the following, which I'm not sure if it is correct.
			System.Reflection.PropertyInfo[] names = typeof(Relationship).GetProperties (BindingFlags.Public| BindingFlags.Static);

           // System.Reflection.PropertyInfo[] names = Reflection.GetPropertyNames(temp);
            if (names == null)
            {
                throw new Exception("Relationship does not have any names!?");
            }
            DataTable table = new DataTable();
            foreach (System.Reflection.PropertyInfo pInfo in names)
            {
                table.Columns.Add(pInfo.Name, pInfo.PropertyType);
            }

           
            dataGridView1.DataSource = table;
            FixTableOrder();
           

        }
        /// <summary>
        /// arranges the columns nicely
        /// </summary>
        private void FixTableOrder()
        {
            dataGridView1.Columns["Relation"].DisplayIndex = 1;
        }
        string sfilename = "";

        /// <summary>
        /// changes the title
        /// </summary>
        public string Filename
        {
            get { return sfilename; }
            set { sfilename = value; 
                this.Text = "Inspect " + sfilename;}
        }

        /// <summary>
        /// save the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = "xml";
            if (save.ShowDialog() == DialogResult.OK)
            {
                Filename = save.FileName;
                Save(Filename);
               
            }
        }
        /// <summary>
        /// saves the rule table called by Save and Save As
        /// </summary>
        /// <param name="sFile"></param>
        private void Save(string sFile)
        {
            DataTable table = (DataTable)dataGridView1.DataSource;
            DataSet ds = new DataSet();
            if (table.DataSet == null)
            {
                ds.Tables.Add(table);
            }
            else
            {
                // no need to add, already part of dataset
                ds = table.DataSet;
            }



            ds.WriteXml(sFile, XmlWriteMode.WriteSchema);
            ds = null;
            toolStripStatusLabel1.Text = sFile + " saved";
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                ds.ReadXml(open.FileName);
                Filename = open.FileName;
                dataGridView1.DataSource = ds.Tables[0];
                ds = null;
                FixTableOrder();
                toolStripStatusLabel1.Text = "";
            }
        }




        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Filename != "")
            {
                Save(Filename);
            }
        }
    }
}