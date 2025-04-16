using System;
using UnityEngine;

public class NPCUI : MonoBehaviour
{
    public Transform alertAnchor;
    private GameObject currentIcon;
    private float alertDuration = 5.0f;
    public GameObject alertIconPrefab;
    public GameObject notedIconPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowAlertIcon()
    {
        if (currentIcon != null) Destroy(currentIcon);

        currentIcon = Instantiate(alertIconPrefab, alertAnchor.position, Quaternion.identity, alertAnchor);

        // Optional: distruggi l’icona dopo un po’
        Destroy(currentIcon, alertDuration);
    }

    internal void ShowNotedIcon()
    {
         if (currentIcon != null) Destroy(currentIcon);

        currentIcon = Instantiate(notedIconPrefab, alertAnchor.position, Quaternion.identity, alertAnchor);

        // Optional: distruggi l’icona dopo un po’
        Destroy(currentIcon, alertDuration);
    }
}
