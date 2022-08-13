using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : PickUpItem
{
    protected override void Start()
    {
        base.Start();
        EvaluationForm.Instance.collectTotal++;
        CheckState();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        EvaluationForm.Instance.collectCount++;
        if (other.gameObject.name == "Player")
        {
            SoundManager.Instance.Play(Sound.GetStar);
            //    Instantiate(soundObject, transform.position, Quaternion.identity);
            //    var inventory = other.GetComponent<InventoryHolder>();
            //    if (inventory == null) return;
            //    inventory.InventorySystem.AddToInventory(starInformation.itemData, 2);
            //    ¥[¦å
            //     GameManager.instance.hp += 999f;
            //    Destroy(gameObject);

        }
    }

    void CheckState() 
    {
        
    }
}

