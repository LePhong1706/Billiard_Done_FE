using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IBienDongKhoHangRepository : IGenericRepository<BienDongKhoHang>
{
    Task<IEnumerable<BienDongKhoHang>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BienDongKhoHang>> GetByMovementTypeAsync(string loaiBienDong, CancellationToken cancellationToken = default);
    Task<IEnumerable<BienDongKhoHang>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<BienDongKhoHang>> GetByReferenceAsync(string thamChieu, CancellationToken cancellationToken = default);
    Task<(IEnumerable<BienDongKhoHang> Movements, int TotalCount)> GetInventoryHistoryAsync(
        int? sanPhamId = null,
        string? loaiBienDong = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);
    Task<BienDongKhoHang> CreateMovementAsync(
        int sanPhamId,
        string loaiBienDong,
        int soLuong,
        int tonKhoTruoc,
        int tonKhoSau,
        string? thamChieu = null,
        string? ghiChu = null,
        int? nguoiThucHien = null,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<BienDongKhoHang>> GetRecentMovementsAsync(int days = 7, int take = 50, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetMovementStatsByTypeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int ProductId, string ProductName, int TotalIn, int TotalOut, int NetChange)>> GetProductMovementSummaryAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);
    Task<bool> HasMovementsByProductAsync(int sanPhamId, CancellationToken cancellationToken = default);
}