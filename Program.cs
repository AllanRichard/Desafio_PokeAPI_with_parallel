using Desafio_PokeApi_with_parallel.Models;
using System.Net;
using System.Text.Json;

namespace DesafioPokeApi
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task Main()
        {
            string[] pokemons = { "Charmander", "Squirtle", "Caterpie", "Weedle", "Pidgey", "Pidgeotto", "Rattata", "Spearow", "Fearow", "Arbok", "Pikachu", "Sandshrew" };
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Pokemon> pokemonsList = new List<Pokemon>();
            Dictionary<string, string> pokemonImages = new Dictionary<string, string>();

            foreach (var name in pokemons)
            {
                try
                {
                    Pokemon? pokemon = null;
                    HttpResponseMessage response = await client.GetAsync("https://pokeapi.co/api/v2/pokemon/" + name.ToLower());
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        pokemon = JsonSerializer.Deserialize<Pokemon>(content, options);
                        if (pokemon != null)
                        {
                            pokemonsList.Add(pokemon);
                            pokemonImages.Add(pokemon.Name, pokemon.Sprites.Front_Default);
                            Console.WriteLine(pokemon + "\n");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            if (pokemonsList.Count > 0)
            {
                GenerateArchiveTxt(pokemonsList);
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            }
            if (pokemonImages.Count > 0)
            {
                DownloadImages(pokemonImages);
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        static void GenerateArchiveTxt(List<Pokemon> pokemons)
        {
            try
            {
                Directory.SetCurrentDirectory(@"..\..\..\Archives");
                string path = Directory.GetCurrentDirectory();
                var archives = Directory.GetFiles(path, "*.txt");
                foreach (var archive in archives)
                {
                    File.Delete(archive);
                }
                if (pokemons.Count > 0)
                {
                    try
                    {
                        StreamWriter archivePokemon = new StreamWriter(Path.Combine(path, "pokemon.txt"), true);
                        foreach (var pokemon in pokemons)
                        {
                            archivePokemon.WriteLine(pokemon + "\n");
                        }
                        archivePokemon.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Message :{0} ", e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        static void DownloadImages(Dictionary<string, string> images)
        {
            try
            {
                Directory.SetCurrentDirectory(@"..\..\..\Images");
                string path = Directory.GetCurrentDirectory();
                var archives = Directory.GetFiles(path, "*.png");
                foreach (var archive in archives)
                {
                    File.Delete(archive);
                }
                if (images.Count > 0)
                {
                    try
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            foreach (var image in images)
                            {
                                webClient.DownloadFile(new Uri(image.Value), image.Key + ".png");
                            }
                            webClient.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Message :{0} ", e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}