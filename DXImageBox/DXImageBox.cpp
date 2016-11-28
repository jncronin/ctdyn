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

#include "stdafx.h"

#include "DXImageBox.h"
#include <stdint.h>

#include "resource.h"
#include <string>

#include "shaders.shader"

#define TEST(code, msg) if(FAILED((code))) { \
	d3ddev = NULL; \
	d3d = NULL; \
	err_str = (msg); \
	return; \
}

using namespace System::Windows::Forms;

struct CUSTOMVERTEX {
	float x, y, z;
	float r, g, b, a;
};

CUSTOMVERTEX vertices[] =
{
	{ -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f },
	{ 1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f },
	{ -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f },
	{ -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f },
	{ 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f },
	{ 1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f },
};

void DXImageBox::DXImageBox::InitD3D()
{
	// create a struct to hold information about the swap chain
	DXGI_SWAP_CHAIN_DESC scd;

	// clear out the struct for use
	ZeroMemory(&scd, sizeof(DXGI_SWAP_CHAIN_DESC));

	// fill the swap chain description struct
	scd.BufferCount = 1;                                    // one back buffer
	scd.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;     // use 32-bit color
	scd.BufferDesc.Width = 5120;
	scd.BufferDesc.Height = 5120;
	scd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;      // how swap chain is to be used
	scd.OutputWindow = (HWND)Handle.ToPointer();            // the window to be used
	scd.SampleDesc.Count = 1;                               // how many multisamples
	scd.Windowed = TRUE;                                    // windowed/full-screen mode

	IDXGISwapChain *swapchain;             // the pointer to the swap chain interface
	ID3D11Device *dev;                     // the pointer to our Direct3D device interface
	ID3D11DeviceContext *devcon;           // the pointer to our Direct3D device context

	// create a device, device context and swap chain using the information in the scd struct
	auto cdscerr = D3D11CreateDeviceAndSwapChain(NULL,
		D3D_DRIVER_TYPE_HARDWARE,
		NULL,
		NULL,
		NULL,
		NULL,
		D3D11_SDK_VERSION,
		&scd,
		&swapchain,
		&dev,
		NULL,
		&devcon);
	TEST(cdscerr, "CreateDevice");

	d3ddev = dev;
	d3d = devcon;
	sc = swapchain;

	// get the address of the back buffer
	ID3D11Texture2D *pBackBuffer;
	auto gberr = swapchain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*)&pBackBuffer);
	TEST(gberr, "GetBuffer");

	// use the back buffer address to create the render target
	ID3D11RenderTargetView *backbuffer;
	auto crtverr = dev->CreateRenderTargetView(pBackBuffer, NULL, &backbuffer);
	TEST(crtverr, "CreateRenderTargetView");
	bb = backbuffer;
	bbt = pBackBuffer;
	//pBackBuffer->Release();

	// set the render target as the back buffer
	devcon->OMSetRenderTargets(1, &backbuffer, NULL);

	// Set viewport
	D3D11_VIEWPORT viewport;
	ZeroMemory(&viewport, sizeof(D3D11_VIEWPORT));

	viewport.TopLeftX = 0;
	viewport.TopLeftY = 0;
	viewport.Width = (float)scd.BufferDesc.Width;
	viewport.Height = (float)scd.BufferDesc.Height;
	viewport.MinDepth = 0.0f;
	viewport.MaxDepth = 1.0f;

	devcon->RSSetViewports(1, &viewport);

	// Shaders
	ID3DBlob *vs = NULL, *ps = NULL, *vserrors = NULL, *pserrors = NULL;

	//auto vserr = D3DCompileFromFile(L"../../../DXImageBox/shaders.shader", NULL, NULL, "vsmain", "vs_4_0", NULL, NULL, &vs, &vserrors);
	//auto pserr = D3DCompileFromFile(L"../../../DXImageBox/shaders.shader", NULL, NULL, "psmain", "ps_4_0", NULL, NULL, &ps, &pserrors);

	auto vserr = D3DCompile((LPCVOID)shader.c_str(), shader.length(), NULL, NULL, NULL, "vsmain", "vs_4_0", NULL, NULL, &vs, &vserrors);
	auto pserr = D3DCompile((LPCVOID)shader.c_str(), shader.length(), NULL, NULL, NULL, "psmain", "ps_4_0", NULL, NULL, &ps, &pserrors);

	TEST(vserr, "Compile Vertex Shader");
	TEST(pserr, "Compile Pixel Shader");
	
	ID3D11VertexShader *pVS;
	ID3D11PixelShader *pPS;
	auto pvserr = dev->CreateVertexShader(vs->GetBufferPointer(), vs->GetBufferSize(), NULL, &pVS);
	auto ppserr = dev->CreatePixelShader(ps->GetBufferPointer(), ps->GetBufferSize(), NULL, &pPS);

	TEST(pvserr, "CreateVertexShader");
	TEST(ppserr, "CreatePixelShader");

	d3d->VSSetShader(pVS, 0, 0);
	d3d->PSSetShader(pPS, 0, 0);

	// Vertex buffer
	ID3D11Buffer *pVBuffer;
	D3D11_BUFFER_DESC bd;
	ZeroMemory(&bd, sizeof(bd));

	bd.Usage = D3D11_USAGE_DYNAMIC;
	bd.ByteWidth = sizeof(vertices);
	bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
	bd.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;

	auto cberr = dev->CreateBuffer(&bd, NULL, &pVBuffer);
	TEST(cberr, "CreateBuffer");
	pVB = pVBuffer;

	// Fill buffer
	D3D11_MAPPED_SUBRESOURCE ms;
	auto maperr = d3d->Map(pVBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &ms);
	TEST(maperr, "Map");
	memcpy(ms.pData, vertices, sizeof(vertices));
	d3d->Unmap(pVBuffer, NULL);

	// Input element description
	D3D11_INPUT_ELEMENT_DESC ied[] =
	{
		{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 },
		{ "COLOR", 0, DXGI_FORMAT_R32G32B32A32_FLOAT, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0 },
	};
	ID3D11InputLayout *pLayout;
	auto iederr = dev->CreateInputLayout(ied, 2, vs->GetBufferPointer(), vs->GetBufferSize(), &pLayout);
	TEST(iederr, "CreateInputLayout");
	d3d->IASetInputLayout(pLayout);

	/*IDirect3DVertexShader9 *vs9;
	IDirect3DPixelShader9 *ps9;

	auto vs9err = dev->CreateVertexShader((const DWORD *)vs->GetBufferPointer(), &vs9);
	auto ps9err = dev->CreatePixelShader((const DWORD *)ps->GetBufferPointer(), &ps9);

	dev->SetVertexShader(vs9);
	dev->SetPixelShader(ps9);

	auto vd9err = dev->CreateVertexDeclaration(velems, &vd9); */

	
}

void DXImageBox::DXImageBox::SetData(array<Int16, 3>^ d)
{
	if (cvt != NULL)
		cvt->Release();

	if (d == nullptr || !d3d || !d3ddev)
	{
		cvt = NULL;
		return;
	}

	void *data = malloc(sizeof(int32_t) * d->GetLength(2) * d->GetLength(1) * d->GetLength(0));
	if (data == NULL)
	{
		cvt = NULL;
		return;
	}
	uint32_t *ap_fls = (uint32_t *)malloc(sizeof(uint32_t) * d->GetLength(2));
	uint32_t *ap_lls = (uint32_t *)malloc(sizeof(uint32_t) * d->GetLength(2));
	uint32_t *ap_zss = (uint32_t *)malloc(sizeof(uint32_t) * d->GetLength(2));

	int32_t *dp = (int32_t *)data;
	for (int z = 0; z < d->GetLength(0); z++)
	{
		int first_line = d->GetLength(1);
		int last_line = 0;
		for (int y = 0; y < d->GetLength(1); y++)
		{
			for (int x = 0; x < d->GetLength(2); x++)
			{
				auto val = d[z, y, x];
				if (val != -1001 && y < first_line)
					first_line = y;
				if (val != -1001 && y > last_line)
					last_line = y;
				*dp++ = val;
			}
		}

		ap_fls[z] = first_line;
		ap_lls[z] = last_line;
		ap_zss[z] = (last_line - first_line) / 6;
	}

	ap_fl = ap_fls;
	ap_ll = ap_lls;
	ap_zs = ap_zss;

	auto test = d[20, 256, 256];

	/* Data */
	D3D11_TEXTURE3D_DESC td;
	ZeroMemory(&td, sizeof(td));
	td.Width = d->GetLength(2);
	td.Height = d->GetLength(1);
	td.Depth = d->GetLength(0);
	td.MipLevels = 1;
	td.Format = DXGI_FORMAT_R32_SINT;
	td.Usage = D3D11_USAGE_DEFAULT;
	td.CPUAccessFlags = 0;
	td.BindFlags = D3D11_BIND_SHADER_RESOURCE;

	D3D11_SUBRESOURCE_DATA sd;
	ZeroMemory(&sd, sizeof(sd));
	sd.pSysMem = data;
	sd.SysMemPitch = d->GetLength(1) * sizeof(int32_t);
	sd.SysMemSlicePitch = d->GetLength(1) * d->GetLength(2) * sizeof(int32_t);

	ID3D11Texture3D *vt;
	auto ct3derr = d3ddev->CreateTexture3D(&td, &sd, &vt);
	TEST(ct3derr, "CreateTexture3D");

	free(data);

	ID3D11ShaderResourceView* srv[2];
	auto srverr = d3ddev->CreateShaderResourceView(vt, NULL, &srv[0]);
	TEST(srverr, "CreateShaderResourceView");

	d3d->PSSetShaderResources(0, 1, srv);

	cvt = vt;
	data_x = d->GetLength(2);
	data_y = d->GetLength(1);
	data_z = d->GetLength(0);

	bv->tex_x_scale = (float)data_x;
	bv->tex_x_offset = 0.0f;
	bv->tex_y_scale = (float)data_y;
	bv->tex_y_offset = 0.0f;
}

DXImageBox::DXImageBox::DXImageBox()
{
	bv = (BufferVars *)malloc(sizeof(BufferVars));
	ZeroMemory(bv, sizeof(BufferVars));

	bv->frame = 0;
	bv->w = 1400.0f;
	bv->l = -500.0f;
	bv->t = 0.0f;
	bv->ap_fl = 0;
	bv->ap_ll = 0;
	bv->ap_zs = 0;

	f = gcnew System::Drawing::Font("Arial", 14.0f);
	fground = gcnew System::Drawing::SolidBrush(System::Drawing::Color::White);
}

void DXImageBox::DXImageBox::OnPaint(PaintEventArgs ^ e)
{
	if (d3ddev == NULL)
	{
		auto g = e->Graphics;
		g->FillRectangle(gcnew System::Drawing::SolidBrush(System::Drawing::Color::Black),
			e->ClipRectangle);

		if (err_str != nullptr)
		{
			g->DrawString("Direct3D Device Not Initialized (" + err_str + ")", f, fground, 10.0f, 10.0f);
		}
		else
		{
			g->DrawString("Direct3D Device Not Initialized", f, fground, 10.0f, 10.0f);
		}
		return;
	}
	if (cvt == NULL)
	{
		auto g = e->Graphics;
		g->FillRectangle(gcnew System::Drawing::SolidBrush(System::Drawing::Color::Black),
			e->ClipRectangle);
		g->DrawString("No image data", f, fground, 10.0f, 10.0f);
		return;
	}

	/* Create a new constant buffer - delete old one if it exists */

	if (cb != NULL)
		cb->Release();

	bv->ap_fl = ap_fl[bv->frame];
	bv->ap_ll = ap_ll[bv->frame];
	bv->ap_zs = ap_zs[bv->frame];

	D3D11_BUFFER_DESC bd;
	ZeroMemory(&bd, sizeof(bd));
	bd.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	bd.ByteWidth = sizeof(BufferVars);
	bd.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	bd.MiscFlags = 0;
	bd.StructureByteStride = 0;
	bd.Usage = D3D11_USAGE_DYNAMIC;

	D3D11_SUBRESOURCE_DATA bdd;
	void *dsrc = bv;
	bdd.pSysMem = dsrc;
	bdd.SysMemPitch = 0;
	bdd.SysMemSlicePitch = 0;

	ID3D11Buffer *cbuff = NULL;
	auto cberr = d3ddev->CreateBuffer(&bd, &bdd, &cbuff);
	d3d->PSSetConstantBuffers(0, 1, &cbuff);

	cb = cbuff;

	/* Render scene */
	UINT stride = sizeof(CUSTOMVERTEX);
	UINT offset = 0;
	ID3D11Buffer *pVBuffer = pVB;
	d3d->IASetVertexBuffers(0, 1, &pVBuffer, &stride, &offset);
	d3d->IASetPrimitiveTopology(D3D10_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
	d3d->Draw(sizeof(vertices) / sizeof(CUSTOMVERTEX), 0);

	sc->Present(0, 0);

	cb->Release();
}

void DXImageBox::DXImageBox::OnPaintBackground(PaintEventArgs ^ e)
{
}

void DXImageBox::DXImageBox::OnCreateControl()
{
	InitD3D();
}

array <System::UInt32, 2> ^DXImageBox::DXImageBox::GetScreenshot()
{
	if (!d3d || !d3ddev || !bbt)
		return nullptr;

	// Create new texture to hold current backbuffer
	D3D11_TEXTURE2D_DESC bbtdesc;
	ZeroMemory(&bbtdesc, sizeof(bbtdesc));
	bbt->GetDesc(&bbtdesc);

	D3D11_TEXTURE2D_DESC ntdesc;
	ZeroMemory(&ntdesc, sizeof(ntdesc));
	ntdesc.ArraySize = 1;
	ntdesc.BindFlags = 0;
	ntdesc.CPUAccessFlags = D3D11_CPU_ACCESS_READ;
	ntdesc.Format = bbtdesc.Format;
	ntdesc.Height = bbtdesc.Height;
	ntdesc.MipLevels = bbtdesc.MipLevels;
	ntdesc.SampleDesc.Count = 1;
	ntdesc.Usage = D3D11_USAGE_STAGING;
	ntdesc.Width = bbtdesc.Width;

	ID3D11Texture2D *nt;
	auto cterr = d3ddev->CreateTexture2D(&ntdesc, NULL, &nt);

	d3d->CopyResource(nt, bbt);

	D3D11_MAPPED_SUBRESOURCE msr;
	auto maperr = d3d->Map(nt, 0, D3D11_MAP_READ, 0, &msr);

	auto ret = gcnew array<System::UInt32, 2>(data_y, data_x);
	UINT xdiff = ntdesc.Width / data_x;
	UINT ydiff = ntdesc.Height / data_y;
	for (int y = 0; y < data_y; y++)
	{
		uint32_t *cur_row = (uint32_t *)((char *)msr.pData + y * msr.RowPitch * ydiff);
		for (int x = 0; x < data_x; x++)
		{
			uint32_t cur_val = cur_row[x * xdiff];
			ret[y, x] = cur_val;
		}
	}
	d3d->Unmap(nt, 0);

	nt->Release();

	return ret;
}