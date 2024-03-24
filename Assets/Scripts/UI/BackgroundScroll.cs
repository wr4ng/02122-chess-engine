using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
	[SerializeField] private RawImage rawImage;
	[SerializeField] private Vector2 offset;

    void Update()
    {
		rawImage.uvRect = new Rect(rawImage.uvRect.position + offset * Time.deltaTime, rawImage.uvRect.size);
    }
}
