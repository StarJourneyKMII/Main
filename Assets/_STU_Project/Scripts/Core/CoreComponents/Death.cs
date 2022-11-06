using System;
using System.Collections;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticles;

    private ParticleManager ParticleManager =>
        particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    
    private ParticleManager particleManager;

    private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
    private Stats stats;

    private Player player;
    private PlayerCollection playerCollection;
    private CameraTarget cameraTarget;

    private int dieStarCount;
    private int maxStarCount;

    protected override void Awake()
    {
        base.Awake();
        playerCollection =core.GetComponentInParent<PlayerCollection>();
        player = core.GetComponentInParent<Player>();
        cameraTarget = FindObjectOfType<CameraTarget>();
    }
    private void Start()
    {
        dieStarCount = stats.StartStarCount;
        maxStarCount = stats.MaxStarCount;
    }
    public void Die()
    {
        foreach (var particle in deathParticles)
        {
            ParticleManager.StartParticles(particle);
        }

        if (dieStarCount >= maxStarCount || PlayerCollection.Instance.CollectStarCount < dieStarCount)
        {
            NewGameManager.Instance.GameOver();
        }
        else
        {
            StartCoroutine(Reborn());
        }
        //core.transform.parent.gameObject.SetActive(false);
    }

    private IEnumerator Reborn()
    {
        cameraTarget.StopFollowPlayer();
        player.Hide();

        int reburnStarIndex = playerCollection.CollectStarCount - dieStarCount - 1;
        if(reburnStarIndex >= 0)
        {
            Star reburnStar = playerCollection.collectStars[reburnStarIndex];
            if (player.CurrentSex == reburnStar.collectPlayerIsGirl)
                player.FlipSex();
            core.transform.parent.position = reburnStar.transform.position;
        }
        else
        {
            if (player.CurrentSex != 1)
                player.FlipSex();
            core.transform.parent.position = new Vector3(-32, 2.8f, 0);
        }

        for (int i = playerCollection.CollectStarCount - 1; i > reburnStarIndex; i--)
        {
            StartCoroutine(ReburnBlend(playerCollection.collectStars[i]));
            playerCollection.collectStars.RemoveAt(i);
            yield return new WaitForSeconds(1.5f);
        }
        cameraTarget.SetFollowTargetPos(core.transform.parent.position);
        cameraTarget.StartFollowPlayer();
        player.Show();
        dieStarCount++;
    }

    private IEnumerator ReburnBlend(Star star)
    {
        cameraTarget.SetFollowTargetPos(star.transform.position);
        yield return new WaitForSeconds(0.3f);
        star.Recovery();
    }

    private void OnEnable()
    {
        Stats.OnHealthZero += Die;
    }

    private void OnDisable()
    {
        Stats.OnHealthZero -= Die;
    }
}