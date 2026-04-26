create database dynamic_form
use dynamic_form

-- bang de user test phan role
CREATE TABLE Users (
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(), 
  Roles varchar(10)
)

CREATE TABLE Forms (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(), 
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    DisplayOrder INT NOT NULL,
    Status Varchar(10) NOT NULL,
    UserId UNIQUEIDENTIFIER,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Forms_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
);

CREATE TABLE FormFields (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    FormId UNIQUEIDENTIFIER NOT NULL,
    Name VARCHAR(100) NOT NULL, 
    Label NVARCHAR(100) NOT NULL,
    FieldType VARCHAR(20) NOT NULL, 
    DisplayOrder INT NOT NULL,
    IsRequired BIT NOT NULL DEFAULT 0,
    Configuration NVARCHAR(MAX) NOT NULL DEFAULT '{}', 
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_FormFields_Forms FOREIGN KEY (FormId) REFERENCES Forms(Id),
    CONSTRAINT UQ_FormFields_FormId_Name UNIQUE (FormId, Name)
);

CREATE TABLE Submissions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT  NEWSEQUENTIALID(),
    FormId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER,
    SubmittedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Data NVARCHAR(MAX) NOT NULL DEFAULT '{}',
    CONSTRAINT FK_Submissions_Forms FOREIGN KEY (FormId) REFERENCES Forms(Id),
    CONSTRAINT FK_Submissions_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);


CREATE NONCLUSTERED INDEX IX_FormFields_FormId ON FormFields(FormId);
CREATE NONCLUSTERED INDEX IX_Submissions_FormId ON Submissions(FormId);
CREATE NONCLUSTERED INDEX IX_Submissions_UserId ON Submissions(UserId);

-- INDEXING 
-- GET /api/forms/active 
CREATE NONCLUSTERED INDEX IX_Forms_Active_DisplayOrder 
ON Forms(Status, DisplayOrder) 
INCLUDE (Title, Description); 

-- GET /api/forms/{id} (Lấy danh sách field của form, luôn luôn phải sort theo order)
CREATE NONCLUSTERED INDEX IX_FormFields_FormId_DisplayOrder 
ON FormFields(FormId, DisplayOrder) 
INCLUDE (Name, Label, FieldType, IsRequired);

-- GET /api/submissions 
CREATE NONCLUSTERED INDEX IX_Submissions_FormId_SubmittedAt 
ON Submissions(FormId, SubmittedAt DESC);