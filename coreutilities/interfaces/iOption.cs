using System;

namespace CoreUtilities
{
	/// <summary>
	/// Any class that is to appear in a Options panel uses this interface
	/// </summary>
	public interface iOption
	{

		/// <summary>
		/// Displays the widget in the Options panel
		/// </summary>
		void DisplayWidget();
	}
}

