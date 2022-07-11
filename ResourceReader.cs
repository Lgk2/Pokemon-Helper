using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    public class ResourceReader
    {
        private readonly string[] resourceNames;
        private readonly Assembly assembly;
        private readonly string executingFolder;

        public ResourceReader()
        {
            assembly = Assembly.GetExecutingAssembly();
            resourceNames = assembly.GetManifestResourceNames();

            string? tempExecutingFolder = Path.GetDirectoryName(AppContext.BaseDirectory);

            executingFolder = !string.IsNullOrEmpty(tempExecutingFolder) ? tempExecutingFolder : "";
        }

        public List<Pokemon> ReadExcel()
        {
            List<Pokemon> result = new();

            Stream? stream = assembly.GetManifestResourceStream(resourceNames.First(resource => resource.EndsWith("Pokemon Availability.xlsx")));

            if (stream == null)
                return new List<Pokemon>();

            var excel = new ExcelMapper(stream);
            var pokemonsEnum = excel.Fetch<PokemonExcel>(0);

            pokemonsEnum = pokemonsEnum.Where(poke => !string.IsNullOrEmpty(poke.Name) && poke.BeforeGymStr.Any(c => char.IsDigit(c))); //Remove empty pokemons
            //TODO maybe handle hidden fields

            var pokemonsList = pokemonsEnum.ToList();

            for (int i = 0; i < pokemonsList.Count; i++)
            {
                PokemonExcel pokemon = pokemonsList[i];
                if (pokemonsList[i].Name.EndsWith("oo"))
                    pokemonsList[i].Name = pokemonsList[i].Name[0..^1] + "-o";

                result.Add(new Pokemon()
                {
                    PokemonExcel = pokemon
                });
            }

            return result;
        }

        public void ReadPBS(List<Pokemon> Pokemons)
        {
            Stream? stream = assembly.GetManifestResourceStream(resourceNames.First(resource => resource.EndsWith("pokemon.txt")));

            if (stream == null)
                return;

            IEnumerable<Pokemon>? selectedPoke = null;
            PokemonStats pokemonStats = new();
            bool nextIsName = false;

            int lineNbr = 1;
            var streamReader = new StreamReader(stream);
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                lineNbr++;

                if (line == null)
                    continue;

                if (line[0] == '[' && line.Contains(']'))
                {
                    nextIsName = true;
                    pokemonStats = new();
                    continue;
                }

                string beforeEquals = line[..(line.IndexOf('='))];
                string value = line[(line.IndexOf('=') + 1)..];

                if (nextIsName && beforeEquals != "InternalName")
                {
                    pokemonStats.Name = value;

                    ReadImage(value, pokemonStats);

                    selectedPoke = Pokemons.Where(poke => poke.PokemonExcel != null && poke.PokemonExcel.Name == value);

                    foreach (var poke in selectedPoke)
                        poke.SetPokemonStats(pokemonStats);

                    continue;
                }
                else if (nextIsName)
                    pokemonStats.InternalName = value;


                if (line.Contains("Type") || line.Contains("BaseStats") || line.Contains("Abilities") || line.Contains("HiddenAbility") || line.Contains("Evolutions"))
                {
                    if (line.Contains("Type"))
                    {
                        string typeStr = value.ToLower();

                        if (string.IsNullOrEmpty(typeStr))
                        {
                            Console.WriteLine("Error at line: " + lineNbr);
                            continue;
                        }

                        typeStr = char.ToUpper(typeStr[0]) + typeStr[1..];

                        if (Enum.TryParse(typeStr, out PokemonType type) && !pokemonStats.PokemonTypes.Contains(type))
                            pokemonStats.AddType(type);
                        else
                            Console.WriteLine("Error reading type from " + line);
                    }
                    else if (line.Contains("BaseStats"))
                    {
                        int[] res = Array.ConvertAll(value.Split(","), s => int.Parse(s));
                        pokemonStats.SetStatNbrs(res);
                    }
                    else if (line.Contains("Abilities") || line.Contains("HiddenAbility"))
                    {
                        string[] abilities = value.Split(",");

                        for (int i = 0; i < abilities.Length; i++)
                            pokemonStats.AddAbility(abilities[i]);
                    }
                    else if (line.Contains("Evolutions"))
                    {
                        value = value.Replace("Level", "");

                        string[] evolutions = value.Split(',');
                        for (int i = 0; i < evolutions.Length; i++)
                        {
                            string evo = evolutions[i];

                            if (evo == "" || int.TryParse(evo, out int _))
                                continue;

                            evo = evo.Replace(",", "");
                            evo = evo.ToLower();
                            evo = char.ToUpper(evo[0]) + evo[1..];

                            if (Pokemons.Any(poke => poke.PokemonExcel != null && poke.PokemonExcel.Name == evo))
                                pokemonStats.Evolutions.Add(evo);
                        }
                    }
                }

                nextIsName = false;
            }
        }

        public List<Trainer> ReadTrainers(List<Pokemon> pokemonList)
        {
            Stream? stream = assembly.GetManifestResourceStream(resourceNames.First(resource => resource.EndsWith("trainers.txt")));

            if (stream == null)
                return new List<Trainer>();

            List<Move> moves = ReadMoves();
            List<Trainer> result = new();
            Trainer newTrainer = new();

            int lineNbr = 1;
            int pokemons = 0;

            var streamReader = new StreamReader(stream);
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();

                if (string.IsNullOrEmpty(line) || line[0] == '#')
                    continue;

                if (lineNbr == 2)
                    newTrainer.Name = line.Split(',')[0];

                else if (lineNbr == 3)
                    pokemons = int.Parse(line.Split(',')[0]);

                else if (lineNbr >= 4 && lineNbr != 4 + pokemons)
                {
                    TrainerPokemon newPokemon = new()
                    {
                        PokemonStats = new()
                    };

                    string[] pokemonAttributes = line.Split(',');

                    bool hasPhysicalMoves = false;
                    bool hasSpecialMoves = false;

                    for (int i = 0; i < pokemonAttributes.Length; i++)
                    {
                        string attribute = pokemonAttributes[i];

                        if (string.IsNullOrEmpty(attribute))
                            continue;

                        if (i == 0)
                        {
                            List<Pokemon> pokes = pokemonList.Where(poke => poke.PokemonStats != null && !string.IsNullOrEmpty(poke.PokemonStats.InternalName)).ToList();
                            Pokemon? foundPokemon = pokes.FirstOrDefault(poke => poke.PokemonStats != null && poke.PokemonStats.InternalName == attribute);

                            if (foundPokemon != null && foundPokemon.PokemonStats != null)
                            {
                                PokemonStats? foundPokemonStats = foundPokemon.PokemonStats;

                                if (foundPokemonStats != null)
                                {
                                    newPokemon.PokemonStats.Name = foundPokemonStats.Name;
                                    newPokemon.PokemonStats.Image = foundPokemonStats.Image;

                                    newPokemon.PokemonStats.SetStatNbrs(foundPokemonStats.StatNbrs);

                                    for (int y = 0; y < foundPokemonStats.PokemonTypes.Count; y++)
                                        newPokemon.PokemonStats.PokemonTypes.Add(foundPokemonStats.PokemonTypes[y]);
                                }
                            }
                        }

                        else if (i == 1)
                            newPokemon.Level = int.Parse(attribute);

                        else if (i == 2)
                            newPokemon.Item = attribute[0] + attribute[1..].ToLower();

                        else if (i is >= 3 and < 7)
                        {
                            if (newPokemon.Moves == null)
                                newPokemon.Moves = new();

                            Move? move = moves.FirstOrDefault(move => move.Name != null && move.Name.ToUpper() == attribute);
                            if (move != null)
                            {
                                if (move.MoveType == MoveType.Physical)
                                    hasPhysicalMoves = true;
                                else if (move.MoveType == MoveType.Special)
                                    hasSpecialMoves = true;

                                newPokemon.AddMove(move);
                            }
                        }

                        //TODO finish this
                        if (i == 7)
                        {
                            List<Pokemon> pokes = pokemonList.Where(poke => poke.PokemonStats != null && !string.IsNullOrEmpty(poke.PokemonStats.InternalName)).ToList();
                            Pokemon? foundPokemon = pokes.FirstOrDefault(poke => poke.PokemonStats != null && poke.PokemonStats.Name == newPokemon.PokemonStats.Name);

                            if (foundPokemon == null)
                                continue;

                            PokemonStats? foundPokeStats = foundPokemon.PokemonStats;
                            if (foundPokeStats != null && foundPokeStats.Abilities != null)
                            {
                                int abilityIndex = int.Parse(attribute);

                                if (abilityIndex >= foundPokeStats.Abilities.Count)
                                    Console.Error.WriteLine("Abilityindex of pokemon " + newPokemon.PokemonStats.Name + " is outside of index!");
                                
                                newPokemon.Ability = foundPokeStats.Abilities[Math.Min(abilityIndex, foundPokeStats.Abilities.Count - 1)];
                            }
                        }

                        //Gender

                        //int, unknown meaning

                        //bool, unknown meaning

                        else if (i == 11)
                        {
                            newPokemon.Nature = (NatureType)Enum.Parse(typeof(NatureType), attribute[0] + attribute[1..].ToLower());

                            if (newPokemon.PokemonStats != null)
                            {
                                if (hasPhysicalMoves && hasSpecialMoves)
                                    newPokemon.PokemonStats.SetPokemonDamageType(DamageType.Hybrid);
                                else if (hasPhysicalMoves)
                                    newPokemon.PokemonStats.SetPokemonDamageType(DamageType.Attack);
                                else
                                    newPokemon.PokemonStats.SetPokemonDamageType(DamageType.Special);
                            }

                            break;
                        }
                    }

                    if (newTrainer.TrainerPokemons == null)
                        newTrainer.TrainerPokemons = new();

                    newTrainer.TrainerPokemons.Add(newPokemon);
                }

                if (lineNbr == 3 + pokemons)
                {
                    result.Add(newTrainer);
                    newTrainer = new();
                    lineNbr = 0;
                }


                lineNbr++;
            }

            return result;
        }

        public List<Move> ReadMoves()
        {
            Stream? stream = assembly.GetManifestResourceStream(resourceNames.First(resource => resource.EndsWith("moves.txt")));

            if (stream == null)
                return new List<Move>();

            List<Move> result = new();
            var streamReader = new StreamReader(stream);
            while (!streamReader.EndOfStream)
            {
                string? line = streamReader.ReadLine();

                if (!string.IsNullOrEmpty(line))
                {
                    Move newMove = new();
                    string[] stats = line.Split(",");

                    if (stats.Length > 5 && stats[5].ToLower() == "qmarks")
                        continue;

                    newMove.Name = stats[1][0] + stats[1][1..].ToLower();
                    newMove.Damage = int.Parse(stats[4]);

                    newMove.Type = (PokemonType)Enum.Parse(typeof(PokemonType), char.ToUpper(stats[5][0]) + stats[5][1..].ToLower());
                    newMove.MoveType = (MoveType)Enum.Parse(typeof(MoveType), stats[6]);

                    newMove.Accuracy = int.Parse(stats[7]);

                    result.Add(newMove);
                }
            }

            return result;
        }

        private void ReadImage(string pokemonName, PokemonStats pokemonStats)
        {
            pokemonName = pokemonName.Replace("'", "");
            pokemonName = pokemonName.Replace(" ", "");
            pokemonName = pokemonName.Replace(".", "");

            //Handle these
            //MrMime
            //MimeJr
            //Type:Null
            //TapuKoko

            string? resourceName = resourceNames.FirstOrDefault(name => name.Contains(pokemonName.ToLower()) && name.EndsWith(".png"));

            if (!string.IsNullOrEmpty(resourceName))
            {
                string fixedResName = resourceName.Replace(".", @"\");
                fixedResName = fixedResName.Replace(@"\png", ".png");
                fixedResName = fixedResName.Replace(@"Pokemon_Helper\", "");

                string result = Path.Combine(executingFolder, fixedResName);

                if (File.Exists(result))
                    pokemonStats.Image = result;
                else
                    MessageBox.Show("Error, couldn't find " + result);
            }
            else
                MessageBox.Show("Error, couldn't find resource containing " + pokemonName + ".png");
        }
    }
}
