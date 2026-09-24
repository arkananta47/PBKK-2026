using System.Windows;
using System.Windows.Controls;

namespace StudentRegistrationApp
{
    public partial class MainWindow : Window
    {
        private readonly AppContext db;

        private Mahasiswa? mahasiswaTerpilih;

        public MainWindow()
        {
            InitializeComponent();

            db = new AppContext();

            cmbProdi.SelectedIndex = 0;

            LoadData();
        }

        private void LoadData()
        {
            listMahasiswa.ItemsSource = null;
            listMahasiswa.ItemsSource = db.GetAllMahasiswa();

            txtJumlah.Text =
                $"Jumlah mahasiswa: {listMahasiswa.Items.Count}";
        }

        private bool ValidasiInput()
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text))
            {
                MessageBox.Show(
                    "Nama mahasiswa wajib diisi.",
                    "Data Tidak Lengkap",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                txtNama.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNRP.Text))
            {
                MessageBox.Show(
                    "NRP mahasiswa wajib diisi.",
                    "Data Tidak Lengkap",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                txtNRP.Focus();

                return false;
            }

            if (cmbProdi.SelectedItem == null)
            {
                MessageBox.Show(
                    "Program studi wajib dipilih.",
                    "Data Tidak Lengkap",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            return true;
        }

        private string GetGender()
        {
            return rbLaki.IsChecked == true
                ? "Laki-laki"
                : "Perempuan";
        }

        // CREATE
        private void BtnSimpan_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!ValidasiInput())
                return;

            string prodi =
                ((ComboBoxItem)cmbProdi.SelectedItem)
                .Content
                .ToString()!;

            Mahasiswa mahasiswa = new Mahasiswa
            {
                Nama = txtNama.Text.Trim(),
                NRP = txtNRP.Text.Trim(),
                Prodi = prodi,
                Gender = GetGender()
            };

            bool berhasil = db.AddMahasiswa(mahasiswa);

            if (!berhasil)
            {
                MessageBox.Show(
                    "NRP sudah terdaftar.",
                    "Gagal Menyimpan",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBox.Show(
                "Data mahasiswa berhasil disimpan.",
                "Berhasil",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            LoadData();

            ClearForm();
        }

        // READ
        private void ListMahasiswa_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (listMahasiswa.SelectedItem is not Mahasiswa mahasiswa)
                return;

            mahasiswaTerpilih = mahasiswa;

            txtNama.Text = mahasiswa.Nama;
            txtNRP.Text = mahasiswa.NRP;

            for (int i = 0; i < cmbProdi.Items.Count; i++)
            {
                ComboBoxItem item =
                    (ComboBoxItem)cmbProdi.Items[i];

                if (item.Content.ToString() == mahasiswa.Prodi)
                {
                    cmbProdi.SelectedIndex = i;
                    break;
                }
            }

            if (mahasiswa.Gender == "Laki-laki")
            {
                rbLaki.IsChecked = true;
            }
            else
            {
                rbPerempuan.IsChecked = true;
            }
        }

        // UPDATE
        private void BtnUpdate_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (mahasiswaTerpilih == null)
            {
                MessageBox.Show(
                    "Pilih data mahasiswa yang ingin diperbarui.",
                    "Data Belum Dipilih",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!ValidasiInput())
                return;

            string prodi =
                ((ComboBoxItem)cmbProdi.SelectedItem)
                .Content
                .ToString()!;

            mahasiswaTerpilih.Nama =
                txtNama.Text.Trim();

            mahasiswaTerpilih.NRP =
                txtNRP.Text.Trim();

            mahasiswaTerpilih.Prodi =
                prodi;

            mahasiswaTerpilih.Gender =
                GetGender();

            bool berhasil =
                db.UpdateMahasiswa(mahasiswaTerpilih);

            if (!berhasil)
            {
                MessageBox.Show(
                    "NRP tersebut sudah digunakan oleh mahasiswa lain.",
                    "Gagal Update",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBox.Show(
                "Data mahasiswa berhasil diperbarui.",
                "Berhasil",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            LoadData();

            ClearForm();
        }

        // DELETE
        private void BtnHapus_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (mahasiswaTerpilih == null)
            {
                MessageBox.Show(
                    "Pilih data mahasiswa yang ingin dihapus.",
                    "Data Belum Dipilih",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult result =
                MessageBox.Show(
                    $"Apakah Anda yakin ingin menghapus data {mahasiswaTerpilih.Nama}?",
                    "Konfirmasi Hapus",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            bool berhasil =
                db.DeleteMahasiswa(mahasiswaTerpilih.Id);

            if (berhasil)
            {
                MessageBox.Show(
                    "Data mahasiswa berhasil dihapus.",
                    "Berhasil",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LoadData();

                ClearForm();
            }
        }

        // CLEAR
        private void BtnClear_Click(
            object sender,
            RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtNama.Clear();
            txtNRP.Clear();

            cmbProdi.SelectedIndex = 0;

            rbLaki.IsChecked = true;

            mahasiswaTerpilih = null;

            listMahasiswa.SelectedItem = null;

            txtNama.Focus();
        }
    }
}