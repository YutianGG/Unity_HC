using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] Rigidbody RB;
    [SerializeField] Animator ani;
    [SerializeField] float Speed = 2f;
    [SerializeField] float MouseSpeed = 1.5f;
    [SerializeField] float JumpSpeed = 50f;
    [SerializeField] Transform Camera;
    [SerializeField] Transform PlayerY;
    [SerializeField] Transform wall;
    [SerializeField] Transform Box;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //��l�ƥͩR�B��q�B����
        Energy = maxEnergy;
        HP = maxHP;
        score = 0f;
        //�]�̤j�Z�������I
        max_dis = this.transform.position.z;
        highScore = highScore;
    }
    private void Update()
    {
        Move();
        Score();
        Wall();
        Dodge();
        CameraShock();
    }
    float vertical = 0f;
    float horizontal = 0f;
    float mouseY = 0f;//�ƹ��첾
    float acc = 0f;
    [SerializeField] float ang = 1f;
    private void Move()
    {
        //���o��V��
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

        //�]�w�ʵe�Ѽ�
        ani.SetFloat("Vertical", vertical);
        ani.SetFloat("Horizontal", horizontal);

        //�]�w�y��
        Vector3 Move = new Vector3
            (horizontal * Speed *(1f + acc * ang), RB.velocity.y, vertical * Speed * (1f + acc * ang));
        
        //�ץ��y��
        Move = PlayerY.transform.TransformDirection(Move);

        ani.speed = 1f + (acc * ang);

        //�ק�RB�t�v���V�qMove
        RB.velocity = Move;
        Move.y = 0f;
        Box.rotation = Quaternion.LookRotation(Move, Vector3.up);

        if (EndUI.ins.isOpen == true)
            return;
        //���o�ƹ������첾  
        float mouseX = Input.GetAxis("Mouse X");

        //���ફ��
        PlayerY.transform.Rotate(0, mouseX * MouseSpeed, 0);
        //RB.angularVelocity = new Vector3(0f, mouseX * MouseSpeed, 0f);

        //���o�ƹ�Y�y�в���
        mouseY += Input.GetAxis("Mouse Y") * MouseSpeed;

        //����ƭȩ�-80��80����
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        //�إ߷s�����
        Quaternion rotation = Quaternion.Euler(mouseY * -1f, 0f, 0f);

        //������v��
        Camera.transform.localRotation = rotation;

        //���D
        if (Input.GetKeyDown(KeyCode.F) && onFloor)
        {
            RB.velocity = new Vector3(RB.velocity.x, JumpSpeed, RB.velocity.z);
            acc = 0;
        }
            
    }
    private void Dodge()
    {   
        //�C�@��[�W10��O
        Energy += Time.deltaTime*10f;

        //��O�C��30���^��
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
        //�e½�u
        if (Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ani.SetTrigger("dodgeF");
        }
        //��½�u
        if (!Input.GetKey(KeyCode.W)
            && Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ani.SetTrigger("dodgeL");
        }
        //��½�u
        if (!Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.A)
            && Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ani.SetTrigger("dodgeB");
        }
        //�k½�u
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
    private void Wall()
    {
        if (wall.transform.position.z < this.transform.position.z - 10f)
            wall.transform.position = this.transform.position + new Vector3(0f, 0f, -10f);
    }
    
    float max_dis = 0f; //�̤j�Z��
    public void Score()
    {
        /*
        if(this.transform.position.z > score)
            score = this.transform.position.z ;
        */
        if (this.transform.position.z > max_dis)
        {
            float add_dis = this.transform.position.z - max_dis;
            score += add_dis;
            max_dis = this.transform.position.z;
        }
        //�����̰���
        if (highScore < score)
            highScore = score;
    }

    [SerializeField] float shockRage;
    [SerializeField] float shockWeak;
    [SerializeField] Transform shockCamera;
    [SerializeField] float slowTimeRage;
    [SerializeField] float slowTimeWeak;
    [SerializeField] float slowTimeStrong;  

    private void CameraShock()
    {
        float shockX = Random.Range(shockRage * -1f, shockRage);
        float shockY = Random.Range(shockRage * -1f, shockRage);
        shockCamera.localPosition = new Vector3(shockX, shockY, 0f);

        //Time.unscaledDeltaTime 
        //Time.deltaTime
        shockRage -= shockWeak * Time.unscaledDeltaTime;
        shockRage = Mathf.Clamp(shockRage, 0f, 2f);

        slowTimeRage -= slowTimeWeak * Time.unscaledDeltaTime;
        slowTimeRage = Mathf.Clamp(slowTimeRage, 0f, 2f);

        if(EndUI.ins.isOpen == false)
            Time.timeScale = Mathf.Clamp(1f - (slowTimeRage * slowTimeStrong), 0f, 1f); 
    }
    public void Injure(float damage)
    {
        if (flashRage > 0)
            return;
        HP -= damage;
        shockRage = 1f;
        slowTimeRage = 1f;
        flashRage = 1f;
        if (HP <= 0)
            EndUI.ins.Open();
    }

    bool onFloor = false;//���D�P�w
    private void FixedUpdate() //���z��s
    {
        onFloor = Physics.Raycast
            (this.transform.position, new Vector3(0f, -1f, 0f), 1.1f);
        ani.SetBool("onFloor", onFloor);

        // �b�c�l�e��U�@�Ӱ������ת��p�g
        // TransformPoint ��X�۹�󥻦a�y�Ъ��@�ӥ@�ɮy���I
        RaycastHit raycastHitA;
        bool raycastA_hit = 
            Physics.Raycast(Box.TransformPoint(0f, 0f, 0.5f), Vector3.down, out raycastHitA, 999f);
        float a = 1f;
        if (raycastA_hit)
            a = Vector3.Distance(Box.TransformPoint(0f, 0f, 0.5f), raycastHitA.point);

        RaycastHit raycastHitB;
        bool raycastB_hit =
            Physics.Raycast(Box.TransformPoint(0f, 0f, -0.5f), Vector3.down, out raycastHitB, 999f);
        float b = 1f;
        if (raycastB_hit)
            b = Vector3.Distance(Box.TransformPoint(0f, 0f, -0.5f), raycastHitB.point);

        if (raycastA_hit && raycastB_hit)
            acc = a - b;
        if (onFloor == false)
            acc = 0;
    }

    /// <summary>
    /// ��q��
    /// </summary>
    [SerializeField] Image energy = null; 
    float Energy
    {
        get { return _energy;}
        set 
        { 
            _energy = Mathf.Clamp(value, 0f, maxEnergy); 
            energy.fillAmount = _energy / maxEnergy;
        }
        
    }
    float _energy = 100f;
    float maxEnergy = 100f;

    /// <summary>
    /// �ͩR��
    /// </summary>
    [SerializeField] float maxHP = 100f;
    [SerializeField] Image hp_Image;

    float HP
    {
        get { return maxHP * hp_Image.fillAmount; }
        set { hp_Image.fillAmount = value / maxHP; }
    }

    /// <summary>
    /// ����
    /// </summary>
    [SerializeField] Text score_text;
    public float score
    {
        get { return _score; }
        set
        {
            _score = value;
            score_text.text = Mathf.Round(value) + "M";
        }
    }
    float _score = 0f;

    [SerializeField] Text highScore_text;
    /// <summary>
    /// �̰���
    /// </summary>
    public float highScore
    {
        get { return PlayerPrefs.GetFloat("HIGHSCORE", 0f); }
        set
        { 
            PlayerPrefs.SetFloat("HIGHSCORE", value);
            highScore_text.text = "HIGH SCORE " + Mathf.Round(value);
        }
    }
    [SerializeField]Renderer render;
    float flashRage = 0f;
    float _flashRage = 1f; //�W�@��
    private void LateUpdate()
    {
        if(_flashRage != flashRage)
        {
            _flashRage = flashRage;
            render.material.SetFloat("_Ctrl", flashRage);
        }

        if (flashRage > 0f)
            flashRage -= Time.unscaledDeltaTime;
        else if (flashRage < 0f)
            flashRage = 0f;
    }
}
