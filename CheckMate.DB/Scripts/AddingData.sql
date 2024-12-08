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
(1, 'm.checkmate', 'checkmate@checkmate.com', '$argon2id$v=19$m=65536,t=3,p=1$SMleJAg6gemG+JJfSzFiow$u7INNdtHM9r2MtezG7lMGOED2vhAflP7vnsd7FxjBNc', 'afcba622-6259-4bc5-babd-228c2dc8afe5', '1972-12-23 00:00:00.000', 'M', 1200, 2),
(2, 'user2', 'user2@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'd0f8777e-9a3a-430b-bcea-6baa8f4593c3', '1995-01-24', 'F', 1928, 2),
(3, 'user3', 'user3@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'd94b5f85-342b-4491-9299-f7fae9ae3e49', '1972-06-27', 'F', 1241, 2),
(4, 'user4', 'user4@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'e0cc71a8-1003-4f5a-a27b-637f9af1916e', '2005-02-27', 'F', 1228, 2),
(5, 'user5', 'user5@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'b1c66e2b-dc3c-458e-bba8-d9663d5435cb', '1996-04-08', 'F', 1502, 2),
(6, 'user6', 'user6@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '2d16c000-795d-4ab4-bcbc-41d8e2809840', '1978-06-06', 'F', 1970, 2),
(7, 'user7', 'user7@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'e8580325-575d-4230-848a-92b5767cd506', '1979-04-23', 'F', 1804, 1),
(8, 'user8', 'user8@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '5301c40b-8bc6-461b-be46-3087e3d1f631', '1982-12-10', 'F', 1689, 1),
(9, 'user9', 'user9@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'bded49d1-3cd4-49b6-bb95-61805aebe292', '1984-08-16', 'F', 956, 1),
(10, 'user10', 'user10@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'a4ad0bbf-e522-41c8-872e-c38979fe4e66', '1997-07-19', 'F', 1526, 1),
(11, 'user11', 'user11@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '77373791-f46f-4276-983a-e45ec0d769e5', '1976-07-10', 'M', 1034, 1),
(12, 'user12', 'user12@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'b3c9ed7c-8865-4473-8d9f-ef131624c508', '2004-03-20', 'M', 1462, 2),
(13, 'user13', 'user13@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'e0cad9f4-dc24-4579-ac03-806ab4860338', '1987-07-05', 'M', 878, 1),
(14, 'user14', 'user14@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '204ef194-3011-41b8-900b-eebea15969cd', '1972-04-15', 'F', 1703, 1),
(15, 'user15', 'user15@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'b45dffc6-e659-4dbf-82f4-daef9ff07f7e', '1991-12-02', 'M', 1493, 2),
(16, 'user16', 'user16@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'd72e711b-5b31-48f9-a189-a923a48d9f67', '1973-11-14', 'M', 1610, 1),
(17, 'user17', 'user17@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'fb53fe4f-37f7-442e-bfa9-ef3b7d217936', '1975-02-16', 'M', 1955, 2),
(18, 'user18', 'user18@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '03186ef9-ca62-4508-aba1-030e392be438', '2003-08-08', 'F', 1498, 1),
(19, 'user19', 'user19@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '02d77fb3-8f6a-4cee-8abd-0b0d752594e4', '1993-01-29', 'F', 1301, 1),
(20, 'user20', 'user20@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '53920d64-6ef1-4026-b475-57b210c7c90b', '2007-06-08', 'M', 1857, 1),
(21, 'user21', 'user21@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '272d5e89-30a4-4281-ad82-d5794f24f144', '1997-03-07', 'F', 1754, 1),
(22, 'user22', 'user22@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'a74d6b28-91e4-4af3-a35a-f127ae680158', '2006-10-19', 'M', 1055, 1),
(23, 'user23', 'user23@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '3b107db0-7eee-4890-8996-6c1849995329', '2008-04-09', 'F', 888, 2),
(24, 'user24', 'user24@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '7dee02c2-67e1-4ba2-95b1-bf05982d8d56', '2000-11-17', 'M', 1357, 2),
(25, 'user25', 'user25@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'aedec6fa-4b31-4e7c-8be7-13d39daf9ad7', '1992-04-19', 'M', 1912, 1),
(26, 'user26', 'user26@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '2ecf5c2e-a211-45d1-bcaf-1aa573e8b05b', '2004-03-01', 'M', 1072, 2),
(27, 'user27', 'user27@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'a3808153-fd95-4360-ae53-2e11dcb5be74', '1977-12-03', 'F', 1395, 1),
(28, 'user28', 'user28@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'f69a9ec2-36ae-43fe-8618-2656e09d48c5', '1995-06-07', 'M', 837, 1),
(29, 'user29', 'user29@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'd6a4da79-0084-481d-92b2-8e584d31e17d', '2001-10-14', 'F', 1667, 2),
(30, 'user30', 'user30@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '3920c5a4-56a6-437f-8775-f345f21f3e3a', '1975-03-20', 'F', 1181, 2),
(31, 'user31', 'user31@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '43d2ee34-665f-47b5-95c8-be91f7b8dfbf', '1997-12-14', 'M', 1445, 1),
(32, 'user32', 'user32@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '5736a6d3-f234-49f4-8436-442c6206562b', '1981-06-07', 'F', 1000, 1),
(33, 'user33', 'user33@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'c35e9fbf-4ad3-4d7f-abc0-eda5809a8c77', '1989-03-17', 'M', 1926, 1),
(34, 'user34', 'user34@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '6f47e550-7478-44ab-a87b-ae5e105fc192', '2005-08-03', 'F', 1032, 2),
(35, 'user35', 'user35@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'badaee9d-ccfa-41af-a612-a85b23b1feba', '1981-12-06', 'F', 1500, 1),
(36, 'user36', 'user36@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '884aa8f6-6628-40a8-952a-1c5c229c0bbe', '2002-10-08', 'M', 1052, 1),
(37, 'user37', 'user37@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '80d5897d-a9a1-4faa-aefc-ca03c676fe9c', '2008-03-02', 'M', 1066, 1),
(38, 'user38', 'user38@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '087abd33-ee37-48e5-ba34-ff7f8aee1d09', '1998-11-07', 'F', 1880, 1),
(39, 'user39', 'user39@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '87149123-85da-416c-ad4b-b1e2cb60013e', '1996-10-12', 'M', 1440, 1),
(40, 'user40', 'user40@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '5f71a797-2610-414b-93ba-95144066491c', '1987-06-16', 'F', 1963, 1),
(41, 'user41', 'user41@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '9ea0cac2-33df-434f-905a-5bc48aa5ab8f', '2000-01-18', 'M', 1333, 2),
(42, 'user42', 'user42@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'c9b48964-4978-4c21-8c41-1f90aca4e7cd', '1994-12-18', 'F', 1549, 2),
(43, 'user43', 'user43@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '37e275c7-f68f-449d-b375-98e445b69d14', '1984-12-20', 'F', 871, 1),
(44, 'user44', 'user44@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '5f78af66-0733-4b95-96de-d5c2dcc15c0a', '1975-10-27', 'F', 1556, 2),
(45, 'user45', 'user45@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '588c5ab3-c40a-4ec4-bad9-3b84686ae4cf', '1978-01-09', 'F', 1853, 2),
(46, 'user46', 'user46@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '2ca43237-77bb-4fb1-9c11-dd788356bee8', '1985-10-27', 'M', 1891, 2),
(47, 'user47', 'user47@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '0575d991-ed57-42e2-ae76-6fd96e526092', '1972-12-10', 'M', 1306, 2),
(48, 'user48', 'user48@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '9518a2f6-584e-4796-9950-34b40892fdfa', '1988-09-09', 'F', 1781, 2),
(49, 'user49', 'user49@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', '9aa53bdd-df71-4315-afed-d528eba73126', '1979-02-12', 'F', 1336, 1),
(50, 'user50', 'user50@example.com', '$argon2id$v=19$m=65536,t=3,p=1$xyz0987$abc7654', 'b6cc0cbd-0b3e-487b-b0f7-131b5d58b966', '1975-11-26', 'M', 1481, 2);

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


INSERT INTO [MM_Tournament_Registration]([UserId], [TournamentId]) VALUES
(1, 1),
(2, 1),
(3, 1),
(4, 1),
(5, 1),
(6, 1),
(7, 1),
(4, 2),
(5, 2),
(8, 2),
(9, 2),
(10, 2),
(11, 2),
(6, 3),
(7, 3),
(12, 3),
(13, 3),
(14, 3),
(15, 3),
(1, 3),
(3, 3),
(2, 2),
(8, 1),
(15, 2);
