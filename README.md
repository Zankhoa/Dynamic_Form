# Dynamic Form System
## Cấu Trúc Kiến Trúc (Architecture)
* Dự án được xây dựng theo Layered Architecture (kiến trúc phân tầng), áp dụng chặt chẽ nguyên lý SOLID các tầng giao tiếp với nhau thông qua các Abstraction (Interfaces) nhờ cơ chế Dependency Injection (DI) mặc định của ASP.NET Core giúp giảm phụ thuộc và tăng khả năng mở rộng.
Tầng API (Presentation Layer): Là nơi là tiếp nhận HTTP Requests, điều phối request xuống tầng Services và trả về kết quả được chuẩn hóa qua ApiResponse<T> kèm theo HTTP Status Codes phù hợp.
Tầng BLL (Business Logic Layer): Là nơi xử lý nghiệp vụ. Chứa Services, Validators và DTOs. BLL đảm nhiệm việc mapping dữ liệu (giữa DTOs và Entities), và điều phối các thao tác xử lý dữ liệu. Nhận data từ Controller, map sang Entity, xử lý nghiệp vụ, chạy Validation và điều phối việc gọi Database. Việc xử lý logic tại đây giúp code dễ dàng được viết Unit Test mà không phụ thuộc vào cơ sở dữ liệu hay giao diện.
Tầng DAL (Data Access Layer): Tầng chuyên biệt hóa cho việc tương tác với cơ sở dữ liệu (SQL Server), Chứa Repositories và thực hiện các thao tác CRUD và truy vấn dữ liệu thông qua Entity Framework Core kết hợp với Repository Pattern. Tầng này trừu tượng hóa toàn bộ chi tiết về cơ sở dữ liệu, đảm bảo tầng BLL ở trên không cần biết dữ liệu được lưu trữ hay truy xuất bằng công nghệ nào (Dependency Inversion).

----------------------------------------------------
## Database
* Về tổng thể, kiến trúc database thì em tạo ra 3 bang chính gồm có Forms – FormFields – Submissions (vì em không rõ nghiệp vụ hơn nên không biết về phía user em có tạo ra id và role để chia ra). Bảng Forms dùng để lưu thông tin tổng quan của một form như tiêu đề, mô tả, trạng thái và thứ tự hiển thị. Em thêm UserId giúp xác định ai là người tạo form, từ đó hỗ trợ phân quyền và quản lý. Bảng FormFields định nghĩa cấu trúc của form, trong đó mỗi field có Name (được unique trong từng form) để làm key mapping với dữ liệu submit. Thuộc tính Configuration được lưu dưới dạng JSON giúp hệ thống có thể cấu hình động cho từng loại field (ví dụ: maxLength cho text, options cho select) mà không cần thay đổi schema database. Bảng Submissions sẽ có thể lưu dữ liệu người dùng gửi lên, với cột Data dạng JSON. Thiết kế này cho phép lưu trữ dữ liệu linh hoạt theo từng form khác nhau mà không cần tạo bảng riêng cho mỗi form, giúp giảm độ phức tạp và tăng khả năng mở rộng. 
* Em có sự dụng UNIQUEIDENTIFIER kết hợp với NEWSEQUENTIALID() để đảm bảo tính duy nhất và phù hợp với hệ thống có khả năng scale hoặc phân tán, đồng thời giảm fragmentation so với GUID ngẫu nhiên. Em cũng có đánh index như là lấy danh sách form active (Status, DisplayOrder), lấy danh sách field theo form (FormId, DisplayOrder) hay xem submissions mới nhất (FormId, SubmittedAt DESC). Việc sử dụng composite index kết hợp include giúp tối ưu hiệu năng truy vấn.

----------------------------------------------------
## Công nghệ sử dụng
* **Framework:** ASP.NET Core 8 Web API
* **Ngôn ngữ:** C# 
* **Database:** SQL Server
* **ORM:** Entity Framework Core 8 
* **Containerization:** Docker & Docker Compose
-------------------------
Note: Còn nhiều chỗ em chưa test được bao quát hết vì thời gian nên có chỗ nào chưa hoàn chỉnh thì mong anh/chị có thể chỉ bảo thêm ạ. Em xin cảm ơn.
----------------------

## Hướng dẫn chạy dự án

Dự án hỗ trợ 2 cách chạy: Sử dụng Docker hoặc chạy trực tiếp trên máy tính.

### CÁCH 1: Chạy bằng Docker 
**Yêu cầu:** Máy tính đã cài đặt [Docker Desktop](https://www.docker.com/products/docker-desktop/).

1. Mở Terminal / Command Prompt tại thư mục gốc của dự án (nơi có file `docker-compose.yml`).
2. Chạy câu lệnh sau:
   ```bash
   docker-compose up -d --build
   ```
3. Chờ khoảng 1-2 phút để Docker tải image và khởi động.
(Lưu ý: API đã được cấu hình tự động chạy EF Core Migrations khi khởi động, nên Database và các bảng sẽ được tự động tạo sẵn).
### CÁCH 2: Chạy Local (Thủ công) 
**Note::**
- Sự dung bản .NET 8 SDK
- Có SQL Server  

**Bước 1:** Cấu hình database  
Mở file `appsettings.json` trong project `dynamic_form_system` và chỉnh lại chuỗi `DefaultConnection` cho phù hợp với SQL Server của anh/chị.

**Bước 2:** Cập nhật database
Mở Terminal tại thư mục DynamicForm.

```bash
dotnet ef database update
```
Khởi chạy ứng dụng:

```Bash
dotnet run
```
----------------------------------------
## API Documentation (Swagger)

* Sau khi ứng dụng khởi chạy thành công, có thể truy cập giao diện Swagger để xem tài liệu chi tiết và test trực tiếp các API:

- **URL (Docker):** http://localhost:8080/swagger
* Note: Nếu cổng 8080 không được thì anh/chị có thể chuyển sang cổng khác ạ 
- **URL (Local):** https://localhost:"Port"/swagger  

### Các luồng nghiệp vụ cơ bản (API Workflows)

**Dành cho Quản trị viên (Admin):**  
* Em cũng có để id của user để test ạ
* 103F42D6-EE32-4CA9-BED1-137989DE5C2C - role Employee
* 1BE5A09D-2240-43CC-8376-5944631D2ED3 - role Admin

#### Form Management

* `[POST] /api/forms`: Tạo form và field.

```json
{
  "title": "Khảo sát nhân viên",
  "description": "Form đánh giá hiệu suất quý 1",
  "displayOrder": 1,
  "status": "Active",
  "fields": [
    {
      "name": "FullName",
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
}
```

* `[GET] /api/forms/{id}`: Lấy form theo id của form 


* `[PUT] /api/forms/{id}`: Update thông tin form 

```json
{
  "title": "Khảo sát nhân viên 2026",
  "description": "Form đánh giá hiệu suất quý 2026",
  "displayOrder": 1,
  "status": "Draft"
}
```

* `[Delete] /api/forms/{id}`: Xoá thông tin form  
* Note: vì em khong rõ nghiệp vụ là xoá form thì field của đã có data được điền rồi sẽ xử lý thế nào và em không đủ thời gian nên mới chỉ có api còn chưa làm hoàn chỉnh ạ

----------------------

#### Field Management

* `[POST] /api/forms/{formId}/fields`: Tạo field mới 

```json
{
  "name": "fullName",
  "label": "Họ và tên nhân viên",
  "fieldType": "text",
  "isRequired": true,
  "configuration": "{\"maxLength\":20,\"minLength\":3,\"placeholder\":\"Nhập họ tên\",\"regex\":\"^[A-Za-zÀ-ỹ\\\\s]+$\"}",
  "displayOrder": 1
}
```

* `[PUT] /api/forms/{formId}/fields/{fid}`: Update field   

```json
{
  "name": "fullName",
  "label": "Họ và tên nhân viên phòng ban",
  "fieldType": "text",
  "isRequired": true,
  "configuration": "{\"maxLength\":20}",
  "displayOrder": 2
}
```

* `[PUT] /api/forms/{formId}/fields/reorder`: update lại thứ tự của field 
* Note: id là id của field vừa tạo

```json
{
  "fields": [
    {
      "id": "475B0294-CD1F-4B88-987C-039A202F61EC",
      "displayOrder": 1
    }
  ]
}
```
----------
**Dành cho nhân viên (SW):**

#### Submissions
* `[GET] /api/forms/active`: lấy danh sách form active cho nhân viên 


* `[POST] /api/forms/{id}/submit`: submit data được điền trong form 
 
```json
{
  "data": "{\"FullName\": \"Nguyễn Văn A\", \"age\": 25}"
}
```

* `[GET] /api/submissions`: xem lại những bài đã nộp 

