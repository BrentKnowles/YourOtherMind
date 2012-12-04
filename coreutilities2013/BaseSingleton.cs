using System;
using System.Collections.Generic;
using System.Text;

namespace CoreUtilities
{
	/// <summary>
	/// Base class to be used wen creating a singleton
	/// </summary>
	public class BaseSingleton
	{
		protected static volatile BaseSingleton instance;
		protected static object syncRoot = new Object();
		protected BaseSingleton() { }
		/*
		public virtual static BaseSingleton Instance
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
							instance = new BaseSingleton();
						}
					}
				}
				return instance;
			}
		}*/
		
	
	}
}
