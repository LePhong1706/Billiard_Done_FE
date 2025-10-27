using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IDiaChiNguoiDungRepository : IGenericRepository<DiaChiNguoiDung>
{
    /// <summary>
    /// Lấy tất cả địa chỉ của một người dùng
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách địa chỉ của người dùng</returns>
    Task<IEnumerable<DiaChiNguoiDung>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy địa chỉ theo loại của một người dùng
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="loaiDiaChi">Loại địa chỉ (GiaoHang, ThanhToan, CaHai)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách địa chỉ theo loại</returns>
    Task<IEnumerable<DiaChiNguoiDung>> GetByUserAndTypeAsync(int nguoiDungId, string loaiDiaChi, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy địa chỉ mặc định của người dùng
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="loaiDiaChi">Loại địa chỉ (null để lấy bất kỳ loại nào)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Địa chỉ mặc định hoặc null</returns>
    Task<DiaChiNguoiDung?> GetDefaultAddressAsync(int nguoiDungId, string? loaiDiaChi = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy địa chỉ giao hàng mặc định
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Địa chỉ giao hàng mặc định</returns>
    Task<DiaChiNguoiDung?> GetDefaultShippingAddressAsync(int nguoiDungId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy địa chỉ thanh toán mặc định
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Địa chỉ thanh toán mặc định</returns>
    Task<DiaChiNguoiDung?> GetDefaultBillingAddressAsync(int nguoiDungId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đặt một địa chỉ làm mặc định và bỏ mặc định các địa chỉ khác
    /// </summary>
    /// <param name="diaChiId">ID địa chỉ muốn đặt mặc định</param>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="loaiDiaChi">Loại địa chỉ (null để áp dụng cho tất cả)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu thành công</returns>
    Task<bool> SetDefaultAddressAsync(int diaChiId, int nguoiDungId, string? loaiDiaChi = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bỏ đặt mặc định tất cả địa chỉ của người dùng theo loại
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="loaiDiaChi">Loại địa chỉ (null để áp dụng cho tất cả)</param>
    /// <param name="excludeAddressId">ID địa chỉ cần loại trừ</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu thành công</returns>
    Task<bool> UnsetDefaultAddressesAsync(int nguoiDungId, string? loaiDiaChi = null, int? excludeAddressId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số địa chỉ của một người dùng
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="loaiDiaChi">Loại địa chỉ (null để đếm tất cả)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Số lượng địa chỉ</returns>
    Task<int> CountByUserAsync(int nguoiDungId, string? loaiDiaChi = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra người dùng có địa chỉ nào không
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="loaiDiaChi">Loại địa chỉ (null để kiểm tra tất cả)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu có địa chỉ</returns>
    Task<bool> HasAddressAsync(int nguoiDungId, string? loaiDiaChi = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy địa chỉ theo thành phố/tỉnh
    /// </summary>
    /// <param name="tinhThanhPho">Tên tỉnh thành phố</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách địa chỉ theo tỉnh thành</returns>
    Task<IEnumerable<DiaChiNguoiDung>> GetByProvinceAsync(string tinhThanhPho, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa tất cả địa chỉ của một người dùng
    /// </summary>
    /// <param name="nguoiDungId">ID người dùng</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Số lượng địa chỉ đã xóa</returns>
    Task<int> DeleteAllByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật thông tin người nhận cho địa chỉ
    /// </summary>
    /// <param name="diaChiId">ID địa chỉ</param>
    /// <param name="hoTenNguoiNhan">Họ tên người nhận</param>
    /// <param name="soDienThoaiNguoiNhan">Số điện thoại người nhận</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu cập nhật thành công</returns>
    Task<bool> UpdateRecipientInfoAsync(int diaChiId, string? hoTenNguoiNhan, string? soDienThoaiNguoiNhan, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tìm kiếm địa chỉ theo nhiều tiêu chí
    /// </summary>
    /// <param name="searchTerm">Từ khóa tìm kiếm</param>
    /// <param name="nguoiDungId">ID người dùng (null để tìm tất cả)</param>
    /// <param name="loaiDiaChi">Loại địa chỉ</param>
    /// <param name="tinhThanhPho">Tỉnh thành phố</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách địa chỉ phù hợp</returns>
    Task<IEnumerable<DiaChiNguoiDung>> SearchAddressesAsync(
        string? searchTerm = null,
        int? nguoiDungId = null,
        string? loaiDiaChi = null,
        string? tinhThanhPho = null,
        CancellationToken cancellationToken = default);
}