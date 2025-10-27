using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface ICaiDatHeThongRepository : IGenericRepository<CaiDatHeThong>
{
    /// <summary>
    /// Lấy cài đặt theo khóa
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Cài đặt hệ thống hoặc null nếu không tìm thấy</returns>
    Task<CaiDatHeThong?> GetByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy giá trị cài đặt theo khóa
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Giá trị cài đặt hoặc giá trị mặc định</returns>
    Task<string?> GetValueAsync(string khoaCaiDat, string? defaultValue = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy giá trị cài đặt dạng int
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="defaultValue">Giá trị mặc định</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Giá trị int hoặc giá trị mặc định</returns>
    Task<int> GetIntValueAsync(string khoaCaiDat, int defaultValue = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy giá trị cài đặt dạng decimal
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="defaultValue">Giá trị mặc định</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Giá trị decimal hoặc giá trị mặc định</returns>
    Task<decimal> GetDecimalValueAsync(string khoaCaiDat, decimal defaultValue = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy giá trị cài đặt dạng bool
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="defaultValue">Giá trị mặc định</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Giá trị bool hoặc giá trị mặc định</returns>
    Task<bool> GetBoolValueAsync(string khoaCaiDat, bool defaultValue = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật hoặc tạo mới cài đặt
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="giaTriCaiDat">Giá trị cài đặt</param>
    /// <param name="moTa">Mô tả cài đặt</param>
    /// <param name="kieuDuLieu">Kiểu dữ liệu</param>
    /// <param name="nguoiCapNhat">ID người cập nhật</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu thành công</returns>
    Task<bool> SetValueAsync(
        string khoaCaiDat, 
        string? giaTriCaiDat, 
        string? moTa = null, 
        string kieuDuLieu = "String", 
        int? nguoiCapNhat = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật nhiều cài đặt cùng lúc
    /// </summary>
    /// <param name="caiDats">Dictionary chứa các cặp key-value cần cập nhật</param>
    /// <param name="nguoiCapNhat">ID người cập nhật</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Số lượng cài đặt đã cập nhật</returns>
    Task<int> SetMultipleValuesAsync(
        Dictionary<string, string?> caiDats, 
        int? nguoiCapNhat = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra khóa cài đặt có tồn tại không
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu tồn tại</returns>
    Task<bool> ExistsByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy tất cả cài đặt theo nhóm (prefix)
    /// </summary>
    /// <param name="prefix">Tiền tố khóa cài đặt</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách cài đặt có khóa bắt đầu bằng prefix</returns>
    Task<IEnumerable<CaiDatHeThong>> GetByPrefixAsync(string prefix, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy tất cả cài đặt theo kiểu dữ liệu
    /// </summary>
    /// <param name="kieuDuLieu">Kiểu dữ liệu (String, Integer, Decimal, Boolean)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách cài đặt theo kiểu dữ liệu</returns>
    Task<IEnumerable<CaiDatHeThong>> GetByDataTypeAsync(string kieuDuLieu, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa cài đặt theo khóa
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu xóa thành công</returns>
    Task<bool> DeleteByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy tất cả cài đặt dạng Dictionary để cache
    /// </summary>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Dictionary chứa tất cả cài đặt</returns>
    Task<Dictionary<string, string?>> GetAllAsDictionaryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Tìm kiếm cài đặt theo từ khóa
    /// </summary>
    /// <param name="searchTerm">Từ khóa tìm kiếm (trong khóa hoặc mô tả)</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Danh sách cài đặt phù hợp</returns>
    Task<IEnumerable<CaiDatHeThong>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reset cài đặt về giá trị mặc định
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="defaultValue">Giá trị mặc định</param>
    /// <param name="nguoiCapNhat">ID người cập nhật</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>True nếu thành công</returns>
    Task<bool> ResetToDefaultAsync(
        string khoaCaiDat, 
        string defaultValue, 
        int? nguoiCapNhat = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lịch sử thay đổi của một cài đặt
    /// </summary>
    /// <param name="khoaCaiDat">Khóa cài đặt</param>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="cancellationToken">Token để hủy operation</param>
    /// <returns>Lịch sử thay đổi</returns>
    Task<IEnumerable<CaiDatHeThong>> GetHistoryAsync(
        string khoaCaiDat, 
        DateTime? fromDate = null, 
        DateTime? toDate = null, 
        CancellationToken cancellationToken = default);
}