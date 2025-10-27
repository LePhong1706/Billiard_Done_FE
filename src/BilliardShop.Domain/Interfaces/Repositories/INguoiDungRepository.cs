using System.Linq.Expressions;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface INguoiDungRepository : IGenericRepository<NguoiDung>
{
    /// <summary>
    /// Lấy người dùng theo email
    /// </summary>
    /// <param name="email">Email của người dùng</param>
    /// <param name="includeVaiTro">Có load thông tin vai trò không</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Người dùng hoặc null nếu không tìm thấy</returns>
    Task<NguoiDung?> GetByEmailAsync(string email, bool includeVaiTro = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy người dùng theo tên đăng nhập
    /// </summary>
    /// <param name="tenDangNhap">Tên đăng nhập</param>
    /// <param name="includeVaiTro">Có load thông tin vai trò không</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Người dùng hoặc null nếu không tìm thấy</returns>
    Task<NguoiDung?> GetByTenDangNhapAsync(string tenDangNhap, bool includeVaiTro = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy người dùng theo email hoặc tên đăng nhập (dùng cho đăng nhập)
    /// </summary>
    /// <param name="emailOrUsername">Email hoặc tên đăng nhập</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Người dùng hoặc null nếu không tìm thấy</returns>
    Task<NguoiDung?> GetByEmailOrUsernameAsync(string emailOrUsername, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra email có tồn tại không
    /// </summary>
    /// <param name="email">Email cần kiểm tra</param>
    /// <param name="excludeUserId">ID người dùng cần loại trừ (dùng khi update)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu email đã tồn tại</returns>
    Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra tên đăng nhập có tồn tại không
    /// </summary>
    /// <param name="tenDangNhap">Tên đăng nhập cần kiểm tra</param>
    /// <param name="excludeUserId">ID người dùng cần loại trừ (dùng khi update)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu tên đăng nhập đã tồn tại</returns>
    Task<bool> IsUsernameExistsAsync(string tenDangNhap, int? excludeUserId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật thời gian đăng nhập cuối
    /// </summary>
    /// <param name="userId">ID người dùng</param>
    /// <param name="lastLoginTime">Thời gian đăng nhập</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu cập nhật thành công</returns>
    Task<bool> UpdateLastLoginAsync(int userId, DateTime lastLoginTime, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật trạng thái xác thực email
    /// </summary>
    /// <param name="userId">ID người dùng</param>
    /// <param name="daXacThuc">Trạng thái xác thực</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu cập nhật thành công</returns>
    Task<bool> UpdateEmailVerificationStatusAsync(int userId, bool daXacThuc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật mật khẩu người dùng
    /// </summary>
    /// <param name="userId">ID người dùng</param>
    /// <param name="matKhauMaHoa">Mật khẩu đã mã hóa</param>
    /// <param name="muoiMatKhau">Salt của mật khẩu</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu cập nhật thành công</returns>
    Task<bool> UpdatePasswordAsync(int userId, string matKhauMaHoa, string muoiMatKhau, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách người dùng theo vai trò
    /// </summary>
    /// <param name="vaiTroId">ID vai trò</param>
    /// <param name="activeOnly">Chỉ lấy người dùng đang hoạt động</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách người dùng</returns>
    Task<IEnumerable<NguoiDung>> GetUsersByRoleAsync(int vaiTroId, bool activeOnly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tìm kiếm người dùng với phân trang
    /// </summary>
    /// <param name="searchTerm">Từ khóa tìm kiếm (tên, email, số điện thoại)</param>
    /// <param name="vaiTroId">ID vai trò (null để không lọc theo vai trò)</param>
    /// <param name="trangThaiHoatDong">Trạng thái hoạt động (null để không lọc)</param>
    /// <param name="pageNumber">Số trang</param>
    /// <param name="pageSize">Kích thước trang</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách người dùng và tổng số</returns>
    Task<(IEnumerable<NguoiDung> Users, int TotalCount)> SearchUsersAsync(
        string? searchTerm = null,
        int? vaiTroId = null,
        bool? trangThaiHoatDong = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy người dùng với đầy đủ thông tin liên quan
    /// </summary>
    /// <param name="userId">ID người dùng</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Người dùng với thông tin vai trò và địa chỉ</returns>
    Task<NguoiDung?> GetUserWithDetailsAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số người dùng mới trong khoảng thời gian
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Số người dùng mới</returns>
    Task<int> CountNewUsersAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy người dùng đăng nhập gần đây
    /// </summary>
    /// <param name="days">Số ngày gần đây</param>
    /// <param name="take">Số lượng lấy tối đa</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách người dùng đăng nhập gần đây</returns>
    Task<IEnumerable<NguoiDung>> GetRecentActiveUsersAsync(int days = 30, int take = 100, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật trạng thái hoạt động của người dùng
    /// </summary>
    /// <param name="userId">ID người dùng</param>
    /// <param name="trangThaiHoatDong">Trạng thái mới</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu cập nhật thành công</returns>
    Task<bool> UpdateActiveStatusAsync(int userId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);
}