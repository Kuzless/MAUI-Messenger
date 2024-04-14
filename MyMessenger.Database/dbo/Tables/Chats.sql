CREATE TABLE [dbo].[Chats] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Chats] PRIMARY KEY CLUSTERED ([Id] ASC)
);

