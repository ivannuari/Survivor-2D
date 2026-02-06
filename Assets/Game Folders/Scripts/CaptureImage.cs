using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class CaptureImage : MonoBehaviour
{
    public Camera captureCamera;   // kamera khusus untuk render
    public int resolutionWidth = 512;
    public int resolutionHeight = 512;

    public InputAction captureAction;

    void OnEnable()
    {
        captureAction.Enable();
        captureAction.performed += _ => Capture();
    }

    void OnDisable()
    {
        captureAction.Disable();
    }


    private void Update()
    {
        
    }

    public void Capture()
    {
        // Buat RenderTexture sementara
        RenderTexture rt = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        captureCamera.targetTexture = rt;

        // Render ke texture
        Texture2D screenshot = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGBA32, false);
        captureCamera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        screenshot.Apply();

        // Reset
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Encode ke PNG
        byte[] bytes = screenshot.EncodeToPNG();
        int ind = PlayerPrefs.GetInt("Cam", 0);
        string fileName = $"Image_{ind}.png";
        string path = Path.Combine(Application.dataPath, fileName);
        File.WriteAllBytes(path, bytes);

        ind++;
        PlayerPrefs.SetInt("Cam", ind);

        Debug.Log("Captured to: " + path);
    }
}
