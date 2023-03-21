using Backend.Application.Actions.Players.Queries.GetPLayer;
using Backend.Application.Actions.Players.Queries.GetPlayers;
using Backend.Application.Games.Board.Interfaces;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Update.Updates;
using Backend.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Player;

public class PlayersManager : IPlayersManager
{
    private readonly IGameBoard _board;
    private readonly IGenerator _generator;
    private readonly IPlayerMover _playerMover;
    private readonly IGameTime _gameTime;
    private readonly IMediator _mediator;
    private readonly IGameContext _gameContext;
    private readonly IPlayerGapGenerator _playerGapGenerator;
    private readonly ICollisionDetector _collisionDetector;
    private readonly IServiceProvider _serviceProvider;

    private List<IGamePlayer> _players = new();
    private int _nextPlayerPlace = 0;

    public PlayersManager(
        IGameBoard board,
        IGenerator generator,
        IPlayerMover playerMover,
        IGameTime gameTime,
        IMediator mediator,
        IGameContext gameContext,
        IPlayerGapGenerator playerGapGenerator,
        ICollisionDetector collisionDetector,
        IServiceProvider serviceProvider)
    {
        _board = board;
        _generator = generator;
        _playerMover = playerMover;
        _gameTime = gameTime;
        _mediator = mediator;
        _gameContext = gameContext;
        _playerGapGenerator = playerGapGenerator;
        _collisionDetector = collisionDetector;
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<IGamePlayer> Players => _players;

    public IEnumerable<IGamePlayer> AlivePlayers => _players.Where(p => !p.isDead);

    public async Task Init(CancellationToken token)
    {
        var playersData = await _mediator.Send(new GetPlayersQuery { GameId = _gameContext.Game.Id }, token);
        if (playersData is null || playersData.Count() < 2)
        {
            throw new ArgumentNullException(nameof(playersData));
        }
        _players = playersData.Select((data, index) => CreatePlayerFromData(data, index)).ToList();
    }
    private IGamePlayer CreatePlayerFromData(PlayerDto data, int index)
    {
        var player = _serviceProvider.GetRequiredService<IGamePlayer>();
        player.Init(data, index);
        return player;
    }

    public IGamePlayer? GetPlayer(Guid playerId) => _players.FirstOrDefault(p => p.Id == playerId);

    public void Reset()
    {
        _nextPlayerPlace = _players.Count;
        Clear();
        SeedPlayers();
        Update();
    }

    public void Clear() => _players.ForEach(p => p.Clear());

    public void SeedPlayers() => _players.ForEach(p => SeedPlayer(p));


    public void SeedPlayer(IGamePlayer player)
    {
        player.Direction = _generator.GenerateVector();
        Point position;
        do
        {
            position = _generator.GeneratePoint(player.Radious * 3);
        } while (_collisionDetector.PlayerCollideWithPlayers(player, _players) != null);
        player.Position = position;
    }


    public IEnumerable<PlayerUpdate> GetUpdates() => _players.Select(p => p.CurrentUpdate).Where(d => d is not null).Cast<PlayerUpdate>();

    public void Update()
    {
        foreach (var player in AlivePlayers)
        {
            player.Update();
            _playerGapGenerator.Check(player);
            if (_playerMover.Move(player))
            {
                GenerateMoveUpdate(player);
            }
            if (CheckIfDied(player))
            {
                GenerateScoreUpdate(player);
            }
        }
        if(_nextPlayerPlace == 1)
        {
            var winner = AlivePlayers.First();
            UpdateScoreAndStats(winner);
            GenerateScoreUpdate(winner);
        }
    }

    private void GenerateMoveUpdate(IGamePlayer player)
    {
        var update = player.GetOrCreateUpdate();
        update.X = player.Position.X;
        update.Y = player.Position.Y;
        update.R = player.Radious;
        update.C = player.Colour;
        update.V = player.Visible;
        update.B = player.Blind;
        update.A = player.UndefEffect;
    }

    private void GenerateScoreUpdate(IGamePlayer player)
    {
        var update = player.GetOrCreateUpdate();
        update.S = player.TotalScore;
    }

    private bool CheckIfDied(IGamePlayer player)
    {
        if (_collisionDetector.PlayerCollideWithBoard(player, _board))
        {
            player.isDead = true;
        }
        else
        {
            player.KilledByPlayer = _collisionDetector.PlayerCollideWithPlayers(player, _players);
            player.isDead = player.KilledByPlayer != null;
        }
        if (player.isDead)
        {
            UpdateScoreAndStats(player);
        }
        return player.isDead;
    }

    private void UpdateScoreAndStats(IGamePlayer player)
    {
        player.Place = _nextPlayerPlace;
        player.RoundScore = _players.Count - player.Place + 1;
        player.TotalScore += player.RoundScore;
        player.LiveTime = _gameTime.StageTime;
        _nextPlayerPlace--;
    }
}