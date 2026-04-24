IF OBJECT_ID(N'Basketball.Players') IS NULL
BEGIN
    CREATE TABLE Basketball.Players
    (
        PlayerID INT NOT NULL IDENTITY(1,1),
        GameID INT NOT NULL,
        JerseyNumber INT NOT NULL,
        FirstName NVARCHAR(32) NOT NULL,
        LastName NVARCHAR(32) NOT NULL,
        Age INT NULL,
        Height NVARCHAR(16) NULL,
        Weight INT NULL,
        CONSTRAINT PK_Basketball_Players_PlayerID PRIMARY KEY CLUSTERED
        (
            PlayerID ASC
        ),
        CONSTRAINT FK_Basketball_Players_GameID_Basketball_Games
            FOREIGN KEY (GameID)
            REFERENCES Basketball.Games(GameID)
    );
END;