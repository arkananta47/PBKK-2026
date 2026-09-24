using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace StudentRegistrationApp
{
    public class AppContext
    {
        private readonly string connectionString =
            "Data Source=mahasiswa.db";

        public AppContext()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using SqliteConnection connection =
                new SqliteConnection(connectionString);

            connection.Open();

            string query = @"
                CREATE TABLE IF NOT EXISTS mahasiswa (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    nama TEXT NOT NULL,
                    nrp TEXT NOT NULL UNIQUE,
                    prodi TEXT NOT NULL,
                    gender TEXT NOT NULL
                );
            ";

            using SqliteCommand command =
                new SqliteCommand(query, connection);

            command.ExecuteNonQuery();
        }

        public List<Mahasiswa> GetAllMahasiswa()
        {
            List<Mahasiswa> mahasiswaList = new();

            using SqliteConnection connection =
                new SqliteConnection(connectionString);

            connection.Open();

            string query = @"
                SELECT id, nama, nrp, prodi, gender
                FROM mahasiswa
                ORDER BY id DESC;
            ";

            using SqliteCommand command =
                new SqliteCommand(query, connection);

            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                mahasiswaList.Add(new Mahasiswa
                {
                    Id = reader.GetInt32(0),
                    Nama = reader.GetString(1),
                    NRP = reader.GetString(2),
                    Prodi = reader.GetString(3),
                    Gender = reader.GetString(4)
                });
            }

            return mahasiswaList;
        }

        public bool AddMahasiswa(Mahasiswa mahasiswa)
        {
            using SqliteConnection connection =
                new SqliteConnection(connectionString);

            connection.Open();

            string query = @"
                INSERT INTO mahasiswa
                (nama, nrp, prodi, gender)
                VALUES
                (@nama, @nrp, @prodi, @gender);
            ";

            using SqliteCommand command =
                new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@nama", mahasiswa.Nama);
            command.Parameters.AddWithValue("@nrp", mahasiswa.NRP);
            command.Parameters.AddWithValue("@prodi", mahasiswa.Prodi);
            command.Parameters.AddWithValue("@gender", mahasiswa.Gender);

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException)
            {
                return false;
            }
        }

        public bool UpdateMahasiswa(Mahasiswa mahasiswa)
        {
            using SqliteConnection connection =
                new SqliteConnection(connectionString);

            connection.Open();

            string query = @"
                UPDATE mahasiswa
                SET nama = @nama,
                    nrp = @nrp,
                    prodi = @prodi,
                    gender = @gender
                WHERE id = @id;
            ";

            using SqliteCommand command =
                new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@id", mahasiswa.Id);
            command.Parameters.AddWithValue("@nama", mahasiswa.Nama);
            command.Parameters.AddWithValue("@nrp", mahasiswa.NRP);
            command.Parameters.AddWithValue("@prodi", mahasiswa.Prodi);
            command.Parameters.AddWithValue("@gender", mahasiswa.Gender);

            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (SqliteException)
            {
                return false;
            }
        }

        public bool DeleteMahasiswa(int id)
        {
            using SqliteConnection connection =
                new SqliteConnection(connectionString);

            connection.Open();

            string query = @"
                DELETE FROM mahasiswa
                WHERE id = @id;
            ";

            using SqliteCommand command =
                new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }
    }
}