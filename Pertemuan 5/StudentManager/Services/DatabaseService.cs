using Microsoft.Data.SqlClient;
using StudentManager.Models;
using System.Data;

namespace StudentManager.Services;

public class DatabaseService
{
    private const string MasterConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Integrated Security=True;TrustServerCertificate=True;";
    private const string DatabaseName = "StudentManagerDB";
    private string ConnectionString => $"{MasterConnectionString}Database={DatabaseName};";

    public async Task InitializeAsync()
    {
        await using (var connection = new SqlConnection(MasterConnectionString))
        {
            await connection.OpenAsync();
            var sql = $"""
                IF DB_ID(N'{DatabaseName}') IS NULL
                BEGIN
                    CREATE DATABASE [{DatabaseName}]
                END
                """;
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }

        await using (var connection = new SqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var sql = """
                IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.Students
                    (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Nim NVARCHAR(20) NOT NULL UNIQUE,
                        Nama NVARCHAR(100) NOT NULL,
                        Prodi NVARCHAR(100) NOT NULL,
                        JenisKelamin NVARCHAR(20) NOT NULL,
                        Gmail NVARCHAR(150) NOT NULL
                    );

                    INSERT INTO dbo.Students (Nim, Nama, Prodi, JenisKelamin, Gmail)
                    VALUES
                    ('5025241001', 'Alya Putri', 'Informatika', 'Perempuan', 'alya.putri@gmail.com'),
                    ('5025241002', 'Raka Pratama', 'Sistem Informasi', 'Laki-laki', 'raka.pratama@gmail.com'),
                    ('5025241003', 'Nadia Safitri', 'Informatika', 'Perempuan', 'nadia.safitri@gmail.com');
                END
                """;
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<List<Student>> GetStudentsAsync(string keyword = "")
    {
        var result = new List<Student>();
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        const string sql = """
            SELECT Id, Nim, Nama, Prodi, JenisKelamin, Gmail
            FROM dbo.Students
            WHERE @Keyword = N''
               OR Nim LIKE N'%' + @Keyword + N'%'
               OR Nama LIKE N'%' + @Keyword + N'%'
               OR Prodi LIKE N'%' + @Keyword + N'%'
            ORDER BY Id DESC
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Keyword", SqlDbType.NVarChar, 100).Value = keyword.Trim();

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new Student
            {
                Id = reader.GetInt32(0),
                Nim = reader.GetString(1),
                Nama = reader.GetString(2),
                Prodi = reader.GetString(3),
                JenisKelamin = reader.GetString(4),
                Gmail = reader.GetString(5)
            });
        }
        return result;
    }

    public async Task<int> GetTotalStudentsAsync()
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Students", connection);
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public Task AddStudentAsync(Student student) =>
        ExecuteNonQueryAsync("""
            INSERT INTO dbo.Students (Nim, Nama, Prodi, JenisKelamin, Gmail)
            VALUES (@Nim, @Nama, @Prodi, @JenisKelamin, @Gmail)
            """, student);

    public Task UpdateStudentAsync(Student student) =>
        ExecuteNonQueryAsync("""
            UPDATE dbo.Students
            SET Nim = @Nim, Nama = @Nama, Prodi = @Prodi,
                JenisKelamin = @JenisKelamin, Gmail = @Gmail
            WHERE Id = @Id
            """, student);

    public async Task DeleteStudentAsync(int id)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(
            "DELETE FROM dbo.Students WHERE Id = @Id", connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        await command.ExecuteNonQueryAsync();
    }

    private async Task ExecuteNonQueryAsync(string sql, Student student)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@Id", SqlDbType.Int).Value = student.Id;
        command.Parameters.Add("@Nim", SqlDbType.NVarChar, 20).Value = student.Nim.Trim();
        command.Parameters.Add("@Nama", SqlDbType.NVarChar, 100).Value = student.Nama.Trim();
        command.Parameters.Add("@Prodi", SqlDbType.NVarChar, 100).Value = student.Prodi.Trim();
        command.Parameters.Add("@JenisKelamin", SqlDbType.NVarChar, 20).Value = student.JenisKelamin.Trim();
        command.Parameters.Add("@Gmail", SqlDbType.NVarChar, 150).Value = student.Gmail.Trim();

        await command.ExecuteNonQueryAsync();
    }
}
