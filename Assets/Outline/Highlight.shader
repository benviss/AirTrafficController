﻿Shader "Custom/Highlight" {

    Properties{
        _MainTex("Main Texture", 2D) = "black" {}
    }

        CGINCLUDE

        fixed4 _Color;
    uniform float4 _MainTex_TexelSize;

    float4 simple_vert(float4 v:POSITION) : POSITION{
        return UnityObjectToClipPos(v);
    }

        fixed4 simple_frag() : COLOR{
            return _Color;
    }

        ENDCG

        SubShader {

        ZWrite Off
            ZTest Always
            Cull Off

            // OVERLAY GLOW		
            Pass{
                CGPROGRAM
                    #pragma vertex vert_img
                    #pragma fragment frag
                    #pragma fragmentoption ARB_precision_hint_fastest
                    #include "UnityCG.cginc"

                    sampler2D _MainTex;
                    sampler2D _SecondaryTex;

                    fixed _ControlValue;

                    fixed4 frag(v2f_img IN) : COLOR
                    {
                        fixed4 mCol = tex2D(_MainTex, IN.uv);
                        #if UNITY_UV_STARTS_AT_TOP
                        if (_MainTex_TexelSize.y < 0)
                            IN.uv.y = 1.0 - IN.uv.y;
                        #endif

                        fixed3 overCol = tex2D(_SecondaryTex, IN.uv) * _ControlValue;
                        return mCol + fixed4(overCol, 1.0);
                    }
                ENDCG
        }

            // OVERLAY SOLID
                        Pass{

                            CGPROGRAM
                                #pragma vertex vert_img
                                #pragma fragment frag
                                #pragma fragmentoption ARB_precision_hint_fastest
                                #include "UnityCG.cginc"

                                sampler2D _MainTex;
                                sampler2D _SecondaryTex;

                                fixed _ControlValue;

                                fixed4 frag(v2f_img IN) : COLOR
                                {
                                    fixed4 mCol = tex2D(_MainTex, IN.uv);

                                    #if UNITY_UV_STARTS_AT_TOP
                                    if (_MainTex_TexelSize.y < 0)
                                        IN.uv.y = 1.0 - IN.uv.y;
                                    #endif
                                    fixed4 oCol = tex2D(_SecondaryTex, IN.uv);

                                    fixed solid = step(1.0 - _ControlValue, oCol.a);
                                    return mCol + solid * fixed4(oCol.rgb, 1.0);
                                }
                            ENDCG
                    }


                        // OCCLUSION	
                                    Pass{
                                            CGPROGRAM
                                            #pragma vertex vert_img
                                            #pragma fragment frag
                                            #pragma fragmentoption ARB_precision_hint_fastest
                                            #include "UnityCG.cginc"

                                            sampler2D _MainTex;
                                            sampler2D _SecondaryTex;

                                            fixed4 frag(v2f_img IN) : COLOR
                                            {
                                                fixed4 occludeCol = tex2D(_SecondaryTex, IN.uv);
                                                return tex2D(_MainTex, IN.uv) - occludeCol.aaaa;
                                            }

                                        ENDCG
                                }

                                    // Draw to render texture, overlayed.
                                                Pass{

                                                    CGPROGRAM

                                                    #pragma vertex simple_vert
                                                    #pragma fragment simple_frag

                                                    ENDCG
                                            }

                                                // 4 Draw to render texture, depth filtered.
                                                Pass{

                                                    ZTest LEqual

                                                    CGPROGRAM

                                                    #pragma vertex simple_vert
                                                    #pragma fragment simple_frag

                                                    ENDCG
                                            }
    }
}
