
CREATE TABLE [dbo].[Trees](
	[TreeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](1000) NOT NULL,
	[OwnerId] [int] NOT NULL CONSTRAINT [DF_Trees_OwnerId]  DEFAULT ((-1)),
	CONSTRAINT [PK_Trees] PRIMARY KEY CLUSTERED ([TreeID] ASC)
 )

 CREATE TABLE [dbo].[Individuals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TreeID] [int] NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Sex] [smallint] NOT NULL,
	[FatherID] [int] NULL,
	[MotherID] [int] NULL,
	CONSTRAINT [PK_Individuals] PRIMARY KEY CLUSTERED ([ID] ASC)
)

CREATE TABLE [dbo].[Families](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TreeID] [int] NOT NULL,
	[HusbandID] [int] NOT NULL,
	[WifeID] [int] NOT NULL,
	CONSTRAINT [PK_Families] PRIMARY KEY CLUSTERED ([ID] ASC)
)
