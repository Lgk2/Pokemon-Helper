using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    public class Move
    {
        public string? Name { get; set; }
        public PokemonType Type { get; set; }
        public MoveType MoveType { get; set; }
        public int Damage { get; set; }
        public int Accuracy { get; set; }
    }
}
