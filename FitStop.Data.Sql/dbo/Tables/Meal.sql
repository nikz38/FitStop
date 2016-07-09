CREATE TABLE [dbo].[Meal] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Text]        NVARCHAR (500) NOT NULL,
    [DateTimeFor] DATETIME       NOT NULL,
    [Calories]    FLOAT (53)     NOT NULL,
    [UserId]      INT            NULL,
    CONSTRAINT [PK_Meal] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Meal_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);

