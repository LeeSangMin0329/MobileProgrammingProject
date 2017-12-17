using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySynchronizer : MonoBehaviour {

    enum BitField
    {
        Running,
        Shouting,
        Biting,
        Breathing,
        WingStriking,
        Flighting,
        FlightRush,
        FlightDontMove,
        FlightFire,
        Died,
    };

    NetworkView netView;

    Vector3 position;
    Quaternion rotation;
  
    TerrorDragonStatus status;

	// Use this for initialization
	void Start () {
        position = transform.position;
        rotation = transform.rotation;
        status = GetComponent<TerrorDragonStatus>();
        netView = GetComponent<NetworkView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!netView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 5.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);
           
        }
	}

    //@override
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
           
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            
            if(status != null)
            {
                int hp = status.HP;
                int packedFlags = PackStatusFlags();
                stream.Serialize(ref hp);
                stream.Serialize(ref packedFlags);
            }
        }
        else
        {
            stream.Serialize(ref position);
            stream.Serialize(ref rotation);
           
            if(status != null)
            {
                int hp = 0;
                int flags = 0;
                stream.Serialize(ref hp);
                stream.Serialize(ref flags);
                status.HP = hp;
                UnpackStatusBit(flags);
            }
        }
    }

    int PackStatusFlags()
    {
        int packed = 0;

        packed |= status.running ? (1 << (int)BitField.Running) : 0;
        packed |= status.shouting ? (1 << (int)BitField.Shouting) : 0;
        packed |= status.biting ? (1 << (int)BitField.Biting) : 0;
        packed |= status.breathing ? (1 << (int)BitField.Breathing) : 0;
        packed |= status.wingStriking ? (1 << (int)BitField.WingStriking) : 0;
        packed |= status.flighting ? (1 << (int)BitField.Flighting) : 0;
        packed |= status.flightRush ? (1 << (int)BitField.FlightRush) : 0;
        packed |= status.flightDontMove ? (1 << (int)BitField.FlightDontMove) : 0;
        packed |= status.flightFire ? (1 << (int)BitField.FlightFire) : 0;
        packed |= status.died ? (1 << (int)BitField.Died) : 0;

        return packed;
    }

    void UnpackStatusBit(int bit)
    {
        status.running = (bit & (1 << (int)BitField.Running)) != 0;
        status.shouting = (bit & (1 << (int)BitField.Shouting)) != 0;
        status.biting = (bit & (1 << (int)BitField.Biting)) != 0;
        status.breathing = (bit & (1 << (int)BitField.Breathing)) != 0;
        status.wingStriking = (bit & (1 << (int)BitField.WingStriking)) != 0;
        status.flighting = (bit & (1 << (int)BitField.Flighting)) != 0;
        status.flightRush = (bit & (1 << (int)BitField.FlightRush)) != 0;
        status.flightDontMove = (bit & (1 << (int)BitField.FlightDontMove)) != 0;
        status.flightFire = (bit & (1 << (int)BitField.FlightFire)) != 0;
        status.died = (bit & (1 << (int)BitField.Died)) != 0;
    }
}
