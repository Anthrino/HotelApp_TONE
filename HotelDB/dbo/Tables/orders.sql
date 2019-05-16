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
ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK_order_item] FOREIGN KEY([itemId])
REFERENCES [dbo].[item] ([Id])
GO

ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_order_item]
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK_order_user] FOREIGN KEY([userId])
REFERENCES [dbo].[users] ([Id])
GO

ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK_order_user]