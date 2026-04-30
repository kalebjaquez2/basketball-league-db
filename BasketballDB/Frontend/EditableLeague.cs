using System.ComponentModel;
using System.Runtime.CompilerServices;
using Backend.Models;

namespace Frontend
{
    public class EditableLeague : INotifyPropertyChanged
    {
        private bool _isEditing;
        private string _editName;

        public event PropertyChangedEventHandler? PropertyChanged;

        public League Model { get; }

        public int LeagueID  => Model.LeagueID;
        public string LeagueName => Model.LeagueName;
        public string Location   => Model.Location;
        public int LocationID    => Model.LocationID;

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

        public EditableLeague(League model)
        {
            Model = model;
            _editName = model.LeagueName;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
