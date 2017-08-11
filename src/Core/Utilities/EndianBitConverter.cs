// <copyright file="EndianBitConverter.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2017 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
//
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

namespace AEGIS.Utilities
{
    using System;
    using AEGIS.Collections.Resources;
    using AEGIS.Numerics;
    using Resources;

    /// <summary>
    /// Converts base data types to an array of bytes, and an array of bytes to base data types with respect to byte-order.
    /// </summary>
    public static class EndianBitConverter
    {
        /// <summary>
        /// Gets the system default byte order.
        /// </summary>
        /// <value>The system default byte order.</value>
        public static ByteOrder DefaultByteOrder { get { return BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian; } }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit signed integer formed by two bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int16 ToInt16(Byte[] array)
        {
            return ToInt16(array, 0);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two byte at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int16 ToInt16(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int16) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return BitConverter.ToInt16(array, startIndex);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 16-bit signed integer formed by two bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int16 ToInt16(Byte[] array, ByteOrder order)
        {
            return ToInt16(array, 0, order);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int16 ToInt16(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int16) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[sizeof(Int16)];
                Array.Copy(array, startIndex, valueArray, 0, sizeof(Int16));
                Array.Reverse(valueArray, 0, sizeof(Int16));

                return BitConverter.ToInt16(valueArray, 0);
            }

            return BitConverter.ToInt16(array, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit signed integer formed by four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int32 ToInt32(Byte[] array)
        {
            return ToInt32(array, 0);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int32 ToInt32(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return BitConverter.ToInt32(array, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 32-bit signed integer formed by four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int32 ToInt32(Byte[] array, ByteOrder order)
        {
            return ToInt32(array, 0, order);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int32 ToInt32(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[sizeof(Int32)];
                Array.Copy(array, startIndex, valueArray, 0, sizeof(Int32));
                Array.Reverse(valueArray, 0, sizeof(Int32));

                return BitConverter.ToInt32(valueArray, 0);
            }

            return BitConverter.ToInt32(array, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int64 ToInt64(Byte[] array)
        {
            return ToInt64(array, 0);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int64 ToInt64(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int64) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return BitConverter.ToInt64(array, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int64 ToInt64(Byte[] array, ByteOrder order)
        {
            return ToInt64(array, 0, order);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int64 ToInt64(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int64) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[sizeof(Int64)];
                Array.Copy(array, startIndex, valueArray, 0, sizeof(Int64));
                Array.Reverse(valueArray, 0, sizeof(Int64));

                return BitConverter.ToInt64(valueArray, 0);
            }

            return BitConverter.ToInt64(array, startIndex);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt16 ToUInt16(Byte[] array)
        {
            return ToUInt16(array, 0);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt16 ToUInt16(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt16) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return BitConverter.ToUInt16(array, startIndex);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt16 ToUInt16(Byte[] array, ByteOrder order)
        {
            return ToUInt16(array, 0, order);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt16 ToUInt16(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt16) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[sizeof(UInt16)];
                Array.Copy(array, startIndex, valueArray, 0, sizeof(UInt16));
                Array.Reverse(valueArray, 0, sizeof(UInt16));

                return BitConverter.ToUInt16(valueArray, 0);
            }

            return BitConverter.ToUInt16(array, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt32 ToUInt32(Byte[] array)
        {
            return ToUInt32(array, 0);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt32 ToUInt32(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return BitConverter.ToUInt32(array, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt32 ToUInt32(Byte[] array, ByteOrder order)
        {
            return ToUInt32(array, 0, order);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt32 ToUInt32(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[sizeof(UInt32)];
                Array.Copy(array, startIndex, valueArray, 0, sizeof(UInt32));
                Array.Reverse(valueArray, 0, sizeof(UInt32));

                return BitConverter.ToUInt32(valueArray, 0);
            }

            return BitConverter.ToUInt32(array, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt64 ToUInt64(Byte[] array)
        {
            return ToUInt64(array, 0);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt64 ToUInt64(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt64) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return BitConverter.ToUInt64(array, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt64 ToUInt64(Byte[] array, ByteOrder order)
        {
            return ToUInt64(array, 0, order);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt64 ToUInt64(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt64) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[sizeof(UInt64)];
                Array.Copy(array, startIndex, valueArray, 0, sizeof(UInt64));
                Array.Reverse(valueArray, 0, sizeof(UInt64));

                return BitConverter.ToUInt64(valueArray, 0);
            }

            return BitConverter.ToUInt64(array, startIndex);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Single ToSingle(Byte[] array)
        {
            return ToSingle(array, 0);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A single-precision floating point number converted from four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Single ToSingle(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Single) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return BitConverter.ToSingle(array, startIndex);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Single ToSingle(Byte[] array, ByteOrder order)
        {
            return ToSingle(array, 0, order);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Single ToSingle(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Single) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[sizeof(Single)];
                Array.Copy(array, startIndex, valueArray, 0, sizeof(Single));
                Array.Reverse(valueArray, 0, sizeof(Single));

                return BitConverter.ToSingle(valueArray, 0);
            }

            return BitConverter.ToSingle(array, startIndex);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Double ToDouble(Byte[] array)
        {
            return ToDouble(array, 0);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A double-precision floating point number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Double ToDouble(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return BitConverter.ToDouble(array, startIndex);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A double-precision floating point number converted from eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Double ToDouble(Byte[] array, ByteOrder order)
        {
            return ToDouble(array, 0, order);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A double-precision floating point number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Double ToDouble(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[sizeof(Double)];
                Array.Copy(array, startIndex, valueArray, 0, sizeof(Double));
                Array.Reverse(valueArray, 0, sizeof(Double));

                return BitConverter.ToDouble(valueArray, 0);
            }

            return BitConverter.ToDouble(array, startIndex);
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A rational number converted from eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Rational ToRational(Byte[] array)
        {
            return ToRational(array, 0);
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A rational number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Rational ToRational(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            return new Rational(BitConverter.ToInt32(array, startIndex), BitConverter.ToInt32(array, startIndex + sizeof(Int32)));
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A rational number converted from eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Rational ToRational(Byte[] array, ByteOrder order)
        {
            return ToRational(array, 0, order);
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A rational number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Rational ToRational(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + 2 * sizeof(Int32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                Byte[] valueArray = new Byte[2 * sizeof(Int32)];
                Array.Copy(array, startIndex, valueArray, 0, 2 * sizeof(Int32));
                Array.Reverse(valueArray, 0, 2 * sizeof(Int32));

                return new Rational(BitConverter.ToInt32(valueArray, 0), BitConverter.ToInt32(valueArray, sizeof(Int32)));
            }

            return new Rational(BitConverter.ToInt32(array, startIndex), BitConverter.ToInt32(array, startIndex + sizeof(Int32)));
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The spatial dimension of the coordinate must be 2 or 3.
        /// or
        /// The number of bytes in the array is less than required.
        /// </exception>
        public static Coordinate ToCoordinate(Byte[] array, Int32 spatialDimension)
        {
            return ToCoordinate(array, 0, spatialDimension);
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="dimension">The spatial dimension of the coordinate.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// or
        /// The dimension is less than 2.
        /// or
        /// The dimension is greater than 3.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Coordinate ToCoordinate(Byte[] array, Int32 startIndex, Int32 dimension)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (dimension < 2)
                throw new ArgumentOutOfRangeException(nameof(dimension), AEGIS.Resources.CoreMessages.DimensionIsLessThan2);
            if (dimension > 3)
                throw new ArgumentOutOfRangeException(nameof(dimension), AEGIS.Resources.CoreMessages.DimensionIsGreaterThan3);
            if (startIndex + dimension * sizeof(Double) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (dimension == 3)
            {
                return new Coordinate(BitConverter.ToDouble(array, startIndex), BitConverter.ToDouble(array, startIndex + sizeof(Double)), BitConverter.ToDouble(array, startIndex + 2 * sizeof(Double)));
            }
            else
            {
                return new Coordinate(BitConverter.ToDouble(array, startIndex), BitConverter.ToDouble(array, startIndex + sizeof(Double)));
            }
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The spatial dimension of the coordinate must be 2 or 3.
        /// or
        /// The number of bytes in the array is less than required.
        /// </exception>
        public static Coordinate ToCoordinate(Byte[] array, Int32 spatialDimension, ByteOrder order)
        {
            return ToCoordinate(array, 0, spatialDimension, order);
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="dimension">The spatial dimension of the coordinate.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// or
        ///
        /// The dimension is less than 2.
        /// or
        /// The dimension is greater than 3.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Coordinate ToCoordinate(Byte[] array, Int32 startIndex, Int32 dimension, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (dimension < 2)
                throw new ArgumentOutOfRangeException(nameof(dimension), AEGIS.Resources.CoreMessages.DimensionIsLessThan2);
            if (dimension > 3)
                throw new ArgumentOutOfRangeException(nameof(dimension), AEGIS.Resources.CoreMessages.DimensionIsGreaterThan3);
            if (startIndex + dimension * sizeof(Double) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            if (order != DefaultByteOrder)
            {
                // case of reverse order
                if (dimension == 3)
                {
                    // the values are in reverse order, but the coordinate order is forward (X, Y, Z)
                    Byte[] valueArray = new Byte[3 * sizeof(Double)];
                    Array.Copy(array, startIndex, valueArray, 0, 3 * sizeof(Double));
                    Array.Reverse(valueArray, 0, 3 * sizeof(Double));

                    return new Coordinate(BitConverter.ToDouble(valueArray, 2 * sizeof(Double)), BitConverter.ToDouble(valueArray, sizeof(Double)), BitConverter.ToDouble(valueArray, 0));
                }
                else
                {
                    Byte[] valueArray = new Byte[2 * sizeof(Double)];
                    Array.Copy(array, startIndex, valueArray, 0, 2 * sizeof(Double));
                    Array.Reverse(valueArray, 0, 2 * sizeof(Double));

                    return new Coordinate(BitConverter.ToDouble(valueArray, sizeof(Double)), BitConverter.ToDouble(valueArray, 0));
                }
            }
            else
            {
                // case of forward order
                if (dimension == 3)
                {
                    return new Coordinate(BitConverter.ToDouble(array, startIndex), BitConverter.ToDouble(array, startIndex + sizeof(Double)), BitConverter.ToDouble(array, startIndex + 2 * sizeof(Double)));
                }
                else
                {
                    return new Coordinate(BitConverter.ToDouble(array, startIndex), BitConverter.ToDouble(array, startIndex + sizeof(Double)));
                }
            }
        }

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 2.</returns>
        public static Byte[] GetBytes(Int16 value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 2.</returns>
        public static Byte[] GetBytes(Int16 value, ByteOrder order)
        {
            if (order != DefaultByteOrder)
            {
                Byte[] array = BitConverter.GetBytes(value);
                Array.Reverse(array, 0, array.Length);
                return array;
            }
            else
            {
                return BitConverter.GetBytes(value);
            }
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(Int32 value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(Int32 value, ByteOrder order)
        {
            if (order != DefaultByteOrder)
            {
                Byte[] array = BitConverter.GetBytes(value);
                Array.Reverse(array, 0, array.Length);
                return array;
            }
            else
            {
                return BitConverter.GetBytes(value);
            }
        }

        /// <summary>
        /// Returns the specified 64-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 8.</returns>
        public static Byte[] GetBytes(Int64 value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns the specified 64-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 8.</returns>
        public static Byte[] GetBytes(Int64 value, ByteOrder order)
        {
            if (order != DefaultByteOrder)
            {
                Byte[] array = BitConverter.GetBytes(value);
                Array.Reverse(array, 0, array.Length);
                return array;
            }
            else
            {
                return BitConverter.GetBytes(value);
            }
        }

        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 2.</returns>
        public static Byte[] GetBytes(UInt16 value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 2.</returns>
        public static Byte[] GetBytes(UInt16 value, ByteOrder order)
        {
            if (order != DefaultByteOrder)
            {
                Byte[] array = BitConverter.GetBytes(value);
                Array.Reverse(array, 0, array.Length);
                return array;
            }
            else
            {
                return BitConverter.GetBytes(value);
            }
        }

        /// <summary>
        /// Returns the specified 32-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(UInt32 value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns the specified 32-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(UInt32 value, ByteOrder order)
        {
            if (order != DefaultByteOrder)
            {
                Byte[] array = BitConverter.GetBytes(value);
                Array.Reverse(array, 0, array.Length);
                return array;
            }
            else
            {
                return BitConverter.GetBytes(value);
            }
        }

        /// <summary>
        /// Returns the specified 64-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 8.</returns>
        public static Byte[] GetBytes(UInt64 value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns the specified 64-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 8.</returns>
        public static Byte[] GetBytes(UInt64 value, ByteOrder order)
        {
            if (order != DefaultByteOrder)
            {
                Byte[] array = BitConverter.GetBytes(value);
                Array.Reverse(array, 0, array.Length);
                return array;
            }
            else
            {
                return BitConverter.GetBytes(value);
            }
        }

        /// <summary>
        /// Returns the specified single-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(Single value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns the specified single-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(Single value, ByteOrder order)
        {
            if (order != DefaultByteOrder)
            {
                Byte[] array = BitConverter.GetBytes(value);
                Array.Reverse(array, 0, array.Length);
                return array;
            }
            else
            {
                return BitConverter.GetBytes(value);
            }
        }

        /// <summary>
        /// Returns the specified double-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(Double value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Returns the specified double-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(Double value, ByteOrder order)
        {
            if (order != DefaultByteOrder)
            {
                Byte[] array = BitConverter.GetBytes(value);
                Array.Reverse(array, 0, array.Length);
                return array;
            }
            else
            {
                return BitConverter.GetBytes(value);
            }
        }

        /// <summary>
        /// Returns the specified rational value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(Rational value)
        {
            Byte[] rationalBytes = new Byte[8];

            Byte[] array = BitConverter.GetBytes(value.Numerator);
            array.CopyTo(rationalBytes, 0);
            array = BitConverter.GetBytes(value.Denominator);
            array.CopyTo(rationalBytes, sizeof(Int32));

            return rationalBytes;
        }

        /// <summary>
        /// Returns the specified rational value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(Rational value, ByteOrder order)
        {
            Byte[] rationalBytes = new Byte[8];

            Byte[] array = BitConverter.GetBytes(value.Numerator);
            array.CopyTo(rationalBytes, 0);
            array = BitConverter.GetBytes(value.Denominator);
            array.CopyTo(rationalBytes, sizeof(Int32));

            if (order != DefaultByteOrder)
            {
                Array.Reverse(rationalBytes, 0, rationalBytes.Length);
            }

            return rationalBytes;
        }

        /// <summary>
        /// Returns the specified coordinate as an array of bytes.
        /// </summary>
        /// <param name="value">The coordinate.</param>
        /// <param name="dimension">The spatial dimension of the conversion.</param>
        /// <returns>The array of bytes in the default byte-order with length 16 or 24.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The dimension is less than 2.
        /// or
        /// The dimension is greater than 3.
        /// </exception>
        public static Byte[] GetBytes(Coordinate value, Int32 dimension)
        {
            if (dimension < 2)
                throw new ArgumentOutOfRangeException(nameof(dimension), AEGIS.Resources.CoreMessages.DimensionIsLessThan2);
            if (dimension > 3)
                throw new ArgumentOutOfRangeException(nameof(dimension), AEGIS.Resources.CoreMessages.DimensionIsGreaterThan3);

            Byte[] coordinateBytes = null;
            if (dimension == 3)
            {
                coordinateBytes = new Byte[24];

                Byte[] array = BitConverter.GetBytes(value.X);
                array.CopyTo(coordinateBytes, 0);

                array = BitConverter.GetBytes(value.Y);
                array.CopyTo(coordinateBytes, 8);

                array = BitConverter.GetBytes(value.Z);
                array.CopyTo(coordinateBytes, 16);

                return coordinateBytes;
            }
            else
            {
                coordinateBytes = new Byte[16];

                Byte[] array = BitConverter.GetBytes(value.X);
                array.CopyTo(coordinateBytes, 0);

                array = BitConverter.GetBytes(value.Y);
                array.CopyTo(coordinateBytes, 8);
            }

            return coordinateBytes;
        }

        /// <summary>
        /// Returns the specified coordinate as an array of bytes.
        /// </summary>
        /// <param name="value">The coordinate.</param>
        /// <param name="dimension">The spatial dimension of the conversion.</param>
        /// <param name="order">The byte order of the resulting array.</param>
        /// <returns>The array of bytes in the specified byte-order with length 16 or 24.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The dimension is less than 2.
        /// or
        /// The dimension is greater than 3.
        /// </exception>
        public static Byte[] GetBytes(Coordinate value, Int32 dimension, ByteOrder order)
        {
            if (dimension < 2)
                throw new ArgumentOutOfRangeException(nameof(dimension), AEGIS.Resources.CoreMessages.DimensionIsLessThan2);
            if (dimension > 3)
                throw new ArgumentOutOfRangeException(nameof(dimension), AEGIS.Resources.CoreMessages.DimensionIsGreaterThan3);

            Byte[] coordinateBytes = null;
            if (order != DefaultByteOrder)
            {
                // case of reverse order
                // the values should be in reverse order, but the coordinate order is forward (X, Y, Z)
                if (dimension == 3)
                {
                    coordinateBytes = new Byte[24];

                    Byte[] array = BitConverter.GetBytes(value.Z);
                    Array.Reverse(array, 0, array.Length);
                    array.CopyTo(coordinateBytes, 0);

                    array = BitConverter.GetBytes(value.Y);
                    Array.Reverse(array, 0, array.Length);
                    array.CopyTo(coordinateBytes, 8);

                    array = BitConverter.GetBytes(value.X);
                    Array.Reverse(array, 0, array.Length);
                    array.CopyTo(coordinateBytes, 16);
                }
                else
                {
                    coordinateBytes = new Byte[16];

                    Byte[] array = BitConverter.GetBytes(value.Y);
                    Array.Reverse(array, 0, array.Length);
                    array.CopyTo(coordinateBytes, 0);

                    array = BitConverter.GetBytes(value.X);
                    Array.Reverse(array, 0, array.Length);
                    array.CopyTo(coordinateBytes, 8);
                }
            }
            else
            {
                // case of forward order
                if (dimension == 3)
                {
                    coordinateBytes = new Byte[24];

                    Byte[] array = BitConverter.GetBytes(value.X);
                    array.CopyTo(coordinateBytes, 0);

                    array = BitConverter.GetBytes(value.Y);
                    array.CopyTo(coordinateBytes, 8);

                    array = BitConverter.GetBytes(value.Z);
                    array.CopyTo(coordinateBytes, 16);

                    return coordinateBytes;
                }
                else
                {
                    coordinateBytes = new Byte[16];

                    Byte[] array = BitConverter.GetBytes(value.X);
                    array.CopyTo(coordinateBytes, 0);

                    array = BitConverter.GetBytes(value.Y);
                    array.CopyTo(coordinateBytes, 8);
                }
            }

            return coordinateBytes;
        }

        /// <summary>
        /// Copies the specified 16-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int16 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int16) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 16-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int16 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int16) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 32-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int32 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 32-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int32 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 64-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int64 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int64) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 64-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int64 value, Byte[] array, Int32 startIndex, ByteOrder order = ByteOrder.LittleEndian)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Int64) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 16-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt16 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt16) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 16-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt16 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt16) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 32-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt32 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 32-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt32 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 64-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt64 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt64) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified 64-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt64 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(UInt64) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified single-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Single value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Single) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified single-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Single value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Single) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified double-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Double value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified double-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Double value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified rational value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Rational value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + 2 * sizeof(Int32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value.Numerator);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);

            valueArray = BitConverter.GetBytes(value.Denominator);

            Array.Copy(valueArray, 0, array, startIndex + sizeof(Int32), valueArray.Length);
        }

        /// <summary>
        /// Copies the specified rational value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Rational value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + 2 * sizeof(Int32) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value.Numerator);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);

            valueArray = BitConverter.GetBytes(value.Denominator);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex + sizeof(Int32), valueArray.Length);
        }

        /// <summary>
        /// Copies the specified Unicode character value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Char value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Char) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified Unicode character value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Char value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + sizeof(Char) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            Byte[] valueArray = BitConverter.GetBytes(value);

            if (order != DefaultByteOrder)
                Array.Reverse(valueArray, 0, valueArray.Length);

            Array.Copy(valueArray, 0, array, startIndex, valueArray.Length);
        }

        /// <summary>
        /// Copies the specified Unicode string value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(String value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + value.Length * sizeof(Char) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            for (Int32 i = 0; i < value.Length; i++)
            {
                Byte[] valueArray = BitConverter.GetBytes(value[i]);
                Array.Copy(valueArray, 0, array, startIndex + i, valueArray.Length);
            }
        }

        /// <summary>
        /// Copies the specified Unicode string value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(String value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsLessThan0);
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.StartingIndexIsEqualToOrGraterThanArrayLength);
            if (startIndex + value.Length * sizeof(Char) > array.Length)
                throw new ArgumentException(CollectionMessages.ArraySizeLessThanRequired, nameof(array));

            for (Int32 i = 0; i < value.Length; i++)
            {
                Byte[] valueArray = BitConverter.GetBytes(value[i]);
                Array.Copy(valueArray, 0, array, startIndex + i, valueArray.Length);
            }

            if (order != DefaultByteOrder)
                Array.Reverse(array, startIndex, value.Length * sizeof(Char));
        }
    }
}
