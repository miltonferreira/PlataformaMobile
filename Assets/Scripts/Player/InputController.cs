using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // input horizontal do teclado -----------------
    private float _horizontal;

    public float horizontal{
        get {return _horizontal;}
    }

    // input de pulo do teclado ------------------

    private bool _jump;

    public bool jump{
        get {return _jump;}
        set {_jump = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        
        jump = Input.GetButtonDown("Jump");

    }
}
