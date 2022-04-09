using Desafio_PokeApi_with_parallel.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace DesafioPokeApi
{
    class Program
    {
        static async Task Main()
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon/"),
            };

            string[] pokemons = { "charmander", "squirtle", "caterpie", "weedle", "pidgey", "pidgeotto", "rattata", "spearow", "fearow", "arbok", "pikachu", "sandshrew" };
            List<Pokemon> pokemonsList = new List<Pokemon>();
            Dictionary<string, string> pokemonImages = new Dictionary<string, string>();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DotNet", "6"));

            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 3
            };

            await Parallel.ForEachAsync(pokemons, parallelOptions, async (uri, token) =>
            {
                Pokemon? pokemon = null;
                pokemon = await client.GetFromJsonAsync<Pokemon>(uri, token);
                if (pokemon != null)
                {
                    pokemonsList.Add(pokemon);
                    pokemonImages.Add(pokemon.Name, pokemon.Sprites.Front_Default);

                    Console.WriteLine(pokemon + "\n");
                }
            });

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