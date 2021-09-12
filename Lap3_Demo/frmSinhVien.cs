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
    public partial class frmSinhVien : Form
    {
        QuanLySinhVien qlsv;
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien();
            bool gt = true;
            List<string> cn = new List<string>();
            sv.MaSo = mtxtMaSo.Text;
            sv.HoTen = txtHoTen.Text;
            sv.NgaySinh = dtpNgaySinh.Value;
            sv.DiaChi = txtDiaChi.Text;
            sv.Lop = cbbLop.Text;
            sv.Hinh = txtHinh.Text;
            if (rdbNu.Checked)
                gt = false;
            sv.GioiTinh = gt;
            for (int i = 0; i < clbChuyenNganh.Items.Count; i++)
            {
                if (clbChuyenNganh.GetItemChecked(i))
                    cn.Add(clbChuyenNganh.Items[i].ToString());

            }
            sv.ChuyenNganh = cn;
            return sv;
        }
        

        private SinhVien GetSinhVienLV(ListViewItem lvitem)
        {
            SinhVien sv = new SinhVien();
            sv.MaSo = lvitem.SubItems[0].Text;
            sv.HoTen = lvitem.SubItems[1].Text;
            sv.NgaySinh = DateTime.Parse(lvitem.SubItems[2].Text);
            sv.DiaChi = lvitem.SubItems[3].Text;
            sv.Lop = lvitem.SubItems[4].Text;
            sv.GioiTinh = false;
            if (lvitem.SubItems[5].Text == "Nam")
                sv.GioiTinh = true;

            List<string> cn = new List<string>();
            string[] s = lvitem.SubItems[6].Text.Split(',');
            foreach (String t in s)
            {
                cn.Add(t);
            }
            sv.ChuyenNganh = cn;
            sv.Hinh = lvitem.SubItems[7].Text;
            return sv;
        }

    //Thiết lập các thông tin lên controls sinh viên
    private void ThietLapThongTin(SinhVien sv)
     {
            this.mtxtMaSo.Text = sv.MaSo;
            this.txtHoTen.Text = sv.HoTen;
            this.dtpNgaySinh.Value = sv.NgaySinh;
            this.txtDiaChi.Text = sv.DiaChi;
            this.cbbLop.Text = sv.Lop;
            this.txtHinh.Text = sv.Hinh;
            this.pbHinh.ImageLocation = sv.Hinh;
            if (sv.GioiTinh)
             this.rdbNam.Checked = true;
            else
             this.rdbNu.Checked = true;
 
         for (int i = 0; i< this.clbChuyenNganh.Items.Count; i++)
             this.clbChuyenNganh.SetItemChecked(i, false);
 
        foreach (string s in sv.ChuyenNganh)
         {
            for (int i = 0; i< this.clbChuyenNganh.Items.Count; i++)
                    if (s.CompareTo(this.clbChuyenNganh.Items[i]) == 0)
            this.clbChuyenNganh.SetItemChecked(i, true);
         }
      }

        private void ThemSV(SinhVien sv)
        {
            ListViewItem lvitem = new ListViewItem(sv.MaSo);
            lvitem.SubItems.Add(sv.HoTen);
            lvitem.SubItems.Add(sv.NgaySinh.ToShortDateString());
            lvitem.SubItems.Add(sv.DiaChi);
            lvitem.SubItems.Add(sv.Lop);
            string gt = "Nữ";
            if (sv.GioiTinh)
                gt = "Nam";
            lvitem.SubItems.Add(gt);
            string cn = "";
            foreach (string s in sv.ChuyenNganh)
                cn += s + ",";
            cn = cn.Substring(0, cn.Length - 1);
            lvitem.SubItems.Add(cn);
            lvitem.SubItems.Add(sv.Hinh);
            lvSinhVien.Items.Add(lvitem);
        }

        //Hiển thị các sinh viên trong qlsv lên ListView
        public void LoadListView()
        {

            this.lvSinhVien.Items.Clear();
            foreach (SinhVien sv in qlsv.danhSach)
            {
                ThemSV(sv);
            }
        }

   

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            qlsv.DocTuFile();
            LoadListView();
        }
        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
          int count = this.lvSinhVien.SelectedItems.Count;
          if (count > 0)
          {
            ListViewItem lvitem = this.lvSinhVien.SelectedItems[0];
            SinhVien sv = GetSinhVienLV(lvitem);
            ThietLapThongTin(sv);
          }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            SinhVien kq = qlsv.Tim(sv.MaSo, delegate (object obj1, object obj2)
                {
                 return (obj2 as SinhVien).MaSo.CompareTo(obj1.ToString());
                 });
             if (kq != null)
             MessageBox.Show("Mã sinh viên đã tồn tại!", "Lỗi thêm dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
             else
                 {
                this.qlsv.Them(sv);
                 this.LoadListView();
                 }
        }

        private int SoSanhTheoMa(object obj1, object obj2)
        {
         SinhVien sv = obj2 as SinhVien;
         return sv.MaSo.CompareTo(obj1);
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            {
                int count, i;
                ListViewItem lvitem;
                count = this.lvSinhVien.Items.Count - 1;
               
                     for (i = count; i >= 0; i--)
                     {
                     lvitem = this.lvSinhVien.Items[i];
                     if (lvitem.Checked)
                         qlsv.Xoa(lvitem.SubItems[0].Text, SoSanhTheoMa);
                     }
                 this.LoadListView();
                
                 this.btnMacDinh.PerformClick();
                 }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
             SinhVien sv = GetSinhVien();
             bool kqsua; 
             kqsua = qlsv.Sua(sv, sv.MaSo, SoSanhTheoMa);
             if (kqsua)
                {
                 this.LoadListView();
                }
        }

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            this.mtxtMaSo.Text = "";
            this.txtHoTen.Text = "";
            this.dtpNgaySinh.Value = DateTime.Now;
            this.txtDiaChi.Text = "";
            this.cbbLop.Text = this.cbbLop.Items[0].ToString();
            this.txtHinh.Text = "";
            this.pbHinh.ImageLocation = "";
            this.rdbNam.Checked = true;
             for (int i = 0; i < this.clbChuyenNganh.Items.Count - 1; i++)
                 this.clbChuyenNganh.SetItemChecked(i, false);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Picture"; // add photos
            dlg.Filter = "Image Files (JPEG, GIF, BMF, BMP, etc.)|"
                  + "*.jpg;*.jpeg;*.gif;*.bmp;"
                  + "*.tif;*.tiff;*.png|"
                  + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
                  + "GIF files (*.gif)|*.gif|"
                  + "BMP files (*.bmp)|*.bmp|"
                  + "TIFF files (*.tif;*.tiff)|*.tif;*tiff|"
                  + "PNG files (*.png)|*.png|"
                  + "All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.CurrentDirectory;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var fileName = dlg.FileName;
                txtHinh.Text = fileName;
                pbHinh.Load(fileName);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            SinhVien kq = qlsv.Tim(sv.MaSo, delegate (object obj1, object obj2)
            {
                return (obj2 as SinhVien).MaSo.CompareTo(obj1.ToString());
            });
            if (kq != null)
                MessageBox.Show("Mã sinh viên đã tồn tại!", "Lỗi thêm dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.qlsv.Them(sv);
                this.LoadListView();
            }
        }

        private void hhToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mởFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Picture"; // add photos
            dlg.Filter = "Image Files (JPEG, GIF, BMF, BMP, etc.)|"
                  + "*.jpg;*.jpeg;*.gif;*.bmp;"
                  + "*.tif;*.tiff;*.png|"
                  + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
                  + "GIF files (*.gif)|*.gif|"
                  + "BMP files (*.bmp)|*.bmp|"
                  + "TIFF files (*.tif;*.tiff)|*.tif;*tiff|"
                  + "PNG files (*.png)|*.png|"
                  + "All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.CurrentDirectory;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var fileName = dlg.FileName;
                txtHinh.Text = fileName;
                pbHinh.Load(fileName);
            }
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                int count, i;
                ListViewItem lvitem;
                count = this.lvSinhVien.Items.Count - 1;

                for (i = count; i >= 0; i--)
                {
                    lvitem = this.lvSinhVien.Items[i];
                    if (lvitem.Checked)
                        qlsv.Xoa(lvitem.SubItems[0].Text, SoSanhTheoMa);
                }
                this.LoadListView();

                this.btnMacDinh.PerformClick();
            
        }

        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

          
        }

        private void sắpXếpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTuyChon frmtuyChon = new frmTuyChon();
            frmtuyChon.AnTimKiem();
            frmtuyChon.ShowDialog();
            
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTuyChon frmtuyChon = new frmTuyChon();
            frmtuyChon.AnSapXep();
            frmtuyChon.ShowDialog();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvSinhVien.Font = new Font("Arial",15);
        }

        private void màuChữToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvSinhVien.ForeColor = Color.Red;
        }
    }
}
