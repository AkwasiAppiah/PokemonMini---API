using System.Threading.Tasks;
using PokemonMiniTest.Models;

namespace PokemonMiniTest.Services
{
    public interface IShakespeareTranslationService
    {
        Task<ServiceResult<ModelPokemon>> TranslateShakespeareAsyncTask(ModelPokemon modelToTranslate);
    }
}