using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Toggle lockToggle;
    [SerializeField] private Image faceImage;
    [SerializeField] private GameObject lockOverlay;

    [Header("주사위 눈 이미지")]
    [SerializeField] private Sprite[] faceSprites;

    public int Value { get; private set; }
    public bool IsLocked { get; private set; }

    private bool hasRolled;

    private void Awake()
    {
        lockToggle.onValueChanged.AddListener(OnLockChanged);

        ResetDice();
    }

    public void Roll()
    {
        if (IsLocked)
            return;

        Value = Random.Range(1, 7);
        hasRolled = true;

        UpdateFaceImage();
    }

    private void OnLockChanged(bool isOn)
    {
        // 아직 한 번도 굴리지 않은 주사위는 잠그지 못하게 함
        if (!hasRolled)
        {
            lockToggle.SetIsOnWithoutNotify(false);
            return;
        }

        IsLocked = isOn;
        UpdateLockVisual();
    }

    public void ResetDice()
    {
        Value = 0;
        IsLocked = false;
        hasRolled = false;

        lockToggle.SetIsOnWithoutNotify(false);
        lockToggle.interactable = false;

        faceImage.enabled = false;
        lockOverlay.SetActive(false);
    }

    public void SetToggleInteractable(bool interactable)
    {
        lockToggle.interactable = interactable && hasRolled;
    }

    private void UpdateFaceImage()
    {
        if (Value < 1 || Value > 6)
        {
            faceImage.enabled = false;
            return;
        }

        if (faceSprites == null || faceSprites.Length < 6)
        {
            Debug.LogError($"{gameObject.name}의 Face Sprites에 주사위 이미지 6개가 등록되지 않았습니다.");
            return;
        }

        faceImage.enabled = true;
        faceImage.sprite = faceSprites[Value - 1];
    }

    private void UpdateLockVisual()
    {
        lockOverlay.SetActive(IsLocked);
    }
}