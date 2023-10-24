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

namespace QLSachWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        QLSachModel db = new QLSachModel();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            string txtTenDNForm = txtTenDangNhap.Text;
            string txtMatKhauForm = txtMK.Password;
            var nguoiDung = db.NguoiDungs.SingleOrDefault(x =>x.TenNguoiDung.Equals(txtTenDNForm) && x.MatKhau.Equals(txtMatKhauForm));
            if (nguoiDung == null)
            {
                MessageBox.Show("Tên tài người dùng hoặc mật khẩu không chính xác", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int id = nguoiDung.IDNguoiDung;
                MainWindow mainWindow = new MainWindow(id);
                mainWindow.Show();
                this.Close();

            }
        }
    }
}
