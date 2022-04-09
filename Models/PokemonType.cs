namespace Desafio_PokeApi_with_parallel.Models
{
    internal class PokemonType
    {
        public int Slot { get; set; }
        public Type Type { get; set; }
        public override string ToString()
        {
            return $"{ Type.Name }";
        }
    }
}
