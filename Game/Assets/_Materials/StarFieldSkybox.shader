Shader "Unlit/StarFieldSkybox"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        STAR_ORIGIN("STAR_ORIGIN", Vector) = (0, 0, 0, 1)
        dummy("dummy", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Adapted from http://casual-effects.blogspot.com/2013/08/starfield-shader.html
            float4 STAR_ORIGIN;
            #define STAR_ITERS 19
            #define STAR_SPARSITY 0.9
            #define STAR_VOL_VAR   1.1

            #define STAR_VOL_STEPS 4
            #define STAR_VOL_STSIZE 0.2
            #define STAR_DISTFADING 0.6800

            #define STAR_BRIGHTNESS 0.0018

            float3 starfield(float3 dir) {
                float dist = 0.1, fade = 0.01;
                float3 col = 0.0;

                for (int r = 0; r < STAR_VOL_STEPS; r++) {
                    float3 p = STAR_ORIGIN.xyz * STAR_ORIGIN.w;
                    p += dir * (dist * 0.5);
                    p = abs(STAR_VOL_VAR - (p % (STAR_VOL_VAR * 2.0)));

                    float prevlen = 0.0, a = 0.0;
                    for (int i = 0; i < STAR_ITERS; i++) {
                        p = abs(p);
                        p = p * (1.0 / dot(p, p)) + (-STAR_SPARSITY); // the magic formula            
                        float len = length(p);
                        a += abs(len - prevlen); // absolute sum of average change
                        prevlen = len;
                    }

                    a *= a * a; // add contrast

                    // coloring based on distance        
                    col += (float3(dist, dist * dist, dist * dist * dist) * a * STAR_BRIGHTNESS + 1.0) * fade;
                    fade *= STAR_DISTFADING; // distance fading
                    dist += STAR_VOL_STSIZE;
                }

                return col;
            }

            float3 star_sky(float3 dir) {
                float3 d = normalize(dir);
                d += 1.0;
                d.x *= 1.3;
                d.y *= 0.9;
                return starfield(d);
            }

            float4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float3 col = star_sky(i.uv);

                float4 ret = float4(col, 1.0);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, ret);
                return ret;
            }
            ENDCG
        }
    }
}
