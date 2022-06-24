using System.Collections.Generic;
using System.Linq;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    public class Pokemon
    {
        public PokemonExcel PokemonExcel 
        { 
            get { return pokemonExcel; } 
            set 
            {
                pokemonExcel = value;
                BeforeGym = int.Parse(PokemonExcel.BeforeGymStr.Split()[0]);
            } 
        }

        public PokemonStats? PokemonStats { get; set; }
        public bool Hidden { get; set; } = false;
        public int BeforeGym { get; set; }

        private PokemonExcel pokemonExcel = new();

        public void SetPokemonExcel(PokemonExcel pokemonExcel)
        {
            PokemonExcel = pokemonExcel;
        }

        public void SetPokemonStats(PokemonStats pokemonStats)
        {
            PokemonStats = pokemonStats;
        }
    }
}
