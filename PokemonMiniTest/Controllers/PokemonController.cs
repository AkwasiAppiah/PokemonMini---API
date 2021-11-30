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
        private readonly IPokemonService _pokemonService;
        private readonly IYodaTranslationService _yodaTranslationService;
        private readonly IShakespeareTranslationService _shakespeareTranslationService;
        public PokemonController(IPokemonService getSinglePokemon, IYodaTranslationService yodaTranslationService, IShakespeareTranslationService shakespeareTranslationService)
        {
            _pokemonService = getSinglePokemon;
            _yodaTranslationService = yodaTranslationService;
            _shakespeareTranslationService = shakespeareTranslationService;
        }
        // GET: PokemonController - basic pokemon info 

        [HttpGet("{pokemonName}")] 
        public async Task<ActionResult<ModelPokemon>> GetSinglePokemonAsyncTask(string pokemonName)
        {
            var lowercasePokemonName = pokemonName.ToLower();
            var pokemonFromApi = await _pokemonService.GetSinglePokemon(lowercasePokemonName);

            if(pokemonFromApi.IsSuccessful)
            {
                 return Ok(pokemonFromApi.Data);
            }
           
            return NotFound($"Cannot find pokemon {pokemonName}");
        }

        [HttpGet("/translated/{pokemonName}")]
        public async Task<ActionResult<ModelPokemon>> GetSingleTranslatedPokemonAsyncTask(string pokemonName)
        {
            var serviceResult = await _pokemonService.GetSinglePokemon(pokemonName);

            var pokemonFromPokemonApi = serviceResult.Data;

            if (!serviceResult.IsSuccessful)
            {
                return new StatusCodeResult(500);
            }

            if (serviceResult.Data == null)
            {
                return NotFound($"Cannot find pokemon {pokemonName}");
            }

            if (pokemonFromPokemonApi.Habitat == "cave" || pokemonFromPokemonApi.IsLegendary)
            {
                var translatedPokemon = await _yodaTranslationService.GetTranslatedYodaPokemonModel(pokemonFromPokemonApi);
                if (!translatedPokemon.IsSuccessful)
                {
                    return new StatusCodeResult(500);
                }

                if(translatedPokemon.Data == null)
                {
                    return NotFound();
                }

                return Ok(translatedPokemon.Data);
            }
                
            var pokemonFromShakespeareApi =
                await _shakespeareTranslationService.TranslateShakespeareAsyncTask(pokemonFromPokemonApi);
            return Ok(pokemonFromShakespeareApi.Data);
        }
    }
}
