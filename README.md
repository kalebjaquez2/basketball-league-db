# Basketball League DB

A .NET 8.0 WPF desktop application for managing a recreational basketball league. Users can browse leagues, seasons, teams, rosters, games, and per-game player statistics through a dark-themed UI backed by a SQL Server database.

> **No ORM frameworks were used.** All database interaction is performed via raw SQL stored procedures using `Microsoft.Data.SqlClient`.

---

## Team Members

- Kaleb Jaquez
- Brett Stockwell
- Christian Rottinghaus

---

## Tech Stack

| Layer | Technology |
|---|---|
| UI | WPF (.NET 8, C#) |
| Data Access | Custom `SqlCommandExecutor` / command-object pattern |
| Database | SQL Server (LocalDB `MSSQLLocalDB`) |
| Auth | SHA-256 password hashing |

---

## Project Structure

```
BasketballDB/
├── Frontend/               WPF UI — pages, dialogs, editable view-models
├── Backend/
│   ├── Models/             Domain model classes
│   ├── Repositories/       Repository interfaces + SQL implementations
│   └── Sql/
│       ├── Procedures/     Stored procedure definitions (one file per entity)
│       └── Tables/         Table DDL scripts (one file per table)
├── DataAccess/             SqlCommandExecutor, delegate base classes, interfaces
├── Scripts/
│   ├── Setup.sql           Creates the database, all tables, and all procedures
│   ├── Populate.sql        Seeds initial data (locations, leagues, teams, players, games, stats)
│   ├── DropTables.sql      Drops all tables for a clean rebuild
│   └── RunSetup.bat        Convenience script — runs Setup.sql then Populate.sql via sqlcmd
└── BasketballDB.sln
```

---

## Database Setup

### Requirements
- SQL Server LocalDB (`(localdb)\MSSQLLocalDB`) — installed with Visual Studio

### Option A — Batch script (easiest)
```
cd BasketballDB\Scripts
RunSetup.bat
```

### Option B — Manual (SSMS or sqlcmd)
1. Run `Scripts/Setup.sql` — creates the `BasketballLeague560` database, schema, tables, and all stored procedures.
2. Run `Scripts/Populate.sql` — seeds locations, leagues, seasons, teams, players, games, and player game stats.

The database and all objects are created idempotently — safe to re-run.

---

## Running the Application

1. Complete the database setup above.
2. Open `BasketballDB.sln` in Visual Studio 2022.
3. Set `Frontend` as the startup project.
4. Press **F5**.

**Default login:** `admin` / `admin`

---

## Database Schema

| Table | Description |
|---|---|
| `Basketball.Location` | Cities/states where leagues are based |
| `Basketball.League` | Named leagues tied to a location |
| `Basketball.Seasons` | Date-ranged seasons within a league |
| `Basketball.Teams` | Teams competing in a season |
| `Basketball.Games` | Scheduled games between two teams |
| `Basketball.Players` | Players rostered to a team |
| `Basketball.PlayerGameStats` | Per-player per-game statistics |
| `Basketball.Users` | Application user accounts (username + SHA-256 password hash) |

---

## Stored Procedures

All CRUD operations are performed through stored procedures in the `Basketball` schema. Procedure files are in `BasketballDB/Backend/Sql/Procedures/`:

| File | Procedures |
|---|---|
| `Location.sql` | CreateLocation, FetchLocation, RetrieveLocations, UpdateLocation |
| `League.sql` | CreateLeague, FetchLeague, RetrieveLeagues, UpdateLeague, DeleteLeague |
| `Seasons.sql` | CreateSeason, FetchSeason, RetrieveSeasonsByLeague, UpdateSeason |
| `Teams.sql` | CreateTeam, FetchTeam, RetrieveTeamsBySeason, UpdateTeam, DeleteTeam |
| `Games.sql` | CreateGame, FetchGame, RetrieveGamesByTeam, RetrieveGamesBySeason, UpdateGame |
| `Players.sql` | CreatePlayer, FetchPlayer, RetrievePlayersByTeam, UpdatePlayer, DeletePlayer |
| `PlayerGameStats.sql` | CreatePlayerGameStats, FetchPlayerGameStats, RetrieveStatsByGame, RetrieveStatsByPlayer, UpdatePlayerGameStats, DeletePlayerGameStats |
| `Stats.sql` | RetrieveMostActivePlayers, RetrieveTopScorersByTeam, RetrieveTeamPerformance, RetrieveGameStatsSummary |

---

## Features

- **League / Season / Team / Player management** — full CRUD for admins, read-only for regular users
- **Game scheduling** — create games between two teams in a season
- **Box score editor** — double-click a player row to edit their stats inline with auto-save
- **Auto-fill stats** — admin button generates random realistic stats for all players in a game
- **Standings** — win/loss record and average score per game, updated live
- **Most Active Players** — ranked by games played and points per game for a season
- **Top Scorers** — per-team ranking by total points for a season
- **Game Stats Summary** — average points, rebounds, assists, and turnovers per game over a date range
- **User management** — admins can create accounts, toggle admin status, and edit credentials
- **Sign out / re-login** without restarting the application
