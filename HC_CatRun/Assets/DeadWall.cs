using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadWall : MonoBehaviour
{
    [SerializeField] Vector3 speed = Vector3.zero;

    private void Update()
    {
        //判斷是否遊戲結束
        bool gameOver = false;
        if (gameOver)
            return;
        
        //與死亡距離
        float dis = Mathf.Abs(this.transform.position.z - PlayerControl.instance.transform.position.z);
        
        //距離死亡過遠
        if(dis > 30f)
            this.transform.Translate(speed * Time.deltaTime * 10f);
        else
            this.transform.Translate(speed * Time.deltaTime * 1f);
        this.transform.position =
            new Vector3(this.transform.position.x,
                        Mathf.Lerp(this.transform.position.y, PlayerControl.instance.transform.position.y, Time.deltaTime * 0.3f),
                        this.transform.position.z);

        //於死亡範圍內遊戲結束
        if(dis < 1f)
        {
            PlayerControl.instance.Injure(999999f);
            gameOver = true;
        }
    }
}
