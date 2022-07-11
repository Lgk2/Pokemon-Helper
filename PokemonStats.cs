using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    public class PokemonStats
    {
        public string? Name { get; set; }
        public string? InternalName { get; set; }

        public List<PokemonType> PokemonTypes { get; set; } = new();
        public List<Move> Moves { get; set; } = new();

        public List<string> Abilities { get; set; } = new();

        public List<string> Evolutions { get; set; } = new();

        public int TotalBaseStats { get; set; }
        public int TotalRelevantBaseStats { get; set; }

        public int HighestEvolTBS { get; set; }
        public int HighestEvolRelevantBaseStats { get; set; }

        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
        public string? Image { get; set; }

        public int[] StatNbrs { get; set; } = new int[] { 0, 0, 0, 0, 0 };
        public int[] HighestEvolStatNbrs { get; set; } = new int[] { 0, 0, 0, 0, 0 };

        public string TypesString { get; set; } = "";

        private TrainerPokemon? comparisonPokemon;

        public DamageType? DamageType { get; private set; }
        public DamageType? HighestEvolDmgType { get; private set; }

        [JsonIgnore]
        public TrainerPokemon? ComparisonPokemon
        {
            get { return comparisonPokemon; }
            set
            {
                comparisonPokemon = value;
                SetRelevantStats();
            }
        }


        public void AddType(PokemonType type)
        {
            PokemonTypes.Add(type);

            if (TypesString != "")
                TypesString += " ";

            TypesString += type.ToString();
        }

        public void SetStatNbrs(int[] statNbrs)
        {
            StatNbrs = statNbrs;
            TotalBaseStats = statNbrs.Sum();

            UpdateStats(false);

            SetPokemonDamageType(statNbrs, false);
        }

        public void SetHighestEvolStats(List<Pokemon> pokemons)
        {
            HighestEvolStatNbrs = MaxEvolutionStatNbrs(pokemons);
            HighestEvolTBS = HighestEvolStatNbrs.Sum();

            SetPokemonDamageType(HighestEvolStatNbrs, true);
        }

        private void SetPokemonDamageType(int[] statNbrs, bool highestEvol)
        {
            DamageType res;

            if (statNbrs[1] > statNbrs[4] + 20)
                res = Types.DamageType.Attack;
            else if (statNbrs[4] > statNbrs[1] + 20)
                res = Types.DamageType.Special;
            else
                res = Types.DamageType.Hybrid;

            if (!highestEvol)
                DamageType = res;
            else
                HighestEvolDmgType = res;
        }

        public void SetPokemonDamageType(DamageType damageType)
        {
            DamageType = damageType;
        }

        public void UpdateStats(bool evolution)
        {
            int[] statArray = evolution ? HighestEvolStatNbrs : StatNbrs;

            Health = statArray[0];
            Attack = statArray[1];
            Defense = statArray[2];
            Speed = statArray[3];
            SpecialAttack = statArray[4];
            SpecialDefense = statArray[5];
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

        public void SetRelevantStats()
        {
            TotalRelevantBaseStats = TotalBaseStats;
            HighestEvolRelevantBaseStats = HighestEvolTBS;

            if (ComparisonPokemon != null && ComparisonPokemon.PokemonStats != null)
            {
                if (Speed < ComparisonPokemon.PokemonStats.StatNbrs[3])
                {
                    TotalRelevantBaseStats -= StatNbrs[3];
                    HighestEvolRelevantBaseStats = HighestEvolTBS - HighestEvolStatNbrs[3];
                }
                else if (Speed >= ComparisonPokemon.PokemonStats.StatNbrs[3])
                {
                    int unnecessarySpeedUnEvol = Math.Max(StatNbrs[3] - ComparisonPokemon.PokemonStats.Speed - 10, 0);
                    int unnecessarySpeed = Math.Max(HighestEvolStatNbrs[3] - ComparisonPokemon.PokemonStats.Speed - 10, 0);

                    TotalRelevantBaseStats -= unnecessarySpeedUnEvol;
                    HighestEvolRelevantBaseStats -= unnecessarySpeed;
                }

                if (ComparisonPokemon.Moves == null)
                    return;

                if (ComparisonPokemon.PokemonStats.DamageType == Types.DamageType.Attack)
                {
                    TotalRelevantBaseStats -= StatNbrs[5];
                    HighestEvolRelevantBaseStats -= HighestEvolStatNbrs[5];
                }
                else if (ComparisonPokemon.PokemonStats.DamageType == Types.DamageType.Special)
                {
                    TotalRelevantBaseStats -= StatNbrs[2];
                    HighestEvolRelevantBaseStats -= HighestEvolStatNbrs[2];
                }

                //TODO take account into comparisonpokemon tank stats, for example don't recommend a special attacker against blissey
            }

            if (DamageType.HasValue)
            {
                if (DamageType.Value == Types.DamageType.Attack)
                    TotalRelevantBaseStats -= StatNbrs[4];
                else if (DamageType.Value == Types.DamageType.Special)
                    TotalRelevantBaseStats -= StatNbrs[1];
                else
                    TotalRelevantBaseStats -= (StatNbrs[1] + StatNbrs[4]) / 2;
            }

            if (HighestEvolDmgType.HasValue)
            {
                if (HighestEvolDmgType.Value == Types.DamageType.Attack)
                    HighestEvolRelevantBaseStats -= StatNbrs[4];
                else if (HighestEvolDmgType.Value == Types.DamageType.Special)
                    HighestEvolRelevantBaseStats -= StatNbrs[1];
                else
                    HighestEvolRelevantBaseStats -= (StatNbrs[1] + StatNbrs[4]) / 2;
            }
        }

        public void AddAbility(string ability)
        {
            Abilities.Add(char.ToUpper(ability[0]) + ability[1..].ToLower());
        }
    }
}
