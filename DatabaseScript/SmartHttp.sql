create database SmartHttp;

USE [SmartHttp]
GO
/****** Object:  Table [dbo].[HttpApp]    Script Date: 18/10/25 20:05:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HttpApp](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[AppID] [int] NOT NULL,
	[Description] [nvarchar](256) NULL,
	[CreateTime] [datetime] NOT NULL,
	[MoudlesJsonString] [text] NULL,
	[UpdateTime] [datetime] NOT NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_HttpApp] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HttpConfig]    Script Date: 18/10/25 20:05:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HttpConfig](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[TestValue] [nvarchar](256) NULL,
	[ProdcutValue] [nvarchar](256) NULL,
	[Description] [nvarchar](256) NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_dbo.HttpConfig] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HttpInvokeArg]    Script Date: 18/10/25 20:05:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HttpInvokeArg](
	[ID] [uniqueidentifier] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[RowVersion] [int] NOT NULL,
	[AppID] [int] NOT NULL,
	[Method] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](1024) NOT NULL,
	[Value] [nvarchar](1024) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HttpMessage]    Script Date: 18/10/25 20:05:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HttpMessage](
	[ID] [uniqueidentifier] NOT NULL,
	[Version] [nvarchar](128) NOT NULL,
	[Method] [nvarchar](128) NOT NULL,
	[AppID] [int] NOT NULL,
	[AppName] [nvarchar](128) NOT NULL,
	[Url] [nvarchar](1024) NOT NULL,
	[SubMoudle] [nvarchar](128) NOT NULL,
	[Moudle] [nvarchar](128) NOT NULL,
	[HttpType] [int] NOT NULL,
	[ResultPath] [nvarchar](128) NOT NULL,
	[TrueResult] [nvarchar](128) NOT NULL,
	[TrueResultContain] [nvarchar](128) NOT NULL,
	[FalseResultContain] [nvarchar](128) NOT NULL,
	[ExceptionPath] [nvarchar](128) NOT NULL,
	[WebServiceTemplate] [text] NOT NULL,
	[ContentType] [nvarchar](128) NOT NULL,
	[UserAgent] [nvarchar](1024) NOT NULL,
	[Headers] [nvarchar](128) NOT NULL,
	[HttpEncoding] [nvarchar](128) NOT NULL,
	[InterfaceArgsJsonString] [text] NULL,
	[WsExcepitonsJsonString] [text] NULL,
	[WSExceptionType] [nvarchar](10) NOT NULL,
	[Description] [nvarchar](128) NOT NULL,
	[Status] [int] NOT NULL,
	[IsValid] [bit] NOT NULL,
	[TimeOut] [int] NOT NULL,
	[LoopTime] [int] NOT NULL,
	[LoopWaitTime] [int] NOT NULL,
	[EncryptDLLName] [nvarchar](128) NOT NULL,
	[IsCache] [bit] NOT NULL,
	[CacheSeconds] [int] NOT NULL,
	[IsLog] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[RowVersion] [int] NOT NULL,
	[InterfaceArgsCount] [int] NOT NULL,
	[Define] [text] NULL,
	[IsNeedLogin] [bit] NOT NULL,
	[IsNotify] [bit] NOT NULL,
 CONSTRAINT [PK_HttpMessage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
