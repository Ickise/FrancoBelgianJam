
using UnityEngine;
using UnityEngine.AI;

public class Cow : MonoBehaviour
{
    [SerializeField] private int m_minTimeStatic = 3;
    [SerializeField] private int m_maxTimeStatic = 5;
    private float _currentTimeStatic;
    private int _currentRandTimeStatic;
    private Vector3 _peaceDirection;
    [SerializeField] private int m_minDistance = 1;
    [SerializeField] private int m_maxDistance = 3;
    private int _currentRandDistance;
    private Vector3 _randNextPosPeace;
    [SerializeField] private float m_speedPeace = 3f;
    [SerializeField] private int m_afraidDistance = 3;
    [SerializeField] private float m_speedAfraid = 5f;
    [SerializeField] private int m_fartDistance = 9;
    [SerializeField] private float m_speedFart = 9f;
    [SerializeField] private int m_minTimerecoverFromfart = 5;
    [SerializeField] private int m_maxTimeRecoverFromFart = 9;
    [SerializeField] private int m_timeFartCreation = 2;

    private bool _peace = false;
    private bool _afraid = false;
    private bool _fart = false;

    [SerializeField] private CowState _cowState;
    public enum CowState
    {
        Peace,
        Afraid,
        Fart,
    }
    
    
    void Start()
    {
        _cowState = CowState.Peace;
        SwitchCowState(_cowState);
        _currentRandTimeStatic = 0;
        _currentTimeStatic = 0f;
        _currentRandDistance = 0;
        _randNextPosPeace = new Vector3(0,0,0);
    }

    
    void Update()
    {
        if (_peace == true & _afraid == false & _fart == false)
        {
            _currentTimeStatic = Mathf.MoveTowards(_currentTimeStatic, _currentRandTimeStatic, 1 * Time.deltaTime);
            if(_currentTimeStatic == _currentRandTimeStatic)
            {
                RandomPeaceDistance();
                _randNextPosPeace = new Vector3(transform.position.x + _currentRandDistance, 0, transform.position.z + _currentRandDistance);
                _peaceDirection = _randNextPosPeace - transform.position;
            }
        }

        if (_peace == false & _afraid == true & _fart == false)
        {
            
        }

        if (_peace == false & _afraid == false & _fart == true)
        {
            
        }
    }

    public void SwitchCowState(CowState cowState)
    {
        switch(cowState)
        {
            case CowState.Peace:
            RandomTimeStatic();
            _currentTimeStatic = 0;
            _currentRandDistance = 0;
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

    private void RandomTimeStatic()
    {
        _currentRandTimeStatic = Random.Range(m_minTimeStatic, m_maxTimeStatic);
    }
    private void RandomPeaceDistance()
    {
        _currentRandDistance = Random.Range(m_minDistance, m_maxDistance);
    }
}
