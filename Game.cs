using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace TrianguloTK
{
    public class Game : GameWindow
    {
        private int vertexBufferHandle;
        private int shaderProgramHandle;
        private int vertexArrayHandle;
        public Game()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(800, 500));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            GL.ClearColor(new Color4(0.1f, 1f, 1f, 1f));


            float[] vertices = new float[]
            {
                0.0f, 0.7f, 0f, //vertice sup
                0.7f, -0.7f, 0f, //vertice lat der
                -0.7f, -0.7f, 0f, // vertize lat iz
            };

            this.vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // talvez no tan necesario

            this.vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayHandle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);


            string vertexShaderCode =
                @"
                #version 330 core

                layout (location = 0) in vec3 aPosition;
                
                void main()
                {
                    gl_Position = vec4(aPosition, 1f);
                }
                ";

            string pixelShaderCode =
            @"
            #version 330 core
            
            out vec4 pixelColor;

            void main()
            {
                pixelColor = vec4(0.8f, 0.8f, 0.1f, 1f); 
            }
            ";

            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
            GL.CompileShader(vertexShaderHandle);

            int pixelShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(pixelShaderHandle, pixelShaderCode);
            GL.CompileShader(pixelShaderHandle);

            this.shaderProgramHandle = GL.CreateProgram();

            GL.AttachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(this.shaderProgramHandle, pixelShaderHandle);

            GL.LinkProgram(this.shaderProgramHandle);

            GL.DetachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(this.shaderProgramHandle, pixelShaderHandle);

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(pixelShaderHandle);

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //talvez no tan necesario
            GL.DeleteBuffer(this.vertexBufferHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(this.shaderProgramHandle);
            base.OnUnload();
        }
        
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(this.shaderProgramHandle);

            GL.BindVertexArray(this.vertexArrayHandle);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3); //llamada de la grafica
           
            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }


        
    }
}
