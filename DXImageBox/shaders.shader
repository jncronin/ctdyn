Texture3D<int> tex : register(t0);
Texture2D<int> ap : register(t1);

int frame;
float w = 1400.0f;
float l = -500.0f;
float threshold = 0.0f;
uint ap_fl;
uint ap_ll;
uint ap_zs;
int ap_show;

SamplerState SampleType
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct VertexInputType
{
	float3 position : POSITION;
	float4 color : COLOR;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex_coords : TEXCOORD0;
	float4 color : COLOR;
};

PixelInputType vsmain(VertexInputType input)
{
	PixelInputType ret;
	ret.position.x = input.position.x;
	ret.position.y = input.position.y;
	ret.position.z = 1.0f;
	ret.position.w = 1.0f;

	float intens = (ret.position.x + ret.position.y + 2.0f) / 4.0f;

	ret.color.x = intens;
	ret.color.y = intens;
	ret.color.z = intens;
	ret.color.w = 1.0f;

	ret.tex_coords.x = (ret.position.x + 1.0f) / 2.0f;
	ret.tex_coords.y = (ret.position.y + 1.0f) / 2.0f;

	return ret;
}

float4 psmain(PixelInputType input) : SV_TARGET
{
	uint4 tex_coords;
	tex_coords.x = (int)(input.tex_coords.x * 512.0f);
	tex_coords.y = (int)(input.tex_coords.y * 512.0f);
	tex_coords.z = frame;
	tex_coords.w = 0;

	//tex_coords.x = 256;
	//tex_coords.y = 256;
	//tex_coords.z = 20;

	int tex_col = tex.Load(tex_coords);

	float tex_colf = (float)tex.Load(tex_coords);

	tex_colf -= l;
	tex_colf /= w;
	//tex_colf = clamp(tex_colf, 0.0f, 1.0f);

	float4 ret;
	ret.x = tex_colf;
	ret.y = tex_colf;
	ret.z = tex_colf;
	ret.w = 1.0f;


	float4 overlay;

	if (tex_col >= -100 && tex_col <= 100)
		overlay = float4(255.0f / 255.0f, 1.0f / 255.0f, 0.0f / 255.0f, threshold);
	else if (tex_col >= -500 && tex_col <= -101)
		overlay = float4(246.0f / 255.0f, 255.0f / 255.0f, 4.0f / 255.0f, threshold);
	else if (tex_col >= -900 && tex_col <= -501)
		overlay = float4(58.0f / 255.0f, 255.0f / 255.0f, 28.0f / 255.0f, threshold);
	else if (tex_col >= -1000 && tex_col <= -901)
		overlay = float4(71.0f / 255.0f, 47.0f / 255.0f, 255.0f / 255.0f, threshold);
	else
		overlay = float4(0.0f, 0.0f, 0.0f, 0.0f);

	ret.xyz = ret.xyz * (1 - overlay.w) + overlay.xyz * overlay.w;

	if (ap_show == 1)
	{
		if ((((tex_coords.y - ap_fl) % ap_zs >= -1 || (tex_coords.y - ap_fl) % ap_zs <= 1) && tex_coords.y <= ap_ll && tex_coords.y >= ap_fl))
		{
			if (tex_coords.x % 8 < 4)
				ret.xyz = 1.0f;
		}
	}


	//ret.x = (tex_coords.x + tex_coords.y) / 2.0f;
	//ret.y = (tex_coords.x + tex_coords.y) / 2.0f;
	//ret.z = (tex_coords.x + tex_coords.y) / 2.0f;


	return ret;
}
