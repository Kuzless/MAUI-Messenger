CREATE TABLE [dbo].[ChatUser] (
    [ChatsId] INT            NOT NULL,
    [UsersId] NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_ChatUser] PRIMARY KEY CLUSTERED ([ChatsId] ASC, [UsersId] ASC),
    CONSTRAINT [FK_ChatUser_AspNetUsers_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ChatUser_Chats_ChatsId] FOREIGN KEY ([ChatsId]) REFERENCES [dbo].[Chats] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ChatUser_UsersId]
    ON [dbo].[ChatUser]([UsersId] ASC);

