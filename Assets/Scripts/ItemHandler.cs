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
    private GameObject ItemGameObject;
    private GameObject itemGameObject 
    { 
        get { return ItemGameObject; }
        set {
            NetworkObject itemNetworkObject = value.GetComponent<NetworkObject>();
            SetItemGameObjectServerRpc(itemNetworkObject);
        }
    }

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
            DisableItemRigidbodyServerRpc(item.GetNetworkObject());
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
        SetItemGameObjectNullServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetItemGameObjectNullServerRpc()
    {
        SetItemGameObjectNullClientRpc();
    }
    [ClientRpc]
    private void SetItemGameObjectNullClientRpc()
    {
        ItemGameObject = null;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetItemGameObjectServerRpc(NetworkObjectReference itemNetworkObjectReference)
    {
        itemNetworkObjectReference.TryGet(out NetworkObject itemNetworkObject);
        SetItemGameObjectClientRpc(itemNetworkObject);
    }
    [ClientRpc]
    private void SetItemGameObjectClientRpc(NetworkObjectReference itemNetworkObjectReference)
    {
        itemNetworkObjectReference.TryGet(out NetworkObject itemNetworkObject);
        ItemGameObject = itemNetworkObject.gameObject;
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

    [ServerRpc(RequireOwnership = false)]
    private void DisableItemRigidbodyServerRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject itemNetworkObject);
        DisableItemRigidbodyClientRpc(itemNetworkObject);
    }

    [ClientRpc]
    private void DisableItemRigidbodyClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject itemNetworkObject);
        Item item = itemNetworkObject.GetComponent<Item>();
        item.DisableRigidbody();
    }
}
