using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokeAPI;
using PokemonMiniTest.Models;

namespace PokemonMiniTest.Services
{
    public interface IPokemonService
    {
        Task<ServiceResult<ModelPokemon>> GetSinglePokemonAsync(string pokemonName);
    }
}
