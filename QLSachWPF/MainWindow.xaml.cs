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
using System.Windows.Navigation;
using System.Windows.Shapes;
using QLSachWPF.Models;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Diagnostics;
using OfficeOpenXml.Style;

namespace QLSachWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int idNguoiDung = -1;
        QLSachModel db = new QLSachModel();
        public MainWindow(int id)
        {
            InitializeComponent();
            checkVaitro(id);
            getData();
            HienThiLoaiSach();
            
        }
        public void checkVaitro(int id)
        {
            idNguoiDung = id;
            var nguoidungDangNhap = db.NguoiDungs.Find(id);
            if (nguoidungDangNhap.VaiTro == "NGUOIDUNG")
            {
                btnSua.IsEnabled = false;
                btnThem.IsEnabled = false;
                btnXoa.IsEnabled = false;
            }
        }
        public void getData()
        {
            QLSachModel db = new QLSachModel();

            dgSach.ItemsSource = db.Saches.ToList();
        }
        public void HienThiLoaiSach()
        {
            var listLoaiSach = db.LoaiSaches.ToList();
            cbbLoaiSach.ItemsSource = listLoaiSach;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            InsertWindow insertWindow = new InsertWindow();
            insertWindow.DataChanged += Window_DataChanged;
            insertWindow.ShowDialog();
        }
        public ObservableCollection<Sach> SachCollection { get; set; }

        private void Window_DataChanged(object sender, EventArgs e)
        {
            getData();
        }
        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int countSelecteditem = 0;
                countSelecteditem = dgSach.SelectedItems.Count;
                if (countSelecteditem == 0)
                {
                    MessageBox.Show(" Bạn chưa chọn item cần sửa");
                }
                else
                {
                    Sach sachSelected = (Sach)dgSach.SelectedItem;
                    int idSachSelected = sachSelected.IDSach;
                    InsertWindow insertWindow = new InsertWindow(idSachSelected);
                    insertWindow.DataChanged += Window_DataChanged;
                    insertWindow.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int countSelecteditem = 0;
                countSelecteditem = dgSach.SelectedItems.Count;
                if (countSelecteditem == 0)
                {
                    MessageBox.Show(" Bạn chưa chọn item cần xoá");
                }
                else
                {
                    var Result = MessageBox.Show("Bạn có chắc chắn xoá dữ liệu?", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (Result == MessageBoxResult.Yes)
                    {
                        Sach sachSelected = (Sach)dgSach.SelectedItem;
                        int idSachSelected = sachSelected.IDSach;
                        Sach sach = db.Saches.Find(idSachSelected);
                        db.Saches.Remove(sach);
                        db.SaveChanges();
                        MessageBox.Show(" Xóa thành công");
                        getData();
                    }
                    else if (Result == MessageBoxResult.No)
                    {
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        private void btnTaiLai_Click(object sender, RoutedEventArgs e)
        {
            dgSach.ItemsSource = null;
            dgSach.ItemsSource = db.Saches.ToList();
        }

        private void cbbLoaiSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idLoaiSach = -1;
            idLoaiSach = int.Parse(cbbLoaiSach.SelectedValue.ToString());
            var dataSearch = db.Saches.Where(x => x.IDLoaiSach == idLoaiSach).ToList();
            dgSach.ItemsSource = dataSearch;
        }

        private void btnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Thử lấy danh sách sách từ cơ sở dữ liệu
                var listSach = db.Saches.ToList();

                // Kiểm tra xem danh sách có dữ liệu không
                if (listSach.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Sử dụng thư viện EPPlus để tạo tệp Excel
                using (var package = new ExcelPackage())
                {
                    // Tạo một trang tính trong tệp Excel và thiết lập các thuộc tính
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sach");
                    worksheet.Workbook.Properties.Author = "FICT";
                    worksheet.Workbook.Properties.Title = "DuLieuSach";
                    worksheet.Cells.Style.Font.Size = 12;
                    worksheet.Cells.Style.Font.Name = "Times New Roman";

                    // Tạo header cho trang tính
                    string[] arr_col = { "TT", "Tên sách", "Loại sách", "Số lượng", "Đơn giá" };
                    int colIndex = 1, rowIndex = 1;
                    foreach (string columnName in arr_col)
                    {
                        worksheet.Cells[rowIndex, colIndex].Value = columnName;
                        colIndex++;
                    }

                    // Căn giữa dữ liệu trong tất cả các ô trên trang tính
                    worksheet.Cells[1, 1, listSach.Count + 1, arr_col.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, listSach.Count + 1, arr_col.Length].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Đổ dữ liệu sách vào trang tính
                    foreach (var item in listSach)
                    {
                        colIndex = 1;
                        rowIndex++;
                        worksheet.Cells.Style.Font.Bold = false;
                        worksheet.Cells.Style.WrapText = true;
                        worksheet.Cells[rowIndex, colIndex++].Value = rowIndex - 1;
                        worksheet.Cells[rowIndex, colIndex++].Value = item.TenSach;
                        worksheet.Cells[rowIndex, colIndex++].Value = item.LoaiSach.TenLoaiSach;
                        worksheet.Cells[rowIndex, colIndex++].Value = item.SoLuong;
                        worksheet.Cells[rowIndex, colIndex++].Value = item.DonGia;
                    }

                    // Đặt chiều rộng của từng cột
                    for (int indexCol = 1; indexCol <= arr_col.Length; indexCol++)
                    {
                        if (indexCol == 1) { worksheet.Column(indexCol).Width = 6; }       // Cột 1
                        if (indexCol == 2) { worksheet.Column(indexCol).Width = 40; }      // Cột 2
                        if (indexCol == 3) { worksheet.Column(indexCol).Width = 25; }      // Cột 3
                        if (indexCol == 4) { worksheet.Column(indexCol).Width = 15; }     // Cột 4
                        if (indexCol == 5) { worksheet.Column(indexCol).Width = 15; }      // Cột 5
                        worksheet.Cells[1, 1, 1, indexCol].Style.Font.Bold = true; // Đặt kiểu chữ đậm cho dòng tiêu đề
                    }

                    // Yêu cầu người dùng chỉ định nơi lưu trữ tệp Excel
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Title = "Xuất dữ liệu";
                    dialog.Filter = "Excel Files|*.xlsx";

                    // Nếu người dùng đã chọn vị trí lưu trữ và đặt tên cho tệp Excel
                    if (dialog.ShowDialog() == true)
                    {
                        string filePath = dialog.FileName;

                        // Kiểm tra xem tên tệp không rỗng
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            // Chuyển dữ liệu tệp Excel thành mảng byte và lưu vào tệp
                            Byte[] bin = package.GetAsByteArray();
                            File.WriteAllBytes(filePath, bin);

                            // Hiển thị thông báo thành công
                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Mở tệp Excel sau khi xuất
                            Process.Start(filePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi và hiển thị thông báo lỗi
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
