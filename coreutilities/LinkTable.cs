using System;
using System.Data;

namespace CoreUtilities.Links
{
	/// <summary>
	/// This is my slow attempt to replace the LinkTable system on StickIt pages.
	/// WHY
	/// 1. The current system still fails and I do not know why
	/// 2. Putting it as a DataTable on a StickIt allows me to debug issues.
	/// 
	/// OTHER CHANGES
	/// 1. I'm removing the double redraw on groupems
	/// </summary>
	public class LinkTable
	{
		public const string STICKY_TABLE ="_linktable_";
		DataTable Table = null;
		public const int KEY = 0;
		public const int TITLE = 1;
		public const int LINKID = 2;
		public const int IDFORTHIS = 3;
		public const int ISPICTURE = 4;
		public const int GROUP = 5;
		public const int LINKTYPE = 6;
		public const int FILEEXISTS = 7;
		public const int EXTRA = 8;
		public const int EXTRA2 = 9;




		/// <summary>
		/// returns a table with default columns put in
		/// </summary>
		/// <returns></returns>
		public DataTable BuildNewTable()
		{
			DataTable table = new DataTable();
			DataColumn key = table.Columns.Add("key",typeof(int));
			table.PrimaryKey = new DataColumn[] { key };
			key.AutoIncrement = true;
			key.AutoIncrementSeed = 100;
			key.AutoIncrementStep = 5;
			table.Columns.Add("title"); // record.sText,
			table.Columns.Add("linkid"); // record.sFileName,
			table.Columns.Add("idforthisgroup"); // record.sSource,
			table.Columns.Add("ispicture"); //1 means it is a picture // record.nBookmarkKey,
			table.Columns.Add("group"); //record.sExtra,
			table.Columns.Add("linktype"); // i.e, FILE, which I think they all are //record.linkType.ToString()
			table.Columns.Add("fileexists", typeof(bool));
			table.Columns.Add("extra"); // i.e, FILE, which I think they all are //record.linkType.ToString()
			table.Columns.Add("extra2"); // i.e, FILE, which I think they all are //record.linkType.ToString()
			return table;
			
		}
		
		public int Count()
		{
			return Table.Rows.Count;
		}

		/// <summary>
		/// Not a copy.
		/// The real deal.
		/// Linked to this from LayoutDatabase CreateLinkTableIfNecessary
		/// </summary>
		/// <param name="table"></param>
		public void SetTable(DataTable table)
		{
			Table = table;
		}
		/// <summary>
		/// called from the GROUP EM
		/// 
		/// Called one by one from the Convert method in Sticky
		/// </summary>
		public void Convert(LinkTableRecord record)
		{
			Add(record);
		}
		/// <summary>
		/// Finds the current record in the database and updates it with the info in here.
		/// </summary>
		/// <param name="record"></param>
		public void Edit(LinkTableRecord record)
		{
			if (null == Table)
			{
				throw new Exception("Your data in StickyLinkTable is null. That would be bad.");
			}
			DataRow EditRow = Table.Rows.Find(record.sKey);
			if (null != EditRow)
			{
				
				
				EditRow[TITLE] = record.sText;
				EditRow[LINKID] = record.sFileName;
				EditRow[IDFORTHIS] = record.sSource;
				EditRow[ISPICTURE] = record.nBookmarkKey;
				EditRow[GROUP] = record.sExtra;
				EditRow[LINKTYPE] = record.linkType.ToString();
				EditRow[FILEEXISTS] = record.bStatus;
				/*
                EditRow.BeginEdit();
                BuildRow(record).ItemArray.CopyTo(EditRow.ItemArray, 0);
                EditRow.EndEdit();*/
				// EditRow = BuildRow(record); // replace with a new record
			}
		}
		/// <summary>
		/// returns an array of LinkITable records, the contents of the datatable
		/// </summary>
		/// <returns></returns>
		public LinkTableRecord[] GetRecords()
		{
			LinkTableRecord[] records = new LinkTableRecord[Table.Rows.Count];
			for (int i = 0; i < Table.Rows.Count; i++)
			{
				records[i] = RowToRecord(Table.Rows[i]);
			}
			return records;
		}
		/// <summary>
		/// Finds and returns the row matching, nULL otherwise
		/// Used by Edit and Deelete
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		private DataRow FindRow(LinkTableRecord record)
		{
			DataRow FindThisRow = null;
			/*
            foreach (DataRow row in Table.Rows)
            {
                if (row[TITLE].ToString() == record.sText &&
                    row[LINKID].ToString() == record.sFileName &&
                    row[IDFORTHIS].ToString() == record.sSource &&
                    row[ISPICTURE].ToString() == record.nBookmarkKey.ToString() &&
                    ((row[GROUP].ToString() == record.sExtra) || (row[GROUP].GetType() == typeof(DBNull) && record.sExtra == null)) &&
                    row[LINKTYPE].ToString() == record.linkType.ToString()
                    )
                {
                    FindThisRow = row;
                    break;
                }

            }*/
			FindThisRow = Table.Rows.Find(record.sKey);
			//FindThisRow[GROUP] = "boo!";
			return FindThisRow;
		}
		
		/// <summary>
		/// Will look for a match for this record in the DataTable and remove
		/// the sucker.
		/// </summary>
		/// <param name="record"></param>
		/// <returns>True if found somethign to delete</returns>
		public bool Delete(LinkTableRecord record)
		{
			if (null == Table)
			{
				throw new Exception("Your data in StickyLinkTable is null. That would be bad.");
			}
			
			DataRow DeleteThisRow = FindRow(record);
			
			if (null != DeleteThisRow)
			{
				Table.Rows.Remove(DeleteThisRow);
				return true;
			}
			return false;
			
		}
		/// <summary>
		/// Used in an Add or Edit operation.
		/// Build the row and then either add it or replace it
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		private DataRow BuildRow(LinkTableRecord record)
		{
			
			// we let the DataRow decide on the required KEY
			DataRow row = Table.NewRow();
			row[TITLE] = record.sText;
			row[LINKID] = record.sFileName;
			row[IDFORTHIS] = record.sSource;
			row[ISPICTURE] = record.nBookmarkKey;
			row[GROUP] = record.sExtra;
			row[LINKTYPE] = record.linkType.ToString();
			row[FILEEXISTS] = record.bStatus;
			row [EXTRA] = record.ExtraField;
			return row;
			
			
		}
		/// <summary>
		/// takes the row and returns a rowtorecord
		/// used for when getting the data out of the table
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		private LinkTableRecord RowToRecord (DataRow row)
		{
			LinkTableRecord record = new LinkTableRecord ();
			try {
			
				record.sKey = row [KEY].ToString ();
				record.sText = row [TITLE].ToString (); 
				record.sFileName = row [LINKID].ToString ();
				record.sSource = row [IDFORTHIS].ToString ();
				int isPicture = 0;
				Int32.TryParse (row [ISPICTURE].ToString (), out isPicture);

				record.nBookmarkKey = isPicture;
				record.sExtra = row [GROUP].ToString ();
				record.linkType = (LinkType)Enum.Parse (typeof(LinkType), row [LINKTYPE].ToString ());
				record.bStatus = (bool)row [FILEEXISTS];
				record.ExtraField = row[EXTRA].ToString();
			} catch (Exception ex) {
				NewMessage.Show (ex.ToString());
			}
			return record;
		}
		/// <summary>
		/// Uses the data stored in the linktable record to fill in a NEW ROW in a datatable
		/// </summary>
		/// <param name="record"></param>
		public LinkTableRecord Add(LinkTableRecord record)
		{
			if (null == Table)
			{
				throw new Exception("Your data in StickyLinkTable is null. That would be bad.");
				
			}
			
			
			/*
            record.sText = sTitle;
            // Jan 2010: If you ahve a title of text%link the link will be the preview used
            record.sFileName = sLinkURL;
            record.sSource = sSource;
            record.nBookmarkKey = nType;
            //record.sExtra is USED FOR GROUPS, we cannot use it
            record.linkType = LinkType.FILE;  // FILE = Stretch POPUP = Center
            */
			
			/* Table.Rows.Add(new object[6] {
                record.sText,
                record.sFileName,
                record.sSource,
                record.nBookmarkKey,
                record.sExtra,
                record.linkType.ToString()

            });*/
			DataRow row = BuildRow(record);
			Table.Rows.Add(row);
			record.sKey = row[KEY].ToString();
			return record;
		}
	}
}

