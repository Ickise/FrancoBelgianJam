using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("References")] private InputReader inputReader;

    private void OnEnable()
    {
        inputReader.EnablePlayerInputs();
    }

    private void OnDisable()
    {
        inputReader.DisablePlayerInputs();
    }
}
