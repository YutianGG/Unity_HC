using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Rigidbody RB;
    [SerializeField] float Speed = 2f;
    [SerializeField] float MouseSpeed = 1.5f;
    [SerializeField] float JumpSpeed = 50f;
    [SerializeField] Transform Camera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Move();
    }

    float mouseY = 0f;//滑鼠位移
    private void Move()
    {
        //取得方向鍵
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        //Debug.Log(vertical + " ," + horizontal);

        //設定座標
        Vector3 Move = new Vector3
            (vertical * Speed, RB.velocity.y, -horizontal * Speed);

        Move = this.transform.TransformDirection(Move);//修正座標
        RB.velocity = Move;//修改RB速率為向量Move

        float mouseX = Input.GetAxis("Mouse X");//取得滑鼠水平位移  
        //this.transform.Rotate(0, mouseX * MouseSpeed, 0);
        RB.angularVelocity = new Vector3(0f, mouseX * MouseSpeed, 0f);//旋轉物件

        mouseY += Input.GetAxis("Mouse Y") * MouseSpeed;//取得滑鼠Y座標移動
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);//限制數值於-80到80之間
        Quaternion rotation = Quaternion.Euler(mouseY * -1f, 90f, 0f);//建立新旋轉值
        Camera.transform.localRotation = rotation;//旋轉攝影機

        if (Input.GetKeyDown(KeyCode.Space) && onFlood)//跳躍
            RB.velocity = new Vector3(0f, JumpSpeed, 0f); 
    }

    bool onFlood = false;//跳躍判定
    private void FixedUpdate() //物理刷新
    {
        onFlood = Physics.Raycast
            (this.transform.position, new Vector3(0f, -1f, 0f), 1.1f);
    }
}
