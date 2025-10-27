using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;

namespace BilliardShop.Application.Interfaces.Services;

public interface ICaiDatHeThongService : IReadOnlyService<CaiDatHeThongDto>
{
    Task<ServiceResult<CaiDatHeThongDto>> CreateOrUpdateAsync(CaiDatHeThongDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default);
    Task<CaiDatHeThongDto?> GetByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default);
    Task<string?> GetValueAsync(string khoaCaiDat, string? defaultValue = null, CancellationToken cancellationToken = default);
    Task<int> GetIntValueAsync(string khoaCaiDat, int defaultValue = 0, CancellationToken cancellationToken = default);
    Task<decimal> GetDecimalValueAsync(string khoaCaiDat, decimal defaultValue = 0, CancellationToken cancellationToken = default);
    Task<bool> GetBoolValueAsync(string khoaCaiDat, bool defaultValue = false, CancellationToken cancellationToken = default);
    Task<ServiceResult> SetValueAsync(string khoaCaiDat, string? giaTriCaiDat, string? moTa = null, string kieuDuLieu = "String", int? nguoiCapNhat = null, CancellationToken cancellationToken = default);
    Task<ServiceResult<int>> SetMultipleValuesAsync(Dictionary<string, string?> caiDats, int? nguoiCapNhat = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<CaiDatHeThongDto>> GetByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
    Task<IEnumerable<CaiDatHeThongDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Dictionary<string, string?>> GetAllAsDictionaryAsync(CancellationToken cancellationToken = default);
}