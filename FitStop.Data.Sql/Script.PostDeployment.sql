--Users
SET IDENTITY_INSERT [User] ON
GO

INSERT [dbo].[User] ([Id], [FirstName], [LastName], [EMail], [Password], [Active], [Role])
	VALUES (1, N'software', N'engineers', N'software@enginee.rs', N'1000:lJdeOzygf9Ae7K/ZH4u9BInt/36nNp7p:h1qIqSiVDgJb4zWtNifyDeQPzH+G+LWH', 1, 1)
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [EMail], [Password], [Active], [Role])
	VALUES (2, N'john', N'smith', N'john@fitstop.com', N'1000:QSMUuFx6fr6XAXMlJUxlybPHJBegWCJ+:Jc17zKlPEvZR365xGMNfpcn3dVFtydt8', 1, 2)
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [EMail], [Password], [Active], [Role])
	VALUES (3, N'mark', N'smith', N'mark@fitstop.com', N'1000:QSMUuFx6fr6XAXMlJUxlybPHJBegWCJ+:Jc17zKlPEvZR365xGMNfpcn3dVFtydt8', 1, 3)
GO

SET IDENTITY_INSERT [User] OFF
GO

--UserSettings
INSERT [dbo].[UserSetting] ([UserId], [DailyCalorieIntake]) VALUES (1, 2000)
INSERT [dbo].[UserSetting] ([UserId], [DailyCalorieIntake]) VALUES (2, 2000)
INSERT [dbo].[UserSetting] ([UserId], [DailyCalorieIntake]) VALUES (3, 2000)
GO


INSERT [dbo].[Meal] ([UserId], [DateTimeFor], [Text], [Calories]) VALUES (3, CAST(N'2016-05-01 09:00:00.000' AS DateTime), N'Breakfast', 400)
INSERT [dbo].[Meal] ([UserId], [DateTimeFor], [Text], [Calories]) VALUES (3, CAST(N'2016-05-01 13:00:00.000' AS DateTime), N'Lunch', 800)
INSERT [dbo].[Meal] ([UserId], [DateTimeFor], [Text], [Calories]) VALUES (3, CAST(N'2016-05-01 16:00:00.000' AS DateTime), N'Mid-day snack', 450)
INSERT [dbo].[Meal] ([UserId], [DateTimeFor], [Text], [Calories]) VALUES (3, CAST(N'2016-05-01 19:00:00.000' AS DateTime), N'Dinner', 600)
INSERT [dbo].[Meal] ([UserId], [DateTimeFor], [Text], [Calories]) VALUES (3, CAST(N'2016-04-30 09:00:00.000' AS DateTime), N'Sandwitch', 420)
GO

