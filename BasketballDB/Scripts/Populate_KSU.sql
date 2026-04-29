/*******************************************************
 * Populate.sql
 * Seed data for BasketballLeague560
 * Run AFTER Setup.sql
 *******************************************************/

USE kalebjaquez;
GO

/****************************
 * Basketball.Location
 ****************************/
INSERT INTO Basketball.Location (City, State, Country)
VALUES
    (N'Springfield',  N'Illinois',        N'USA'),
    (N'Riverside',    N'California',      N'USA'),
    (N'Lakewood',     N'Colorado',        N'USA'),
    (N'Greenville',   N'South Carolina',  N'USA');
GO

/****************************
 * Basketball.League
 ****************************/
INSERT INTO Basketball.League (LeagueName, LocationID)
VALUES
    (N'Midwest Rec Basketball League',   1),
    (N'West Coast Amateur Hoops League', 2);
GO

/****************************
 * Basketball.Seasons
 ****************************/
INSERT INTO Basketball.Seasons (LeagueID, StartDate, EndDate)
VALUES
    (1, '2022-09-01', '2022-12-15'),
    (1, '2023-09-01', '2023-12-15'),
    (2, '2022-10-01', '2023-01-20'),
    (2, '2023-10-01', '2024-01-20');
GO

/****************************
 * Basketball.Teams
 * SeasonID 1: TeamIDs 1-4
 * SeasonID 2: TeamIDs 5-8
 * SeasonID 3: TeamIDs 9-12
 * SeasonID 4: TeamIDs 13-16
 ****************************/
INSERT INTO Basketball.Teams (SeasonID, TeamName)
VALUES
    (1, N'Springfield Ballers'),
    (1, N'Decatur Dunkers'),
    (1, N'Peoria Pivots'),
    (1, N'Rockford Rockets'),
    (2, N'Springfield Slammers'),
    (2, N'Decatur Dribblers'),
    (2, N'Peoria Panthers'),
    (2, N'Rockford Rims'),
    (3, N'Riverside Runners'),
    (3, N'Anaheim Aces'),
    (3, N'Pasadena Pistons'),
    (3, N'Torrance Titans'),
    (4, N'Riverside Raptors'),
    (4, N'Anaheim Arrows'),
    (4, N'Pasadena Prowlers'),
    (4, N'Torrance Thunder');
GO

/****************************
 * Basketball.Games
 * GameIDs 1-52
 ****************************/
INSERT INTO Basketball.Games (HomeTeamID, AwayTeamID, HomeTeamScore, AwayTeamScore, CourtNumber, OvertimeCount, Date)
VALUES
    -- Midwest Season 1 (TeamIDs 1-4)
    (1, 2,  68, 74,  1, 0, '2022-09-10'),
    (3, 4,  81, 79,  2, 0, '2022-09-10'),
    (1, 3,  55, 60,  1, 0, '2022-09-17'),
    (2, 4,  88, 85,  2, 1, '2022-09-17'),
    (1, 4,  72, 69,  1, 0, '2022-09-24'),
    (2, 3,  63, 63,  2, 1, '2022-09-24'),
    (4, 1,  77, 80,  1, 0, '2022-10-01'),
    (3, 2,  91, 84,  2, 0, '2022-10-01'),
    (4, 2,  66, 70,  1, 0, '2022-10-08'),
    (3, 1,  58, 74,  2, 0, '2022-10-08'),
    (1, 2,  83, 79,  1, 0, '2022-10-15'),
    (3, 4,  70, 68,  2, 0, '2022-10-15'),
    (2, 1,  76, 90,  1, 2, '2022-10-22'),
    -- Midwest Season 2 (TeamIDs 5-8)
    (5, 6,  64, 71,  1, 0, '2023-09-09'),
    (7, 8,  88, 82,  2, 0, '2023-09-09'),
    (5, 7,  77, 73,  1, 0, '2023-09-16'),
    (6, 8,  59, 67,  2, 0, '2023-09-16'),
    (5, 8,  80, 80,  1, 1, '2023-09-23'),
    (6, 7,  74, 68,  2, 0, '2023-09-23'),
    (8, 5,  66, 72,  1, 0, '2023-09-30'),
    (7, 6,  85, 79,  2, 0, '2023-09-30'),
    (8, 6,  90, 88,  1, 1, '2023-10-07'),
    (7, 5,  61, 75,  2, 0, '2023-10-07'),
    (5, 6,  78, 82,  1, 0, '2023-10-14'),
    (7, 8,  69, 65,  2, 0, '2023-10-14'),
    (6, 5,  73, 77,  1, 0, '2023-10-21'),
    -- West Coast Season 1 (TeamIDs 9-12)
    (9,  10, 84, 76,  3, 0, '2022-10-08'),
    (11, 12, 70, 74,  4, 0, '2022-10-08'),
    (9,  11, 66, 61,  3, 0, '2022-10-15'),
    (10, 12, 79, 83,  4, 0, '2022-10-15'),
    (9,  12, 88, 85,  3, 1, '2022-10-22'),
    (10, 11, 72, 69,  4, 0, '2022-10-22'),
    (12, 9,  65, 78,  3, 0, '2022-10-29'),
    (11, 10, 90, 87,  4, 1, '2022-10-29'),
    (12, 10, 74, 80,  3, 0, '2022-11-05'),
    (11, 9,  68, 71,  4, 0, '2022-11-05'),
    (9,  10, 77, 73,  3, 0, '2022-11-12'),
    (11, 12, 83, 79,  4, 0, '2022-11-12'),
    (10, 9,  86, 91,  3, 0, '2022-11-19'),
    -- West Coast Season 2 (TeamIDs 13-16)
    (13, 14, 75, 82,  3, 0, '2023-10-07'),
    (15, 16, 68, 65,  4, 0, '2023-10-07'),
    (13, 15, 90, 88,  3, 1, '2023-10-14'),
    (14, 16, 77, 72,  4, 0, '2023-10-14'),
    (13, 16, 63, 70,  3, 0, '2023-10-21'),
    (14, 15, 84, 80,  4, 0, '2023-10-21'),
    (16, 13, 71, 69,  3, 0, '2023-10-28'),
    (15, 14, 78, 85,  4, 0, '2023-10-28'),
    (16, 14, 92, 89,  3, 1, '2023-11-04'),
    (15, 13, 66, 74,  4, 0, '2023-11-04'),
    (13, 14, 80, 76,  3, 0, '2023-11-11'),
    (15, 16, 73, 71,  4, 0, '2023-11-11'),
    (14, 13, 88, 84,  3, 0, '2023-11-18');
GO

/****************************
 * Basketball.Players
 * TeamID used instead of GameID
 * 5 players per team = 80 players total
 * Teams 1-16, PlayerIDs 1-80
 ****************************/
INSERT INTO Basketball.Players (TeamID, JerseyNumber, FirstName, LastName, Position, Age, Height, Weight)
VALUES
    -- Team 1: Springfield Ballers
    (1, 4,  N'Marcus',   N'Webb',       N'PG', 28, N'6''2"', 195),
    (1, 5,  N'Derek',    N'Hollins',    N'SG', 31, N'6''0"', 180),
    (1, 6,  N'Tyrone',   N'Bass',       N'SF', 25, N'6''4"', 210),
    (1, 7,  N'Calvin',   N'Morrow',     N'PG', 29, N'5''11"', 175),
    (1, 8,  N'Jerome',   N'Patel',      N'C',  33, NULL,      NULL),

    -- Team 2: Decatur Dunkers
    (2, 4,  N'Andre',    N'Simmons',    N'SG', 27, N'6''3"', 200),
    (2, 5,  N'Lamar',    N'Hughes',     N'PG', 30, N'6''1"', 185),
    (2, 6,  N'Darnell',  N'Cruz',       N'C',  24, N'6''5"', 220),
    (2, 7,  N'Kenny',    N'Foster',     N'SF', 35, N'6''0"', 178),
    (2, 8,  N'Reggie',   N'Banks',      N'PF', 22, NULL,      NULL),

    -- Team 3: Peoria Pivots
    (3, 4,  N'Terrence', N'Price',      N'PG', 26, N'6''2"', 192),
    (3, 5,  N'Malik',    N'Stone',      N'SG', 29, N'6''0"', 183),
    (3, 6,  N'Darius',   N'Cole',       N'C',  31, N'6''6"', 230),
    (3, 7,  N'Victor',   N'James',      N'PG', 28, N'5''10"', 170),
    (3, 8,  N'Alvin',    N'Grant',      N'PF', 23, NULL,      NULL),

    -- Team 4: Rockford Rockets
    (4, 4,  N'Shawn',    N'Murphy',     N'SG', 32, N'6''1"', 188),
    (4, 5,  N'Curtis',   N'Bell',       N'SF', 27, N'6''3"', 205),
    (4, 6,  N'Ronnie',   N'Dixon',      N'C',  25, N'6''4"', 215),
    (4, 7,  N'Larry',    N'Shaw',       N'PG', 34, N'5''11"', 172),
    (4, 8,  N'Eddie',    N'Torres',     N'PF', 29, NULL,      NULL),

    -- Team 5: Springfield Slammers
    (5, 4,  N'Byron',    N'King',       N'SF', 26, N'6''2"', 194),
    (5, 5,  N'Cedric',   N'Ross',       N'PG', 30, N'6''0"', 181),
    (5, 6,  N'Nathan',   N'Perry',      N'C',  28, N'6''5"', 218),
    (5, 7,  N'Elijah',   N'Reed',       N'SG', 24, N'6''1"', 190),
    (5, 8,  N'Harvey',   N'Cook',       N'PF', 33, NULL,      NULL),

    -- Team 6: Decatur Dribblers
    (6, 4,  N'Oscar',    N'Rivera',     N'SG', 27, N'6''3"', 202),
    (6, 5,  N'Willis',   N'Morgan',     N'PG', 31, N'6''1"', 186),
    (6, 6,  N'Quincy',   N'Powell',     N'C',  25, N'6''6"', 225),
    (6, 7,  N'Roland',   N'Hayes',      N'SF', 29, N'5''10"', 168),
    (6, 8,  N'Sidney',   N'Bryant',     N'PF', 22, NULL,      NULL),

    -- Team 7: Peoria Panthers
    (7, 4,  N'Walter',   N'Coleman',    N'SF', 30, N'6''2"', 196),
    (7, 5,  N'Eugene',   N'Jenkins',    N'PG', 28, N'6''0"', 182),
    (7, 6,  N'Francis',  N'Sanders',    N'C',  26, N'6''4"', 212),
    (7, 7,  N'Herbert',  N'Long',       N'SG', 35, N'5''11"', 174),
    (7, 8,  N'Irving',   N'Patterson',  N'PF', 24, NULL,      NULL),

    -- Team 8: Rockford Rims
    (8, 4,  N'Julian',   N'Alexander',  N'SG', 29, N'6''3"', 204),
    (8, 5,  N'Luther',   N'Scott',      N'PG', 32, N'6''1"', 187),
    (8, 6,  N'Milton',   N'Green',      N'C',  27, N'6''5"', 222),
    (8, 7,  N'Nelson',   N'Adams',      N'SF', 25, N'6''0"', 176),
    (8, 8,  N'Otto',     N'Baker',      N'PF', 31, NULL,      NULL),

    -- Team 9: Riverside Runners
    (9, 4,  N'Percy',    N'Nelson',     N'SF', 28, N'6''2"', 193),
    (9, 5,  N'Quinn',    N'Carter',     N'PG', 26, N'6''0"', 180),
    (9, 6,  N'Russell',  N'Mitchell',   N'C',  30, N'6''4"', 208),
    (9, 7,  N'Stanley',  N'Perez',      N'SG', 33, N'5''11"', 171),
    (9, 8,  N'Theron',   N'Roberts',    N'PF', 22, NULL,      NULL),

    -- Team 10: Anaheim Aces
    (10, 4, N'Ulysses',  N'Turner',     N'SG', 27, N'6''3"', 201),
    (10, 5, N'Vernon',   N'Phillips',   N'PG', 31, N'6''1"', 184),
    (10, 6, N'Winston',  N'Campbell',   N'C',  25, N'6''6"', 228),
    (10, 7, N'Xavier',   N'Parker',     N'SF', 29, N'5''10"', 167),
    (10, 8, N'Yosef',    N'Evans',      N'PF', 24, NULL,      NULL),

    -- Team 11: Pasadena Pistons
    (11, 4, N'Zachary',  N'Edwards',    N'SF', 30, N'6''2"', 197),
    (11, 5, N'Aaron',    N'Collins',    N'PG', 28, N'6''0"', 183),
    (11, 6, N'Bernard',  N'Stewart',    N'C',  26, N'6''4"', 213),
    (11, 7, N'Chester',  N'Sanchez',    N'SG', 35, N'5''11"', 173),
    (11, 8, N'Donald',   N'Morris',     N'PF', 23, NULL,      NULL),

    -- Team 12: Torrance Titans
    (12, 4, N'Edward',   N'Rogers',     N'SG', 29, N'6''3"', 205),
    (12, 5, N'Floyd',    N'Reed',       N'PG', 32, N'6''1"', 188),
    (12, 6, N'George',   N'Cook',       N'C',  27, N'6''5"', 220),
    (12, 7, N'Harold',   N'Morgan',     N'SF', 25, N'6''0"', 177),
    (12, 8, N'Ivan',     N'Bell',       N'PF', 31, NULL,      NULL),

    -- Team 13: Riverside Raptors
    (13, 4, N'Jack',     N'Murphy',     N'SF', 28, N'6''2"', 194),
    (13, 5, N'Kevin',    N'Rivera',     N'PG', 26, N'6''0"', 181),
    (13, 6, N'Leonard',  N'Torres',     N'C',  30, N'6''4"', 209),
    (13, 7, N'Manuel',   N'Flores',     N'SG', 33, N'5''11"', 172),
    (13, 8, N'Norman',   N'Washington', N'PF', 22, NULL,      NULL),

    -- Team 14: Anaheim Arrows
    (14, 4, N'Patrick',  N'Lee',        N'SG', 27, N'6''3"', 202),
    (14, 5, N'Raymond',  N'Harris',     N'PG', 31, N'6''1"', 185),
    (14, 6, N'Samuel',   N'Clark',      N'C',  25, N'6''6"', 226),
    (14, 7, N'Timothy',  N'Lewis',      N'SF', 29, N'5''10"', 169),
    (14, 8, N'Uriah',    N'Robinson',   N'PF', 24, NULL,      NULL),

    -- Team 15: Pasadena Prowlers
    (15, 4, N'Vincent',  N'Walker',     N'SF', 30, N'6''2"', 198),
    (15, 5, N'William',  N'Hall',       N'PG', 28, N'6''0"', 184),
    (15, 6, N'Alton',    N'Allen',      N'C',  26, N'6''4"', 214),
    (15, 7, N'Bennie',   N'Young',      N'SG', 35, N'5''11"', 174),
    (15, 8, N'Carlos',   N'Hernandez',  N'PF', 23, NULL,      NULL),

    -- Team 16: Torrance Thunder
    (16, 4, N'Darryl',   N'King',       N'SG', 29, N'6''3"', 206),
    (16, 5, N'Earnest',  N'Wright',     N'PG', 32, N'6''1"', 189),
    (16, 6, N'Freddie',  N'Lopez',      N'C',  27, N'6''5"', 221),
    (16, 7, N'Gilbert',  N'Hill',       N'SF', 25, N'6''0"', 178),
    (16, 8, N'Horace',   N'Scott',      N'PF', 31, NULL,      NULL);
GO

/****************************
 * Basketball.PlayerGameStats
 * One row per player per game
 * Each player (1-80) plays in multiple games
 * GameID matches actual games their team played
 ****************************/

INSERT INTO Basketball.PlayerGameStats
(PlayerID, GameID, TeamID, PlayingTime, Turnovers, Rebounds, Assists,
 FieldGoalsMade, FieldGoalsTaken, ThreePointersMade, ThreePointersTaken, PersonalFouls)

-- =========================
-- HOME TEAM
-- =========================
SELECT
    p.PlayerID,
    g.GameID,
    p.TeamID,

    stats.playingTime,
    stats.turnovers,
    stats.rebounds,
    stats.assists,

    fg.fgMade,
    fg.fgTaken,
    tp.tpMade,
    tp.tpTaken,

    stats.fouls

FROM Basketball.Games g
JOIN Basketball.Players p
    ON p.TeamID = g.HomeTeamID

CROSS APPLY (
    SELECT
        ABS(CHECKSUM(NEWID())) % 30 + 5 AS playingTime,
        ABS(CHECKSUM(NEWID())) % 6 AS turnovers,
        ABS(CHECKSUM(NEWID())) % 10 AS rebounds,
        ABS(CHECKSUM(NEWID())) % 8 AS assists,
        ABS(CHECKSUM(NEWID())) % 5 AS fouls
) stats

-- FG chain (FIXED)
CROSS APPLY (
    SELECT (ABS(CHECKSUM(NEWID())) % 18) + 5 AS fgTaken
) fgTakenCalc
CROSS APPLY (
    SELECT ABS(CHECKSUM(NEWID())) % (fgTakenCalc.fgTaken + 1) AS fgMade,
           fgTakenCalc.fgTaken AS fgTaken
) fg

-- 3PT chain (FIXED)
CROSS APPLY (
    SELECT (ABS(CHECKSUM(NEWID())) % 10) + 1 AS tpTaken
) tpTakenCalc
CROSS APPLY (
    SELECT ABS(CHECKSUM(NEWID())) % (tpTakenCalc.tpTaken + 1) AS tpMade,
           tpTakenCalc.tpTaken AS tpTaken
) tp


UNION ALL

-- =========================
-- AWAY TEAM
-- =========================
SELECT
    p.PlayerID,
    g.GameID,
    p.TeamID,

    stats.playingTime,
    stats.turnovers,
    stats.rebounds,
    stats.assists,

    fg.fgMade,
    fg.fgTaken,
    tp.tpMade,
    tp.tpTaken,

    stats.fouls

FROM Basketball.Games g
JOIN Basketball.Players p
    ON p.TeamID = g.AwayTeamID

CROSS APPLY (
    SELECT
        ABS(CHECKSUM(NEWID())) % 30 + 5 AS playingTime,
        ABS(CHECKSUM(NEWID())) % 6 AS turnovers,
        ABS(CHECKSUM(NEWID())) % 10 AS rebounds,
        ABS(CHECKSUM(NEWID())) % 8 AS assists,
        ABS(CHECKSUM(NEWID())) % 5 AS fouls
) stats

-- FG chain (FIXED)
CROSS APPLY (
    SELECT (ABS(CHECKSUM(NEWID())) % 18) + 5 AS fgTaken
) fgTakenCalc
CROSS APPLY (
    SELECT ABS(CHECKSUM(NEWID())) % (fgTakenCalc.fgTaken + 1) AS fgMade,
           fgTakenCalc.fgTaken AS fgTaken
) fg

-- 3PT chain (FIXED)
CROSS APPLY (
    SELECT (ABS(CHECKSUM(NEWID())) % 10) + 1 AS tpTaken
) tpTakenCalc
CROSS APPLY (
    SELECT ABS(CHECKSUM(NEWID())) % (tpTakenCalc.tpTaken + 1) AS tpMade,
           tpTakenCalc.tpTaken AS tpTaken
) tp;

/****************************
 * Initial admin account
 * Username: admin  Password: admin  (SHA-256)
 ****************************/
IF NOT EXISTS (SELECT 1 FROM Basketball.Users WHERE Username = N'admin')
    INSERT INTO Basketball.Users (Username, PasswordHash, IsAdmin)
    VALUES (N'admin',
            N'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',
            1);
GO

PRINT 'Populate.sql complete.';
PRINT 'Totals: 4 Locations, 2 Leagues, 4 Seasons, 16 Teams, 52 Games, 80 Players, 160 PlayerGameStats rows.';
GO
