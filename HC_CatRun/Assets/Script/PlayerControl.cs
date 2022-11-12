using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Rigidbody RB;
    [SerializeField] Animator ani;
    [SerializeField] float Speed = 2f;
    [SerializeField] float MouseSpeed = 1.5f;
    [SerializeField] float JumpSpeed = 50f;
    [SerializeField] Transform Camera;
    [SerializeField] Transform PlayerY;
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
        ani.SetFloat("Vertical", vertical);
        ani.SetFloat("Horizontal", horizontal);

        //設定座標
        Vector3 Move = new Vector3
            (horizontal * Speed, RB.velocity.y, vertical * Speed);
        
        //修正座標
        Move = PlayerY.transform.TransformDirection(Move);

        //修改RB速率為向量Move
        RB.velocity = Move;

        //取得滑鼠水平位移  
        float mouseX = Input.GetAxis("Mouse X");

        //旋轉物件
        PlayerY.transform.Rotate(0, mouseX * MouseSpeed, 0);
        //RB.angularVelocity = new Vector3(0f, mouseX * MouseSpeed, 0f);

        //取得滑鼠Y座標移動
        mouseY += Input.GetAxis("Mouse Y") * MouseSpeed;

        //限制數值於-80到80之間
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        //建立新旋轉值
        Quaternion rotation = Quaternion.Euler(mouseY * -1f, 0f, 0f);

        //旋轉攝影機
        Camera.transform.localRotation = rotation;

        //跳躍
        if (Input.GetKeyDown(KeyCode.Space) && onFlood)
            RB.velocity = new Vector3(RB.velocity.x, JumpSpeed, RB.velocity.z); 
    }

    bool onFlood = false;//跳躍判定
    private void FixedUpdate() //物理刷新
    {
        onFlood = Physics.Raycast
            (this.transform.position, new Vector3(0f, -1f, 0f), 1.1f);
    }
}
