IF OBJECT_ID(N'Basketball.League') IS NULL
BEGIN
    CREATE TABLE Basketball.League
    (
        LeagueID INT NOT NULL IDENTITY(1,1),
        LeagueName NVARCHAR(64) NOT NULL,
        LocationID INT NOT NULL,
        CONSTRAINT PK_Basketball_League_LeagueID PRIMARY KEY CLUSTERED
        (
            LeagueID ASC
        ),
        CONSTRAINT FK_Basketball_League_LocationID_Basketball_Location
            FOREIGN KEY (LocationID)
            REFERENCES Basketball.Location(LocationID)
    );
END;

/****************************
 * Unique Constraints
 ****************************/
IF NOT EXISTS
    (
        SELECT *
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.League')
            AND kc.[name] = N'UK_Basketball_League_LeagueName'
    )
BEGIN
    ALTER TABLE Basketball.League
    ADD CONSTRAINT [UK_Basketball_League_LeagueName] UNIQUE NONCLUSTERED
    (
        LeagueName ASC
    );
END;

/****************************
 * Soft Delete Column
 ****************************/
IF NOT EXISTS
    (
        SELECT *
        FROM sys.columns
        WHERE object_id = OBJECT_ID(N'Basketball.League')
            AND name = 'IsDeleted'
    )
BEGIN
    ALTER TABLE Basketball.League
    ADD IsDeleted BIT NOT NULL DEFAULT 0;
END;