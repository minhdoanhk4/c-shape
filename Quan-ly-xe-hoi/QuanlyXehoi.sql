-- Tạo Database (nếu chưa tồn tại)
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'ManagementCarDB')
BEGIN
    CREATE DATABASE ManagementCarDB;
END
GO

USE ManagementCarDB;
GO

--------------------------------------------------------------------------------
-- XÓA BẢNG VÀ TẠO LẠI ĐỂ ĐẢM BẢO KHÔNG CÓ LỖI TỪ CÁC LẦN CHẠY TRƯỚC
--------------------------------------------------------------------------------
IF OBJECT_ID('Car', 'U') IS NOT NULL DROP TABLE Car;
IF OBJECT_ID('Type', 'U') IS NOT NULL DROP TABLE Type;
IF OBJECT_ID('Brand', 'U') IS NOT NULL DROP TABLE Brand;

-- 1. Tạo bảng Brand (Thương hiệu)
CREATE TABLE Brand (
    brandID INT PRIMARY KEY,
    brandName NVARCHAR(100),
    foundingYear INT,
    brandNational NVARCHAR(50),
    brandDescription NVARCHAR(MAX)
);
GO

-- 2. Tạo bảng Type (Loại xe)
CREATE TABLE Type (
    typeID INT PRIMARY KEY,
    carType NVARCHAR(100),
    typeName NVARCHAR(100), -- Tên cột chính xác
    typeDescription NVARCHAR(MAX),
    status BIT
);
GO

-- 3. Tạo bảng Car (Xe ô tô)
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

    CONSTRAINT FK_Car_Type FOREIGN KEY (typeID)
    REFERENCES Type(typeID),

    CONSTRAINT FK_Car_Brand FOREIGN KEY (brandID)
    REFERENCES Brand(brandID)
);
GO

--------------------------------------------------------------------------------
-- CHÈN DỮ LIỆU (ĐÃ SỬA TÊN CỘT nameType -> typeName)
--------------------------------------------------------------------------------

-- Dữ liệu cho bảng Brand
INSERT INTO Brand (brandID, brandName, foundingYear, brandNational, brandDescription) VALUES
(1, N'Toyota', 1937, N'Nhật Bản', N'Thương hiệu lâu đời và phổ biến nhất tại Việt Nam, nổi tiếng với độ bền bỉ, giữ giá tốt và mạng lưới dịch vụ rộng khắp. Các dòng xe xăng/diesel truyền thống có doanh số rất cao.'),
(2, N'Hyundai', 1967, N'Hàn Quốc', N'"Ngựa ô" đang vươn lên mạnh mẽ, thu hút khách hàng nhờ thiết kế trẻ trung, hiện đại, nhiều tính năng công nghệ và giá cả cạnh tranh.'),
(3, N'Ford', 1903, N'Mỹ', N'Mạnh về các dòng xe bán tải (Pickup) và SUV, nổi tiếng với sự mạnh mẽ, bền bỉ và khả năng vận hành vượt trội.'),
(4, N'VinFast', 2017, N'Việt Nam', N'Thương hiệu ô tô nội địa hàng đầu, đã chuyển đổi hoàn toàn sang sản xuất xe điện. Hiện đang là hãng dẫn đầu về doanh số xe điện tại Việt Nam. Nổi bật với dải sản phẩm đa dạng và hệ thống trạm sạc rộng khắp.'),
(5, N'Honda', 1948, N'Nhật Bản', N'Được ưa chuộng với các mẫu xe có động cơ ổn định, tiết kiệm nhiên liệu, thiết kế thể thao và hiện đại, mang lại trải nghiệm lái tốt.'),
(6, N'Mitsubishi', 1970, N'Nhật Bản', N'Các mẫu xe được đánh giá cao về khả năng vận hành, bền bỉ và phù hợp với nhiều điều kiện đường sá.'),
(7, N'Mazda', 1920, N'Nhật Bản', N'Thuộc sở hữu của THACO tại Việt Nam. Xe Mazda nổi tiếng với thiết kế KODO đẹp mắt, nội thất sang trọng và trải nghiệm lái xe đầy cảm xúc.'),
(8, N'Kia', 1944, N'Hàn Quốc', N'Thuộc sở hữu của THACO tại Việt Nam. Mang đến nhiều sự lựa chọn với mẫu mã đa dạng, thiết kế mới mẻ và mức giá dễ tiếp cận.'),
(9, N'Mercedes-Benz', 1926, N'Đức', N'Thương hiệu xe sang hàng đầu, lắp ráp tại Việt Nam. Nổi tiếng với sự sang trọng, đẳng cấp và công nghệ an toàn.'),
(10, N'Audi', 1909, N'Đức', N'Thương hiệu xe sang của Đức, nổi tiếng với công nghệ đèn chiếu sáng (ví dụ: Matrix LED), hệ thống dẫn động 4 bánh Quattro và khoang nội thất kỹ thuật số cao cấp (Audi Virtual Cockpit).');
GO

-- Dữ liệu cho bảng Type (Đã sửa nameType thành typeName)
INSERT INTO Type (typeID, carType, typeName, typeDescription, status) VALUES
(1, N'A-SUV', N'SUV Hạng A (Mini SUV)', N'Xe SUV gầm cao cỡ nhỏ nhất, chủ yếu dùng trong đô thị, kích thước nhỏ gọn.', 1),
(2, N'B-Sedan', N'Sedan Hạng B', N'Dòng sedan 4 cửa, 3 khoang phổ thông, kích thước nhỏ gọn hơn hạng C, rất phổ biến cho gia đình và kinh doanh dịch vụ.', 1),
(3, N'C-SUV', N'SUV Hạng C', N'SUV cỡ trung phổ biến, cân bằng giữa kích thước nội thất và khả năng vận hành, thường là 5 chỗ.', 1),
(4, N'D-SUV', N'SUV Hạng D', N'SUV cỡ lớn, không gian rộng rãi và cao cấp hơn hạng C, có thể là 5 chỗ hoặc 7 chỗ tùy mẫu xe.', 1),
(5, N'Bán tải', N'Xe bán tải (Pickup)', N'Xe có cabin (khoang hành khách) và thùng hàng riêng biệt phía sau, kết hợp chức năng chở người và chở hàng.', 1),
(6, N'MPV 7 chỗ', N'Xe đa dụng 7 chỗ', N'Xe gia đình (Multi-Purpose Vehicle) ưu tiên tối đa hóa không gian và sự thoải mái cho 7 người, trần xe cao, thiết kế thân xe liền khối (Monocoque).', 1),
(7, N'MPV Hạng sang', N'Xe đa dụng cao cấp', N'Dòng xe MPV với thiết kế và tiện nghi nội thất sang trọng, hướng đến trải nghiệm cao cấp (ghế thương gia), thường là 7-8 chỗ.', 1),
(8, N'SUV 7 chỗ', N'Xe SUV cỡ lớn 7 chỗ', N'SUV cỡ lớn được thiết kế trên khung gầm cứng cáp (thường là Body-on-frame), mạnh mẽ, ưu tiên khả năng vượt địa hình và tải nặng.', 1),
(9, N'Sedan Coupe', N'Sedan/Coupe 4 cửa', N'Xe có kiểu dáng thể thao của Coupe (mái dốc, không viền cửa) nhưng vẫn có 4 cửa tiện lợi, thường thuộc phân khúc hạng sang.', 1);
GO

-- Dữ liệu cho bảng Car
INSERT INTO Car (carID, carModel, carEngine, typeID, carSeat, carDescription, yearPublish, dollarPrice, carWarranty, brandID) VALUES
(1, N'VF 5 Plus', N'Điện', 1, 5, N'Xe điện mini đô thị, giá dễ tiếp cận, kích thước nhỏ gọn phù hợp di chuyển trong phố.', 2023, 19000.00, 7, 4),
(2, N'Ranger', N'Diesel', 5, 5, N'"Vua bán tải" với khả năng chở hàng, kéo tải và vượt địa hình hàng đầu.', 1983, 26800.00, 3, 3),
(3, N'Vios', N'Xăng', 2, 5, N'Độ bền bỉ, giữ giá tốt và cực kỳ tiết kiệm nhiên liệu, "xe quốc dân" cho gia đình và kinh doanh dịch vụ.', 2002, 18300.00, 3, 1),
(4, N'Accent', N'Xăng', 2, 5, N'Thiết kế hiện đại, nhiều tiện nghi điện tử, giá cả rất cạnh tranh.', 1994, 17000.00, 5, 2),
(5, N'VF e34', N'Điện', 3, 5, N'Trang bị nhiều tính năng thông minh như trợ lý ảo và cập nhật phần mềm từ xa (OTA).', 2021, 28800.00, 10, 4),
(6, N'Corolla Cross', N'Xăng/Hybrid', 3, 5, N'Có phiên bản Hybrid tiết kiệm xăng, trang bị công nghệ an toàn Toyota Safety Sense.', 2004, 32800.00, 3, 1),
(7, N'Tucson', N'Xăng/Diesel', 3, 5, N'Thiết kế ngoại hình táo bạo, nội thất rộng rãi, có tính năng sưởi/làm mát ghế.', 2020, 33000.00, 5, 2),
(8, N'CX-5', N'Xăng', 3, 5, N'Thiết kế KODO đẹp mắt, nội thất tinh tế, nổi bật với công nghệ hỗ trợ lái i-Activsense.', 2012, 30000.00, 3, 7),
(9, N'VF 8', N'Điện', 4, 5, N'SUV điện hiệu suất cao, có thị trường xuất khẩu tại Bắc Mỹ và châu Âu.', 2023, 40800.00, 10, 4),
(10, N'Fortuner', N'Xăng/Diesel', 4, 7, N'SUV 7 chỗ khung gầm rời (Body-on-frame) cứng cáp, phù hợp cho địa hình phức tạp, khả năng vận hành mạnh mẽ.', 2005, 41000.00, 3, 1),
(11, N'Xpander', N'Xăng', 6, 7, N'"Ông vua" phân khúc MPV, không gian rộng rãi, chi phí vận hành hợp lý.', 2017, 22400.00, 3, 6),
(12, N'Carnival', N'Xăng/Diesel', 7, 8, N'Khoang hành khách tiện nghi, rộng rãi như hạng thương gia (business class), thích hợp cho doanh nhân.', 1998, 52000.00, 3, 8),
(13, N'Everest', N'Diesel', 8, 7, N'Mạnh mẽ, dựa trên khung gầm Ranger, nổi tiếng về sự tiện nghi, cách âm tốt và an toàn.', 2003, 44000.00, 3, 3),
(14, N'A5 Sportback', N'Xăng', 9, 4, N'Thiết kế mui dốc (Sportback) độc đáo, nội thất công nghệ cao, thiên về sự tiện nghi và tinh tế.', 2009, 86500.00, 3, 10);
GO

