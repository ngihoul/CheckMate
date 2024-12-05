﻿CREATE TABLE [Tournament_category]
(
	[Id] INT IDENTITY,
	[Name] VARCHAR(100) NOT NULL,
	[Rules] VARCHAR(50) NOT NULL,

	CONSTRAINT PK_TOURNAMENT_CATEGORY_PRIMARY_KEY PRIMARY KEY ([Id]),
	CONSTRAINT UK_TOURNAMENT_CATEGORY_NAME UNIQUE ([Name])
)