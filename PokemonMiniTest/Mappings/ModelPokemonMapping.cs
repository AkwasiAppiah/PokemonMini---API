using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PokemonMiniTest.Models;

namespace PokemonMiniTest.Mappings
{
    public class ModelPokemonMapping : Profile
    {
        public ModelPokemonMapping()
        {
            CreateMap<PokemonResponse, ModelPokemon>()
                .ForMember(x => x.Description, y => y.MapFrom(b => b.flavor_text_entries.FirstOrDefault(z => z.language.name == "en").flavor_text))
                .ForMember(x => x.Name, y => y.MapFrom(z => z.name))
                .ForMember(x => x.Habitat, y => y.MapFrom(z => z.habitat.name))
                .ForMember(x => x.IsLegendary, y => y.MapFrom(z => z.is_legendary));
        }
    }
}
