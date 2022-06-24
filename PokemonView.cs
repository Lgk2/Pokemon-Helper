using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pokemon_Helper
{
    public class PokemonView : INotifyPropertyChanged
    {
        private List<Pokemon> pokemonList = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Pokemon> PokemonList 
        { 
            get { return pokemonList; } 
            set
            {
                pokemonList = value;
                OnPropertyChanged(nameof(PokemonList));
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
