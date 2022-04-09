namespace Desafio_PokeApi_with_parallel.Models
{
    internal class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PokemonType> Types { get; set; }
        public List<PokemonAbility> Abilities { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public PokemonSprites Sprites { get; set; }
        public override string ToString()
        {
            return $"Name: {Name}, Types: { string.Join(", ", Types) }, Abilities: { string.Join(", ", Abilities) }, Weight: {Weight}, Height: {Height}, Sprites: {Sprites.Front_Default}";
        }
    }
}
