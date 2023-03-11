using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] Level = new GameObject[0];
    [SerializeField] GameObject[] Trap = new GameObject[0];
    [SerializeField] GameObject stone = null;
    [SerializeField] float posDis = 100f;
    private void Update()
    {
        Vector3 playerPos = PlayerControl.instance.transform.position;
        if (posDis > Vector3.Distance(playerPos, pos) && running == false)
        {
            StartCoroutine(Generate());
        }
    }
    Vector3 pos = Vector3.zero;
    [SerializeField]int maxStore = 3;
    
    // 判斷是否執行中
    bool running = false;
    IEnumerator Generate()
    {
        running = true;
        //等待一幀
        //yield return null; 
        //等待一秒
        //yield return new WaitForSeconds(1f);

        //生成地板
        int LevelRage = Random.Range(0, Level.Length);
        Instantiate(Level[LevelRage], pos, Quaternion.identity);
        //生成地上物
        int TrapRage = Random.Range(0, Trap.Length);
        Instantiate(Trap[TrapRage], pos, Quaternion.identity);

        //等待四個物理幀
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        //生成石頭數量
        int stoneBorn = Random.Range(0, maxStore);
        for(int i = 0; i <= stoneBorn; i++)
        {
            //隨機位置
            float length = Random.Range(0, 10);
            float width = Random.Range(-4, 4);
            Instantiate(stone, pos + new Vector3(width, 0, length), Quaternion.identity);
        }
            
        //生成地板位置
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

        yield return null;
        running = false;
    }
}
