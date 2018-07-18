using UnityEngine;

[ExecuteInEditMode]
public class Mandelbrot : MonoBehaviour
{
	public Material MandelbrotMaterial;
	public int maxIter = 512;
	public float scale = 2.5f, offsetX = -0.5f, offsetY = 0.0f, speed = 0.5f;
	float aspectRatio = (float)Screen.width / Screen.height;
	Vector4 transformation = new Vector4();

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		aspectRatio = (float)Screen.width / Screen.height;
		transformation.x = transformation.y = scale;
		transformation.z = offsetX;
		transformation.w = offsetY;
		MandelbrotMaterial.SetVector("_ST", transformation);
		MandelbrotMaterial.SetFloat("_maxIter", maxIter);
		MandelbrotMaterial.SetFloat("_aspectRatio", aspectRatio);
		Graphics.Blit(src, dst, MandelbrotMaterial);
	}

	void Update()
	{
		if (Input.GetButton("Up")) offsetY += scale * speed * Time.deltaTime;
		if (Input.GetButton("Down")) offsetY -= scale * speed * Time.deltaTime;
		if (Input.GetButton("Left")) offsetX -= scale * speed * Time.deltaTime * (1f / aspectRatio);
		if (Input.GetButton("Right")) offsetX += scale * speed * Time.deltaTime * (1f / aspectRatio);
		if (Input.GetButton("Zoom In")) scale -= 0.98f * scale * Time.deltaTime;
		if (Input.GetButton("Zoom Out")) scale += 1.02f * scale * Time.deltaTime;
		if (scale > 20.0f) scale = 20.0f;
		if (scale < 0.00002f) scale = 0.00002f;
		if (Input.GetButtonDown("More Iterations"))
		{
			int iter = maxIter * 2;
			if (iter > 16384) maxIter = 16384;
			else maxIter = iter;
		}
		if (Input.GetButtonDown("Less Iterations"))
		{
			float iter = maxIter * 0.5f;
			if (iter < 1) maxIter = 1;
			else maxIter = (int)iter;
		}
		if (Input.GetButtonDown("Restart"))
		{
			scale = 2.5f;
			offsetX = -0.5f;
			offsetY = 0.0f;
			speed = 0.5f;
			maxIter = 512;
		}
		if (Input.GetButtonDown("Quit")) Application.Quit();
	}
}