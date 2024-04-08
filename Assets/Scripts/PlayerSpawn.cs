using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        if (Player.localInstance != null)
        {
            Player.localInstance.transform.position = transform.position;
        }
        else
        {
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        if (Player.localInstance != null)
        {
            Player.localInstance.transform.position = transform.position;
        }
    }
}
