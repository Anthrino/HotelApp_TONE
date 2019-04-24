USE master
GO
IF NOT EXISTS (
   SELECT name
   FROM sys.databases
   WHERE name = N'HotelDB'
)
CREATE DATABASE HotelDB
GO


use HotelDB;

CREATE TABLE dbo.item  
(  
	Id int IDENTITY(1,1) Primary Key,
	title varchar(20), 
	category varchar(25), 
	description varchar(25), 
	price Numeric 
); 

CREATE TABLE dbo.users
(  
    Id int IDENTITY(1,1) Primary Key,
	username varchar(25),
	password varchar(12),
	role int
);

CREATE TABLE dbo.cart  
(  
    Id int IDENTITY(1,1) Primary Key,
	userId int,
	itemId int,
	constraint FK_cart_user foreign key (userId) references dbo.users (Id),
	constraint FK_cart_item foreign key (itemId) references dbo.item (Id)
); 
