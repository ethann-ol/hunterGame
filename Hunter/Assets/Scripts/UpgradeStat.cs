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
    private TextMeshProUGUI textStamPoint;
    [SerializeField]
    private TextMeshProUGUI textRangePoint;
    [SerializeField]
    private TextMeshProUGUI textDamagePoint;

    private void Start()
    {
        scriptPlayer = player.GetComponent<Player>();
    }

    public void AddStamina()
    {
        scriptPlayer.bonusStamina += 1;
        textStamPoint.text = $"You have {scriptPlayer.bonusStamina - 10} point.";
    }

    public void AddRange()
    {
        scriptPlayer.multiplierRange +=1;
        textRangePoint.text = $"You have {scriptPlayer.multiplierRange - 10} point.";
    }

    public void AddDamage()
    {
        scriptPlayer.multiplierDamage += 1;
        textDamagePoint.text = $"You have {scriptPlayer.multiplierDamage - 10} point.";
    }
}
