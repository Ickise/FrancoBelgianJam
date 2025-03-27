using Unity.AI.Navigation;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class Cow : MonoBehaviour
{
    [SerializeField] private int m_minTimeStatic = 3;
    [SerializeField] private int m_maxTimeStatic = 5;
    private float _currentTimeStatic;
    private int _currentRandTimeStatic;
    private Vector3 _peaceDirection;
    [SerializeField] private int m_minDistance = -1;
    [SerializeField] private int m_maxDistance = 1;
    private int _currentRandDistancex;
    private int _currentRandDistancez;
    private Vector3 _randNextPosPeace;
    [SerializeField] private float m_speedPeace = 3f;
    [SerializeField] private float m_afraidDistance = 3;
    [SerializeField] private float m_speedAfraid = 5f;
    [SerializeField] private int m_fartDistance = 9;
    [SerializeField] private float m_speedFart = 9f;
    [SerializeField] private int m_minTimerecoverFromfart = 5;
    [SerializeField] private int m_maxTimeRecoverFromFart = 9;
    [SerializeField] private float m_timeFartCreation = 2;

    [SerializeField] private GameObject _character;
    [SerializeField] private NavMeshAgent _navmesh;
    [SerializeField] private CharacterController _charactercontroller;
    [SerializeField] private LevelManager _levelManager;
    private GameObject _currentWorld;
    [SerializeField] private int _EdgeOfWorldToCenter;
    [SerializeField]private float _roundDeplacement;
    private Vector3 _scaredThingPos;
    private Vector3 _flyCowDirection;
    private float _secondAfraidDistance;
    private Vector3 _ricochetDirection;
    private Vector3 _obstacleRicochetPos;
    [SerializeField] private GameObject _fartObject;
    [SerializeField] private Transform _instantiateFartPos;
    private bool _timerfartToPeace = false;
    private float _currentTimer;
    [SerializeField] private float _maxTimer = 1;
    [SerializeField] private float _timerSpeed;

    private bool _peace = false;
    private bool _afraid = false;
    private bool _fart = false;
    private bool _deplacementPeace = false;

    [SerializeField] private CowState _cowState;
    public enum CowState
    {
        Peace,
        Afraid,
        Fart,
    }
    
    
    void Start()  // initial initialize of this value for a good start
    {
        _cowState = CowState.Peace;
        SwitchCowState(_cowState);
        _currentRandTimeStatic = 0;
        _currentTimeStatic = 0f;
        _currentRandDistancex = 0;
        _currentRandDistancez = 0;
        _randNextPosPeace = new Vector3(0,0,0);
        RandomTimeStatic();
        RandomPeaceDistance();
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCowState(CowState.Fart);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            SwitchCowState(CowState.Peace);
        }


        if (_peace == true && _afraid == false && _fart == false)  // peace state     manque the other cow detection
        {
            print(" in peace State");
            if (_currentTimeStatic != _currentRandTimeStatic)    
            {
                _currentTimeStatic = Mathf.MoveTowards(_currentTimeStatic, _currentRandTimeStatic, 1 * Time.deltaTime);
            }
            if(_currentTimeStatic == _currentRandTimeStatic)
            {
                _currentWorld = _levelManager.m_intermediateLevelSwitch;            // get the current world
                if (_randNextPosPeace.x <= _currentWorld.transform.position.x +  _EdgeOfWorldToCenter ||
                    _randNextPosPeace.x >= _currentWorld.transform.position.x -  _EdgeOfWorldToCenter && 
                    _randNextPosPeace.z <= _currentWorld.transform.position.z +  _EdgeOfWorldToCenter ||
                    _randNextPosPeace.z >= _currentWorld.transform.position.z -  _EdgeOfWorldToCenter && 
                    _deplacementPeace == false)  // verify new random pos for cow by verify the bound of the current world 
                {
                    _deplacementPeace = true;
                }
                else
                {
                    //RandomPeaceDistance();     // see if it worse to do that
                }

                if (_deplacementPeace == true)     // deplacement here
                {
                    //transform.position = Vector3.Lerp(transform.position, _randNextPosPeace, m_speedPeace * Time.deltaTime);
                    transform.position = Vector3.MoveTowards(transform.position, _randNextPosPeace, m_speedPeace * Time.deltaTime); 
                    if (transform.position.x <= _randNextPosPeace.x + _roundDeplacement && 
                        transform.position.z <= _randNextPosPeace.z + _roundDeplacement ||
                        transform.position.x >= _randNextPosPeace.x - _roundDeplacement && 
                        transform.position.z >= _randNextPosPeace.z - _roundDeplacement) // initialize new timer when x and z are equal (y change i don't know why)
                    {
                        _deplacementPeace = false;
                        RandomTimeStatic();
                        RandomPeaceDistance();
                        _currentTimeStatic = 0;       //end of cycle
                    }
                }
                
            }
        }

        if (_peace == false & _afraid == true & _fart == false) // afraid state
        {
            //print(" in afraid State");
            if (transform.position != _flyCowDirection)
            {
                transform.position = Vector3.MoveTowards(transform.position, _flyCowDirection, m_speedAfraid * Time.deltaTime);
            }
            if (transform.position.x >= _flyCowDirection.x - (_roundDeplacement / 100)&& 
                transform.position.z >= _flyCowDirection.z - (_roundDeplacement / 100)||
                transform.position.x <= _flyCowDirection.x + (_roundDeplacement / 100)&& 
                transform.position.z <= _flyCowDirection.z + (_roundDeplacement / 100))
            {
                SwitchCowState(CowState.Peace);
            }
        }

        if (_peace == false & _afraid == false & _fart == true) // fart state
        {
            //print(" in fart State");
            if (transform.position != _flyCowDirection)
            {
                transform.position = Vector3.MoveTowards(transform.position, _flyCowDirection, m_speedFart * Time.deltaTime);
            }
            if (transform.position.x >= _flyCowDirection.x - (_roundDeplacement / 100) && 
                transform.position.z >= _flyCowDirection.z - (_roundDeplacement / 100) ||
                transform.position.x <= _flyCowDirection.x + (_roundDeplacement / 100) && 
                transform.position.z <= _flyCowDirection.z + (_roundDeplacement / 100))
            {
                StopCoroutine(FartsCoroutine());
                _timerfartToPeace = true;
                _currentTimer = 0;
            }
            if (_timerfartToPeace == true && _currentTimer != _maxTimer)
            {
                _currentTimer = Mathf.MoveTowards(_currentTimer, _maxTimer, _timerSpeed * Time.deltaTime);
                if (_currentTimer == _maxTimer)
                {
                    SwitchCowState(CowState.Peace);
                    _currentTimer = 0;
                }
                
            }
        }
    }
    IEnumerator FartsCoroutine ()  //start in switch
    {
        Instantiate(_fartObject, _instantiateFartPos.position, Quaternion.identity);
        yield return new WaitForSeconds(m_timeFartCreation);
        StartCoroutine(FartsCoroutine());
    }

    public void SwitchCowState(CowState cowState)
    {
        switch(cowState)
        {
            case CowState.Peace:
            RandomTimeStatic();
            _currentTimeStatic = 0;
            _currentRandDistancex = 0;
            _peace = true;
            _afraid = false;
            _fart = false;
                break;
            case CowState.Afraid:
            _scaredThingPos = _character.transform.position;
            CalculDirectionToFly(_scaredThingPos);
            _peace = false;
            _afraid = true;
            _fart = false;
                break;
            case CowState.Fart:
            _scaredThingPos = _character.transform.position;
            CalculDirectionToFlyFart(_scaredThingPos);
            StartCoroutine(FartsCoroutine());
            _peace = false;
            _afraid = false;
            _fart = true;
                break;
            default:
                cowState = CowState.Peace;
                break;
        }
    }
    private void RandomTimeStatic() // get a random time for cow stay static
    {
        _currentRandTimeStatic = Random.Range(m_minTimeStatic, m_maxTimeStatic);
    }
    private void RandomPeaceDistance() // get a random pos x z to get a random pos
    {
        _currentRandDistancex = Random.Range(m_minDistance, m_maxDistance); 
        _currentRandDistancez = Random.Range(m_minDistance, m_maxDistance); 
        _randNextPosPeace = new Vector3(transform.position.x + _currentRandDistancex, 0, transform.position.z + _currentRandDistancez);    // compose the destination with the random offset on x z
    }

    void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag(""))            // put tag of collider to make cow affraid here
        {
            _scaredThingPos = _character.transform.position;
            SwitchCowState(CowState.Afraid);
        }*/

        if (other.CompareTag("Obstacle") && _afraid == true)   // cow or obstacle tags
        {
            _obstacleRicochetPos = other.transform.position;
            CalculDirectionToFly(_obstacleRicochetPos);
        }
        /*if (other.CompareTag(""))                               // big trouble == fart mode
        {
            _scaredThingPos = _character.transform.position;
            SwitchCowState(CowState.Fart);
        }*/
    }

    private void CalculDirectionToFly(Vector3 _scaredPos)
    {
        _flyCowDirection = transform.position - _scaredPos ;
        _flyCowDirection = _flyCowDirection * m_afraidDistance;
        _flyCowDirection = _flyCowDirection  + transform.position;
    }

    private void CalculDirectionToFlyFart(Vector3 _scaredPos)
    {
        _flyCowDirection = transform.position - _scaredPos ;
        _flyCowDirection = _flyCowDirection * m_fartDistance;
        _flyCowDirection = _flyCowDirection  + transform.position;
    }
}
