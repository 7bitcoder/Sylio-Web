using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Player.Models;
using Backend.Domain.Enums;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Player.Helpers;
internal class PlayerMover : IPlayerMover
{
    private readonly int DIFF = 3;
    private readonly IGameTime _gameTime;

    public PlayerMover(IGameTime gameTime)
    {
        _gameTime = gameTime;
    }
    public bool Move(IGamePlayer player)
    {
        UpdateDirection(player);
        UpdatePosition(player);
        return UpdateStates(player);
    }

    private void UpdatePosition(IGamePlayer player)
    {
        var speed = player.Speed * _gameTime.LastFrameTime.TotalMilliseconds;
        var newX = player.Position.X + speed * player.Direction.W;
        var newY = player.Position.Y + speed * player.Direction.H;

        player.Position = new Point((float)newX, (float)newY);
    }

    private bool UpdateStates(IGamePlayer player)
    {
        var lastPosition = player.LastStates.LastOrDefault()?.Position ?? new Point(-1000, -1000);
        if (Math.Abs(player.Position.X - lastPosition.X) > DIFF || Math.Abs(player.Position.Y - lastPosition.Y) > DIFF)
        {
            var state = new PlayerState(player.Visible, player.Position, player.Radious, player.Colour);
            player.LastStates.Add(state);
            UpdateLength(player);
            return true;
        }
        return false;
    }

    private void UpdateLength(IGamePlayer player)
    {
        if (player.LastStates.Count < 2)
        {
            return;
        }
        player.Length += DIFF;
    }

    private void UpdateDirection(IGamePlayer player)
    {
        var angle = player.KeyPressed switch
        {
            KeyType.Left => -_gameTime.LastFrameTime.TotalMilliseconds * player.TurnSpeed,
            KeyType.Right => _gameTime.LastFrameTime.TotalMilliseconds * player.TurnSpeed,
            _ => 0,
        };

        if (angle != 0)
        {
            var newDirX = player.Direction.W * Math.Cos(angle) - player.Direction.H * Math.Sin(angle);
            var newDirY = player.Direction.W * Math.Sin(angle) + player.Direction.H * Math.Cos(angle);
            player.Direction = new Vector((float)newDirX, (float)newDirY);
        }
    }
}
