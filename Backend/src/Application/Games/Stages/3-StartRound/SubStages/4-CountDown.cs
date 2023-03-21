using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Stages.Interfaces;

namespace Backend.Application.Games.Stages.StartRound.SubStages;
public class CountDown : IGameStage
{
    private readonly IGameConnection _gameConnection;
    private readonly IGameTime _time;

    private readonly int _maxSeconds = 3;
    private int _secondsPassed = 0;

    private bool CountDownEnded => _secondsPassed == _maxSeconds;

    public bool IsDone => CountDownEnded;

    public CountDown(
        IGameConnection gameConnection,
        IGameTime time)
    {
        _gameConnection = gameConnection;
        _time = time;
    }

    public async Task Run(CancellationToken token)
    {
        await CheckCountDown();
    }

    private async Task CheckCountDown()
    {
        if (_time.StageTime.TotalSeconds > _secondsPassed)
        {
            await SendCountDownNotification(_maxSeconds - _secondsPassed);
            _secondsPassed++;
        }
    }

    private async Task SendCountDownNotification(int coundDown)
    {
        await _gameConnection.SendNotification(new CountDownNotification { Data = coundDown });
    }

    public IGameStage? Next()
    {
        return null;
    }
}
