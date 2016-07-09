CREATE TABLE [dbo].[User] (
    [Id]			INT					IDENTITY (1, 1) NOT NULL,
    [FirstName]		NVARCHAR (50)		NULL,
    [LastName]		NVARCHAR (100)		NULL,
    [EMail]			NVARCHAR (50)		NOT NULL,
    [Password]		NVARCHAR (100)		NOT NULL,
    [Role]			INT					NOT NULL,
    [Active]		BIT					NOT NULL,
    [ConfirmHash]   UNIQUEIDENTIFIER	NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);

