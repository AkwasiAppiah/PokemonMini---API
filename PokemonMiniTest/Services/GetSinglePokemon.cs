using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokeAPI;
using PokemonMiniTest.Models;
using AutoMapper;
using PokemonMiniTest.Mappings;
using PokemonMiniTest.HTTPClientHelpers;

namespace PokemonMiniTest.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonHTTPClientHelper _hTTPClientHelper;
        public PokemonService(IPokemonHTTPClientHelper hTTPClientHelper)
        {
            _hTTPClientHelper = hTTPClientHelper;
        }

        public async Task<ServiceResult<ModelPokemon>> GetSinglePokemonAsync(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                return new ServiceResult<ModelPokemon>
                {
                    ErrorMessage = "Pokemon name is required"
                };
            }

            //Format pokemon name so that it is uniform
            pokemonName = pokemonName.Trim().ToLower();
            try
            {
                var pokemonApiResponse = await _hTTPClientHelper.GetAsync<PokemonResponse>(pokemonName);

                var config = new MapperConfiguration(cfg => cfg.AddProfile<ModelPokemonMapping>());
                var mapper = new Mapper(config);
                var modelPokemon = mapper.Map<ModelPokemon>(pokemonApiResponse);

                return new ServiceResult<ModelPokemon>()
                {
                    Data = modelPokemon,
                };
            }
            catch (Exception e)
            {
                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = e.Message,
                };
            }

        }
    }
}

