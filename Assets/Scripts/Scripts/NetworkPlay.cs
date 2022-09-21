using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;
using Niantic.ARDKExamples.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlay : MonoBehaviour
{

    public InputField SessionIDField;
    public GameObject PeerPoseIndicator;
    
    private IARNetworking _arNetworking;
    private IMultipeerNetworking _networking;
    private IARSession _session;

    private Dictionary<IPeer, GameObject> _PoseIndicatorDict = new Dictionary<IPeer, GameObject>();

    private void CreateAndRunSharedAR()
    {
        _arNetworking = ARNetworkingFactory.Create();

        _networking = _arNetworking.Networking;
        _session = _arNetworking.ARSession;

        var worldTrackingConfig = ARWorldTrackingConfigurationFactory.Create();
        worldTrackingConfig.WorldAlignment = WorldAlignment.Gravity;
        worldTrackingConfig.IsAutoFocusEnabled = true;

        worldTrackingConfig.IsSharedExperienceEnabled = true;
        
        _session.Run(worldTrackingConfig);
        _session.Ran += OnSessionRan;

        var sessionID = SessionIDField.text;

        var sessionIdAsByte = Encoding.UTF8.GetBytes(sessionID);
        
        _networking.Join(sessionIdAsByte);

        _networking.Connected += OnNetworkConnected;

        _arNetworking.PeerStateReceived += OnPeerStateReceived;
        _arNetworking.PeerPoseReceived += OnPeerPoseReceived;
    }

    private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log("AR Session Ran");
    }

    private void OnNetworkConnected(ConnectedArgs args)
    {
        Debug.LogFormat("Networking joined, peer Id: {0}, isHost: {1}", args.Self, args.IsHost);
    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        Debug.LogFormat("Peer {0} is at state {1}", args.Peer, args.State);
    }

    private void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        if(!_PoseIndicatorDict.ContainsKey(args.Peer))
            _PoseIndicatorDict.Add(args.Peer, Instantiate(PeerPoseIndicator));
        GameObject poseIndicator;
        if (_PoseIndicatorDict.TryGetValue(args.Peer, out poseIndicator))
            poseIndicator.transform.position = args.Pose.ToPosition() + new Vector3(0, 0, -.5f);
    }

    private void OnDestroy()
    {
        _session?.Dispose();
        _networking?.Dispose();
        _arNetworking?.Dispose();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateAndRunSharedAR();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
