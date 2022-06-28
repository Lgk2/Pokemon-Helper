using System.Collections.Generic;
using System.Linq;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    public class PokemonStats
    {
        public string? Name { get; set; }
        public string? InternalName { get; set; }

        public List<PokemonType> Types { get; set; } = new();
        public List<Move> Moves { get; set; } = new();
        public List<string> Evolutions { get; set; } = new();

        public int TotalBaseStats { get; set; }
        public int HighestEvolTBS { get; set; }

        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
        public string? Image { get; set; }


        public int[] StatNbrs { get; set; } = new int[] { 0, 0, 0, 0, 0 };

        public string TypesString { get; set; } = "";

        public void AddType(PokemonType type)
        {
            Types.Add(type);

            if (TypesString != "")
                TypesString += " ";

            TypesString += type.ToString();
        }

        public void SetStatNbrs(int[] statNbrs)
        {
            StatNbrs = statNbrs;

            TotalBaseStats = statNbrs.Sum();

            Health = statNbrs[0];
            Attack = StatNbrs[1];
            Defense = StatNbrs[2];
            Speed = StatNbrs[3];
            SpecialAttack = StatNbrs[4];
            SpecialDefense = StatNbrs[5];
        }

        public void SetHighestEvolStats(List<Pokemon> pokemons)
        {
            HighestEvolTBS = MaxEvolutionStatNbrs(pokemons).Sum();
        }

        public int[] MaxEvolutionStatNbrs(List<Pokemon> pokemons)
        {
            if (Evolutions.Count != 0)
            {
                Pokemon? pokemon = pokemons.First(poke => poke.PokemonExcel != null && poke.PokemonExcel.Name == Evolutions[0]);

                if (pokemon != null && pokemon.PokemonStats != null)
                    return pokemon.PokemonStats.MaxEvolutionStatNbrs(pokemons);
            }

            return StatNbrs;
        }
    }
}
