using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private int m_levelSwitch = 1;
    [SerializeField] private GameObject m_character;
    [SerializeField] private List<GameObject> m_positionLevel;
    private GameObject m_intermediateLevelSwitch;
    
    void Start()
    {
        //m_levelSwitch = 1;
        SwitchLevel(m_levelSwitch);
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
            m_intermediateLevelSwitch = m_positionLevel.ElementAt(m_levelSwitch);
            m_character.transform.position = m_intermediateLevelSwitch.transform.position;
                break;


            case 1:
            m_intermediateLevelSwitch = m_positionLevel.ElementAt(m_levelSwitch);
            m_character.transform.position = m_intermediateLevelSwitch.transform.position;
                break;


            case 2:
            m_intermediateLevelSwitch = m_positionLevel.ElementAt(m_levelSwitch);
            m_character.transform.position = m_intermediateLevelSwitch.transform.position;
                break;

            case 3:
            m_intermediateLevelSwitch = m_positionLevel.ElementAt(m_levelSwitch);
            m_character.transform.position = m_intermediateLevelSwitch.transform.position;
                break;

            
            default:
            print("erreorStateLevel");
            
                break;
        }
    }

    
}
