#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

// This is a neat trick to work around a bug in the shader graph when
// enabling shadow keywords. Created by @cyanilux
// https://github.com/Cyanilux/URP_ShaderGraphCustomLighting
#ifndef SHADERGRAPH_PREVIEW
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#if (SHADERPASS != SHADERPASS_FORWARD)
#undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
#endif
#endif

struct CustomLightingData {
    // Position and orientation
    float3 positionWS;
    float3 normalWS;
    float3 viewDirectionWS;
    float4 shadowCoord;

    // Surface attributes
    float3 albedo;
    float smoothness;
    float ambientOcclusion;
    float quantization;
    float minDiffuse;

    //baked lighting
    float3 bakedGI;
    float shadowHardness;
};

float GetSmoothnessPower(float rawSmoothness) {
    return exp2(10 * rawSmoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW

float3 CustomGlobalIllumination(CustomLightingData d) {
    float3 indirectDiffuse = d.albedo * d.bakedGI * d.ambientOcclusion;
    return indirectDiffuse;
}

float3 CustomLightHandling(CustomLightingData d, Light light) {
    //Min Clamp > 0 extends light across a surface
    float3 radiance = light.color * clamp((light.distanceAttenuation * (light.shadowAttenuation + d.shadowHardness)), d.minDiffuse, 1);

    float quant = d.quantization;
    bool inc = false;
    for (float i = 0; i < quant + 1; i++) {
        float calc = 1 - (i / quant);
        inc = false;
        if (radiance.x > calc) {
            radiance.x *= calc;
            inc = true;
        }
        if (radiance.y > calc) {
            radiance.y *= calc;
            inc = true;
        }
        if (radiance.y > calc) {
            radiance.y *= calc;
            inc = true;
        }
        if (inc) {
            i = quant + 1;
        }
    }

    float diffuse = saturate(dot(d.normalWS, light.direction)); //put into ramp sampler
    //custom ramp, works, kinda, replaced with a ramp of radiance
   /* float quant = 8;
    for (float i = 0; i < quant + 1; i++) {
        float calc = 1 - (i / quant);

        if (diffuse > calc) {
            diffuse *= calc;
            i = quant+1;
        }
    }*/

    float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
    float specular = pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse;

    float3 color = d.albedo * radiance * (diffuse + specular);
    //sample texture colour with dot product and multiply colour?


    return color;
}
#endif

float3 CalculateCustomLighting(CustomLightingData d) {
#ifdef SHADERGRAPH_PREVIEW
    // In preview, estimate diffuse + specular
    float3 lightDir = float3(0.5, 0.5, 0);
    float intensity = saturate(dot(d.normalWS, lightDir)) + pow(saturate(dot(d.normalWS, normalize(d.viewDirectionWS + lightDir))), GetSmoothnessPower(d.smoothness));
    return d.albedo * intensity;
#else   
    // Get the main light. Located in URP/ShaderLibrary/Lighting.hlsl
    Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, 1);

    MixRealtimeAndBakedGI(mainLight, d.normalWS, d.bakedGI);
    float3 color = CustomGlobalIllumination(d);
    // Shade the main light
    color += CustomLightHandling(d, mainLight);

#ifdef _ADDITIONAL_LIGHTS
    uint numAdditionalLights = GetAdditionalLightsCount();
    for (uint lightI = 0; lightI < numAdditionalLights; lightI++) {
        Light light = GetAdditionalLight(lightI, d.positionWS, 1);
        color += CustomLightHandling(d, light);
    }
#endif


    return color;
#endif
}





void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 ViewDirection, float3 Albedo, float Smoothness, float AmbientOcclusion, float2 LightMapUV, float Quants, float ShadowHardness, float MinDiffuse,
    out float3 Color) {

    CustomLightingData d;
    d.positionWS = Position;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.albedo = Albedo;
    d.smoothness = Smoothness;
    d.ambientOcclusion = AmbientOcclusion;
    d.quantization = Quants;
    d.shadowHardness = ShadowHardness;
    d.minDiffuse = MinDiffuse;

#ifdef SHADERGRAPH_PREVIEW
    // In preview, there's no shadows or bakedGI
    d.shadowCoord = 0;
    d.bakedGI = 0;
#else
    // Calculate the main light shadow coord
    // There are two types depending on if cascades are enabled
#if SHADOWS_SCREEN
    float4 positionCS = TransformWorldToHClip(Position);
    d.shadowCoord = ComputeScreenPos(positionCS);
#else
    d.shadowCoord = TransformWorldToShadowCoord(Position);
#endif

    float3 lightMapUV;

    OUTPUT_LIGHTMAP_UV(LightmapUV, unity_LightmapST, lightmapUV);
    float3 vertexSH;
    OUTPUT_SH(Normal, vertexSH);
    d.bakedGI = SAMPLE_GI(lightmapUV, vertexSH, Normal);


#endif

    Color = CalculateCustomLighting(d);

}

#endif