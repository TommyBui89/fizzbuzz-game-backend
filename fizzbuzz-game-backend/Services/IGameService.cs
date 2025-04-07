using FizzBuzzGameBackend.DTOs;

namespace FizzBuzzGameBackend.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameResponseDto>> GetAllGamesAsync();
        Task<GameResponseDto?> GetGameByIdAsync(int id);
        Task<GameResponseDto> CreateGameAsync(CreateGameDto dto);
        Task<bool> UpdateGameAsync(UpdateGameDto dto);
        Task<bool> DeleteGameAsync(int id);
    }
}