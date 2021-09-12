using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lap3_Demo.File
{
    public delegate int SoSanh(object sv1, object sv2);
    public class QuanLySinhVien
    {
        public List<SinhVien> danhSach;
        public QuanLySinhVien()
        {
            danhSach = new List<SinhVien>();
        }

        public void Them(SinhVien sv)
        {
            danhSach.Add(sv);
        }

        public SinhVien this[int index]
        {
            get { return danhSach[index]; }
            set { danhSach[index] = value; }
        }

        public void Xoa(object obj, SoSanh ss)
        {
            int i = danhSach.Count - 1;
            for (; i >=0; i--)
            {
                if (ss(obj, this[i]) == 0)
                    this.danhSach.RemoveAt(i);
            }
        }

        public SinhVien Tim(object obj, SoSanh ss)
        {
            SinhVien svresult = null;
            foreach (SinhVien sinhVien in danhSach)
            {
                if (ss(obj, sinhVien) == 0)
                {
                    svresult = sinhVien;
                    break;
                }
            }
            return svresult;
        }

        public bool Sua(SinhVien svSua, object obj, SoSanh ss)
        {
            int i, count;
            bool kq = false;
            count = danhSach.Count - 1;
            for (i = 0; i < count; i++)
            {
                if (ss(obj, this[i]) == 0)
                {
                    this[i] = svSua;
                    kq = true;
                    break;
                }
            }
            return kq;
        }

        

        public void DocTuFile()
        {
            string fileName = "DanhSachSV.txt", t;
            string[] s;
            SinhVien sv;

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    while ((t = reader.ReadLine())!=null)
                    {
                        s = t.Split('*');
                        sv = new SinhVien();
                        sv.MaSo = s[0];
                        sv.HoTen = s[1];
                        sv.NgaySinh = DateTime.Parse(s[2]);
                        sv.DiaChi = s[3];
                        sv.Lop = s[4];
                        sv.Hinh = s[5];
                        sv.GioiTinh = true;
                        if (s[6] == "1")
                            sv.GioiTinh = true;
                        string[] cn = s[7].Split(',');
                        foreach (string c in cn)
                        {
                            sv.ChuyenNganh.Add(c);
                        }
                        this.Them(sv);
                    }

                }
            }  
        }

        public void SapXepTheoMaSo()
        {
            danhSach.Sort((x1, x2) => (x1.MaSo).CompareTo(x2.MaSo));
        }

        public void SapXepTheoHoTen()
        {
            danhSach.Sort((x1, x2) => (x1.HoTen).CompareTo(x2.HoTen));
        }

        public void SapXepTheoNgaySinh()
        {
            danhSach.Sort((x1, x2) => (x1.NgaySinh).CompareTo(x2.NgaySinh));
        }
    }
}
