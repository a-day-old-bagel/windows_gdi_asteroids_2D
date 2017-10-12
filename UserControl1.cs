using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrasparentTextBox
{
    public partial class TransparentTextBox: RichTextBox
    {
        public TransparentTextBox()
        {
            InitializeComponent();

            //[DllImport("coredll.dll")]
            

            //SetStyle(ControlStyles.SupportsTransparentBackColor |
            //     ControlStyles.OptimizedfloatBuffer |
            //     ControlStyles.AllPaintingInWmPaint |
            //     ControlStyles.ResizeRedraw |
            //     ControlStyles.UserPaint, true);
        }

        /*
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Asteroids2._0.Form1.HideCaret(Handle);
        }
        */
    }
}
