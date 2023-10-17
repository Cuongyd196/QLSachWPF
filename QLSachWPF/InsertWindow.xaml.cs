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
using Microsoft.Win32;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Drawing;
using Path = System.IO.Path;

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
            pathImage.Text = "";
            string pathNoImage = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Uploads/static/noimage.jpg";
            imageSach.Source = new BitmapImage(new Uri(pathNoImage));
        }
        public InsertWindow(int idSelected) : this()
        {
            id = idSelected;
            HienThiLoaiSach();
            HienThiDuLieuSua();
        }
        public void HienThiDuLieuSua()
        {
            string pathNoImage = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Uploads/static/noimage.jpg";
            var sachSelected = db.Saches.Find(id);
            if (sachSelected != null)
            {
                string pathImageSach = (sachSelected.AnhBia == "" || sachSelected.AnhBia == null) ? pathNoImage : sachSelected.AnhBia;
                imageSach.Source = new BitmapImage(new Uri(pathImageSach));
                pathImage.Text = sachSelected.AnhBia;
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

        public string HandleUploadImage()
        {
            string pathNoImage = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Uploads/static/noimage.jpg";
            // Xử lý upload ảnh
            // Lấy thời gian hiện tại dưới dạng timestamp
            string timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            // Lấy đường dẫn đến tệp ảnh từ nguồn (được giả định là bạn đã cung cấp pathImage.Text)
            string imageUpload = pathImage.Text;
            //Nếu không có ảnh upload khi thêm mới Return ảnh NoImage
            if (imageUpload == null || imageUpload == "") return pathNoImage;
            // Phân tách tên file và định dạng file
            string[] nameArray = imageUpload.Split('.');
            string imgTempName = nameArray[0];
            string extension = nameArray[1];
            // Tạo tên tệp mới với thời gian đính kèm
            string pathFileNew = imgTempName + "_" + timeStamp + "." + extension;
            // Xác định thư mục lưu trữ ảnh đã upload
            string uploadDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Uploads/Images";
            // Kiểm tra xem thư mục tồn tại chưa, nếu chưa thì tạo mới
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            // Tạo đường dẫn đến tệp ảnh trong thư mục Uploads
            string destinationPath = Path.Combine(uploadDirectory, Path.GetFileName(pathFileNew));
            // Xử lý lưu tệp ảnh từ nguồn đến đích
            File.Copy(imageUpload, destinationPath);
            // Trả về đường dẫn đến tệp ảnh đã lưu
            return destinationPath;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Thêm mới dữ liệu     
                // Lấy dữ liệu từ các ô nhập và chọn tại InsertWindow
                string maSach = txtMaSach.Text;
                string tenSach = txtTenSach.Text;
                int donGia = int.Parse(txtDonGia.Text);
                int soLuong = int.Parse(txtSoLuong.Text);
                int loaiSach = int.Parse(cbbLoaiSach.SelectedValue.ToString());
                if (id == -1)
                {
                    string pathAnhBia = "";
                    pathAnhBia = HandleUploadImage();
                    var sach = new Sach();
                    sach.MaSach = maSach;
                    sach.TenSach = tenSach;
                    sach.DonGia = donGia;
                    sach.SoLuong = soLuong;
                    sach.IDLoaiSach = loaiSach;
                    sach.AnhBia = pathAnhBia;
                    db.Saches.Add(sach);
                    db.SaveChanges();
                    MessageBox.Show("Thêm dữ liệu thành công!", " Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    string pathAnhBia = pathImage.Text;
                    var sachEdit = db.Saches.Find(id);
                    if (pathAnhBia != sachEdit.AnhBia)
                    {
                        pathAnhBia = HandleUploadImage();
                    }
                    sachEdit.MaSach = maSach;
                    sachEdit.TenSach = tenSach;
                    sachEdit.DonGia = donGia;
                    sachEdit.SoLuong = soLuong;
                    sachEdit.IDLoaiSach = loaiSach;
                    sachEdit.AnhBia = pathAnhBia;
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
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                MessageBox.Show("Có lỗi xảy ra vui lòng kiểm tra lại!", " Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files |*.jpg;*.png;*.bmp;*.gif;*.jpeg";
            if (dlg.ShowDialog() == true)
            {
                string selectedImagePath = dlg.FileName;
                pathImage.Text = selectedImagePath;
                // Display the selected image
                imageSach.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }

        //System.Diagnostics.Debug.WriteLine(imageUpload);
        //System.Diagnostics.Debug.WriteLine(imgTempName);
        //System.Diagnostics.Debug.WriteLine(extension);
        //System.Diagnostics.Debug.WriteLine(timeStamp);
    }
}
