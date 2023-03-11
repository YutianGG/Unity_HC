using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadWall : MonoBehaviour
{
    [SerializeField] Vector3 speed = Vector3.zero;

    private void Update()
    {
        //�P�_�O�_�C������
        bool gameOver = false;
        if (gameOver)
            return;
        
        //�P���`�Z��
        float dis = Mathf.Abs(this.transform.position.z - PlayerControl.instance.transform.position.z);
        
        //�Z�����`�L��
        if(dis > 30f)
            this.transform.Translate(speed * Time.deltaTime * 10f);
        else
            this.transform.Translate(speed * Time.deltaTime * 1f);
        this.transform.position =
            new Vector3(this.transform.position.x,
                        Mathf.Lerp(this.transform.position.y, PlayerControl.instance.transform.position.y, Time.deltaTime * 0.3f),
                        this.transform.position.z);

        //�󦺤`�d�򤺹C������
        if(dis < 1f)
        {
            PlayerControl.instance.Injure(999999f);
            gameOver = true;
        }
    }
}
