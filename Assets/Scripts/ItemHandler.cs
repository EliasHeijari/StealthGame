using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHandler : NetworkBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private float throwForce = 300f;
    private GameObject item;

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
        if (this.item == itemGO) return;

        Item item = itemGO.GetComponent<Item>();
        if (item.isRigidbodyEnable)
        {
            item.DisableRigidbody();
        }

        if (this.item == null)
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

    private void SetItemToHand(GameObject item)
    {
        item.GetComponent<FollowTransform>().target = hand;
        this.item = item;
    }

    private void DropItem()
    {
        Debug.Log("Drop Item");
        if (item == null) return;
        item.GetComponent<FollowTransform>().target = null;
        item.GetComponent<Item>().EnableRigidbody();
        item.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        item = null;
    }
}
