using System.Collections;
using System.Collections.Generic;
using Ubiq.Spawning;
using UnityEngine;
using Ubiq.Messaging;
using Ubiq.Geometry;
using Org.BouncyCastle.Asn1.Tsp;
using Unity.VisualScripting;
using static UnityEngine.UI.GridLayoutGroup;


public class Sync : MonoBehaviour, INetworkSpawnable
{
    public NetworkId NetworkId { get; set; }

    private NetworkContext context;
    private GameObject avatar;

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
        avatar = gameObject;
        SendMessage();
    }

    private void FixedUpdate()
    {

            // 4. Send transform update messages if we are the current 'owner'
            context.SendJson(new Message());
        
    }

    public struct Message
    {
        public PositionRotation pose;
        public GameObject avatar;
    }

    // Update is called once per frame
    private void SendMessage()
    {
        var message = new Message();
        message.pose = Transforms.ToLocal(transform, context.Scene.transform);
        message.avatar = gameObject;
        context.SendJson(message);
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>();
        var pose = Transforms.ToWorld(msg.pose, context.Scene.transform);
        transform.position = pose.position;
        transform.rotation = pose.rotation;
        avatar = msg.avatar;
    }
}
