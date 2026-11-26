using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudentManager.Commands;
using StudentManager.Models;
using StudentManager.Services;

namespace StudentManager.ViewModels;

public class StudentViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _database = new();
    private Student? _selectedStudent;
    private string _nim = "", _nama = "", _prodi = "", _jenisKelamin = "", _gmail = "", _searchText = "";
    private int _totalStudents;
    private int _informatikaCount;
    private int _sistemInformasiCount;
    private int _lakiLakiCount;
    private int _perempuanCount;

    public ObservableCollection<Student> Students { get; } = new();

    public Student? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnPropertyChanged();

            if (value is not null)
            {
                Nim = value.Nim;
                Nama = value.Nama;
                Prodi = value.Prodi;
                JenisKelamin = value.JenisKelamin;
                Gmail = value.Gmail;
            }

            CommandManager.InvalidateRequerySuggested();
        }
    }

    public string Nim
    {
        get => _nim;
        set
        {
            _nim = value;
            OnPropertyChanged();
        }
    }

    public string Nama
    {
        get => _nama;
        set
        {
            _nama = value;
            OnPropertyChanged();
        }
    }

    public string Prodi
    {
        get => _prodi;
        set
        {
            _prodi = value;
            OnPropertyChanged();
        }
    }

    public string JenisKelamin
    {
        get => _jenisKelamin;
        set
        {
            _jenisKelamin = value;
            OnPropertyChanged();
        }
    }

    public string Gmail
    {
        get => _gmail;
        set
        {
            _gmail = value;
            OnPropertyChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            _ = LoadStudentsAsync();
        }
    }

    public int TotalStudents
    {
        get => _totalStudents;
        private set
        {
            _totalStudents = value;
            OnPropertyChanged();
        }
    }

    public int InformatikaCount
    {
        get => _informatikaCount;
        private set
        {
            _informatikaCount = value;
            OnPropertyChanged();
        }
    }

    public int SistemInformasiCount
    {
        get => _sistemInformasiCount;
        private set
        {
            _sistemInformasiCount = value;
            OnPropertyChanged();
        }
    }

    public int LakiLakiCount
    {
        get => _lakiLakiCount;
        private set
        {
            _lakiLakiCount = value;
            OnPropertyChanged();
        }
    }

    public int PerempuanCount
    {
        get => _perempuanCount;
        private set
        {
            _perempuanCount = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand RefreshCommand { get; }

    public StudentViewModel()
    {
        AddCommand = new RelayCommand(
            async _ => await AddAsync());

        UpdateCommand = new RelayCommand(
            async _ => await UpdateAsync());

        DeleteCommand = new RelayCommand(
            async _ => await DeleteAsync());

        ResetCommand = new RelayCommand(
            _ => ResetForm());

        RefreshCommand = new RelayCommand(
            async _ => await LoadStudentsAsync());

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        try
        {
            await _database.InitializeAsync();
            await LoadStudentsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Database tidak dapat diinisialisasi. Pastikan SQL Server LocalDB tersedia.\n\n{ex.Message}",
                "Database Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async Task LoadStudentsAsync()
    {
        try
        {
            var students = await _database.GetStudentsAsync(SearchText);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Students.Clear();

                foreach (var student in students)
                {
                    Students.Add(student);
                }
            });

            TotalStudents = Students.Count;
            InformatikaCount = Students.Count(s => 
                s.Prodi.Equals("Informatika", StringComparison.OrdinalIgnoreCase));

            SistemInformasiCount = Students.Count(s => 
                s.Prodi.Equals("Sistem Informasi", StringComparison.OrdinalIgnoreCase));

            LakiLakiCount = Students.Count(s => 
                s.JenisKelamin.Equals("Laki-laki", StringComparison.OrdinalIgnoreCase));

            PerempuanCount = Students.Count(s => 
                s.JenisKelamin.Equals("Perempuan", StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Load Data",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async Task AddAsync()
    {
        if (!ValidateForm())
            return;

        try
        {
            await _database.AddStudentAsync(CreateStudent());
            await LoadStudentsAsync();
            ResetForm();

            MessageBox.Show(
                "Data mahasiswa berhasil ditambahkan.",
                "Berhasil",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Data gagal ditambahkan.\n\n{ex.Message}",
                "Tambah Data",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async Task UpdateAsync()
    {
        if (SelectedStudent is null)
    {
        MessageBox.Show(
            "Pilih mahasiswa yang ingin diedit dari tabel.",
            "Edit Data",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
        return;
    }

    if (!ValidateForm())
        return;

        try
        {
            var student = CreateStudent();
            student.Id = SelectedStudent.Id;

            await _database.UpdateStudentAsync(student);
            await LoadStudentsAsync();
            ResetForm();

            MessageBox.Show(
                "Data mahasiswa berhasil diperbarui.",
                "Berhasil",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Data gagal diperbarui.\n\n{ex.Message}",
                "Update Data",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async Task DeleteAsync()
    {
        if (SelectedStudent is null)
{
    MessageBox.Show(
        "Pilih mahasiswa yang ingin dihapus dari tabel.",
        "Hapus Data",
        MessageBoxButton.OK,
        MessageBoxImage.Warning);
    return;
}

        var confirm = MessageBox.Show(
            $"Hapus data {SelectedStudent.Nama}?",
            "Konfirmasi",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            await _database.DeleteStudentAsync(SelectedStudent.Id);
            await LoadStudentsAsync();
            ResetForm();

            MessageBox.Show(
                "Data mahasiswa berhasil dihapus.",
                "Berhasil",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Data gagal dihapus.\n\n{ex.Message}",
                "Delete Data",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private Student CreateStudent() => new()
    {
        Nim = Nim,
        Nama = Nama,
        Prodi = Prodi,
        JenisKelamin = JenisKelamin,
        Gmail = Gmail
    };

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(Nim) ||
            string.IsNullOrWhiteSpace(Nama) ||
            string.IsNullOrWhiteSpace(Prodi) ||
            string.IsNullOrWhiteSpace(JenisKelamin) ||
            string.IsNullOrWhiteSpace(Gmail))
        {
            MessageBox.Show(
                "Semua field harus diisi.",
                "Validasi",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return false;
        }

        if (!Gmail.Contains('@') || !Gmail.Contains('.'))
        {
            MessageBox.Show(
                "Format Gmail tidak valid.",
                "Validasi",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return false;
        }

        return true;
    }

    private void ResetForm()
    {
        SelectedStudent = null;
        Nim = Nama = Prodi = JenisKelamin = Gmail = "";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}