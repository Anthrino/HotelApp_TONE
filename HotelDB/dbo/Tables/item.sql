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