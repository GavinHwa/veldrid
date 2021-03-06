﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Veldrid.Graphics
{
    /// <summary>
    /// Contains functionality for creating GPU device resources,
    /// including buffers, textures, and other pipeline state objects.
    /// </summary>
    public abstract class ResourceFactory : DeviceTextureCreator
    {
        public string ShaderAssetRootPath { get; set; } = AppContext.BaseDirectory;

        /// <summary>
        /// Creates a <see cref="VertexBuffer"/> with the given storage size.
        /// </summary>
        /// <param name="sizeInBytes">The total capacity in bytes of the buffer.</param>
        /// <param name="isDynamic">A value indicating whether or not the buffer should be optimized for dynamic access.</param>
        /// <returns>A new <see cref="VertexBuffer"/>.</returns>
        public abstract VertexBuffer CreateVertexBuffer(int sizeInBytes, bool isDynamic);

        /// <summary>
        /// Creates a <see cref="VertexBuffer"/> containing the given vertex data.
        /// </summary>
        /// <typeparam name="T">The type of vertex data; must be a value type.</typeparam>
        /// <param name="data">An array of vertices to store in the <see cref="VertexBuffer"/></param>
        /// <param name="descriptor">A description of the vertex content.</param>
        /// <param name="isDynamic"></param>
        /// <returns>A new <see cref="VertexBuffer"/> containing the given data.</returns>
        public VertexBuffer CreateVertexBuffer<T>(T[] data, VertexDescriptor descriptor, bool isDynamic) where T : struct
        {
            VertexBuffer vb = CreateVertexBuffer(descriptor.VertexSizeInBytes * descriptor.ElementCount, isDynamic);
            vb.SetVertexData(data, descriptor);
            return vb;
        }

        /// <summary>
        /// Constructs a <see cref="IndexBuffer"/> with the given storage size.
        /// </summary>
        /// <param name="sizeInBytes">The total capacity in bytes of the buffer.</param>
        /// <param name="isDynamic">A value indicating whether or not the buffer should be optimized for dynamic access.</param>
        /// <returns>A new <see cref="IndexBuffer"/></returns>
        public abstract IndexBuffer CreateIndexBuffer(int sizeInBytes, bool isDynamic);
        /// <summary>
        /// Constructs a <see cref="IndexBuffer"/> with the given storage size.
        /// </summary>
        /// <param name="sizeInBytes">The total capacity in bytes of the buffer.</param>
        /// <param name="isDynamic">A value indicating whether or not the buffer should be optimized for dynamic access.</param>
        /// <param name="format">The format of index data that will be contained in the buffer.</param>
        /// <returns>A new <see cref="IndexBuffer"/></returns>
        public abstract IndexBuffer CreateIndexBuffer(int sizeInBytes, bool isDynamic, IndexFormat format);

        /// <summary>
        /// Creates an <see cref="IndexBuffer"/> containing the given <see cref="System.Int32"/> index data.
        /// </summary>
        /// <param name="indices">The index data.</param>
        /// <param name="isDynamic">A value indicating whether or not the buffer should be optimized for dynamic access.</param>
        /// <returns>A new <see cref="IndexBuffer"/></returns>
        public IndexBuffer CreateIndexBuffer(int[] indices, bool isDynamic)
        {
            IndexBuffer ib = CreateIndexBuffer(sizeof(int) * indices.Length, isDynamic);
            ib.SetIndices(indices);
            return ib;
        }

        /// <summary>
        /// Creates an <see cref="IndexBuffer"/> containing the given <see cref="System.Int32"/> index data.
        /// </summary>
        /// <param name="indices">The index data.</param>
        /// <param name="format">The format of the index data.</param>
        /// <param name="isDynamic">A value indicating whether or not the buffer should be optimized for dynamic access.</param>
        /// <returns>A new <see cref="IndexBuffer"/></returns>
        public IndexBuffer CreateIndexBuffer<T>(T[] indices, IndexFormat format, bool isDynamic) where T : struct
        {
            IndexBuffer ib = CreateIndexBuffer(sizeof(int) * indices.Length, isDynamic);
            ib.SetIndices(indices, format);
            return ib;
        }

        public Material CreateMaterial(
            RenderContext rc,
            string vertexShaderName,
            string fragmentShaderName,
            MaterialVertexInput vertexInputs,
            MaterialInputs<MaterialGlobalInputElement> globalInputs,
            MaterialInputs<MaterialPerObjectInputElement> perObjectInputs,
            MaterialTextureInputs textureInputs)
        {
            return CreateMaterial(rc, vertexShaderName, fragmentShaderName, new[] { vertexInputs }, globalInputs, perObjectInputs, textureInputs);
        }

        public Material CreateMaterial(
            RenderContext rc,
            string vertexShaderName,
            string fragmentShaderName,
            MaterialVertexInput vertexInputs0,
            MaterialVertexInput vertexInputs1,
            MaterialInputs<MaterialGlobalInputElement> globalInputs,
            MaterialInputs<MaterialPerObjectInputElement> perObjectInputs,
            MaterialTextureInputs textureInputs)
        {
            return CreateMaterial(rc, vertexShaderName, fragmentShaderName, new[] { vertexInputs0, vertexInputs1 }, globalInputs, perObjectInputs, textureInputs);
        }

        public Material CreateMaterial(
            RenderContext rc,
            string vertexShaderName,
            string fragmentShaderName,
            MaterialVertexInput[] vertexInputs,
            MaterialInputs<MaterialGlobalInputElement> globalInputs,
            MaterialInputs<MaterialPerObjectInputElement> perObjectInputs,
            MaterialTextureInputs textureInputs)
        {
            Shader vs = CreateShader(ShaderType.Vertex, vertexShaderName);
            Shader fs = CreateShader(ShaderType.Fragment, fragmentShaderName);
            VertexInputLayout inputLayout = CreateInputLayout(vs, vertexInputs);
            ShaderSet shaderSet = CreateShaderSet(inputLayout, vs, fs);
            ShaderConstantBindings constantBindings = CreateShaderConstantBindings(rc, shaderSet, globalInputs, perObjectInputs);
            ShaderTextureBindingSlots textureSlots = CreateShaderTextureBindingSlots(shaderSet, textureInputs);
            DefaultTextureBindingInfo[] defaultTextureInfos = CreateDefaultTextureBindingInfos(rc, textureInputs);

            return new Material(rc, shaderSet, constantBindings, textureSlots, defaultTextureInfos);
        }

        /// <summary>
        /// Creates a new <see cref="Shader"/> with the given name.
        /// </summary>
        /// <param name="type">The type of <see cref="Shader"/>.</param>
        /// <param name="name">The name of the shader. Must be discoverable by a registered <see cref="ShaderLoader"/>.</param>
        /// <returns>A new <see cref="Shader"/>.</returns>
        public abstract Shader CreateShader(ShaderType type, string name);

        /// <summary>
        /// Creates a new <see cref="Shader"/> from shader source code.
        /// </summary>
        /// <param name="type">The type of the <see cref="Shader"/></param>
        /// <param name="shaderCode">The raw text source of the <see cref="Shader"/></param>
        /// <param name="name">The name of the <see cref="Shader"/>, generally used for debugging.</param>
        /// <returns>A new Shader object.</returns>
        public abstract Shader CreateShader(ShaderType type, string shaderCode, string name);

        public abstract ShaderSet CreateShaderSet(VertexInputLayout inputLayout, Shader vertexShader, Shader fragmentShader);

        public abstract ShaderSet CreateShaderSet(VertexInputLayout inputLayout, Shader vertexShader, Shader geometryShader, Shader fragmentShader);

        public abstract ShaderConstantBindings CreateShaderConstantBindings(
            RenderContext rc,
            ShaderSet shaderSet,
            MaterialInputs<MaterialGlobalInputElement> globalInputs,
            MaterialInputs<MaterialPerObjectInputElement> perObjectInputs);

        /// <summary>
        /// Creates a new <see cref="ShaderTextureBindingSlots"/> for the given shader set and a device-agnostic description.
        /// </summary>
        /// <param name="shaderSet">The <see cref="ShaderSet"/> which the slots will be valid for.</param>
        /// <param name="textureInputs">The texture slot descriptions.</param>
        /// <returns>A new <see cref="ShaderTextureBindingSlots"/>.</returns>
        public abstract ShaderTextureBindingSlots CreateShaderTextureBindingSlots(ShaderSet shaderSet, MaterialTextureInputs textureInputs);

        /// <summary>
        /// Creates a device-specific <see cref="VertexInputLayout"/> from a generic description.
        /// </summary>
        /// <param name="vertexShader">The vertex <see cref="Shader"/>.</param>
        /// <param name="vertexInputs">An array of vertex input descriptions, one for each <see cref="VertexBuffer"/> input.</param>
        /// <returns>A new <see cref="VertexInputLayout"/>.</returns>
        public abstract VertexInputLayout CreateInputLayout(Shader vertexShader, params MaterialVertexInput[] vertexInputs);

        /// <summary>
        /// Creates a new <see cref="ConstantBuffer"/>, used for storing global <see cref="Shader"/> parameters.
        /// </summary>
        /// <param name="sizeInBytes">The initial capacity in bytes of the buffer.</param>
        /// <returns>A new <see cref="ConstantBuffer"/>.</returns>
        public abstract ConstantBuffer CreateConstantBuffer(int sizeInBytes);

        /// <summary>
        /// Creates a new <see cref="Framebuffer"/>, with no color and depth textures initially attached.
        /// </summary>
        /// <returns>A new, empty <see cref="Framebuffer"/>.</returns>
        public abstract Framebuffer CreateFramebuffer();

        /// <summary>
        /// Creates a new <see cref="Framebuffer"/> with the given dimensions.
        /// </summary>
        /// <param name="width">The width of the color and depth textures.</param>
        /// <param name="height">The height of the color and depth textures.</param>
        /// <returns></returns>
        public abstract Framebuffer CreateFramebuffer(int width, int height);

        /// <summary>
        /// Creates a new <see cref="DeviceTexture2D"/>.
        /// </summary>
        /// <typeparam name="T">The type of pixel data; must be a value type.</typeparam>
        /// <param name="pixelData">An array of pixel information.</param>
        /// <param name="width">The width of the texture</param>
        /// <param name="height">The height of the texture</param>
        /// <param name="pixelSizeInBytes">The total size in bytes of the pixel data.</param>
        /// <param name="format">The format of pixel information.</param>
        /// <returns>A new <see cref="DeviceTexture2D"/> containing the given pixel data.</returns>
        public abstract DeviceTexture2D CreateTexture<T>(T[] pixelData, int width, int height, int pixelSizeInBytes, PixelFormat format) where T : struct;

        /// <summary>
        /// Creates a new <see cref="DeviceTexture2D"/>.
        /// </summary>
        /// <param name="pixelData">An array of pixel information.</param>
        /// <param name="width">The width of the texture</param>
        /// <param name="height">The height of the texture</param>
        /// <param name="pixelSizeInBytes">The total size in bytes of the pixel data.</param>
        /// <param name="format">The format of pixel information.</param>
        /// <returns>A new <see cref="DeviceTexture2D"/> containing the given pixel data.</returns>
        public abstract DeviceTexture2D CreateTexture(IntPtr pixelData, int width, int height, int pixelSizeInBytes, PixelFormat format);

        /// <summary>
        /// Creates a new <see cref="ShaderTextureBinding"/> for the given <see cref="DeviceTexture"/>.
        /// </summary>
        /// <param name="texture">The <see cref="DeviceTexture"/> to associate with the binding.</param>
        /// <returns>A new <see cref="ShaderTextureBinding"/>.</returns>
        public abstract ShaderTextureBinding CreateShaderTextureBinding(DeviceTexture texture);

        /// <summary>
        /// Creates a new <see cref="DeviceTexture2D"/> which can be bound as a depth texture in a <see cref="Framebuffer"/>.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="pixelSizeInBytes">The total size in bytes of the depth data.</param>
        /// <param name="format">The format of the depth data.</param>
        /// <returns></returns>
        public abstract DeviceTexture2D CreateDepthTexture(int width, int height, int pixelSizeInBytes, PixelFormat format);

        /// <summary>
        /// Creates a new <see cref="CubemapTexture"/> from six pointers containing each cube face's texture data.
        /// </summary>
        /// <param name="pixelsFront">The front face pixel data.</param>
        /// <param name="pixelsBack">The back face pixel data.</param>
        /// <param name="pixelsLeft">The left face pixel data.</param>
        /// <param name="pixelsRight">The right face pixel data.</param>
        /// <param name="pixelsTop">The top face pixel data.</param>
        /// <param name="pixelsBottom">The bottom face pixel data.</param>
        /// <param name="width">The width of each cube face texture.</param>
        /// <param name="height">The height of each cube face texture.</param>
        /// <param name="pixelSizeinBytes">The total size in bytes of the pixel data for each individual cube face.</param>
        /// <param name="format">The format of the pixel data.</param>
        /// <returns>A new <see cref="CubemapTexture"/>.</returns>
        public abstract CubemapTexture CreateCubemapTexture(
            IntPtr pixelsFront,
            IntPtr pixelsBack,
            IntPtr pixelsLeft,
            IntPtr pixelsRight,
            IntPtr pixelsTop,
            IntPtr pixelsBottom,
            int width,
            int height,
            int pixelSizeinBytes,
            PixelFormat format);

        /// <summary>
        /// Creates a new <see cref="BlendState"/> used to control the device's output merger blending behavior.
        /// </summary>
        /// <param name="isBlendEnabled">A value indicating whether blending is enabled in the <see cref="BlendState"/>.</param>
        /// <param name="srcBlend">The source blend factor.</param>
        /// <param name="destBlend">The destination blend factor.</param>
        /// <param name="blendFunc">The blend function.</param>
        /// <returns>A new <see cref="BlendState"/>.</returns>
        public abstract BlendState CreateCustomBlendState(
            bool isBlendEnabled,
            Blend srcBlend, Blend destBlend, BlendFunction blendFunc);

        /// <summary>
        /// Creates a new <see cref="BlendState"/>, used to control blending behavior in the device's output merger.
        /// with separate factors for alpha and color blending.
        /// </summary>
        /// <param name="isBlendEnabled">A value indicating whether blending is enabled in the <see cref="BlendState"/>.</param>
        /// <param name="srcAlpha">The source alpha blend factor.</param>
        /// <param name="destAlpha">The destination alpha blend factor.</param>
        /// <param name="alphaBlendFunc">The alpha blend function.</param>
        /// <param name="srcColor">The source color blend factor.</param>
        /// <param name="destColor">The destenation color blend factor.</param>
        /// <param name="colorBlendFunc">The color blend function.</param>
        /// <returns>A new <see cref="BlendState"/>.</returns>
        public abstract BlendState CreateCustomBlendState(
            bool isBlendEnabled,
            Blend srcAlpha, Blend destAlpha, BlendFunction alphaBlendFunc,
            Blend srcColor, Blend destColor, BlendFunction colorBlendFunc);

        /// <summary>
        /// Creates a new <see cref="DepthStencilState"/>, used to control depth and stencil comparisons in the device's output merger.
        /// </summary>
        /// <param name="isDepthEnabled">A value indicating whether depth testing is enabled in the new state.</param>
        /// <param name="comparison">The kind of <see cref="DepthComparison"/> to use in the new state.</param>
        /// <returns>A new <see cref="DepthStencilState"/>.</returns>
        public DepthStencilState CreateDepthStencilState(bool isDepthEnabled, DepthComparison comparison)
        {
            return CreateDepthStencilState(isDepthEnabled, comparison, true);
        }

        /// <summary>
        /// Creates a new <see cref="DepthStencilState"/>, used to control depth and stencil comparisons in the device's output merger.
        /// </summary>
        /// <param name="isDepthEnabled">A value indicating whether depth testing is enabled in the new state.</param>
        /// <param name="comparison">The kind of <see cref="DepthComparison"/> to use in the new state.</param>
        /// <param name="isDepthWriteEnabled">A value indicating whether the depth buffer is written to when drawing.</param>
        /// <returns>A new <see cref="DepthStencilState"/>.</returns>
        public abstract DepthStencilState CreateDepthStencilState(bool isDepthEnabled, DepthComparison comparison, bool isDepthWriteEnabled);

        /// <summary>
        /// Creates a new <see cref="RasterizerState"/>, used to control various behaviors of the device's rasterizer.
        /// </summary>
        /// <param name="cullMode">Controls which primitive faces are culled.</param>
        /// <param name="fillMode">The kind of triangle filling to use.</param>
        /// <param name="isDepthClipEnabled">Whether or not primitives are clipped by depth.</param>
        /// <param name="isScissorTestEnabled">Whether or not primitives are clipped by the scissor test.</param>
        /// <returns>A new <see cref="RasterizerState"/>.</returns>
        public abstract RasterizerState CreateRasterizerState(
            FaceCullingMode cullMode,
            TriangleFillMode fillMode,
            bool isDepthClipEnabled,
            bool isScissorTestEnabled);

        /// <summary>
        /// Adds an additional <see cref="ShaderLoader"/> which provides additional <see cref="Shader"/> loading locations.
        /// </summary>
        /// <param name="loader">The <see cref="ShaderLoader"/> to add.</param>
        public abstract void AddShaderLoader(ShaderLoader loader);

        public DefaultTextureBindingInfo[] CreateDefaultTextureBindingInfos(RenderContext rc, MaterialTextureInputs textureInputs)
        {
            List<DefaultTextureBindingInfo> textures = new List<DefaultTextureBindingInfo>();
            for (int i = 0; i < textureInputs.Elements.Length; i++)
            {
                var element = textureInputs.Elements[i];
                var texture = element.GetDeviceTexture(rc);
                if (texture != null)
                {
                    textures.Add(new DefaultTextureBindingInfo(i, CreateShaderTextureBinding(texture)));
                }
            }

            return textures.ToArray();
        }
    }
}
