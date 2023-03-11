using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] GameObject stoneEffect = null;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(stoneEffect != null)
            Instantiate(stoneEffect, this.transform.position, Quaternion.identity);
            SaveManager.instance.playerData.stone++;
            // �����ۤv��������
            Destroy(this.transform.parent.gameObject);
        }
 
        
    }
}
