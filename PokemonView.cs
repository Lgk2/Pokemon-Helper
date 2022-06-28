using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pokemon_Helper
{
    public class PokemonView : INotifyPropertyChanged
    {
        private List<Pokemon>? pokemonList;
        private List<Trainer>? trainerList;
        private Trainer? selectedTrainer;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Pokemon>? PokemonList
        {
            get { return pokemonList; }
            set
            {
                pokemonList = value;
                OnPropertyChanged(nameof(PokemonList));
            }
        }

        public List<Trainer>? TrainerList
        {
            get { return trainerList; }
            set
            {
                trainerList = value;
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
