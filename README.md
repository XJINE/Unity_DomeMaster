# Unity_DomeMaster

<img src="https://github.com/XJINE/Unity_DomeMaster/blob/master/Screenshot.png" width="100%" height="auto" />

DomeMaster is an ImageEffect to make Cubemap into DomeMaster format.

## Import to Your Project

You can import this asset from UnityPackage.

- [DomeMaster.unitypackage](https://github.com/XJINE/Unity_DomeMaster/blob/master/DomeMaster.unitypackage)

### Dependencies

You have to import following assets to use this asset.

- [Unity_ImageEffectBase](https://github.com/XJINE/Unity_ImageEffectBase)
- [Unity_CubemapCamera](https://github.com/XJINE/Unity_CubemapCamera)

## How to Use

 1. Set ``CubemapCamera`` into a Camera. Then, disable the camera.
 2. Set ``DomeMaster`` into an another Camera.
 3. Or, you can set your own Cubemap texture into DomeMaster material.

DomeMaster has 3 parameter, ``LOD``, ``Angle`` and ``Rotation``.