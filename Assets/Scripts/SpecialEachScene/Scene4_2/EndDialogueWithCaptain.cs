using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitDialogueEnd : MonoBehaviour
{
    [SerializeField] private TextAsset FileName;
    [SerializeField] private float flashDuration = 2f; // ������������ ����������
    [SerializeField] private Color flashColor = Color.white; // ���� �������
    [SerializeField] private Canvas flashCanvasPrefab; // ������ ������� ��� �������

    private Dialogue _dialogue;
    private GameObject DialogueWindow;
    private ShowDialogueDubl _director;
    private bool isDialogueFinished = false;
    private Canvas flashCanvas;
    private Image flashImage;


    [SerializeField] GameObject[] needToEnable;
    [SerializeField] GameObject[] needToDisable;

    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();

        if (flashCanvasPrefab != null)
        {
            flashCanvas = Instantiate(flashCanvasPrefab);
            flashImage = flashCanvas.GetComponentInChildren<Image>();
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f); // ���������� �����
            flashCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("�� ������ ������ flashCanvasPrefab ��� �������!");
        }
    }

    private void Update()
    {
        if (_director.FileName == FileName)
        {
            DialogueWindow = _director.DialogueWindow.gameObject;
            _dialogue = _director.GetDialogue();
        }

        if (!isDialogueFinished && !DialogueWindow.activeSelf && _dialogue.GetCurentNode().exit == "True")
        {
            isDialogueFinished = true;
            StartCoroutine(FlashScreen());
            foreach (GameObject go in needToDisable) 
            {
                go.SetActive(false);
            }
            foreach (GameObject go in needToEnable)
            {
                go.SetActive(true);
            }
            Destroy(this.gameObject);
        }
    }

    private IEnumerator FlashScreen()
    {
        if (flashCanvas == null)
            yield break;

        flashCanvas.gameObject.SetActive(true);

        // ������ ����������� ������������ �� 1 (������ �������)
        float elapsed = 0f;
        while (elapsed < flashDuration / 2)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / (flashDuration / 2));
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        // ����� �� ���� ������
        yield return new WaitForSeconds(0.2f);

        // ������ ������� ����� �����
        elapsed = 0f;
        while (elapsed < flashDuration / 2)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / (flashDuration / 2));
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        flashCanvas.gameObject.SetActive(false);
    }
}
