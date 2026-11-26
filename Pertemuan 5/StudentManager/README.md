# Student Manager

Aplikasi desktop pengelolaan data mahasiswa menggunakan C# WPF, MVVM, ADO.NET, dan SQL Server LocalDB.

## Fitur
- Read data mahasiswa
- Create/tambah mahasiswa
- Update/edit mahasiswa
- Delete/hapus mahasiswa
- Reset form
- Search berdasarkan NIM, nama, dan program studi
- Statistik total mahasiswa
- Validasi field wajib dan Gmail

## Menjalankan
1. Install Visual Studio 2022 dengan workload .NET desktop development.
2. Pastikan SQL Server Express LocalDB tersedia pada instance `(localdb)\MSSQLLocalDB`.
3. Buka `StudentManager.csproj`.
4. Restore NuGet package.
5. Run project.
6. Database `StudentManagerDB` dan tabel `Students` dibuat otomatis saat startup.

## Arsitektur
View -> ViewModel -> DatabaseService -> SQL Server LocalDB
