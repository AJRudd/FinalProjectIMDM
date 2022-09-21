using System;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.HLAPI;
using Niantic.ARDK.Networking.HLAPI.Authority;
using Niantic.ARDK.Networking.HLAPI.Data;
using Niantic.ARDK.Networking.HLAPI.Object;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.Networking.HLAPI.Routing;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;

using UnityEngine;
using UnityEngine.UI;

public class LightshipAdaptation : MonoBehaviour
{

    /// Reference to the StartGame button
    [SerializeField]
    private GameObject startGame = null;

    [SerializeField]
    private Button joinButton = null;

    [SerializeField]
    private FeaturePreloadManager preloadManager = null;

    /// Reference to AR Camera, used for hit test
    [SerializeField]
    private Camera _camera = null;

    /// References to game objects after instantiation
    private GameObject _ball;

    private GameObject _player;
    private GameObject _playingField;
    private GameObject _opponent;

  

    /// HLAPI Networking objects
    private IHlapiSession _manager;

    private IAuthorityReplicator _auth;
    private MessageStreamReplicator<Vector3> _hitStreamReplicator;

    private INetworkedField<string> _scoreText;
    private int _redScore;
    private int _blueScore;
    private INetworkedField<Vector3> _fieldPosition;
    private INetworkedField<byte> _gameStarted;

    /// Cache your location every frame
    private Vector3 _location;

    /// Some fields to provide a lockout upon hitting the ball, in case the hit message is not
    /// processed in a single frame
    private bool _recentlyHit = false;

    private int _hitLockout = 0;
    
    

    private IARNetworking _arNetworking;
    private GamePlay _ballBehaviour;

    private bool _isHost;
    private IPeer _self;

    private bool _gameStart;
    private bool _synced;

    private void Start()
    {
      startGame.SetActive(false);
      ARNetworkingFactory.ARNetworkingInitialized += OnAnyARNetworkingSessionInitialized;

      if (preloadManager.AreAllFeaturesDownloaded())
        OnPreloadFinished(true);
      else
        preloadManager.ProgressUpdated += PreloadProgressUpdated;
    }

    internal void GameStart(bool isHost)
    {
      _isHost = isHost;
      _gameStart = true;

      if (!_isHost)
        return;
    }

    private void PreloadProgressUpdated(FeaturePreloadManager.PreloadProgressUpdatedArgs args)
    {
      if (args.PreloadAttemptFinished)
      {
        preloadManager.ProgressUpdated -= PreloadProgressUpdated;
        OnPreloadFinished(args.FailedPreloads.Count == 0);
      }
    }

    private void OnPreloadFinished(bool success)
    {
      if (success)
        joinButton.interactable = true;
      else
        Debug.LogError("Failed to download resources needed to run AR Multiplayer");
    }

    // When all players are ready, create the game. Only the host will have the option to call this
    public void StartGame()
    {
      
        startGame.SetActive(false);

      //_gameStart = true;
      _gameStarted.Value = Convert.ToByte(true);
    }

    
  
    private void Update()
    {
      if (_manager != null)
        _manager.SendQueuedData();

      if (!_gameStart)
        return;
    }

    // Every updated frame, get our location from the frame data and move the local player's avatar
    private void OnFrameUpdated(FrameUpdatedArgs args)
    {
      _location = MatrixUtils.PositionFromMatrix(args.Frame.Camera.Transform);

      if (_player == null)
        return;

      var playerPos = _player.transform.position;
      playerPos.x = _location.x;
      _player.transform.position = playerPos;
    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
      if (_self.Identifier != args.Peer.Identifier)
      {
        if (args.State == PeerState.Stable)
        {
          _synced = true;

          if (_isHost)
          {
            startGame.SetActive(true);
          }
        }

        return;
      }

      string message = args.State.ToString();
      Debug.Log("We reached state " + message);
    }

    private void OnDidConnect(ConnectedArgs connectedArgs)
    {
      _isHost = connectedArgs.IsHost;
      _self = connectedArgs.Self;

      _manager = new HlapiSession(19244);

      var group = _manager.CreateAndRegisterGroup(new NetworkId(4321));
      _auth = new GreedyAuthorityReplicator("pongHLAPIAuth", group);

      _auth.TryClaimRole(_isHost ? Role.Authority : Role.Observer, () => { }, () => { });

      var authToObserverDescriptor =
        _auth.AuthorityToObserverDescriptor(TransportType.ReliableUnordered);

      _gameStarted = new NetworkedField<byte>("gameStarted", authToObserverDescriptor, group);
    }

    private void OnAnyARNetworkingSessionInitialized(AnyARNetworkingInitializedArgs args)
    {
      _arNetworking = args.ARNetworking;
      _arNetworking.PeerStateReceived += OnPeerStateReceived;

      _arNetworking.ARSession.FrameUpdated += OnFrameUpdated;
      _arNetworking.Networking.Connected += OnDidConnect;
    }

    private void OnDestroy()
    {
      ARNetworkingFactory.ARNetworkingInitialized -= OnAnyARNetworkingSessionInitialized;

      if (_arNetworking != null)
      {
        _arNetworking.PeerStateReceived -= OnPeerStateReceived;
        _arNetworking.ARSession.FrameUpdated -= OnFrameUpdated;
        _arNetworking.Networking.Connected -= OnDidConnect;
      }
    }
    
  }
