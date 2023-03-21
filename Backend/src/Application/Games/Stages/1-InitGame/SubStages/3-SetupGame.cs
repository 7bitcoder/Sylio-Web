using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Stages.Interfaces;

namespace Backend.Application.Games.Stages.InitGame.SubStages;
public class SetupGame : IGameStage
{
    private readonly IGameConnection _connection;
    private readonly IPlayersManager _players;

    public bool IsDone { get; private set; }

    public SetupGame(
        IGameConnection connection,
        IPlayersManager players)
    {
        _connection = connection;
        _players = players;
    }

    public async Task Run(CancellationToken token)
    {
        await SendSetupNotification(token);
        IsDone = true;
    }

    private async Task SendSetupNotification(CancellationToken token)
    {
        await _connection.SendNotification(new GameSetupNotification
        {
            Data = new GameSetupNotification.Setup
            {
                PlayerIdMap = _players.Players.Select(p => new GameSetupNotification.PlayerIdMap
                {
                    PlayerId = p.Id,
                    ShortId = p.ShortId
                }).ToList(),
            }
        });
    }

    public IGameStage? Next()
    {
        return null;
    }
}
