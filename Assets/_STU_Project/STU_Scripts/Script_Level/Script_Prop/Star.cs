using MiProduction.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : PickUpItem
{
    protected override void Start()
    {
        base.Start();
        FindObjectOfType<EvaluationForm>().collectTotal++;
        CheckState();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        FindObjectOfType<EvaluationForm>().collectCount++;
        if (other.gameObject.name == "Player")
        {
            //SoundManager.Instance.PlaySFX(Sound.GetStar);
            //    Instantiate(soundObject, transform.position, Quaternion.identity);
            //    var inventory = other.GetComponent<InventoryHolder>();
            //    if (inventory == null) return;
            //    inventory.InventorySystem.AddToInventory(starInformation.itemData, 2);
            //    �[��
            //     GameManager.instance.hp += 999f;
            //    Destroy(gameObject);

        }
    }

    void CheckState() 
    {
        
    }
}

