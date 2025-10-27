using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;

namespace BilliardShop.Application.Interfaces.Services;

public interface INguoiDungService : IBaseService<NguoiDungDto>
{
    Task<ServiceResult<NguoiDungDto>> CreateAsync(CreateNguoiDungDto createDto, CancellationToken cancellationToken = default);
    Task<NguoiDungDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<NguoiDungDto?> GetByTenDangNhapAsync(string tenDangNhap, CancellationToken cancellationToken = default);
    Task<NguoiDungDto?> GetByEmailOrUsernameAsync(string emailOrUsername, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdateLastLoginAsync(int userId, DateTime lastLoginTime, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdateEmailVerificationStatusAsync(int userId, bool daXacThuc, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdatePasswordAsync(int userId, string matKhauMoi, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdateActiveStatusAsync(int userId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);
    Task<IEnumerable<NguoiDungDto>> GetUsersByRoleAsync(int vaiTroId, bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<PagedResult<NguoiDungDto>> SearchUsersAsync(string? searchTerm = null, int? vaiTroId = null, bool? trangThaiHoatDong = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task<NguoiDungDto?> GetUserWithDetailsAsync(int userId, CancellationToken cancellationToken = default);
    Task<int> CountNewUsersAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<NguoiDungDto>> GetRecentActiveUsersAsync(int days = 30, int take = 100, CancellationToken cancellationToken = default);
    Task<bool> IsEmailAvailableAsync(string email, int? excludeUserId = null, CancellationToken cancellationToken = default);
    Task<bool> IsUsernameAvailableAsync(string tenDangNhap, int? excludeUserId = null, CancellationToken cancellationToken = default);
    Task<ServiceResult<NguoiDungDto>> ValidateLoginAsync(string usernameOrEmail, string password, CancellationToken cancellationToken = default);
}