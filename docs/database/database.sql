-- =============================================
-- CƠ SỞ DỮ LIỆU CỬA HÀNG BILLIARD
-- Phiên bản tiếng Việt - SQL Server
-- =============================================

-- Tạo cơ sở dữ liệu
CREATE DATABASE BilliardShop
GO

USE BilliardShop
GO

-- =============================================
-- 1. BẢNG QUẢN LÝ NGƯỜI DÙNG
-- =============================================

-- Bảng vai trò người dùng
CREATE TABLE VaiTroNguoiDung
(
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(255),
    NgayTao DATETIME2 DEFAULT GETDATE(),
    TrangThaiHoatDong BIT DEFAULT 1
)
GO

-- Bảng người dùng
CREATE TABLE NguoiDung
(
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    MatKhauMaHoa NVARCHAR(255) NOT NULL,
    MuoiMatKhau NVARCHAR(255) NOT NULL,
    Ho NVARCHAR(50),
    Ten NVARCHAR(50),
    SoDienThoai NVARCHAR(20),
    NgaySinh DATE,
    GioiTinh CHAR(1) CHECK (GioiTinh IN ('M', 'F', 'K')),
    -- M=Nam, F=Nữ, K=Khác
    MaVaiTro INT NOT NULL,
    DaXacThucEmail BIT DEFAULT 0,
    TrangThaiHoatDong BIT DEFAULT 1,
    NgayTao DATETIME2 DEFAULT GETDATE(),
    LanDangNhapCuoi DATETIME2,
    NgayCapNhatCuoi DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_NguoiDung_VaiTroNguoiDung FOREIGN KEY (MaVaiTro) REFERENCES VaiTroNguoiDung(MaVaiTro)
)
GO

-- Bảng địa chỉ người dùng
CREATE TABLE DiaChiNguoiDung
(
    MaDiaChi INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    LoaiDiaChi NVARCHAR(20) NOT NULL CHECK (LoaiDiaChi IN ('GiaoHang', 'ThanhToan', 'CaHai')),
    HoTenNguoiNhan NVARCHAR(100),
    SoDienThoaiNguoiNhan NVARCHAR(20),
    DiaChi NVARCHAR(255) NOT NULL,
    PhuongXa NVARCHAR(100),
    QuanHuyen NVARCHAR(100),
    ThanhPho NVARCHAR(100) NOT NULL,
    TinhThanhPho NVARCHAR(100) NOT NULL,
    MaBuuDien NVARCHAR(10),
    LaDiaChiMacDinh BIT DEFAULT 0,
    NgayTao DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_DiaChiNguoiDung_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung) ON DELETE CASCADE
)
GO

-- =============================================
-- 2. BẢNG QUẢN LÝ SẢN PHẨM
-- =============================================

-- Bảng danh mục sản phẩm
CREATE TABLE DanhMucSanPham
(
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    DuongDanDanhMuc NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(500),
    MaDanhMucCha INT NULL,
    HinhAnhDaiDien NVARCHAR(255),
    ThuTuSapXep INT DEFAULT 0,
    TrangThaiHoatDong BIT DEFAULT 1,
    NgayTao DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_DanhMucSanPham_DanhMucCha FOREIGN KEY (MaDanhMucCha) REFERENCES DanhMucSanPham(MaDanhMuc)
)
GO

-- Bảng thương hiệu
CREATE TABLE ThuongHieu
(
    MaThuongHieu INT IDENTITY(1,1) PRIMARY KEY,
    TenThuongHieu NVARCHAR(100) NOT NULL UNIQUE,
    DuongDanThuongHieu NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(500),
    LogoThuongHieu NVARCHAR(255),
    Website NVARCHAR(255),
    QuocGia NVARCHAR(50),
    TrangThaiHoatDong BIT DEFAULT 1,
    NgayTao DATETIME2 DEFAULT GETDATE()
)
GO

-- Bảng sản phẩm
CREATE TABLE SanPham
(
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
    MaCodeSanPham NVARCHAR(50) NOT NULL UNIQUE,
    TenSanPham NVARCHAR(255) NOT NULL,
    DuongDanSanPham NVARCHAR(255) NOT NULL UNIQUE,
    MoTaNgan NVARCHAR(500),
    MoTaChiTiet NVARCHAR(MAX),
    -- Đổi từ NTEXT sang NVARCHAR(MAX)
    MaDanhMuc INT NOT NULL,
    MaThuongHieu INT,

    -- Giá cả
    GiaGoc DECIMAL(18,2) NOT NULL CHECK (GiaGoc >= 0),
    GiaKhuyenMai DECIMAL(18,2) CHECK (GiaKhuyenMai >= 0),
    GiaVon DECIMAL(18,2) CHECK (GiaVon >= 0),

    -- Tồn kho
    SoLuongTonKho INT NOT NULL DEFAULT 0 CHECK (SoLuongTonKho >= 0),
    SoLuongToiThieu INT DEFAULT 0,
    SoLuongToiDa INT,

    -- Thuộc tính sản phẩm
    TrongLuong DECIMAL(10,3),
    KichThuoc NVARCHAR(50),
    -- D x R x C
    ChatLieu NVARCHAR(100),
    MauSac NVARCHAR(50),
    KichCo NVARCHAR(50),

    -- SEO
    TieuDeSEO NVARCHAR(255),
    MoTaSEO NVARCHAR(500),
    TuKhoaSEO NVARCHAR(255),

    -- Trạng thái
    TrangThaiHoatDong BIT DEFAULT 1,
    LaSanPhamNoiBat BIT DEFAULT 0,
    LaSanPhamKhuyenMai BIT DEFAULT 0,
    NgayTao DATETIME2 DEFAULT GETDATE(),
    NgayCapNhatCuoi DATETIME2 DEFAULT GETDATE(),
    NguoiTaoMaSanPham INT,

    CONSTRAINT FK_SanPham_DanhMucSanPham FOREIGN KEY (MaDanhMuc) REFERENCES DanhMucSanPham(MaDanhMuc),
    CONSTRAINT FK_SanPham_ThuongHieu FOREIGN KEY (MaThuongHieu) REFERENCES ThuongHieu(MaThuongHieu),
    CONSTRAINT FK_SanPham_NguoiTao FOREIGN KEY (NguoiTaoMaSanPham) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- Bảng hình ảnh sản phẩm
CREATE TABLE HinhAnhSanPham
(
    MaHinhAnh INT IDENTITY(1,1) PRIMARY KEY,
    MaSanPham INT NOT NULL,
    DuongDanHinhAnh NVARCHAR(255) NOT NULL,
    TextThayThe NVARCHAR(255),
    ThuTuSapXep INT DEFAULT 0,
    LaHinhAnhChinh BIT DEFAULT 0,
    NgayTao DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_HinhAnhSanPham_SanPham FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham) ON DELETE CASCADE
)
GO

-- Bảng thuộc tính động của sản phẩm
CREATE TABLE ThuocTinhSanPham
(
    MaThuocTinh INT IDENTITY(1,1) PRIMARY KEY,
    MaSanPham INT NOT NULL,
    TenThuocTinh NVARCHAR(100) NOT NULL,
    GiaTriThuocTinh NVARCHAR(255),

    CONSTRAINT FK_ThuocTinhSanPham_SanPham FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham) ON DELETE CASCADE
)
GO

-- =============================================
-- 3. QUẢN LÝ KHO HÀNG
-- =============================================

-- Bảng nhà cung cấp
CREATE TABLE NhaCungCap
(
    MaNhaCungCap INT IDENTITY(1,1) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(255) NOT NULL,
    NguoiLienHe NVARCHAR(100),
    Email NVARCHAR(100),
    SoDienThoai NVARCHAR(20),
    DiaChi NVARCHAR(255),
    ThanhPho NVARCHAR(100),
    QuocGia NVARCHAR(50),
    TrangThaiHoatDong BIT DEFAULT 1,
    NgayTao DATETIME2 DEFAULT GETDATE()
)
GO

-- Bảng biến động kho hàng
CREATE TABLE BienDongKhoHang
(
    MaBienDong INT IDENTITY(1,1) PRIMARY KEY,
    MaSanPham INT NOT NULL,
    LoaiBienDong NVARCHAR(20) NOT NULL CHECK (LoaiBienDong IN ('NHAP', 'XUAT', 'DIEU_CHINH')),
    SoLuong INT NOT NULL,
    TonKhoTruoc INT NOT NULL,
    TonKhoSau INT NOT NULL,
    ThamChieu NVARCHAR(100),
    -- Mã đơn hàng, lý do điều chỉnh, v.v.
    GhiChu NVARCHAR(255),
    NgayTao DATETIME2 DEFAULT GETDATE(),
    NguoiThucHien INT,

    CONSTRAINT FK_BienDongKhoHang_SanPham FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham),
    CONSTRAINT FK_BienDongKhoHang_NguoiDung FOREIGN KEY (NguoiThucHien) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- =============================================
-- 4. QUẢN LÝ ĐỖN HÀNG
-- =============================================

-- Bảng trạng thái đơn hàng
CREATE TABLE TrangThaiDonHang
(
    MaTrangThai INT IDENTITY(1,1) PRIMARY KEY,
    TenTrangThai NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(255),
    ThuTuSapXep INT DEFAULT 0
)
GO

-- Bảng phương thức thanh toán
CREATE TABLE PhuongThucThanhToan
(
    MaPhuongThucThanhToan INT IDENTITY(1,1) PRIMARY KEY,
    TenPhuongThuc NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(255),
    TrangThaiHoatDong BIT DEFAULT 1
)
GO

-- Bảng phương thức vận chuyển
CREATE TABLE PhuongThucVanChuyen
(
    MaPhuongThucVanChuyen INT IDENTITY(1,1) PRIMARY KEY,
    TenPhuongThuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255),
    PhiCoBan DECIMAL(18,2) NOT NULL DEFAULT 0,
    PhiTheoTrongLuong DECIMAL(18,2) DEFAULT 0,
    SoNgayDuKien INT,
    TrangThaiHoatDong BIT DEFAULT 1
)
GO

-- Bảng đơn hàng
CREATE TABLE DonHang
(
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    SoDonHang NVARCHAR(20) NOT NULL UNIQUE,
    MaNguoiDung INT,

    -- Thông tin khách hàng (cho khách vãng lai)
    EmailKhachHang NVARCHAR(100),
    SoDienThoaiKhachHang NVARCHAR(20),
    TenKhachHang NVARCHAR(100),

    -- Số tiền đơn hàng
    TongTienHang DECIMAL(18,2) NOT NULL DEFAULT 0,
    TienGiamGia DECIMAL(18,2) DEFAULT 0,
    PhiVanChuyen DECIMAL(18,2) DEFAULT 0,
    TienThue DECIMAL(18,2) DEFAULT 0,
    TongThanhToan DECIMAL(18,2) NOT NULL DEFAULT 0,

    -- Địa chỉ
    DiaChiGiaoHang NVARCHAR(500),
    DiaChiThanhToan NVARCHAR(500),

    -- Trạng thái & Thanh toán
    MaTrangThai INT NOT NULL,
    MaPhuongThucThanhToan INT,
    MaPhuongThucVanChuyen INT,
    TrangThaiThanhToan NVARCHAR(20) DEFAULT 'ChoThanhToan' CHECK (TrangThaiThanhToan IN ('ChoThanhToan', 'DaThanhToan', 'ThatBai', 'HoanTien')),

    -- Ngày tháng
    NgayDatHang DATETIME2 DEFAULT GETDATE(),
    NgayYeuCauGiao DATETIME2,
    NgayGiaoHang DATETIME2,
    NgayNhanHang DATETIME2,

    -- Ghi chú
    GhiChuKhachHang NVARCHAR(500),
    GhiChuQuanTri NVARCHAR(500),

    CONSTRAINT FK_DonHang_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_DonHang_TrangThaiDonHang FOREIGN KEY (MaTrangThai) REFERENCES TrangThaiDonHang(MaTrangThai),
    CONSTRAINT FK_DonHang_PhuongThucThanhToan FOREIGN KEY (MaPhuongThucThanhToan) REFERENCES PhuongThucThanhToan(MaPhuongThucThanhToan),
    CONSTRAINT FK_DonHang_PhuongThucVanChuyen FOREIGN KEY (MaPhuongThucVanChuyen) REFERENCES PhuongThucVanChuyen(MaPhuongThucVanChuyen)
)
GO

-- Bảng chi tiết đơn hàng
CREATE TABLE ChiTietDonHang
(
    MaChiTietDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    TenSanPham NVARCHAR(255) NOT NULL,
    -- Lưu tên sản phẩm tại thời điểm đặt hàng
    MaCodeSanPham NVARCHAR(50),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    ThanhTien DECIMAL(18,2) NOT NULL CHECK (ThanhTien >= 0),

    CONSTRAINT FK_ChiTietDonHang_DonHang FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang) ON DELETE CASCADE,
    CONSTRAINT FK_ChiTietDonHang_SanPham FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
)
GO

-- =============================================
-- 5. KHUYẾN MÃI & GIẢM GIÁ
-- =============================================

-- Bảng mã giảm giá
CREATE TABLE MaGiamGia
(
    MaMaGiamGia INT IDENTITY(1,1) PRIMARY KEY,
    MaCode NVARCHAR(50) NOT NULL UNIQUE,
    TenMaGiamGia NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255),

    -- Cài đặt giảm giá
    LoaiGiamGia NVARCHAR(20) NOT NULL CHECK (LoaiGiamGia IN ('PhanTram', 'SoTienCoDinh')),
    GiaTriGiamGia DECIMAL(18,2) NOT NULL CHECK (GiaTriGiamGia > 0),
    GiaTriDonHangToiThieu DECIMAL(18,2) DEFAULT 0,
    SoTienGiamToiDa DECIMAL(18,2),

    -- Giới hạn sử dụng
    SoLuotSuDungToiDa INT,
    SoLuotDaSuDung INT DEFAULT 0,
    SoLuotSuDungToiDaMoiNguoi INT DEFAULT 1,

    -- Thời gian hiệu lực
    NgayBatDau DATETIME2 NOT NULL,
    NgayKetThuc DATETIME2 NOT NULL,
    TrangThaiHoatDong BIT DEFAULT 1,

    NgayTao DATETIME2 DEFAULT GETDATE(),
    NguoiTao INT,

    CONSTRAINT FK_MaGiamGia_NguoiTao FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- Bảng sử dụng mã giảm giá
CREATE TABLE SuDungMaGiamGia
(
    MaSuDung INT IDENTITY(1,1) PRIMARY KEY,
    MaMaGiamGia INT NOT NULL,
    MaNguoiDung INT,
    MaDonHang INT NOT NULL,
    SoTienGiamGia DECIMAL(18,2) NOT NULL,
    NgaySuDung DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_SuDungMaGiamGia_MaGiamGia FOREIGN KEY (MaMaGiamGia) REFERENCES MaGiamGia(MaMaGiamGia),
    CONSTRAINT FK_SuDungMaGiamGia_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_SuDungMaGiamGia_DonHang FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang)
)
GO

-- =============================================
-- 6. TƯƠNG TÁC KHÁCH HÀNG
-- =============================================

-- Bảng đánh giá sản phẩm
CREATE TABLE DanhGiaSanPham
(
    MaDanhGia INT IDENTITY(1,1) PRIMARY KEY,
    MaSanPham INT NOT NULL,
    MaNguoiDung INT,
    MaDonHang INT,
    -- Đảm bảo người dùng đã mua sản phẩm
    DiemDanhGia INT NOT NULL CHECK (DiemDanhGia BETWEEN 1 AND 5),
    TieuDe NVARCHAR(255),
    NoiDungDanhGia NVARCHAR(MAX),
    LaMuaHangXacThuc BIT DEFAULT 0,
    DaDuyet BIT DEFAULT 0,
    NgayTao DATETIME2 DEFAULT GETDATE(),
    NgayDuyet DATETIME2,
    NguoiDuyet INT,

    CONSTRAINT FK_DanhGiaSanPham_SanPham FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham),
    CONSTRAINT FK_DanhGiaSanPham_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_DanhGiaSanPham_DonHang FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),
    CONSTRAINT FK_DanhGiaSanPham_NguoiDuyet FOREIGN KEY (NguoiDuyet) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- Bảng danh sách yêu thích
CREATE TABLE DanhSachYeuThich
(
    MaYeuThich INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaSanPham INT NOT NULL,
    NgayTao DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_DanhSachYeuThich_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung) ON DELETE CASCADE,
    CONSTRAINT FK_DanhSachYeuThich_SanPham FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham) ON DELETE CASCADE,
    CONSTRAINT UK_DanhSachYeuThich_NguoiDung_SanPham UNIQUE (MaNguoiDung, MaSanPham)
)
GO

-- Bảng giỏ hàng
CREATE TABLE GioHang
(
    MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT,
    MaPhienLamViec NVARCHAR(255),
    -- Cho khách vãng lai
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    NgayTao DATETIME2 DEFAULT GETDATE(),
    NgayCapNhatCuoi DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_GioHang_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung) ON DELETE CASCADE,
    CONSTRAINT FK_GioHang_SanPham FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham) ON DELETE CASCADE
)
GO

-- =============================================
-- 7. CÀI ĐẶT HỆ THỐNG & BLOG
-- =============================================

-- Bảng cài đặt hệ thống
CREATE TABLE CaiDatHeThong
(
    MaCaiDat INT IDENTITY(1,1) PRIMARY KEY,
    KhoaCaiDat NVARCHAR(100) NOT NULL UNIQUE,
    GiaTriCaiDat NVARCHAR(MAX),
    MoTa NVARCHAR(255),
    KieuDuLieu NVARCHAR(20) DEFAULT 'String',
    NgayCapNhatCuoi DATETIME2 DEFAULT GETDATE(),
    NguoiCapNhatCuoi INT,

    CONSTRAINT FK_CaiDatHeThong_NguoiCapNhat FOREIGN KEY (NguoiCapNhatCuoi) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- Bảng bài viết/tin tức
CREATE TABLE BaiViet
(
    MaBaiViet INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    DuongDanBaiViet NVARCHAR(255) NOT NULL UNIQUE,
    TomTat NVARCHAR(500),
    NoiDung NVARCHAR(MAX),
    HinhAnhDaiDien NVARCHAR(255),

    -- SEO
    TieuDeSEO NVARCHAR(255),
    MoTaSEO NVARCHAR(500),
    TuKhoaSEO NVARCHAR(255),

    -- Thông tin tác giả
    TacGia INT NOT NULL,
    NgayTao DATETIME2 DEFAULT GETDATE(),
    NgayCapNhatCuoi DATETIME2 DEFAULT GETDATE(),
    NgayXuatBan DATETIME2,

    -- Trạng thái
    TrangThai NVARCHAR(20) DEFAULT 'NhapBan' CHECK (TrangThai IN ('NhapBan', 'ChoXuatBan', 'XuatBan')),
    NoiBat BIT DEFAULT 0,
    LuotXem INT DEFAULT 0,

    CONSTRAINT FK_BaiViet_TacGia FOREIGN KEY (TacGia) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- Bảng bình luận bài viết
CREATE TABLE BinhLuanBaiViet
(
    MaBinhLuan INT IDENTITY(1,1) PRIMARY KEY,
    MaBaiViet INT NOT NULL,
    MaNguoiDung INT,
    TenNguoiBinhLuan NVARCHAR(100),
    -- Cho khách vãng lai
    EmailNguoiBinhLuan NVARCHAR(100),
    NoiDungBinhLuan NVARCHAR(1000) NOT NULL,
    MaBinhLuanCha INT,
    -- Cho reply
    TrangThai NVARCHAR(20) DEFAULT 'ChoDuyet' CHECK (TrangThai IN ('ChoDuyet', 'DaDuyet', 'BiTuChoi')),
    NgayTao DATETIME2 DEFAULT GETDATE(),
    NgayDuyet DATETIME2,
    NguoiDuyet INT,

    CONSTRAINT FK_BinhLuanBaiViet_BaiViet FOREIGN KEY (MaBaiViet) REFERENCES BaiViet(MaBaiViet) ON DELETE CASCADE,
    CONSTRAINT FK_BinhLuanBaiViet_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung),
    CONSTRAINT FK_BinhLuanBaiViet_BinhLuanCha FOREIGN KEY (MaBinhLuanCha) REFERENCES BinhLuanBaiViet(MaBinhLuan),
    CONSTRAINT FK_BinhLuanBaiViet_NguoiDuyet FOREIGN KEY (NguoiDuyet) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- Bảng nhật ký hệ thống
CREATE TABLE NhatKyHeThong
(
    MaNhatKy INT IDENTITY(1,1) PRIMARY KEY,
    TenBang NVARCHAR(100) NOT NULL,
    MaBanGhi INT NOT NULL,
    HanhDong NVARCHAR(20) NOT NULL CHECK (HanhDong IN ('THEM', 'SUA', 'XOA')),
    GiaTriCu NVARCHAR(MAX),
    GiaTriMoi NVARCHAR(MAX),
    MaNguoiDung INT,
    DiaChiIP NVARCHAR(45),
    ThongTinTrinhDuyet NVARCHAR(255),
    ThoiGian DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_NhatKyHeThong_NguoiDung FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- =============================================
-- 8. CHỈ MỤC ĐỂ TỐI ƯU HIỆU SUẤT
-- =============================================

-- Chỉ mục cho bảng NguoiDung
CREATE NONCLUSTERED INDEX IX_NguoiDung_Email ON NguoiDung(Email)
CREATE NONCLUSTERED INDEX IX_NguoiDung_TenDangNhap ON NguoiDung(TenDangNhap)
CREATE NONCLUSTERED INDEX IX_NguoiDung_MaVaiTro ON NguoiDung(MaVaiTro)
CREATE NONCLUSTERED INDEX IX_NguoiDung_TrangThaiHoatDong ON NguoiDung(TrangThaiHoatDong)

-- Chỉ mục cho bảng SanPham
CREATE NONCLUSTERED INDEX IX_SanPham_MaDanhMuc ON SanPham(MaDanhMuc)
CREATE NONCLUSTERED INDEX IX_SanPham_MaThuongHieu ON SanPham(MaThuongHieu)
CREATE NONCLUSTERED INDEX IX_SanPham_MaCodeSanPham ON SanPham(MaCodeSanPham)
CREATE NONCLUSTERED INDEX IX_SanPham_DuongDanSanPham ON SanPham(DuongDanSanPham)
CREATE NONCLUSTERED INDEX IX_SanPham_TrangThaiHoatDong_NoiBat ON SanPham(TrangThaiHoatDong, LaSanPhamNoiBat)
CREATE NONCLUSTERED INDEX IX_SanPham_GiaKhuyenMai ON SanPham(GiaKhuyenMai) WHERE GiaKhuyenMai IS NOT NULL
CREATE NONCLUSTERED INDEX IX_SanPham_NgayTao ON SanPham(NgayTao)

-- Chỉ mục cho bảng DonHang
CREATE NONCLUSTERED INDEX IX_DonHang_MaNguoiDung ON DonHang(MaNguoiDung)
CREATE NONCLUSTERED INDEX IX_DonHang_SoDonHang ON DonHang(SoDonHang)
CREATE NONCLUSTERED INDEX IX_DonHang_NgayDatHang ON DonHang(NgayDatHang)
CREATE NONCLUSTERED INDEX IX_DonHang_MaTrangThai ON DonHang(MaTrangThai)
CREATE NONCLUSTERED INDEX IX_DonHang_TrangThaiThanhToan ON DonHang(TrangThaiThanhToan)

-- Chỉ mục cho bảng ChiTietDonHang
CREATE NONCLUSTERED INDEX IX_ChiTietDonHang_MaDonHang ON ChiTietDonHang(MaDonHang)
CREATE NONCLUSTERED INDEX IX_ChiTietDonHang_MaSanPham ON ChiTietDonHang(MaSanPham)

-- Chỉ mục cho bảng DanhGiaSanPham
CREATE NONCLUSTERED INDEX IX_DanhGiaSanPham_MaSanPham ON DanhGiaSanPham(MaSanPham)
CREATE NONCLUSTERED INDEX IX_DanhGiaSanPham_MaNguoiDung ON DanhGiaSanPham(MaNguoiDung)
CREATE NONCLUSTERED INDEX IX_DanhGiaSanPham_DaDuyet ON DanhGiaSanPham(DaDuyet)

-- Chỉ mục cho bảng BaiViet
CREATE NONCLUSTERED INDEX IX_BaiViet_DuongDanBaiViet ON BaiViet(DuongDanBaiViet)
CREATE NONCLUSTERED INDEX IX_BaiViet_TrangThai ON BaiViet(TrangThai)
CREATE NONCLUSTERED INDEX IX_BaiViet_NgayXuatBan ON BaiViet(NgayXuatBan)

-- =============================================
-- 9. THÊM DỮ LIỆU BAN ĐẦU
-- =============================================

-- Thêm vai trò người dùng
INSERT INTO VaiTroNguoiDung
    (TenVaiTro, MoTa)
VALUES
    ('QuanTriVien', 'Quản trị viên cấp cao với toàn quyền'),
    ('QuanLy', 'Quản lý với hầu hết các quyền'),
    ('NhanVien', 'Nhân viên với quyền hạn giới hạn'),
    ('KhachHang', 'Khách hàng thường xuyên'),
    ('KhachVangLai', 'Khách vãng lai')
GO

-- Thêm trạng thái đơn hàng
INSERT INTO TrangThaiDonHang
    (TenTrangThai, MoTa, ThuTuSapXep)
VALUES
    ('ChoDuyet', 'Đơn hàng đã đặt, chờ duyệt', 1),
    ('DangXuLy', 'Đơn hàng đang được xử lý', 2),
    ('DongGoi', 'Đơn hàng đã được đóng gói', 3),
    ('DangGiao', 'Đơn hàng đang được giao', 4),
    ('DaGiao', 'Đơn hàng đã được giao thành công', 5),
    ('DaHuy', 'Đơn hàng đã bị hủy', 6),
    ('HoanTra', 'Đơn hàng đã được hoàn trả', 7)
GO

-- Thêm phương thức thanh toán
INSERT INTO PhuongThucThanhToan
    (TenPhuongThuc, MoTa)
VALUES
    ('TienMat', 'Thanh toán tiền mặt khi nhận hàng'),
    ('ChuyenKhoan', 'Chuyển khoản ngân hàng'),
    ('TheTinDung', 'Thanh toán bằng thẻ tín dụng'),
    ('ViDienTu', 'Thanh toán qua ví điện tử MoMo, ZaloPay'),
    ('TraGop', 'Thanh toán trả góp')
GO

-- Thêm phương thức vận chuyển
INSERT INTO PhuongThucVanChuyen
    (TenPhuongThuc, MoTa, PhiCoBan, SoNgayDuKien)
VALUES
    ('VanChuyenTieuChuan', 'Giao hàng tiêu chuẩn', 30000, 3),
    ('VanChuyenNhanh', 'Giao hàng nhanh trong ngày', 60000, 1),
    ('MienPhiVanChuyen', 'Miễn phí vận chuyển cho đơn hàng trên 1 triệu', 0, 5),
    ('VanChuyenHoaToc', 'Giao hàng hỏa tốc 2-4 giờ', 100000, 0)
GO

-- Thêm danh mục sản phẩm
INSERT INTO DanhMucSanPham
    (TenDanhMuc, DuongDanDanhMuc, MoTa, ThuTuSapXep)
VALUES
    ('Ban Billiard', 'ban-billiard', 'Các loại bàn billiard chuyên nghiệp và giải trí', 1),
    ('Co Billiard', 'co-billiard', 'Cơ billiard cao cấp và phổ thông', 2),
    ('Bi Billiard', 'bi-billiard', 'Bộ bi billiard chính hãng', 3),
    ('Phu Kien', 'phu-kien', 'Phụ kiện hỗ trợ chơi billiard', 4),
    ('Phan Co', 'phan-co', 'Phấn cơ billiard các loại', 5),
    ('Do Bao Ve', 'do-bao-ve', 'Đồ bảo vệ và phụ kiện an toàn', 6)
GO

-- Thêm danh mục con
INSERT INTO DanhMucSanPham
    (TenDanhMuc, DuongDanDanhMuc, MoTa, MaDanhMucCha, ThuTuSapXep)
VALUES
    ('Ban Pool 8 Bi', 'ban-pool-8-bi', 'Bàn billiard Pool 8 bi', 1, 1),
    ('Ban Carom 3 Bi', 'ban-carom-3-bi', 'Bàn billiard Carom 3 bi', 1, 2),
    ('Ban Snooker', 'ban-snooker', 'Bàn billiard Snooker', 1, 3),
    ('Co 1 Manh', 'co-1-manh', 'Cơ billiard nguyên khối', 2, 1),
    ('Co 2 Manh', 'co-2-manh', 'Cơ billiard 2 mảnh có thể tháo rời', 2, 2),
    ('Co 3 Manh', 'co-3-manh', 'Cơ billiard 3 mảnh', 2, 3),
    ('Gia Co', 'gia-co', 'Giá để cơ billiard', 4, 1),
    ('Nhung Ban', 'nhung-ban', 'Nỉ nhung bàn billiard', 4, 2),
    ('Den Chieu Sang', 'den-chieu-sang', 'Đèn chiếu sáng bàn billiard', 4, 3)
GO

-- Thêm thương hiệu
INSERT INTO ThuongHieu
    (TenThuongHieu, DuongDanThuongHieu, MoTa, QuocGia)
VALUES
    ('Mezz', 'mezz', 'Thương hiệu cơ billiard cao cấp từ Nhật Bản', 'Nhật Bản'),
    ('Predator', 'predator', 'Thương hiệu cơ billiard nổi tiếng từ Mỹ', 'Mỹ'),
    ('McDermott', 'mcdermott', 'Cơ billiard thủ công cao cấp', 'Mỹ'),
    ('Brunswick', 'brunswick', 'Thương hiệu bàn billiard lâu đời', 'Mỹ'),
    ('Diamond', 'diamond', 'Bàn billiard thi đấu chuyên nghiệp', 'Mỹ'),
    ('Kamui', 'kamui', 'Phụ kiện billiard cao cấp', 'Nhật Bản'),
    ('Cuetec', 'cuetec', 'Cơ billiard chất lượng cao', 'Mỹ'),
    ('Aramith', 'aramith', 'Bi billiard chất lượng cao', 'Bỉ'),
    ('Simonis', 'simonis', 'Nỉ bàn billiard cao cấp', 'Bỉ'),
    ('Vietnam Billiard', 'vietnam-billiard', 'Thương hiệu billiard Việt Nam', 'Việt Nam')
GO

-- Thêm cài đặt hệ thống
INSERT INTO CaiDatHeThong
    (KhoaCaiDat, GiaTriCaiDat, MoTa, KieuDuLieu)
VALUES
    ('TenWebsite', 'Cửa Hàng Billiard Việt Nam', 'Tên website', 'String'),
    ('MoTaWebsite', 'Cửa hàng billiard uy tín #1 Việt Nam', 'Mô tả website', 'String'),
    ('DonViTienTe', 'VND', 'Đơn vị tiền tệ', 'String'),
    ('ThueSuat', '10', 'Thuế VAT (%)', 'Decimal'),
    ('MienPhiVanChuyenTu', '1000000', 'Miễn phí vận chuyển từ giá trị đơn hàng', 'Decimal'),
    ('TienToMaDonHang', 'DH', 'Tiền tố mã đơn hàng', 'String'),
    ('SoSanPhamMoiTrang', '20', 'Số sản phẩm hiển thị mỗi trang', 'Integer'),
    ('SoLuongToiDaGioHang', '100', 'Số lượng sản phẩm tối đa trong giỏ hàng', 'Integer'),
    ('EmailLienHe', 'info@billiardstore.vn', 'Email liên hệ', 'String'),
    ('SoDienThoaiLienHe', '0123456789', 'Số điện thoại liên hệ', 'String'),
    ('DiaChiCuaHang', '123 Đường ABC, Quận 1, TP.HCM', 'Địa chỉ cửa hàng', 'String')
GO

-- =============================================
-- 10. THỦ TỤC LƯU TRỮ (STORED PROCEDURES)
-- =============================================

-- Thủ tục: Lấy chi tiết sản phẩm với điểm đánh giá trung bình
CREATE PROCEDURE sp_LayChiTietSanPham
    @MaSanPham INT = NULL,
    @DuongDanSanPham NVARCHAR(255) = NULL
AS
BEGIN
    SELECT
        sp.*,
        dm.TenDanhMuc,
        dm.DuongDanDanhMuc,
        th.TenThuongHieu,
        th.DuongDanThuongHieu,

        -- Điểm đánh giá trung bình
        ISNULL(AVG(CAST(dg.DiemDanhGia AS DECIMAL(3,2))), 0) AS DiemDanhGiaTrungBinh,
        COUNT(dg.MaDanhGia) AS SoLuongDanhGia,

        -- Hình ảnh chính
        (SELECT TOP 1
            DuongDanHinhAnh
        FROM HinhAnhSanPham ha
        WHERE ha.MaSanPham = sp.MaSanPham AND ha.LaHinhAnhChinh = 1) AS HinhAnhChinh

    FROM SanPham sp
        INNER JOIN DanhMucSanPham dm ON sp.MaDanhMuc = dm.MaDanhMuc
        LEFT JOIN ThuongHieu th ON sp.MaThuongHieu = th.MaThuongHieu
        LEFT JOIN DanhGiaSanPham dg ON sp.MaSanPham = dg.MaSanPham AND dg.DaDuyet = 1
    WHERE 
        sp.TrangThaiHoatDong = 1
        AND (@MaSanPham IS NULL OR sp.MaSanPham = @MaSanPham)
        AND (@DuongDanSanPham IS NULL OR sp.DuongDanSanPham = @DuongDanSanPham)
    GROUP BY 
        sp.MaSanPham, sp.MaCodeSanPham, sp.TenSanPham, sp.DuongDanSanPham,
        sp.MoTaNgan, sp.MaDanhMuc, sp.MaThuongHieu,
        sp.GiaGoc, sp.GiaKhuyenMai, sp.GiaVon, sp.SoLuongTonKho,
        sp.SoLuongToiThieu, sp.SoLuongToiDa, sp.TrongLuong, sp.KichThuoc,
        sp.ChatLieu, sp.MauSac, sp.KichCo, sp.TieuDeSEO, sp.MoTaSEO,
        sp.TuKhoaSEO, sp.TrangThaiHoatDong, sp.LaSanPhamNoiBat, sp.LaSanPhamKhuyenMai,
        sp.NgayTao, sp.NgayCapNhatCuoi, sp.NguoiTaoMaSanPham,
        dm.TenDanhMuc, dm.DuongDanDanhMuc, th.TenThuongHieu, th.DuongDanThuongHieu, sp.MoTaChiTiet
END
GO

-- Thủ tục: Thêm sản phẩm vào giỏ hàng
CREATE PROCEDURE sp_ThemVaoGioHang
    @MaNguoiDung INT = NULL,
    @MaPhienLamViec NVARCHAR(255) = NULL,
    @MaSanPham INT,
    @SoLuong INT
AS
BEGIN
    DECLARE @SoLuongHienTai INT = 0
    DECLARE @SoLuongTonKho INT

    -- Kiểm tra tồn kho sản phẩm
    SELECT @SoLuongTonKho = SoLuongTonKho
    FROM SanPham
    WHERE MaSanPham = @MaSanPham AND TrangThaiHoatDong = 1

    IF @SoLuongTonKho IS NULL
    BEGIN
        RAISERROR('Sản phẩm không tồn tại hoặc đã ngưng hoạt động', 16, 1)
        RETURN
    END

    -- Kiểm tra sản phẩm đã có trong giỏ hàng chưa
    SELECT @SoLuongHienTai = ISNULL(SoLuong, 0)
    FROM GioHang
    WHERE MaSanPham = @MaSanPham
        AND ((@MaNguoiDung IS NOT NULL AND MaNguoiDung = @MaNguoiDung)
        OR (@MaPhienLamViec IS NOT NULL AND MaPhienLamViec = @MaPhienLamViec))

    -- Kiểm tra tổng số lượng có vượt quá tồn kho không
    IF (@SoLuongHienTai + @SoLuong) > @SoLuongTonKho
    BEGIN
        RAISERROR('Số lượng yêu cầu vượt quá tồn kho', 16, 1)
        RETURN
    END

    -- Cập nhật hoặc thêm mới vào giỏ hàng
    IF @SoLuongHienTai > 0
    BEGIN
        UPDATE GioHang 
        SET SoLuong = SoLuong + @SoLuong,
            NgayCapNhatCuoi = GETDATE()
        WHERE MaSanPham = @MaSanPham
            AND ((@MaNguoiDung IS NOT NULL AND MaNguoiDung = @MaNguoiDung)
            OR (@MaPhienLamViec IS NOT NULL AND MaPhienLamViec = @MaPhienLamViec))
    END
    ELSE
    BEGIN
        INSERT INTO GioHang
            (MaNguoiDung, MaPhienLamViec, MaSanPham, SoLuong)
        VALUES
            (@MaNguoiDung, @MaPhienLamViec, @MaSanPham, @SoLuong)
    END

    SELECT 'Thành công' AS KetQua
END
GO

-- Thủ tục: Tạo đơn hàng
CREATE PROCEDURE sp_TaoDonHang
    @MaNguoiDung INT = NULL,
    @EmailKhachHang NVARCHAR(100) = NULL,
    @SoDienThoaiKhachHang NVARCHAR(20) = NULL,
    @TenKhachHang NVARCHAR(100) = NULL,
    @DiaChiGiaoHang NVARCHAR(500),
    @DiaChiThanhToan NVARCHAR(500) = NULL,
    @MaPhuongThucThanhToan INT,
    @MaPhuongThucVanChuyen INT,
    @MaCodeGiamGia NVARCHAR(50) = NULL,
    @GhiChuKhachHang NVARCHAR(500) = NULL,
    @MaPhienLamViec NVARCHAR(255) = NULL,
    @MaDonHang INT OUTPUT
AS
BEGIN
    BEGIN TRANSACTION

    DECLARE @SoDonHang NVARCHAR(20)
    DECLARE @TongTienHang DECIMAL(18,2) = 0
    DECLARE @TienGiamGia DECIMAL(18,2) = 0
    DECLARE @PhiVanChuyen DECIMAL(18,2) = 0
    DECLARE @TongThanhToan DECIMAL(18,2) = 0
    DECLARE @MaMaGiamGia INT = NULL

    -- Tạo số đơn hàng
    DECLARE @SoDemDonHang INT
    SELECT @SoDemDonHang = COUNT(*)
    FROM DonHang
    SET @SoDonHang = 'DH' + RIGHT('00000' + CAST(@SoDemDonHang + 1 AS VARCHAR(5)), 5)

    -- Tính phí vận chuyển
    SELECT @PhiVanChuyen = PhiCoBan
    FROM PhuongThucVanChuyen
    WHERE MaPhuongThucVanChuyen = @MaPhuongThucVanChuyen

    -- Kiểm tra và tính giảm giá
    IF @MaCodeGiamGia IS NOT NULL
    BEGIN
        SELECT @MaMaGiamGia = MaMaGiamGia, @TienGiamGia = GiaTriGiamGia
        FROM MaGiamGia
        WHERE MaCode = @MaCodeGiamGia
            AND TrangThaiHoatDong = 1
            AND NgayBatDau <= GETDATE()
            AND NgayKetThuc >= GETDATE()
            AND (SoLuotSuDungToiDa IS NULL OR SoLuotDaSuDung < SoLuotSuDungToiDa)

        IF @MaMaGiamGia IS NULL
        BEGIN
            ROLLBACK TRANSACTION
            RAISERROR('Mã giảm giá không hợp lệ hoặc đã hết hạn', 16, 1)
            RETURN
        END
    END

    -- Tạo đơn hàng
    INSERT INTO DonHang
        (
        SoDonHang, MaNguoiDung, EmailKhachHang, SoDienThoaiKhachHang, TenKhachHang,
        TongTienHang, TienGiamGia, PhiVanChuyen, TongThanhToan,
        DiaChiGiaoHang, DiaChiThanhToan, MaTrangThai, MaPhuongThucThanhToan,
        MaPhuongThucVanChuyen, GhiChuKhachHang
        )
    VALUES
        (
            @SoDonHang, @MaNguoiDung, @EmailKhachHang, @SoDienThoaiKhachHang, @TenKhachHang,
            0, @TienGiamGia, @PhiVanChuyen, 0,
            @DiaChiGiaoHang, ISNULL(@DiaChiThanhToan, @DiaChiGiaoHang), 1,
            @MaPhuongThucThanhToan, @MaPhuongThucVanChuyen, @GhiChuKhachHang
    )

    SET @MaDonHang = SCOPE_IDENTITY()

    -- Thêm chi tiết đơn hàng từ giỏ hàng
    INSERT INTO ChiTietDonHang
        (MaDonHang, MaSanPham, TenSanPham, MaCodeSanPham, SoLuong, DonGia, ThanhTien)
    SELECT
        @MaDonHang,
        sp.MaSanPham,
        sp.TenSanPham,
        sp.MaCodeSanPham,
        gh.SoLuong,
        ISNULL(sp.GiaKhuyenMai, sp.GiaGoc) AS DonGia,
        gh.SoLuong * ISNULL(sp.GiaKhuyenMai, sp.GiaGoc) AS ThanhTien
    FROM GioHang gh
        INNER JOIN SanPham sp ON gh.MaSanPham = sp.MaSanPham
    WHERE ((@MaNguoiDung IS NOT NULL AND gh.MaNguoiDung = @MaNguoiDung)
        OR (@MaPhienLamViec IS NOT NULL AND gh.MaPhienLamViec = @MaPhienLamViec))

    -- Tính tổng tiền hàng
    SELECT @TongTienHang = SUM(ThanhTien)
    FROM ChiTietDonHang
    WHERE MaDonHang = @MaDonHang

    -- Áp dụng mã giảm giá
    IF @MaMaGiamGia IS NOT NULL
    BEGIN
        DECLARE @LoaiGiamGia NVARCHAR(20), @GiaTriGiamGia DECIMAL(18,2), @SoTienGiamToiDa DECIMAL(18,2)
        SELECT @LoaiGiamGia = LoaiGiamGia, @GiaTriGiamGia = GiaTriGiamGia, @SoTienGiamToiDa = SoTienGiamToiDa
        FROM MaGiamGia
        WHERE MaMaGiamGia = @MaMaGiamGia

        IF @LoaiGiamGia = 'PhanTram'
        BEGIN
            SET @TienGiamGia = @TongTienHang * (@GiaTriGiamGia / 100)
            IF @SoTienGiamToiDa IS NOT NULL AND @TienGiamGia > @SoTienGiamToiDa
                SET @TienGiamGia = @SoTienGiamToiDa
        END
        ELSE
        BEGIN
            SET @TienGiamGia = @GiaTriGiamGia
        END

        -- Ghi nhận sử dụng mã giảm giá
        INSERT INTO SuDungMaGiamGia
            (MaMaGiamGia, MaNguoiDung, MaDonHang, SoTienGiamGia)
        VALUES
            (@MaMaGiamGia, @MaNguoiDung, @MaDonHang, @TienGiamGia)

        -- Cập nhật số lượt đã sử dụng
        UPDATE MaGiamGia SET SoLuotDaSuDung = SoLuotDaSuDung + 1 WHERE MaMaGiamGia = @MaMaGiamGia
    END

    -- Tính tổng thanh toán
    SET @TongThanhToan = @TongTienHang - @TienGiamGia + @PhiVanChuyen

    -- Cập nhật tổng tiền đơn hàng
    UPDATE DonHang 
    SET TongTienHang = @TongTienHang,
        TienGiamGia = @TienGiamGia,
        TongThanhToan = @TongThanhToan
    WHERE MaDonHang = @MaDonHang

    -- Cập nhật tồn kho và tạo biến động kho
    UPDATE sp 
    SET SoLuongTonKho = sp.SoLuongTonKho - ct.SoLuong
    FROM SanPham sp
        INNER JOIN ChiTietDonHang ct ON sp.MaSanPham = ct.MaSanPham
    WHERE ct.MaDonHang = @MaDonHang

    INSERT INTO BienDongKhoHang
        (MaSanPham, LoaiBienDong, SoLuong, TonKhoTruoc, TonKhoSau, ThamChieu)
    SELECT
        sp.MaSanPham,
        'XUAT',
        ct.SoLuong,
        sp.SoLuongTonKho + ct.SoLuong,
        sp.SoLuongTonKho,
        'Đơn hàng #' + @SoDonHang
    FROM SanPham sp
        INNER JOIN ChiTietDonHang ct ON sp.MaSanPham = ct.MaSanPham
    WHERE ct.MaDonHang = @MaDonHang

    -- Xóa giỏ hàng
    DELETE FROM GioHang 
    WHERE ((@MaNguoiDung IS NOT NULL AND MaNguoiDung = @MaNguoiDung)
        OR (@MaPhienLamViec IS NOT NULL AND MaPhienLamViec = @MaPhienLamViec))

    COMMIT TRANSACTION

    SELECT @MaDonHang AS MaDonHang, @SoDonHang AS SoDonHang, @TongThanhToan AS TongThanhToan
END
GO

-- =============================================
-- 11. VIEW (CHẾ ĐỘ XEM)
-- =============================================

-- View: Danh sách sản phẩm với thông tin đầy đủ
CREATE VIEW vw_DanhSachSanPham
AS
    SELECT
        sp.MaSanPham,
        sp.MaCodeSanPham,
        sp.TenSanPham,
        sp.DuongDanSanPham,
        sp.MoTaNgan,
        sp.GiaGoc,
        sp.GiaKhuyenMai,
        sp.SoLuongTonKho,
        sp.LaSanPhamNoiBat,
        sp.LaSanPhamKhuyenMai,
        sp.NgayTao,

        dm.TenDanhMuc,
        dm.DuongDanDanhMuc,
        th.TenThuongHieu,
        th.DuongDanThuongHieu,

        -- Hình ảnh chính
        (SELECT TOP 1
            DuongDanHinhAnh
        FROM HinhAnhSanPham ha
        WHERE ha.MaSanPham = sp.MaSanPham AND ha.LaHinhAnhChinh = 1) AS HinhAnhChinh,

        -- Điểm đánh giá trung bình
        ISNULL(AVG(CAST(dg.DiemDanhGia AS DECIMAL(3,2))), 0) AS DiemDanhGiaTrungBinh,
        COUNT(dg.MaDanhGia) AS SoLuongDanhGia

    FROM SanPham sp
        INNER JOIN DanhMucSanPham dm ON sp.MaDanhMuc = dm.MaDanhMuc
        LEFT JOIN ThuongHieu th ON sp.MaThuongHieu = th.MaThuongHieu
        LEFT JOIN DanhGiaSanPham dg ON sp.MaSanPham = dg.MaSanPham AND dg.DaDuyet = 1
    WHERE sp.TrangThaiHoatDong = 1
    GROUP BY 
    sp.MaSanPham, sp.MaCodeSanPham, sp.TenSanPham, sp.DuongDanSanPham,
    sp.MoTaNgan, sp.GiaGoc, sp.GiaKhuyenMai, sp.SoLuongTonKho,
    sp.LaSanPhamNoiBat, sp.LaSanPhamKhuyenMai, sp.NgayTao,
    dm.TenDanhMuc, dm.DuongDanDanhMuc, th.TenThuongHieu, th.DuongDanThuongHieu
GO

-- View: Tóm tắt đơn hàng
CREATE VIEW vw_TomTatDonHang
AS
    SELECT
        dh.MaDonHang,
        dh.SoDonHang,
        dh.NgayDatHang,
        dh.TongThanhToan,
        dh.TrangThaiThanhToan,
        tt.TenTrangThai,

        -- Thông tin khách hàng
        CASE 
        WHEN dh.MaNguoiDung IS NOT NULL THEN nd.Ho + ' ' + nd.Ten
        ELSE dh.TenKhachHang
    END AS TenKhachHang,

        CASE 
        WHEN dh.MaNguoiDung IS NOT NULL THEN nd.Email
        ELSE dh.EmailKhachHang
    END AS EmailKhachHang,

        -- Số lượng sản phẩm
        (SELECT COUNT(*)
        FROM ChiTietDonHang ct
        WHERE ct.MaDonHang = dh.MaDonHang) AS SoLuongSanPham,

        ptt.TenPhuongThuc AS PhuongThucThanhToan,
        pvc.TenPhuongThuc AS PhuongThucVanChuyen

    FROM DonHang dh
        INNER JOIN TrangThaiDonHang tt ON dh.MaTrangThai = tt.MaTrangThai
        LEFT JOIN NguoiDung nd ON dh.MaNguoiDung = nd.MaNguoiDung
        LEFT JOIN PhuongThucThanhToan ptt ON dh.MaPhuongThucThanhToan = ptt.MaPhuongThucThanhToan
        LEFT JOIN PhuongThucVanChuyen pvc ON dh.MaPhuongThucVanChuyen = pvc.MaPhuongThucVanChuyen
GO

-- =============================================
-- 12. HÀM TÍNH TOÁN (FUNCTIONS)
-- =============================================

-- Hàm: Lấy giá cuối cùng của sản phẩm
CREATE FUNCTION fn_LayGiaCuoiCungSanPham(@MaSanPham INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @GiaCuoiCung DECIMAL(18,2)

    SELECT @GiaCuoiCung = ISNULL(GiaKhuyenMai, GiaGoc)
    FROM SanPham
    WHERE MaSanPham = @MaSanPham

    RETURN ISNULL(@GiaCuoiCung, 0)
END
GO

-- Hàm: Kiểm tra tồn kho
CREATE FUNCTION fn_KiemTraTonKho(@MaSanPham INT, @SoLuong INT)
RETURNS BIT
AS
BEGIN
    DECLARE @SoLuongTonKho INT
    DECLARE @CoSanHang BIT = 0

    SELECT @SoLuongTonKho = SoLuongTonKho
    FROM SanPham
    WHERE MaSanPham = @MaSanPham AND TrangThaiHoatDong = 1

    IF @SoLuongTonKho >= @SoLuong
        SET @CoSanHang = 1

    RETURN @CoSanHang
END
GO

-- =============================================
-- 13. TRIGGERS (TRIGGERS)
-- =============================================

-- Trigger: Cập nhật ngày sửa đổi cuối cho sản phẩm
CREATE TRIGGER tr_SanPham_CapNhatNgayChinhSua
ON SanPham
AFTER UPDATE
AS
BEGIN
    UPDATE SanPham 
    SET NgayCapNhatCuoi = GETDATE()
    WHERE MaSanPham IN (SELECT MaSanPham
    FROM inserted)
END
GO

-- Trigger: Nhật ký hệ thống cho đơn hàng
CREATE TRIGGER tr_DonHang_NhatKyHeThong
ON DonHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @HanhDong NVARCHAR(20)

    IF EXISTS(SELECT *
        FROM inserted) AND EXISTS(SELECT *
        FROM deleted)
        SET @HanhDong = 'SUA'
    ELSE IF EXISTS(SELECT *
    FROM inserted)
        SET @HanhDong = 'THEM'
    ELSE
        SET @HanhDong = 'XOA'

    INSERT INTO NhatKyHeThong
        (TenBang, MaBanGhi, HanhDong, GiaTriMoi, GiaTriCu)
    SELECT
        'DonHang',
        ISNULL(i.MaDonHang, d.MaDonHang),
        @HanhDong,
        (SELECT *
        FROM inserted i2
        WHERE i2.MaDonHang = ISNULL(i.MaDonHang, d.MaDonHang)
        FOR JSON AUTO),
        (SELECT *
        FROM deleted d2
        WHERE d2.MaDonHang = ISNULL(i.MaDonHang, d.MaDonHang)
        FOR JSON AUTO)
    FROM inserted i
        FULL OUTER JOIN deleted d ON i.MaDonHang = d.MaDonHang
END
GO

-- =============================================
-- 14. THÊM SẢN PHẨM MẪU
-- =============================================

-- Tạo tài khoản admin mặc định
INSERT INTO NguoiDung
    (TenDangNhap, Email, MatKhauMaHoa, MuoiMatKhau, Ho, Ten, MaVaiTro)
VALUES
    ('admin', 'admin@billiardstore.vn', 'hashedpassword123', 'salt123', 'Quản', 'Trị', 1)
GO

-- Thêm một số sản phẩm mẫu
DECLARE @AdminID INT = 1

INSERT INTO SanPham
    (MaCodeSanPham, TenSanPham, DuongDanSanPham, MoTaNgan, MoTaChiTiet, MaDanhMuc, MaThuongHieu, GiaGoc, GiaKhuyenMai, SoLuongTonKho, TrongLuong, KichThuoc, ChatLieu, MauSac, LaSanPhamNoiBat, NguoiTaoMaSanPham)
VALUES
    ('CO-MEZZ-EC7-1', 'Cơ Mezz EC7-1', 'co-mezz-ec7-1', 'Cơ billiard Mezz EC7-1 cao cấp', 'Cơ billiard Mezz EC7-1 được làm từ gỗ Maple chất lượng cao, thiết kế tinh xảo, phù hợp cho các tay chơi chuyên nghiệp.', 4, 1, 12000000, 10800000, 5, 0.52, '147cm x 13mm', 'Gỗ Maple', 'Nâu', 1, @AdminID),
    ('CO-PREDATOR-P3', 'Cơ Predator P3', 'co-predator-p3', 'Cơ billiard Predator P3 chuyên nghiệp', 'Cơ billiard Predator P3 với công nghệ Revo shaft tiên tiến, mang lại độ chính xác cao và cảm giác tuyệt vời.', 4, 2, 15000000, 13500000, 3, 0.53, '147cm x 12.75mm', 'Gỗ Maple + Carbon', 'Đen', 1, @AdminID),
    ('BAN-DIAMOND-PRO', 'Bàn Diamond Pro', 'ban-diamond-pro', 'Bàn billiard Diamond Pro thi đấu', 'Bàn billiard Diamond Pro chuẩn thi đấu quốc tế, sử dụng trong các giải đấu chuyên nghiệp.', 7, 5, 180000000, NULL, 2, 500, '254cm x 127cm x 80cm', 'Gỗ cứng + Slate', 'Xanh lá', 1, @AdminID),
    ('BI-ARAMITH-TOURNAMENT', 'Bộ bi Aramith Tournament', 'bi-aramith-tournament', 'Bộ bi Aramith Tournament chuyên nghiệp', 'Bộ bi Aramith Tournament chất lượng cao, được sử dụng trong các giải đấu chuyên nghiệp.', 3, 8, 8500000, 7650000, 10, 1.2, 'Đường kính 57.2mm', 'Phenolic resin', 'Đa màu', 0, @AdminID),
    ('PHAN-KAMUI-CLEAR', 'Phấn Kamui Clear', 'phan-kamui-clear', 'Phấn cơ Kamui Clear cao cấp', 'Phấn cơ Kamui Clear giúp tăng độ ma sát và kiểm soát bi tốt hơn.', 5, 6, 450000, 405000, 50, 0.01, '14mm x 8mm', 'Da tự nhiên', 'Nâu', 0, @AdminID)
GO

-- Thêm hình ảnh sản phẩm mẫu
INSERT INTO HinhAnhSanPham
    (MaSanPham, DuongDanHinhAnh, TextThayThe, LaHinhAnhChinh, ThuTuSapXep)
VALUES
    (1, '/images/co-mezz-ec7-1-main.jpg', 'Cơ Mezz EC7-1 - Hình chính', 1, 1),
    (1, '/images/co-mezz-ec7-1-detail.jpg', 'Cơ Mezz EC7-1 - Chi tiết', 0, 2),
    (2, '/images/co-predator-p3-main.jpg', 'Cơ Predator P3 - Hình chính', 1, 1),
    (3, '/images/ban-diamond-pro-main.jpg', 'Bàn Diamond Pro - Hình chính', 1, 1),
    (4, '/images/bi-aramith-tournament-main.jpg', 'Bộ bi Aramith Tournament - Hình chính', 1, 1),
    (5, '/images/phan-kamui-clear-main.jpg', 'Phấn Kamui Clear - Hình chính', 1, 1)
GO

PRINT 'Cơ sở dữ liệu đã được tạo thành công!'
PRINT 'Sẵn sàng cho ứng dụng Cửa hàng Billiard!'
GO


