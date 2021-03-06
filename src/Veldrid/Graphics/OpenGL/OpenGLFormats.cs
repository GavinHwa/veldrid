﻿using System;
using OpenTK.Graphics.OpenGL;

namespace Veldrid.Graphics.OpenGL
{
    public static class OpenGLFormats
    {
        public static OpenTK.Graphics.OpenGL.PixelFormat MapPixelFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R32_G32_B32_A32_Float:
                    return OpenTK.Graphics.OpenGL.PixelFormat.Rgba;
                case PixelFormat.R8_UInt:
                    return OpenTK.Graphics.OpenGL.PixelFormat.RedInteger;
                case PixelFormat.Alpha_UInt8:
                    return OpenTK.Graphics.OpenGL.PixelFormat.Alpha;
                case PixelFormat.R8_G8_B8_A8:
                    return OpenTK.Graphics.OpenGL.PixelFormat.Rgba;
                case PixelFormat.Alpha_UInt16:
                    return OpenTK.Graphics.OpenGL.PixelFormat.Alpha;
                default:
                    throw Illegal.Value<PixelFormat>();
            }
        }

        public static OpenTK.Graphics.OpenGL.ShaderType VeldridToGLShaderType(ShaderType type)
        {
            switch (type)
            {
                case ShaderType.Vertex:
                    return OpenTK.Graphics.OpenGL.ShaderType.VertexShader;
                case ShaderType.Geometry:
                    return OpenTK.Graphics.OpenGL.ShaderType.GeometryShader;
                case ShaderType.Fragment:
                    return OpenTK.Graphics.OpenGL.ShaderType.FragmentShader;
                default:
                    throw Illegal.Value<ShaderType>();
            }
        }

        public static ShaderType GLToVeldridShaderType(OpenTK.Graphics.OpenGL.ShaderType type)
        {
            switch (type)
            {
                case OpenTK.Graphics.OpenGL.ShaderType.FragmentShader:
                    return ShaderType.Fragment;
                case OpenTK.Graphics.OpenGL.ShaderType.VertexShader:
                    return ShaderType.Vertex;
                case OpenTK.Graphics.OpenGL.ShaderType.GeometryShader:
                    return ShaderType.Geometry;
                case OpenTK.Graphics.OpenGL.ShaderType.TessEvaluationShader:
                case OpenTK.Graphics.OpenGL.ShaderType.TessControlShader:
                case OpenTK.Graphics.OpenGL.ShaderType.ComputeShader:
                    throw new NotSupportedException($"The shader type {type} is not supported.");
                default:
                    throw Illegal.Value<OpenTK.Graphics.OpenGL.ShaderType>();
            }
        }

        internal static DepthFunction ConvertDepthComparison(DepthComparison depthComparison)
        {
            switch (depthComparison)
            {
                case DepthComparison.Never:
                    return DepthFunction.Never;
                case DepthComparison.Less:
                    return DepthFunction.Less;
                case DepthComparison.Equal:
                    return DepthFunction.Equal;
                case DepthComparison.LessEqual:
                    return DepthFunction.Lequal;
                case DepthComparison.Greater:
                    return DepthFunction.Greater;
                case DepthComparison.NotEqual:
                    return DepthFunction.Notequal;
                case DepthComparison.GreaterEqual:
                    return DepthFunction.Gequal;
                case DepthComparison.Always:
                    return DepthFunction.Always;
                default:
                    throw Illegal.Value<DepthComparison>();
            }
        }

        internal static PolygonMode ConvertFillMode(TriangleFillMode fillMode)
        {
            switch (fillMode)
            {
                case TriangleFillMode.Solid:
                    return PolygonMode.Fill;
                case TriangleFillMode.Wireframe:
                    return PolygonMode.Line;
                default:
                    throw Illegal.Value<TriangleFillMode>();
            }
        }

        internal static CullFaceMode ConvertCullMode(FaceCullingMode cullMode)
        {
            switch (cullMode)
            {
                case FaceCullingMode.Back:
                    return CullFaceMode.Back;
                case FaceCullingMode.Front:
                    return CullFaceMode.Front;
                default:
                    // FaceCullingMode.None should not be converted.
                    // It translates to GL.Disable(EnableCap.CullFace).
                    throw Illegal.Value<FaceCullingMode>();
            }
        }

        public static PixelType MapPixelType(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R32_G32_B32_A32_Float:
                    return PixelType.Float;
                case PixelFormat.R8_UInt:
                    return PixelType.UnsignedByte;
                case PixelFormat.Alpha_UInt8:
                    return PixelType.UnsignedByte;
                case PixelFormat.R8_G8_B8_A8:
                    return PixelType.UnsignedByte;
                case PixelFormat.Alpha_UInt16:
                    return PixelType.UnsignedShort;
                default:
                    throw Illegal.Value<PixelFormat>();
            }
        }

        internal static PixelInternalFormat MapPixelInternalFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R32_G32_B32_A32_Float:
                    return PixelInternalFormat.Rgba32f;
                case PixelFormat.R8_UInt:
                    return PixelInternalFormat.R8ui;
                case PixelFormat.Alpha_UInt8:
                    return PixelInternalFormat.Alpha;
                case PixelFormat.R8_G8_B8_A8:
                    return PixelInternalFormat.Rgba;
                case PixelFormat.Alpha_UInt16:
                    return PixelInternalFormat.Alpha16;
                default:
                    throw Illegal.Value<PixelFormat>();
            }
        }

        internal static BlendingFactorSrc ConvertBlendSrc(Blend blend)
        {
            switch (blend)
            {
                case Blend.Zero:
                    return BlendingFactorSrc.Zero;
                case Blend.One:
                    return BlendingFactorSrc.One;
                case Blend.SourceAlpha:
                    return BlendingFactorSrc.SrcAlpha;
                case Blend.InverseSourceAlpha:
                    return BlendingFactorSrc.OneMinusSrcAlpha;
                case Blend.DestinationAlpha:
                    return BlendingFactorSrc.DstAlpha;
                case Blend.InverseDestinationAlpha:
                    return BlendingFactorSrc.OneMinusDstAlpha;
                case Blend.SourceColor:
                    return BlendingFactorSrc.SrcColor;
                case Blend.InverseSourceColor:
                    return BlendingFactorSrc.OneMinusSrcColor;
                case Blend.DestinationColor:
                    return BlendingFactorSrc.DstColor;
                case Blend.InverseDestinationColor:
                    return BlendingFactorSrc.OneMinusDstColor;
                case Blend.BlendFactor:
                    return BlendingFactorSrc.ConstantColor;
                case Blend.InverseBlendFactor:
                    return BlendingFactorSrc.OneMinusConstantColor;
                default:
                    throw Illegal.Value<Blend>();
            }
        }

        internal static PrimitiveType ConvertPrimitiveTopology(PrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.TriangleList:
                    return PrimitiveType.Triangles;
                case PrimitiveTopology.TriangleStrip:
                    return PrimitiveType.TriangleStrip;
                case PrimitiveTopology.LineList:
                    return PrimitiveType.Lines;
                case PrimitiveTopology.LineStrip:
                    return PrimitiveType.LineStrip;
                case PrimitiveTopology.PointList:
                    return PrimitiveType.Points;
                default:
                    throw Illegal.Value<PrimitiveTopology>();
            }
        }

        internal static BlendingFactorDest ConvertBlendDest(Blend blend) => (BlendingFactorDest)ConvertBlendSrc(blend);

        public static BlendEquationMode ConvertBlendEquation(BlendFunction function)
        {
            switch (function)
            {
                case BlendFunction.Add:
                    return BlendEquationMode.FuncAdd;
                case BlendFunction.Subtract:
                    return BlendEquationMode.FuncSubtract;
                case BlendFunction.ReverseSubtract:
                    return BlendEquationMode.FuncReverseSubtract;
                case BlendFunction.Minimum:
                    return BlendEquationMode.Min;
                case BlendFunction.Maximum:
                    return BlendEquationMode.Max;
                default:
                    throw Illegal.Value<BlendFunction>();
            }
        }

        internal static DrawElementsType MapIndexFormat(IndexFormat format)
        {
            switch (format)
            {
                case IndexFormat.UInt32:
                    return DrawElementsType.UnsignedInt;
                case IndexFormat.UInt16:
                    return DrawElementsType.UnsignedShort;
                case IndexFormat.UInt8:
                    return DrawElementsType.UnsignedByte;
                default:
                    throw Illegal.Value<DrawElementsType>();
            }
        }

        public static int GetIndexFormatSize(DrawElementsType type)
        {
            switch (type)
            {
                case DrawElementsType.UnsignedByte:
                    return 1;
                case DrawElementsType.UnsignedShort:
                    return 2;
                case DrawElementsType.UnsignedInt:
                    return 4;
                default:
                    throw Illegal.Value<DrawElementsType>();
            }
        }
    }
}
