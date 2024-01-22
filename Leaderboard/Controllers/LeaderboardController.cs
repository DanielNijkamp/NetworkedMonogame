using Leaderboard.DTO;
using Leaderboard.Service;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Controllers;

[ApiController]
[Route("[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly LeaderboardService leaderboardService;

    public LeaderboardController(LeaderboardService leaderboardService)
    {
        this.leaderboardService = leaderboardService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var leaderboard = leaderboardService.GetLeaderboard();
        var dto = new LeaderboardQueryDto(leaderboard);
        return Ok(dto);
    }
}