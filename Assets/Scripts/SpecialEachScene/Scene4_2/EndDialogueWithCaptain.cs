using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitDialogueEnd : MonoBehaviour
{
    [SerializeField] private TextAsset FileName;
    [SerializeField] private float flashDuration = 2f; // Длительность ослепления
    [SerializeField] private Color flashColor = Color.white; // Цвет вспышки
    [SerializeField] private Canvas flashCanvasPrefab; // Префаб канваса для вспышки

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
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f); // Прозрачный старт
            flashCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Не указан префаб flashCanvasPrefab для вспышки!");
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

        // Плавно увеличиваем прозрачность до 1 (полная белизна)
        float elapsed = 0f;
        while (elapsed < flashDuration / 2)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / (flashDuration / 2));
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        // Пауза на пике белого
        yield return new WaitForSeconds(0.2f);

        // Плавно убираем белый экран
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
