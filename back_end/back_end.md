## Cấu Trúc Kiến Trúc (Architecture)
* Dự án được xây dựng theo Layered Architecture (kiến trúc phân tầng), áp dụng chặt chẽ nguyên lý SOLID các tầng giao tiếp với nhau thông qua các Abstraction (Interfaces) nhờ cơ chế Dependency Injection (DI) mặc định của ASP.NET Core giúp giảm phụ thuộc và tăng khả năng mở rộng.
Tầng API (Presentation Layer): Là nơi là tiếp nhận HTTP Requests, điều phối request xuống tầng Services và trả về kết quả được chuẩn hóa qua ApiResponse<T> kèm theo HTTP Status Codes phù hợp.
Tầng BLL (Business Logic Layer): Là nơi xử lý nghiệp vụ. Chứa Services, Validators và DTOs. BLL đảm nhiệm việc mapping dữ liệu (giữa DTOs và Entities), và điều phối các thao tác xử lý dữ liệu. Nhận data từ Controller, map sang Entity, xử lý nghiệp vụ, chạy Validation và điều phối việc gọi Database. Việc xử lý logic tại đây giúp code dễ dàng được viết Unit Test mà không phụ thuộc vào cơ sở dữ liệu hay giao diện.
Tầng DAL (Data Access Layer): Tầng chuyên biệt hóa cho việc tương tác với cơ sở dữ liệu (SQL Server), Chứa Repositories và thực hiện các thao tác CRUD và truy vấn dữ liệu thông qua Entity Framework Core kết hợp với Repository Pattern. Tầng này trừu tượng hóa toàn bộ chi tiết về cơ sở dữ liệu, đảm bảo tầng BLL ở trên không cần biết dữ liệu được lưu trữ hay truy xuất bằng công nghệ nào (Dependency Inversion).

----------------------------------------------------
## API Documentation (Swagger)

*Sau khi ứng dụng khởi chạy thành công, có thể truy cập giao diện Swagger để xem tài liệu chi tiết và test trực tiếp các API:

** URL: http://localhost:8080/swagger (Nếu chạy bằng Docker) 
** URL: https://localhost:<port>/swagger (Nếu chạy Local - xem port thực tế trong terminal)

### Các luồng nghiệp vụ cơ bản (API Workflows)

**Dành cho Quản trị viên (Admin):**

#### Form Management

* `[GET] /api/forms`: Lấy danh sách form đã được phân trang cho admin.
**Test:**

```json
{
  "title": "Khảo sát nhân viên",
  "description": "Form đánh giá hiệu suất quý 1",
  "displayOrder": 1,
  "status": "Active",
  "fields": [
    {
      "name": "fullName",
      "label": "Họ và tên",
      "fieldType": "text",
      "displayOrder": 1,
      "isRequired": true,
      "configuration": "{ \"maxLength\": 50 }"
    },
    {
      "name": "age",
      "label": "Tuổi",
      "fieldType": "number",
      "displayOrder": 2,
      "isRequired": false,
      "configuration": "{ \"min\": 18, \"max\": 60 }"
    }
  ]
}```





