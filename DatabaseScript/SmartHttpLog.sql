create database SmartHttpLog;

USE [SmartHttpLog]
GO
/****** Object:  FullTextCatalog [Dictionary]    Script Date: 18/10/25 20:09:48 ******/
CREATE FULLTEXT CATALOG [Dictionary] WITH ACCENT_SENSITIVITY = ON
GO
/****** Object:  Table [dbo].[HttpLogInterfaceCall]    Script Date: 18/10/25 20:09:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HttpLogInterfaceCall](
	[ID] [uniqueidentifier] NOT NULL,
	[Mills] [bigint] NOT NULL,
	[Url] [text] NULL,
	[Request] [nvarchar](max) NULL,
	[Response] [nvarchar](max) NULL,
	[Method] [nvarchar](64) NULL,
	[HttpMethod] [nvarchar](16) NULL,
	[IsSuccess] [bit] NULL,
	[Exception] [text] NULL,
	[ExceptionMessage] [text] NULL,
	[AppID] [bigint] NULL,
	[Moudle] [nvarchar](64) NULL,
	[RequestArgs] [text] NULL,
	[ResponseEncrypt] [text] NULL,
	[RequestEncrypt] [text] NULL,
	[CreateTime] [datetime] NULL,
	[StatusCode] [int] NULL,
	[ExceptionType] [text] NULL,
	[Channel] [nvarchar](128) NULL,
	[Version] [nvarchar](128) NULL,
	[SenderTime] [nvarchar](23) NULL,
	[IsAynsc] [bit] NULL,
	[Browser] [nvarchar](1000) NULL,
	[CIP] [nvarchar](100) NULL,
	[IP] [nvarchar](100) NULL,
	[IsPreLoad] [bit] NULL,
	[Other] [nvarchar](1000) NULL,
	[Retry] [bit] NULL,
	[Token] [nvarchar](100) NULL,
	[UID] [nvarchar](64) NULL,
	[RequestID] [nvarchar](64) NULL,
	[ComputerName] [nvarchar](1000) NULL,
	[IsNotify] [bit] NULL,
	[IsProcess] [bit] NULL,
	[ExecuteID] [uniqueidentifier] NULL,
	[ExecuteResult] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[RowVersion] [int] NULL,
 CONSTRAINT [PK_HttpLogInterfaceCall20180426] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HttpNotify]    Script Date: 18/10/25 20:09:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HttpNotify](
	[ID] [uniqueidentifier] NOT NULL,
	[NotifyUsers] [nvarchar](2000) NOT NULL,
	[Method] [nvarchar](128) NOT NULL,
	[Version] [nvarchar](128) NOT NULL,
	[AppID] [int] NOT NULL,
	[AppIDs] [nvarchar](2000) NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
	[MessagePaths] [nvarchar](2000) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[RowVersion] [int] NOT NULL,
 CONSTRAINT [PK_HttpNotify] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SingleJob]    Script Date: 18/10/25 20:09:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SingleJob](
	[ID] [uniqueidentifier] NOT NULL,
	[PriorityLevel] [int] NOT NULL,
	[TimeOut] [int] NOT NULL,
	[AppID] [int] NOT NULL,
	[Method] [nvarchar](1024) NOT NULL,
	[Version] [nvarchar](100) NOT NULL,
	[RequestObjs] [nvarchar](max) NOT NULL,
	[Response] [nvarchar](1024) NOT NULL,
	[IsSuccess] [bit] NOT NULL,
	[CanSuccessDelete] [bit] NOT NULL,
	[SuccessDoAppID] [int] NOT NULL,
	[SuccessDoMethod] [nvarchar](1024) NOT NULL,
	[SuccessVersion] [nvarchar](100) NOT NULL,
	[HttpCode] [int] NOT NULL,
	[ExecuteTimes] [int] NOT NULL,
	[MaxExecuteTimes] [int] NOT NULL,
	[ErrorIsSend] [bit] NOT NULL,
	[RequestAppID] [int] NOT NULL,
	[RequestUserID] [nvarchar](1024) NOT NULL,
	[JobProcesser] [nvarchar](1024) NOT NULL,
	[Exception] [nvarchar](1024) NOT NULL,
	[Remark] [nvarchar](1024) NOT NULL,
	[SendMessage] [nvarchar](max) NOT NULL,
	[GroupID] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](1024) NOT NULL,
	[Mobile] [nvarchar](1024) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[RowVersion] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
