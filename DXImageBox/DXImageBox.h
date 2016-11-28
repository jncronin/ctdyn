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
		float tex_x_scale, tex_x_offset;
		float tex_y_scale, tex_y_offset;
	};

	public ref class DXImageBox : System::Windows::Forms::Control
	{
	private:
		void InitD3D();
		void SetData(array<Int16, 3> ^d);

		ID3D11Device* d3ddev = NULL;
		ID3D11DeviceContext *d3d;
		ID3D11RenderTargetView *bb;
		IDXGISwapChain *sc;
		ID3D11Buffer *pVB;
		ID3D11Texture3D *cvt = NULL;
		ID3D11Texture2D *bbt;

		ID3D11Buffer *cb = NULL;

		System::Drawing::Font ^f;
		System::Drawing::Brush ^fground;

		BufferVars *bv;
		uint32_t *ap_fl, *ap_ll, *ap_zs;

		int data_x, data_y, data_z;

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

		property Boolean Flip {
			void set(Boolean v)
			{
				if (v) {
					bv->tex_x_scale = (float)(-data_x);
					bv->tex_x_offset = (float)data_x;
					bv->tex_y_scale = (float)(-data_y);
					bv->tex_y_offset = (float)data_y;
				}
				else
				{
					bv->tex_x_scale = (float)data_x;
					bv->tex_x_offset = 0.0f;
					bv->tex_y_scale = (float)data_y;
					bv->tex_y_offset = 0.0f;
				}
				Invalidate();
			}
		}

		virtual array<System::UInt32, 2> ^GetScreenshot();
	};
}
