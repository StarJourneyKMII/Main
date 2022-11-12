using MiProduction.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour, IData
{
    [SerializeField] private ItemData ItemData;
    [SerializeField] private int StackCount = 1;

    private string id;

    private SpriteRenderer sr;
    private CircleCollider2D collider;
    private bool collected = false;

    public int collectPlayerIsGirl { get; private set; }

    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collected == false)
        {
            //TODO: ChangeToFlipState
            collectPlayerIsGirl = collision.GetComponent<Player>().CurrentSex;
            Collect();
        }
    }

    private void Collect()
    {
        collected = true;
        SoundSystem.PlaySFX(Sound.GetStar);
        PlayerCollection.Instance.CollectStar(this);
        sr.enabled = false;
        collider.enabled = false;
    }
    private void UnCollect()
    {
        collected = false;
        sr.enabled = true;
        collider.enabled = true;
    }
    public void Recovery()
    {
        sr.enabled = true;
        collider.enabled = true;
        sr.DOFade(1, 0.3f).From(0).OnComplete(() =>
        {
            collected = false;
        });
    }
    public void SaveData(ref GameData data)
    {
        if(data.starCollected.ContainsKey(id))
        {
            data.starCollected.Remove(id);
        }
        data.starCollected.Add(id, collected);
    }

    public void LoadData(GameData data)
    {
        data.starCollected.TryGetValue(id, out collected);
        if(collected)
        {
            sr.enabled = false;
            PlayerCollection.Instance.CollectStar(this);
        }
        PlayerCollection.Instance.AddStar(this);
    }
}

