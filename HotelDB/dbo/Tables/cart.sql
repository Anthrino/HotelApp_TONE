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
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FK_cart_item] FOREIGN KEY([itemId])
REFERENCES [dbo].[item] ([Id])
GO

ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FK_cart_item]
GO
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FK_cart_user] FOREIGN KEY([userId])
REFERENCES [dbo].[users] ([Id])
GO

ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FK_cart_user]