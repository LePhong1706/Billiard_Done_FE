using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilliardShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMucSanPham",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DuongDanDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaDanhMucCha = table.Column<int>(type: "int", nullable: true),
                    HinhAnhDaiDien = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThuTuSapXep = table.Column<int>(type: "int", nullable: false),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucSanPham", x => x.MaDanhMuc);
                    table.ForeignKey(
                        name: "FK_DanhMucSanPham_DanhMucCha",
                        column: x => x.MaDanhMucCha,
                        principalTable: "DanhMucSanPham",
                        principalColumn: "MaDanhMuc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaGiamGia",
                columns: table => new
                {
                    MaMaGiamGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenMaGiamGia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LoaiGiamGia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GiaTriGiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaTriDonHangToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoTienGiamToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoLuotSuDungToiDa = table.Column<int>(type: "int", nullable: true),
                    SoLuotDaSuDung = table.Column<int>(type: "int", nullable: false),
                    SoLuotSuDungToiDaMoiNguoi = table.Column<int>(type: "int", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaGiamGia", x => x.MaMaGiamGia);
                    table.CheckConstraint("CK_MaGiamGia_GiaTriGiamGia", "[GiaTriGiamGia] > 0");
                    table.CheckConstraint("CK_MaGiamGia_LoaiGiamGia", "[LoaiGiamGia] IN ('PhanTram', 'SoTienCoDinh')");
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NguoiLienHe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuocGia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCap", x => x.MaNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToan",
                columns: table => new
                {
                    MaPhuongThucThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhuongThuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToan", x => x.MaPhuongThucThanhToan);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucVanChuyen",
                columns: table => new
                {
                    MaPhuongThucVanChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhuongThuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhiCoBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhiTheoTrongLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoNgayDuKien = table.Column<int>(type: "int", nullable: true),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucVanChuyen", x => x.MaPhuongThucVanChuyen);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DuongDanThuongHieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LogoThuongHieu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    QuocGia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieu", x => x.MaThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "TrangThaiDonHang",
                columns: table => new
                {
                    MaTrangThai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThuTuSapXep = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThaiDonHang", x => x.MaTrangThai);
                });

            migrationBuilder.CreateTable(
                name: "VaiTroNguoiDung",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTroNguoiDung", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhauMaHoa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MuoiMatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Ho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MaVaiTro = table.Column<int>(type: "int", nullable: false),
                    DaXacThucEmail = table.Column<bool>(type: "bit", nullable: false),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false),
                    LanDangNhapCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.MaNguoiDung);
                    table.CheckConstraint("CK_NguoiDung_GioiTinh", "[GioiTinh] IN ('M', 'F', 'K')");
                    table.ForeignKey(
                        name: "FK_NguoiDung_VaiTroNguoiDung",
                        column: x => x.MaVaiTro,
                        principalTable: "VaiTroNguoiDung",
                        principalColumn: "MaVaiTro",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaiViet",
                columns: table => new
                {
                    MaBaiViet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DuongDanBaiViet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TomTat = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnhDaiDien = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TieuDeSEO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTaSEO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TuKhoaSEO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TacGia = table.Column<int>(type: "int", nullable: false),
                    NgayXuatBan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NoiBat = table.Column<bool>(type: "bit", nullable: false),
                    LuotXem = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaiViet", x => x.MaBaiViet);
                    table.CheckConstraint("CK_BaiViet_TrangThai", "[TrangThai] IN ('NhapBan', 'ChoXuatBan', 'XuatBan')");
                    table.ForeignKey(
                        name: "FK_BaiViet_TacGia",
                        column: x => x.TacGia,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaiDatHeThong",
                columns: table => new
                {
                    MaCaiDat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KhoaCaiDat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiaTriCaiDat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    KieuDuLieu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaiDatHeThong", x => x.MaCaiDat);
                    table.ForeignKey(
                        name: "FK_CaiDatHeThong_NguoiCapNhat",
                        column: x => x.NguoiCapNhatCuoi,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DiaChiNguoiDung",
                columns: table => new
                {
                    MaDiaChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    LoaiDiaChi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTenNguoiNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoDienThoaiNguoiNhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhuongXa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuanHuyen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TinhThanhPho = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaBuuDien = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LaDiaChiMacDinh = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaChiNguoiDung", x => x.MaDiaChi);
                    table.CheckConstraint("CK_DiaChiNguoiDung_LoaiDiaChi", "[LoaiDiaChi] IN ('GiaoHang', 'ThanhToan', 'CaHai')");
                    table.ForeignKey(
                        name: "FK_DiaChiNguoiDung_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoDonHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    EmailKhachHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoDienThoaiKhachHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TenKhachHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TongTienHang = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienGiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhiVanChuyen = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienThue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongThanhToan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiaChiThanhToan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaTrangThai = table.Column<int>(type: "int", nullable: false),
                    MaPhuongThucThanhToan = table.Column<int>(type: "int", nullable: true),
                    MaPhuongThucVanChuyen = table.Column<int>(type: "int", nullable: true),
                    TrangThaiThanhToan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayDatHang = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayYeuCauGiao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayGiaoHang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayNhanHang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GhiChuKhachHang = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GhiChuQuanTri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.MaDonHang);
                    table.CheckConstraint("CK_DonHang_TrangThaiThanhToan", "[TrangThaiThanhToan] IN ('ChoThanhToan', 'DaThanhToan', 'ThatBai', 'HoanTien')");
                    table.ForeignKey(
                        name: "FK_DonHang_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DonHang_PhuongThucThanhToan",
                        column: x => x.MaPhuongThucThanhToan,
                        principalTable: "PhuongThucThanhToan",
                        principalColumn: "MaPhuongThucThanhToan",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DonHang_PhuongThucVanChuyen",
                        column: x => x.MaPhuongThucVanChuyen,
                        principalTable: "PhuongThucVanChuyen",
                        principalColumn: "MaPhuongThucVanChuyen",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DonHang_TrangThaiDonHang",
                        column: x => x.MaTrangThai,
                        principalTable: "TrangThaiDonHang",
                        principalColumn: "MaTrangThai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyHeThong",
                columns: table => new
                {
                    MaNhatKy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaBanGhi = table.Column<int>(type: "int", nullable: false),
                    HanhDong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GiaTriCu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiaTriMoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    DiaChiIP = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ThongTinTrinhDuyet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyHeThong", x => x.MaNhatKy);
                    table.CheckConstraint("CK_NhatKyHeThong_HanhDong", "[HanhDong] IN ('THEM', 'SUA', 'XOA')");
                    table.ForeignKey(
                        name: "FK_NhatKyHeThong_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaCodeSanPham = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DuongDanSanPham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MoTaNgan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MoTaChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false),
                    MaThuongHieu = table.Column<int>(type: "int", nullable: true),
                    GiaGoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaKhuyenMai = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiaVon = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoLuongTonKho = table.Column<int>(type: "int", nullable: false),
                    SoLuongToiThieu = table.Column<int>(type: "int", nullable: false),
                    SoLuongToiDa = table.Column<int>(type: "int", nullable: true),
                    TrongLuong = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    KichThuoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChatLieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MauSac = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KichCo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TieuDeSEO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTaSEO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TuKhoaSEO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false),
                    LaSanPhamNoiBat = table.Column<bool>(type: "bit", nullable: false),
                    LaSanPhamKhuyenMai = table.Column<bool>(type: "bit", nullable: false),
                    NguoiTaoMaSanPham = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.MaSanPham);
                    table.CheckConstraint("CK_SanPham_GiaGoc", "[GiaGoc] >= 0");
                    table.CheckConstraint("CK_SanPham_GiaKhuyenMai", "[GiaKhuyenMai] >= 0");
                    table.CheckConstraint("CK_SanPham_GiaVon", "[GiaVon] >= 0");
                    table.CheckConstraint("CK_SanPham_SoLuongTonKho", "[SoLuongTonKho] >= 0");
                    table.ForeignKey(
                        name: "FK_SanPham_DanhMucSanPham",
                        column: x => x.MaDanhMuc,
                        principalTable: "DanhMucSanPham",
                        principalColumn: "MaDanhMuc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SanPham_NguoiTao",
                        column: x => x.NguoiTaoMaSanPham,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SanPham_ThuongHieu",
                        column: x => x.MaThuongHieu,
                        principalTable: "ThuongHieu",
                        principalColumn: "MaThuongHieu",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BinhLuanBaiViet",
                columns: table => new
                {
                    MaBinhLuan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBaiViet = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    TenNguoiBinhLuan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailNguoiBinhLuan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NoiDungBinhLuan = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MaBinhLuanCha = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayDuyet = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiDuyet = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinhLuanBaiViet", x => x.MaBinhLuan);
                    table.CheckConstraint("CK_BinhLuanBaiViet_TrangThai", "[TrangThai] IN ('ChoDuyet', 'DaDuyet', 'BiTuChoi')");
                    table.ForeignKey(
                        name: "FK_BinhLuanBaiViet_BaiViet",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViet",
                        principalColumn: "MaBaiViet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BinhLuanBaiViet_BinhLuanCha",
                        column: x => x.MaBinhLuanCha,
                        principalTable: "BinhLuanBaiViet",
                        principalColumn: "MaBinhLuan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BinhLuanBaiViet_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BinhLuanBaiViet_NguoiDuyet",
                        column: x => x.NguoiDuyet,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SuDungMaGiamGia",
                columns: table => new
                {
                    MaSuDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaMaGiamGia = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    SoTienGiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgaySuDung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuDungMaGiamGia", x => x.MaSuDung);
                    table.ForeignKey(
                        name: "FK_SuDungMaGiamGia_DonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuDungMaGiamGia_MaGiamGia",
                        column: x => x.MaMaGiamGia,
                        principalTable: "MaGiamGia",
                        principalColumn: "MaMaGiamGia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuDungMaGiamGia_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BienDongKhoHang",
                columns: table => new
                {
                    MaBienDong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    LoaiBienDong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TonKhoTruoc = table.Column<int>(type: "int", nullable: false),
                    TonKhoSau = table.Column<int>(type: "int", nullable: false),
                    ThamChieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NguoiThucHien = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienDongKhoHang", x => x.MaBienDong);
                    table.CheckConstraint("CK_BienDongKhoHang_LoaiBienDong", "[LoaiBienDong] IN ('NHAP', 'XUAT', 'DIEU_CHINH')");
                    table.ForeignKey(
                        name: "FK_BienDongKhoHang_NguoiDung",
                        column: x => x.NguoiThucHien,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BienDongKhoHang_SanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    MaChiTietDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaCodeSanPham = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => x.MaChiTietDonHang);
                    table.CheckConstraint("CK_ChiTietDonHang_DonGia", "[DonGia] >= 0");
                    table.CheckConstraint("CK_ChiTietDonHang_SoLuong", "[SoLuong] > 0");
                    table.CheckConstraint("CK_ChiTietDonHang_ThanhTien", "[ThanhTien] >= 0");
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_SanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanhGiaSanPham",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    MaDonHang = table.Column<int>(type: "int", nullable: true),
                    DiemDanhGia = table.Column<int>(type: "int", nullable: false),
                    TieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NoiDungDanhGia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaMuaHangXacThuc = table.Column<bool>(type: "bit", nullable: false),
                    DaDuyet = table.Column<bool>(type: "bit", nullable: false),
                    NgayDuyet = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiDuyet = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaSanPham", x => x.MaDanhGia);
                    table.CheckConstraint("CK_DanhGiaSanPham_DiemDanhGia", "[DiemDanhGia] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPham_DonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPham_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPham_NguoiDuyet",
                        column: x => x.NguoiDuyet,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPham_SanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanhSachYeuThich",
                columns: table => new
                {
                    MaYeuThich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhSachYeuThich", x => x.MaYeuThich);
                    table.ForeignKey(
                        name: "FK_DanhSachYeuThich_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhSachYeuThich_SanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    MaPhienLamViec = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.MaGioHang);
                    table.CheckConstraint("CK_GioHang_SoLuong", "[SoLuong] > 0");
                    table.ForeignKey(
                        name: "FK_GioHang_NguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHang_SanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhSanPham",
                columns: table => new
                {
                    MaHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    DuongDanHinhAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TextThayThe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThuTuSapXep = table.Column<int>(type: "int", nullable: false),
                    LaHinhAnhChinh = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NguoiTao = table.Column<int>(type: "int", nullable: true),
                    NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhSanPham", x => x.MaHinhAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhSanPham_SanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThuocTinhSanPham",
                columns: table => new
                {
                    MaThuocTinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    TenThuocTinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiaTriThuocTinh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuocTinhSanPham", x => x.MaThuocTinh);
                    table.ForeignKey(
                        name: "FK_ThuocTinhSanPham_SanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaiViet_DuongDanBaiViet",
                table: "BaiViet",
                column: "DuongDanBaiViet",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaiViet_TacGia",
                table: "BaiViet",
                column: "TacGia");

            migrationBuilder.CreateIndex(
                name: "IX_BienDongKhoHang_MaSanPham",
                table: "BienDongKhoHang",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_BienDongKhoHang_NguoiThucHien",
                table: "BienDongKhoHang",
                column: "NguoiThucHien");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuanBaiViet_MaBaiViet",
                table: "BinhLuanBaiViet",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuanBaiViet_MaBinhLuanCha",
                table: "BinhLuanBaiViet",
                column: "MaBinhLuanCha");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuanBaiViet_MaNguoiDung",
                table: "BinhLuanBaiViet",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuanBaiViet_NguoiDuyet",
                table: "BinhLuanBaiViet",
                column: "NguoiDuyet");

            migrationBuilder.CreateIndex(
                name: "IX_CaiDatHeThong_KhoaCaiDat",
                table: "CaiDatHeThong",
                column: "KhoaCaiDat",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaiDatHeThong_NguoiCapNhatCuoi",
                table: "CaiDatHeThong",
                column: "NguoiCapNhatCuoi");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaDonHang",
                table: "ChiTietDonHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaSanPham",
                table: "ChiTietDonHang",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPham_MaDonHang",
                table: "DanhGiaSanPham",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPham_MaNguoiDung",
                table: "DanhGiaSanPham",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPham_MaSanPham",
                table: "DanhGiaSanPham",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPham_NguoiDuyet",
                table: "DanhGiaSanPham",
                column: "NguoiDuyet");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucSanPham_DuongDanDanhMuc",
                table: "DanhMucSanPham",
                column: "DuongDanDanhMuc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucSanPham_MaDanhMucCha",
                table: "DanhMucSanPham",
                column: "MaDanhMucCha");

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachYeuThich_MaSanPham",
                table: "DanhSachYeuThich",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "UK_DanhSachYeuThich_NguoiDung_SanPham",
                table: "DanhSachYeuThich",
                columns: new[] { "MaNguoiDung", "MaSanPham" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiaChiNguoiDung_MaNguoiDung",
                table: "DiaChiNguoiDung",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaNguoiDung",
                table: "DonHang",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaPhuongThucThanhToan",
                table: "DonHang",
                column: "MaPhuongThucThanhToan");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaPhuongThucVanChuyen",
                table: "DonHang",
                column: "MaPhuongThucVanChuyen");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaTrangThai",
                table: "DonHang",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_SoDonHang",
                table: "DonHang",
                column: "SoDonHang",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_MaNguoiDung",
                table: "GioHang",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_MaSanPham",
                table: "GioHang",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPham_MaSanPham",
                table: "HinhAnhSanPham",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_MaGiamGia_MaCode",
                table: "MaGiamGia",
                column: "MaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_Email",
                table: "NguoiDung",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_MaVaiTro",
                table: "NguoiDung",
                column: "MaVaiTro");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_TenDangNhap",
                table: "NguoiDung",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyHeThong_MaNguoiDung",
                table: "NhatKyHeThong",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_DuongDanSanPham",
                table: "SanPham",
                column: "DuongDanSanPham",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaCodeSanPham",
                table: "SanPham",
                column: "MaCodeSanPham",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaDanhMuc",
                table: "SanPham",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaThuongHieu",
                table: "SanPham",
                column: "MaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_NguoiTaoMaSanPham",
                table: "SanPham",
                column: "NguoiTaoMaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SuDungMaGiamGia_MaDonHang",
                table: "SuDungMaGiamGia",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_SuDungMaGiamGia_MaMaGiamGia",
                table: "SuDungMaGiamGia",
                column: "MaMaGiamGia");

            migrationBuilder.CreateIndex(
                name: "IX_SuDungMaGiamGia_MaNguoiDung",
                table: "SuDungMaGiamGia",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_ThuocTinhSanPham_MaSanPham",
                table: "ThuocTinhSanPham",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ThuongHieu_DuongDanThuongHieu",
                table: "ThuongHieu",
                column: "DuongDanThuongHieu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThuongHieu_TenThuongHieu",
                table: "ThuongHieu",
                column: "TenThuongHieu",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BienDongKhoHang");

            migrationBuilder.DropTable(
                name: "BinhLuanBaiViet");

            migrationBuilder.DropTable(
                name: "CaiDatHeThong");

            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "DanhGiaSanPham");

            migrationBuilder.DropTable(
                name: "DanhSachYeuThich");

            migrationBuilder.DropTable(
                name: "DiaChiNguoiDung");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "HinhAnhSanPham");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "NhatKyHeThong");

            migrationBuilder.DropTable(
                name: "SuDungMaGiamGia");

            migrationBuilder.DropTable(
                name: "ThuocTinhSanPham");

            migrationBuilder.DropTable(
                name: "BaiViet");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "MaGiamGia");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToan");

            migrationBuilder.DropTable(
                name: "PhuongThucVanChuyen");

            migrationBuilder.DropTable(
                name: "TrangThaiDonHang");

            migrationBuilder.DropTable(
                name: "DanhMucSanPham");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "ThuongHieu");

            migrationBuilder.DropTable(
                name: "VaiTroNguoiDung");
        }
    }
}
