using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace JAutoQueue
{
    public partial class donate : Form
    {
        public int second = 10;
        public donate()
        {
            InitializeComponent();
        }

        private void donate_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = "http://www.mpgh.net/forum/customavatars/avatar1504134_8.gif";
            pictureBox2.ImageLocation = "https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=B7RYFKQS3KH68");
            MessageBox.Show("Thanks for support us, you will reciev after the donation a key and instruction to your PayPal email.");
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            second -= 1;
            label1.Text = ("Popup auto close in "+ second +" second(s).");
            if (second == 0)
            {
                Close();
            }
        }
    }
}
