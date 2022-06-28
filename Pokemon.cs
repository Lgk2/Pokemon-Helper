using System.Collections.Generic;
using System.Text;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    public class Pokemon
    {
        public PokemonExcel? PokemonExcel
        {
            get { return pokemonExcel; }
            set
            {
                pokemonExcel = value;

                if (PokemonExcel != null && PokemonExcel.BeforeGymStr != "")
                    BeforeGym = int.Parse(PokemonExcel.BeforeGymStr.Split()[0]);
            }
        }

        public PokemonStats? PokemonStats { get; set; }
        public bool Hidden { get; set; } = false;
        public int BeforeGym { get; set; }

        private PokemonExcel? pokemonExcel;

        public int Level { get; set; }
        public string? Item { get; set; }
        
        public List<Move>? Moves { get; set; }
        public string? MovesStr { get; private set; }

        public NatureType Nature { get; set; }

        public void SetPokemonExcel(PokemonExcel pokemonExcel)
        {
            PokemonExcel = pokemonExcel;
        }

        public void SetPokemonStats(PokemonStats pokemonStats)
        {
            PokemonStats = pokemonStats;
        }

        public void AddMove(Move newMove)
        {
            if (Moves == null)
                return;

            Moves.Add(newMove);

            string[] moveNames = new string[Moves.Count];
            for (int i = 0; i < moveNames.Length; i++)
            {
                string? name = Moves[i].Name;

                if (string.IsNullOrEmpty(name))
                    return;

                StringBuilder res = new();
                res.Append(name);

                if (i != moveNames.Length - 1)
                    res.Append(' ');

                moveNames[i] = res.ToString();
            }

            MovesStr = string.Join(' ', moveNames);
        }
    }
}
