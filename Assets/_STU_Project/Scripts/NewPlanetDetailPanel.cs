using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewPlanetDetailPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text planetNameText;
    [SerializeField] private TMP_Text planetDescriptionText;

    private Planet planet;

    public void SetInfo(Planet planet)
    {
        this.planet = planet;
        PlanetData data = planet.data;
        planetNameText.text = data.planetName;
        planetDescriptionText.text = data.description;
    }
    public void OnClickEnter()
    {
        SceneChangeManager.Instance.LoadSceneByName("STJ_Old_Level" + (planet.data.newIndex - 1));
    }
}
