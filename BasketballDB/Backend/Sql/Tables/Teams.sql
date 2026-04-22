IF OBJECT_ID(N'Basketball.Teams') IS NULL
BEGIN
    CREATE TABLE Basketball.Teams
    (
        TeamID INT NOT NULL IDENTITY(1,1),
        SeasonID INT NOT NULL,
        TeamName NVARCHAR(64) NOT NULL,
        CONSTRAINT PK_Basketball_Teams_TeamID PRIMARY KEY CLUSTERED
        (
            TeamID ASC
        ),
        CONSTRAINT FK_Basketball_Teams_SeasonID_Basketball_Seasons
            FOREIGN KEY (SeasonID)
            REFERENCES Basketball.Seasons(SeasonID)
    );
END;

/****************************
 * Unique Constraints
 ****************************/
IF NOT EXISTS
    (
        SELECT *
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'Basketball.Teams')
            AND kc.[name] = N'UK_Basketball_Teams_TeamName'
    )
BEGIN
    ALTER TABLE Basketball.Teams
    ADD CONSTRAINT [UK_Basketball_Teams_TeamName] UNIQUE NONCLUSTERED
    (
        TeamName ASC
    );
END;