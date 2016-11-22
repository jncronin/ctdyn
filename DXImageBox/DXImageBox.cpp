// This is the main DLL file.

#include "stdafx.h"

#include "DXImageBox.h"
#include <stdint.h>

using namespace System::Windows::Forms;

typedef float RGBA[4]; //pre-c++11

struct CUSTOMVERTEX {
	float x, y, z;
	float r, g, b, a;
};

/*CUSTOMVERTEX vertices[] =
{
	{ 0.0f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f },
	{ 0.45f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f },
	{ -0.45f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f }
};*/

CUSTOMVERTEX vertices[] =
{
	{ -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f },
	{ 1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f },
	{ -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f },
	{ -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f },
	{ 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f },
	{ 1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f },
};

/*D3DVERTEXELEMENT9 velems[] =
{
	{ 0, 0, D3DDECLTYPE_FLOAT3, D3DDECLMETHOD_DEFAULT, D3DDECLUSAGE_POSITION, 0 },
	{ 0, 12, D3DDECLTYPE_FLOAT2, D3DDECLMETHOD_DEFAULT, D3DDECLUSAGE_TEXCOORD, 0 },
	D3DDECL_END()
};

LPDIRECT3DVERTEXBUFFER9 vb;
IDirect3DVertexDeclaration9 *vd9;*/

	
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
	scd.SampleDesc.Count = 4;                               // how many multisamples
	scd.Windowed = TRUE;                                    // windowed/full-screen mode

	IDXGISwapChain *swapchain;             // the pointer to the swap chain interface
	ID3D11Device *dev;                     // the pointer to our Direct3D device interface
	ID3D11DeviceContext *devcon;           // the pointer to our Direct3D device context

	// create a device, device context and swap chain using the information in the scd struct
	D3D11CreateDeviceAndSwapChain(NULL,
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

	d3ddev = dev;
	d3d = devcon;
	sc = swapchain;

	// get the address of the back buffer
	ID3D11Texture2D *pBackBuffer;
	swapchain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*)&pBackBuffer);

	// use the back buffer address to create the render target
	ID3D11RenderTargetView *backbuffer;
	dev->CreateRenderTargetView(pBackBuffer, NULL, &backbuffer);
	bb = backbuffer;
	pBackBuffer->Release();

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
	auto vserr = D3DCompileFromFile(L"../../../DXImageBox/shaders.shader", NULL, NULL, "vsmain", "vs_4_0", NULL, NULL, &vs, &vserrors);
	auto pserr = D3DCompileFromFile(L"../../../DXImageBox/shaders.shader", NULL, NULL, "psmain", "ps_4_0", NULL, NULL, &ps, &pserrors);

	if (FAILED(vserr) || FAILED(pserr))
	{
		d3ddev = NULL;
		return;
	}
	
	ID3D11VertexShader *pVS;
	ID3D11PixelShader *pPS;
	auto pvserr = dev->CreateVertexShader(vs->GetBufferPointer(), vs->GetBufferSize(), NULL, &pVS);
	auto ppserr = dev->CreatePixelShader(ps->GetBufferPointer(), ps->GetBufferSize(), NULL, &pPS);

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

	dev->CreateBuffer(&bd, NULL, &pVBuffer);
	pVB = pVBuffer;

	// Fill buffer
	D3D11_MAPPED_SUBRESOURCE ms;
	d3d->Map(pVBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &ms);
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
	if (d == nullptr)
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

	free(data);

	ID3D11ShaderResourceView* srv[2];
	auto srverr = d3ddev->CreateShaderResourceView(vt, NULL, &srv[0]);

	d3d->PSSetShaderResources(0, 1, srv);

	cvt = vt;
}

DXImageBox::DXImageBox::DXImageBox()
{
	bv = (BufferVars *)malloc(sizeof(BufferVars));

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
		g->DrawString("Direct3D Device Not Initialized", f, fground, 10.0f, 10.0f);
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
	//d3d->ClearRenderTargetView(bb, RGBA{ 0.0f, 0.2f, 0.4f, 1.0f });

	UINT stride = sizeof(CUSTOMVERTEX);
	UINT offset = 0;
	ID3D11Buffer *pVBuffer = pVB;
	d3d->IASetVertexBuffers(0, 1, &pVBuffer, &stride, &offset);
	d3d->IASetPrimitiveTopology(D3D10_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
	d3d->Draw(sizeof(vertices) / sizeof(CUSTOMVERTEX), 0);

	sc->Present(0, 0);
}

void DXImageBox::DXImageBox::OnPaintBackground(PaintEventArgs ^ e)
{
}

void DXImageBox::DXImageBox::OnCreateControl()
{
	InitD3D();
}
