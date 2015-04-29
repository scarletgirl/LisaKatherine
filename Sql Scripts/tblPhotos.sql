USE [LisaKatherine]
GO

/****** Object:  Table [dbo].[tblPhotos]    Script Date: 29/04/2015 14:35:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblPhotos](
	[photoId] [int] IDENTITY(1,1) NOT NULL,
	[alt] [nvarchar](50) NULL,
	[width] [int] NULL,
	[height] [int] NULL,
	[uploaded] [datetime] NULL,
	[isHomePage] [bit] NOT NULL,
	[userId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tblPhotos] PRIMARY KEY CLUSTERED 
(
	[photoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblPhotos] ADD  CONSTRAINT [DF_tblPhotos_isHomePage]  DEFAULT ((0)) FOR [isHomePage]
GO

ALTER TABLE [dbo].[tblPhotos]  WITH CHECK ADD  CONSTRAINT [FK_tblPhotos_tblUsers] FOREIGN KEY([userId])
REFERENCES [dbo].[tblUsers] ([userId])
GO

ALTER TABLE [dbo].[tblPhotos] CHECK CONSTRAINT [FK_tblPhotos_tblUsers]
GO


