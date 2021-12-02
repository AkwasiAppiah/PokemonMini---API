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
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _getSinglePokemonService;
        private readonly IYodaTranslationService _yodaTranslationService;
        private readonly IShakespeareTranslationService _shakespeareTranslationService;
        public PokemonController(IPokemonService getSinglePokemon, IYodaTranslationService yodaTranslationService, IShakespeareTranslationService shakespeareTranslationService)
        {
            _getSinglePokemonService = getSinglePokemon;
            _yodaTranslationService = yodaTranslationService;
            _shakespeareTranslationService = shakespeareTranslationService;
        }
        // GET: PokemonController - basic pokemon info 

        [HttpGet("{pokemonName}")] 
        public async Task<ActionResult<ModelPokemon>> GetSinglePokemonAsyncTask(string pokemonName)
        {
            //var lowercasePokemonName = pokemonName.ToLower();
            var serviceResult = await _getSinglePokemonService.GetSinglePokemonAsync(pokemonName.ToLower());
            var pokemonFromApi = serviceResult.Data;

            if (serviceResult.ErrorMessage == "External Service error")
            {
                return BadRequest($"Third Party API down... please try again later");
            }
            if(serviceResult.Data == null)
            {
                return NotFound($"Pokemon {pokemonName} was not found");
            }
            if (!serviceResult.IsSuccessful)
            {
                return NotFound($"{pokemonName} not found");
            }
            
            return Ok(pokemonFromApi);
        }

        [HttpGet("/translated/{pokemonName}")]
        public async Task<ActionResult<ModelPokemon>> GetSingleTranslatedPokemonAsyncTask(string pokemonName)
        {

            var serviceResult = await _getSinglePokemonService.GetSinglePokemonAsync(pokemonName);
            var pokemonFromPokemonApi = serviceResult.Data;

            if (!serviceResult.IsSuccessful)
            {
                return new StatusCodeResult(500);
            }

            if(serviceResult.Data == null)
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
                    return Ok(pokemonFromPokemonApi);
                }

                return Ok(translatedPokemon.Data);
            }

            var pokemonFromShakespeareApi = await _shakespeareTranslationService.TranslateShakespeareAsyncTask(pokemonFromPokemonApi);

            if (!pokemonFromShakespeareApi.IsSuccessful)
            {
                return new StatusCodeResult(500);
            }
            if(pokemonFromShakespeareApi.Data == null)
            {
                return BadRequest($"Unable to translate {pokemonFromPokemonApi}");
            }
            return Ok(pokemonFromShakespeareApi.Data);
        }
    }
}
