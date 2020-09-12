#include "hooks.h"
#include "globals.h"

#define GLEW_STATIC
#include "GL\glew.h"
#pragma comment(lib, "glew32s.lib")
#pragma comment(lib, "OpenGL32.lib")

//#include "imgui/imgui.h"
//#define IMGUI_DEFINE_MATH_OPERATORS
//#include "imgui/imgui_internal.h"
//#include "imgui/imgui_impl_opengl3.h"
//#include "imgui/imgui_impl_win32.h"

#include "menu.h"
#include "drawing.h"
#include "BebasNeueRegular.h"

f_wglSwapBuffers H::owglSwapBuffers = NULL;

BOOL __stdcall H::wglSwapBuffers(HDC hDc) {
	static bool once = true;

	static GLuint       g_GlVersion = 0;                // Extracted at runtime using GL_MAJOR_VERSION, GL_MINOR_VERSION queries (e.g. 320 for GL 3.2)
	static char         g_GlslVersionString[32] = "";   // Specified by user or detected based on compile time GL settings.
	static GLuint       g_FontTexture = 0;
	static GLuint       g_ShaderHandle = 0, g_VertHandle = 0, g_FragHandle = 0;
	static GLint        g_AttribLocationTex = 0, g_AttribLocationProjMtx = 0;                                // Uniforms location
	static GLuint       g_AttribLocationVtxPos = 0, g_AttribLocationVtxUV = 0, g_AttribLocationVtxColor = 0; // Vertex attributes location
	static unsigned int g_VboHandle = 0, g_ElementsHandle = 0;
	static GLuint text_xd = NULL;

	if (once) {
		once = false;

		G::hGameWindow = WindowFromDC(hDc);
		//HookWindow(G::hGameWindow);

		glewInit();

		static const char* g_GlslVersionString = "#version 130\n";

		const GLchar* vertex_shader =
			"uniform mat4 ProjMtx;\n"
			"in vec2 Position;\n"
			"in vec2 UV;\n"
			"in vec4 Color;\n"
			"out vec2 Frag_UV;\n"
			"out vec4 Frag_Color;\n"
			"void main()\n"
			"{\n"
			"    Frag_UV = UV;\n"
			"    Frag_Color = Color;\n"
			"    gl_Position = ProjMtx * vec4(Position.xy,0,1);\n"
			"}\n";

		const GLchar* fragment_shader =
			"uniform sampler2D Texture;\n"
			"in vec2 Frag_UV;\n"
			"in vec4 Frag_Color;\n"
			"out vec4 Out_Color;\n"
			"void main()\n"
			"{\n"
			"    Out_Color = Frag_Color * texture(Texture, Frag_UV.st);\n"
			"}\n";

		// Create shaders
		const GLchar* vertex_shader_with_version[2] = { g_GlslVersionString, vertex_shader };
		g_VertHandle = glCreateShader(GL_VERTEX_SHADER);
		glShaderSource(g_VertHandle, 2, vertex_shader_with_version, NULL);
		glCompileShader(g_VertHandle);

		const GLchar* fragment_shader_with_version[2] = { g_GlslVersionString, fragment_shader };
		g_FragHandle = glCreateShader(GL_FRAGMENT_SHADER);
		glShaderSource(g_FragHandle, 2, fragment_shader_with_version, NULL);
		glCompileShader(g_FragHandle);

		g_ShaderHandle = glCreateProgram();
		glAttachShader(g_ShaderHandle, g_VertHandle);
		glAttachShader(g_ShaderHandle, g_FragHandle);
		glLinkProgram(g_ShaderHandle);

		g_AttribLocationProjMtx = glGetUniformLocation(g_ShaderHandle, "ProjMtx");
		g_AttribLocationVtxPos = (GLuint)glGetAttribLocation(g_ShaderHandle, "Position");
		g_AttribLocationVtxUV = (GLuint)glGetAttribLocation(g_ShaderHandle, "UV");
		g_AttribLocationVtxColor = (GLuint)glGetAttribLocation(g_ShaderHandle, "Color");

		// Create buffers
		glGenBuffers(1, &g_VboHandle);
		glGenBuffers(1, &g_ElementsHandle);

		GLint last_texture;
		glGetIntegerv(GL_TEXTURE_BINDING_2D, &last_texture);
		glGenTextures(1, &text_xd);
		glBindTexture(GL_TEXTURE_2D, text_xd);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
#ifdef GL_UNPACK_ROW_LENGTH
		glPixelStorei(GL_UNPACK_ROW_LENGTH, 0);
#endif
		unsigned char reloadImg[] = {
			0xFF, 0xFF, 0xFF, 0xFF,
		};
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, 1, 1, 0, GL_RGBA, GL_UNSIGNED_BYTE, reloadImg);

		glBindTexture(GL_TEXTURE_2D, last_texture);

		return true;
	}

	if (GetAsyncKeyState(VK_DELETE) & 0x1) {
		H::UnhookAll();
	}
	RECT rect;
	::GetClientRect(G::hGameWindow, &rect);
	int fb_width = (int)(rect.right - rect.left);
	int fb_height = (int)(rect.bottom - rect.top);

	glViewport(0, 0, (GLsizei)fb_width, (GLsizei)fb_height);
	float L = 0;
	float R = (float)fb_width;
	float T = 0;
	float B = (float)fb_height;
	const float ortho_projection[4][4] =
	{
		{ 2.0f / (R - L),   0.0f,         0.0f,   0.0f },
		{ 0.0f,         2.0f / (T - B),   0.0f,   0.0f },
		{ 0.0f,         0.0f,        -1.0f,   0.0f },
		{ (R + L) / (L - R),  (T + B) / (B - T),  0.0f,   1.0f },
	};
	glUseProgram(g_ShaderHandle);
	//glUniform1i(g_AttribLocationTex, 0);
	glUniformMatrix4fv(g_AttribLocationProjMtx, 1, GL_FALSE, &ortho_projection[0][0]);

	glBindBuffer(GL_ARRAY_BUFFER, g_VboHandle);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, g_ElementsHandle);
	glEnableVertexAttribArray(g_AttribLocationVtxPos);
	glEnableVertexAttribArray(g_AttribLocationVtxUV);
	glEnableVertexAttribArray(g_AttribLocationVtxColor);
	glVertexAttribPointer(g_AttribLocationVtxPos, 2, GL_FLOAT, GL_FALSE, sizeof(ImDrawVert), (GLvoid*)IM_OFFSETOF(ImDrawVert, pos));
	glVertexAttribPointer(g_AttribLocationVtxUV, 2, GL_FLOAT, GL_FALSE, sizeof(ImDrawVert), (GLvoid*)IM_OFFSETOF(ImDrawVert, uv));
	glVertexAttribPointer(g_AttribLocationVtxColor, 4, GL_UNSIGNED_BYTE, GL_TRUE, sizeof(ImDrawVert), (GLvoid*)IM_OFFSETOF(ImDrawVert, col));

	
	ImVector<ImDrawIdx>     IdxBuffer;          // Index buffer. Each command consume ImDrawCmd::ElemCount of those
	ImVector<ImDrawVert>    VtxBuffer;          // Vertex buffer.

	int vtx_buffer_old_size = VtxBuffer.Size;
	VtxBuffer.resize(vtx_buffer_old_size + 4);
	ImDrawVert* _VtxWritePtr = VtxBuffer.Data + vtx_buffer_old_size;

	int idx_buffer_old_size = IdxBuffer.Size;
	IdxBuffer.resize(idx_buffer_old_size + 6);
	ImDrawIdx* _IdxWritePtr = IdxBuffer.Data + idx_buffer_old_size;

	ImVec2 a(100, 100);
	ImVec2 c(50, 50);
	ImVec2 b(c.x, a.y), d(a.x, c.y), uv(0, 0);
	ImDrawIdx idx = (ImDrawIdx)0;
	_IdxWritePtr[0] = idx; _IdxWritePtr[1] = (ImDrawIdx)(idx + 1); _IdxWritePtr[2] = (ImDrawIdx)(idx + 2);
	_IdxWritePtr[3] = idx; _IdxWritePtr[4] = (ImDrawIdx)(idx + 2); _IdxWritePtr[5] = (ImDrawIdx)(idx + 3);
	_VtxWritePtr[0].pos = a; _VtxWritePtr[0].uv = uv; _VtxWritePtr[0].col = 0xFF0000FF;
	_VtxWritePtr[1].pos = b; _VtxWritePtr[1].uv = uv; _VtxWritePtr[1].col = 0xFF0000FF;
	_VtxWritePtr[2].pos = c; _VtxWritePtr[2].uv = uv; _VtxWritePtr[2].col = 0xFFFF0000;
	_VtxWritePtr[3].pos = d; _VtxWritePtr[3].uv = uv; _VtxWritePtr[3].col = 0xFFFF0000;
	_VtxWritePtr += 4;
	_IdxWritePtr += 6;

	glBufferData(GL_ARRAY_BUFFER, (GLsizeiptr)VtxBuffer.Size * (int)sizeof(ImDrawVert), (const GLvoid*)VtxBuffer.Data, GL_STREAM_DRAW);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, (GLsizeiptr)IdxBuffer.Size * (int)sizeof(ImDrawIdx), (const GLvoid*)IdxBuffer.Data, GL_STREAM_DRAW);
	glBindTexture(GL_TEXTURE_2D, (GLuint)(intptr_t)text_xd);
	glDrawElements(GL_TRIANGLES, (GLsizei)IdxBuffer.Size, sizeof(ImDrawIdx) == 2 ? GL_UNSIGNED_SHORT : GL_UNSIGNED_INT, (void*)(intptr_t)0);

    return owglSwapBuffers(hDc);
}
