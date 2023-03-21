using Backend.Application.Actions.Players.Queries.GetPLayer;
using Backend.Application.Games.Effect.Interfaces;
using Backend.Application.Games.Player.Models;
using Backend.Application.Games.Update.Updates;
using Backend.Domain.Enums;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Player.Interfaces;
public interface IGamePlayer : IPlayerActionsObserver
{
    Guid Id { get; }

    string NickName { get; }

    int ShortId { get; }

    bool Connected { get; }

    bool Ready { get; }

    Point Position { get; set; }

    bool isDead { get; set; }

    bool Visible { get; set; }

    bool Immortal { get; set; }

    bool Blind { get; set; }

    bool UndefEffect { get; set; }

    float Radious { get; set; }

    double Speed { get; set; }

    double TurnSpeed { get; set; }

    string Colour { get; set; }

    Vector Direction { get; set; }

    KeyType KeyPressed { get; set; }

    double AverageGapLength { get; set; }

    double AverageLength { get; set; }

    double GapBegin { get; set; }

    double GapEnd { get; set; }

    PlayerUpdate? CurrentUpdate { get; set; }

    List<PlayerState> LastStates { get; set; }

    int RoundScore { get; set; }

    int TotalScore { get; set; }

    int Place { get; set; }

    double Length { get; set; }

    TimeSpan LiveTime { get; set; }

    IGamePlayer? KilledByPlayer { get; set; }

    void Init(PlayerDto player, int shortId);

    void AddEffect(IEffect<IGamePlayer> effect);

    PlayerUpdate GetOrCreateUpdate();

    void Update();

    void Clear();
}
