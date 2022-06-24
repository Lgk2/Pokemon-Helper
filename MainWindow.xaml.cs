using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Pokemon> Pokemons = new();

        private readonly List<string> acceptableTypes = new();
        private readonly List<string> excludedTypes = new();


        private readonly PokemonView pokemonView = new();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = pokemonView;

            if (!Directory.Exists(Save.SaveDir))
            {
                ReadExcelAndPbs();
            }
            else
            {
                try
                {
                    Save save = new();
                    Pokemons = save.LoadPokemons();
                    UpdateMatchingPokemon();
                }
                catch (Exception)
                {
                    ReadExcelAndPbs();
                }
            }

            SetStyles();
        }

        private void SetStyles()
        {
            SolidColorBrush backgroundColor = new(new()
            {
                R = 36,
                G = 37,
                B = 38,
                A = 255
            });

            Background = backgroundColor;
            PokemonListView.Background = backgroundColor;
            PokemonListView.Foreground = Brushes.Coral;
        }

        private enum DamageType
        {
            Attack,
            Special,
            Hybrid
        }

        private void ReadExcelAndPbs()
        {
            ResourceReader resourceReader = new();
            Pokemons = resourceReader.ReadExcel();
            resourceReader.ReadPBS(Pokemons);
            for (int i = 0; i < Pokemons.Count; i++)
            {
                PokemonStats? stats = Pokemons[i].PokemonStats;
                if (stats != null && stats.HighestEvolTBS == 0)
                    stats.SetHighestEvolStats(Pokemons);
            }
            UpdateMatchingPokemon();
        }

        private static DamageType GetPokemonType(Pokemon poke)
        {
            PokemonStats? stats = poke.PokemonStats;

            if (stats == null)
                return DamageType.Hybrid;

            int[] statNbrs = stats.StatNbrs;

            if (statNbrs[1] > statNbrs[3] + 10)
                return DamageType.Attack;
            else if (statNbrs[3] > statNbrs[1] + 10)
                return DamageType.Special;

            return DamageType.Hybrid;
        }

        private void MyUpDownControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object>? e)
        {
            UpdateMatchingPokemon();
        }

        private void UpdateMatchingPokemon()
        {
            if (MatchingPkmnNbr == null)
                return;

            if (!int.TryParse(beforeGymCtrl.Text, out int beforeGymInt))
                beforeGymInt = 5;

            if (!int.TryParse(MinimumBaseStats.Text, out int minBaseStatsInt))
                minBaseStatsInt = 500;

            IEnumerable<Pokemon> matchingEnum = OnlyCurrentGym.IsChecked == false
                ? Pokemons.Where(poke => beforeGymInt >= poke.BeforeGym)
                : Pokemons.Where(poke => beforeGymInt == poke.BeforeGym);

            matchingEnum = matchingEnum.Where(poke => poke.PokemonStats != null && poke.PokemonStats.MaxEvolutionStatNbrs(Pokemons).Sum() >= minBaseStatsInt); //Base stats

            if (AttackTypeBox.SelectedIndex == 1)
                matchingEnum = matchingEnum.Where(poke => GetPokemonType(poke) == DamageType.Attack);
            else if (AttackTypeBox.SelectedIndex == 2)
                matchingEnum = matchingEnum.Where(poke => GetPokemonType(poke) == DamageType.Special);
            else if (AttackTypeBox.SelectedIndex == 3)
                matchingEnum = matchingEnum.Where(poke => GetPokemonType(poke) == DamageType.Hybrid);

            if (ShowPassword.IsChecked == false)
                matchingEnum = matchingEnum.Where(poke => !poke.PokemonExcel.Notes.ToLower().Contains("password"));

            if (ShowEgg.IsChecked == false)
                matchingEnum = matchingEnum.Where(poke => !poke.PokemonExcel.Notes.ToLower().Contains("egg"));

            if (ShowStarters.IsChecked == false)
                matchingEnum = matchingEnum.Where(poke => !((poke.PokemonExcel.Location == "Grand Hall" && string.IsNullOrEmpty(poke.PokemonExcel.Notes)) || poke.PokemonExcel.Notes == "A")); //Remove Starters

            if (TypeCount.SelectedIndex == 1)
                matchingEnum = matchingEnum.Where(poke => poke.PokemonStats != null && poke.PokemonStats.Types.Count == 1);

            if (TypeCount.SelectedIndex == 2)
                matchingEnum = matchingEnum.Where(poke => poke.PokemonStats != null && poke.PokemonStats.Types.Count > 1);

            if (ShowHidden.IsChecked == false)
                matchingEnum = matchingEnum.Where(poke => !poke.Hidden);

            string searchTxt = SearchBox.Text.ToLower();
            if (!string.IsNullOrEmpty(searchTxt))
                matchingEnum = matchingEnum.Where(poke => poke.PokemonExcel.Name.ToLower().Contains(searchTxt));

            List<Pokemon> matchingPokemon = matchingEnum.Any() ? matchingEnum.ToList() : (new());
            if (acceptableTypes.Count > 0 || excludedTypes.Count > 0)
            {
                for (int i = matchingPokemon.Count - 1; i >= 0; i--)
                {
                    Pokemon poke = matchingPokemon[i];

                    if (poke.PokemonStats == null) continue;

                    List<PokemonTypes> types = poke.PokemonStats.Types;
                    bool matching = acceptableTypes.Count == 0;

                    for (int y = 0; y < types.Count; y++)
                    {
                        string typeStr = types[y].ToString();

                        if (acceptableTypes.Contains(typeStr))
                            matching = true;

                        if (excludedTypes.Contains(typeStr))
                        {
                            matching = false;
                            break;
                        }
                    }
                        

                    if (!matching)
                        matchingPokemon.Remove(poke);
                }
            }

            MatchingPkmnNbr.Content = matchingPokemon.Count;
            pokemonView.PokemonList = matchingPokemon;
        }

        private void AttackTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMatchingPokemon();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Save save = new(Pokemons);
            save.SavePokemons();
        }

        private void BeforeGymCtrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMatchingPokemon();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMatchingPokemon();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeNotesColumn();
        }

        private void ResizeNotesColumn()
        {
            var columns = PokemonListView.Columns;
            DataGridColumn noteColumn = columns.First(col => (string)col.Header == "Notes");
            noteColumn.Width = IdealSize();
        }

        private double IdealSize()
        {
            double gridWidth = PokemonListView.ActualWidth;

            var columns = PokemonListView.Columns;

            double totalWidth = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                if ((string)columns[i].Header != "Notes")
                    totalWidth += columns[i].ActualWidth;
            }

            return gridWidth - totalWidth - 25;
        }

        #region Clicks
        private void Button_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;

            if (e.ClickCount == 2)
                Type_MouseDoubleClick(btn);
            else
                TypeButton_Click(btn);
        }

        private void TypeButton_Click(Button clickedBtn)
        {
            string content = ((Run)((Bold)clickedBtn.Content).Inlines.First()).Text;

            if (!acceptableTypes.Contains(content) && !excludedTypes.Contains(content))
            {
                clickedBtn.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                acceptableTypes.Add(content);
            }
            else
            {
                clickedBtn.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
                acceptableTypes.Remove(content);
                excludedTypes.Remove(content);
            }
            UpdateMatchingPokemon();
        }

        private void Type_MouseDoubleClick(Button clickedBtn)
        {
            string content = ((Run)((Bold)clickedBtn.Content).Inlines.First()).Text;

            if (!excludedTypes.Contains(content))
            {
                clickedBtn.Background = Brushes.Red;
                excludedTypes.Add(content);
                acceptableTypes.Remove(content);
            }

            UpdateMatchingPokemon();
        }

        private void PokemonListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Pokemon poke = (Pokemon)PokemonListView.SelectedItem;

            if (poke == null)
                return;

            poke.Hidden = !poke.Hidden;
            UpdateMatchingPokemon();
        }

        private void OnlyCurrentGym_Click(object sender, RoutedEventArgs e)
        {
            GymLabelStr.Content = OnlyCurrentGym.IsChecked == true ? "Gym" : "Before Gym";
            UpdateMatchingPokemon();
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            Pokemons = new();
            ReadExcelAndPbs();
        }

        private void DualTypesOnly_Click(object sender, RoutedEventArgs e)
        {
            UpdateMatchingPokemon();
        }

        private void HighestEvolStats_Click(object sender, RoutedEventArgs e)
        {
            if (HighestEvolStats.IsChecked == false)
                TotalColumn.Binding = new Binding("PokemonStats.TotalBaseStats");
            else if (HighestEvolStats.IsChecked == true)
                TotalColumn.Binding = new Binding("PokemonStats.HighestEvolTBS");

            UpdateMatchingPokemon();
        }

        private void UpdateMatchingPokemon_Click(object sender, RoutedEventArgs e)
        {
            UpdateMatchingPokemon();
        }
        #endregion

    }
}
