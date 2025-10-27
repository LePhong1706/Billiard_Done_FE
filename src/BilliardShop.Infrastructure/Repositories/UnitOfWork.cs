using Microsoft.EntityFrameworkCore.Storage;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BilliardShopDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    // User Management Repositories
    private IVaiTroNguoiDungRepository? _vaiTroNguoiDungRepository;
    private INguoiDungRepository? _nguoiDungRepository;
    private IDiaChiNguoiDungRepository? _diaChiNguoiDungRepository;

    // Product Management Repositories
    private IDanhMucSanPhamRepository? _danhMucSanPhamRepository;
    private IThuongHieuRepository? _thuongHieuRepository;
    private ISanPhamRepository? _sanPhamRepository;
    private IHinhAnhSanPhamRepository? _hinhAnhSanPhamRepository;
    private IThuocTinhSanPhamRepository? _thuocTinhSanPhamRepository;

    // Inventory Management Repositories
    private INhaCungCapRepository? _nhaCungCapRepository;
    private IBienDongKhoHangRepository? _bienDongKhoHangRepository;

    // Order Management Repositories
    private ITrangThaiDonHangRepository? _trangThaiDonHangRepository;
    private IPhuongThucThanhToanRepository? _phuongThucThanhToanRepository;
    private IPhuongThucVanChuyenRepository? _phuongThucVanChuyenRepository;
    private IDonHangRepository? _donHangRepository;
    private IChiTietDonHangRepository? _chiTietDonHangRepository;

    // Promotions & Discounts Repositories
    private IMaGiamGiaRepository? _maGiamGiaRepository;
    private ISuDungMaGiamGiaRepository? _suDungMaGiamGiaRepository;

    // Customer Interactions Repositories
    private IDanhGiaSanPhamRepository? _danhGiaSanPhamRepository;
    private IDanhSachYeuThichRepository? _danhSachYeuThichRepository;
    private IGioHangRepository? _gioHangRepository;

    // System Settings & Blog Repositories
    private ICaiDatHeThongRepository? _caiDatHeThongRepository;
    private IBaiVietRepository? _baiVietRepository;
    private IBinhLuanBaiVietRepository? _binhLuanBaiVietRepository;
    private INhatKyHeThongRepository? _nhatKyHeThongRepository;


    public UnitOfWork(BilliardShopDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // User Management Repository Properties
    public IVaiTroNguoiDungRepository VaiTroNguoiDungRepository
        => _vaiTroNguoiDungRepository ??= new VaiTroNguoiDungRepository(_context);

    public INguoiDungRepository NguoiDungRepository
        => _nguoiDungRepository ??= new NguoiDungRepository(_context);

    public IDiaChiNguoiDungRepository DiaChiNguoiDungRepository
        => _diaChiNguoiDungRepository ??= new DiaChiNguoiDungRepository(_context);

    // Product Management Repository Properties
    public IDanhMucSanPhamRepository DanhMucSanPhamRepository
        => _danhMucSanPhamRepository ??= new DanhMucSanPhamRepository(_context);

    public IThuongHieuRepository ThuongHieuRepository
        => _thuongHieuRepository ??= new ThuongHieuRepository(_context);

    public ISanPhamRepository SanPhamRepository
        => _sanPhamRepository ??= new SanPhamRepository(_context);

    public IHinhAnhSanPhamRepository HinhAnhSanPhamRepository
        => _hinhAnhSanPhamRepository ??= new HinhAnhSanPhamRepository(_context);

    public IThuocTinhSanPhamRepository ThuocTinhSanPhamRepository
        => _thuocTinhSanPhamRepository ??= new ThuocTinhSanPhamRepository(_context);

    // Inventory Management Repository Properties
    public INhaCungCapRepository NhaCungCapRepository
        => _nhaCungCapRepository ??= new NhaCungCapRepository(_context);

    public IBienDongKhoHangRepository BienDongKhoHangRepository
        => _bienDongKhoHangRepository ??= new BienDongKhoHangRepository(_context);

    // Order Management Repository Properties
    public ITrangThaiDonHangRepository TrangThaiDonHangRepository
        => _trangThaiDonHangRepository ??= new TrangThaiDonHangRepository(_context);

    public IPhuongThucThanhToanRepository PhuongThucThanhToanRepository
        => _phuongThucThanhToanRepository ??= new PhuongThucThanhToanRepository(_context);

    public IPhuongThucVanChuyenRepository PhuongThucVanChuyenRepository
        => _phuongThucVanChuyenRepository ??= new PhuongThucVanChuyenRepository(_context);

    public IDonHangRepository DonHangRepository
        => _donHangRepository ??= new DonHangRepository(_context);

    public IChiTietDonHangRepository ChiTietDonHangRepository
        => _chiTietDonHangRepository ??= new ChiTietDonHangRepository(_context);

    // Promotions & Discounts Repository Properties
    public IMaGiamGiaRepository MaGiamGiaRepository
        => _maGiamGiaRepository ??= new MaGiamGiaRepository(_context);

    public ISuDungMaGiamGiaRepository SuDungMaGiamGiaRepository
        => _suDungMaGiamGiaRepository ??= new SuDungMaGiamGiaRepository(_context);

    // Customer Interactions Repository Properties
    public IDanhGiaSanPhamRepository DanhGiaSanPhamRepository
        => _danhGiaSanPhamRepository ??= new DanhGiaSanPhamRepository(_context);

    public IDanhSachYeuThichRepository DanhSachYeuThichRepository
        => _danhSachYeuThichRepository ??= new DanhSachYeuThichRepository(_context);

    public IGioHangRepository GioHangRepository
        => _gioHangRepository ??= new GioHangRepository(_context);

    // System Settings & Blog Repository Properties
    public ICaiDatHeThongRepository CaiDatHeThongRepository
        => _caiDatHeThongRepository ??= new CaiDatHeThongRepository(_context);

    public IBaiVietRepository BaiVietRepository
        => _baiVietRepository ??= new BaiVietRepository(_context);

    public IBinhLuanBaiVietRepository BinhLuanBaiVietRepository
        => _binhLuanBaiVietRepository ??= new BinhLuanBaiVietRepository(_context);

    public INhatKyHeThongRepository NhatKyHeThongRepository
        => _nhatKyHeThongRepository ??= new NhatKyHeThongRepository(_context);

    // Transaction Methods
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    // Transaction Management
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Transaction is already started.");
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction started.");
        }

        try
        {
            await SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction started.");
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    // Dispose Pattern
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}