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

namespace PokemonMiniTest.Services
{
    public class GetSingleModelPokemon : IPokemonService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GetSingleModelPokemon(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

       
        public async Task<ServiceResult<ModelPokemon>> GetSinglePokemon(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return new ServiceResult<ModelPokemon>
                {
                    ErrorMessage = "Pokemon name is required"
                };
            }

            pokemonName = pokemonName.Trim().ToLower();

            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            try
            {
                //too much in once method SOLID ... (Single responsibility)
                // Read up on .net core configuration appsettings.json
                var endpoint = $"https://pokeapi.co/api/v2/pokemon-species/{pokemonName}";
                //DI
                //look into 'using' statements
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var result = await httpClient.GetAsync(endpoint);

                    if (!result.IsSuccessStatusCode)
                    {
                        return new ServiceResult<ModelPokemon>()
                        {
                            HttpStatusCode = result.StatusCode,
                            ErrorMessage = "External Service error"
                        };
                    }

                    var responseBody = await result.Content.ReadAsStringAsync();

                    var response = JsonSerializer.Deserialize<PokemonResponse>(responseBody);

                    var mapper = new Mapper(config);
                    var modelPokemon = mapper.Map<ModelPokemon>(response);

                    return new ServiceResult<ModelPokemon>()
                    {
                        Data = modelPokemon,
                        HttpStatusCode = result.StatusCode
                    };
                }
            }
            catch (Exception e)
            {
                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = e.Message
                };
            }

        }
    }
}

