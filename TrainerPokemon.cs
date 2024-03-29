﻿using System.Collections.Generic;
using System.Text;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    public class TrainerPokemon
    {
        public PokemonStats? PokemonStats { get; set; }

        public int Level { get; set; }
        public string Item { get; set; } = "None";
        public string? Ability { get; set; }

        public List<Move>? Moves { get; set; }
        public string? MovesStr { get; private set; }

        public NatureType Nature { get; set; }

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

            StringBuilder movesStrBuilder = new(Moves[0].Name + " ");

            for (int i = 1; i < Moves.Count; i++)
            {
                if (i != 1)
                    movesStrBuilder.Append(Moves[i].Name + " ");
                else
                    movesStrBuilder.AppendLine(Moves[i].Name);
            }

            MovesStr = movesStrBuilder.ToString();
        }
    }
}
