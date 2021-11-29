using System.Threading.Tasks;
using PokemonMiniTest.Models;

namespace PokemonMiniTest.Services
{
    public interface IYodaTranslationService
    {
        Task<ServiceResult<ModelPokemon>> GetTranslatedYodaPokemonModel(ModelPokemon modelPokemon);
    }
}