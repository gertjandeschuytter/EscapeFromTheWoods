CREATE TABLE [dbo].[BosRecords] (
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [BosId]  INT NOT NULL,
    [BoomId] INT NOT NULL,
    [X]      INT NOT NULL,
    [Y]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE TABLE [dbo].[AapRecords] (
    [Id]              INT IDENTITY (1, 1)  NOT NULL,
    [BosId]           INT NOT NULL,
    [Naam]            NVARCHAR (255) NOT NULL,
    [BosRecordsId]    INT NOT NULL,
    [BoomId]          INT NOT NULL,
    [SequencieNummer] INT NOT NULL,
    [X]               INT NOT NULL,
    [Y]               INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AapRecords_BosRecords] FOREIGN KEY ([BosRecordsId]) REFERENCES [dbo].[BosRecords] ([Id])
);
GO
CREATE TABLE [dbo].[Logs] (
    [Id]      INT IDENTITY (1, 1)  NOT NULL,
    [BosId]   INT NOT NULL,
    [AapId]   INT NOT NULL,
    [Bericht] NVARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Logs_AapRecords] FOREIGN KEY ([AapId]) REFERENCES [dbo].[AapRecords] ([Id])
);
GO