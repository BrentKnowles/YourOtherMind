// Structures.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
//#define USE_HASH_TABLE

using System;
using System.Collections;

namespace DiffEngine
{
	public interface IDiffList
	{
		int Count();
		IComparable GetByIndex(int index);
	}

	internal enum DiffStatus 
	{
		Matched = 1,
		NoMatch = -1,
		Unknown = -2
			
	}

	internal class DiffState
	{
		private const int BAD_INDEX = -1;
		private int _startIndex;
		private int _length;

		public int StartIndex {get{return _startIndex;}}
		public int EndIndex {get{return ((_startIndex + _length) - 1);}}
		public int Length 
		{
			get
			{
				int len;
				if (_length > 0)
				{
					len = _length;
				}
				else
				{
					if (_length == 0)
					{
						len = 1;
					}
					else
					{
						len = 0;
					}
				}
				return len;
			}
		}
		public DiffStatus Status 
		{
			get
			{
				DiffStatus stat;
				if (_length > 0)
				{
					stat = DiffStatus.Matched; 
				}
				else
				{
					switch (_length)
					{
						case -1:
							stat = DiffStatus.NoMatch;
							break;
						default:
							System.Diagnostics.Debug.Assert(_length == -2,"Invalid status: _length < -2");
							stat = DiffStatus.Unknown;
							break;
					}
				}
				return stat;
			}
		}

		public DiffState()
		{
			SetToUnkown();
		}

		protected void SetToUnkown()
		{
			_startIndex = BAD_INDEX;
			_length = (int)DiffStatus.Unknown;
		}

		public void SetMatch(int start, int length)
		{
			System.Diagnostics.Debug.Assert(length > 0,"Length must be greater than zero");
			System.Diagnostics.Debug.Assert(start >= 0,"Start must be greater than or equal to zero");
			_startIndex = start;
			_length = length;
		}

		public void SetNoMatch()
		{
			_startIndex = BAD_INDEX;
			_length = (int)DiffStatus.NoMatch;
		}


		public bool HasValidLength(int newStart, int newEnd, int maxPossibleDestLength)
		{
			if (_length > 0) //have unlocked match
			{
				if ((maxPossibleDestLength < _length)||
					((_startIndex < newStart)||(EndIndex > newEnd)))
				{
					SetToUnkown();
				}
			}
			return (_length != (int)DiffStatus.Unknown);
		}
	}

	internal class DiffStateList
	{
#if USE_HASH_TABLE
		private Hashtable _table;
#else
		private DiffState[] _array;
#endif
		public DiffStateList(int destCount)
		{
#if USE_HASH_TABLE
			_table = new Hashtable(Math.Max(9,destCount/10));
#else
			_array = new DiffState[destCount];
#endif
		}

		public DiffState GetByIndex(int index)
		{
#if USE_HASH_TABLE
			DiffState retval = (DiffState)_table[index];
			if (retval == null)
			{
				retval = new DiffState();
				_table.Add(index,retval);
			}
#else
			DiffState retval = _array[index];
			if (retval == null)
			{
				retval = new DiffState();
				_array[index] = retval;
			}
#endif
			return retval;
		}
	}


	public enum DiffResultSpanStatus
	{
		NoChange,
		Replace,
		DeleteSource,
		AddDestination
	}

	public class DiffResultSpan : IComparable
	{
		private const int BAD_INDEX = -1;
		private int _destIndex;
		private int _sourceIndex;
		private int _length;
		private DiffResultSpanStatus _status;

		public int DestIndex {get{return _destIndex;}}
		public int SourceIndex {get{return _sourceIndex;}}
		public int Length {get{return _length;}}
		public DiffResultSpanStatus Status {get{return _status;}}
		
		protected DiffResultSpan(
			DiffResultSpanStatus status,
			int destIndex,
			int sourceIndex,
			int length)
		{
			_status = status;
			_destIndex = destIndex;
			_sourceIndex = sourceIndex;
			_length = length;
		}

		public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
		{
			return new DiffResultSpan(DiffResultSpanStatus.NoChange,destIndex,sourceIndex,length); 
		}

		public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
		{
			return new DiffResultSpan(DiffResultSpanStatus.Replace,destIndex,sourceIndex,length); 
		}

		public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
		{
			return new DiffResultSpan(DiffResultSpanStatus.DeleteSource,BAD_INDEX,sourceIndex,length); 
		}

		public static DiffResultSpan CreateAddDestination(int destIndex, int length)
		{
			return new DiffResultSpan(DiffResultSpanStatus.AddDestination,destIndex,BAD_INDEX,length); 
		}

		public void AddLength(int i)
		{
			_length += i;
		}

		public override string ToString()
		{
			return string.Format("{0} (Dest: {1},Source: {2}) {3}",
				_status.ToString(),
				_destIndex.ToString(),
				_sourceIndex.ToString(),
				_length.ToString());
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			return _destIndex.CompareTo(((DiffResultSpan)obj)._destIndex);
		}

		#endregion
	}
}