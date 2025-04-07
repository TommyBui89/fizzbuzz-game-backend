using FizzBuzzGameBackend.Models;

namespace FizzBuzzGameBackend.Services
{
    public class GameSessionManager
    {
        private readonly Dictionary<Guid, GameSession> _sessions = new();
        private readonly Random _random = new();

        private const int DefaultRangeMax = 100;

        public GameSession CreateSession(int gameId, int durationSeconds)
        {
            var session = new GameSession
            {
                SessionId = Guid.NewGuid(),
                GameId = gameId,
                DurationSeconds = durationSeconds
            };

            session.LastNumber = GetNextRandomNumber(session);
            _sessions[session.SessionId] = session;
            return session;
        }

        public bool TryGetSession(Guid sessionId, out GameSession session)
        {
            return _sessions.TryGetValue(sessionId, out session);
        }

        public int GetNextRandomNumber(GameSession session)
        {
            var availableNumbers = Enumerable.Range(1, DefaultRangeMax)
                                             .Where(n => !session.NumbersUsed.Contains(n))
                                             .ToList();
            if (!availableNumbers.Any())
                throw new Exception("No more numbers available in this session");

            var index = _random.Next(availableNumbers.Count);
            var nextNumber = availableNumbers[index];
            session.NumbersUsed.Add(nextNumber);
            session.LastNumber = nextNumber;
            return nextNumber;
        }

        public void RemoveSession(Guid sessionId)
        {
            _sessions.Remove(sessionId);
        }
    }
}
