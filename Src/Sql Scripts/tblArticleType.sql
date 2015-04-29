USE [LisaKatherine]
GO

/****** Object:  Table [dbo].[tblArticleType]    Script Date: 29/04/2015 14:35:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblArticleType](
	[articleTypeId] [int] NOT NULL,
	[articleType] [nvarchar](20) NULL,
	[sectionID] [int] NULL
) ON [PRIMARY]

GO


