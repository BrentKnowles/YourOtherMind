using System;
using System.Windows.Forms;

namespace Addin_Test2
{
	public class memoryForm :Form
	{
		public Label labelMin;
		public Label labelMax;
		public Label labelMemoryUse;
		public memoryForm ()
		{
			labelMin = new Label();
			labelMax = new Label();
			labelMemoryUse = new Label();

			labelMin.Parent = this;
			labelMax.Parent = this;
			labelMemoryUse.Parent = this;


			labelMax.Dock = DockStyle.Top;
			labelMin.Dock = DockStyle.Top;
			labelMemoryUse.Dock = DockStyle.Top;

		}
	}
}

