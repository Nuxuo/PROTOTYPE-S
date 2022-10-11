--CREATE DATABASE Restful_API;
--USE Restful_API;

CREATE TABLE Users (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	Firstname NVARCHAR(50),
	Lastname NVARCHAR(50),
	Email NVARCHAR(100),
	Password NVARCHAR(100),
	SoftDeleted BIT,
	CreatedDate DATE,
	UpdatedDate DATE
);

CREATE TABLE Posts (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	Headline NVARCHAR(50),
	Content NVARCHAR(500),
	Likes INT,
	UserId UNIQUEIDENTIFIER REFERENCES Users(Id),
	CreatedDate DATE,
	UpdatedDate DATE
);

CREATE TABLE Comments (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	Content NVARCHAR(100),
	Likes INT,
	UserId UNIQUEIDENTIFIER REFERENCES Users(Id),
	PostId UNIQUEIDENTIFIER REFERENCES Posts(Id),
	SoftDeleted BIT,
	CreatedDate DATE,
	UpdatedDate DATE
);

CREATE TABLE Tags (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	Name NVARCHAR(50),
);

CREATE TABLE UserTags (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	Likes INT,
	UserId UNIQUEIDENTIFIER REFERENCES Users(Id),
	TagId UNIQUEIDENTIFIER REFERENCES Tags(Id)

);

CREATE TABLE PostTags (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	PostId UNIQUEIDENTIFIER REFERENCES Posts(Id),
	TagId UNIQUEIDENTIFIER REFERENCES Tags(Id)
);

CREATE TABLE UserPostRelations (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	Liked BIT,
	PostId UNIQUEIDENTIFIER REFERENCES Posts(Id),
	UserId UNIQUEIDENTIFIER REFERENCES Users(Id)
);


CREATE TABLE UserCommentRelations (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	Liked BIT,
	CommentId UNIQUEIDENTIFIER REFERENCES Comments(Id),
	UserId UNIQUEIDENTIFIER REFERENCES Users(Id)
);