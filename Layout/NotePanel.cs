using System;

namespace Layout
{
	/// <summary>
	/// The basic Floating Note
	/// No text on it.
	/// 
	/// In version 2.0 I will test whether it is sufficient that the NotePanel can descend into more
	/// advanced objects but the underlying NOTEDATA remains the same (that is fields that are neeed downstream remain.
	/// 
	/// NOTE:
	///   * LayoutPanel holds an array of NoteData in memory [Any other note can obtain this NoteData by loading the file]
	///                                 --> TODO: Ideally no other note should be able to open the current file
	///                                           but must make a Copy of it first and grab info from that? 
	///                                           Or each Layout will save a backup copy of itself which others can use.
	///   * The FIRST note in NoteData is data about the LayoutPanel itself (??? not sure ???)
	///   * NotePanel holds a reference to its NoteData
	///   * NoteData is a BROKER(?) that pulls data from wherever Data is specified (specified by Whom? LayoutPanel?)
	///   * 
	/// </summary>
	public class NotePanel : NotePanelInterface
	{
		public NotePanel ()
		{
		}
	}
}

