using MonoMod.RuntimeDetour;
using Sector_dll.util;
using sectorsedge.cheat.Drawing;
using sectorsedge.cheat.Drawing.Fonts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Sector_dll.util.OpenGL;

namespace Sector_dll.cheat.Hooks
{
    unsafe class GL
    {
        public static bool glinit = false;

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

                try
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

                        int last_texture1;
                        glGetIntegerv(GL_TEXTURE_BINDING_2D, &last_texture1);
                        foreach (IFont font in Drawing.fonts)
                        {
                            uint new_texture;
                            glGenTextures(1, &new_texture);
                            glBindTexture(GL_TEXTURE_2D, new_texture);
                            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
                            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
                            glPixelStorei(GL_UNPACK_ROW_LENGTH, 0);

                            byte[] texture = font.GetTextureData(out int w, out int h);
                            fixed (void* texturePtr = texture)
                                glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, w, h, 0, GL_RGBA, GL_UNSIGNED_BYTE, texturePtr);
                            font.Texture_id = new_texture;
                        }
                        glBindTexture(GL_TEXTURE_2D, (uint)last_texture1);

                        Drawing.NewFrame();
                    }

                    int last_active_texture; glGetIntegerv(GL_ACTIVE_TEXTURE, &last_active_texture);
                    glActiveTexture(GL_TEXTURE0);
                    int last_program; glGetIntegerv(GL_CURRENT_PROGRAM, &last_program);
                    int last_texture; glGetIntegerv(GL_TEXTURE_BINDING_2D, &last_texture);
                    int last_sampler; glGetIntegerv(GL_SAMPLER_BINDING, &last_sampler);
                    int last_array_buffer; glGetIntegerv(GL_ARRAY_BUFFER_BINDING, &last_array_buffer);
                    int last_vertex_array_object; glGetIntegerv(GL_VERTEX_ARRAY_BINDING, &last_vertex_array_object);
                    int[] last_polygon_mode = new int[2];
                    fixed (int* last_polygon_modePtr = &last_polygon_mode[0])
                        glGetIntegerv(GL_POLYGON_MODE, last_polygon_modePtr);
                    int[] last_viewport = new int[4];
                    fixed (int* last_viewportPtr = &last_viewport[0])
                        glGetIntegerv(GL_VIEWPORT, last_viewportPtr);
                    int[] last_scissor_box = new int[4];
                    fixed (int* last_scissor_boxPtr = &last_scissor_box[0])
                        glGetIntegerv(GL_SCISSOR_BOX, last_scissor_boxPtr);
                    int last_blend_src_rgb; glGetIntegerv(GL_BLEND_SRC_RGB, &last_blend_src_rgb);
                    int last_blend_dst_rgb; glGetIntegerv(GL_BLEND_DST_RGB, &last_blend_dst_rgb);
                    int last_blend_src_alpha; glGetIntegerv(GL_BLEND_SRC_ALPHA, &last_blend_src_alpha);
                    int last_blend_dst_alpha; glGetIntegerv(GL_BLEND_DST_ALPHA, &last_blend_dst_alpha);
                    int last_blend_equation_rgb; glGetIntegerv(GL_BLEND_EQUATION_RGB, &last_blend_equation_rgb);
                    int last_blend_equation_alpha; glGetIntegerv(GL_BLEND_EQUATION_ALPHA, &last_blend_equation_alpha);
                    bool last_enable_blend = glIsEnabled(GL_BLEND);
                    bool last_enable_cull_face = glIsEnabled(GL_CULL_FACE);
                    bool last_enable_depth_test = glIsEnabled(GL_DEPTH_TEST);
                    bool last_enable_scissor_test = glIsEnabled(GL_SCISSOR_TEST);

                    glEnable(GL_BLEND);
                    glBlendEquation(GL_FUNC_ADD);
                    glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
                    glDisable(GL_CULL_FACE);
                    glDisable(GL_DEPTH_TEST);
                    glEnable(GL_SCISSOR_TEST);
                    glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);

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

                    glVertexAttribPointer(g_AttribLocationVtxPos, 2, GL_FLOAT, false, 20, (void*)0);
                    glVertexAttribPointer(g_AttribLocationVtxUV, 2, GL_FLOAT, false, 20, (void*)8);
                    glVertexAttribPointer(g_AttribLocationVtxColor, 4, GL_UNSIGNED_BYTE, true, 20, (void*)16);

                    
                    Drawing.Draw();

                    IntPtr VtxBufferPtr = ListToPtr(Drawing.VtxBuffer, out int VtxBufferSize);
                    IntPtr IdxBufferPtr = ListToPtr(Drawing.IdxBuffer, out int IdxBufferSize);

                    glBufferData(GL_ARRAY_BUFFER, (int*)VtxBufferSize, VtxBufferPtr.ToPointer(), GL_STREAM_DRAW);
                    glBufferData(GL_ELEMENT_ARRAY_BUFFER, (int*)IdxBufferSize, IdxBufferPtr.ToPointer(), GL_STREAM_DRAW);

                    glScissor(0, 0, width, height);
                    foreach (DrawCmd cmd in Drawing.CmdBuffer)
                    {
                        glBindTexture(GL_TEXTURE_2D, cmd.TextureId);
                        glDrawElements(GL_TRIANGLES, cmd.ElemCount, GL_UNSIGNED_INT, (void*)(cmd.IdxOffset * 4));
                    }

                    Marshal.FreeHGlobal(VtxBufferPtr);
                    Marshal.FreeHGlobal(IdxBufferPtr);

                    Drawing.NewFrame();

                    glUseProgram((uint)last_program);
                    glBindTexture(GL_TEXTURE_2D, (uint)last_texture);
                    glBindSampler(0, (uint)last_sampler);
                    glActiveTexture((uint)last_active_texture);
                    glBindVertexArray((uint)last_vertex_array_object);
                    glBindBuffer(GL_ARRAY_BUFFER, (uint)last_array_buffer);
                    glBlendEquationSeparate((uint)last_blend_equation_rgb, (uint)last_blend_equation_alpha);
                    glBlendFuncSeparate((uint)last_blend_src_rgb, (uint)last_blend_dst_rgb, (uint)last_blend_src_alpha, (uint)last_blend_dst_alpha);
                    if (last_enable_blend) glEnable(GL_BLEND); else glDisable(GL_BLEND);
                    if (last_enable_cull_face) glEnable(GL_CULL_FACE); else glDisable(GL_CULL_FACE);
                    if (last_enable_depth_test) glEnable(GL_DEPTH_TEST); else glDisable(GL_DEPTH_TEST);
                    if (last_enable_scissor_test) glEnable(GL_SCISSOR_TEST); else glDisable(GL_SCISSOR_TEST);
                    glPolygonMode(GL_FRONT_AND_BACK, (uint)last_polygon_mode[0]);
                    glViewport(last_viewport[0], last_viewport[1], last_viewport[2], last_viewport[3]);
                    //glScissor(last_scissor_box[0], last_scissor_box[1], last_scissor_box[2], last_scissor_box[3]);
                }
                catch (Exception ed)
                {
                    Log.Debug(ed);
                }

            }
            orig(self, a1);
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
