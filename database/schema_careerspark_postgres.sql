-- ========================================
-- DROP & CREATE DATABASE (chạy riêng nếu cần)
-- ========================================
 --DROP DATABASE IF EXISTS "CareerSparkDB";
 --CREATE DATABASE "CareerSparkDB";
 --\c CareerSparkDB

-- ========================================
-- CREATE TABLES
-- ========================================

-- Role
CREATE TABLE "Role" (
    "Id" SERIAL PRIMARY KEY,
    "RoleName" VARCHAR(100) NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE
);

-- User
CREATE TABLE "User" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(200) NOT NULL,
    "Phone" VARCHAR(20),
    "Email" VARCHAR(200) UNIQUE,
    "Password" VARCHAR(255),
    "RefreshToken" VARCHAR(500),
    "ExpiredRefreshTokenAt" TIMESTAMPTZ,
    "avatarURL" VARCHAR(255),
	"avatarPublicId" VARCHAR(200),
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	"IsVerified" BOOLEAN NOT NULL DEFAULT FALSE,
	"SecurityStamp" VARCHAR(100) NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "RoleId" INT NOT NULL REFERENCES "Role"("Id") ON DELETE CASCADE
);

-- Blogs
CREATE TABLE "Blogs" (
    "Id" SERIAL PRIMARY KEY,
	"AuthorId" INT NOT NULL REFERENCES "User"("Id") ON DELETE CASCADE,
    "Title" VARCHAR(255) NOT NULL,
    "Content" TEXT NOT NULL,
    "CreateAt" TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMPTZ
);

-- Comments
CREATE TABLE "Comments" (
    "Id" SERIAL PRIMARY KEY,
    "Content" TEXT NOT NULL,
    "UserId" INT NOT NULL REFERENCES "User"("Id") ON DELETE CASCADE,
    "BlogId" INT NOT NULL REFERENCES "Blogs"("Id") ON DELETE CASCADE,
    "CreateAt" TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    "UpdateAt" TIMESTAMPTZ
);

-- QuestionTest
CREATE TABLE "QuestionTest" (
    "Id" SERIAL PRIMARY KEY,
    "Content" TEXT NOT NULL,
    "QuestionType" VARCHAR(50) NOT NULL
);

-- TestSession
CREATE TABLE "TestSession" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL REFERENCES "User"("Id") ON DELETE CASCADE,
    "StartAt" TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

-- TestAnswer
CREATE TABLE "TestAnswer" (
    "Id" SERIAL PRIMARY KEY,
    "IsSelected" BOOLEAN DEFAULT FALSE,
    "QuestionId" INT NOT NULL REFERENCES "QuestionTest"("Id") ON DELETE CASCADE,
    "TestSessionId" INT NOT NULL REFERENCES "TestSession"("Id") ON DELETE CASCADE
);

-- Result
CREATE TABLE "Result" (
    "Id" SERIAL PRIMARY KEY,
    "Content" TEXT,
    "R" INT,
    "I" INT,
    "A" INT,
    "S" INT,
    "E" INT,
    "C" INT,
    "TestSessionId" INT NOT NULL REFERENCES "TestSession"("Id") ON DELETE CASCADE
);

-- TestHistory
CREATE TABLE "TestHistory" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL REFERENCES "User"("Id") ON DELETE CASCADE,
    "TestSessionId" INT NOT NULL REFERENCES "TestSession"("Id") ON DELETE CASCADE,
    "TestAnswerId" INT NOT NULL REFERENCES "TestAnswer"("Id") ON DELETE CASCADE
);

-- CareerField
CREATE TABLE "CareerField" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(200) NOT NULL,
    "Description" TEXT
);

-- CareerPath
CREATE TABLE "CareerPath" (
    "Id" SERIAL PRIMARY KEY,
    "Title" VARCHAR(255) NOT NULL,
    "Description" TEXT,
    "CareerFieldId" INT NOT NULL REFERENCES "CareerField"("Id") ON DELETE CASCADE
);

-- CareerRoadmap
CREATE TABLE "CareerRoadmap" (
    "Id" SERIAL PRIMARY KEY,
    "CareerPathId" INT NOT NULL REFERENCES "CareerPath"("Id") ON DELETE CASCADE,
    "StepOrder" INT NOT NULL,
    "Title" VARCHAR(200) NOT NULL,
    "Description" VARCHAR(500),
    "SkillFocus" VARCHAR(200),
    "DifficultyLevel" VARCHAR(50),
    "DurationWeeks" INT,
    "SuggestedCourseUrl" VARCHAR(255)
);

-- SubscriptionPlan
CREATE TABLE "SubscriptionPlan" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(200) NOT NULL,
    "Price" DECIMAL(18,2) NOT NULL,
    "DurationDays" INT NOT NULL,
    "Description" TEXT,
	"Benefits" TEXT,
    "Level" INT NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- UserSubscription
CREATE TABLE "UserSubscription" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL REFERENCES "User"("Id") ON DELETE CASCADE,
    "PlanId" INT NOT NULL REFERENCES "SubscriptionPlan"("Id") ON DELETE CASCADE,
    "StartDate" DATE NOT NULL,
    "EndDate" DATE NOT NULL,
    "IsActive" BOOLEAN DEFAULT TRUE
);

-- CareerMapping
CREATE TABLE "CareerMapping" (
    "Id" SERIAL PRIMARY KEY,
    "RiasecType" VARCHAR(20) NOT NULL,
    "CareerFieldId" INT NOT NULL REFERENCES "CareerField"("Id") ON DELETE CASCADE
);

-- Orders
CREATE TABLE "Orders" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL REFERENCES "User"("Id") ON DELETE CASCADE,
    "SubscriptionPlanId" INT NOT NULL REFERENCES "SubscriptionPlan"("Id") ON DELETE CASCADE,
    "Amount" DECIMAL(18,2) NOT NULL,
    "Status" VARCHAR(50) NOT NULL DEFAULT 'Pending',
    "VnPayTransactionId" VARCHAR(255),
    "VnPayOrderInfo" VARCHAR(500),
    "VnPayResponseCode" VARCHAR(10),
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "PaidAt" TIMESTAMPTZ,
    "ExpiredAt" TIMESTAMPTZ
);
