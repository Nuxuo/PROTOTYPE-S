--CREATE DATABASE Restful_API;
--USE Restful_API;

CREATE TABLE Users (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Firstname NVARCHAR(50),
	Lastname NVARCHAR(50),
	Email NVARCHAR(100),
	Password NVARCHAR(100),
	SoftDeleted BIT,
	CreatedDate DATE,
	UpdatedDate DATE
);

CREATE TABLE Posts (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Headline NVARCHAR(50),
	Content NVARCHAR(500),
	Likes INT,
	UserGuid INT REFERENCES Users(Id),
	CreatedDate DATE,
	UpdatedDate DATE
);

CREATE TABLE Comments (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Content NVARCHAR(100),
	Likes INT,
	UserGuid INT REFERENCES Users(Id),
	PostGuid INT REFERENCES Posts(Id),
	SoftDeleted BIT,
	CreatedDate DATE,
	UpdatedDate DATE
);

CREATE TABLE Tags (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(50),
);

CREATE TABLE UserTags (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Likes INT,
	UserGuid INT REFERENCES Users(Id),
	TagGuid INT REFERENCES Tags(Id)

);

CREATE TABLE PostTags (
	Id INT PRIMARY KEY IDENTITY(1,1),
	PostGuid INT REFERENCES Posts(Id),
	TagGuid INT REFERENCES Tags(Id)
);

CREATE TABLE UserPostRelations (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Liked BIT,
	PostGuid INT REFERENCES Posts(Id),
	UserGuid INT REFERENCES Users(Id)
);


CREATE TABLE UserCommentRelations (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Liked BIT,
	CommentGuid INT REFERENCES Comments(Id),
	UserGuid INT REFERENCES Users(Id)
);