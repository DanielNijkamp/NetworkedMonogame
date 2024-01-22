namespace Leaderboard.Service;

public class LeaderboardService
{
    private readonly Dictionary<Guid, int> leaderboard = new();

    public void UpdateLeaderboard(Guid entityId)
    {
        if (!leaderboard.TryAdd(entityId, 1))
        {
            leaderboard[entityId]++;
        }
    }

    public IReadOnlyDictionary<Guid, int> GetLeaderboard()
    {
        return leaderboard
            .OrderByDescending(pair => pair.Value)
            .ToDictionary(pair => pair.Key, pair => pair.Value)
            .AsReadOnly();
    }
}