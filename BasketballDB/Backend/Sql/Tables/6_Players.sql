IF OBJECT_ID(N'Basketball.Players') IS NULL
BEGIN
    CREATE TABLE Basketball.Players
    (
        PlayerID INT NOT NULL IDENTITY(1,1),
        TeamID INT NOT NULL,
        JerseyNumber INT NOT NULL,
        FirstName NVARCHAR(32) NOT NULL,
        LastName NVARCHAR(32) NOT NULL,
        Position NVARCHAR(2) NULL,
        Age INT NULL,
        Height NVARCHAR(16) NULL,
        Weight INT NULL,

        CONSTRAINT PK_Basketball_Players_PlayerID PRIMARY KEY CLUSTERED (PlayerID ASC),

        CONSTRAINT FK_Basketball_Players_TeamID
            FOREIGN KEY (TeamID)
            REFERENCES Basketball.Teams(TeamID)
    );
END;