using UnityEngine;
using UnityEngine.UI;

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
        Energy = maxEnergy;
    }
    private void Update()
    {
        Move();
        Dodge();
    }
    float vertical = 0f;
    float horizontal = 0f;
    float mouseY = 0f;//滑鼠位移
    private void Move()
    {
        //取得方向鍵
        float local_vertical = Input.GetAxisRaw("Vertical");
        float local_horizontal = Input.GetAxisRaw("Horizontal");
        //Debug.Log(vertical + " ," + horizontal);       

        vertical = Mathf.Lerp(vertical, local_vertical, Time.deltaTime * 10f);
        horizontal = Mathf.Lerp(horizontal, local_horizontal, Time.deltaTime * 10f);

        float FV = ani.GetFloat("FV");
        float FH = ani.GetFloat("FH");
        float T = ani.GetFloat("T");
        vertical = Mathf.Lerp(vertical, FV, T);
        horizontal = Mathf.Lerp(horizontal, FH, T);

        //設定動畫參數
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
        if (Input.GetKeyDown(KeyCode.F) && onFloor)
            RB.velocity = new Vector3(RB.velocity.x, JumpSpeed, RB.velocity.z);
    }
    private void Dodge()
    {   
        //每一秒加上10體力
        Energy += Time.deltaTime*10f;

        //體力低於30不回傳
        if (Energy < 30f)
            return;

        float angel = Mathf.Atan2(vertical, horizontal) / Mathf.PI;
        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            Energy -= 30f;
            if (angel >= 0.75f || angel < -0.75f)
                ani.SetTrigger("dodgeL");
            else if (angel >= 0.25f)
                ani.SetTrigger("dodgeF");
            else if (angel >= -0.25f)
                ani.SetTrigger("dodgeR");
            else if (angel >= -0.75f)
                ani.SetTrigger("dodgeB");
        }
        
        /*
        //前翻滾
        if (Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ani.SetTrigger("dodgeF");
        }
        //左翻滾
        if (!Input.GetKey(KeyCode.W)
            && Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ani.SetTrigger("dodgeL");
        }
        //後翻滾
        if (!Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A)
            && Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ani.SetTrigger("dodgeB");
        }
        //右翻滾
        if (!Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S)
            && Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ani.SetTrigger("dodgeR");
        }
        */
    }

    bool onFloor = false;//跳躍判定
    private void FixedUpdate() //物理刷新
    {
        onFloor = Physics.Raycast
            (this.transform.position, new Vector3(0f, -1f, 0f), 1.1f);
        ani.SetBool("onFloor", onFloor);
    }

    [SerializeField] Image energy = null; 
    float Energy
    {
        get 
        {
            return _energy;
        }
        set 
        { 
            _energy = Mathf.Clamp(value, 0f, maxEnergy); 
            energy.fillAmount = _energy / maxEnergy;
        }
    }
    float _energy = 100f;
    float maxEnergy = 100f;
}
