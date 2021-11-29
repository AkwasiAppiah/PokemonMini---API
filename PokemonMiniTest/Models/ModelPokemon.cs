using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonMiniTest.Models
{
    public class ModelPokemon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }

        protected bool Equals(ModelPokemon other)
        {
            return Name == other.Name && Description == other.Description && Habitat == other.Habitat && IsLegendary == other.IsLegendary;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ModelPokemon) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, Habitat, IsLegendary);
        }
    }
}
