using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IVaiTroNguoiDungRepository : IGenericRepository<VaiTroNguoiDung>
{
    /// <summary>
    /// Lấy vai trò theo tên vai trò
    /// </summary>
    /// <param name="tenVaiTro">Tên vai trò cần tìm</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Vai trò người dùng hoặc null nếu không tìm thấy</returns>
    Task<VaiTroNguoiDung?> GetByTenVaiTroAsync(string tenVaiTro, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy tất cả vai trò đang hoạt động
    /// </summary>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách vai trò đang hoạt động</returns>
    Task<IEnumerable<VaiTroNguoiDung>> GetActiveRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra vai trò có tồn tại theo tên không
    /// </summary>
    /// <param name="tenVaiTro">Tên vai trò cần kiểm tra</param>
    /// <param name="excludeId">ID vai trò cần loại trừ (dùng khi update)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu tồn tại, false nếu không</returns>
    Task<bool> ExistsByTenVaiTroAsync(string tenVaiTro, int? excludeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số người dùng theo vai trò
    /// </summary>
    /// <param name="vaiTroId">ID vai trò</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Số lượng người dùng có vai trò này</returns>
    Task<int> CountUsersByRoleAsync(int vaiTroId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật trạng thái hoạt động của vai trò
    /// </summary>
    /// <param name="vaiTroId">ID vai trò</param>
    /// <param name="trangThaiHoatDong">Trạng thái mới</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu cập nhật thành công</returns>
    Task<bool> UpdateActiveStatusAsync(int vaiTroId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy vai trò với thống kê số người dùng
    /// </summary>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách vai trò kèm số lượng người dùng</returns>
    Task<IEnumerable<(VaiTroNguoiDung VaiTro, int SoLuongNguoiDung)>> GetRolesWithUserCountAsync(CancellationToken cancellationToken = default);
}