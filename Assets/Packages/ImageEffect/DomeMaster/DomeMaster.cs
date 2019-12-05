using UnityEngine;

public class DomeMaster : ImageEffectBase
{
    public CubemapCamera cubemapCamera;

    protected virtual void Awake()
    {
        if (this.cubemapCamera != null)
        {
            this.cubemapCamera.Initialize();
            base.material.SetTexture("_Cubemap", this.cubemapCamera.Cubemap);
        }
    }

    [ContextMenu("SetCubemap")]
    public virtual void SetCubemap()
    {
        // NOTE:
        // Need to SetTexture when the CubemapCamera.Cubemap is updated
        // with CubemapCamera.InitializeCubemap.
        // Because the instance is a difference from the before.

        base.material.SetTexture("_Cubemap", this.cubemapCamera.Cubemap);
    }
}