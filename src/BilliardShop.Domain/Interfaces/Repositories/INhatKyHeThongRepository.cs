using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface INhatKyHeThongRepository : IGenericRepository<NhatKyHeThong>
{
    /// <summary>
    /// Ghi log hoạt động hệ thống
    /// </summary>
    /// <param name="tenBang">Tên bảng</param>
    /// <param name="maBanGhi">ID bản ghi</param>
    /// <param name="hanhDong">Hành động (THEM, SUA, XOA)</param>
    /// <param name="giaTriCu">Giá trị cũ (JSON)</param>
    /// <param name="giaTriMoi">Giá trị mới (JSON)</param>
    /// <param name="maNguoiDung">ID người dùng thực hiện</param>
    /// <param name="diaChiIP">Địa chỉ IP</param>
    /// <param name="thongTinTrinhDuyet">Thông tin trình duyệt</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>NhatKyHeThong đã tạo</returns>
    Task<NhatKyHeThong> LogAsync(
        string tenBang,
        int maBanGhi,
        string hanhDong,
        string? giaTriCu = null,
        string? giaTriMoi = null,
        int? maNguoiDung = null,
        string? diaChiIP = null,
        string? thongTinTrinhDuyet = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ghi log thêm mới
    /// </summary>
    /// <param name="tenBang">Tên bảng</param>
    /// <param name="maBanGhi">ID bản ghi</param>
    /// <param name="giaTriMoi">Giá trị mới (JSON)</param>
    /// <param name="maNguoiDung">ID người dùng</param>
    /// <param name="diaChiIP">Địa chỉ IP</param>
    /// <param name="thongTinTrinhDuyet">Thông tin trình duyệt</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>NhatKyHeThong đã tạo</returns>
    Task<NhatKyHeThong> LogCreateAsync(
        string tenBang,
        int maBanGhi,
        string? giaTriMoi = null,
        int? maNguoiDung = null,
        string? diaChiIP = null,
        string? thongTinTrinhDuyet = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ghi log cập nhật
    /// </summary>
    /// <param name="tenBang">Tên bảng</param>
    /// <param name="maBanGhi">ID bản ghi</param>
    /// <param name="giaTriCu">Giá trị cũ (JSON)</param>
    /// <param name="giaTriMoi">Giá trị mới (JSON)</param>
    /// <param name="maNguoiDung">ID người dùng</param>
    /// <param name="diaChiIP">Địa chỉ IP</param>
    /// <param name="thongTinTrinhDuyet">Thông tin trình duyệt</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>NhatKyHeThong đã tạo</returns>
    Task<NhatKyHeThong> LogUpdateAsync(
        string tenBang,
        int maBanGhi,
        string? giaTriCu = null,
        string? giaTriMoi = null,
        int? maNguoiDung = null,
        string? diaChiIP = null,
        string? thongTinTrinhDuyet = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ghi log xóa
    /// </summary>
    /// <param name="tenBang">Tên bảng</param>
    /// <param name="maBanGhi">ID bản ghi</param>
    /// <param name="giaTriCu">Giá trị cũ (JSON)</param>
    /// <param name="maNguoiDung">ID người dùng</param>
    /// <param name="diaChiIP">Địa chỉ IP</param>
    /// <param name="thongTinTrinhDuyet">Thông tin trình duyệt</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>NhatKyHeThong đã tạo</returns>
    Task<NhatKyHeThong> LogDeleteAsync(
        string tenBang,
        int maBanGhi,
        string? giaTriCu = null,
        int? maNguoiDung = null,
        string? diaChiIP = null,
        string? thongTinTrinhDuyet = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy nhật ký theo bảng và ID bản ghi
    /// </summary>
    /// <param name="tenBang">Tên bảng</param>
    /// <param name="maBanGhi">ID bản ghi</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách nhật ký</returns>
    Task<IEnumerable<NhatKyHeThong>> GetByTableAndRecordAsync(
        string tenBang,
        int maBanGhi,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy nhật ký theo người dùng
    /// </summary>
    /// <param name="maNguoiDung">ID người dùng</param>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách nhật ký của người dùng</returns>
    Task<IEnumerable<NhatKyHeThong>> GetByUserAsync(
        int maNguoiDung,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy nhật ký theo bảng
    /// </summary>
    /// <param name="tenBang">Tên bảng</param>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách nhật ký của bảng</returns>
    Task<IEnumerable<NhatKyHeThong>> GetByTableAsync(
        string tenBang,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy nhật ký theo hành động
    /// </summary>
    /// <param name="hanhDong">Hành động (THEM, SUA, XOA)</param>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách nhật ký theo hành động</returns>
    Task<IEnumerable<NhatKyHeThong>> GetByActionAsync(
        string hanhDong,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy nhật ký với phân trang và lọc
    /// </summary>
    /// <param name="pageNumber">Số trang</param>
    /// <param name="pageSize">Kích thước trang</param>
    /// <param name="tenBang">Tên bảng (tùy chọn)</param>
    /// <param name="hanhDong">Hành động (tùy chọn)</param>
    /// <param name="maNguoiDung">ID người dùng (tùy chọn)</param>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách nhật ký với phân trang</returns>
    Task<(IEnumerable<NhatKyHeThong> Logs, int TotalCount)> GetPagedLogsAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? tenBang = null,
        string? hanhDong = null,
        int? maNguoiDung = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số lượng log theo bảng
    /// </summary>
    /// <param name="tenBang">Tên bảng</param>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Số lượng log</returns>
    Task<int> CountByTableAsync(
        string tenBang,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số lượng log theo người dùng
    /// </summary>
    /// <param name="maNguoiDung">ID người dùng</param>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Số lượng log</returns>
    Task<int> CountByUserAsync(
        int maNguoiDung,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thống kê hoạt động theo ngày
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Thống kê hoạt động theo ngày</returns>
    Task<IEnumerable<(DateTime Date, int Count)>> GetDailyActivityStatsAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thống kê hoạt động theo bảng
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Thống kê hoạt động theo bảng</returns>
    Task<IEnumerable<(string TableName, int Count)>> GetTableActivityStatsAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thống kê hoạt động theo người dùng
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="top">Số lượng top user</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Thống kê hoạt động theo người dùng</returns>
    Task<IEnumerable<(int UserId, int Count)>> GetUserActivityStatsAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int top = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa log cũ hơn số ngày chỉ định
    /// </summary>
    /// <param name="daysToKeep">Số ngày giữ lại</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Số lượng log đã xóa</returns>
    Task<int> CleanupOldLogsAsync(int daysToKeep = 90, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tìm kiếm log theo từ khóa
    /// </summary>
    /// <param name="searchTerm">Từ khóa tìm kiếm</param>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách log phù hợp</returns>
    Task<IEnumerable<NhatKyHeThong>> SearchLogsAsync(
        string searchTerm,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);
}