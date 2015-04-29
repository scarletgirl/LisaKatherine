USE [LisaKatherine]
GO

/****** Object:  Table [dbo].[tblArticlePublished]    Script Date: 29/04/2015 14:34:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblArticlePublished](
	[articleId] [int] NOT NULL,
	[headline] [nvarchar](255) NULL,
	[strapline] [nvarchar](800) NULL,
	[body] [ntext] NULL,
	[dateCreated] [datetime2](7) NOT NULL,
	[datePublished] [datetime2](7) NULL,
	[isPublished] [bit] NOT NULL,
	[userid] [uniqueidentifier] NULL,
	[articleTypeId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


