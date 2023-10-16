CREATE DATABASE QLSach;
GO
USE QLSach;


CREATE TABLE NguoiDung (
    IDNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    TenNguoiDung NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    MatKhau NVARCHAR(100) NOT NULL,
    NgayDangKy DATETIME,
    VaiTro NVARCHAR(50)
);
-- Tạo bảng LoaiSach
CREATE TABLE LoaiSach (
    IDLoaiSach INT IDENTITY(1,1) PRIMARY KEY,
    TenLoaiSach NVARCHAR(150) NOT NULL
);

-- Tạo Sach

CREATE TABLE Sach (
    IDSach INT IDENTITY(1,1) PRIMARY KEY,
	MaSach NVARCHAR(10),
    TenSach NVARCHAR(250) NOT NULL,
	DonGia DECIMAL(18, 0),
	SoLuong INT,
    IDLoaiSach INT NOT NULL,
	AnhBia NVARCHAR(250),
    FOREIGN KEY (IDLoaiSach) REFERENCES LoaiSach(IDLoaiSach)
);

-- Chèn dữ liệu tài khoản admin
INSERT INTO NguoiDung (TenNguoiDung, Email, MatKhau, NgayDangKy, VaiTro)
VALUES
    ('admin', 'admin@example.com', '123456', GETDATE(), 'admin');

-- Chèn dữ liệu tài khoản người dùng
INSERT INTO NguoiDung (TenNguoiDung, Email, MatKhau, NgayDangKy, VaiTro)
VALUES
    ('user', 'user@example.com', '123456', GETDATE(), 'nguoidung');

-- Chèn dữ liệu mẫu vào bảng "LoaiSach"
INSERT INTO LoaiSach (TenLoaiSach)
VALUES
    ('CSDL'),
    ('AI'),
    ('PyThon');

-- Chèn dữ liệu mẫu vào bảng "Sach"
INSERT INTO Sach (MaSach, TenSach, DonGia, SoLuong, IDLoaiSach, AnhBia)
VALUES
    ('S01', 'Database Systems A Pragmatic Approach', 250000, 50, 1, 'avatar_book1.jpg'),
    ('S02', 'Artificial Intelligence By Example', 180000, 30, 2, 'avatar_book2.jpg'),
    ('S03', 'Artificial Intelligence Basics: A Non-Technical Introduction', 150000, 60, 2, 'avatar_book3.jpg'),
    ('S04', 'PyThon Basic', 220000, 40, 3, 'avatar_book4.jpg');
