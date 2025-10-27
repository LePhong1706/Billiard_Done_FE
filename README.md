# Billiard Shop - Hệ thống Thương mại điện tử Bida

Dự án website thương mại điện tử chuyên về các sản phẩm và phụ kiện bida, được xây dựng với ASP.NET Core MVC và SQL Server.

## Tính năng chính

### Dành cho Khách hàng
- **Duyệt & Tìm kiếm Sản phẩm**: Xem danh mục sản phẩm với khả năng lọc và tìm kiếm
- **Chi tiết Sản phẩm**: Xem thông tin chi tiết, hình ảnh, giá cả và tình trạng tồn kho
- **Giỏ hàng**: Thêm, cập nhật số lượng, xóa sản phẩm trong giỏ hàng
- **Thanh toán**: Quy trình đặt hàng đơn giản với xác nhận đơn hàng
- **Đánh giá Sản phẩm**: Xem và viết đánh giá cho các sản phẩm đã mua
- **Danh sách Yêu thích**: Lưu các sản phẩm yêu thích để xem lại sau
- **Sản phẩm Đã xem**: Theo dõi các sản phẩm đã xem gần đây

## Công nghệ sử dụng

### Backend
- **ASP.NET Core 9.0** - Framework chính
- **Entity Framework Core** - ORM cho database
- **SQL Server** - Hệ quản trị cơ sở dữ liệu
- **Clean Architecture** - Kiến trúc phân lớp rõ ràng
  - `Domain`: Entities và Business Logic
  - `Application`: Services và Interfaces
  - `Infrastructure`: Data Access và External Services
  - `Web`: Presentation Layer (MVC)

### Frontend
- **ASP.NET Core MVC** - Razor Views
- **Bootstrap 5** - UI Framework
- **jQuery** - JavaScript Library

## Yêu cầu Hệ thống

- **.NET 9.0 SDK** hoặc mới hơn
- **SQL Server** (Express/Developer/Standard)
- **Visual Studio 2022** hoặc **VS Code** (tùy chọn)
- **SQL Server Management Studio** hoặc **Azure Data Studio** (để quản lý database)

## Hướng dẫn Cài đặt

### 1. Clone Repository

```bash
git clone <repository-url>
cd BilliardShop
```

### 2. Thiết lập Database

1. Mở SQL Server Management Studio hoặc Azure Data Studio
2. Chạy script `docs/database/database.sql` để tạo database và dữ liệu mẫu
3. Database `BilliardShop` sẽ được tạo với đầy đủ tables, views, và stored procedures

### 3. Cấu hình Connection String

Mở file `src/BilliardShop.Web/appsettings.json` và cập nhật connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BilliardShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### 4. Build và Chạy

```bash
# Restore packages và build
dotnet build

# Chạy ứng dụng
dotnet run --project src/BilliardShop.Web

# Hoặc chạy với watch mode (tự động reload khi có thay đổi)
dotnet watch --project src/BilliardShop.Web
```

Truy cập ứng dụng tại: `https://localhost:7123` hoặc `http://localhost:5123`

### 5. Chạy Unit Tests

```bash
# Chạy tất cả tests
dotnet test

# Chạy tests cho một project cụ thể
dotnet test tests/BilliardShop.Application.Tests
dotnet test tests/BilliardShop.Domain.Tests
```

## Cấu trúc Dự án

```
BilliardShop/
├── src/
│   ├── BilliardShop.Domain/           # Domain Entities & Common Types
│   │   ├── Entities/                  # Database entities
│   │   ├── Common/                    # Base classes & interfaces
│   │   └── Interfaces/                # Domain interfaces
│   │
│   ├── BilliardShop.Application/      # Business Logic Layer
│   │   ├── Services/                  # Application services
│   │   ├── Interfaces/                # Service interfaces
│   │   └── Common/                    # DTOs & mappings
│   │
│   ├── BilliardShop.Infrastructure/   # Data Access Layer
│   │   ├── Data/                      # DbContext
│   │   ├── Repositories/              # Repository implementations
│   │   └── Services/                  # Infrastructure services
│   │
│   └── BilliardShop.Web/              # Presentation Layer
│       ├── Controllers/               # MVC Controllers
│       ├── Models/                    # ViewModels
│       ├── Views/                     # Razor views
│       └── wwwroot/                   # Static files
│
├── tests/
│   ├── BilliardShop.Domain.Tests/     # Domain tests
│   └── BilliardShop.Application.Tests/# Application tests
│
├── docs/
│   └── database/                      # Database scripts
│
└── specs/                             # Feature specifications
    └── 001-billiard-ecommerce-site/
```

## Các Services chính

### ProductService
- Quản lý danh sách và chi tiết sản phẩm
- Tìm kiếm và lọc sản phẩm
- Quản lý danh mục sản phẩm

### CartService
- Quản lý giỏ hàng (session-based)
- Thêm/cập nhật/xóa items
- Tính toán tổng giá trị

### OrderService
- Xử lý đơn hàng
- Lưu thông tin khách hàng và chi tiết đơn hàng
- Cập nhật trạng thái đơn hàng

### ReviewService
- Quản lý đánh giá sản phẩm
- Tính toán điểm trung bình
- Phê duyệt đánh giá

### WishlistService
- Quản lý danh sách yêu thích
- Thêm/xóa sản phẩm khỏi wishlist

### RecentlyViewedService
- Theo dõi sản phẩm đã xem
- Hiển thị lịch sử xem gần đây

## Patterns & Best Practices

- **Repository Pattern**: Tách biệt data access logic
- **Unit of Work Pattern**: Quản lý transactions
- **Dependency Injection**: Loose coupling và testability
- **Clean Architecture**: Separation of concerns
- **Async/Await**: Non-blocking operations

## Môi trường Development

### Recommended Extensions (VS Code)
- C# Dev Kit
- SQL Server (mssql)
- ESLint
- Prettier

### Coding Standards
- Follow C# naming conventions
- Use async/await for database operations
- Write unit tests for business logic
- Comment complex logic

## Troubleshooting

### Lỗi Connection String
- Kiểm tra SQL Server đang chạy
- Xác nhận connection string trong appsettings.json
- Thử kết nối bằng SSMS trước

### Lỗi Build
- Chạy `dotnet clean` rồi `dotnet restore`
- Xóa folders `bin/` và `obj/`
- Kiểm tra .NET SDK version: `dotnet --version`

### Lỗi Database
- Chạy lại database script
- Kiểm tra permissions của SQL user
- Xác nhận database `BilliardShop` tồn tại

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is part of a learning exercise for ASP.NET Core MVC development.

## Contact

Project Link: [GitHub Repository]

---

**Developed with ASP.NET Core 9.0 | Clean Architecture | Entity Framework Core**
