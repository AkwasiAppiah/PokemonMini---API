using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LitJson;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using PokemonMiniTest.Controllers;
using PokemonMiniTest.Mappings;
using PokemonMiniTest.Models;
using PokemonMiniTest.Services;
using Shouldly;
using Xunit;

namespace PokemonMiniTest.Unit.Tests
{
    public class IntergrationTest
    {
        [Fact]
        public void Test_Pokemon_Controller_If_Service_Returns_Null()
        {
            var _getsinglemodelpokemon = new Mock<IGetSingleModelPokemon>();
            var _yodaTranslationservice = new Mock<IYodaTranslationService>();
            var _shakespeareTranslationService = new Mock<IShakespeareTranslationService>();

            var modelPokemonServiceReturns = new ModelPokemon()
            {
                Name = null,
                Description = null,
                Habitat = null,
                IsLegendary = false
            };


            var ServiceResultServiceReturns = new ServiceResult<ModelPokemon>()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                ErrorMessage = null,
                Data = modelPokemonServiceReturns
            };

            PokemonController sut = new PokemonController(_getsinglemodelpokemon.Object, _yodaTranslationservice.Object, _shakespeareTranslationService.Object);

            _getsinglemodelpokemon.Setup(x => x.GetSingleModelPokemonService(It.IsAny<string>())).ReturnsAsync(ServiceResultServiceReturns);

            //Assert 

            var data = sut.GetSingleTranslatedPokemonAsyncTask("hello");

            var result = data.Result.Result as NotFoundObjectResult;

            Assert.Equal(null, data.Result.Value);
            //result.StatusCode.ShouldBe(404);
            //data.Result.Result.Value.ShouldBeNull();
        }

        [Fact]
        public void Test_Pokemon_Controller_If_Service_Handles_Condition_of_Cave()
        {
            var _getsinglemodelpokemon = new Mock<IGetSingleModelPokemon>();
            var _yodaTranslationservice = new Mock<IYodaTranslationService>();
            var _shakespeareTranslationService = new Mock<IShakespeareTranslationService>();

            var modelPokemonServiceReturns = new ModelPokemon()
            {
                Name = "Akwasi",
                Description = "dfjhkl;hjhklfgdhjkl",
                Habitat = "cave",
                IsLegendary = false
            };


            var ServiceResultServiceReturns = new ServiceResult<ModelPokemon>()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                ErrorMessage = "",
                Data = modelPokemonServiceReturns
            };

            PokemonController sut = new PokemonController(_getsinglemodelpokemon.Object, _yodaTranslationservice.Object, _shakespeareTranslationService.Object);

            _getsinglemodelpokemon.Setup(x => x.GetSingleModelPokemonService(It.IsAny<string>())).ReturnsAsync(ServiceResultServiceReturns);
            _yodaTranslationservice.Setup(x => x.GetTranslatedYodaPokemonModel(It.Is<ModelPokemon>(x => x == modelPokemonServiceReturns))).ReturnsAsync(new ServiceResult<ModelPokemon>()
            {
                HttpStatusCode = HttpStatusCode.OK,
                ErrorMessage = null,
                Data = modelPokemonServiceReturns
            });

            //Assert 

            var data = sut.GetSingleTranslatedPokemonAsyncTask("hello");

            var result = data.Result.Result as OkObjectResult;

            Assert.Equal(modelPokemonServiceReturns, result.Value);
            //result.StatusCode.ShouldBe(200);
            //data.Result.Result.Value.ShouldBeNull();
        }

        [Fact]
        public void Test_Pokemon_Controller_If_Service_Handles_Condition_of_IsLegendary()
        {
            var _getsinglemodelpokemon = new Mock<IGetSingleModelPokemon>();
            var _yodaTranslationservice = new Mock<IYodaTranslationService>();
            var _shakespeareTranslationService = new Mock<IShakespeareTranslationService>();

            var modelPokemonServiceReturns = new ModelPokemon()
            {
                Name = "Akwasi",
                Description = "dfjhkl;hjhklfgdhjkl",
                Habitat = "forest",
                IsLegendary = true
            };

            var ServiceResultServiceReturns = new ServiceResult<ModelPokemon>()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                ErrorMessage = "",
                Data = modelPokemonServiceReturns
            };

            PokemonController sut = new PokemonController(_getsinglemodelpokemon.Object, _yodaTranslationservice.Object, _shakespeareTranslationService.Object);

            _getsinglemodelpokemon.Setup(x => x.GetSingleModelPokemonService(It.IsAny<string>())).ReturnsAsync(ServiceResultServiceReturns);
            _yodaTranslationservice.Setup(x => x.GetTranslatedYodaPokemonModel(It.Is<ModelPokemon>(x => x == modelPokemonServiceReturns))).ReturnsAsync(new ServiceResult<ModelPokemon>()
            {
                HttpStatusCode = HttpStatusCode.OK,
                ErrorMessage = null,
                Data = modelPokemonServiceReturns
            });

            //Assert 

            var data = sut.GetSingleTranslatedPokemonAsyncTask("hello");

            var result = data.Result.Result as OkObjectResult;

            Assert.Equal(modelPokemonServiceReturns, result.Value);
            //result.StatusCode.ShouldBe(200);
            //data.Result.Result.Value.ShouldBeNull();
        }

        [Fact]
        public void Test_Pokemon_Controller_If_Service_Handles_Condition_of_Not_Cave_Or_Not_Legendary()
        {
            var _getsinglemodelpokemon = new Mock<IGetSingleModelPokemon>();
            var _yodaTranslationservice = new Mock<IYodaTranslationService>();
            var _shakespeareTranslationService = new Mock<IShakespeareTranslationService>();

            var modelPokemonServiceReturns = new ModelPokemon()
            {
                Name = "Akwasi",
                Description = "dfjhkl;hjhklfgdhjkl",
                Habitat = "forest",
                IsLegendary = false
            };


            var ServiceResultServiceReturns = new ServiceResult<ModelPokemon>()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                ErrorMessage = null,
                Data = modelPokemonServiceReturns
            };

            PokemonController sut = new PokemonController(_getsinglemodelpokemon.Object, _yodaTranslationservice.Object, _shakespeareTranslationService.Object);

            _getsinglemodelpokemon.Setup(x => x.GetSingleModelPokemonService(It.IsAny<string>())).ReturnsAsync(ServiceResultServiceReturns);
            //_yodaTranslationservice.Setup(x => x.GetTranslatedYodaPokemonModel(It.Is<ModelPokemon>(x => x == modelPokemonServiceReturns))).ReturnsAsync(new ServiceResult<ModelPokemon>());
            _shakespeareTranslationService.Setup(x => x.TranslateShakespeareAsyncTask(It.Is<ModelPokemon>(x => x == modelPokemonServiceReturns))).ReturnsAsync(new ServiceResult<ModelPokemon>()

            {
                HttpStatusCode = HttpStatusCode.OK,
                ErrorMessage = null,
                Data = modelPokemonServiceReturns
            });

            //Assert 

            var data = sut.GetSingleTranslatedPokemonAsyncTask("hello");

            var result = data.Result.Result as OkObjectResult;

            Assert.Equal(modelPokemonServiceReturns, result.Value);
            //result.StatusCode.ShouldBe(200);
            //data.Result.Result.Value.ShouldBeNull();
        }
    }
}
