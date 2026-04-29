using System.ComponentModel;
using System.Runtime.CompilerServices;
using Backend.Models;

namespace Frontend
{
    public class EditablePlayerGameStats : INotifyPropertyChanged
    {
        private bool _isEditing;
        private string _editMinutes;
        private string _editRebounds;
        private string _editAssists;
        private string _editTurnovers;
        private string _editSteals;
        private string _editBlocks;
        private string _editFGMade;
        private string _editFGTaken;
        private string _editThreeMade;
        private string _editThreeTaken;
        private string _editFouls;

        public event PropertyChangedEventHandler? PropertyChanged;

        public PlayerGameStats Model { get; }

        public int PlayerID => Model.PlayerID;
        public int GameID => Model.GameID;
        public int TeamID => Model.TeamID;
        public string? PlayerName => Model.PlayerName;
        public int JerseyNumber => 0; // not stored on stats
        public int Points => Model.Points;
        public int PlayingTime => Model.PlayingTime;
        public int Rebounds => Model.Rebounds;
        public int Assists => Model.Assists;
        public int Turnovers => Model.Turnovers;
        public int Steals => Model.Steals;
        public int Blocks => Model.Blocks;
        public int FieldGoalsMade => Model.FieldGoalsMade;
        public int FieldGoalsTaken => Model.FieldGoalsTaken;
        public int ThreePointersMade => Model.ThreePointersMade;
        public int ThreePointersTaken => Model.ThreePointersTaken;
        public int PersonalFouls => Model.PersonalFouls;
        public string FieldGoalsDisplay => Model.FieldGoalsDisplay;
        public string ThreePointersDisplay => Model.ThreePointersDisplay;

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }

        public string EditMinutes   { get => _editMinutes;   set { _editMinutes = value;   OnPropertyChanged(); } }
        public string EditRebounds  { get => _editRebounds;  set { _editRebounds = value;  OnPropertyChanged(); } }
        public string EditAssists   { get => _editAssists;   set { _editAssists = value;   OnPropertyChanged(); } }
        public string EditTurnovers { get => _editTurnovers; set { _editTurnovers = value; OnPropertyChanged(); } }
        public string EditSteals    { get => _editSteals;    set { _editSteals = value;    OnPropertyChanged(); } }
        public string EditBlocks    { get => _editBlocks;    set { _editBlocks = value;    OnPropertyChanged(); } }
        public string EditFGMade    { get => _editFGMade;    set { _editFGMade = value;    OnPropertyChanged(); } }
        public string EditFGTaken   { get => _editFGTaken;   set { _editFGTaken = value;   OnPropertyChanged(); } }
        public string EditThreeMade { get => _editThreeMade; set { _editThreeMade = value; OnPropertyChanged(); } }
        public string EditThreeTaken{ get => _editThreeTaken;set { _editThreeTaken = value;OnPropertyChanged(); } }
        public string EditFouls     { get => _editFouls;     set { _editFouls = value;     OnPropertyChanged(); } }

        public EditablePlayerGameStats(PlayerGameStats model)
        {
            Model = model;
            _editMinutes    = model.PlayingTime.ToString();
            _editRebounds   = model.Rebounds.ToString();
            _editAssists    = model.Assists.ToString();
            _editTurnovers  = model.Turnovers.ToString();
            _editSteals     = model.Steals.ToString();
            _editBlocks     = model.Blocks.ToString();
            _editFGMade     = model.FieldGoalsMade.ToString();
            _editFGTaken    = model.FieldGoalsTaken.ToString();
            _editThreeMade  = model.ThreePointersMade.ToString();
            _editThreeTaken = model.ThreePointersTaken.ToString();
            _editFouls      = model.PersonalFouls.ToString();
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
