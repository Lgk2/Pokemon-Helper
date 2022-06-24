using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    public class ResourceReader
    {
        private readonly string[] resourceNames;
        private readonly Assembly assembly;

        public ResourceReader()
        {
            assembly = Assembly.GetExecutingAssembly();

            resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        }

        public List<Pokemon> ReadExcel()
        {
            List<Pokemon> result = new();

            Stream? stream = assembly.GetManifestResourceStream(resourceNames[1]);

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
            Stream? stream = assembly.GetManifestResourceStream(resourceNames[2]);

            if (stream == null)
                return;

            PokemonStats pokemonStats = new();
            IEnumerable<Pokemon>? selectedPoke = null;
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

                string value = line[(line.IndexOf('=') + 1)..];

                if (nextIsName)
                {
                    pokemonStats.Name = value;
                    selectedPoke = Pokemons.Where(poke => poke.PokemonExcel.Name == value);

                    foreach (var poke in selectedPoke)
                        poke.SetPokemonStats(pokemonStats);
                }

                if (line.Contains("Type") || line.Contains("BaseStats") || line.Contains("Evolutions"))
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

                        if (Enum.TryParse(typeStr, out PokemonTypes type) && !pokemonStats.Types.Contains(type))
                            pokemonStats.AddType(type);
                        else
                            Console.WriteLine("Error reading type from " + line);
                    }
                    else if (line.Contains("BaseStats"))
                    {
                        int[] res = Array.ConvertAll(value.Split(","), s => int.Parse(s));
                        pokemonStats.SetStatNbrs(res);
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

                            if (Pokemons.Any(poke => poke.PokemonExcel.Name == evo))
                                pokemonStats.Evolutions.Add(evo);
                        }
                    }

                }

                nextIsName = false;
            }
        }
    }
}
