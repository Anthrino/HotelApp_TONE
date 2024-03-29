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
CREATE TABLE [dbo].[cart](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[itemId] [int] NULL,
	[quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[item]    Script Date: 5/14/2019 12:51:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[item](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](20) NULL,
	[category] [varchar](25) NULL,
	[description] [varchar](25) NULL,
	[price] [numeric](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orders]    Script Date: 5/14/2019 12:51:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[orderId] [int] NULL,
	[userId] [int] NULL,
	[itemId] [int] NULL,
	[price] [numeric](18, 0) NULL,
	[quantity] [int] NULL,
	[dop] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 5/14/2019 12:51:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](25) NULL,
	[password] [varchar](12) NULL,
	[role] [int] NULL,
	[orderCount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[cart] ON 

INSERT [dbo].[cart] ([Id], [userId], [itemId], [quantity]) VALUES (1037, 1, 1, 3)
INSERT [dbo].[cart] ([Id], [userId], [itemId], [quantity]) VALUES (1038, 2, 2, 1)
INSERT [dbo].[cart] ([Id], [userId], [itemId], [quantity]) VALUES (1039, 2, 1, 1)
SET IDENTITY_INSERT [dbo].[cart] OFF
SET IDENTITY_INSERT [dbo].[item] ON 

INSERT [dbo].[item] ([Id], [title], [category], [description], [price]) VALUES (1, N'Pizza', N'Snacks', N'Italian', CAST(10 AS Numeric(18, 0)))
INSERT [dbo].[item] ([Id], [title], [category], [description], [price]) VALUES (2, N'Brownie', N'Desert', N'Sweet', CAST(2 AS Numeric(18, 0)))
INSERT [dbo].[item] ([Id], [title], [category], [description], [price]) VALUES (3, N'Beer', N'Beverage', N'Casual drink', CAST(1 AS Numeric(18, 0)))
INSERT [dbo].[item] ([Id], [title], [category], [description], [price]) VALUES (11, N'Brownie', N'Desert', N'explosive', CAST(100 AS Numeric(18, 0)))
SET IDENTITY_INSERT [dbo].[item] OFF
SET IDENTITY_INSERT [dbo].[orders] ON 

INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (1, 1, 2, 2, CAST(2 AS Numeric(18, 0)), 1, CAST(N'2019-05-01T18:03:53.317' AS DateTime))
INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (23, 4, 3, 2, CAST(2 AS Numeric(18, 0)), 10, CAST(N'2019-05-02T08:23:03.833' AS DateTime))
INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (24, 4, 3, 11, CAST(100 AS Numeric(18, 0)), 1, CAST(N'2019-05-02T08:23:03.870' AS DateTime))
INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (25, 10, 2, 3, CAST(1 AS Numeric(18, 0)), 1, CAST(N'2019-05-12T11:23:46.687' AS DateTime))
INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (26, 10, 2, 1, CAST(10 AS Numeric(18, 0)), 2, CAST(N'2019-05-12T11:23:46.797' AS DateTime))
INSERT [dbo].[orders] ([Id], [orderId], [userId], [itemId], [price], [quantity], [dop]) VALUES (27, 5, 3, 3, CAST(1 AS Numeric(18, 0)), 1, CAST(N'2019-05-12T11:48:11.320' AS DateTime))
SET IDENTITY_INSERT [dbo].[orders] OFF
SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([Id], [username], [password], [role], [orderCount]) VALUES (1, N'admin@hotel.com', N'root', 1, 0)
INSERT [dbo].[users] ([Id], [username], [password], [role], [orderCount]) VALUES (2, N'filip@hotel.com', N'qwerty', 3, 10)
INSERT [dbo].[users] ([Id], [username], [password], [role], [orderCount]) VALUES (3, N'jones@hotel.com', N'b99', 3, 5)
INSERT [dbo].[users] ([Id], [username], [password], [role], [orderCount]) VALUES (1005, N'admin2@hotel.com', N'cpp', 1, 0)
SET IDENTITY_INSERT [dbo].[users] OFF
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FK_cart_item] FOREIGN KEY([itemId])
REFERENCES [dbo].[item] ([Id])
GO
ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FK_cart_item]
GO
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FK_cart_user] FOREIGN KEY([userId])
REFERENCES [dbo].[users] ([Id])
GO
ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FK_cart_user]
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK_order_item] FOREIGN KEY([itemId])
REFERENCES [dbo].[item] ([Id])
GO
ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_order_item]
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK_order_user] FOREIGN KEY([userId])
REFERENCES [dbo].[users] ([Id])
GO
ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_order_user]
GO
ALTER DATABASE [HotelDB] SET  READ_WRITE 
GO
