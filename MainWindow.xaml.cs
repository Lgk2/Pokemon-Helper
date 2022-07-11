using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static Pokemon_Helper.Types;

namespace Pokemon_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Pokemon> Pokemons = new();
        private List<Trainer> Trainers = new();

        private readonly List<string> includedTypes = new();
        private readonly List<string> excludedTypes = new();

        private readonly PokemonView pokemonView = new();

        private readonly SolidColorBrush defaultBtnColor = new(Color.FromRgb(180, 180, 180));
        private readonly SolidColorBrush TwoXEffectiveness = new(Color.FromRgb(0, 128, 0)); //This color is also the color chosen when user manually picks

        private readonly SolidColorBrush TwoXWeak = new(Color.FromRgb(128, 0, 0));
        private readonly SolidColorBrush userExcludedType = new(Color.FromRgb(255, 0, 0));

        private readonly SolidColorBrush FourXEffectiveness = new(Color.FromRgb(0, 255, 0));
        private readonly SolidColorBrush TwoXResistant = new(Color.FromRgb(0, 0, 128)); //Only takes half damage
        private readonly SolidColorBrush FourXResistant = new(Color.FromRgb(0, 0, 192)); //Only takes half damage
        private readonly SolidColorBrush Immune = new(Color.FromRgb(255, 255, 255));

        private string pokemonSearchTxt = "";
        private string trainerSearchTxt = "";

        //TODO remove hardcoding
        private readonly int[] levelcaps = new int[] { 20, 25, 35, 40, 45, 50, 55, 60, 65, 70, 70, 75, 75, 80, 85, 90, 90, 95, 150 }; //Hardcoded from Reborn/SystemConstants.rb'

        private readonly CheckBox[] checkBoxes;

        public MainWindow()
        {
            InitializeComponent();
            checkBoxes = new[] { HighestEvolStats, ShowHidden, ShowPassword, ShowEgg, ShowStarters, OnlyCurrentGym, OnlyCurrentTrainers, OnlyCountRelevantStats, SearchForTrainers };

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
                }
                catch (Exception)
                {
                    ReadExcelAndPbs();
                }
            }
            SetStyles();

            ReadTrainers();

            try
            {
                string[] settings = Save.LoadSettings();

                if (settings.Length != 0)
                {
                    beforeGymCtrl.Text = settings[0];
                    AttackTypeBox.SelectedIndex = int.Parse(settings[1]);
                    MinimumBaseStats.Text = settings[2];
                    pokemonSearchTxt = settings[3];
                    trainerSearchTxt = settings[4];
                    TypeCount.SelectedIndex = int.Parse(settings[5]);
                    TrainerList.SelectedIndex = int.Parse(settings[6]);

                    if (settings.Length > 8)
                        for (int i = 0; i < checkBoxes.Length; i++)
                        {
                            CheckBox cb = checkBoxes[i];
                            cb.IsChecked = bool.Parse(settings[7 + i]);
                        }
                }
            }
            catch
            {
                MessageBox.Show("Failed to load your settings, hopefully it works next time!");
            }

            UpdateMatchingPokemon();
            UpdateMatchingTrainers();

            SearchForTrainers_Click(this, new RoutedEventArgs());
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

                if (stats != null && stats.TotalRelevantBaseStats == 0)
                    stats.SetRelevantStats();
            }

            UpdateMatchingPokemon();
        }

        private void ReadTrainers()
        {
            ResourceReader resourceReader = new();
            Trainers = resourceReader.ReadTrainers(Pokemons);

            UpdateMatchingTrainers();
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

            if (HighestEvolStats.IsChecked == false)
                TotalColumn.Binding = new Binding("PokemonStats.TotalBaseStats");
            else if (HighestEvolStats.IsChecked == true)
                TotalColumn.Binding = new Binding("PokemonStats.HighestEvolTBS");

            if (AttackTypeBox.SelectedIndex != 0)
            {
                DamageType selectedType;

                if (AttackTypeBox.SelectedIndex == 1) selectedType = DamageType.Attack;
                else if (AttackTypeBox.SelectedIndex == 2) selectedType = DamageType.Special;
                else selectedType = DamageType.Hybrid;

                matchingEnum = HighestEvolStats.IsChecked == false
                    ? matchingEnum.Where(poke => poke.PokemonStats != null && poke.PokemonStats.DamageType == selectedType)
                    : matchingEnum.Where(poke => poke.PokemonStats != null && poke.PokemonStats.HighestEvolDmgType == selectedType);
            }

            if (ShowPassword.IsChecked == false)
                matchingEnum = matchingEnum.Where(poke => poke.PokemonExcel != null && !poke.PokemonExcel.Notes.ToLower().Contains("password"));

            if (ShowEgg.IsChecked == false)
                matchingEnum = matchingEnum.Where(poke => poke.PokemonExcel != null && !poke.PokemonExcel.Notes.ToLower().Contains("egg"));

            if (ShowStarters.IsChecked == false)
                matchingEnum = matchingEnum.Where(poke => poke.PokemonExcel != null && !((poke.PokemonExcel.Location == "Grand Hall" && string.IsNullOrEmpty(poke.PokemonExcel.Notes)) || poke.PokemonExcel.Notes == "A")); //Remove Starters

            if (TypeCount.SelectedIndex == 1)
                matchingEnum = matchingEnum.Where(poke => poke.PokemonStats != null && poke.PokemonStats.PokemonTypes.Count == 1);

            if (TypeCount.SelectedIndex == 2)
                matchingEnum = matchingEnum.Where(poke => poke.PokemonStats != null && poke.PokemonStats.PokemonTypes.Count > 1);

            if (ShowHidden.IsChecked == false)
                matchingEnum = matchingEnum.Where(poke => !poke.Hidden);

            if (OnlyCountRelevantStats.IsChecked == true)
            {
                if (HighestEvolStats.IsChecked == true)
                {
                    TotalColumn.Binding = OnlyCountRelevantStats.IsChecked == true
                        ? new Binding("PokemonStats.HighestEvolRelevantBaseStats")
                        : new Binding("PokemonStats.HighestEvolTBS");
                }
                else if (HighestEvolStats.IsChecked == false)
                {
                    TotalColumn.Binding = OnlyCountRelevantStats.IsChecked == true
                        ? new Binding("PokemonStats.TotalRelevantBaseStats")
                        : (BindingBase)new Binding("PokemonStats.TotalBaseStats");
                }
            }

            string searchTxt = SearchBox.Text.ToLower();
            if (!string.IsNullOrEmpty(searchTxt))
                matchingEnum = matchingEnum.Where(poke => poke.PokemonExcel != null && poke.PokemonExcel.Name.ToLower().Contains(searchTxt));

            //TODO take into account the following:
            //Dry Skin -fire moves do more damage, makes water type moves heal it

            //Flash Fire -raises the power of fire type moves when hit by a fire type move
            //Heat Proof - lowers damage taken by fire type moves
            //Levitate - immune to Ground type moves
            //Motor Drive -raises speed when hit by electric move
            //Scrappy - makes normal and fighting type moves hit ghost types
            //Difficult to implement, need sound bool: Sound Proof -makes sound related move not do anything
            //Thick Fat - weakens fire and ice type moves
            //Volt Absorb - heals when hit by electric move
            //Water Absorb - heal when hit by water move

            List<Pokemon> matchingPokemon = matchingEnum.Any() ? matchingEnum.ToList() : (new());
            if (includedTypes.Count > 0 || excludedTypes.Count > 0)
            {
                for (int i = matchingPokemon.Count - 1; i >= 0; i--)
                {
                    Pokemon poke = matchingPokemon[i];

                    if (poke.PokemonStats == null) continue;

                    List<PokemonType> types = poke.PokemonStats.PokemonTypes;
                    bool matching = includedTypes.Count == 0;

                    for (int y = 0; y < types.Count; y++)
                    {
                        string typeStr = types[y].ToString();

                        if (includedTypes.Contains(typeStr))
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

            TotalColumn.SortDirection = ListSortDirection.Descending;
        }

        private void UpdateMatchingTrainers()
        {
            IEnumerable<Trainer> matchingTrainers = Trainers;

            if (SearchBox == null)
                return;

            if (ShowHidden.IsChecked == false)
                matchingTrainers = matchingTrainers.Where(trainer => !trainer.Hidden);

            string searchTxt = SearchBox.Text.ToLower();
            if (!string.IsNullOrEmpty(searchTxt))
                matchingTrainers = matchingTrainers.Where(trainer => !string.IsNullOrEmpty(trainer.Name) && trainer.Name.ToLower().Contains(searchTxt));

            if (OnlyCurrentTrainers.IsChecked == true && int.TryParse(beforeGymCtrl.Text, out int parsedBeforeGym))
            {
                int beforeGymClamped = Math.Clamp(parsedBeforeGym, 0, levelcaps.Length - 1);

                int levelCap = levelcaps[beforeGymClamped];

                int minLevel = levelCap - 15;
                int maxLevel = levelCap + 10;

                matchingTrainers = matchingTrainers.Where(trainer => trainer.TrainerPokemons != null &&
                    trainer.TrainerPokemons.All(poke => poke.Level >= minLevel && poke.Level <= maxLevel));
            }

            pokemonView.TrainerList = matchingTrainers.ToList();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Save save = new(Pokemons);
            save.SavePokemons();

            List<string> settings = new();

            settings.AddRange(new List<string>() { beforeGymCtrl.Text, AttackTypeBox.SelectedIndex.ToString(), MinimumBaseStats.Text, pokemonSearchTxt, trainerSearchTxt, TypeCount.SelectedIndex.ToString(), TrainerList.SelectedIndex.ToString() });

            if (checkBoxes.All(box => box.IsChecked != null))
                for (int i = 0; i < checkBoxes.Length; i++)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    settings.Add(checkBoxes[i].IsChecked.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
                }

            Save.SaveSettings(settings);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeNotesColumn();
            ResizeTrainerPokemons();
        }

        private void ResizeNotesColumn()
        {
            var columns = PokemonListView.Columns;
            DataGridColumn noteColumn = columns.First(col => (string)col.Header == "Notes");
            noteColumn.Width = IdealSize();
        }

        private void ResizeTrainerPokemons()
        {
            double alt1 = TrainerListView.ActualWidth - TrainerList.ActualWidth;
            double alt2 = ActualWidth - PokemonListView.ActualWidth - TrainerList.ActualWidth - InputPanel.ActualWidth;

            if (alt1 > 0)
                TrainerPokemons.Width = alt1;
            else if (alt2 > 0)
                TrainerPokemons.Width = alt2;
        }

        private double IdealSize()
        {
            double gridWidth = PokemonListView.ActualWidth;

            var columns = PokemonListView.Columns;

            double totalWidth = 0;
            for (int i = 0; i < columns.Count; i++)
                if ((string)columns[i].Header != "Notes")
                    totalWidth += columns[i].ActualWidth;

            return gridWidth - totalWidth - 25;
        }

        #region Clicks
        private void Button_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;

            if (e.ClickCount == 2)
                Type_MouseDoubleClick(btn, null);
            else
                TypeButton_Click(btn, null);
        }

        private void TypeButton_Click(Button clickedBtn, SolidColorBrush? color)
        {
            if (color == null)
                color = FourXEffectiveness;

            string content = ((Run)((Bold)clickedBtn.Content).Inlines.First()).Text;

            if (!includedTypes.Contains(content) && !excludedTypes.Contains(content))
            {
                clickedBtn.Background = color;
                includedTypes.Add(content);
            }
            else
            {
                clickedBtn.Background = defaultBtnColor;
                includedTypes.Remove(content);
                excludedTypes.Remove(content);
            }
            UpdateMatchingPokemon();
        }

        private void ClickedBtn(string type, bool addToAcceptable, SolidColorBrush? color)
        {
            object wantedButton = TypeBtnPanel.FindName(type + "TypeBtn");
            if (wantedButton is Button clickedBtn)
            {
                if (addToAcceptable)
                    TypeButton_Click(clickedBtn, color);
                else
                    Type_MouseDoubleClick(clickedBtn, color);
            }
            else
                MessageBox.Show("Button errored: " + wantedButton.GetType());
        }

        private void Type_MouseDoubleClick(Button clickedBtn, SolidColorBrush? color)
        {
            if (color == null)
                color = userExcludedType;

            string content = ((Run)((Bold)clickedBtn.Content).Inlines.First()).Text;

            if (!excludedTypes.Contains(content))
            {
                clickedBtn.Background = color;
                excludedTypes.Add(content);
                includedTypes.Remove(content);
            }

            UpdateMatchingPokemon();
        }

        private void PokemonListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Pokemon poke = (Pokemon)PokemonListView.SelectedItem;

            if (poke == null)
                return;

            poke.Hidden = !poke.Hidden;

            if (ShowHidden.IsChecked == false)
                UpdateMatchingTrainers();
        }

        private void TrainerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Trainer trainer = (Trainer)TrainerList.SelectedItem;

            if (trainer == null)
                return;

            trainer.Hidden = !trainer.Hidden;

            if (ShowHidden.IsChecked == false)
                UpdateMatchingTrainers();
        }

        private void OnlyCurrentGym_Click(object sender, RoutedEventArgs e)
        {
            GymLabelStr.Content = OnlyCurrentGym.IsChecked == true ? "Gym" : "Before Gym";
            UpdateMatchingPokemon();
            UpdateMatchingTrainers();
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            Pokemons = new();
            ReadExcelAndPbs();
        }

        private void UpdateMatchingPokemonAndTrainers(object sender, RoutedEventArgs e)
        {
            UpdateMatchingPokemon();
            UpdateMatchingTrainers();
        }

        private void UpdateMatchingPokemon_Click(object sender, RoutedEventArgs e)
        {
            UpdateMatchingPokemon();
        }

        private void UpdateMatchingTrainers_Click(object sender, RoutedEventArgs e)
        {
            UpdateMatchingPokemon();
        }

        private void SearchForTrainers_Click(object sender, RoutedEventArgs e)
        {
            if (SearchForTrainers.IsChecked == true)
            {
                PokemonListView.Visibility = Visibility.Collapsed;
                TrainerListView.Visibility = Visibility.Visible;

                SearchBox.Text = trainerSearchTxt;

                ResizeTrainerPokemons();
            }
            else
            {
                PokemonListView.Visibility = Visibility.Visible;
                TrainerListView.Visibility = Visibility.Collapsed;

                SearchBox.Text = pokemonSearchTxt;

                ResizeNotesColumn();
            }

            UpdateMatchingPokemon();
        }
        #endregion

        private void ResetTypeSelections()
        {
            UIElementCollection children = TypeBtnPanel.Children;

            for (int i = 0; i < children.Count; i++)
            {
                UIElement selectedChild = children[i];

                if (selectedChild is Button button)
                    button.Background = defaultBtnColor;
            }

            includedTypes.Clear();
            excludedTypes.Clear();
        }

        private void TrainerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Trainer selectedTrainer = (Trainer)((DataGrid)sender).SelectedItem;

            if (selectedTrainer == null)
                return;

            pokemonView.SelectedTrainer = selectedTrainer;
        }

        private void TrainerPokemon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TrainerPokemon selectedPokemon = (TrainerPokemon)((DataGrid)sender).SelectedItem;

            if (selectedPokemon == null)
                return;

            for (int i = 0; i < Pokemons.Count; i++)
            {
                Pokemon pokemon = Pokemons[i];

                if (pokemon.PokemonStats == null)
                    continue;

                pokemon.PokemonStats.ComparisonPokemon = selectedPokemon;
                pokemon.PokemonStats.SetRelevantStats();
            }

            ResetTypeSelections();

            int physicalMoves = 0;
            int specialMoves = 0;
            List<PokemonType> moveTypes = new();
            List<PokemonType> damageMoveTypes = new();

            for (int i = 0; selectedPokemon.Moves != null && i < selectedPokemon.Moves.Count; i++)
            {
                Move selectedMove = selectedPokemon.Moves[i];
                PokemonType moveType = selectedMove.Type;

                if (selectedMove.MoveType is MoveType.Physical or MoveType.Special)
                {
                    if (!damageMoveTypes.Contains(moveType))
                        damageMoveTypes.Add(moveType);

                    if (selectedMove.MoveType == MoveType.Physical)
                        physicalMoves++;
                    else
                        specialMoves++;
                }

                if (!moveTypes.Contains(moveType))
                    moveTypes.Add(moveType);
            }

            //Get which types of pokemon are weak against the trainer pokemons moves and hide them
            Types types = new();

            List<TypeEffectiveness> enemyMoveEffectivenesses = types.GetOffensiveMatchups(damageMoveTypes).ToList();
            for (int i = 0; i < enemyMoveEffectivenesses.Count; i++)
            {
                TypeEffectiveness selectedEff = enemyMoveEffectivenesses[i];
                string typeStr = selectedEff.Type.ToString();
                if (selectedEff.Effectiveness > 1 && !excludedTypes.Contains(typeStr))
                    ClickedBtn(typeStr, false, selectedEff.Effectiveness == 2 ? TwoXWeak : userExcludedType);
                else if (selectedEff.Effectiveness < 1 && !includedTypes.Contains(typeStr) && !excludedTypes.Contains(typeStr))
                {
                    SolidColorBrush color;

                    #pragma warning disable IDE0045 // Pointless to follow since it will complain if i use nested conditional expressions too
                    if (selectedEff.Effectiveness == 0.5)
                        color = TwoXResistant;
                    else if (selectedEff.Effectiveness == 0.25)
                        color = FourXResistant;
                    else
                        color = Immune;
                    #pragma warning restore IDE0045 // Convert to conditional expression

                    ClickedBtn(typeStr, true, color);
                }
            }

            //Get which types of moves are supereffective against the trainer pokemons types and filter for them
            if (selectedPokemon.PokemonStats != null)
            {
                List<TypeEffectiveness> enemyWeaknesses = types.GetDefensiveMatchups(selectedPokemon.PokemonStats.PokemonTypes).Where(eff => eff.Effectiveness > 1).ToList();
                for (int i = 0; i < enemyWeaknesses.Count; i++)
                {
                    TypeEffectiveness selectedEff = enemyWeaknesses[i];
                    string typeStr = selectedEff.Type.ToString();

                    if (!includedTypes.Contains(typeStr) && !excludedTypes.Contains(typeStr))
                        ClickedBtn(typeStr, true, selectedEff.Effectiveness == 2 ? TwoXEffectiveness : FourXEffectiveness);
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchForTrainers.IsChecked == true)
            {
                trainerSearchTxt = SearchBox.Text;
                UpdateMatchingTrainers();
            }   
            else
            {
                pokemonSearchTxt = SearchBox.Text;
                UpdateMatchingPokemon();
            }
        }

        private void HighestEvolStats_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Pokemons.Count; i++)
            {
                Pokemon poke = Pokemons[i];

                if (poke.PokemonStats != null)
                    poke.PokemonStats.UpdateStats(HighestEvolStats.IsChecked == true);
            }

            UpdateMatchingPokemon_Click(sender, e);
        }
    }
}
