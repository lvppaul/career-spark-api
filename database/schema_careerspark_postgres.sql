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

--News
CREATE TABLE "News" (
    "Id" SERIAL PRIMARY KEY,
    "Title" VARCHAR(255) NOT NULL,
    "Content" TEXT NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "ImageUrl" VARCHAR(255),
    "avatarPublicId" VARCHAR(200)
);

--CareerVectors: Langflow AI (chỉ chạy lệnh dưới khi dùng bên supabase)
-- Bật extension vector nếu chưa có
-- create extension if not exists vector;

-- -- Tạo bảng lưu embeddings
-- create table if not exists "CareerVectors" (
--    id UUID PRIMARY KEY,
--   content text,               -- nội dung gốc (ví dụ đoạn văn, file .txt)
--   metadata jsonb,             -- thông tin phụ (tên file, query name, v.v.)
--   embedding vector(1536)      -- vector embedding (độ dài 1536 cho model text-embedding-3-large)
-- );
-- -- Index
-- CREATE INDEX IF NOT EXISTS careervectors_embedding_idx 
-- ON "CareerVectors" 
-- USING ivfflat (embedding vector_cosine_ops)
-- WITH (lists = 100);
--   -- Xóa hàm cũ trước (với đúng kiểu tham số)
-- DROP FUNCTION IF EXISTS match_documents(vector, double precision, integer);
-- -- Sau đó tạo lại hàm đúng
-- CREATE OR REPLACE FUNCTION match_documents(
--   query_embedding VECTOR(1536),
--   match_threshold FLOAT DEFAULT 0.7,
--   match_count INT DEFAULT 4
-- )
-- RETURNS TABLE (
--   id UUID,
--   content TEXT,
--   metadata JSONB,
--   similarity FLOAT
-- )
-- LANGUAGE plpgsql
-- AS $$
-- BEGIN
--   RETURN QUERY
--   SELECT
--     "CareerVectors".id,
--     "CareerVectors".content,
--     "CareerVectors".metadata,
--     1 - ("CareerVectors".embedding <=> query_embedding) AS similarity
--   FROM "CareerVectors"
--   WHERE 1 - ("CareerVectors".embedding <=> query_embedding) > match_threshold
--   ORDER BY "CareerVectors".embedding <=> query_embedding
--   LIMIT match_count;
-- END;
-- $$;
