/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [Role] ON

INSERT INTO [Role] ([Id], [Name]) VALUES
            (1, 'User'),
            (2, 'Admin')

SET IDENTITY_INSERT [Role] OFF


SET IDENTITY_INSERT [User] ON

INSERT INTO [User]([Id], [Username], [Email], [Password], [Salt], [Date_of_birth], [Gender], [Elo], [RoleId]) VALUES
            (1, 'm.checkmate', 'checkmate@checkmate.com', '$argon2id$v=19$m=65536,t=3,p=1$SMleJAg6gemG+JJfSzFiow$u7INNdtHM9r2MtezG7lMGOED2vhAflP7vnsd7FxjBNc', 'afcba622-6259-4bc5-babd-228c2dc8afe5', '1972-12-23 00:00:00.000', 'M', 1200, 2);

SET IDENTITY_INSERT [User] OFF


SET IDENTITY_INSERT [Tournament_category] ON

INSERT INTO [Tournament_category]([Id], [Name], [Rules]) VALUES
			(1, 'Junior', '< 18'),
			(2, 'Senior', '>= 18 && < 60'),
			(3, 'Veteran', '>= 60');

SET IDENTITY_INSERT [Tournament_category] OFF


SET IDENTITY_INSERT [Tournament] ON

INSERT INTO [Tournament] ([Id], [Name], [Place], [Min_players], [Max_players], [Min_elo], [Max_elo], [Status], [Current_round], [Women_only], [End_Registration], [Created_at], [Updated_at], [Cancelled], [Cancelled_at])
VALUES
    (1, 'Tournoi de Printemps', 'Paris', 4, 8, 1000, 1500, 1, 0, 0, DATEADD(day, 20, '2024-12-09 14:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (2, 'Coupe de la Ville de Lyon', 'Lyon', 2, 4, 500, 1000, 1, 0, 0, DATEADD(day, 20, '2024-12-07 10:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (3, 'Marseille Open', 'Marseille', 8, 16, 1500, 2000, 1, 0, 0, DATEADD(day, 20, '2024-12-13 12:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (4, 'Bordeaux Chess Festival', 'Bordeaux', 4, 8, 1000, 1500, 1, 0, 0, DATEADD(day, 20, '2024-12-09 14:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (5, 'Toulouse Chess Tournament', 'Toulouse', 2, 4, 500, 1000, 1, 0, 0, DATEADD(day, 20, '2024-12-07 10:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (6, 'Nice International Chess Tournament', 'Nice', 8, 16, 1500, 2000, 1, 0, 0, DATEADD(day, 20, '2024-12-13 12:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (7, 'Nantes Chess Championship', 'Nantes', 4, 8, 1000, 1500, 1, 0, 0, DATEADD(day, 20, '2024-12-09 14:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (8, 'Strasbourg Chess Open', 'Strasbourg', 2, 4, 500, 1000, 1, 0, 0, DATEADD(day, 20, '2024-12-07 10:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (9, 'Montpellier Chess Festival', 'Montpellier', 8, 16, 1500, 2000, 1, 0, 0, DATEADD(day, 20, '2024-12-13 12:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (10, 'Rennes Chess Tournament', 'Rennes', 4, 8, 1000, 1500, 1, 0, 0, DATEADD(day, 20, '2024-12-09 14:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (11, 'Reims Chess Championship', 'Reims', 2, 4, 500, 1000, 1, 0, 0, DATEADD(day, 20, '2024-12-07 10:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (12, 'Toulouse Chess Tournament 2', 'Toulouse', 2, 4, 500, 1000, 1, 0, 0, DATEADD(day, 20, '2025-12-07 10:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (13, 'Nice International Chess Tournament 2', 'Nice', 8, 16, 1500, 2000, 1, 0, 0, DATEADD(day, 20, '2025-12-13 12:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (14, 'Nantes Chess Championship II', 'Nantes', 4, 8, 1000, 1500, 1, 0, 0, DATEADD(day, 20, '2025-12-09 14:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (15, 'Strasbourg Chess Open Back', 'Strasbourg', 2, 4, 500, 1000, 1, 0, 0, DATEADD(day, 20, '2025-12-07 10:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (16, 'Montpellier Chess Festival II', 'Montpellier', 8, 16, 1500, 2000, 1, 0, 0, DATEADD(day, 20, '2025-12-13 12:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (17, 'Rennes Chess Tournament 2', 'Rennes', 4, 8, 1000, 1500, 1, 0, 0, DATEADD(day, 20, '2025-12-09 14:00:00'), GETDATE(), GETDATE(), 0, NULL),
    (18, 'Reims Chess Championship II', 'Reims', 2, 4, 500, 1000, 1, 0, 0, DATEADD(day, 20, '2025-12-07 10:00:00'), GETDATE(), GETDATE(), 0, NULL);

SET IDENTITY_INSERT [Tournament] OFF


    INSERT INTO [MM_Tournament_Category] ([TournamentId], [CategoryId])
VALUES
    (1, 2),  -- Tournoi de Printemps : Senior
    (2, 1),  -- Coupe de la Ville de Lyon : Junior
    (3, 2),  -- Marseille Open : Senior
    (4, 2),  -- Bordeaux Chess Festival : Senior
    (5, 1),  -- Toulouse Chess Tournament : Junior
    (6, 2),  -- Nice International Chess Tournament : Senior
    (7, 2),  -- Nantes Chess Championship : Senior
    (8, 1),  -- Strasbourg Chess Open : Junior
    (9, 2),  -- Montpellier Chess Festival : Senior
    (10, 2),  -- Rennes Chess Tournament : Senior
    (11, 1),  -- Reims Chess Championship : Junior
    (12, 1),  -- Toulouse Chess Tournament 2 : Junior
    (13, 2),  -- Nice International Chess Tournament 2 : Senior
    (14, 2),  -- Nantes Chess Championship II : Senior
    (15, 1),  -- Strasbourg Chess Open Back : Junior
    (16, 2),  -- Montpellier Chess Festival II : Senior
    (17, 2),  -- Rennes Chess Tournament 2 : Senior
    (18, 1),  -- Reims Chess Championship II : Junior
    (1, 3),  -- Tournoi de Printemps : Veteran
    (3, 3),  -- Marseille Open : Veteran
    (6, 3),  -- Nice International Chess Tournament : Veteran
    (9, 3),  -- Montpellier Chess Festival : Veteran
    (13, 3),  -- Nice International Chess Tournament 2 : Veteran
    (16, 3);  -- Montpellier Chess Festival II : Veteran