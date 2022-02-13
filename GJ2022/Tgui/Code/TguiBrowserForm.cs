using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GJ2022.Tgui.Code
{
    public partial class TguiBrowserForm : Form
    {
        public TguiBrowserForm(string pageDir)
        {
            InitializeComponent();
            EmbeddedBrowser.Navigate(pageDir);
        }
    }
}
