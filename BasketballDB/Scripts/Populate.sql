/*******************************************************
 * Populate.sql
 * Seed data for BasketballLeague560
 * Run AFTER Setup.sql
 *******************************************************/

USE BasketballLeague560;
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
INSERT INTO Basketball.Players (TeamID, JerseyNumber, FirstName, LastName, Age, Height, Weight)
VALUES
    -- Team 1: Springfield Ballers
    (1, 4,  N'Marcus',   N'Webb',       28, N'6''2"',  195),
    (1, 5,  N'Derek',    N'Hollins',    31, N'6''0"',  180),
    (1, 6,  N'Tyrone',   N'Bass',       25, N'6''4"',  210),
    (1, 7,  N'Calvin',   N'Morrow',     29, N'5''11"', 175),
    (1, 8,  N'Jerome',   N'Patel',      33, NULL,      NULL),
    -- Team 2: Decatur Dunkers
    (2, 4,  N'Andre',    N'Simmons',    27, N'6''3"',  200),
    (2, 5,  N'Lamar',    N'Hughes',     30, N'6''1"',  185),
    (2, 6,  N'Darnell',  N'Cruz',       24, N'6''5"',  220),
    (2, 7,  N'Kenny',    N'Foster',     35, N'6''0"',  178),
    (2, 8,  N'Reggie',   N'Banks',      22, NULL,      NULL),
    -- Team 3: Peoria Pivots
    (3, 4,  N'Terrence', N'Price',      26, N'6''2"',  192),
    (3, 5,  N'Malik',    N'Stone',      29, N'6''0"',  183),
    (3, 6,  N'Darius',   N'Cole',       31, N'6''6"',  230),
    (3, 7,  N'Victor',   N'James',      28, N'5''10"', 170),
    (3, 8,  N'Alvin',    N'Grant',      23, NULL,      NULL),
    -- Team 4: Rockford Rockets
    (4, 4,  N'Shawn',    N'Murphy',     32, N'6''1"',  188),
    (4, 5,  N'Curtis',   N'Bell',       27, N'6''3"',  205),
    (4, 6,  N'Ronnie',   N'Dixon',      25, N'6''4"',  215),
    (4, 7,  N'Larry',    N'Shaw',       34, N'5''11"', 172),
    (4, 8,  N'Eddie',    N'Torres',     29, NULL,      NULL),
    -- Team 5: Springfield Slammers
    (5, 4,  N'Byron',    N'King',       26, N'6''2"',  194),
    (5, 5,  N'Cedric',   N'Ross',       30, N'6''0"',  181),
    (5, 6,  N'Nathan',   N'Perry',      28, N'6''5"',  218),
    (5, 7,  N'Elijah',   N'Reed',       24, N'6''1"',  190),
    (5, 8,  N'Harvey',   N'Cook',       33, NULL,      NULL),
    -- Team 6: Decatur Dribblers
    (6, 4,  N'Oscar',    N'Rivera',     27, N'6''3"',  202),
    (6, 5,  N'Willis',   N'Morgan',     31, N'6''1"',  186),
    (6, 6,  N'Quincy',   N'Powell',     25, N'6''6"',  225),
    (6, 7,  N'Roland',   N'Hayes',      29, N'5''10"', 168),
    (6, 8,  N'Sidney',   N'Bryant',     22, NULL,      NULL),
    -- Team 7: Peoria Panthers
    (7, 4,  N'Walter',   N'Coleman',    30, N'6''2"',  196),
    (7, 5,  N'Eugene',   N'Jenkins',    28, N'6''0"',  182),
    (7, 6,  N'Francis',  N'Sanders',    26, N'6''4"',  212),
    (7, 7,  N'Herbert',  N'Long',       35, N'5''11"', 174),
    (7, 8,  N'Irving',   N'Patterson',  24, NULL,      NULL),
    -- Team 8: Rockford Rims
    (8, 4,  N'Julian',   N'Alexander',  29, N'6''3"',  204),
    (8, 5,  N'Luther',   N'Scott',      32, N'6''1"',  187),
    (8, 6,  N'Milton',   N'Green',      27, N'6''5"',  222),
    (8, 7,  N'Nelson',   N'Adams',      25, N'6''0"',  176),
    (8, 8,  N'Otto',     N'Baker',      31, NULL,      NULL),
    -- Team 9: Riverside Runners
    (9, 4,  N'Percy',    N'Nelson',     28, N'6''2"',  193),
    (9, 5,  N'Quinn',    N'Carter',     26, N'6''0"',  180),
    (9, 6,  N'Russell',  N'Mitchell',   30, N'6''4"',  208),
    (9, 7,  N'Stanley',  N'Perez',      33, N'5''11"', 171),
    (9, 8,  N'Theron',   N'Roberts',    22, NULL,      NULL),
    -- Team 10: Anaheim Aces
    (10, 4, N'Ulysses',  N'Turner',     27, N'6''3"',  201),
    (10, 5, N'Vernon',   N'Phillips',   31, N'6''1"',  184),
    (10, 6, N'Winston',  N'Campbell',   25, N'6''6"',  228),
    (10, 7, N'Xavier',   N'Parker',     29, N'5''10"', 167),
    (10, 8, N'Yosef',    N'Evans',      24, NULL,      NULL),
    -- Team 11: Pasadena Pistons
    (11, 4, N'Zachary',  N'Edwards',    30, N'6''2"',  197),
    (11, 5, N'Aaron',    N'Collins',    28, N'6''0"',  183),
    (11, 6, N'Bernard',  N'Stewart',    26, N'6''4"',  213),
    (11, 7, N'Chester',  N'Sanchez',    35, N'5''11"', 173),
    (11, 8, N'Donald',   N'Morris',     23, NULL,      NULL),
    -- Team 12: Torrance Titans
    (12, 4, N'Edward',   N'Rogers',     29, N'6''3"',  205),
    (12, 5, N'Floyd',    N'Reed',       32, N'6''1"',  188),
    (12, 6, N'George',   N'Cook',       27, N'6''5"',  220),
    (12, 7, N'Harold',   N'Morgan',     25, N'6''0"',  177),
    (12, 8, N'Ivan',     N'Bell',       31, NULL,      NULL),
    -- Team 13: Riverside Raptors
    (13, 4, N'Jack',     N'Murphy',     28, N'6''2"',  194),
    (13, 5, N'Kevin',    N'Rivera',     26, N'6''0"',  181),
    (13, 6, N'Leonard',  N'Torres',     30, N'6''4"',  209),
    (13, 7, N'Manuel',   N'Flores',     33, N'5''11"', 172),
    (13, 8, N'Norman',   N'Washington', 22, NULL,      NULL),
    -- Team 14: Anaheim Arrows
    (14, 4, N'Patrick',  N'Lee',        27, N'6''3"',  202),
    (14, 5, N'Raymond',  N'Harris',     31, N'6''1"',  185),
    (14, 6, N'Samuel',   N'Clark',      25, N'6''6"',  226),
    (14, 7, N'Timothy',  N'Lewis',      29, N'5''10"', 169),
    (14, 8, N'Uriah',    N'Robinson',   24, NULL,      NULL),
    -- Team 15: Pasadena Prowlers
    (15, 4, N'Vincent',  N'Walker',     30, N'6''2"',  198),
    (15, 5, N'William',  N'Hall',       28, N'6''0"',  184),
    (15, 6, N'Alton',    N'Allen',      26, N'6''4"',  214),
    (15, 7, N'Bennie',   N'Young',      35, N'5''11"', 174),
    (15, 8, N'Carlos',   N'Hernandez',  23, NULL,      NULL),
    -- Team 16: Torrance Thunder
    (16, 4, N'Darryl',   N'King',       29, N'6''3"',  206),
    (16, 5, N'Earnest',  N'Wright',     32, N'6''1"',  189),
    (16, 6, N'Freddie',  N'Lopez',      27, N'6''5"',  221),
    (16, 7, N'Gilbert',  N'Hill',       25, N'6''0"',  178),
    (16, 8, N'Horace',   N'Scott',      31, NULL,      NULL);
GO

/****************************
 * Basketball.PlayerGameStats
 * One row per player per game
 * Each player (1-80) plays in multiple games
 * GameID matches actual games their team played
 ****************************/

-- Team 1 players (PlayerIDs 1-5) in Game 1 (Team1 vs Team2)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(1, 1, 1, 14, 28, 2, 5, 3),
(2, 1, 1, 18, 32, 3, 4, 6),
(3, 1, 1, 22, 35, 1, 8, 2),
(4, 1, 1,  8, 20, 4, 3, 1),
(5, 1, 1, 10, 18, 2, 2, 4);

-- Team 2 players (PlayerIDs 6-10) in Game 1 (Team1 vs Team2)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(6,  1, 2, 16, 30, 2, 6, 5),
(7,  1, 2, 20, 34, 1, 7, 3),
(8,  1, 2, 12, 22, 3, 4, 2),
(9,  1, 2,  6, 15, 4, 2, 1),
(10, 1, 2, 24, 38, 0, 9, 4);

-- Team 3 players (PlayerIDs 11-15) in Game 2 (Team3 vs Team4)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(11, 2, 3, 18, 31, 2, 5, 6),
(12, 2, 3, 10, 21, 3, 3, 2),
(13, 2, 3, 26, 40, 1, 10, 3),
(14, 2, 3,  8, 17, 4, 2, 1),
(15, 2, 3, 14, 27, 2, 4, 5);

-- Team 4 players (PlayerIDs 16-20) in Game 2 (Team3 vs Team4)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(16, 2, 4, 20, 33, 1, 7, 4),
(17, 2, 4, 16, 29, 2, 6, 3),
(18, 2, 4, 12, 23, 3, 3, 2),
(19, 2, 4,  4, 12, 5, 1, 0),
(20, 2, 4, 22, 36, 1, 8, 5);

-- Team 1 players in Game 3 (Team1 vs Team3)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(1, 3, 1, 10, 25, 3, 4, 2),
(2, 3, 1, 14, 28, 2, 5, 4),
(3, 3, 1, 18, 32, 1, 7, 3),
(4, 3, 1,  6, 18, 4, 2, 1),
(5, 3, 1, 12, 22, 2, 3, 5);

-- Team 3 players in Game 3 (Team1 vs Team3)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(11, 3, 3, 22, 36, 1, 8, 4),
(12, 3, 3, 14, 26, 3, 4, 3),
(13, 3, 3, 28, 40, 0, 11, 6),
(14, 3, 3,  8, 16, 4, 3, 1),
(15, 3, 3, 16, 29, 2, 6, 3);

-- Team 2 players in Game 4 (Team2 vs Team4)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(6,  4, 2, 20, 33, 1, 7, 5),
(7,  4, 2, 16, 29, 2, 6, 3),
(8,  4, 2, 12, 22, 3, 4, 2),
(9,  4, 2,  6, 14, 5, 1, 1),
(10, 4, 2, 18, 31, 2, 5, 4);

-- Team 4 players in Game 4 (Team2 vs Team4)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(16, 4, 4, 24, 38, 0, 9, 5),
(17, 4, 4, 10, 19, 4, 3, 2),
(18, 4, 4, 14, 25, 3, 4, 4),
(19, 4, 4, 28, 40, 0, 12, 6),
(20, 4, 4,  8, 15, 5, 2, 1);

-- Team 1 players in Game 5 (Team1 vs Team4)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(1, 5, 1, 16, 30, 2, 6, 3),
(2, 5, 1, 20, 34, 1, 7, 5),
(3, 5, 1, 12, 22, 3, 4, 2),
(4, 5, 1,  8, 16, 4, 2, 1),
(5, 5, 1, 18, 31, 2, 5, 4);

-- Team 4 players in Game 5 (Team1 vs Team4)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(16, 5, 4, 22, 36, 1, 8, 3),
(17, 5, 4, 10, 19, 4, 3, 2),
(18, 5, 4, 14, 25, 3, 4, 4),
(19, 5, 4, 24, 38, 0, 9, 5),
(20, 5, 4,  8, 15, 5, 2, 1);

-- Team 5 players (PlayerIDs 21-25) in Game 14 (Team5 vs Team6)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(21, 14, 5, 18, 30, 2, 5, 4),
(22, 14, 5, 14, 26, 3, 4, 3),
(23, 14, 5, 10, 20, 4, 2, 2),
(24, 14, 5, 28, 40, 0, 11, 6),
(25, 14, 5,  8, 16, 4, 3, 1);

-- Team 6 players (PlayerIDs 26-30) in Game 14 (Team5 vs Team6)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(26, 14, 6, 16, 29, 2, 6, 3),
(27, 14, 6, 20, 34, 1, 7, 5),
(28, 14, 6, 12, 22, 3, 4, 2),
(29, 14, 6,  6, 14, 5, 1, 1),
(30, 14, 6, 18, 31, 2, 5, 4);

-- Team 7 players (PlayerIDs 31-35) in Game 15 (Team7 vs Team8)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(31, 15, 7, 22, 36, 1, 8, 3),
(32, 15, 7, 10, 19, 4, 3, 2),
(33, 15, 7, 14, 25, 3, 4, 4),
(34, 15, 7, 24, 38, 0, 9, 5),
(35, 15, 7,  8, 15, 5, 2, 1);

-- Team 8 players (PlayerIDs 36-40) in Game 15 (Team7 vs Team8)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(36, 15, 8, 16, 28, 2, 6, 3),
(37, 15, 8, 20, 33, 1, 7, 4),
(38, 15, 8, 12, 21, 3, 3, 2),
(39, 15, 8,  4, 11, 6, 1, 0),
(40, 15, 8, 26, 40, 0, 10, 6);

-- Team 9 players (PlayerIDs 41-45) in Game 27 (Team9 vs Team10)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(41, 27, 9, 18, 30, 2, 5, 4),
(42, 27, 9, 14, 26, 3, 4, 3),
(43, 27, 9, 10, 19, 4, 2, 2),
(44, 27, 9, 22, 35, 1, 8, 5),
(45, 27, 9,  8, 16, 4, 3, 1);

-- Team 10 players (PlayerIDs 46-50) in Game 27 (Team9 vs Team10)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(46, 27, 10, 16, 28, 2, 6, 3),
(47, 27, 10, 20, 33, 1, 7, 4),
(48, 27, 10, 12, 22, 3, 4, 2),
(49, 27, 10,  6, 13, 5, 1, 1),
(50, 27, 10, 18, 31, 2, 5, 4);

-- Team 11 players (PlayerIDs 51-55) in Game 28 (Team11 vs Team12)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(51, 28, 11, 24, 38, 0, 9, 5),
(52, 28, 11, 10, 19, 4, 3, 2),
(53, 28, 11, 14, 25, 3, 4, 4),
(54, 28, 11, 28, 40, 0, 12, 6),
(55, 28, 11,  8, 15, 5, 2, 1);

-- Team 12 players (PlayerIDs 56-60) in Game 28 (Team11 vs Team12)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(56, 28, 12, 16, 28, 2, 6, 3),
(57, 28, 12, 20, 33, 1, 7, 4),
(58, 28, 12, 12, 21, 3, 3, 2),
(59, 28, 12,  4, 10, 6, 1, 0),
(60, 28, 12, 22, 36, 1, 8, 5);

-- Team 13 players (PlayerIDs 61-65) in Game 40 (Team13 vs Team14)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(61, 40, 13, 18, 30, 2, 5, 4),
(62, 40, 13, 14, 26, 3, 4, 3),
(63, 40, 13, 10, 19, 4, 2, 2),
(64, 40, 13, 26, 40, 0, 10, 6),
(65, 40, 13,  8, 16, 4, 3, 1);

-- Team 14 players (PlayerIDs 66-70) in Game 40 (Team13 vs Team14)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(66, 40, 14, 16, 29, 2, 6, 3),
(67, 40, 14, 20, 34, 1, 7, 5),
(68, 40, 14, 12, 22, 3, 4, 2),
(69, 40, 14,  6, 14, 5, 1, 1),
(70, 40, 14, 18, 31, 2, 5, 4);

-- Team 15 players (PlayerIDs 71-75) in Game 41 (Team15 vs Team16)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(71, 41, 15, 22, 36, 1, 8, 3),
(72, 41, 15, 10, 19, 4, 3, 2),
(73, 41, 15, 14, 25, 3, 4, 4),
(74, 41, 15, 24, 38, 0, 9, 5),
(75, 41, 15,  8, 15, 5, 2, 1);

-- Team 16 players (PlayerIDs 76-80) in Game 41 (Team15 vs Team16)
INSERT INTO Basketball.PlayerGameStats (PlayerID, GameID, TeamID, Points, PlayingTime, Turnovers, Rebounds, Assists) VALUES
(76, 41, 16, 16, 28, 2, 6, 3),
(77, 41, 16, 20, 33, 1, 7, 4),
(78, 41, 16, 12, 21, 3, 3, 2),
(79, 41, 16,  4, 11, 6, 1, 0),
(80, 41, 16, 28, 40, 0, 11, 6);
GO

PRINT 'Populate.sql complete.';
PRINT 'Totals: 4 Locations, 2 Leagues, 4 Seasons, 16 Teams, 52 Games, 80 Players, 160 PlayerGameStats rows.';
GO
