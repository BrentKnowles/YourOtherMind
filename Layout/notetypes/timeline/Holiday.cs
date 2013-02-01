using System;

namespace Timeline
{
	public struct holiday
	{
		public int nIcon;
		public string sText;
		public newGenericDate date;
		public string sCaption;

		public holiday(int Icon, string Text, newGenericDate _date, string Caption)
		{
			nIcon = Icon;
			sText =Text;
			date = _date;
			sCaption = Caption;
		}

	}
}

