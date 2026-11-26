IF DB_ID(N'StudentManagerDB') IS NULL
BEGIN
    CREATE DATABASE StudentManagerDB;
END
GO
USE StudentManagerDB;
GO
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
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Students)
BEGIN
    INSERT INTO dbo.Students (Nim, Nama, Prodi, JenisKelamin, Gmail)
    VALUES
    ('5025241001', 'Alya Putri', 'Informatika', 'Perempuan', 'alya.putri@gmail.com'),
    ('5025241002', 'Raka Pratama', 'Sistem Informasi', 'Laki-laki', 'raka.pratama@gmail.com'),
    ('5025241003', 'Nadia Safitri', 'Informatika', 'Perempuan', 'nadia.safitri@gmail.com');
END
GO
