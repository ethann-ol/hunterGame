using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeStat : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Player scriptPlayer;

    [SerializeField]
    private TextMeshProUGUI textClimbSpeedPoint;
    [SerializeField]
    private TextMeshProUGUI textStamPoint;
    [SerializeField]
    private TextMeshProUGUI textRangePoint;
    [SerializeField]
    private TextMeshProUGUI textDamagePoint;

    private void Start()
    {
        scriptPlayer = player.GetComponent<Player>();
    }
    
    public void AddClimbSpeed()
    {
        Upgrade("Climb");
    }

    public void AddStamina()
    {
        Upgrade("Stamina");
    }

    public void AddRange()
    {
        Upgrade("Range");
    }

    public void AddDamage()
    {
        Upgrade("Damage");
    }

    private void Upgrade(string stat)
    {
        if (scriptPlayer.pointAvailable > 0)
        {
            switch (stat)
            {
                case "Climb":
                    scriptPlayer.bonusClimbSpeed += .1f;
                    textClimbSpeedPoint.text = $"({scriptPlayer.bonusClimbSpeed})";
                    break;
                case "Stamina":
                    scriptPlayer.bonusStamina += .1f;
                    textStamPoint.text = $"({scriptPlayer.bonusStamina})";
                    break;
                case "Range":
                    scriptPlayer.multiplierRange += .1f;
                    textRangePoint.text = $"({scriptPlayer.multiplierRange})";
                    break;
                case "Damage":
                    scriptPlayer.multiplierDamage += .1f;
                    textDamagePoint.text = $"({scriptPlayer.multiplierDamage})";
                    break;
            }
            scriptPlayer.pointAvailable--;
        }
    }
}
