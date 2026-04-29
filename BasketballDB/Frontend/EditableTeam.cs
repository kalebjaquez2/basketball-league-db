using System.ComponentModel;
using System.Runtime.CompilerServices;
using Backend.Models;

namespace Frontend
{
    public class EditableTeam : INotifyPropertyChanged
    {
        private bool _isEditing;
        private string _editName;

        public event PropertyChangedEventHandler? PropertyChanged;

        public TeamWithPerformance Model { get; }

        public int TeamID => Model.TeamID;
        public int SeasonID => Model.SeasonID;
        public string TeamName => Model.TeamName;
        public int Wins => Model.Wins;
        public int Losses => Model.Losses;
        public decimal AverageScorePerGame => Model.AverageScorePerGame;

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }

        public string EditName
        {
            get => _editName;
            set { _editName = value; OnPropertyChanged(); }
        }

        public EditableTeam(TeamWithPerformance model)
        {
            Model = model;
            _editName = model.TeamName;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
