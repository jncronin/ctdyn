/* Copyright (C) 2016 by John Cronin
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:

* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.

* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

const std::string shader = R"SHADER(

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
float tex_x_scale, tex_x_offset;
float tex_y_scale, tex_y_offset;

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
	tex_coords.x = (int)(input.tex_coords.x * tex_x_scale + tex_x_offset);
	tex_coords.y = (int)(input.tex_coords.y * tex_y_scale + tex_y_offset);
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

)SHADER";
