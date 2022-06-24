using Ganss.Excel;

namespace Pokemon_Helper
{
    public class PokemonExcel
    {
        [Column("Pokemon")]
        public string Name { get; set; } = "";

        [Column("Before Gym")]
        public string BeforeGymStr { get; set; } = "";

        [Column("Location")]
        public string Location { get; set; } = "";

        [Column("Notes")]
        public string Notes { get; set; } = "";
    }
}
