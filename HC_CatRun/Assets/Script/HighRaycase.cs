using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighRaycase : MonoBehaviour
{
    [SerializeField] Vector3 posEnd = Vector3.zero;
    private void Start()
    {
        this.transform.position += Vector3.up * 10f;
    }

    int t = 0;
    bool check = false;

    private void FixedUpdate()
    {
        t++;
        if(t > 2 && check == false)
        {
            check = true;
            RaycastHit raycastHit;
            bool hit_ray =
                Physics.Raycast(this.transform.position, Vector3.down, out raycastHit);
            if (hit_ray)
                this.transform.position = raycastHit.point + posEnd;
        }
    }
}
