using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FactoryMenu : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button startProcessButton;
    [SerializeField]
    private Slider progressSlider;
    private float duration = 5f; 
    private float elapsed = 0f;
    private bool isUpdating = false;

    public void Init()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        startProcessButton.onClick.AddListener(() => StartTheProcess());
        progressSlider.value = 0;
    }
    public void StartTheProcess()
    {   
        isUpdating = true;
        elapsed = 0f;
    }
    private void Update()
    {
        if (isUpdating)
        {
            if (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                progressSlider.value = Mathf.Clamp01(elapsed / duration); 
            }
            else
            {
                progressSlider.value = 1f; 
                isUpdating = false; 
            }
        }
        
    }
}
