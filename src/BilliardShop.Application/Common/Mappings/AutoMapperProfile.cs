using AutoMapper;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Application.Common.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CaiDatHeThong mappings
        CreateMap<CaiDatHeThong, CaiDatHeThongDto>().ReverseMap();

        // VaiTroNguoiDung mappings
        CreateMap<VaiTroNguoiDung, VaiTroNguoiDungDto>()
            .ForMember(dest => dest.SoLuongNguoiDung, opt => opt.Ignore())
            .ReverseMap();

        // NguoiDung mappings
        CreateMap<NguoiDung, NguoiDungDto>()
            .ForMember(dest => dest.TenVaiTro, opt => opt.MapFrom(src => src.VaiTro.TenVaiTro))
            .ReverseMap()
            .ForMember(dest => dest.VaiTro, opt => opt.Ignore());

        CreateMap<CreateNguoiDungDto, NguoiDung>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MatKhauMaHoa, opt => opt.Ignore())
            .ForMember(dest => dest.MuoiMatKhau, opt => opt.Ignore())
            .ForMember(dest => dest.VaiTro, opt => opt.Ignore())
            .ForMember(dest => dest.DiaChis, opt => opt.Ignore())
            .ForMember(dest => dest.DonHangs, opt => opt.Ignore())
            .ForMember(dest => dest.DanhGias, opt => opt.Ignore())
            .ForMember(dest => dest.YeuThichs, opt => opt.Ignore())
            .ForMember(dest => dest.GioHangs, opt => opt.Ignore())
            .ForMember(dest => dest.SanPhamsTao, opt => opt.Ignore())
            .ForMember(dest => dest.BaiViets, opt => opt.Ignore());

        // ThuongHieu mappings
        CreateMap<ThuongHieu, ThuongHieuDto>()
            .ForMember(dest => dest.SoLuongSanPham, opt => opt.Ignore())
            .ReverseMap();

        // DanhMucSanPham mappings
        CreateMap<DanhMucSanPham, DanhMucSanPhamDto>()
            .ForMember(dest => dest.TenDanhMucCha, opt => opt.MapFrom(src => src.DanhMucCha != null ? src.DanhMucCha.TenDanhMuc : null))
            .ForMember(dest => dest.DanhMucCons, opt => opt.MapFrom(src => src.DanhMucCons))
            .ForMember(dest => dest.SoLuongSanPham, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.DanhMucCha, opt => opt.Ignore())
            .ForMember(dest => dest.DanhMucCons, opt => opt.Ignore())
            .ForMember(dest => dest.SanPhams, opt => opt.Ignore());
    }
}