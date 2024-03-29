USE [FoodFusion]
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Email], [HashPassword], [FullName], [Role]) VALUES (1, N'registeremail001', N'AK7toV4m1HxrElkPnLNR1+SaL36tNuNV/FE1/QLqzCHApqpiibOKKZIb7RljYjZF6Q==', N'Register Email', 1)
INSERT [dbo].[Users] ([Id], [Email], [HashPassword], [FullName], [Role]) VALUES (6, N'registeremail002', N'AHlKmvm280NM4mCyvWz10nerwvkk86FyyclVFGbOZvuH0vrXivW0eW3ihvnLy98RPw==', N'Register Email', 0)
INSERT [dbo].[Users] ([Id], [Email], [HashPassword], [FullName], [Role]) VALUES (13, N'test1', N'AEZb80amXQ+MCsVrf/AXwNJB+Fi1JQnP0g0rJmYqHB2x3KAVBTq8bZHyb8L+tT0PyA==', N'test3', 0)
INSERT [dbo].[Users] ([Id], [Email], [HashPassword], [FullName], [Role]) VALUES (14, N'test4', N'AFqEJmkQH9z6EisJ2lBl5Y8tY0691MF/TDuKRVcfT3dEKHuBT1jrDwMUgkbHTVfyWA==', N'test6', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
SET IDENTITY_INSERT [dbo].[Restaurants] ON 

INSERT [dbo].[Restaurants] ([Id], [Name], [Description], [Contact], [ManagerId], [City]) VALUES (4, N'string12', N'string13', N'string', 13, N'string')
INSERT [dbo].[Restaurants] ([Id], [Name], [Description], [Contact], [ManagerId], [City]) VALUES (5, N'Pizza Restaurant', N'The best pizza in town.', N'office@pizzarestaurant.com', NULL, N'Iasi')
SET IDENTITY_INSERT [dbo].[Restaurants] OFF
SET IDENTITY_INSERT [dbo].[Menus] ON 

INSERT [dbo].[Menus] ([Id], [RestaurantId]) VALUES (3, 4)
INSERT [dbo].[Menus] ([Id], [RestaurantId]) VALUES (1, 5)
SET IDENTITY_INSERT [dbo].[Menus] OFF
SET IDENTITY_INSERT [dbo].[MenuItems] ON 

INSERT [dbo].[MenuItems] ([Id], [MenuId], [Name], [Price]) VALUES (1, 1, N'capriciosa', 19)
INSERT [dbo].[MenuItems] ([Id], [MenuId], [Name], [Price]) VALUES (2, 1, N'margherita', 15)
INSERT [dbo].[MenuItems] ([Id], [MenuId], [Name], [Price]) VALUES (4, 3, N'pui vinenez', 0)
SET IDENTITY_INSERT [dbo].[MenuItems] OFF
SET IDENTITY_INSERT [dbo].[RestaurantMaps] ON 

INSERT [dbo].[RestaurantMaps] ([Id], [RestaurantId]) VALUES (1, 5)
SET IDENTITY_INSERT [dbo].[RestaurantMaps] OFF
SET IDENTITY_INSERT [dbo].[RestaurantTables] ON 

INSERT [dbo].[RestaurantTables] ([Id], [RestaurantMapId], [Name], [Seats]) VALUES (1, 1, N't1', 2)
INSERT [dbo].[RestaurantTables] ([Id], [RestaurantMapId], [Name], [Seats]) VALUES (2, 1, N't2', 2)
INSERT [dbo].[RestaurantTables] ([Id], [RestaurantMapId], [Name], [Seats]) VALUES (3, 1, N't3', 5)
INSERT [dbo].[RestaurantTables] ([Id], [RestaurantMapId], [Name], [Seats]) VALUES (4, 1, N't4', 6)
INSERT [dbo].[RestaurantTables] ([Id], [RestaurantMapId], [Name], [Seats]) VALUES (5, 1, N't5', 7)
SET IDENTITY_INSERT [dbo].[RestaurantTables] OFF
SET IDENTITY_INSERT [dbo].[Reservations] ON 

INSERT [dbo].[Reservations] ([Id], [UserId], [RestaurantId], [ParticipantsCount], [StartTime], [EndTime]) VALUES (2, 13, 5, 1, CAST(N'2018-11-18T20:00:00.0000000' AS DateTime2), CAST(N'2018-11-18T21:00:00.0000000' AS DateTime2))
INSERT [dbo].[Reservations] ([Id], [UserId], [RestaurantId], [ParticipantsCount], [StartTime], [EndTime]) VALUES (3, 13, 5, 2, CAST(N'2018-11-18T12:00:00.0000000' AS DateTime2), CAST(N'2018-11-18T15:30:00.0000000' AS DateTime2))
INSERT [dbo].[Reservations] ([Id], [UserId], [RestaurantId], [ParticipantsCount], [StartTime], [EndTime]) VALUES (4, 13, 5, 3, CAST(N'2018-11-18T19:00:00.0000000' AS DateTime2), CAST(N'2018-11-18T22:00:00.0000000' AS DateTime2))
INSERT [dbo].[Reservations] ([Id], [UserId], [RestaurantId], [ParticipantsCount], [StartTime], [EndTime]) VALUES (5, 13, 5, 2, CAST(N'2018-11-18T20:00:00.0000000' AS DateTime2), CAST(N'2018-11-18T22:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Reservations] OFF
SET IDENTITY_INSERT [dbo].[RestaurantEmployees] ON 

INSERT [dbo].[RestaurantEmployees] ([Id], [RestaurantId], [UserId]) VALUES (2, 4, 14)
SET IDENTITY_INSERT [dbo].[RestaurantEmployees] OFF
SET IDENTITY_INSERT [dbo].[ReservedTables] ON 

INSERT [dbo].[ReservedTables] ([Id], [ReservationId], [RestaurantTableId]) VALUES (1, 2, 1)
INSERT [dbo].[ReservedTables] ([Id], [ReservationId], [RestaurantTableId]) VALUES (2, 3, 2)
INSERT [dbo].[ReservedTables] ([Id], [ReservationId], [RestaurantTableId]) VALUES (3, 4, 3)
INSERT [dbo].[ReservedTables] ([Id], [ReservationId], [RestaurantTableId]) VALUES (4, 5, 5)
SET IDENTITY_INSERT [dbo].[ReservedTables] OFF
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20181103110042_InitialMigration', N'2.1.4-rtm-31024')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20181106194907_RemovedRequiredOnRestaurantForeignKeys', N'2.1.4-rtm-31024')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20181106204003_MovedRestaurantMenuForeignKey', N'2.1.4-rtm-31024')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20181106205845_MovedRestaurantMapForeignKey', N'2.1.4-rtm-31024')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20181106213420_RemovedDeleteRestrictBehaviour', N'2.1.4-rtm-31024')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20181106213938_RemovedDeleteRestrictBehaviourForRestaurantEmployees', N'2.1.4-rtm-31024')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20181111202919_RenamedRestaurantEmployee', N'2.1.4-rtm-31024')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20181118142427_AddedUserReservationFK', N'2.1.4-rtm-31024')
