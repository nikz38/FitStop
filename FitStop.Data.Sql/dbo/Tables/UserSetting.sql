CREATE TABLE [dbo].[UserSetting] (
    [UserId]             INT        NOT NULL,
    [DailyCalorieIntake] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_UserSetting] PRIMARY KEY CLUSTERED ([UserId]),
    CONSTRAINT [FK_UserSetting_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);

