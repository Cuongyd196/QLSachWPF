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

namespace QLSachWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QLSachModel db = new QLSachModel();
        public MainWindow()
        {
            InitializeComponent();
            getData();
        }
        public void getData()
        {
            dgSach.ItemsSource = db.Saches.ToList();
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
            this.Close();
        }
        private void btnTaiLai_Click(object sender, RoutedEventArgs e)
        {
            dgSach.ItemsSource = null;
            dgSach.ItemsSource = db.Saches.ToList();
        }


    }
}
