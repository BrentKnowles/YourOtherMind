using System;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
namespace CoreUtilities
{
	/// January 15 2009 - the actual generic functiosn ahve been moved to their own project in App2010 framework
	/// since they should not reference any application sepcific code
	
	/// <summary>
	/// For adding dropdowns to fields in a propertygrid that are based
	/// on the ClassSubtypes
	/// Just override the Filename
	///         [TypeConverter(typeof(StatusTypeListConverter))]
	///    public string StatusType
	///
	/// </summary>
	public class GenericTypeListConverter : StringConverter
	{
		
		protected virtual List<string> Strings
		{
			get { return new List<string>(); }
		}
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;//return base.GetStandardValuesSupported(context);
		}
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			Strings.Sort ();
//			string[] list = ClassSubtypes.LoadUserInfo(FileName).Subtypes;
//			Array.Sort(list);
			return new StandardValuesCollection(Strings);
		}
		
	} // Listconverter
	
}
