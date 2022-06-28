using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pokemon_Helper
{
    public class PokemonView : INotifyPropertyChanged
    {
        private List<Pokemon>? visiblePokemons;
        private List<Trainer>? visibleTrainers;
        private Trainer? selectedTrainer;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Pokemon>? PokemonList
        {
            get { return visiblePokemons; }
            set
            {
                visiblePokemons = value;
                OnPropertyChanged(nameof(PokemonList));
            }
        }

        public List<Trainer>? TrainerList
        {
            get { return visibleTrainers; }
            set
            {
                visibleTrainers = value;
                OnPropertyChanged(nameof(TrainerList));
            }
        }

        public Trainer? SelectedTrainer
        {
            get { return selectedTrainer; }
            set
            {
                selectedTrainer = value;
                OnPropertyChanged(nameof(selectedTrainer));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}
