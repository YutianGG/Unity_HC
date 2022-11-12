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

    float mouseY = 0f;//�ƹ��첾
    private void Move()
    {
        //���o��V��
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        //Debug.Log(vertical + " ," + horizontal);
        ani.SetFloat("Vertical", vertical);
        ani.SetFloat("Horizontal", horizontal);

        //�]�w�y��
        Vector3 Move = new Vector3
            (horizontal * Speed, RB.velocity.y, vertical * Speed);
        
        //�ץ��y��
        Move = PlayerY.transform.TransformDirection(Move);

        //�ק�RB�t�v���V�qMove
        RB.velocity = Move;

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
        if (Input.GetKeyDown(KeyCode.Space) && onFlood)
            RB.velocity = new Vector3(RB.velocity.x, JumpSpeed, RB.velocity.z); 
    }

    bool onFlood = false;//���D�P�w
    private void FixedUpdate() //���z��s
    {
        onFlood = Physics.Raycast
            (this.transform.position, new Vector3(0f, -1f, 0f), 1.1f);
    }
}
