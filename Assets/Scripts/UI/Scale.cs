using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent (typeof(Slider))]
public class Scale : MonoBehaviour
{
    [SerializeField] private AudioSource _fillingSound;

    private Slider _slider;

    private const float MinValue = 0;
    private const float MaxValue = 1;
    private const float Duration = 0.01f;

    public bool IsEmpty { get; private set; }
    public float CurrentValue => _slider.value;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _fillingSound.Play();
        _slider.value = MinValue;
        IsEmpty = false;
    }

    private void OnDisable()
    {
        _slider.value = MinValue;
        IsEmpty = false;
    }

    private void Update()
    {
        if (CurrentValue == MaxValue)
            IsEmpty = true;
        else
            IsEmpty = false;

        OnValueChanged();
    }

    private void OnValueChanged()
    {
        _slider.DOValue((_slider.value + Time.deltaTime), Duration);
    }
}
