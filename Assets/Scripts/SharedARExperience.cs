using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class SharedARExperience : MonoBehaviour
{
    private IARNetworking _arNetworking;

    private IMultipeerNetworking _networking;

    private IARSession _session;
    public InputField SessionIDField;
    public GameObject PeerPoseIndicator;
    private Dictionary<IPeer, GameObject> _poseIndicatorDict = new Dictionary<IPeer, GameObject>();

    private void Awake()
    {
        ARNetworkingFactory.ARNetworkingInitialized += OnInitialized;
    }

    private void OnInitialized(AnyARNetworkingInitializedArgs args)
    {
        _arNetworking = args.ARNetworking;
        _session = _arNetworking.ARSession;
        _networking = _arNetworking.Networking;
        _session.Ran += OnSessionRan;
        _networking.Connected += OnNetworkConnected;
        _arNetworking.PeerStateReceived += OnPeerStateRecieved;
        _arNetworking.PeerPoseReceived += OnPeerPoseReceived;
    }

    public void CreateAndRunSharedAR()
    {
        /*
        // Create Mock in editor. LiveDevice on Mobile(Android, IOS)
        _arNetworking = ARNetworkingFactory.Create();
        _networking = _arNetworking.Networking;
        _session = _arNetworking.ARSession;
        
        // The configuration that the AR session will be run with
        var worldTrackingConfig = ARWorldTrackingConfigurationFactory.Create();
        worldTrackingConfig.WorldAlignment = WorldAlignment.Gravity;
        worldTrackingConfig.IsAutoFocusEnabled = true;
        
        // Creates CVC synchronization pipeline
        worldTrackingConfig.IsSharedExperienceEnabled = true;
        
        // Run the session, listen to the event
        _session.Run(worldTrackingConfig);
        _session.Ran += OnSessionRan;
        
        // Assume that this is not null
        var sessionID = SessionIDField.text;
        var sessionIdAsByte = Encoding.UTF8.GetBytes(sessionID);
        
        // Joining the Network Session
        _networking.Join(sessionIdAsByte);
        
        // Listening to the network events
        _networking.Connected += OnNetworkConnected;
        _arNetworking.PeerStateReceived += OnPeerStateRecieved;
        _arNetworking.PeerPoseReceived += OnPeerPoseReceived;
        */
    }

    private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log("AR session ran");
    }

    private void OnNetworkConnected(ConnectedArgs args)
    {
        Debug.LogFormat("Networking joined, peerID: {0}, isHost {1}", args.Self, args.IsHost);
    }

    private void OnPeerStateRecieved(PeerStateReceivedArgs args)
    {
        Debug.LogFormat("Peer {0} is at state: {1}", args.Peer, args.State);
    }

    private void OnDestroy()
    {
        ARNetworkingFactory.ARNetworkingInitialized -= OnInitialized;
    }

    private void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        if (!_poseIndicatorDict.ContainsKey(args.Peer))
        {
            _poseIndicatorDict.Add(args.Peer, Instantiate(PeerPoseIndicator));
        }

        GameObject poseIndicator;
        if (_poseIndicatorDict.TryGetValue(args.Peer, out poseIndicator))
            poseIndicator.transform.position = args.Pose.ToPosition() + new Vector3(0, 0, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        Awake();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
