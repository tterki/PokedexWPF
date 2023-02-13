using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json;

namespace PokedexWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PokemonList pokemons;
        public MainWindow()
        {
            InitializeComponent();
             GetPokemonListFromApi("pokemon");
        }

        public async Task GetPokemonListFromApi(string url)
        {
            listPokemonList.Items.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
                var response = await client.GetAsync(url);
                var body = await response.Content.ReadAsStringAsync();
                pokemons = JsonConvert.DeserializeObject<PokemonList>(body);
                foreach (Pokemon pokemon in pokemons.results)
                {
                    listPokemonList.Items.Add(pokemon.name);
                }
            }
        }

        public async Task GetPokemonImage(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
                var response = await client.GetAsync(url);
                var body = await response.Content.ReadAsStringAsync();
                dynamic pokemonImage = JsonConvert.DeserializeObject<dynamic>(body);
                
                imgPokemon.Source = pokemonImage.sprites.front_shiny;

            }
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            string nextUrl = pokemons.next;
            if (nextUrl != null)
            {
                await GetPokemonListFromApi(nextUrl);
            }
        }

        private async void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            string previousUrl = pokemons.previous;
            if (previousUrl != null) await GetPokemonListFromApi(previousUrl);
        }

        private async void listPokemonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void listPokemonList_MouseDown(object sender, MouseButtonEventArgs e)
        {
          
        }

        private void listPokemonList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private async void listPokemonList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedPokemon = listPokemonList.SelectedItem.ToString();
            if (selectedPokemon != null) { await GetPokemonImage("pokemon/" + selectedPokemon); }

        }
    }

    public class PokemonList
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<Pokemon> results { get; set; }
    }

    public class Pokemon
    {
        public string name { get; set; }
        public string url { get; set; }
    }
}
