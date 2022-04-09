namespace Desafio_PokeApi_with_parallel.Models
{
    internal class PokemonAbility
    {
        public bool Is_Hidden { get; set; }
        public int Slot { get; set; }
        public Ability Ability { get; set; }
        public override string ToString()
        {
            return $"{ Ability.Name }";
        }

    }
}
