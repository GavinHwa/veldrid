﻿using System;

namespace Veldrid.Graphics
{
    /// <summary>
    /// A DeviceBuffer storing index information.
    /// </summary>
    public interface IndexBuffer: IDisposable
    {
        /// <summary>
        /// Stores the given index data into the device-side buffer.
        /// </summary>
        /// <param name="indices">The index data.</param>
        void SetIndices(int[] indices);
        /// <summary>
        /// Stores the given index data into the device-side buffer.
        /// </summary>
        /// <param name="indices">The index data.</param>
        /// <param name="stride">The stride of the data.</param>
        /// <param name="elementOffset">The number of elements to skip in the destination buffer.</param>
        void SetIndices(int[] indices, int stride, int elementOffset);
        /// <summary>
        /// Stores the given index data into the device-side buffer.
        /// </summary>
        /// <typeparam name="T">The type of index data. Must be a value type.</typeparam>
        /// <param name="indices">The index data.</param>
        /// <param name="format">The format of the index data.</param>
        void SetIndices<T>(T[] indices, IndexFormat format) where T : struct;
        /// <summary>
        /// Stores the given index data into the device-side buffer.
        /// </summary>
        /// <typeparam name="T">The type of index data. Must be a value type.</typeparam>
        /// <param name="indices">The index data.</param>
        /// <param name="format">The format of the index data.</param>
        /// <param name="stride">The stride of the data.</param>
        /// <param name="elementOffset">The number of elements to skip in the destination buffer.</param>
        void SetIndices<T>(T[] indices, IndexFormat format, int stride, int elementOffset) where T : struct;
        /// <summary>
        /// Stores the given index data into the device-side buffer.
        /// </summary>
        /// <param name="indices">The index data.</param>
        /// <param name="format">The format of the index data.</param>
        /// <param name="elementSizeInBytes">The size of individual elements, in bytes.</param>
        /// <param name="count">The number of elements to store in the buffer.</param>
        void SetIndices(IntPtr indices, IndexFormat format, int elementSizeInBytes, int count);
        /// <summary>
        /// Stores the given index data into the device-side buffer.
        /// </summary>
        /// <param name="indices">The index data.</param>
        /// <param name="format">The format of the index data.</param>
        /// <param name="elementSizeInBytes">The size of individual elements, in bytes.</param>
        /// <param name="count">The number of elements to store in the buffer.</param>
        /// <param name="elementOffset">The number of elements to skip in the destination buffer.</param>
        void SetIndices(IntPtr indices, IndexFormat format, int elementSizeInBytes, int count, int elementOffset);
    }
}