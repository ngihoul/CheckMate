﻿CREATE TABLE [Tournament]
(
	[Id] INT IDENTITY,
	[Name] VARCHAR(100) NOT NULL,
	[Place] VARCHAR(100) NULL,
	[Min_players] INT NOT NULL,
	[Max_players] INT NOT NULL,
	[Min_elo] INT NULL,
	[Max_elo] INT NULL,
	[Status] TINYINT NOT NULL DEFAULT 1,
	[Current_round] INT NOT NULL DEFAULT 0,
	[Women_only] BIT NOT NULL DEFAULT 0,
	[End_Registration] DATETIME NOT NULL,
	[Created_at] DATETIME NOT NULL DEFAULT GETDATE(),
	[Updated_at] DATETIME NOT NULL DEFAULT GETDATE(),
	[Cancelled] BIT NOT NULL DEFAULT 0,
	[Cancelled_at] DATETIME NULL

	CONSTRAINT PK_TOURNAMENT_ID PRIMARY KEY ([Id]),
	CONSTRAINT CK_TOURNAMENT_MIN_PLAYERS_MIN CHECK ([Min_players] >= 2),
	CONSTRAINT CK_TOURNAMENT_MAX_PLAYERS_MAX CHECK ([Max_players] >= [Min_players]),
	CONSTRAINT CK_TOURNAMENT_MIN_ELO_MIN CHECK ([Min_elo] >= 0),
	CONSTRAINT CK_TOURNAMENT_ELO_MAX CHECK ([Max_elo] >= [Min_elo]),
)
