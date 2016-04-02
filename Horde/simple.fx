cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projMatrix;
};


float4 VShader(float3 position : POSITION) : SV_POSITION
{
//	matrix wvp = worldMatrix*viewMatrix*projMatrix;
	float4 pos = mul(float4(position,1.0f),worldMatrix);
	pos = mul(pos, viewMatrix);
	pos = mul(pos, projMatrix);

	return pos;
}

float4 PShader(float4 position : SV_POSITION) : SV_Target
{
	return float4(1.0f, 0.0f, 0.0f, 1.0f);
}