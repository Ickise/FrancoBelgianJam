using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIBattery : MonoBehaviour
{
    [SerializeField, Header("References")] private TextMeshProUGUI batteryText;

    [SerializeField] private BatteryManager _batteryManager;
    [SerializeField] private GameObject[] _batteryGO;

    private void Start()
    {
        UpdateEnergyUI();
    }

    public void UpdateEnergyUI()
    {
        batteryText.text = $"Energy: {_batteryManager.GetCurrentBattery()} / {_batteryManager.GetMaxBattery()}";
        switch (_batteryManager.GetCurrentBattery())
        {
            case 0:
                _batteryGO[0].SetActive(false);
                _batteryGO[1].SetActive(false);
                _batteryGO[2].SetActive(false);
                _batteryGO[3].SetActive(false);
                _batteryGO[4].SetActive(false);
                _batteryGO[5].SetActive(false);
                break;
            case <=17:
                _batteryGO[0].SetActive(true);
                _batteryGO[1].SetActive(false);
                _batteryGO[2].SetActive(false);
                _batteryGO[3].SetActive(false);
                _batteryGO[4].SetActive(false);
                _batteryGO[5].SetActive(false);
                break;
            case <=34:
                _batteryGO[0].SetActive(true);
                _batteryGO[1].SetActive(true);
                _batteryGO[2].SetActive(false);
                _batteryGO[3].SetActive(false);
                _batteryGO[4].SetActive(false);
                _batteryGO[5].SetActive(false);
                break;
            case <=51:
                _batteryGO[0].SetActive(true);
                _batteryGO[1].SetActive(true);
                _batteryGO[2].SetActive(true);
                _batteryGO[3].SetActive(false);
                _batteryGO[4].SetActive(false);
                _batteryGO[5].SetActive(false);
                break;
            case <=68:
                _batteryGO[0].SetActive(true);
                _batteryGO[1].SetActive(true);
                _batteryGO[2].SetActive(true);
                _batteryGO[3].SetActive(true);
                _batteryGO[4].SetActive(false);
                _batteryGO[5].SetActive(false);
                break;
            case <=85:
                _batteryGO[0].SetActive(true);
                _batteryGO[1].SetActive(true);
                _batteryGO[2].SetActive(true);
                _batteryGO[3].SetActive(true);
                _batteryGO[4].SetActive(true);
                _batteryGO[5].SetActive(false);
                break;
            case <=100:
                _batteryGO[0].SetActive(true);
                _batteryGO[1].SetActive(true);
                _batteryGO[2].SetActive(true);
                _batteryGO[3].SetActive(true);
                _batteryGO[4].SetActive(true);
                _batteryGO[5].SetActive(true);
                break;
        }

    }
}