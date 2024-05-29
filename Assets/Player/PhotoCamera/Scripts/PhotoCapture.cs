using System.Collections;
using UnityEngine;

public class PhotoCapture : MonoBehaviour
{
    #region Variables
    [Header("Photo Taker")]
    [SerializeField]
    private Transform cameraBarrelRaycast;

    [SerializeField]
    private float photoDelay;
    public bool canTakePhoto = true;

    [Header("Raycasting")]
    [SerializeField]
    private float sphereCastRadius;

    [SerializeField]
    private float sphereCastLength;

    [SerializeField]
    private LayerMask photoLayer;

    private RaycastHit photoHit;

    [Header("Photo Textures")]
    [SerializeField]
    private RenderTexture cameraRenderTexture;

    private Texture2D screenCapture;

    [Header("Audio")]
    [SerializeField]
    private AudioClip photoSFX;

    [SerializeField]
    private AudioClip failedPhotoSFX;
    #endregion Variables

    #region Lifecycle

    private void OnDrawGizmos()
    {
        // Draws gizmos to visualize photography range
        Gizmos.color = Color.red;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawWireSphere(
                (
                    cameraBarrelRaycast.transform.position
                    - cameraBarrelRaycast.transform.forward * -sphereCastLength / i
                ),
                sphereCastRadius
            );
        }
        Gizmos.DrawRay(
            cameraBarrelRaycast.transform.position,
            cameraBarrelRaycast.transform.forward * sphereCastLength
        );
    }
    #endregion Lifecycle

    #region Functions
    public IEnumerator CapturePhoto()
    {
        if (PhotoCameraUIManager.Instance.activePhotoIndex < 2)
        {
            // Checks if cast sphere hits a valid target
            bool hasHit = Physics.SphereCast(
                cameraBarrelRaycast.transform.position,
                sphereCastRadius,
                cameraBarrelRaycast.transform.forward,
                out photoHit,
                sphereCastLength,
                photoLayer
            );

            // Disables ability take photos
            canTakePhoto = false;
            SoundFXManager.Instance.PlaySoundFXClip(photoSFX, transform, 1);

            yield return new WaitForEndOfFrame();

            // Creates a new texture 2D to store photo information
            screenCapture = new Texture2D(
                cameraRenderTexture.width,
                cameraRenderTexture.height,
                TextureFormat.RGBAHalf,
                false
            );

            // Gets temporary texture information from the camera render texture
            RenderTexture textureTemporary = RenderTexture.GetTemporary(
                cameraRenderTexture.width,
                cameraRenderTexture.height,
                24,
                UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat
            );

            // Stores current render texture to reapply later
            RenderTexture currentActiveRT = RenderTexture.active;

            // Sets temporary render texture as the active render texture
            RenderTexture.active = textureTemporary;

            // Copies camera render texture data into temporary render texture
            Graphics.Blit(cameraRenderTexture, textureTemporary);

            // Defines the size of the texture
            Rect regionToRead = new(0, 0, cameraRenderTexture.width, cameraRenderTexture.height);

            // Copies and applies texture data from render texture area
            screenCapture.ReadPixels(regionToRead, 0, 0, false);
            screenCapture.Apply(true, true);

            // Creates a sprite out of the texture 2D
            Sprite photoSprite = Sprite.Create(
                screenCapture,
                regionToRead,
                new Vector2(0.5f, 0.5f),
                100f
            );

            // Restores original render texture
            RenderTexture.active = currentActiveRT;

            // Releases temporary render texture
            RenderTexture.ReleaseTemporary(textureTemporary);

            string label = null;

            if (hasHit)
            {
                // Checks whether the photographed object was a valid one
                if (
                    photoHit.collider.gameObject.CompareTag(TagStrings.Enemy)
                    || photoHit.collider.gameObject.CompareTag(TagStrings.Food)
                )
                {
                    PhotoObject photoObject = photoHit
                        .collider
                        .gameObject
                        .GetComponent<PhotoObject>();

                    photoObject.WasPhotographed();

                    label = photoObject.data.label;
                }
            }

            // Adds photo to UI
            PhotoCameraUIManager.Instance.AddPhoto(photoSprite, label);

            // Waits for a delay to restore ability take photos
            yield return new WaitForSeconds(photoDelay);
            canTakePhoto = true;
        }
        else
        {
            // Play failed photo SFX
            SoundFXManager.Instance.PlaySoundFXClip(failedPhotoSFX, transform, 1);
        }
    }
    #endregion Functions
}
