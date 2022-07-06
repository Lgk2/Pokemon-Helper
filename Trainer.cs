using System.Collections.Generic;

namespace Pokemon_Helper
{
    public class Trainer
    {
        public List<TrainerPokemon>? TrainerPokemons { get; set; }
        public string? Name { get; set; }
        public bool Hidden { get; set; } = false;
    }
}
