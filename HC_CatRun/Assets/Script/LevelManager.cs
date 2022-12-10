using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] Level = new GameObject[0];
    [SerializeField] GameObject[] Trap = new GameObject[0];
    [SerializeField] float posDis = 100f;
    private void Update()
    {
        Vector3 playerPos = PlayerControl.instance.transform.position;
        if (posDis > Vector3.Distance(playerPos, pos))
            Generate();
    }
    Vector3 pos = Vector3.zero;
    void Generate()
    {
        int LevelRage = Random.Range(0, Level.Length);
        Instantiate(Level[LevelRage], pos, Quaternion.identity);
        int TrapRage = Random.Range(0, Trap.Length);
        Instantiate(Trap[TrapRage], pos, Quaternion.identity);
        switch (LevelRage)
        {
            case 0:
                pos += new Vector3(0f, 0f, 20f);
                break;
            case 1:
                pos += new Vector3(0f, 5f, 20f);
                break;
            case 2:
                pos += new Vector3(0f, -5f, 20f);
                break;
        }
    }
}
