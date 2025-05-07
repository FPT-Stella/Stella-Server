# Stella Server 🌠🪐

![GitHub](https://img.shields.io/github/license/FPT-Stella/Stella-Server)  
![GitHub last commit](https://img.shields.io/github/last-commit/FPT-Stella/Stella-Server)  
![GitHub issues](https://img.shields.io/github/issues/FPT-Stella/Stella-Server)

**Stella Server** là backend của hệ thống FPT Stella, một ứng dụng quản lý sinh viên và người dùng được xây dựng với kiến trúc phân tầng (layered architecture) sử dụng .NET 8.0 và MongoDB. Dự án cung cấp các API RESTful để xử lý các nghiệp vụ liên quan đến quản lý sinh viên, người dùng, và các thực thể khác trong hệ thống.

Bạn có thể xem tài liệu API tại: [Swagger UI](https://stella.dacoban.studio/swagger/index.html).

## Mục lục trong  📚

- [Cấu trúc dự án](#cấu-trúc-thư-mục)
- [Ý nghĩa của các tầng](#ý-nghĩa-của-các-tầng)
- [Yêu cầu cài đặt](#yêu-cầu-cài-đặt)
- [Hướng dẫn setup](#hướng-dẫn-setup)
- [Workflow phát triển](#workflow-phát-triển)
- [Đóng góp](#đóng-góp)
- [Giấy phép](#giấy-phép)

## 🏗️ **Contruct Folder**
Code
```
FPTStella/
├── FPTStella.API/             # Lớp API - cung cấp các endpoint cho client
├── FPTStella.Application/     # Lớp ứng dụng - xử lý logic nghiệp vụ
├── FPTStella.Domain/          # Lớp domain - định nghĩa các thực thể và quy tắc nghiệp vụ
├── FPTStella.Infrastructure/  # Lớp hạ tầng - quản lý truy cập dữ liệu và repository
└── Tests/                     # Thư mục chứa các bài kiểm thử đơn vị và tích hợp
```
## 🍀 Ý nghĩa của các tầng 🍀

### 1. Domain Layer (`FPTStella.Domain`)
- **Ý nghĩa**: Đây là tầng cốt lõi, chứa các entity (như `Student`, `User`) và logic nghiệp vụ thuần túy không phụ thuộc vào bất kỳ công nghệ nào. Tầng này định nghĩa các quy tắc và cấu trúc dữ liệu cơ bản của hệ thống.
- **Vai trò**: Đảm bảo tính toàn vẹn của dữ liệu và logic nghiệp vụ. Ví dụ: Các thuộc tính của `Student` (như `UserId`, `Name`) và các quy tắc, DTOs liên quan được định nghĩa ở đây.

### 2. Application Layer (`FPTStella.Application`)
- **Ý nghĩa**: Tầng này chứa logic ứng dụng, bao gồm các service và interface để xử lý các nghiệp vụ cụ thể (như lấy thông tin sinh viên, tạo người dùng mới).
- **Vai trò**: Là cầu nối giữa tầng API và tầng Domain, xử lý các yêu cầu từ người dùng và điều phối dữ liệu. Ví dụ: `IStudentService` và `StudentService` được định nghĩa để xử lý các nghiệp vụ liên quan đến sinh viên.

### 3. Infrastructure Layer (`FPTStella.Infrastructure`)
- **Ý nghĩa**: Tầng này chịu trách nhiệm triển khai các chi tiết kỹ thuật liên quan đến cơ sở hạ tầng, như kết nối database (MongoDB), persistence, và các dịch vụ bên ngoài.
- **Vai trò**: Cung cấp các triển khai cụ thể cho các interface được định nghĩa ở tầng Application. Ví dụ: `MongoDbContext` và `StudentRepository` xử lý việc lưu trữ và truy xuất dữ liệu từ MongoDB.

### 4. API Layer (`FPTStella.Api`)
- **Ý nghĩa**: Tầng này chứa các API endpoint và cấu hình liên quan đến giao tiếp HTTP.
- **Vai trò**: Xử lý các yêu cầu HTTP từ client, gọi các service ở tầng Application, và trả về kết quả. Tầng này sử dụng Swagger để cung cấp tài liệu API tự động.

## Yêu cầu cài đặt

- **Hệ điều hành**: Windows, macOS, hoặc Linux.
- **.NET SDK**: Phiên bản 8.0 hoặc mới hơn.
- **MongoDB**: Phiên bản 4.4 hoặc mới hơn (cần cài đặt và chạy MongoDB server).
- **IDE**: Visual Studio 2022, Visual Studio Code, hoặc bất kỳ IDE nào hỗ trợ .NET.
- **Git**: Để clone repository.

## Hướng dẫn setup

### 1. Clone repository
```bash
git clone https://github.com/FPT-Stella/Stella-Server.git
cd Stella-Server
```

### 2. Cài đặt MongoDB
- Tải và cài đặt MongoDB từ [trang chính thức](https://www.mongodb.com/try/download/community).
- Khởi động MongoDB server:
  ```bash
  mongod
  ```
- Đảm bảo MongoDB đang chạy trên `localhost:27017` (hoặc cấu hình connection string nếu cần).

### 3. Cấu hình connection string
- Tạo file `appsettings.json` trong thư mục `FPTStella.Api` (nếu chưa có) hoặc sử dụng User Secrets:
  ```json
  {
    "ConnectionStrings": {
      "StellaConnection": "mongodb://localhost:27017/StellaDb"
    }
  }
  ```
- Nếu sử dụng User Secrets (cho môi trường Development):
  ```bash
  cd src/FPTStella.Api
  dotnet user-secrets init
  dotnet user-secrets set "ConnectionStrings:StellaConnection" "mongodb://localhost:27017/StellaDb"
  ```

### 4. Cài đặt dependencies
- Đảm bảo bạn đang ở thư mục gốc của dự án (`Stella-Server`):
  ```bash
  dotnet restore
  ```

### 5. Chạy ứng dụng
- Di chuyển đến thư mục API:
  ```bash
  cd src/FPTStella.Api
  ```
- Chạy ứng dụng:
  ```bash
  dotnet run
  ```
- Mở trình duyệt và truy cập Swagger UI tại: `http://localhost:5000/swagger/index.html` (hoặc port được cấu hình trong `launchSettings.json`).

### 6. Truy cập API deploy
- API đã được deploy tại: [Swagger UI](https://stella.dacoban.studio/swagger/index.html).
- Sử dụng Swagger UI để khám phá và thử nghiệm các endpoint.

## Workflow phát triển

### 1. Tạo branch mới
- Tạo branch cho mỗi tính năng hoặc sửa lỗi:
  ```bash
  git checkout -b feature/<tên-tính-năng>
  ```

### 2. Phát triển
- **Thêm entity**: Thêm các entity mới vào `FPTStella.Domain`.
- **Triển khai service**: Tạo interface trong `FPTStella.Application.Common.Interfaces` và triển khai trong `FPTStella.Application`.
- **Triển khai repository**: Triển khai các repository trong `FPTStella.Infrastructure` để tương tác với MongoDB.
- **Tạo API endpoint**: Thêm controller và endpoint trong `FPTStella.Api`.

### 3. Kiểm tra
- Chạy ứng dụng cục bộ và kiểm tra các endpoint qua Swagger UI.
- Viết unit test (nếu có) để kiểm tra logic nghiệp vụ.

### 4. Commit và push
- Commit thay đổi:
  ```bash
  git add .
  git commit -m "feat: thêm tính năng XYZ"
  ```
- Push lên branch:
  ```bash
  git push origin feature/<tên-tính-năng>
  ```

### 5. Tạo Pull Request
- Tạo Pull Request (PR) từ branch của bạn vào `main`.
- Đợi review và merge.

### 6. Deploy
- Sau khi merge vào `main`, CI/CD pipeline (nếu có) sẽ tự động deploy lên server tại `http://103.179.185.152:5000`.

## Đóng góp

Chúng tôi hoan nghênh mọi đóng góp! Vui lòng làm theo các bước sau:

1. Fork repository.
2. Tạo branch mới (`git checkout -b feature/<tên-tính-năng>`).
3. Commit thay đổi (`git commit -m "feat: thêm tính năng XYZ"`).
4. Push lên branch (`git push origin feature/<tên-tính-năng>`).
5. Tạo Pull Request.

Xem [CONTRIBUTING.md](CONTRIBUTING.md) để biết thêm chi tiết.

## Giấy phép

Dự án được phân phối dưới [MIT License](LICENSE). Xem file `LICENSE` để biết thêm thông tin.

---

**FPT Stella Team**
