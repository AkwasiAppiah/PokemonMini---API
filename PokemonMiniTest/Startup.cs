using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PokemonMiniTest.HTTPClientHelpers;
using PokemonMiniTest.Models;
using PokemonMiniTest.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonMiniTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var apiConfig = new ApiConfig();
            Configuration.GetSection(ApiConfig.Api).Bind(apiConfig);
            services.AddControllers();
            services.AddScoped<IPokemonService, PokemonService>();
            services.AddScoped<IYodaTranslationService, YodaTranslationService>();
            services.AddScoped<IShakespeareTranslationService, ShakespeareTranslationService>();

            //services.AddHttpClient("PokemonApi");
            //services.AddSingleton<IHTTPClientHelper, HTTPClientHelper>(s => new HTTPClientHelper(s.GetService<IHttpClientFactory>(), "PokemonApi"));

            services.AddHttpClient("Pokemon", client => { client.BaseAddress = new System.Uri(apiConfig.PokemonBaseUrl);});
            services.AddSingleton<IPokemonHTTPClientHelper, PokemonHTTPClientHelper>(s => new PokemonHTTPClientHelper(s.GetService<IHttpClientFactory>(), "Pokemon"));

            services.AddHttpClient("Yoda", client => { client.BaseAddress = new System.Uri($"https://api.funtranslations.com/translate/yoda"); });
            services.AddSingleton<IYodaHTTPClientHelper, YodaHTTPClientHelper>(s => new YodaHTTPClientHelper(s.GetService<IHttpClientFactory>(), "Yoda"));

            services.AddHttpClient("Shakespeare", client => { client.BaseAddress = new System.Uri($"https://api.funtranslations.com/translate/shakespeare"); });
            services.AddSingleton<IShakespeareHTTPClientHelper, ShakespeareHTTPClientHelper>(s => new ShakespeareHTTPClientHelper(s.GetService<IHttpClientFactory>(), "Shakespeare"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        //public class HTTPClientHelper : IHTTPClientHelper
        //{
        //    IHttpClientFactory httpClientFactory;
        //    HttpClient client;
        //    String ClientName;

        //    public HTTPClientHelper(IHttpClientFactory httpClientFactory, string ClientName)
        //    {
        //        this.httpClientFactory = httpClientFactory;
        //        this.ClientName = ClientName;
        //    }

        //    #region Generic, Async, static HTTP functions for GET, POST, PUT, and DELETE             

        //    public async Task<T> GetAsync<T>(string url)
        //    {
        //        T data;
        //        client = httpClientFactory.CreateClient(ClientName);
        //        try
        //        {
        //            using (HttpResponseMessage response = await client.GetAsync(url))
        //            using (HttpContent content = response.Content)
        //            {
        //                if (!response.IsSuccessStatusCode)
        //                {
        //                    throw new ThirdPartyApiException($"API at this address {url} failed.");
        //                }
        //                string d = await content.ReadAsStringAsync();
        //                if (d != null)
        //                {
        //                    data = JsonConvert.DeserializeObject<T>(d);
        //                    return (T)data;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //        Object o = new Object();
        //        return (T)o;
        //    }

        //    public async Task<T> PostAsync<T>( HttpContent contentPost)
        //    {
        //        T data;
        //        string url,
        //        client = httpClientFactory.CreateClient(ClientName);
        //        using (HttpResponseMessage response = await client.PostAsync(url, contentPost))
        //        using (HttpContent content = response.Content)
        //        {

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                throw new ThirdPartyApiException($"API at this address {url} failed.");
        //            }

        //            string d = await content.ReadAsStringAsync();
        //            if (d != null)
        //            {
        //                data = JsonConvert.DeserializeObject<T>(d);
        //                return (T)data;
        //            }
        //        }
        //        Object o = new Object();
        //        return (T)o;
        //    }

        //    public async Task<T> PutAsync<T>(string url, HttpContent contentPut)
        //    {
        //        T data;
        //        client = httpClientFactory.CreateClient(ClientName);

        //        using (HttpResponseMessage response = await client.PutAsync(url, contentPut))
        //        using (HttpContent content = response.Content)
        //        {
        //            string d = await content.ReadAsStringAsync();
        //            if (d != null)
        //            {
        //                data = JsonConvert.DeserializeObject<T>(d);
        //                return (T)data;
        //            }
        //        }
        //        Object o = new Object();
        //        return (T)o;
        //    }

        //    public async Task<T> DeleteAsync<T>(string url)
        //    {
        //        T newT;
        //        client = httpClientFactory.CreateClient(ClientName);

        //        using (HttpResponseMessage response = await client.DeleteAsync(url))
        //        using (HttpContent content = response.Content)
        //        {
        //            string data = await content.ReadAsStringAsync();
        //            if (data != null)
        //            {
        //                newT = JsonConvert.DeserializeObject<T>(data);
        //                return newT;
        //            }
        //        }
        //        Object o = new Object();
        //        return (T)o;
        //    }

        //    public class ThirdPartyApiException : Exception
        //    {
        //        public ThirdPartyApiException() : base() { }
        //        public ThirdPartyApiException(string message) : base(message) { }
        //        public ThirdPartyApiException(string message, Exception inner) : base(message, inner) { }

        //        // A constructor is needed for serialization when an
        //        // exception propagates from a remoting server to the client.
        //        protected ThirdPartyApiException(System.Runtime.Serialization.SerializationInfo info,
        //            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        //    }
        //    #endregion
        //}
    }
}
