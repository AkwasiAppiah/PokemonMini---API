using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Threading.Tasks;
using PokeAPI;
using PokemonMiniTest.Models;
using PokemonMiniTest.Services;

namespace PokemonMiniTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : Controller
    {
        private readonly IGetSingleModelPokemon _getSinglePokemon;
        private readonly IYodaTranslationService _yodaTranslationService;
        private readonly IShakespeareTranslationService _shakespeareTranslationService;
        public PokemonController(IGetSingleModelPokemon getSinglePokemon, IYodaTranslationService yodaTranslationService, IShakespeareTranslationService shakespeareTranslationService)
        {
            _getSinglePokemon = getSinglePokemon;
            _yodaTranslationService = yodaTranslationService;
            _shakespeareTranslationService = shakespeareTranslationService;
        }
        // GET: PokemonController - basic pokemon info 

        [HttpGet("{pokemonName}")] 
        public async Task<ActionResult<ModelPokemon>> GetSinglePokemonAsyncTask(string pokemonName)
        {
            var lowercasePokemonName = pokemonName.ToLower();
            var pokemonFromApi = await _getSinglePokemon.GetSingleModelPokemonService(lowercasePokemonName);

            if(pokemonFromApi.ErrorMessage == null)
            {
                 return Ok(pokemonFromApi.Data);
            }
           
            return NotFound(pokemonFromApi.Data);
        }

        [HttpGet("/translated/{pokemonName}")]
        public async Task<ActionResult<ModelPokemon>> GetSingleTranslatedPokemonAsyncTask(string pokemonName)
        {
            var serviceResult = await _getSinglePokemon.GetSingleModelPokemonService(pokemonName);

            var pokemonFromPokemonApi = serviceResult.Data;

            if (serviceResult.ErrorMessage == null)
            {
                if (pokemonFromPokemonApi.Habitat == "cave" || pokemonFromPokemonApi.IsLegendary)
                {
                    var translatedPokemon = await _yodaTranslationService.GetTranslatedYodaPokemonModel(pokemonFromPokemonApi);
                    return translatedPokemon.Data;
                }
                else
                {
                    var pokemonFromShakespeareApi =
                        await _shakespeareTranslationService.TranslateShakespeareAsyncTask(pokemonFromPokemonApi);
                }

                return Ok(pokemonFromPokemonApi);
            }

            return NotFound(serviceResult.Data);
        }
    }
}
