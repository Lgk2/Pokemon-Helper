﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Pokemon_Helper
{
    public class Save
    {
        public static readonly string SaveDir = "Saves";
        public static readonly string FileExtension = ".json";

        public static readonly string ProgressFile = "Settings.txt";

        private List<Pokemon> pokemons = new();

        public Save() { }

        public Save(List<Pokemon> pokemons)
        {
            this.pokemons = pokemons;
        }

        public void SavePokemons()
        {
            DirectoryInfo directoryInfo = new(SaveDir);

            if (!directoryInfo.Exists)
                Directory.CreateDirectory(SaveDir);
            else
                foreach (var file in directoryInfo.GetFiles())
                    file.Delete();


            for (int i = 0; i < pokemons.Count; i++)
            {
                Pokemon poke = pokemons[i];

                if (poke.PokemonExcel == null)
                    continue;

                string jsonString = JsonSerializer.Serialize(poke);
                string fileName = Path.Combine(SaveDir, poke.PokemonExcel.Name);
                string number = "";

                if (File.Exists(fileName + FileExtension))
                {
                    int nbr = 2;

                    while (File.Exists(fileName + nbr + FileExtension))
                        nbr++;

                    number = nbr.ToString();
                }

                File.WriteAllText(fileName + number + FileExtension, jsonString);
            }
        }

        public List<Pokemon> LoadPokemons()
        {
            pokemons = new();

            if (Directory.Exists(SaveDir))
            {
                string[] files = Directory.GetFiles(SaveDir);
                for (int i = 0; i < files.Length; i++)
                {
                    string JsonString = File.ReadAllText(files[i]);
                    Pokemon? poke = JsonSerializer.Deserialize<Pokemon>(JsonString);

                    if (poke != null)
                    {
                        pokemons.Add(poke);

                        if (poke.PokemonStats != null && poke.PokemonStats.TotalBaseStats == 0)
                        {
                            if (poke.PokemonExcel != null)
                                Console.WriteLine("Error reading stats from pokemon " + poke.PokemonExcel.Name);
                            else
                                Console.WriteLine("Error reading pokemonexcel from pokemon " + poke.PokemonStats.Name);
                        }
                    }
                }
            }

            return pokemons;
        }

        public static void SaveSettings(List<string> settings)
        {
            File.WriteAllText(ProgressFile, SettingsString('\n', settings));
        }

        public static string SettingsString(char separator, List<string> strings)
        {
            StringBuilder builder = new();

            for (int i = 0; i < strings.Count; i++)
            {
                if (i <= strings.Count - 1)
                    builder.Append(strings[i] + separator);
                else
                    builder.Append(strings[i]);
            }

            return builder.ToString();
        }

        public static string[] LoadSettings()
        {
            return !File.Exists(ProgressFile) ? Array.Empty<string>() : File.ReadAllText(ProgressFile).Split('\n');
        }
    }
}
