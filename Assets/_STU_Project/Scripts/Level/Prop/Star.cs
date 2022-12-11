using MiProduction.BroAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour, IData
{
    //[SerializeField] private ItemData ItemData;
    //[SerializeField] private int StackCount = 1;

    private string id;

    private SpriteRenderer sr;
    private CircleCollider2D collider;

    public StarData starData;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        PlayerCollection.Instance.AddStar(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && starData.collected == false)
        {
            //TODO: ChangeToFlipState
            starData.collectPlayerSex = collision.GetComponent<Player>().CurrentSex;
            Collect();
        }
    }

    private void Collect()
    {
        starData.collected = true;
        SoundSystem.PlaySFX(Sound.GetStar);
        PlayerCollection.Instance.CollectStar(this);
        sr.enabled = false;
        collider.enabled = false;
    }
    private void UnCollect()
    {
        starData.collected = false;
        sr.enabled = true;
        collider.enabled = true;
    }
    public void Recovery()
    {
        sr.enabled = true;
        collider.enabled = true;
        sr.DOFade(1, 0.3f).From(0).OnComplete(() =>
        {
            starData.collected = false;
        });
    }

    public void SaveData(ref GameData data)
    {
        if (data.CurrentLevelData.stars.ContainsKey(id))
        {
            data.CurrentLevelData.stars.Remove(id);
        }
        data.CurrentLevelData.stars.Add(id, starData);
    }

    public void LoadData(GameData data)
    {
        id = GetComponent<UniqueID>().ID;
        
        if (data.CurrentLevelData.stars != null)
            data.CurrentLevelData.stars.TryGetValue(id, out starData);

        if (!starData.Equals(default(StarData)) && starData.collected)
        {
            sr.enabled = false;
            PlayerCollection.Instance.CollectStar(this);
        }
    }

    private void OnEnable()
    {
        NewGameManager.Instance.OnRestartEvent += UnCollect;
    }

    private void OnDestroy()
    {
        NewGameManager.Instance.OnRestartEvent -= UnCollect;
    }
}

[System.Serializable]
public struct  StarData
{
    public bool collected;
    public PlayerSex collectPlayerSex;
}

