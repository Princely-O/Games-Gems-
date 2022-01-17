Shader "Graph/Point Shader"
{
     Properties {
           _Smoothness ("Smoothness1", Range(0,1)) = 0.5
       }
   SubShader {
       CGPROGRAM
       #pragma surface ConfigureSurface Standard fullforwardshadows
       #pragma target 3.0

       // input struct
       struct Input {
           float3 worldPos;
       };
        
       float _Smoothness; 
       void ConfigureSurface (Input input, inout SurfaceOutputStandard surface){
            surface.Albedo = input.worldPos * 0.5 + 0.5;
            surface.Smoothness = _Smoothness;
       }

       ENDCG
   }

   FallBack "Diffue"
  
}
