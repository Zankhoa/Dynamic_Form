# 📋 Dynamic Form System (Young Talent Assessment)

Đây là hệ thống quản lý form động (Dynamic Form) cho phép Admin cấu hình các biểu mẫu linh hoạt và Nhân viên có thể điền thông tin dựa trên các cấu hình đó. Dự án được xây dựng với mục tiêu tối ưu hóa tính **mở rộng (Scalability)**, **tổ chức code sạch (Clean Code)** và **dễ dàng vận hành**.

---
## Công nghệ sử dụng
* **Framework:** ASP.NET Core 8 Web API
* **Ngôn ngữ:** C# 12
* **Database:** SQL Server (Áp dụng tính năng lưu trữ JSON gốc)
* **ORM:** Entity Framework Core 8 (Code-First)
* **Containerization:** Docker & Docker Compose
* **Tài liệu API:** Swagger / OpenAPI
---

## Kiến trúc & Quyết định thiết kế (Design Decisions)

Dự án này giải quyết bài toán "Dynamic Data" thông qua các quyết định kiến trúc sau:

1.  **Lưu trữ dữ liệu động (JSON Column trong SQL Server):**
    Thay vì sử dụng mô hình EAV (Entity-Attribute-Value) truyền thống gây phình to database và chậm truy vấn, dự án lưu toàn bộ kết quả nộp form (`FormData`) dưới dạng chuỗi JSON nguyên bản trong SQL Server. hệ thống vừa đảm bảo tốc độ Insert cực nhanh vừa giữ được tính an toàn dữ liệu.

2.  **Validation động (Strategy Pattern & Factory):**
    Logic kiểm tra tính hợp lệ của dữ liệu (Text, Number, Date, Color, Select) được tách biệt hoàn toàn khỏi Controller/Service thông qua `FieldValidatorFactory`. Điều này tuân thủ nguyên lý **Open/Closed Principle (OCP)**: Khi cần thêm loại field mới (vd: Email, File), chỉ cần thêm class Validator mới mà không cần sửa đổi code cũ.

3.  **Tổ chức Code (N-Tier Architecture):**
    * **Controller:** chỉ nhận Request và trả Response.
    * **Service:** Chứa business logic, không phụ thuộc vào framework HTTP.
    * **Repository(Data):** Đóng gói logic giao tiếp với EF Core.
    * **Middleware:** Bắt lỗi tập trung (`GlobalExceptionMiddleware`) giúp mọi response lỗi đều có cùng một định dạng chuẩn.
---

## 🛠 Hướng dẫn chạy dự án

Dự án hỗ trợ 2 cách chạy: Sử dụng Docker (Khuyên dùng) hoặc chạy trực tiếp trên máy tính.

### CÁCH 1: Chạy bằng Docker (Nhanh nhất - Khuyên dùng)

**Yêu cầu:** Máy tính đã cài đặt [Docker Desktop](https://www.docker.com/products/docker-desktop/). Bạn không cần cài đặt .NET SDK hay SQL Server.

1. Mở Terminal / Command Prompt tại thư mục gốc của dự án (nơi có file `docker-compose.yml`).
2. Chạy câu lệnh sau:
   ```bash
   docker-compose up -d --build
   ```
3. Chờ khoảng 1-2 phút để Docker tải image và khởi động.
(Lưu ý: API đã được cấu hình tự động chạy EF Core Migrations khi khởi động, nên Database và các bảng sẽ được tự động tạo sẵn).
### CÁCH 2: Chạy Local (Thủ công)
Yêu cầu: Máy tính đã cài đặt .NET 8 SDK và có sẵn SQL Server (LocalDB hoặc bản đầy đủ).
Mở file appsettings.json trong project dynamic_form_system và cấu hình lại chuỗi DefaultConnection cho khớp với SQL Server của bạn.
Mở Terminal tại thư mục DynamicForm.API và chạy lệnh cập nhật Database:
```bash
dotnet ef database update
```
Khởi chạy ứng dụng:
```Bash
dotnet run
```
## API Documentation (Swagger)
Sau khi ứng dụng khởi chạy thành công, bạn có thể truy cập giao diện Swagger để xem tài liệu chi tiết và test trực tiếp các API:
URL: http://localhost:8080/swagger (Nếu chạy bằng Docker) * URL: https://localhost:<port>/swagger (Nếu chạy Local - xem port thực tế trong terminal)

Luồng kiểm thử gợi ý:
Admin: Gọi API POST /api/admin/forms để tạo form mới.
Admin: Gọi API POST /api/admin/forms/{id}/fields để thêm các field (như text, date, number) vào form.
Nhân viên: Gọi API GET /api/forms/active để xem danh sách form.
Nhân viên: Gọi API POST /api/forms/{id}/submit để nộp dữ liệu. Thử nhập sai định dạng để xem hệ thống trả về lỗi Validation.
