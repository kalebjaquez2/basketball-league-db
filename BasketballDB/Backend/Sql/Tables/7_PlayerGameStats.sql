IF OBJECT_ID(N'Basketball.PlayerGameStats') IS NULL
BEGIN
    CREATE TABLE Basketball.PlayerGameStats
    (
        PlayerID INT NOT NULL,
         GameID INT NOT NULL,
        Points INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Points DEFAULT(0),
        PlayingTime INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_PlayingTime DEFAULT(0),
        Turnovers INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Turnovers DEFAULT(0),
        Rebounds INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Rebounds DEFAULT(0),
        Assists INT NOT NULL CONSTRAINT DF_Basketball_PlayerGameStats_Assists DEFAULT(0),
        CONSTRAINT PK_Basketball_PlayerGameStats_PlayerID PRIMARY KEY CLUSTERED
        (
            PlayerID ASC
        ),
        CONSTRAINT FK_Basketball_PlayerGameStats_PlayerID_Basketball_Players
            FOREIGN KEY (PlayerID)
            REFERENCES Basketball.Players(PlayerID)
    );
END;