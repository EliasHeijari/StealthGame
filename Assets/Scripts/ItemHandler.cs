using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHandler : NetworkBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private float throwForce = 300f;
    private GameObject itemGameObject;

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }
    }

    public void SetItem(GameObject itemGO)
    {
        if (itemGameObject == itemGO) return;

        Item item = itemGO.GetComponent<Item>();
        if (item.isRigidbodyEnable)
        {
            item.DisableRigidbody();
        }

        if (itemGameObject == null)
        {
            SetItemToHand(itemGO);
        }
        else
        {
            // holding item and trying to pick up new item
            DropItem();
            SetItemToHand(itemGO);
        }
    }

    private void SetItemToHand(GameObject itemGO)
    {
        Item item = itemGO.GetComponent<Item>();
        item.followTransform.target = hand;
        itemGameObject = itemGO;
    }

    private void DropItem()
    {
        if (itemGameObject == null) return;
        Item item = itemGameObject.GetComponent<Item>();
        DropItemServerRpc(item.GetNetworkObject());
        itemGameObject = null;
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropItemServerRpc(NetworkObjectReference networkObjectReference, ServerRpcParams serverRpcParams = default)
    {
        
        networkObjectReference.TryGet(out NetworkObject itemNetworkObject);
        DropItemClientRpc(itemNetworkObject);

        Rigidbody itemRigidbody = itemNetworkObject.GetComponent<Rigidbody>();

        NetworkObject senderNetworkObject = NetworkManager.Singleton.ConnectedClients[serverRpcParams.Receive.SenderClientId].PlayerObject;
        Transform senderPlayerTransform = senderNetworkObject.transform;

        itemRigidbody.AddForce(senderPlayerTransform.forward * throwForce);
    }

    [ClientRpc]
    private void DropItemClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject itemNetworkObject);
        Item item = itemNetworkObject.GetComponent<Item>();

        item.followTransform.target = null;
        item.EnableRigidbody();
    }
}
