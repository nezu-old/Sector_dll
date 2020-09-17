using Sector_dll.util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Sector_dll.util.OpenGL;

namespace Sector_dll.cheat.Hooks
{
    unsafe class GL
    {
        static bool glinit = false;

        static uint g_VertHandle;
        static uint g_FragHandle;
        static uint g_ShaderHandle;
        static int  g_AttribLocationProjMtx;
        static uint g_AttribLocationVtxPos;
        static uint g_AttribLocationVtxUV;
        static uint g_AttribLocationVtxColor;
#pragma warning disable IDE0044 // Add readonly modifier
        static uint g_VboHandle;
        static uint g_ElementsHandle;
        static uint g_texture;
#pragma warning restore IDE0044 // Add readonly modifier

        static IntPtr ListToPtr<T>(IList<T> list, out int size) where T: unmanaged
        {
            size = list.Count * sizeof(T);
            IntPtr addr = Marshal.AllocHGlobal(size);
            T* ptrBuffer = (T*)addr;
            foreach (T ds in list)
            {
                *ptrBuffer = ds;
                ptrBuffer += 1;
            }
            return addr;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SwapBuffers(Action<object, object> orig, object self, object a1)
        {
            object renderer = SignatureManager.RendererWrapper_Renderer.GetValue(self);
            IntPtr hdc = (IntPtr)SignatureManager.Renderer_hdc.GetValue(renderer);
            if (WglGetCurrentContext() != IntPtr.Zero)
            {
                if (!glinit)
                {
                    glinit = true;
                    InitOpenGL();

                    string vertex_shader =
                        "#version 130\n"
                        + "uniform mat4 ProjMtx;\n"
                        + "in vec2 Position;\n"
                        + "in vec2 UV;\n"
                        + "in vec4 Color;\n"
                        + "out vec2 Frag_UV;\n"
                        + "out vec4 Frag_Color;\n"
                        + "void main()\n"
                        + "{\n"
                        + "    Frag_UV = UV;\n"
                        + "    Frag_Color = Color;\n"
                        + "    gl_Position = ProjMtx * vec4(Position.xy,0,1);\n"
                        + "}\n";

                    string fragment_shader =
                        "#version 130\n"
                        + "uniform sampler2D Texture;\n"
                        + "in vec2 Frag_UV;\n"
                        + "in vec4 Frag_Color;\n"
                        + "out vec4 Out_Color;\n"
                        + "void main()\n"
                        + "{\n"
                        + "    Out_Color = Frag_Color * texture(Texture, Frag_UV.st);\n"
                        + "}\n";

                    g_VertHandle = glCreateShader(GL_VERTEX_SHADER);
                    glShaderSource(g_VertHandle, 1, new string[] { vertex_shader }, IntPtr.Zero);
                    glCompileShader(g_VertHandle);

                    g_FragHandle = glCreateShader(GL_FRAGMENT_SHADER);
                    glShaderSource(g_FragHandle, 1, new string[] { fragment_shader }, IntPtr.Zero);
                    glCompileShader(g_FragHandle);

                    g_ShaderHandle = glCreateProgram();
                    glAttachShader(g_ShaderHandle, g_VertHandle);
                    glAttachShader(g_ShaderHandle, g_FragHandle);
                    glLinkProgram(g_ShaderHandle);

                    g_AttribLocationProjMtx = glGetUniformLocation(g_ShaderHandle, "ProjMtx");
                    g_AttribLocationVtxPos = (uint)glGetAttribLocation(g_ShaderHandle, "Position");
                    g_AttribLocationVtxUV = (uint)glGetAttribLocation(g_ShaderHandle, "UV");
                    g_AttribLocationVtxColor = (uint)glGetAttribLocation(g_ShaderHandle, "Color");

                    fixed (uint* g_VboHandlePtr = &g_VboHandle)
                        glGenBuffers(1, g_VboHandlePtr);
                    fixed (uint* g_ElementsHandlePtr = &g_ElementsHandle)
                        glGenBuffers(1, g_ElementsHandlePtr);

                    int last_texture;
                    glGetIntegerv(GL_TEXTURE_BINDING_2D, &last_texture);
                    fixed (uint* g_texturePtr = &g_texture)
                        glGenTextures(1, g_texturePtr);
                    glBindTexture(GL_TEXTURE_2D, g_texture);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
                    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
                    glPixelStorei(GL_UNPACK_ROW_LENGTH, 0);
                    byte[] texture = { 0xFF, 0xFF, 0xFF, 0xFF }; //white
                    fixed (void* texturePtr = texture)
                        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, 1, 1, 0, GL_RGBA, GL_UNSIGNED_BYTE, texturePtr);
                    glBindTexture(GL_TEXTURE_2D, (uint)last_texture);

                    Log.Info(g_ShaderHandle);
                }

                IntPtr hGameWindow = WindowFromDC(hdc);
                GetClientRect(hGameWindow, out RECT rect);
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                glViewport(0, 0, width, height);
                float[,] ortho_projection = new float[4, 4]
                {
                    { 2.0f/width, 0.0f,         0.0f,  0.0f },
                    { 0.0f,       2.0f/-height, 0.0f,  0.0f },
                    { 0.0f,       0.0f,         -1.0f, 0.0f },
                    { -1.0f,      1.0f,         0.0f,  1.0f },
                };
                glUseProgram(g_ShaderHandle);
                fixed (float* ortho_projectionePtr = ortho_projection)
                    glUniformMatrix4fv(g_AttribLocationProjMtx, 1, false, ortho_projectionePtr);

                glBindBuffer(GL_ARRAY_BUFFER, g_VboHandle);
                glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, g_ElementsHandle);
                glEnableVertexAttribArray(g_AttribLocationVtxPos);
                glEnableVertexAttribArray(g_AttribLocationVtxUV);
                glEnableVertexAttribArray(g_AttribLocationVtxColor);

                glVertexAttribPointer(g_AttribLocationVtxPos,   2, GL_FLOAT,         false, 20, (void*)0);
                glVertexAttribPointer(g_AttribLocationVtxUV,    2, GL_FLOAT,         false, 20, (void*)8);
                glVertexAttribPointer(g_AttribLocationVtxColor, 4, GL_UNSIGNED_BYTE, true,  20, (void*)16);


                List<DrawVert>  VtxBuffer = new List<DrawVert>();
                List<ushort>    IdxBuffer = new List<ushort>();

                VtxBuffer.Add(new DrawVert(50,  50,  0xFF0000FF));
                VtxBuffer.Add(new DrawVert(100, 50,  0xFFFF0000));
                VtxBuffer.Add(new DrawVert(50,  100, 0xFFFF0000));
                VtxBuffer.Add(new DrawVert(100, 100, 0xFF0000FF));

                ushort idx = 0;
                IdxBuffer.Add((ushort)(idx + 0));
                IdxBuffer.Add((ushort)(idx + 1));
                IdxBuffer.Add((ushort)(idx + 2));

                IdxBuffer.Add((ushort)(idx + 1));
                IdxBuffer.Add((ushort)(idx + 2));
                IdxBuffer.Add((ushort)(idx + 3));

                IntPtr VtxBufferPtr = ListToPtr(VtxBuffer, out int VtxBufferSize);
                IntPtr IdxBufferPtr = ListToPtr(IdxBuffer, out int IdxBufferSize);

                glBufferData(GL_ARRAY_BUFFER,         (int*)VtxBufferSize, VtxBufferPtr.ToPointer(), GL_STREAM_DRAW);
                glBufferData(GL_ELEMENT_ARRAY_BUFFER, (int*)IdxBufferSize, IdxBufferPtr.ToPointer(), GL_STREAM_DRAW);

                glBindTexture(GL_TEXTURE_2D, g_texture);
                glDrawElements(GL_TRIANGLES, IdxBuffer.Count, GL_UNSIGNED_SHORT, (void*)0);

                Marshal.FreeHGlobal(VtxBufferPtr);
                Marshal.FreeHGlobal(IdxBufferPtr);

            }
            orig(self, a1);
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct DrawVert
        {
            public DrawVert(float x, float y, uint color)
            {
                posX = x;
                posY = y;
                uvX = 0;
                uvY = 0;
                col = color;
            }

            [FieldOffset(0)]
            public float posX;
            [FieldOffset(4)]
            public float posY;
            [FieldOffset(8)]
            public float uvX;
            [FieldOffset(12)]
            public float uvY;
            [FieldOffset(16)]
            public uint col;
        }

        [DllImport("opengl32.dll", EntryPoint = "wglGetCurrentContext", SetLastError = true)]
        public static extern IntPtr WglGetCurrentContext();

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromDC(IntPtr hDC);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

    }
}
