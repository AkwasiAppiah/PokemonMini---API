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
using PokemonMiniTest.Mappings;
using PokemonMiniTest.Models;
using PokemonMiniTest.Services;
using Shouldly;
using Xunit;

namespace PokemonMiniTest.Unit.Tests
{
    public class UnitTests
    {
        public class AutoMapper
        {
        [Fact]
        public void TestAutoMapperNotNull()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            mapper.ShouldNotBeNull();

        }

        [Fact]
        public void Test_AutoMapper_Converts_Description_Name_Where_Language_Name_is_En()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse()
            {
                flavor_text_entries = new Flavor_Text_Entries[]
                {
                    new Flavor_Text_Entries()
                    {
                        flavor_text = "hello",
                        language = new Language1()
                        {
                            name = "en"
                        }
                    }
                }
            };

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.Description.ShouldBe(text_Entry.flavor_text_entries[0].flavor_text);

        }


        [Fact]
        public void Test_AutoMapper_Sets_Description_As_Null_Where_En_is_Unvailable()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse()
            {
                flavor_text_entries = new Flavor_Text_Entries[]
                {
                    /*new Flavor_Text_Entries()
                    {
                        flavor_text = "hello",
                        language = new Language1()
                        {
                            name = "en"
                        }
                    }*/
                }
            };

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.Description.ShouldBeNull();

        }

        [Fact]
        public void Test_AutoMapper_Sets_Description_As_First_Where_There_Are_Multiple_Examples()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse()
            {
                flavor_text_entries = new Flavor_Text_Entries[]
                {
                    new Flavor_Text_Entries()
                    {
                        flavor_text = "hello",
                        language = new Language1()
                        {
                            name = "en"
                        }
                    },
                    new Flavor_Text_Entries()
                    {
                        flavor_text = "goodbye",
                        language = new Language1()
                        {
                            name = "en"
                        }
                    }
                }
            };

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.Description.ShouldBe(text_Entry.flavor_text_entries[0].flavor_text);

        }

        [Fact]
        public void Test_AutoMapper_Sets_Name_Where_Name_Is_Available()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse()
            {
                name = "Hello"
            };

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.Name.ShouldBe(text_Entry.name);

        }


        [Fact]
        public void Test_AutoMapper_Sets_Name_Where_Name_Is_Unavailable()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse();
            /*{
                name = "Hello"
            };*/

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.Name.ShouldBeNull();

        }

        [Fact]
        public void Test_AutoMapper_Sets_Habitat_Where_Habitat_Is_UnAvailable()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse();
            /*{
                name = "Hello"
            };*/

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.Name.ShouldBeNull();

        }


        [Fact]
        public void Test_AutoMapper_Sets_Habitat_Where_Habitat_Is_Available()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse()
            {
                habitat = new Habitat()
                {
                    name = "cave"
                }
            };

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.Habitat.ShouldBe(text_Entry.habitat.name);

        }

        [Fact]
        public void Test_AutoMapper_Sets_isLegendary_Where_Is_Available()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse()
            {
                is_legendary = true
            };

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.IsLegendary.ShouldBeTrue();

        }

        [Fact]
        public void Test_AutoMapper_Sets_isLegendary_as_False_Where_Unavailable()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<ModelPokemonMapping>()
            );

            var mapper = new Mapper(config);

            var text_Entry = new PokemonResponse();
            /*{
                is_legendary = false
            };*/

            var modelPokemon = mapper.Map<ModelPokemon>(text_Entry);

            modelPokemon.IsLegendary.ShouldBeFalse();

        }

    }

        public class FakeHttpMessageHandler : DelegatingHandler
        {
            private readonly HttpResponseMessage _fakeResponse;

            public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
            {
                _fakeResponse = responseMessage;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return await Task.FromResult(_fakeResponse);
            }
        }

        public class GetSinglePokemonTests
        {
            [Fact]
            public async void Test_GetSinglePokemonService_Handles_Unsuccessful_Request()
            {
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(JsonConvert.Null, Encoding.UTF8, "application/json")
                });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                var setup = fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(fakeHttpClient);

                var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await getSingleModelPokemonService.GetSinglePokemon("hello");

                output.Data.ShouldBeNull();
                output.ErrorMessage.ShouldBe("External Service error");
                output.IsSuccessful.ShouldBeFalse();
                Assert.Equal(HttpStatusCode.BadRequest,output.HttpStatusCode);
            }

            [Fact]
            public async void Test_Service_Handles_Successful_Request()
            {
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JsonConvert.SerializeObject(new PokemonResponse()
                    {
                        name = "hi"
                    }), Encoding.UTF8, "application/json")
                });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                var setup = fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(fakeHttpClient);

                var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await getSingleModelPokemonService.GetSinglePokemon("hello");

                output.Data.ShouldNotBeNull();
                output.ErrorMessage.ShouldBeNullOrEmpty();
                output.IsSuccessful.ShouldBeTrue();
                Assert.Equal(HttpStatusCode.Accepted,output.HttpStatusCode);
            }

            [Fact]
            public async void Test_Service_Handles_Error_Thrown()
            {
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JsonConvert.SerializeObject(new PokemonResponse()
                    {
                        name = "hi"
                    }), Encoding.UTF8, "application/json")
                });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                var setup = fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Throws(new Exception());

                var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await getSingleModelPokemonService.GetSinglePokemon("hello");

                output.Data.ShouldBeNull();
                output.ErrorMessage.ShouldNotBeNullOrEmpty();
                output.IsSuccessful.ShouldBeFalse();
                Assert.Equal(HttpStatusCode.InternalServerError, output.HttpStatusCode);
            }
            
        }

        public class YodaServiceTests
        {
            [Fact]
            public async void Test_YodaTranslation_Service_Handles_Exception()
            {
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();

                var expected = new ModelPokemon()
                {
                    Name = "Holla"
                };
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JsonConvert.SerializeObject(new ModelPokemon()
                    {
                        Name = null
                    }), Encoding.UTF8, "application/json")
                });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                var setup = fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Throws(new Exception());

                //var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await new YodaTranslationService(fakeHttpClientFactory.Object).GetTranslatedYodaPokemonModel(expected);

                output.Data.ShouldBeNull();
                output.ErrorMessage.ShouldNotBeNullOrEmpty();
                output.IsSuccessful.ShouldBeFalse();
                Assert.Equal(HttpStatusCode.InternalServerError, output.HttpStatusCode);
            }

            [Fact]
            public async void Test_YodaTranslation_Service_Handles_ThirdParty_Failure()
            {
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();

                var expected = new ModelPokemon()
                {
                    Name = "Holla"
                };
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(JsonConvert.Null, Encoding.UTF8, "application/json")
                });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(fakeHttpClient);

                //var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await new YodaTranslationService(fakeHttpClientFactory.Object).GetTranslatedYodaPokemonModel(expected);

                output.Data.ShouldBeNull();
                output.ErrorMessage.ShouldNotBeNullOrEmpty();
                output.IsSuccessful.ShouldBeFalse();
                Assert.Equal(HttpStatusCode.BadRequest, output.HttpStatusCode);
            }

            [Fact]
            public async void Test_YodaTranslation_Service_Handles_Successful_Request()
            {
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();

                var expected = new YodaApiResponseJson()
                {
                    success = new Success()
                    {
                        total = 200
                    },
                    contents = new Contents()
                    {
                        translated = "goodbye"
                    }
                };
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json")
                });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(fakeHttpClient);

                //var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await new YodaTranslationService(fakeHttpClientFactory.Object).GetTranslatedYodaPokemonModel(new ModelPokemon()
                {
                    Description = "blahblah"
                });

                output.Data.ShouldNotBeNull();
                output.ErrorMessage.ShouldBeNullOrEmpty();
                output.IsSuccessful.ShouldBeTrue();
                Assert.Equal(HttpStatusCode.Accepted, output.HttpStatusCode);
            }
        }

        public class ShakespeareUnitTests
        {
            [Fact]
            public async void Test_Shakespeare_Service_Handles_Internal_Server_Error()
            { 
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();

                var expected = new ModelPokemon()
                {
                    Name = "Holla"
                };
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JsonConvert.SerializeObject(new ModelPokemon()
                {
                    Name = null
                }), Encoding.UTF8, "application/json")
            });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                var setup = fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Throws(new Exception());

            //var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await new ShakespeareTranslationService(fakeHttpClientFactory.Object).TranslateShakespeareAsyncTask(expected);

                output.Data.ShouldBeNull();
                output.ErrorMessage.ShouldNotBeNullOrEmpty();
                output.IsSuccessful.ShouldBeFalse();
                Assert.Equal(HttpStatusCode.InternalServerError, output.HttpStatusCode);
            }

            [Fact]
            public async void Test_Shakespeare_Service_Handles_Successful_Request()
            {
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();

                var expected = new YodaApiResponseJson()
                {
                    success = new Success()
                    {
                        total = 200
                    },
                    contents = new Contents()
                    {
                        translated = "goodbye"
                    }
                };
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json")
                });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                var setup = fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(fakeHttpClient);

                //var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await new ShakespeareTranslationService(fakeHttpClientFactory.Object).TranslateShakespeareAsyncTask(new ModelPokemon()
                {
                    Description = "asdfghjkl;'asdfghjkl;"
                });

                output.Data.ShouldNotBeNull();
                output.ErrorMessage.ShouldBeNullOrEmpty();
                output.IsSuccessful.ShouldBeTrue();
                Assert.Equal(HttpStatusCode.Accepted, output.HttpStatusCode);
            }

            [Fact]
            public async void Test_Shakespeare_Service_Handles_External_Server_Error()
            {
                var fakeHttpClientFactory = new Mock<IHttpClientFactory>();

                var expected = new YodaApiResponseJson()
                {
                    success = new Success()
                    {
                        total = 200
                    },
                    contents = new Contents()
                    {
                        translated = "goodbye"
                    }
                };
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(JsonConvert.SerializeObject(expected), Encoding.UTF8, "application/json")
                });
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                var setup = fakeHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(fakeHttpClient);

                //var getSingleModelPokemonService = new GetSingleModelPokemon(fakeHttpClientFactory.Object);

                var output = await new ShakespeareTranslationService(fakeHttpClientFactory.Object).TranslateShakespeareAsyncTask(new ModelPokemon()
                {
                    Description = "asdfghjklasdfghjkl"
                });

                output.Data.ShouldBeNull();
                output.ErrorMessage.ShouldNotBeNullOrEmpty();
                output.IsSuccessful.ShouldBeFalse();
                Assert.Equal(HttpStatusCode.BadRequest, output.HttpStatusCode);
            }
        }
    }
}
