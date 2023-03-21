using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Player.Helpers;
public class PlayerGapGenerator : IPlayerGapGenerator
{
    public bool Check(IGamePlayer player)
    {
        if (player.Length < player.GapBegin)
        {
            return player.Visible = true;
        }
        if (player.Length < player.GapEnd)
        {
            return player.Visible = false;
        }
        GenerateNextGap(player);
        return player.Visible = true;
    }

    private void GenerateNextGap(IGamePlayer player)
    {
        player.GapBegin = player.Length + Randomize(player.AverageLength, 0.4);

        player.GapEnd = player.GapBegin + Randomize(player.AverageGapLength, 0.4);
    }

    private double Randomize(double value, double randomize)
    {
        var range = value * randomize;
        return value - range + Random.Shared.NextDouble() * 2 * range;
    }
}
