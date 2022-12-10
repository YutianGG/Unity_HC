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
        Energy = maxEnergy;
    }
    private void Update()
    {
        Move();
        Wall();
        Dodge();
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
