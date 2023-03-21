using System.Runtime.CompilerServices;
using Backend.Application.Actions.Players.Queries.GetPLayer;
using Backend.Application.Games.Effect.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Player.Models;
using Backend.Application.Games.Update.Updates;
using Backend.Domain.Enums;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Player;
public class GamePlayer : IGamePlayer
{
    private readonly IPlayerEffectsManager _effects;
    private PlayerDto? _playerData;

    public GamePlayer(IPlayerEffectsManager effects)
    {
        _effects = effects;
    }

    public Guid Id => _playerData!.Id;

    public string NickName => _playerData!.Name;

    public int ShortId { get; set; }

    private bool _connected;
    public bool Connected
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => _connected;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => _connected = value;
    }

    private bool _ready;
    public bool Ready
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => _ready;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => _ready = value;
    }

    public Point Position { get; set; }

    public bool Visible { get; set; }

    public float Radious { get; set; }

    public string Colour { get; set; } = default!;

    public bool isDead { get; set; }

    public double Speed { get; set; }

    public double TurnSpeed { get; set; }

    public bool Immortal { get; set; }

    public bool Blind { get; set; }

    public bool UndefEffect { get; set; }

    public Vector Direction { get; set; }

    private KeyType _keyPressed;
    public KeyType KeyPressed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => _keyPressed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => _keyPressed = value;
    }

    public double AverageGapLength { get; set; }

    public double AverageLength { get; set; }

    public double GapBegin { get; set; }

    public double GapEnd { get; set; }

    public PlayerUpdate? CurrentUpdate { get; set; }

    public List<PlayerState> LastStates { get; set; } = new();

    public int RoundScore { get; set; }

    public int TotalScore { get; set; }

    public int Place { get; set; }

    public double Length { get; set; }

    public TimeSpan LiveTime { get; set; }

    public IGamePlayer? KilledByPlayer { get; set; }

    public void Init(PlayerDto player, int shortId)
    {
        ShortId = shortId;
        _playerData = player;
        ResetState();
    }

    public void AddEffect(IEffect<IGamePlayer> effect)
    {
        _effects.Add(effect);
    }

    public void Update()
    {
        ResetState();
        UndefEffect = _effects.Apply(this);
    }

    public PlayerUpdate GetOrCreateUpdate()
    {
        if (CurrentUpdate is null)
        {
            CurrentUpdate = new PlayerUpdate
            {
                Id = ShortId,
            };
        }
        return CurrentUpdate;
    }

    public void OnKeyPressed(KeyType key)
    {
        KeyPressed |= key;
    }

    public void OnKeyReleased(KeyType key)
    {
        KeyPressed &= ~key;
    }

    public void OnConnected()
    {
        Connected = true;
    }

    public void OnDisconnected()
    {
        Connected = false;
    }

    public void OnReady()
    {
        Ready = true;
    }

    public void Clear()
    {
        ResetState();
        _effects.Clear();
        Position = new();
        isDead = false;
        Ready = false;
        KeyPressed = KeyType.None;
        LastStates = new();
        Direction = new();
        LiveTime = TimeSpan.Zero;
        RoundScore = 0;
        Place = 0;
        Length = 0;
        KilledByPlayer = null;
        GapBegin = 0;
        GapEnd = 0;
    }

    private void ResetState()
    {
        CurrentUpdate = null;
        Radious = 10;
        Colour = _playerData!.Colour;
        Speed = 0.1;
        Visible = true;
        Blind = false;
        TurnSpeed = 0.003;
        Immortal = false;
        AverageGapLength = 50;
        AverageLength = 300;
    }
}
