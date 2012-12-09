using System;

namespace Layout
{
	/// <summary>
	/// Wiill hold info about NoteTypes, About the active layoutpanel
	/// </summary>
	public class LayoutDetails
	{
		#region variables
		protected static volatile LayoutDetails instance;
		protected static object syncRoot = new Object();
		

#endregion;
		public LayoutDetails ()
		{
			TypeList = new Type[2] {typeof(NoteDataXML), typeof(NoteDataXML_RichText)};
		}

		public Type[] TypeList = null;

		public Type[] ListOfTypesToStoreInXML ()
		{
			return TypeList;
		}

		public static LayoutDetails Instance
		{
			get
			{
				if (null == instance)
				{
					// only one instance is created and when needed
					lock (syncRoot)
					{
						if (null == instance)
						{
							instance = new LayoutDetails();
						}
					}
				}
				return (LayoutDetails)instance;
			}
		}
	}
}




