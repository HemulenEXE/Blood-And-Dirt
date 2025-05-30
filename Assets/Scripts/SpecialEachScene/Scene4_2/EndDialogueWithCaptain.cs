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
    private SummonExplosive summonExplosive;


    [SerializeField] GameObject[] needToEnable;
    [SerializeField] GameObject[] needToDisable;

    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
        summonExplosive = GetComponent<SummonExplosive>();

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
            GetComponent<Printer>().enabled = false;
            GetComponent<Talker>().enabled = false;
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
        foreach (GameObject go in needToEnable)
        {
            go.SetActive(true);
        }
        summonExplosive.SummonSound();
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
        foreach (GameObject go in needToDisable)
        {
            go.SetActive(false);
        }
        
        
        Destroy(this.gameObject, 3);
    }
}
