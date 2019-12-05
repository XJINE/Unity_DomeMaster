Shader "Unlit/DomeMaster"
{
    Properties
    {
        _Cubemap    ("Cubemap",   Cube)          = "" {}
        _Lod        ("LOD",       Range( 0, 32)) = 0
        _Angle      ("Angle",     Range( 0, 10)) = 0.5
        _Rotation   ("Rotation",  Vector)        = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Pass
        {
            CGPROGRAM

            #pragma target   3.0
            #pragma vertex   vert
            #pragma fragment frag

            #define PI          3.1415926
            #define PI_HALF     0.5 * PI
            #define DEG_TO_RAD  0.0174533

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4   vertex   : SV_POSITION;
                float2   uv       : TEXCOORD0;
                float3x3 rotation : TEXCOORD1;
            };

            
            UNITY_DECLARE_TEXCUBE(_Cubemap);

            float  _Lod;
            float  _Angle;
            float4 _Rotation;

            float3x3 RotationMatrix(float3 radian)
            {
                float _sinX, _cosX;
                float _sinY, _cosY;
                float _sinZ, _cosZ;

                sincos(radian.x, _sinX, _cosX);
                sincos(radian.y, _sinY, _cosY);
                sincos(radian.z, _sinZ, _cosZ);

                float3 row1 = float3(_sinX * _sinY *_sinZ + _cosY * _cosZ, _sinX *_sinY *_cosZ - _cosY * _sinZ,        _cosX * _sinY);
                float3 row2 = float3(                       _cosX *                                      _sinZ, _cosX *_cosZ, -_sinX);
                float3 row3 = float3(_sinX * _cosY *_sinZ - _sinY * _cosZ, _sinX *_cosY *_cosZ + _sinY * _sinZ,        _cosX * _cosY);

                return float3x3(row1, row2, row3);
            }

            v2f vert (appdata v)
            {
                // NOTE:
                // Need to flip.

                v2f o;
                o.vertex   = UnityObjectToClipPos(v.vertex);
                o.uv       = 2.0 * v.uv - 1.0;
                o.rotation = RotationMatrix(_Rotation * DEG_TO_RAD);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 xy = i.uv;

                float radius = sqrt(dot(xy, xy));
                float phi    = radius * PI_HALF / _Angle;
                float theta  = atan2(xy.y, xy.x);
                float projxy = sin(phi);

                float3 v = float3(cos(theta), sin(theta), cos(phi)) * float3(projxy, projxy, 1);
                       v = mul(i.rotation, v);

                float4 color = UNITY_SAMPLE_TEXCUBE_LOD(_Cubemap, v, _Lod);

                return radius <= 1.0 ? color : 0;
            }

            ENDCG
        }
    }
}