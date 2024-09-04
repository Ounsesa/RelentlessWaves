using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public Utils.Players m_playerID;
        public PlayerInput[] m_playerInput;

        public float speed;

        private InputActions _input;
        private Animator animator;

        [HideInInspector]
        public GameObject m_interactObject;

        public GameObject bulletGameObject;
        private Vector2 lookingDirection = new Vector2(1,0);

        [Header("MeleeWeapons")]
        public GameObject[] MeleeWeapons;
        public Transform RightHand;
        public GameObject currentMeleeWeaponObject;
        private int currentMeleeWeapon = 0;

        [Header("RangeWeapons")]
        public GameObject[] RangeWeapons;
        public Transform LeftHand;
        public GameObject currentRangeWeaponObject;
        private int currentRangeWeapon = 0;


        Dictionary<string, Weapon> weaponsAmmo = new Dictionary<string, Weapon>();

        private void Start()
        {
            animator = GetComponent<Animator>();

            PlayerInput playerInput = GetComponent<PlayerInput>();
            _input = GameManager.m_instance.m_gameplayManager.m_inputManager.RegsiterInputActionsPlayer(playerInput, m_playerID);


            weaponsAmmo.Add("RangeWeapon1(Clone)", new Weapon(15,3,3));
            weaponsAmmo.Add("RangeWeapon2(Clone)", new Weapon(50, 10, 10));
            weaponsAmmo.Add("RangeWeapon3(Clone)", new Weapon(27, 9, 9));
        }


        private void Update()
        {
            Movement();
            Interact();
            Attack();
            ChangeWeapon();
        }

        private void Movement()
        {   

            Vector2 dir = Vector2.zero;
            if (_input.m_buttonLeft.IsPressed())
            {
                dir.x = -1;
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                lookingDirection = new Vector2(-1, 0);
            }
            else if (_input.m_buttonRight.IsPressed())
            {
                dir.x = 1;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                lookingDirection = new Vector2(1, 0);
            }

            if (_input.m_buttonUp.IsPressed())
            {
                dir.y = 1;
                lookingDirection = new Vector2(0, 1);
            }
            else if (_input.m_buttonDown.IsPressed())
            {
                dir.y = -1;
                lookingDirection = new Vector2(0, -1);
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;



        }

        private void Interact()
        {
            if (_input.m_buttonInteract.triggered && m_interactObject)
            {
                m_interactObject.GetComponent<InteractuableItemWithInput>().Interact();
            }
        }

        private void Attack()
        {
            
            if (_input.m_buttonLeftAttack.IsPressed())
            {
                LeftHand.GetChild(0).GetComponent<WeaponController>().Shoot(lookingDirection);
            }
            
            
            if (_input.m_buttonRightAttack.triggered)
            {
                
            }
        }

        private void ChangeWeapon()
        {
            if(_input.m_buttonChangeMeleeWeapon.triggered) 
            {
                if(currentMeleeWeaponObject)
                { 
                    Destroy(currentMeleeWeaponObject);
                }
                currentMeleeWeapon = ++currentMeleeWeapon % MeleeWeapons.Length;
                currentMeleeWeaponObject = Instantiate(MeleeWeapons[currentMeleeWeapon], RightHand);

                // You might need to adjust the position and rotation of the instantiated weapon
                currentMeleeWeaponObject.transform.localPosition = new Vector3(-0.25f,0.15f);
                currentMeleeWeaponObject.transform.localRotation = Quaternion.Euler(0,0,-35);

            }
            if (_input.m_buttonChangeRangeWeapon.triggered) 
            {
                if (currentRangeWeaponObject)
                {
                    Destroy(currentRangeWeaponObject);
                }           
                currentRangeWeapon = ++currentRangeWeapon % RangeWeapons.Length;
                currentRangeWeaponObject = Instantiate(RangeWeapons[currentRangeWeapon], LeftHand);
                currentRangeWeaponObject.GetComponent<WeaponController>().Init(weaponsAmmo[currentRangeWeaponObject.name], LayerMask.LayerToName(gameObject.layer), gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
            }
        }

        
    }
}

public class Weapon
{
    public int maxAmmo = 1;
    public int currentAmmo = 1;
    public int chargerAmmo = 1;

    public Weapon(int _maxAmmo, int _chargerAmmo, int _currentAmmo)
    {
        maxAmmo = _maxAmmo;
        chargerAmmo = _chargerAmmo;
        currentAmmo = _currentAmmo;
    }
}












//private void AvoidCorners()
//{
//    RaycastHit2D UpLeftRay = Physics2D.Raycast(transform.position + new Vector3(-0.2f, 0.25f, 0), Vector2.up, 0.01f, LayerMask.NameToLayer(gameObject.tag));
//    RaycastHit2D UpRightRay = Physics2D.Raycast(transform.position + new Vector3(0.2f, 0.25f, 0), Vector2.up, 0.01f, LayerMask.NameToLayer(gameObject.tag));
//    if (UpLeftRay.collider != null)
//    {
//        if (UpLeftRay.collider.gameObject.GetComponent<BoxCollider2D>() && !UpLeftRay.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger)
//        {
//            print(UpLeftRay.collider.gameObject.name);
//            print("Hit");
//        }

//    }
//    if (UpRightRay.collider != null)
//    {
//        if (UpRightRay.collider.gameObject.GetComponent<BoxCollider2D>() && !UpRightRay.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger)
//        {
//            print(UpRightRay.collider.gameObject.name);
//            print("Hit2");
//        }
//    }


//    Debug.DrawRay(transform.position + new Vector3(-0.2f, 0.25f, 0), Vector2.up * 0.5f, Color.red, 1);
//    Debug.DrawRay(transform.position + new Vector3(0.2f, 0.25f, 0), Vector2.up * 0.5f, Color.red, 1);



//}