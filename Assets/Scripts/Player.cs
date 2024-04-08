using System;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player localInstance;
    public static event EventHandler OnAnyPlayerSpawned;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            localInstance = this;
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void OnDestroy()
    {
        OnAnyPlayerSpawned = null;
    }
}
