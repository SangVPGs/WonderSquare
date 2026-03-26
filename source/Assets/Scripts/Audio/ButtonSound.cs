using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayButton();
        });
    }
}