using FizzBuzzGameBackend.DTOs;

namespace FizzBuzzGameBackend.Services
{
    public interface IGameSessionService
    {
        Task<object?> StartSessionAsync(int gameId, int durationSeconds);
        Task<object?> SubmitAnswerAsync(Guid sessionId, SubmitAnswerRequestDto dto);
    }
}