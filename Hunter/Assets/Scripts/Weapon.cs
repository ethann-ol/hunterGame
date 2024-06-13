using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 100;
    public float damage = 10;
    public Sprite sprite;

    private List<GameObject> explosions = new List<GameObject>();
    public GameObject explosion;
    public bool isEquiped;

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
