using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private int m_afraidDistance = 3;
    [SerializeField] private float m_speedAfraid = 5f;
    [SerializeField] private int m_fartDistance = 9;
    [SerializeField] private float m_speedFart = 9f;
    [SerializeField] private int m_minTimerecoverFromfart = 5;
    [SerializeField] private int m_maxTimeRecoverFromFart = 9;
    [SerializeField] private int m_timeFartCreation = 2;

    [SerializeField] private NavMeshAgent _navmesh;
    [SerializeField] private CharacterController _charactercontroller;
    [SerializeField] private LevelManager _levelManager;
    private GameObject _currentWorld;
    [SerializeField] private int _EdgeOfWorldToCenter;
    [SerializeField]private float _roundDeplacement;

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
        if (_peace == true && _afraid == false && _fart == false)  // peace state     manque the other cow detection
        {
            //print(" in peace State");
            if (_currentTimeStatic != _currentRandTimeStatic)    
            {
                _currentTimeStatic = Mathf.MoveTowards(_currentTimeStatic, _currentRandTimeStatic, 1 * Time.deltaTime);
                //print(_currentTimeStatic);
            }
            if(_currentTimeStatic == _currentRandTimeStatic)
            {
                print("deplacement");
                _currentWorld = _levelManager.m_intermediateLevelSwitch;            // get the current world
                if (_randNextPosPeace.x <= _currentWorld.transform.position.x +  _EdgeOfWorldToCenter ||
                    _randNextPosPeace.x >= _currentWorld.transform.position.x -  _EdgeOfWorldToCenter && 
                    _randNextPosPeace.z <= _currentWorld.transform.position.z +  _EdgeOfWorldToCenter ||
                    _randNextPosPeace.z >= _currentWorld.transform.position.z -  _EdgeOfWorldToCenter && 
                    _deplacementPeace == false)  // verify new random pos for cow by verify the bound of the current world 
                {

                    //_peaceDirection = _randNextPosPeace - transform.position;    // direction calcul
                    //_navmesh.Move(_randNextPosPeace);                //
                    //_charactercontroller.SimpleMove(m_speedPeace);  // was trying something for move but not succed
                    _deplacementPeace = true;
                }
                else
                {
                    //RandomPeaceDistance();     // see if it worse to do that
                }

                if (_deplacementPeace == true)     // deplacement here
                {
                    //transform.position = Vector3.Lerp(transform.position, _randNextPosPeace, m_speedPeace * Time.deltaTime);
                    transform.position = Vector3.LerpUnclamped(transform.position, _randNextPosPeace, m_speedPeace * Time.deltaTime); //made as alternative of incremental
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
            print(" in afraid State");
        }

        if (_peace == false & _afraid == false & _fart == true) // fart state
        {
            print(" in fart State");
        }
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
            _peace = false;
            _afraid = true;
            _fart = false;
                break;
            case CowState.Fart:
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
        print("new time static = " + _currentRandTimeStatic);
    }
    private void RandomPeaceDistance() // get a random pos x z to get a random pos
    {
        _currentRandDistancex = Random.Range(m_minDistance, m_maxDistance); 
        _currentRandDistancez = Random.Range(m_minDistance, m_maxDistance); 
        _randNextPosPeace = new Vector3(transform.position.x + _currentRandDistancex, 0, transform.position.z + _currentRandDistancez);    // compose the destination with the random offset on x z
        print(_randNextPosPeace);
    }
}
