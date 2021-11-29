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
    public class GetSingleModelPokemon : IGetSingleModelPokemon
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GetSingleModelPokemon(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

       
        public async Task<ServiceResult<ModelPokemon>> GetSingleModelPokemonService(string pokemonName)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            try
            {
                //too much in once method SOLID ... (Single responsibility)

                var endpoint = $"https://pokeapi.co/api/v2/pokemon-species/{pokemonName}";
                //DI
                //look into 'using' statements
                var httpClient = _httpClientFactory.CreateClient();

                var result = await httpClient.GetAsync(endpoint);

                if (!result.IsSuccessStatusCode)
                {
                    return new ServiceResult<ModelPokemon>()
                    {
                        HttpStatusCode = result.StatusCode,
                        ErrorMessage = "External Service error",
                        Data = null,
                    };
                }

                var responseBody = await result.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<PokemonResponse>(responseBody);

                var mapper = new Mapper(config);
                var modelPokemon = mapper.Map<ModelPokemon>(response);

                return new ServiceResult<ModelPokemon>()
                {
                    Data = modelPokemon,
                    ErrorMessage = string.Empty,
                    HttpStatusCode = result.StatusCode
                };
            }
            catch (Exception e)
            {
                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = e.Message,
                    Data = null,
                };
            }

        }
    }
}

