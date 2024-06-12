
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Movement variable
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float jumpHeight = 2.0f;
    [SerializeField]
    private float gravity = -9.81f;
    private CharacterController controller;
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float stamina = 100;
    bool isRunning, isClimbing;

    //Camera variable
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private Transform cameraTransform;

    //UI variable
    [SerializeField]
    private Image stamBarUI;

    [SerializeField]
    private TextMeshProUGUI textWeapon1;
    [SerializeField]
    private TextMeshProUGUI textWeapon2;

    [SerializeField]
    private GameObject statUI;

    //Weapon
    [SerializeField]
    private List<GameObject> weaponInStuff;
    [SerializeField]
    private GameObject weaponEquipped;
    [SerializeField]
    private bool weaponEquipOne;
    [SerializeField]
    private TextMeshProUGUI textRangeWeapon;
    [SerializeField]
    private TextMeshProUGUI textRangeShoot;

    //Stat
    public int bonusStamina;
    public int multiplierRange = 10, multiplierDamage = 10;

    //Climb
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatIsWall;
    public float climbSpeed;
    public float detectionLength;
    public float sphereCastRadius;
    private RaycastHit frontWallHit;
    private float wallLookAngle;
    private float maxWallLookAngle = 30;

    //SolDetection
    public LayerMask whatIsFloor;
    private RaycastHit floorHit;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        stamBarUI.fillAmount = stamina / 100;
        weaponInStuff = new List<GameObject>() { null, null};
    }

    void Update()
    {               
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            statUI.SetActive(!statUI.active);
            Cursor.lockState = (statUI.active) ? CursorLockMode.None : CursorLockMode.Locked;
        }

        if (!statUI.active)
        {
            //Camera rotation
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
            cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

            //Player movement
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

            //Crouch
            if (Input.GetKey(KeyCode.LeftControl))
            {
                speed = 5;
            }
            else
            {
                speed = 10;
            }

            //Sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 20;
                stamina -= 4 * Time.deltaTime;
                isRunning = true;
            }
            else
            {
                speed = 10;
                isRunning = false;
            }

            //Add movement to gameObject
            moveDirection *= speed;
            controller.Move(moveDirection * Time.deltaTime);

            //Jump
            bool isGrounded = controller.isGrounded;

            bool canJump = FloorCheck();
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            //Climb
            if (WallCheck() && Input.GetKey(KeyCode.Z) && wallLookAngle < maxWallLookAngle)
            {
                velocity.y = climbSpeed;
                stamina -= 6 * Time.deltaTime;
                isClimbing = true;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
                isClimbing = false;
            }

            //Regen stamina
            if (!isRunning && !isClimbing && stamina < 100)
            {
                stamina += 5 * Time.deltaTime;
            }            

            //Shoot with weapon
            if (Input.GetMouseButtonDown(0) && weaponEquipped != null)
            {
                Shoot();
            }

            //Drop weapon
            if (Input.GetKeyDown(KeyCode.A) && weaponEquipped != null)
            {
                if (weaponEquipOne)
                {
                    textWeapon1.text = "";
                    weaponEquipOne = false;
                    weaponEquipped.transform.position = transform.position;
                    weaponEquipped.SetActive(true);
                    weaponEquipped = null;
                    weaponInStuff[0] = null;
                }
                else
                {
                    textWeapon2.text = "";
                    weaponEquipped.transform.position = transform.position;
                    weaponEquipped.SetActive(true);
                    weaponEquipped = null;
                    weaponInStuff[1] = null;
                }
            }

            //Select weapon
            if (Input.GetKeyDown(KeyCode.Alpha1) && weaponInStuff[0] != null)
            {
                weaponEquipped = weaponInStuff[0];
                textWeapon1.color = Color.red;
                textWeapon2.color = Color.black;
                weaponEquipOne = true;
                textRangeWeapon.text = "Weapon's range : " + (weaponEquipped.GetComponent<Weapon>().range * ((float)multiplierRange / 10)).ToString();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && weaponInStuff[1] != null)
            {
                weaponEquipped = weaponInStuff[1];
                textWeapon2.color = Color.red;
                textWeapon1.color = Color.black;
                weaponEquipOne = false;
                textRangeWeapon.text = "Weapon's range : " + ((float)weaponEquipped.GetComponent<Weapon>().range * (multiplierRange / 10)).ToString();
            }
        }
        //Add vertical movement
        stamBarUI.fillAmount = stamina / 100;
        controller.Move(velocity * Time.deltaTime);
    }

    private bool WallCheck()
    {
        bool frontWall = Physics.SphereCast(new Vector3(transform.position.x, transform.position.y - GetComponent<MeshRenderer>().bounds.size.y / 4, transform.position.z), sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
        return frontWall;
    }

    private bool FloorCheck()
    {
        bool floor = Physics.SphereCast(transform.position, sphereCastRadius, -orientation.up, out floorHit, detectionLength, whatIsFloor);
        return floor;
    }

    private void Shoot()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 2f);
            textRangeShoot.text = "Shoot distance : " + hitInfo.distance.ToString();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Weapon" && Input.GetKey(KeyCode.E))
        {
            if (weaponInStuff[0] == null)
            {
                weaponInStuff[0] = other.gameObject;
                textWeapon1.text = other.name;
                other.gameObject.SetActive(false);
            } else if (weaponInStuff[1] == null)
            {
                weaponInStuff[1] = other.gameObject;
                textWeapon2.text = other.name;
                other.gameObject.SetActive(false);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
    }
}
