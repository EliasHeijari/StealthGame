using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private float throwForce = 300f;
    private GameObject item;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }
    }

    public void SetItem(GameObject item)
    {
        if (this.item == item) return;

        if (item.GetComponent<Rigidbody>() != null)
        {
            Destroy(item.GetComponent<Rigidbody>());
        }

        if (this.item == null)
        {
            SetItemToHand(item);
        }
        else
        {
            // holding item and trying to pick up new item
            DropItem();
            SetItemToHand(item);
        }
    }

    private void SetItemToHand(GameObject item)
    {
        item.GetComponent<FollowTransform>().target = hand;
        this.item = item;
    }

    private void DropItem()
    {
        if (item == null) return;
        item.transform.AddComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        item = null;
    }
}
