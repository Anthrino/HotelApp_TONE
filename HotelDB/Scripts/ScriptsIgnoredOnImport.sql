
/****** Object:  Database [HotelDB]    Script Date: 5/14/2019 12:51:45 AM ******/
CREATE DATABASE [HotelDB]  ;
GO

ALTER DATABASE [HotelDB] SET ANSI_NULL_DEFAULT OFF
GO

ALTER DATABASE [HotelDB] SET ANSI_NULLS OFF
GO

ALTER DATABASE [HotelDB] SET ANSI_PADDING OFF
GO

ALTER DATABASE [HotelDB] SET ANSI_WARNINGS OFF
GO

ALTER DATABASE [HotelDB] SET ARITHABORT OFF
GO

ALTER DATABASE [HotelDB] SET AUTO_SHRINK OFF
GO

ALTER DATABASE [HotelDB] SET AUTO_UPDATE_STATISTICS ON
GO

ALTER DATABASE [HotelDB] SET CURSOR_CLOSE_ON_COMMIT OFF
GO

ALTER DATABASE [HotelDB] SET CONCAT_NULL_YIELDS_NULL OFF
GO

ALTER DATABASE [HotelDB] SET NUMERIC_ROUNDABORT OFF
GO

ALTER DATABASE [HotelDB] SET QUOTED_IDENTIFIER OFF
GO

ALTER DATABASE [HotelDB] SET RECURSIVE_TRIGGERS OFF
GO

ALTER DATABASE [HotelDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO

ALTER DATABASE [HotelDB] SET DATE_CORRELATION_OPTIMIZATION OFF
GO

ALTER DATABASE [HotelDB] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO

ALTER DATABASE [HotelDB] SET PARAMETERIZATION SIMPLE
GO

ALTER DATABASE [HotelDB] SET READ_COMMITTED_SNAPSHOT OFF
GO

ALTER DATABASE [HotelDB] SET  MULTI_USER
GO

ALTER DATABASE [HotelDB] SET QUERY_STORE = OFF
GO

/****** Object:  Table [dbo].[cart]    Script Date: 5/14/2019 12:51:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[item]    Script Date: 5/14/2019 12:51:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[orders]    Script Date: 5/14/2019 12:51:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[users]    Script Date: 5/14/2019 12:51:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET IDENTITY_INSERT [dbo].[cart] ON
GO

INSERT [dbo].[cart] ([Id], [userId], [itemId], [quantity]) VALUES (1037, 1, 1, 3)
GO

INSERT [dbo].[cart] ([Id], [userId], [itemId], [quantity]) VALUES (1038, 2, 2, 1)
GO

INSERT [dbo].[cart] ([Id], [userId], [itemId], [quantity]) VALUES (1039, 2, 1, 1)
GO

SET IDENTITY_INSERT [dbo].[cart] OFF
GO

SET IDENTITY_INSERT [dbo].[item] ON
GO

INSERT [dbo].[item] ([Id], [title], [category], [description], [price]) VALUES (1, N'Pizza', N'Snacks', N'Italian', CAST(10 AS Numeric(18, 0)))
GO

INSERT [dbo].[item] ([Id], [title], [category], [description], [price]) VALUES (2, N'Brownie', N'Desert', N'Sweet', CAST(2 AS Numeric(18, 0)))
GO

INSERT [dbo].[item] ([Id], [title], [category], [description], [price]) VALUES (3, N'Beer', N'Beverage', N'Casual drink', CAST(1 AS Numeric(18, 0)))
GO

INSERT [dbo].[item] ([Id], [title], [category], [description], [price]) VALUES (11, N'Brownie', N'Desert', N'explosive', CAST(100 AS Numeric(18, 0)))
GO

SET IDENTITY_INSERT [dbo].[item] OFF
GO

SET IDENTITY_INSERT [dbo].[orders] ON
GO

INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (1, 1, 2, 2, CAST(2 AS Numeric(18, 0)), 1, CAST(N'2019-05-01T18:03:53.317' AS DateTime))
GO

INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (23, 4, 3, 2, CAST(2 AS Numeric(18, 0)), 10, CAST(N'2019-05-02T08:23:03.833' AS DateTime))
GO

INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (24, 4, 3, 11, CAST(100 AS Numeric(18, 0)), 1, CAST(N'2019-05-02T08:23:03.870' AS DateTime))
GO

INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (25, 10, 2, 3, CAST(1 AS Numeric(18, 0)), 1, CAST(N'2019-05-12T11:23:46.687' AS DateTime))
GO

INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (26, 10, 2, 1, CAST(10 AS Numeric(18, 0)), 2, CAST(N'2019-05-12T11:23:46.797' AS DateTime))
GO

INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (27, 5, 3, 3, CAST(1 AS Numeric(18, 0)), 1, CAST(N'2019-05-12T11:48:11.320' AS DateTime))
GO

SET IDENTITY_INSERT [dbo].[orders] OFF
GO

SET IDENTITY_INSERT [dbo].[users] ON
GO

INSERT [dbo].[users] ([Id], [username], [password], [role], [orderCount]) VALUES (1, N'admin@hotel.com', N'root', 1, 0)
GO

INSERT [dbo].[users] ([Id], [username], [password], [role], [orderCount]) VALUES (2, N'filip@hotel.com', N'qwerty', 3, 10)
GO

INSERT [dbo].[users] ([Id], [username], [password], [role], [orderCount]) VALUES (3, N'jones@hotel.com', N'b99', 3, 5)
GO

INSERT [dbo].[users] ([Id], [username], [password], [role], [orderCount]) VALUES (1005, N'admin2@hotel.com', N'cpp', 1, 0)
GO

SET IDENTITY_INSERT [dbo].[users] OFF
GO

ALTER DATABASE [HotelDB] SET  READ_WRITE
GO
