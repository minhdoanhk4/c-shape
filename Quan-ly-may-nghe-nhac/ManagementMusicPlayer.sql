-- *************** BƯỚC 1: ĐẢM BẢO XÓA DATABASE THÀNH CÔNG ***************

-- Kiểm tra và Xóa Database nếu nó đã tồn tại
IF EXISTS (SELECT name FROM master.sys.databases WHERE name = N'ManagementMusicPlayer')
BEGIN
    -- Buộc đóng tất cả các kết nối đang hoạt động đến Database
    ALTER DATABASE ManagementMusicPlayer SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    
    -- Xóa Database
    DROP DATABASE ManagementMusicPlayer;
END
GO

-- *************** BƯỚC 2: TẠO MỚI DATABASE VÀ CÁC BẢNG ***************

-- 1. Tạo Database
CREATE DATABASE ManagementMusicPlayer;
GO

-- 2. Sử dụng Database vừa tạo
USE ManagementMusicPlayer;
GO

-- 3. Tạo Bảng Account
CREATE TABLE Account (
    id INT IDENTITY(1,1) PRIMARY KEY,
    fullName NVARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL, 
    password VARCHAR(100) NOT NULL,
    roleID INT NOT NULL
);
GO

-- 4. Tạo Bảng Company
CREATE TABLE Company (
    companyID INT IDENTITY(1,1) PRIMARY KEY,
    companyName NVARCHAR(100) UNIQUE NOT NULL,
    status BIT NOT NULL DEFAULT 1
);
GO

-- 5. Tạo Bảng MusicPlayer
CREATE TABLE MusicPlayer (
    id INT IDENTITY(1,1) PRIMARY KEY,
    playerName NVARCHAR(100) NOT NULL,
    category NVARCHAR(50) NOT NULL, -- Sẽ dùng: "Flagship", "Premium", "Medium"
    price DECIMAL(18, 2) NOT NULL,
    quantity INT NOT NULL,
    publishDate DATE NOT NULL,
    companyID INT NOT NULL,

    -- Thiết lập khóa ngoại
    FOREIGN KEY (companyID) REFERENCES Company(companyID)
);
GO

-- *************** BƯỚC 3: CHÈN DỮ LIỆU MẪU ***************

-- 6. Thêm dữ liệu mẫu vào bảng Company
INSERT INTO Company (companyName, status) VALUES
('Sony', 1),
('Apple', 1),
('Astell&Kern', 1),
('FiiO', 1);
GO

-- 7. Thêm dữ liệu mẫu vào bảng Account
INSERT INTO Account (fullName, email, password, roleID) VALUES
('Admin User', 'admin@app.com', '123456', 1),
('Manager User', 'manager@app.com', '123456', 2),
('Staff User', 'staff@app.com', '123456', 3),
('Member User', 'member@app.com', '123456', 4);
GO

-- 8. Thêm dữ liệu mẫu vào bảng MusicPlayer (Sử dụng category mới)
INSERT INTO MusicPlayer (playerName, category, price, quantity, publishDate, companyID) VALUES
('Walkman NW-A105', 'Medium', 350.00, 10, '2023-11-01', 1), -- Category: Medium
('iPod Classic Gen 7', 'Medium', 250.50, 5, '2009-09-09', 2), -- Category: Medium
('A&K Kann Max', 'Flagship', 1300.99, 3, '2024-05-15', 3), -- Category: Flagship
('FiiO M11 Plus', 'Premium', 700.00, 8, '2024-10-20', 4), -- Category: Premium
('Walkman ZX507', 'Premium', 900.00, 6, '2023-08-25', 1); -- Category: Premium
GO