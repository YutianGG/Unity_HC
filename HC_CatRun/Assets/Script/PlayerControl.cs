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

    float mouseY = 0f;//�ƹ��첾
    private void Move()
    {
        //���o��V��
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        //Debug.Log(vertical + " ," + horizontal);

        //�]�w�y��
        Vector3 Move = new Vector3
            (vertical * Speed, RB.velocity.y, -horizontal * Speed);

        Move = this.transform.TransformDirection(Move);//�ץ��y��
        RB.velocity = Move;//�ק�RB�t�v���V�qMove

        float mouseX = Input.GetAxis("Mouse X");//���o�ƹ������첾  
        //this.transform.Rotate(0, mouseX * MouseSpeed, 0);
        RB.angularVelocity = new Vector3(0f, mouseX * MouseSpeed, 0f);//���ફ��

        mouseY += Input.GetAxis("Mouse Y") * MouseSpeed;//���o�ƹ�Y�y�в���
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);//����ƭȩ�-80��80����
        Quaternion rotation = Quaternion.Euler(mouseY * -1f, 90f, 0f);//�إ߷s�����
        Camera.transform.localRotation = rotation;//������v��

        if (Input.GetKeyDown(KeyCode.Space) && onFlood)//���D
            RB.velocity = new Vector3(0f, JumpSpeed, 0f); 
    }

    bool onFlood = false;//���D�P�w
    private void FixedUpdate() //���z��s
    {
        onFlood = Physics.Raycast
            (this.transform.position, new Vector3(0f, -1f, 0f), 1.1f);
    }
}
