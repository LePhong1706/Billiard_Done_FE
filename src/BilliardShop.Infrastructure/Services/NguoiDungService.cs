using AutoMapper;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BilliardShop.Infrastructure.Services;

public class NguoiDungService : BaseService<NguoiDung, NguoiDungDto>, INguoiDungService
{
    public NguoiDungService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    protected override IGenericRepository<NguoiDung> Repository => _unitOfWork.NguoiDungRepository;

    public async Task<ServiceResult<NguoiDungDto>> CreateAsync(CreateNguoiDungDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate
            var errors = new List<string>();
            
            if (await _unitOfWork.NguoiDungRepository.IsEmailExistsAsync(createDto.Email, null, cancellationToken))
                errors.Add("Email đã tồn tại");
                
            if (await _unitOfWork.NguoiDungRepository.IsUsernameExistsAsync(createDto.TenDangNhap, null, cancellationToken))
                errors.Add("Tên đăng nhập đã tồn tại");

            if (errors.Any())
                return ServiceResult<NguoiDungDto>.Failure(errors);

            // Hash password
            var (hashedPassword, salt) = HashPassword(createDto.MatKhau);

            // Create entity
            var user = new NguoiDung
            {
                TenDangNhap = createDto.TenDangNhap,
                Email = createDto.Email,
                MatKhauMaHoa = hashedPassword,
                MuoiMatKhau = salt,
                Ho = createDto.Ho,
                Ten = createDto.Ten,
                SoDienThoai = createDto.SoDienThoai,
                NgaySinh = createDto.NgaySinh,
                GioiTinh = createDto.GioiTinh,
                MaVaiTro = createDto.MaVaiTro
            };

            var createdUser = await Repository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userDto = await GetUserWithDetailsAsync(createdUser.Id, cancellationToken);
            return ServiceResult<NguoiDungDto>.Success(userDto!);
        }
        catch (Exception ex)
        {
            return ServiceResult<NguoiDungDto>.Failure($"Lỗi khi tạo người dùng: {ex.Message}");
        }
    }

    public async Task<NguoiDungDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.NguoiDungRepository.GetByEmailAsync(email, true, cancellationToken);
        return user != null ? _mapper.Map<NguoiDungDto>(user) : null;
    }

    public async Task<NguoiDungDto?> GetByTenDangNhapAsync(string tenDangNhap, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.NguoiDungRepository.GetByTenDangNhapAsync(tenDangNhap, true, cancellationToken);
        return user != null ? _mapper.Map<NguoiDungDto>(user) : null;
    }

    public async Task<NguoiDungDto?> GetByEmailOrUsernameAsync(string emailOrUsername, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.NguoiDungRepository.GetByEmailOrUsernameAsync(emailOrUsername, cancellationToken);
        return user != null ? _mapper.Map<NguoiDungDto>(user) : null;
    }

    public async Task<ServiceResult<bool>> UpdateLastLoginAsync(int userId, DateTime lastLoginTime, CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await _unitOfWork.NguoiDungRepository.UpdateLastLoginAsync(userId, lastLoginTime, cancellationToken);
            return success ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Failure("Không thể cập nhật thời gian đăng nhập");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Lỗi khi cập nhật thời gian đăng nhập: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> UpdateEmailVerificationStatusAsync(int userId, bool daXacThuc, CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await _unitOfWork.NguoiDungRepository.UpdateEmailVerificationStatusAsync(userId, daXacThuc, cancellationToken);
            return success ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Failure("Không thể cập nhật trạng thái xác thực email");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Lỗi khi cập nhật trạng thái xác thực: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> UpdatePasswordAsync(int userId, string matKhauMoi, CancellationToken cancellationToken = default)
    {
        try
        {
            var (hashedPassword, salt) = HashPassword(matKhauMoi);
            var success = await _unitOfWork.NguoiDungRepository.UpdatePasswordAsync(userId, hashedPassword, salt, cancellationToken);
            return success ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Failure("Không thể cập nhật mật khẩu");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Lỗi khi cập nhật mật khẩu: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> UpdateActiveStatusAsync(int userId, bool trangThaiHoatDong, CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await _unitOfWork.NguoiDungRepository.UpdateActiveStatusAsync(userId, trangThaiHoatDong, cancellationToken);
            return success ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Failure("Không thể cập nhật trạng thái");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Lỗi khi cập nhật trạng thái: {ex.Message}");
        }
    }

    public async Task<IEnumerable<NguoiDungDto>> GetUsersByRoleAsync(int vaiTroId, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork.NguoiDungRepository.GetUsersByRoleAsync(vaiTroId, activeOnly, cancellationToken);
        return _mapper.Map<IEnumerable<NguoiDungDto>>(users);
    }

    public async Task<PagedResult<NguoiDungDto>> SearchUsersAsync(string? searchTerm = null, int? vaiTroId = null, bool? trangThaiHoatDong = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var (users, totalCount) = await _unitOfWork.NguoiDungRepository.SearchUsersAsync(searchTerm, vaiTroId, trangThaiHoatDong, pageNumber, pageSize, cancellationToken);
        var userDtos = _mapper.Map<IEnumerable<NguoiDungDto>>(users);

        return new PagedResult<NguoiDungDto>
        {
            Items = userDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<NguoiDungDto?> GetUserWithDetailsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.NguoiDungRepository.GetUserWithDetailsAsync(userId, cancellationToken);
        return user != null ? _mapper.Map<NguoiDungDto>(user) : null;
    }

    public async Task<int> CountNewUsersAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.NguoiDungRepository.CountNewUsersAsync(fromDate, toDate, cancellationToken);
    }

    public async Task<IEnumerable<NguoiDungDto>> GetRecentActiveUsersAsync(int days = 30, int take = 100, CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork.NguoiDungRepository.GetRecentActiveUsersAsync(days, take, cancellationToken);
        return _mapper.Map<IEnumerable<NguoiDungDto>>(users);
    }

    public async Task<bool> IsEmailAvailableAsync(string email, int? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        return !await _unitOfWork.NguoiDungRepository.IsEmailExistsAsync(email, excludeUserId, cancellationToken);
    }

    public async Task<bool> IsUsernameAvailableAsync(string tenDangNhap, int? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        return !await _unitOfWork.NguoiDungRepository.IsUsernameExistsAsync(tenDangNhap, excludeUserId, cancellationToken);
    }

    public async Task<ServiceResult<NguoiDungDto>> ValidateLoginAsync(string usernameOrEmail, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.NguoiDungRepository.GetByEmailOrUsernameAsync(usernameOrEmail, cancellationToken);

            if (user == null)
                return ServiceResult<NguoiDungDto>.Failure("Tên đăng nhập hoặc mật khẩu không đúng");

            if (!user.TrangThaiHoatDong)
                return ServiceResult<NguoiDungDto>.Failure("Tài khoản đã bị khóa");

            if (!VerifyPassword(password, user.MatKhauMaHoa, user.MuoiMatKhau))
                return ServiceResult<NguoiDungDto>.Failure("Tên đăng nhập hoặc mật khẩu không đúng");

            var userDto = _mapper.Map<NguoiDungDto>(user);
            return ServiceResult<NguoiDungDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<NguoiDungDto>.Failure($"Lỗi khi đăng nhập: {ex.Message}");
        }
    }

    private static (string hashedPassword, string salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[32];
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        var hashedPassword = Convert.ToBase64String(hash);

        return (hashedPassword, salt);
    }

    private static bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        var computedHash = Convert.ToBase64String(hash);

        return computedHash == hashedPassword;
    }
}
