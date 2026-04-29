using System.ComponentModel;
using System.Runtime.CompilerServices;
using Backend.Models;

namespace Frontend
{
    public class EditablePlayer : INotifyPropertyChanged
    {
        private bool _isEditing;
        private string _editJerseyNumber;
        private string _editPosition;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Player Model { get; }

        public int PlayerID => Model.PlayerID;
        public int TeamID => Model.TeamID;
        public int JerseyNumber => Model.JerseyNumber;
        public string FirstName => Model.FirstName;
        public string LastName => Model.LastName;
        public string? Position => Model.Position;

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }

        public string EditJerseyNumber
        {
            get => _editJerseyNumber;
            set { _editJerseyNumber = value; OnPropertyChanged(); }
        }

        public string EditPosition
        {
            get => _editPosition;
            set { _editPosition = value; OnPropertyChanged(); }
        }

        public EditablePlayer(Player model)
        {
            Model = model;
            _editJerseyNumber = model.JerseyNumber.ToString();
            _editPosition = model.Position ?? string.Empty;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
