using BilliardShop.Application.Interfaces;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Application.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GioHang>> GetCartItemsAsync(int? nguoiDungId, string? maPhienLamViec)
    {
        if (nguoiDungId.HasValue)
        {
            return await _unitOfWork.GioHangRepository.GetByUserIdAsync(nguoiDungId.Value);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            return await _unitOfWork.GioHangRepository.GetBySessionIdAsync(maPhienLamViec);
        }

        return Enumerable.Empty<GioHang>();
    }

    public async Task AddToCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId, int soLuong = 1)
    {
        // Kiểm tra sản phẩm tồn tại và còn hàng
        var sanPham = await _unitOfWork.SanPhamRepository.GetByIdAsync(sanPhamId);
        if (sanPham == null || !sanPham.TrangThaiHoatDong)
        {
            return;
        }

        // Kiểm tra tồn kho
        if (sanPham.SoLuongTonKho < soLuong)
        {
            return;
        }

        // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
        GioHang? existingCartItem = null;
        if (nguoiDungId.HasValue)
        {
            existingCartItem = await _unitOfWork.GioHangRepository.GetCartItemAsync(nguoiDungId.Value, sanPhamId);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            existingCartItem = await _unitOfWork.GioHangRepository.GetCartItemBySessionAsync(maPhienLamViec, sanPhamId);
        }

        if (existingCartItem != null)
        {
            // Cập nhật số lượng
            int newQuantity = existingCartItem.SoLuong + soLuong;

            // Kiểm tra lại tồn kho
            if (newQuantity > sanPham.SoLuongTonKho)
            {
                return;
            }

            existingCartItem.SoLuong = newQuantity;
            _unitOfWork.GioHangRepository.Update(existingCartItem);
        }
        else
        {
            // Thêm mới vào giỏ hàng
            var gioHang = new GioHang
            {
                MaNguoiDung = nguoiDungId,
                MaPhienLamViec = maPhienLamViec,
                MaSanPham = sanPhamId,
                SoLuong = soLuong
            };

            await _unitOfWork.GioHangRepository.AddAsync(gioHang);
        }
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(int? nguoiDungId, string? maPhienLamViec, int gioHangId, int newQuantity)
    {
        if (newQuantity <= 0)
        {
            return;
        }

        GioHang? gioHang = null;
        if (nguoiDungId.HasValue)
        {
            gioHang = await _unitOfWork.GioHangRepository.GetByIdAsync(gioHangId);
            if (gioHang?.MaNguoiDung != nguoiDungId.Value) return;
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            gioHang = await _unitOfWork.GioHangRepository.GetByIdAsync(gioHangId);
            if (gioHang?.MaPhienLamViec != maPhienLamViec) return;
        }

        if (gioHang == null)
        {
            return;
        }

        // Kiểm tra tồn kho
        var sanPham = await _unitOfWork.SanPhamRepository.GetByIdAsync(gioHang.MaSanPham);
        if (sanPham == null || newQuantity > sanPham.SoLuongTonKho)
        {
            return;
        }

        gioHang.SoLuong = newQuantity;
        _unitOfWork.GioHangRepository.Update(gioHang);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId)
    {
        GioHang? cartItem = null;

        if (nguoiDungId.HasValue)
        {
            cartItem = await _unitOfWork.GioHangRepository.GetCartItemAsync(nguoiDungId.Value, sanPhamId);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            cartItem = await _unitOfWork.GioHangRepository.GetCartItemBySessionAsync(maPhienLamViec, sanPhamId);
        }

        if (cartItem == null)
        {
            return;
        }

        _unitOfWork.GioHangRepository.Remove(cartItem);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> GetCartItemCountAsync(int? nguoiDungId, string? maPhienLamViec)
    {
        if (nguoiDungId.HasValue)
        {
            return await _unitOfWork.GioHangRepository.CountItemsByUserAsync(nguoiDungId.Value);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            return await _unitOfWork.GioHangRepository.CountItemsBySessionAsync(maPhienLamViec);
        }

        return 0;
    }

    public async Task<decimal> GetCartTotalAsync(int? nguoiDungId, string? maPhienLamViec)
    {
        if (nguoiDungId.HasValue)
        {
            return await _unitOfWork.GioHangRepository.GetTotalValueByUserAsync(nguoiDungId.Value);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            return await _unitOfWork.GioHangRepository.GetTotalValueBySessionAsync(maPhienLamViec);
        }

        return 0;
    }

    public async Task<bool> ClearCartAsync(int? nguoiDungId, string? maPhienLamViec)
    {
        int deletedCount = 0;

        if (nguoiDungId.HasValue)
        {
            deletedCount = await _unitOfWork.GioHangRepository.ClearCartByUserAsync(nguoiDungId.Value);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            deletedCount = await _unitOfWork.GioHangRepository.ClearCartBySessionAsync(maPhienLamViec);
        }

        return deletedCount > 0;
    }
}
