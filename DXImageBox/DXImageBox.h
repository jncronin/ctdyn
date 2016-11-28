// DXImageBox.h

#pragma once

using namespace System;
using namespace System::Windows::Forms;

#include <d3d11.h>
#include <stdint.h>

namespace DXImageBox {
	struct BufferVars
	{
		int frame;
		float w;
		float l;
		float t;
		int ap_fl, ap_ll, ap_zs;
		int ap_show;
	};

	public ref class DXImageBox : System::Windows::Forms::Control
	{
		// TODO: Add your methods for this class here.

	private:
		void InitD3D();
		void SetData(array<Int16, 3> ^d);

		ID3D11Device* d3ddev = NULL;
		ID3D11DeviceContext *d3d;
		ID3D11RenderTargetView *bb;
		IDXGISwapChain *sc;
		ID3D11Buffer *pVB;
		ID3D11Texture3D *cvt;

		ID3D11Buffer *cb = NULL;

		System::Drawing::Font ^f;
		System::Drawing::Brush ^fground;

		BufferVars *bv;
		uint32_t *ap_fl, *ap_ll, *ap_zs;

		System::String ^err_str = nullptr;

	public:
		DXImageBox();
		virtual void OnPaint(PaintEventArgs^ e) override;
		virtual void OnPaintBackground(PaintEventArgs^ e) override;

		virtual void OnCreateControl() override;

		property array<Int16, 3> ^ImageData { void set(array<Int16, 3> ^v) { SetData(v); Invalidate(); } }
		property Single Threshold { Single get() { return bv->t; } void set(Single v) { bv->t = v; Invalidate(); } }
		property Boolean ShowZones { Boolean get() { return bv->ap_show == 1; } void set(Boolean v) { if (v == true) bv->ap_show = 1; else bv->ap_show = 0; Invalidate(); } }
		property Int32 Window { Int32 get() { return (int)bv->w; } void set(Int32 v) { bv->w = (float)v; Invalidate(); } }
		property Int32 Level { Int32 get() { return (int)bv->l; } void set(Int32 v) { bv->l = (float)v; Invalidate(); } }
		property Int32 Frame { Int32 get() { return bv->frame; } void set(Int32 v) { bv->frame = v; Invalidate(); } }
	};
}
