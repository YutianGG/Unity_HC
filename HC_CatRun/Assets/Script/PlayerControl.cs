using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Rigidbody RB;
    [SerializeField] float Speed = 2f; 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        //取得方向鍵
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        //Debug.Log(vertical + " ," + horizontal);

        //設定座標
        Vector3 Move = new Vector3(vertical * Speed, 0, -horizontal * Speed );
        
        //修正座標
        Move = this.transform.TransformDirection(Move);
        
        //修改RB速率為向量Move
        RB.velocity = Move;

        //取得滑鼠水平位移
        float mousex = Input.GetAxis("Mouse X");

        //旋轉物件
        //this.transform.Rotate(0, mousex, 0);
        RB.angularVelocity =new Vector3(0, mousex * Speed, 0);
    }
}
