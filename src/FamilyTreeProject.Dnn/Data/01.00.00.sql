/* Copyright (c) Charles Nurse. All rights reserved.										*/
/* Licensed under the MIT License. See LICENSE in the project root for license information. */
/********************************************************************************************/

/*  Trees Table  */
/*****************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_Trees]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_Trees](
		    [TreeID]                 INT             IDENTITY (1, 1) NOT NULL,
			[Title]                  NVARCHAR (100)  NOT NULL,
			[Name]                   NVARCHAR (1000) NOT NULL,
			[Description]            NVARCHAR (4000) NOT NULL,
			[OwnerID]                INT             NOT NULL,
			[HomeIndividualID]       INT             NOT NULL,
			[LastViewedIndividualID] INT             NOT NULL,
			[ImageID]                INT             NOT NULL,
			CONSTRAINT [PK_dnn_FTP_Trees] PRIMARY KEY CLUSTERED ([TreeID] ASC)
		 )
	END

/*  Repositories Table  */
/************************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_Repositories]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_Repositories]
		(
			ID			INT				NOT NULL PRIMARY KEY IDENTITY, 
			TreeID		INT				NOT NULL, 
			Name		NVARCHAR(100)	NOT NULL, 
			[Address]	NVARCHAR(1000)	NULL
		)
	END

/*  Sources Table  */
/*******************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_Sources]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_Sources]
		(
			ID				INT				NOT NULL PRIMARY KEY IDENTITY, 
			TreeID			INT				NOT NULL DEFAULT -1, 
			RepositoryID	INT				NOT NULL DEFAULT -1, 
			Author			NVARCHAR(100)	NULL, 
			Publisher		NVARCHAR(2000)	NULL,
			Title			NVARCHAR(2000)	NULL
		)
	END

/*  Citations Table  */
/*********************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_Citations]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_Citations]
		(
			ID			INT				NOT NULL PRIMARY KEY IDENTITY, 
			TreeID		INT				NOT NULL DEFAULT -1, 
			SourceID	INT				NOT NULL DEFAULT -1, 
			OwnerID		INT				NOT NULL DEFAULT -1, 
			OwnerType	SMALLINT		NOT NULL DEFAULT -1, 
			[Date]		NVARCHAR(100)	NULL, 
			[Page]		NVARCHAR(1000)	NULL,
			[Text]		NVARCHAR(MAX)	NULL
		)
	END

/*  Notes Table  */
/*****************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_Notes]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_Notes]
		(
			ID			INT				NOT NULL PRIMARY KEY IDENTITY, 
			TreeID		INT				NOT NULL DEFAULT -1, 
			OwnerID		INT				NOT NULL DEFAULT -1, 
			OwnerType	SMALLINT		NOT NULL DEFAULT -1, 
			[Text]		NVARCHAR(MAX)	NULL
		)
	END

/*  MultimediaLinks Table  */
/***************************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_MultimediaLinks]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_MultimediaLinks]
		(
			ID			INT				NOT NULL PRIMARY KEY IDENTITY, 
			TreeID		INT				NOT NULL DEFAULT -1, 
			OwnerID		INT				NOT NULL DEFAULT -1, 
			OwnerType	SMALLINT		NOT NULL DEFAULT -1, 
			[File]		NVARCHAR(2000)	NULL,
			[Format]	NVARCHAR(100)	NULL,
			Title		NVARCHAR(1000)	NULL
		)
	END

/*  Individuals Table  */
/***********************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_Individuals]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_Individuals]
		(
			[ID]        INT             IDENTITY (1, 1) NOT NULL,
			[TreeID]    INT             NOT NULL,
			[FirstName] NVARCHAR (1000) NULL,
			[LastName]  NVARCHAR (1000) NULL,
			[Sex]       SMALLINT        NOT NULL,
			[FatherID]  INT             NULL,
			[MotherID]  INT             NULL,
		    [ImageID]   INT				NOT NULL, 
			PRIMARY KEY CLUSTERED ([ID] ASC),
			CONSTRAINT [FK_dnn_FTP_Individuals_dnn_FTP_Trees] 
				FOREIGN KEY ([TreeID]) 
				REFERENCES [dbo].[dnn_FTP_Trees] ([TreeID]) 
				ON DELETE CASCADE
		)
	END

/*  Families Table  */
/********************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_Families]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_Families]
		(
			ID			INT				NOT NULL PRIMARY KEY IDENTITY, 
			TreeID		INT				NOT NULL DEFAULT -1,
			HusbandID	INT				NOT NULL DEFAULT -1, 
			WifeID		INT				NOT NULL DEFAULT -1 
		)
	END

/*  Facts Table  */
/*****************/

IF NOT EXISTS (SELECT * FROM sys.objects 
		WHERE object_id = OBJECT_ID(N'dbo.[dnn_FTP_Facts]') 
		AND type in (N'U'))
	BEGIN
		CREATE TABLE [dbo].[dnn_FTP_Facts]
		(
			ID			INT				NOT NULL PRIMARY KEY IDENTITY, 
			TreeID		INT				NOT NULL DEFAULT -1,
			OwnerID		INT				NOT NULL DEFAULT -1, 
			OwnerType	SMALLINT		NOT NULL DEFAULT -1, 
			FactType	SMALLINT		NOT NULL DEFAULT -1, 
			[Date]		NVARCHAR(100)	NULL, 
			Place		NVARCHAR(1000)	NULL
		)
	END
