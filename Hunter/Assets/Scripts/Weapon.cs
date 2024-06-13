using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 100;
    public float damage = 10;
    public Sprite sprite;

    private List<GameObject> explosions = new List<GameObject>();
    public GameObject explosion;
    public bool isEquiped;

    [SerializeField]
    private float timeSinceLastShoot = 0;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform muzzleLocation;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        timeSinceLastShoot += Time.deltaTime;
    }

    public void Shoot(EnemyAi enemy, Vector3 impact, float multiplier)
    {
        if (timeSinceLastShoot > cooldown)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, muzzleLocation.position);
            lineRenderer.SetPosition(1, impact);
            MakeExplosion(impact);
            enemy.TakeDamage(damage * multiplier);
            timeSinceLastShoot = 0;
            StartCoroutine(RemoveLineAfterTime((name == "pistoLaser")? 0.1f : 0.3f));
        }
    }

    IEnumerator RemoveLineAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.enabled = false;
    }

    public void MakeExplosion(Vector3 pos)
    {
        GameObject obj = Instantiate(explosion, pos, Quaternion.identity);
        explosions.Add(obj);

        StartCoroutine(InvokeWithArgs(1f, () => DeleteExplosion(obj)));
    }

    IEnumerator InvokeWithArgs(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    private void DeleteExplosion(GameObject del)
    {
        explosions.Remove(del);
        Destroy(del);
    }
}
