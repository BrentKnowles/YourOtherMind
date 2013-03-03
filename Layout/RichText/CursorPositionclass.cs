using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
namespace RichBoxLinks
{
    /// <summary>
    /// http://www.codeproject.com/cs/miscctrl/RicherRichTextBox.asp
    /// for getting line position and such
    /// </summary>
    public class CursorPosition  // was modified by Internal but I wanted access to this outside of here
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int GetCaretPos(ref Point lpPoint);

        private static int GetCorrection(RichTextBox e, int index)
        {
            Point pt1 = Point.Empty;
            GetCaretPos(ref pt1);
            Point pt2 = e.GetPositionFromCharIndex(index);

            // ocotber 23 2008 - not sure what this function
            // was meant to do so added this code to catch null situations
            if (pt1 == null || pt2 == null)
            {
                return 0;
            }

            if (pt1 != pt2)
                return 1;
            else
                return 0;
        }

        public static int Line(RichTextBox e, int index)
        {
            int correction = 0;
            try
            {
                correction = GetCorrection(e, index);
            }
            catch (Exception)
            {
            }
            return e.GetLineFromCharIndex(index) - correction + 1;

            //  MessageBox.Show(ex.ToString());

            // return 0;
        }

        public static int Column(RichTextBox e, int index1)
        {
            int correction = GetCorrection(e, index1);
            Point p = e.GetPositionFromCharIndex(index1 - correction);

            if (p.X == 1)
                return 1;

            p.X = 0;
            int index2 = e.GetCharIndexFromPosition(p);

            int col = index1 - index2 + 1;

            return col;
        }





    } // - class linepost

    /*  private void richTextBox1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
       {
           if (e.Data.GetDataPresent(DataFormats.Text))
               e.Effect = DragDropEffects.Copy;
           else
               e.Effect = DragDropEffects.None;
       }
       private void richTextBox1_DragDrop(object sender,
       System.Windows.Forms.DragEventArgs e)
       {
           int i;
           String s;

           // Get start position to drop the text.
           i = richTextBox1.SelectionStart;
           s = richTextBox1.Text.Substring(i);
           richTextBox1.Text = richTextBox1.Text.Substring(0, i);

           // Drop the text on to the RichTextBox.
           richTextBox1.Text = richTextBox1.Text +
              e.Data.GetData(DataFormats.Text).ToString();
           richTextBox1.Text = richTextBox1.Text + s;
       }
*/
    /// <summary>
    /// Insert a given text as a link into the RichTextBox at the current insert position.
    /// </summary>
    /// <param name="text">Text to be inserted</param>
    /*public void InsertLink(string text)
    {
        InsertLink(text, this.SelectionStart);
    }

*/


    /// <summary>
    /// Insert a given text at a given position as a link. 
    /// </summary>
    /// <param name="text">Text to be inserted</param>
    /// <param name="position">Insert position</param>
    /*	public void InsertLink(string text, int position)
        {
            if (position < 0 || position > this.Text.Length)
                throw new ArgumentOutOfRangeException("position");

            this.SelectionStart = position;
            this.SelectedText = text;
            this.Select(position, text.Length);
            this.SetSelectionLink(true);
            this.Select(position + text.Length, 0);
        }*/

    /// <summary>
    /// Insert a given text at at the current input position as a link.
    /// The link text is followed by a hash (#) and the given hyperlink text, both of
    /// them invisible.
    /// When clicked on, the whole link text and hyperlink string are given in the
    /// LinkClickedEventArgs.
    /// </summary>
    /// <param name="text">Text to be inserted</param>
    /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
    /*	public void InsertLink(string text, string hyperlink)
        {
            InsertLink(text, hyperlink, this.SelectionStart);
        }*/

    /// <summary>
    /// Insert a given text at a given position as a link. The link text is followed by
    /// a hash (#) and the given hyperlink text, both of them invisible.
    /// When clicked on, the whole link text and hyperlink string are given in the
    /// LinkClickedEventArgs.
    /// </summary>
    /// <param name="text">Text to be inserted</param>
    /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
    /// <param name="position">Insert position</param>
    /*	public void InsertLink(string text, string hyperlink, int position)
        {
            if (position < 0 || position > this.Text.Length)
                throw new ArgumentOutOfRangeException("position");

            this.SelectionStart = position;
            this.SelectedRtf = @"{\rtf1\ansi "+text+@"\v #"+hyperlink+@"\v0}";
            this.Select(position, text.Length + hyperlink.Length + 1);
            this.SetSelectionLink(true);
            this.Select(position + text.Length + hyperlink.Length + 1, 0);
        }*/
}
