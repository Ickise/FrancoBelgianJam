using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int m_levelSwitch = 1;
    [SerializeField] private GameObject m_character;
    [SerializeField] private List<GameObject> m_positionLevel;
    public GameObject m_intermediateLevelSwitch;
    [SerializeField] private GameObject _camera;
    
    void Start()
    {
        //m_levelSwitch = 1;
        SwitchLevel(m_levelSwitch);  // initial set up
    }

    public void Update() //debug only
    {
        SwitchLevel(m_levelSwitch);
    }

    public void SwitchLevel(int m_levelSwitch)
    {
        switch (m_levelSwitch)
        {
            case 0:
            m_intermediateLevelSwitch = m_positionLevel.ElementAt(m_levelSwitch);               //set the new world with index
            m_character.transform.position = m_intermediateLevelSwitch.transform.position;       // set position of character to this new world   
            _camera.transform.position = m_intermediateLevelSwitch.transform.position;            // same with camera    warning change the parent pos, not the camera component itself
                break;


            case 1:
            m_intermediateLevelSwitch = m_positionLevel.ElementAt(m_levelSwitch);
            m_character.transform.position = m_intermediateLevelSwitch.transform.position;
            _camera.transform.position = m_intermediateLevelSwitch.transform.position;
                break;


            case 2:
            m_intermediateLevelSwitch = m_positionLevel.ElementAt(m_levelSwitch);
            m_character.transform.position = m_intermediateLevelSwitch.transform.position;
            _camera.transform.position = m_intermediateLevelSwitch.transform.position;
                break;

            case 3:
            m_intermediateLevelSwitch = m_positionLevel.ElementAt(m_levelSwitch);
            m_character.transform.position = m_intermediateLevelSwitch.transform.position;
            _camera.transform.position = m_intermediateLevelSwitch.transform.position;
                break;

            
            default:
            print("erreorStateLevel");
            
                break;
        }
    }

    
}
