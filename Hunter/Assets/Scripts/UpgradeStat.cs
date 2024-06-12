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
        scriptPlayer.bonusClimbSpeed += 1;
        textClimbSpeedPoint.text = $"({scriptPlayer.bonusClimbSpeed})";
    }

    public void AddStamina()
    {
        scriptPlayer.bonusStamina += 1;
        textStamPoint.text = $"({scriptPlayer.bonusStamina})";
    }

    public void AddRange()
    {
        scriptPlayer.multiplierRange +=1;
        textRangePoint.text = $"({scriptPlayer.multiplierRange})";
    }

    public void AddDamage()
    {
        scriptPlayer.multiplierDamage += 1;
        textDamagePoint.text = $"({scriptPlayer.multiplierDamage})";
    }

}
