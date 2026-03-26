using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameView : MonoBehaviour
{
    [SerializeField] public RectTransform gameView;
    [SerializeField] private Camera cam;
    
    private float boardWidth = 10f;
    private float boardHeight = 22f;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    void Start()
    {
        Apply();
    }

    void Apply()
    {
        FitCameraToGameView();
        FitOrthographicSize();
    }

    void FitCameraToGameView()
    {
        Vector3[] corners = new Vector3[4];
        gameView.GetWorldCorners(corners);

        float xMin = corners[0].x / Screen.width;
        float yMin = corners[0].y / Screen.height;
        float xMax = corners[2].x / Screen.width;
        float yMax = corners[2].y / Screen.height;

        cam.rect = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }

    void FitOrthographicSize()
    {
        float aspect = cam.aspect;

        float sizeByHeight = boardHeight / 2f;
        float sizeByWidth = (boardWidth / 2f) / aspect;

        cam.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
    }

    void Update()
    {
        // hỗ trợ resize màn hình
        if (Screen.width != cam.pixelWidth || Screen.height != cam.pixelHeight)
        {
            Apply();
        }
    }
}