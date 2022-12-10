using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    float posDis = 100f;
    bool gone = false;
    private void Update()
    {
        Vector3 playerPos = PlayerControl.instance.transform.position;
        float dis = Vector3.Distance(this.transform.position, playerPos);
        //�P�_�O�_��L
        if (dis < 20f)
            gone = true;
        //�P�_�Z��
        if (posDis < dis && gone == true)
            Destroy(this.gameObject);
    }
}
