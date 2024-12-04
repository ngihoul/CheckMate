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