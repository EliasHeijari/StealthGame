using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        Player.OnAnyPlayerSpawned -= Player_OnAnyPlayerSpawned;
        Player.localInstance.transform.position = transform.position;
    }
}
