using System;

namespace Timeline
{
	/// <summary>
	/// used for storing hotspot clickable regions each paint cycle
	/// </summary>
	struct SmartPoint
	{
		public int x;
		public int y;
		public string s;
		public SmartPoint(int x, int y, string s)
		{
			this.x = x;
			this.y = y;
			this.s = s;
		}
	}
}

