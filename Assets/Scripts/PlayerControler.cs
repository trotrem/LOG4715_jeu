using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // Déclaration des constantes
    private static readonly Vector3 FlipRotation = new Vector3(0, 180, 0);
    private static readonly Vector3 CameraPosition = new Vector3(10, 1, 0);
    private static readonly Vector3 InverseCameraPosition = new Vector3(-10, 1, 0);

    // Déclaration des variables
    public bool _Grounded { get; private set; }
    bool _Flipped { get; set; }
    bool _HasBow { get; set; }
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    Camera _MainCamera { get; set; }
    float timeLeftUncontrolable;

    // Valeurs exposées
    [SerializeField]
    [Range(0,10)]
    float timeUncontrolable = 0.25f;

    [SerializeField]
    [Range(0, 10)]
    float gradualControl = 0.25f;

    [SerializeField]
    float MoveSpeed = 5.0f;

    [SerializeField]
    float JumpForce = 10f;

    [SerializeField]
    float KnockbackForce = 8f;

    [SerializeField]
    LayerMask WhatIsGround;

    [SerializeField]
    GameObject Bow;
    
    // Awake se produit avait le Start. Il peut être bien de régler les références dans cette section.
    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _MainCamera = Camera.main;
    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Grounded = false;
        _Flipped = false;
        timeLeftUncontrolable = -gradualControl;
    }

    // Vérifie les entrées de commandes du joueur
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;

        if(timeLeftUncontrolable > -gradualControl) 
        {
            timeLeftUncontrolable -= Time.deltaTime;
        }
        if(timeLeftUncontrolable <= 0)
        {
            HorizontalMove(horizontal);
        }

        if (_Grounded && horizontal == 0 && Mathf.Abs(_Rb.velocity.y) <= 0.5f)
        {
            _Rb.isKinematic = true;
        }
        else
        {
            _Rb.isKinematic = false;
        }
        FlipCharacter(horizontal);
        CheckJump();
    }

    // Gère le mouvement horizontal
    void HorizontalMove(float horizontal)
    {

        float control = gradualControl == 0 ? 1f : -timeLeftUncontrolable / gradualControl;
        _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, (control*horizontal)+(_Rb.velocity.z*(1-control)));
        _Anim.SetFloat("MoveSpeed", Mathf.Abs(horizontal));
    }

    // Gère le saut du personnage, ainsi que son animation de saut
    void CheckJump()
    {
        if (_Grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.isKinematic = false;
                _Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
            }
        }
    }

    // Gère l'orientation du joueur et les ajustements de la camera
    void FlipCharacter(float horizontal)
    {
        if (horizontal < 0 && !_Flipped)
        {
            _Flipped = true;
            transform.RotateAround(transform.position, new Vector3(0,1,0), 180);
            _MainCamera.transform.Rotate(-FlipRotation);
            _MainCamera.transform.localPosition = InverseCameraPosition;
        }
        else if (horizontal > 0 && _Flipped)
        {
            _Flipped = false;
            transform.RotateAround(transform.position, new Vector3(0, 1, 0), -180);
            _MainCamera.transform.Rotate(FlipRotation);
            _MainCamera.transform.localPosition = CameraPosition;
        }
    }

    // Collision avec le sol
    void OnCollisionEnter(Collision coll)
    {        
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) == 0)
            return;
        
        // Évite une collision avec le plafond
        if (coll.relativeVelocity.y > 0)
        {
            _Grounded = true;
            _Anim.SetBool("Grounded", _Grounded);
        }
    }

    public void PickupBow()
    {
        _HasBow = true;
        Bow.SetActive(true);
    }

    public void knockBack(Vector3 vector)
    {
        timeLeftUncontrolable = timeUncontrolable;
        Vector3 jumpForce = new Vector3(0f, vector.y, vector.z) * KnockbackForce;
        _Rb.AddForce(jumpForce, ForceMode.Impulse);
    }

}
