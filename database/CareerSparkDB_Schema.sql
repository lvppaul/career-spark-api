-- Nếu DB đã tồn tại thì xóa đi
IF DB_ID('CareerSparkDB') IS NOT NULL
BEGIN
    ALTER DATABASE CareerSparkDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CareerSparkDB;
END
GO

-- Tạo database mới
CREATE DATABASE CareerSparkDB;
GO

USE CareerSparkDB;
GO

-- Bảng Role
CREATE TABLE Role (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL
);

-- Bảng User
CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(200) UNIQUE,
    Password NVARCHAR(255) NOT NULL, 
    RefreshToken NVARCHAR(500),
    ExpiredRefreshTokenAt DATETIME, 
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    RoleId INT NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Role(Id)
);

-- Bảng Blogs
CREATE TABLE Blogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreateAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME
);

-- Bảng Comments
CREATE TABLE Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX) NOT NULL,
    UserId INT NOT NULL,
    BlogId INT NOT NULL,
    CreateAt DATETIME DEFAULT GETDATE(),
    UpdateAt DATETIME,
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (BlogId) REFERENCES Blogs(Id)
);

-- Bảng QuestionTest (đã bỏ Description, CreateAt, UpdateAt)
CREATE TABLE QuestionTest (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX) NOT NULL,
    QuestionType NVARCHAR(50) NOT NULL
);

-- Bảng TestSession
CREATE TABLE TestSession (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    StartAt DATETIME DEFAULT GETDATE(),
    EndAt DATETIME,
    FOREIGN KEY (UserId) REFERENCES [User](Id)
);

-- Bảng TestAnswer (đã bỏ Content dư thừa)
CREATE TABLE TestAnswer (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IsSelected BIT DEFAULT 0,
    QuestionId INT NOT NULL,
    TestSessionId INT NOT NULL,
    FOREIGN KEY (QuestionId) REFERENCES QuestionTest(Id),
    FOREIGN KEY (TestSessionId) REFERENCES TestSession(Id)
);

-- Bảng Result
CREATE TABLE Result (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX),
    R INT,
    I INT,
    A INT,
    S INT,
    E INT,
    C INT,
    TestSessionId INT NOT NULL,
    FOREIGN KEY (TestSessionId) REFERENCES TestSession(Id)
);

-- Bảng TestHistory
CREATE TABLE TestHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    TestSessionId INT NOT NULL,
    TestAnswerId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (TestSessionId) REFERENCES TestSession(Id),
    FOREIGN KEY (TestAnswerId) REFERENCES TestAnswer(Id)
);

-- Bảng CareerField
CREATE TABLE CareerField (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX)
);

-- Bảng CareerPath
CREATE TABLE CareerPath (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    CareerFieldId INT NOT NULL,
    FOREIGN KEY (CareerFieldId) REFERENCES CareerField(Id)
);

-- Bảng CareerMileStone
CREATE TABLE CareerMileStone (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CareerPathId INT NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    [Index] INT NOT NULL,
    SuggestedCourseUrl NVARCHAR(500),
    FOREIGN KEY (CareerPathId) REFERENCES CareerPath(Id)
);

-- Bảng SubscriptionPlan
CREATE TABLE SubscriptionPlan (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    DurationDays INT NOT NULL,
    Description NVARCHAR(MAX)
);

-- Bảng UserSubscription
CREATE TABLE UserSubscription (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    PlanId INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (PlanId) REFERENCES SubscriptionPlan(Id)
);
