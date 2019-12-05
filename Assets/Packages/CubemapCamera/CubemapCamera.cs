using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class CubemapCamera : MonoBehaviour, IInitializable
{
    [System.Flags]
    public enum FaceMask
    {
        Right  = 1,
        Left   = 1 << 1,
        Top    = 1 << 2,
        Bottom = 1 << 3,
        Front  = 1 << 4,
        Back   = 1 << 5
    }

    [System.Serializable]
    public class TextureEvent : UnityEvent<RenderTexture> { };

    #region Field

    // NOTE:
    // Call "InitializeCubemap" function after update these parameters.
    // "protected serialized Backing Field" is not so good.
    // We need to update and test from Inspector.

    // NOTE:
    // Maybe the WarpMode or any other parameters are need not in here.
    // Because these parameters are not important to use Cubemap.

    [Range(0, 16384)] public int  resolution = 4096; // 2^
    [Range(0,    24)] public int  depth      =   24; // 0, 16, 24
    [Range(0,    16)] public int  anisoLevel =   16; // 0 ~ 16,

    public bool       mipmap     = false;
    public FilterMode filterMode = FilterMode.Trilinear;
    public FaceMask   faceMask   = FaceMask.Right | FaceMask.Left
                                 | FaceMask.Top   | FaceMask.Bottom
                                 | FaceMask.Front | FaceMask.Back;

    private new Camera camera;

    #endregion Field

    #region Property

    public bool IsInitialized { get; protected set; }

    public RenderTexture Cubemap { get; protected set; }

    #endregion Property

    #region Method

    protected virtual void Awake()
    {
        this.Initialize();
    }

    protected virtual void LateUpdate()
    {
        // NOTE:
        // Do not in Upddate().

        this.camera.RenderToCubemap(this.Cubemap, (int)this.faceMask);

        // NOTE:
        // Some components may need to initialize with Cubemap texture.
        // So the following event-driven code is not so good.
        // this.onUpdateCubemap.Invoke(this.Cubemap);
    }

    protected virtual void OnDisable()
    {
        Destroy(this.Cubemap);
    }

    public bool Initialize()
    {
        if (this.IsInitialized)
        {
            return false;
        }

        this.IsInitialized = true;

        this.camera = GetComponent<Camera>();

        InitializeCubemap();

        return true;
    }

    [ContextMenu("Initialize Cubemap")]
    public void InitializeCubemap()
    {
        const int MaxResolution = 16384; // in DirectX 11, 12.
        const int MinResolution =     2;
        const int MaxDepth      =    24;
        const int MinDepth      =     0;

        //this.resolution = (int) Mathf.Pow(2, Mathf.Ceil(Mathf.Log(this.resolution)/Mathf.Log(2)));
        this.resolution = Mathf.Max(Mathf.Min(this.resolution, MaxResolution), MinResolution);

        this.depth = this.depth / 16f < 0.5f ? 0 : this.depth / 16f < 1.25f ? 16 : 24;
        this.depth = Mathf.Max(Mathf.Min(this.depth, MaxDepth), MinDepth);

        if (this.Cubemap != null)
        {
            this.Cubemap.Release();
            Destroy(this.Cubemap);
        }

        this.Cubemap = new RenderTexture(this.resolution,
                                         this.resolution,
                                         this.depth,
                                         RenderTextureFormat.ARGB32)
        {
            dimension        = UnityEngine.Rendering.TextureDimension.Cube,
            hideFlags        = HideFlags.HideAndDontSave,
            autoGenerateMips = this.mipmap,
            useMipMap        = this.mipmap,
            anisoLevel       = this.anisoLevel,
            filterMode       = this.filterMode
        };
    }

    #endregion Method
}