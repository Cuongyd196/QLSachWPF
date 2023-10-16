using QLSachWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QLSachWPF.Models;
using System.Text.RegularExpressions;

namespace QLSachWPF
{
    /// <summary>
    /// Interaction logic for InsertWindow.xaml
    /// </summary>
    public partial class InsertWindow : Window
    {
        QLSachModel db = new QLSachModel();
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;
        // Mặc định id = -1 là thêm mới dữ liệu
        private int id = -1;
        public InsertWindow()
        {
            InitializeComponent();
            HienThiLoaiSach();
        }
        public InsertWindow(int idSelected) : this()
        {
            id = idSelected;
            HienThiLoaiSach();
            HienThiDuLieuSua();
        }
        public void HienThiDuLieuSua()
        {
            var sachSelected = db.Saches.Find(id);
            if (sachSelected != null)
            {
                txtMaSach.Text = sachSelected.MaSach.ToString();
                txtTenSach.Text = sachSelected.TenSach.ToString();
                txtDonGia.Text = sachSelected.DonGia.ToString();
                txtSoLuong.Text = sachSelected.SoLuong.ToString();
                LoaiSach loaiSach = db.LoaiSaches.Find(sachSelected.IDLoaiSach);
                cbbLoaiSach.SelectedItem= loaiSach;
            }
        }

        public void HienThiLoaiSach()
        {
            var listLoaiSach = db.LoaiSaches.ToList();
            cbbLoaiSach.ItemsSource= listLoaiSach;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Thêm mới dữ liệu     // Lấy dữ liệu từ các ô nhập và chọn tại InsertWindow
                string maSach = txtMaSach.Text;
                string tenSach = txtTenSach.Text;
                int donGia = int.Parse(txtDonGia.Text);
                int soLuong = int.Parse(txtSoLuong.Text);
                int loaiSach = int.Parse(cbbLoaiSach.SelectedValue.ToString());
                if (id == -1)
                {
                    var sach = new Sach();
                    sach.MaSach = maSach;
                    sach.TenSach = tenSach;
                    sach.DonGia = donGia;
                    sach.SoLuong = soLuong;
                    sach.IDLoaiSach = loaiSach;
                    sach.AnhBia = "";
                    db.Saches.Add(sach);
                    db.SaveChanges();
                    MessageBox.Show("Thêm dữ liệu thành công!", " Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    var sachEdit = db.Saches.Find(id);
                    sachEdit.MaSach = maSach;
                    sachEdit.TenSach = tenSach;
                    sachEdit.DonGia = donGia;
                    sachEdit.SoLuong = soLuong;
                    sachEdit.IDLoaiSach = loaiSach;
                    sachEdit.AnhBia = "";
                    db.SaveChanges();
                    MessageBox.Show("Sửa dữ liệu thành công!", " Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    this.Close();
                }
                DataChangedEventHandler handler = DataChanged;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra vui lòng kiểm tra lại!", " Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
