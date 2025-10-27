using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IThuocTinhSanPhamRepository : IGenericRepository<ThuocTinhSanPham>
{
    Task<IEnumerable<ThuocTinhSanPham>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<ThuocTinhSanPham?> GetByProductAndNameAsync(int sanPhamId, string tenThuocTinh, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuocTinhSanPham>> GetByAttributeNameAsync(string tenThuocTinh, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUniqueAttributeNamesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUniqueAttributeValuesAsync(string tenThuocTinh, CancellationToken cancellationToken = default);
    Task<IEnumerable<(string TenThuocTinh, IEnumerable<string> GiaTriThuocTinhs)>> GetAttributeOptionsAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAttributeValueAsync(int sanPhamId, string tenThuocTinh, string? giaTriThuocTinh, CancellationToken cancellationToken = default);
    Task<bool> SetMultipleAttributesAsync(int sanPhamId, Dictionary<string, string?> attributes, CancellationToken cancellationToken = default);
    Task<int> DeleteByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> DeleteByAttributeNameAsync(string tenThuocTinh, CancellationToken cancellationToken = default);
    Task<bool> ExistsForProductAsync(int sanPhamId, string tenThuocTinh, CancellationToken cancellationToken = default);
    Task<int> CountByProductAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> CountByAttributeNameAsync(string tenThuocTinh, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuocTinhSanPham>> SearchByValueAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuocTinhSanPham>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, string?>> GetAttributesDictionaryAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> BulkUpdateAttributesAsync(int sanPhamId, Dictionary<string, string?> newAttributes, bool deleteOthers = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int ProductId, Dictionary<string, string?> Attributes)>> GetAllProductAttributesAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default);
}