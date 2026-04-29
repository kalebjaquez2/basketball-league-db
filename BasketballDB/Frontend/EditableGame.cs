using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Backend.Models;

namespace Frontend
{
    public class EditableGame : INotifyPropertyChanged
    {
        private bool _isEditing;
        private string _editHomeScore;
        private string _editAwayScore;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Game Model { get; }

        public int GameID => Model.GameID;
        public int HomeTeamID => Model.HomeTeamID;
        public int AwayTeamID => Model.AwayTeamID;
        public string HomeTeamName => Model.HomeTeamName;
        public string AwayTeamName => Model.AwayTeamName;
        public int HomeTeamScore => Model.HomeTeamScore;
        public int AwayTeamScore => Model.AwayTeamScore;
        public int CourtNumber => Model.CourtNumber;
        public int OvertimeCount => Model.OvertimeCount;
        public DateOnly Date => Model.Date;

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }

        public string EditHomeScore
        {
            get => _editHomeScore;
            set { _editHomeScore = value; OnPropertyChanged(); }
        }

        public string EditAwayScore
        {
            get => _editAwayScore;
            set { _editAwayScore = value; OnPropertyChanged(); }
        }

        public EditableGame(Game model)
        {
            Model = model;
            _editHomeScore = model.HomeTeamScore.ToString();
            _editAwayScore = model.AwayTeamScore.ToString();
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
