using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lap3_Demo.File;

namespace Lap3_Demo
{
    public partial class frmTuyChon : Form
    {
        QuanLySinhVien qlsv;
        public frmTuyChon()
        {
            InitializeComponent();
        }

        public void AnTimKiem()
        {
            panel2.Enabled = false;
        }

        public void AnSapXep()
        {
            btnSapXep.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSapXep_Click(object sender, EventArgs e)
        {
            frmSinhVien frmsv = new frmSinhVien();
            if (rdbMaSV.Checked == true)
            {
                qlsv.SapXepTheoMaSo();
                
               
            }
        }


    }
}
