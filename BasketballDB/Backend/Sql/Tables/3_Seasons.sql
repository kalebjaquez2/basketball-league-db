IF OBJECT_ID(N'Basketball.Seasons') IS NULL
BEGIN
    CREATE TABLE Basketball.Seasons
    (
        SeasonID INT NOT NULL IDENTITY(1,1),
        LeagueID INT NOT NULL,
        StartDate DATE NOT NULL,
        EndDate DATE NOT NULL,
        CONSTRAINT PK_Basketball_Seasons_SeasonID PRIMARY KEY CLUSTERED
        (
            SeasonID ASC
        ),
        CONSTRAINT FK_Basketball_Seasons_LeagueID_Basketball_League
            FOREIGN KEY (LeagueID)
            REFERENCES Basketball.League(LeagueID)
    );
END;

/* Unique Constraints */
IF NOT EXISTS
    (
        SELECT *
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.Seasons')
            AND kc.[name] = N'UK_Basketball_Seasons_StartDate'
    )
BEGIN
    ALTER TABLE Basketball.Seasons
    ADD CONSTRAINT [UK_Basketball_Seasons_StartDate] UNIQUE NONCLUSTERED
    (
        StartDate ASC
    );
END;

IF NOT EXISTS
    (
        SELECT *
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.Seasons')
            AND kc.[name] = N'UK_Basketball_Seasons_EndDate'
    )
BEGIN
    ALTER TABLE Basketball.Seasons
    ADD CONSTRAINT [UK_Basketball_Seasons_EndDate] UNIQUE NONCLUSTERED
    (
        EndDate ASC
    );
END;