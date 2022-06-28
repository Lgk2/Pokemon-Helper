using Pokemon_Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon_Helper
{
    public class Types
    {
        private readonly Dictionary<PokemonType, TypeEffectiveness[]> OffensiveTypeMatchups = new() {
            { PokemonType.Normal, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 0 } } },
            { PokemonType.Fire, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 0.5 } } },
            { PokemonType.Water, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 0.5 } } },
            { PokemonType.Electric, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 0 } } },
            { PokemonType.Grass, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 0.5 } } },
            { PokemonType.Ice, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 2.0}, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 0.5 } } },
            { PokemonType.Fighting, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Normal, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 0.5 } } },
            { PokemonType.Poison, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 0.5 } } },
            { PokemonType.Ground, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 0 } } },
            { PokemonType.Flying, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 0.5 } } },
            { PokemonType.Psychic, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 0 } } },
            { PokemonType.Bug, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 0.5 } } },
            { PokemonType.Rock, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 } } },
            { PokemonType.Ghost, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Normal, Effectiveness = 0 } } },
            { PokemonType.Dragon, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 0 } } },
            { PokemonType.Dark, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 0.5 } } },
            { PokemonType.Steel, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 0.5 } } },
            { PokemonType.Fairy, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 } } }
        };

        private readonly Dictionary<PokemonType, TypeEffectiveness[]> DefensiveTypeMatchups = new() {
            { PokemonType.Normal, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 0 } } },
            { PokemonType.Fire, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 0.5 } } },
            { PokemonType.Water, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 } } },
            { PokemonType.Electric, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 } } },
            { PokemonType.Grass, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 2.0 } } },
            { PokemonType.Ice, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 2.0 } } },
            { PokemonType.Fighting, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 2.0 } } },
            { PokemonType.Poison, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 0.5 } } },
            { PokemonType.Ground, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 0.5 } } },
            { PokemonType.Flying, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 } } },
            { PokemonType.Psychic, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 2.0 } } },
            { PokemonType.Bug, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 2.0 } } },
            { PokemonType.Rock, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Normal, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 2.0 } } },
            { PokemonType.Ghost, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Normal, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 2.0 } } },
            { PokemonType.Dragon, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Water, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Electric, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 2.0 } } },
            { PokemonType.Dark, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Ghost, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 2.0 } } },
            { PokemonType.Steel, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Normal, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fire, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Grass, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ice, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Ground, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Flying, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Psychic, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Rock, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Fairy, Effectiveness = 0.5 } } },
            { PokemonType.Fairy, new TypeEffectiveness[] { new TypeEffectiveness() { Type = PokemonType.Fighting, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Poison, Effectiveness = 2.0 }, new TypeEffectiveness() { Type = PokemonType.Bug, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dragon, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Dark, Effectiveness = 0.5 }, new TypeEffectiveness() { Type = PokemonType.Steel, Effectiveness = 2.0 } } }
        };

        public enum PokemonType
        {
            Normal,
            Fire,
            Water,
            Electric,
            Grass,
            Ice,
            Fighting,
            Poison,
            Ground,
            Flying,
            Psychic,
            Bug,
            Rock,
            Ghost,
            Dragon,
            Dark,
            Steel,
            Fairy
        }

        public List<TypeEffectiveness> GetOffensiveMatchups(List<PokemonType> inputTypes)
        {
            OffensiveTypeMatchups.TryGetValue(inputTypes[0], out TypeEffectiveness[]? initialStrengthsAndWeaknesses);

            if (initialStrengthsAndWeaknesses == null)
                return new List<TypeEffectiveness>();

            List<TypeEffectiveness> result = new();
            result.AddRange(initialStrengthsAndWeaknesses);

            for (int i = 1; i < inputTypes.Count; i++)
            {
                OffensiveTypeMatchups.TryGetValue(inputTypes[i], out TypeEffectiveness[]? strengthsAndWeaknesses);

                if (strengthsAndWeaknesses == null)
                    break;

                for (int y = 0; y < strengthsAndWeaknesses.Length; y++)
                {
                    TypeEffectiveness selectedStrengthAndWeakness = strengthsAndWeaknesses[y];

                    TypeEffectiveness? containedType = result.FirstOrDefault(typeEff => typeEff.Type == selectedStrengthAndWeakness.Type);

                    if (containedType != null)
                    {
                        containedType.Effectiveness = Math.Max(selectedStrengthAndWeakness.Effectiveness, containedType.Effectiveness);
                    }
                    //Don't add weaknesses that aren't already in the results because not being in the results represents being neutral (1.0) which is higher than 0.5
                    else if (selectedStrengthAndWeakness.Effectiveness > 1) 
                    {
                        TypeEffectiveness typeEffectiveness = new()
                        {
                            Type = selectedStrengthAndWeakness.Type,
                            Effectiveness = selectedStrengthAndWeakness.Effectiveness
                        };

                        result.Add(typeEffectiveness);
                    }
                }

                List<TypeEffectiveness> weaknesses = result.Where(eff => eff.Effectiveness < 1).ToList();
                for (int weaknessIndex = 0; weaknessIndex < weaknesses.Count; weaknessIndex++)
                {
                    TypeEffectiveness weakness = weaknesses[weaknessIndex];

                    if (!strengthsAndWeaknesses.Where(eff => eff.Effectiveness < 1).Contains(weakness))
                        result.Remove(weakness);
                }
            }

            return result;
        }

        public List<TypeEffectiveness> GetDefensiveMatchups(List<PokemonType> inputTypes)
        {
            List<TypeEffectiveness> result = new();

            for (int i = 0; i < inputTypes.Count; i++)
            {
                DefensiveTypeMatchups.TryGetValue(inputTypes[i], out TypeEffectiveness[]? res);

                if (res != null)
                {
                    for (int y = 0; y < res.Length; y++)
                    {
                        TypeEffectiveness effectiveness = res[y];

                        TypeEffectiveness? containedType = result.FirstOrDefault(typeEff => typeEff.Type == effectiveness.Type);

                        if (containedType != null)
                            containedType.Effectiveness *= effectiveness.Effectiveness;
                        else
                        {
                            TypeEffectiveness typeEffectiveness = new()
                            {
                                Type = effectiveness.Type,
                                Effectiveness = effectiveness.Effectiveness
                            };

                            result.Add(typeEffectiveness);
                        }
                    }

                }
            }

            return result;
        }

        public enum DamageType
        {
            Attack,
            Special,
            Hybrid
        }

        public enum MoveType
        {
            Physical,
            Special,
            Status
        }

        public enum NatureType
        {
            Hardy,
            Lonely,
            Adamant,
            Naughty,
            Brave,
            Bold,
            Docile,
            Impish,
            Lax,
            Relaxed,
            Modest,
            Mild,
            Bashful,
            Rash,
            Quiet,
            Calm,
            Gentle,
            Careful,
            Quirky,
            Sassy,
            Timid,
            Hasty,
            Jolly,
            Naive,
            Serious
        }
    }
}
