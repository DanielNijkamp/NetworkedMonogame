namespace Leaderboard.DTO;

public readonly record struct LeaderboardQueryDto(IReadOnlyDictionary<Guid, int> Leaderboard);