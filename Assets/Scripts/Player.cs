using System;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player localInstance;
    public static event EventHandler OnAnyPlayerSpawned;

    public override void OnNetworkSpawn()
    {
        Debug.Log(NetworkObject.OwnerClientId);
        localInstance = this;
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }
}
