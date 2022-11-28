using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollection : MonoBehaviour
{
    private static PlayerCollection instance;
    public static PlayerCollection Instance 
    { 
        get { return instance; }
        private set { instance = value; }
    }
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

    public event Action OnCollected;

    private void Awake()
    {
        if (instance == null)
            instance = this;
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
