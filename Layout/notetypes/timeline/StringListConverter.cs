using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
namespace Layout
{
	// from: http://stackoverflow.com/questions/6307006/how-can-i-use-a-winforms-propertygrid-to-edit-a-list-of-strings-c
	public class StringListConverter : TypeConverter
	{
		// Overrides the ConvertTo method of TypeConverter.
		public override object ConvertTo(ITypeDescriptorContext context,
		                                 CultureInfo culture, object value, Type destinationType)
		{
			List<String> v = value as List<String>;
			if (destinationType == typeof(string))
			{
				return String.Join(",", v.ToArray()); 
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

