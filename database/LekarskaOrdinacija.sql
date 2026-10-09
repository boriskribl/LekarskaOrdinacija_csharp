IF DB_ID(N'LekarskaOrdinacijaDb') IS NULL CREATE DATABASE LekarskaOrdinacijaDb;
GO
USE LekarskaOrdinacijaDb;
GO

CREATE TABLE AspNetRoles (
 Id nvarchar(450) NOT NULL PRIMARY KEY, Name nvarchar(256) NULL, NormalizedName nvarchar(256) NULL,
 ConcurrencyStamp nvarchar(max) NULL
);
CREATE UNIQUE INDEX RoleNameIndex ON AspNetRoles(NormalizedName) WHERE NormalizedName IS NOT NULL;

CREATE TABLE AspNetUsers (
 Id nvarchar(450) NOT NULL PRIMARY KEY, UserName nvarchar(256) NULL, NormalizedUserName nvarchar(256) NULL,
 Email nvarchar(256) NULL, NormalizedEmail nvarchar(256) NULL, EmailConfirmed bit NOT NULL DEFAULT 0,
 PasswordHash nvarchar(max) NULL, SecurityStamp nvarchar(max) NULL, ConcurrencyStamp nvarchar(max) NULL,
 PhoneNumber nvarchar(max) NULL, PhoneNumberConfirmed bit NOT NULL DEFAULT 0, TwoFactorEnabled bit NOT NULL DEFAULT 0,
 LockoutEnd datetimeoffset NULL, LockoutEnabled bit NOT NULL DEFAULT 0, AccessFailedCount int NOT NULL DEFAULT 0
);
CREATE INDEX EmailIndex ON AspNetUsers(NormalizedEmail);
CREATE UNIQUE INDEX UserNameIndex ON AspNetUsers(NormalizedUserName) WHERE NormalizedUserName IS NOT NULL;

CREATE TABLE AspNetRoleClaims (Id int IDENTITY PRIMARY KEY, RoleId nvarchar(450) NOT NULL, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL, CONSTRAINT FK_RoleClaims_Roles FOREIGN KEY(RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE);
CREATE INDEX IX_AspNetRoleClaims_RoleId ON AspNetRoleClaims(RoleId);
CREATE TABLE AspNetUserClaims (Id int IDENTITY PRIMARY KEY, UserId nvarchar(450) NOT NULL, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL, CONSTRAINT FK_UserClaims_Users FOREIGN KEY(UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE);
CREATE INDEX IX_AspNetUserClaims_UserId ON AspNetUserClaims(UserId);
CREATE TABLE AspNetUserLogins (LoginProvider nvarchar(128) NOT NULL, ProviderKey nvarchar(128) NOT NULL, ProviderDisplayName nvarchar(max) NULL, UserId nvarchar(450) NOT NULL, PRIMARY KEY(LoginProvider,ProviderKey), CONSTRAINT FK_UserLogins_Users FOREIGN KEY(UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE);
CREATE INDEX IX_AspNetUserLogins_UserId ON AspNetUserLogins(UserId);
CREATE TABLE AspNetUserRoles (UserId nvarchar(450) NOT NULL, RoleId nvarchar(450) NOT NULL, PRIMARY KEY(UserId,RoleId), CONSTRAINT FK_UserRoles_Users FOREIGN KEY(UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE, CONSTRAINT FK_UserRoles_Roles FOREIGN KEY(RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE);
CREATE INDEX IX_AspNetUserRoles_RoleId ON AspNetUserRoles(RoleId);
CREATE TABLE AspNetUserTokens (UserId nvarchar(450) NOT NULL, LoginProvider nvarchar(128) NOT NULL, Name nvarchar(128) NOT NULL, Value nvarchar(max) NULL, PRIMARY KEY(UserId,LoginProvider,Name), CONSTRAINT FK_UserTokens_Users FOREIGN KEY(UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE);

CREATE TABLE Patients (
 Id int IDENTITY PRIMARY KEY, FirstName nvarchar(60) NOT NULL, LastName nvarchar(60) NOT NULL,
 Address nvarchar(150) NULL, DateOfBirth datetime2 NOT NULL, Phone nvarchar(30) NULL, Email nvarchar(120) NULL
);
CREATE TABLE MedicalServices (
 Id int IDENTITY PRIMARY KEY, Name nvarchar(120) NOT NULL, Price decimal(10,2) NOT NULL,
 DurationMinutes int NOT NULL, Description nvarchar(500) NULL
);
CREATE TABLE Doctors (
 Id int IDENTITY PRIMARY KEY, FullName nvarchar(120) NOT NULL, Specialization nvarchar(120) NOT NULL,
 Phone nvarchar(30) NULL, Email nvarchar(120) NULL
);
CREATE TABLE Nurses (
 Id int IDENTITY PRIMARY KEY, FullName nvarchar(120) NOT NULL, Phone nvarchar(30) NULL, Email nvarchar(120) NULL
);
CREATE TABLE DoctorSchedules (
 Id int IDENTITY PRIMARY KEY, DoctorId int NOT NULL, DayOfWeek int NOT NULL, StartTime time NOT NULL, EndTime time NOT NULL,
 CONSTRAINT FK_DoctorSchedules_Doctors FOREIGN KEY(DoctorId) REFERENCES Doctors(Id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX IX_DoctorSchedules_DoctorId_DayOfWeek_StartTime ON DoctorSchedules(DoctorId,DayOfWeek,StartTime);
CREATE TABLE Appointments (
 Id int IDENTITY PRIMARY KEY, PatientId int NOT NULL, DoctorId int NOT NULL, MedicalServiceId int NOT NULL,
 Start datetime2 NOT NULL, Status int NOT NULL DEFAULT 0, FinalPrice decimal(10,2) NOT NULL DEFAULT 0, Note nvarchar(1000) NULL,
 CONSTRAINT FK_Appointments_Patients FOREIGN KEY(PatientId) REFERENCES Patients(Id),
 CONSTRAINT FK_Appointments_Doctors FOREIGN KEY(DoctorId) REFERENCES Doctors(Id),
 CONSTRAINT FK_Appointments_MedicalServices FOREIGN KEY(MedicalServiceId) REFERENCES MedicalServices(Id)
);
GO

INSERT INTO Patients(FirstName,LastName,Address,DateOfBirth,Phone,Email) VALUES
(N'Ana',N'Jovanović',N'Bulevar 1, Beograd','1991-04-12',N'060111222',N'ana@example.com'),
(N'Luka',N'Petrović',N'Ulica 2, Beograd',DATEADD(year,-8,GETDATE()),N'060333444',N'luka@example.com'),
(N'Mina',N'Simić',N'Ulica 3, Beograd','1985-11-03',N'060555666',N'mina@example.com');
INSERT INTO Doctors(FullName,Specialization,Phone,Email) VALUES
(N'dr Marko Ilić',N'Opšta medicina',N'011123456',N'marko@ordinacija.rs'),
(N'dr Jelena Nikolić',N'Pedijatrija',N'011123457',N'jelena@ordinacija.rs');
INSERT INTO Nurses(FullName,Phone,Email) VALUES (N'Milica Savić',N'011222333',N'milica@ordinacija.rs');
INSERT INTO MedicalServices(Name,Price,DurationMinutes,Description) VALUES
(N'Opšti pregled',3500,30,N'Standardni pregled lekara opšte prakse.'),
(N'Pedijatrijski pregled',4000,30,N'Pregled deteta kod pedijatra.'),
(N'Terapija',2500,20,N'Primena propisane terapije.');

DECLARE @d int=1;
WHILE @d<=2
BEGIN
 INSERT INTO DoctorSchedules(DoctorId,DayOfWeek,StartTime,EndTime) VALUES
 (@d,1,'08:00','16:00'),(@d,2,'08:00','16:00'),(@d,3,'08:00','16:00'),(@d,4,'08:00','16:00'),(@d,5,'08:00','16:00');
 SET @d=@d+1;
END;
INSERT INTO Appointments(PatientId,DoctorId,MedicalServiceId,Start,Status,FinalPrice,Note) VALUES
(1,1,1,DATEADD(hour,9,CAST(DATEADD(day,1,CAST(GETDATE() AS date)) AS datetime2)),0,3500,N'Kontrolni pregled');
GO

-- Identity uloge i test korisnike aplikacija automatski kreira pri prvom pokretanju kroz DbInitializer.
-- Kredencijali: admin/Admin123!, lekar/Lekar123!, sestra/Sestra123!
