-- 1. table FORMS
CREATE TABLE Forms (
    Id UNIQUEIDENTIFIER PRIMARY KEY, 
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    DisplayOrder INT NOT NULL,
    Status Varchar(10) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 2. table FIELDS
CREATE TABLE FormFields (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FormId UNIQUEIDENTIFIER NOT NULL,
    Name VARCHAR(100) NOT NULL, 
    Label NVARCHAR(100) NOT NULL,
    FieldType VARCHAR(20) NOT NULL, 
    DisplayOrder INT NOT NULL,
    IsRequired BIT NOT NULL DEFAULT 0,
    Configuration NVARCHAR(MAX) NOT NULL DEFAULT '{}', 
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_FormFields_Forms FOREIGN KEY (FormId) REFERENCES Forms(Id),
    CONSTRAINT UQ_FormFields_FormId_Name UNIQUE (FormId, Name),
);

-- 3. BẢNG SUBMISSIONS
CREATE TABLE Submissions (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FormId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER,
    SubmittedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Data NVARCHAR(MAX) NOT NULL DEFAULT '{}',
    CONSTRAINT FK_Submissions_Forms FOREIGN KEY (FormId) REFERENCES Forms(Id),
);


CREATE NONCLUSTERED INDEX IX_FormFields_FormId ON FormFields(FormId);
CREATE NONCLUSTERED INDEX IX_Submissions_FormId ON Submissions(FormId);
CREATE NONCLUSTERED INDEX IX_Submissions_UserId ON Submissions(UserId);

-- INDEXING STRATEGY 

-- GET /api/forms/active (Nhân viên lấy danh sách form để điền)
-- Dùng Composite Index + Covering Index (INCLUDE) để query không cần look-up bảng gốc.
CREATE NONCLUSTERED INDEX IX_Forms_Active_DisplayOrder 
ON Forms(Status, IsDeleted, DisplayOrder) 
INCLUDE (Title, Description); 

-- GET /api/forms/{id} (Lấy danh sách field của form, luôn luôn phải sort theo order)
CREATE NONCLUSTERED INDEX IX_FormFields_FormId_DisplayOrder 
ON FormFields(FormId, DisplayOrder) 
INCLUDE (Name, Label, FieldType, IsRequired);

-- GET /api/submissions (Admin xem danh sách nộp theo Form, xem cái mới nhất trước)
CREATE NONCLUSTERED INDEX IX_Submissions_FormId_SubmittedAt 
ON Submissions(FormId, SubmittedAt DESC);