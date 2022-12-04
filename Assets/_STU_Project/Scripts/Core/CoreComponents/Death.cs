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

    private bool isDeading = false;

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
        if (isDeading) return;

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

        isDeading = true;
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
            if (player.CurrentSex != reburnStar.starData.collectPlayerSex)
                player.FlipSexNoShow();
            core.transform.parent.position = reburnStar.transform.position;
        }
        else
        {
            if (player.CurrentSex != PlayerSex.Girl)
                player.FlipSexNoShow();
            core.transform.parent.position = new Vector3(-32, 2.8f, 0);
        }

        for (int i = playerCollection.CollectStarCount - 1; i > reburnStarIndex; i--)
        {
            StartCoroutine(ReburnBlend(playerCollection.collectStars[i]));
            playerCollection.collectStars.RemoveAt(i);
            UIManager.Instance.RefreshStarBar();
            yield return new WaitForSeconds(1.5f);
        }
        cameraTarget.SetFollowTargetPos(core.transform.parent.position);
        cameraTarget.StartFollowPlayer();
        player.Show();
        dieStarCount++;
        isDeading = false;
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