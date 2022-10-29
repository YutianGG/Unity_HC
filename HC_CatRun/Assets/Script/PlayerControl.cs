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
        //���o��V��
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        //Debug.Log(vertical + " ," + horizontal);

        //�]�w�y��
        Vector3 Move = new Vector3(vertical * Speed, 0, -horizontal * Speed );
        
        //�ץ��y��
        Move = this.transform.TransformDirection(Move);
        
        //�ק�RB�t�v���V�qMove
        RB.velocity = Move;

        //���o�ƹ������첾
        float mousex = Input.GetAxis("Mouse X");

        //���ફ��
        //this.transform.Rotate(0, mousex, 0);
        RB.angularVelocity =new Vector3(0, mousex * Speed, 0);
    }
}
