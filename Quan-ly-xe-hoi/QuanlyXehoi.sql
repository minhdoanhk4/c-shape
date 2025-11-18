-- 1. KHỞI TẠO DATABASE
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'ManagementCarDB')
BEGIN
    CREATE DATABASE ManagementCarDB;
END
GO

USE ManagementCarDB;
GO

--------------------------------------------------------------------------------
-- 2. XÓA BẢNG CŨ (DROP TABLES)
-- Lưu ý: Phải xóa bảng con (Car) trước, rồi mới xóa bảng cha (Brand, Type)
--------------------------------------------------------------------------------
IF OBJECT_ID('Car', 'U') IS NOT NULL DROP TABLE Car;
IF OBJECT_ID('Account', 'U') IS NOT NULL DROP TABLE Account; -- Bảng mới thêm
IF OBJECT_ID('Type', 'U') IS NOT NULL DROP TABLE Type;
IF OBJECT_ID('Brand', 'U') IS NOT NULL DROP TABLE Brand;
GO

--------------------------------------------------------------------------------
-- 3. TẠO BẢNG (CREATE TABLES)
--------------------------------------------------------------------------------

-- Bảng Brand (Thương hiệu)
CREATE TABLE Brand (
    brandID INT PRIMARY KEY,
    brandName NVARCHAR(100),
    foundingYear INT,
    brandNational NVARCHAR(50),
    brandDescription NVARCHAR(MAX)
);
GO

-- Bảng Type (Loại xe)
CREATE TABLE Type (
    typeID INT PRIMARY KEY,
    carType NVARCHAR(100),
    typeName NVARCHAR(100),
    typeDescription NVARCHAR(MAX),
    status BIT
);
GO

-- Bảng Account (Tài khoản - MỚI THÊM)
CREATE TABLE Account (
    accountID INT PRIMARY KEY,
    fullName NVARCHAR(100),    -- Tên hiển thị
    email VARCHAR(100),        -- Email đăng nhập
    password VARCHAR(50),      -- Mật khẩu
    roleID INT,                -- 1: Admin, 2: Manager, 3: Staff, 4: Member
    roleName NVARCHAR(50),     -- Tên quyền hạn
    status BIT                 -- 1: Active, 0: Disabled
);
GO

-- Bảng Car (Xe ô tô)
CREATE TABLE Car (
    carID INT PRIMARY KEY,
    carModel NVARCHAR(100),
    carEngine NVARCHAR(50),
    typeID INT,
    carSeat INT,
    carDescription NVARCHAR(MAX),
    yearPublish INT,
    dollarPrice FLOAT,
    carWarranty INT,
    brandID INT,

    -- Khóa ngoại
    CONSTRAINT FK_Car_Type FOREIGN KEY (typeID) REFERENCES Type(typeID),
    CONSTRAINT FK_Car_Brand FOREIGN KEY (brandID) REFERENCES Brand(brandID)
);
GO

--------------------------------------------------------------------------------
-- 4. CHÈN DỮ LIỆU (INSERT DATA)
--------------------------------------------------------------------------------

-- Dữ liệu Brand
INSERT INTO Brand (brandID, brandName, foundingYear, brandNational, brandDescription) VALUES
(1, N'Toyota', 1937, N'Nhật Bản', N'Thương hiệu lâu đời và phổ biến nhất tại Việt Nam, nổi tiếng với độ bền bỉ.'),
(2, N'Hyundai', 1967, N'Hàn Quốc', N'"Ngựa ô" đang vươn lên mạnh mẽ, thiết kế trẻ trung.'),
(3, N'Ford', 1903, N'Mỹ', N'Mạnh về các dòng xe bán tải (Pickup) và SUV.'),
(4, N'VinFast', 2017, N'Việt Nam', N'Thương hiệu ô tô nội địa hàng đầu, chuyên xe điện.'),
(5, N'Honda', 1948, N'Nhật Bản', N'Động cơ ổn định, tiết kiệm nhiên liệu, cảm giác lái tốt.'),
(6, N'Mitsubishi', 1970, N'Nhật Bản', N'Vận hành bền bỉ, phù hợp nhiều điều kiện đường sá.'),
(7, N'Mazda', 1920, N'Nhật Bản', N'Thiết kế KODO đẹp mắt, công nghệ SkyActiv.'),
(8, N'Kia', 1944, N'Hàn Quốc', N'Mẫu mã đa dạng, giá dễ tiếp cận.'),
(9, N'Mercedes-Benz', 1926, N'Đức', N'Xe sang đẳng cấp, an toàn vượt trội.'),
(10, N'Audi', 1909, N'Đức', N'Nổi tiếng với công nghệ đèn và hệ dẫn động Quattro.');
GO

-- Dữ liệu Type
INSERT INTO Type (typeID, carType, typeName, typeDescription, status) VALUES
(1, N'A-SUV', N'SUV Hạng A (Mini SUV)', N'Xe gầm cao cỡ nhỏ nhất.', 1),
(2, N'B-Sedan', N'Sedan Hạng B', N'Sedan phổ thông, nhỏ gọn.', 1),
(3, N'C-SUV', N'SUV Hạng C', N'SUV cỡ trung phổ biến.', 1),
(4, N'D-SUV', N'SUV Hạng D', N'SUV cỡ lớn, rộng rãi.', 1),
(5, N'Bán tải', N'Xe bán tải (Pickup)', N'Vừa chở người vừa chở hàng.', 1),
(6, N'MPV 7 chỗ', N'Xe đa dụng 7 chỗ', N'Xe gia đình tối ưu không gian.', 1),
(7, N'MPV Hạng sang', N'Xe đa dụng cao cấp', N'MPV với tiện nghi thương gia.', 1),
(8, N'SUV 7 chỗ', N'Xe SUV cỡ lớn 7 chỗ', N'Khung gầm cứng cáp, đi địa hình.', 1),
(9, N'Sedan Coupe', N'Sedan/Coupe 4 cửa', N'Dáng thể thao mui dốc.', 1);
GO

-- Dữ liệu Account (MỚI THÊM)
INSERT INTO Account (accountID, fullName, email, password, roleID, roleName, status) VALUES
(1, N'Trần Xuân',  'tranxuan@email.com',  'ad01', 1, N'Admin',   0),
(2, N'Nguyễn Gia', 'nguyengia@email.com', 'st03', 3, N'Staff',   1),
(3, N'Lý Khanh',   'lykhanh@email.com',   'ma02', 2, N'Manager', 1),
(4, N'Lê Lộc',     'leloc@email.com',     'me04', 4, N'Member',  0),
(5, N'Tân Quan',   'tanquan@email.com',   'st03', 3, N'Staff',   0),
(6, N'Chu Hoa',    'chuhoa@email.com',    'ad01', 1, N'Admin',   1);
GO

-- Dữ liệu Car
INSERT INTO Car (carID, carModel, carEngine, typeID, carSeat, carDescription, yearPublish, dollarPrice, carWarranty, brandID) VALUES
(1, N'VF 5 Plus', N'Điện', 1, 5, N'Xe điện mini đô thị.', 2023, 19000.00, 7, 4),
(2, N'Ranger', N'Diesel', 5, 5, N'Vua bán tải.', 1983, 26800.00, 3, 3),
(3, N'Vios', N'Xăng', 2, 5, N'Xe quốc dân bền bỉ.', 2002, 18300.00, 3, 1),
(4, N'Accent', N'Xăng', 2, 5, N'Thiết kế hiện đại, giá tốt.', 1994, 17000.00, 5, 2),
(5, N'VF e34', N'Điện', 3, 5, N'Nhiều tính năng thông minh.', 2021, 28800.00, 10, 4),
(6, N'Corolla Cross', N'Xăng/Hybrid', 3, 5, N'An toàn, tiết kiệm.', 2004, 32800.00, 3, 1),
(7, N'Tucson', N'Xăng/Diesel', 3, 5, N'Ngoại hình táo bạo.', 2020, 33000.00, 5, 2),
(8, N'CX-5', N'Xăng', 3, 5, N'Thiết kế đẹp, lái hay.', 2012, 30000.00, 3, 7),
(9, N'VF 8', N'Điện', 4, 5, N'SUV hiệu suất cao.', 2023, 40800.00, 10, 4),
(10, N'Fortuner', N'Xăng/Diesel', 4, 7, N'Thánh lật địa hình.', 2005, 41000.00, 3, 1),
(11, N'Xpander', N'Xăng', 6, 7, N'Vua doanh số MPV.', 2017, 22400.00, 3, 6),
(12, N'Carnival', N'Xăng/Diesel', 7, 8, N'Rộng như chuyên cơ.', 1998, 52000.00, 3, 8),
(13, N'Everest', N'Diesel', 8, 7, N'Mạnh mẽ, tiện nghi.', 2003, 44000.00, 3, 3),
(14, N'A5 Sportback', N'Xăng', 9, 4, N'Sang trọng, thể thao.', 2009, 86500.00, 3, 10);
GO

