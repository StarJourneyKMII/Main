using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollection : MonoBehaviour
{
    public static PlayerCollection Instance { get; private set; }
    public int CollectStarCount
    {
        get { return collectStars.Count; }
    }
    public int StarTotal
    {
        get { return activeStars.Count; }
    }

    public HashSet<Star> activeStars = new HashSet<Star>();
    public List<Star> collectStars = new List<Star>();

    public UnityAction OnCollected;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        OnCollected += UIManager.Instance.RefreshStarBar;
    }

    public void AddStar(Star star)
    {
        activeStars.Add(star);
    }

    public void CollectStar(Star star)
    {
        collectStars.Add(star);
        OnCollected?.Invoke();
    }
}
