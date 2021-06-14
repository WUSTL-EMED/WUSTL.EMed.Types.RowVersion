// <copyright file="RowVersion.cs" company="Washington University in St. Louis">
// Copyright (c) 2021 Washington University in St. Louis. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>

namespace WUSTL.EMed.Types
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// A struct respresenting a SQL Server RowVersion value.
    /// </summary>
    [DebuggerDisplay("0x{ToString(),nq}")]
    /* [JsonConverter(typeof(RowVersionConverter))] */
    /* [System.Text.Json.Serialization.JsonConverter(typeof(RowVersionConverter))] */
    public struct RowVersion : IComparable, IEquatable<RowVersion>, IComparable<RowVersion> // Based on https://gist.github.com/jnm2/929d194c87df8ad0438f6cab0139a0a6.
    {
        /// <summary>
        /// A zero value <see cref="RowVersion"/>.
        /// </summary>
        public static readonly RowVersion Zero = new RowVersion(0);

        /*
        /// <summary>
        /// An EntityFramework <see cref="ValueConverter{TModel, TProvider}"/> for <see cref="RowVersion"/>.
        /// </summary>
        public static readonly ValueConverter<RowVersion, byte[]> ValueConverter = new ValueConverter<RowVersion, byte[]>(v => v, v => (RowVersion)v);
        */

        /// <summary>
        /// The SQL Server RowVersion value.
        /// </summary>
        private readonly ulong value;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowVersion"/> struct.
        /// </summary>
        /// <param name="value">The row version value as a <see cref="ulong"/>.</param>
        public RowVersion(ulong value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowVersion"/> struct.
        /// </summary>
        /// <param name="value">The row version value as an 8 byte array.</param>
        public RowVersion(byte[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length != 8)
            {
                throw new ArgumentException("Byte array isn't 8 bytes.", nameof(value));
            }

            var v = 0UL;
            v |= (ulong)value[0] << 56;
            v |= (ulong)value[1] << 48;
            v |= (ulong)value[2] << 40;
            v |= (ulong)value[3] << 32;
            v |= (ulong)value[4] << 24;
            v |= (ulong)value[5] << 16;
            v |= (ulong)value[6] << 8;
            v |= (ulong)value[7] << 0;
            this.value = v;
        }

        /// <summary>
        /// Converts a <see cref="byte"/>[] into a <see cref="RowVersion"/>.
        /// </summary>
        /// <param name="value">A SQL Server RowVersion value as a <see cref="byte"/>[].</param>
        public static explicit operator RowVersion(byte[] value) => new RowVersion(value);

        /// <summary>
        /// Converts a <see cref="byte"/>[] into a <see cref="RowVersion"/>.
        /// </summary>
        /// <param name="value">A SQL Server RowVersion value as a <see cref="byte"/>[].</param>
        public static explicit operator RowVersion?(byte[] value) => value is null ? (RowVersion?)null : new RowVersion(value);

        /// <summary>
        /// Converts a <see cref="RowVersion"/> into a <see cref="byte"/>[].
        /// </summary>
        /// <param name="rowVersion">A <see cref="RowVersion"/> to convert.</param>
        public static implicit operator byte[](RowVersion rowVersion) => rowVersion.ToByteArray();

        /// <summary>
        /// Converts a <see cref="long"/> into a <see cref="RowVersion"/>.
        /// </summary>
        /// <param name="value">A SQL Server RowVersion value as a <see cref="long"/>.</param>
        public static implicit operator RowVersion(long value) => FromInt64(value);

        /// <summary>
        /// Converts a <see cref="ulong"/> into a <see cref="RowVersion"/>.
        /// </summary>
        /// <param name="value">A SQL Server RowVersion value as a <see cref="ulong"/>.</param>
        public static implicit operator RowVersion(ulong value) => new RowVersion(value);

        /// <summary>
        /// Converts a <see cref="RowVersion"/> into a <see cref="ulong"/>.
        /// </summary>
        /// <param name="rowVersion">A <see cref="RowVersion"/> to convert.</param>
        public static implicit operator ulong(RowVersion rowVersion) => rowVersion.ToUInt64();

        /// <summary>
        /// Checks if two <see cref="RowVersion"/> values are not equal.
        /// </summary>
        /// <param name="comparand1">The left <see cref="RowVersion"/> value.</param>
        /// <param name="comparand2">The right <see cref="RowVersion"/> value.</param>
        /// <returns>True if the left <see cref="RowVersion"/> value is not equal to the right <see cref="RowVersion"/> value.</returns>
        public static bool operator !=(RowVersion comparand1, RowVersion comparand2) => !comparand1.Equals(comparand2);

        /// <summary>
        /// Checks if one <see cref="RowVersion"/> value is less than another.
        /// </summary>
        /// <param name="comparand1">The left <see cref="RowVersion"/> value.</param>
        /// <param name="comparand2">The right <see cref="RowVersion"/> value.</param>
        /// <returns>True if the left <see cref="RowVersion"/> value is less than the right <see cref="RowVersion"/> value.</returns>
        public static bool operator <(RowVersion comparand1, RowVersion comparand2) => comparand1.CompareTo(comparand2) < 0;

        /// <summary>
        /// Checks if one <see cref="RowVersion"/> value is less than or equal to another.
        /// </summary>
        /// <param name="comparand1">The left <see cref="RowVersion"/> value.</param>
        /// <param name="comparand2">The right <see cref="RowVersion"/> value.</param>
        /// <returns>True if the left <see cref="RowVersion"/> value is less than or equal to the right <see cref="RowVersion"/> value.</returns>
        public static bool operator <=(RowVersion comparand1, RowVersion comparand2) => comparand1.CompareTo(comparand2) <= 0;

        /// <summary>
        /// Checks if two <see cref="RowVersion"/> values are equal.
        /// </summary>
        /// <param name="comparand1">The left <see cref="RowVersion"/> value.</param>
        /// <param name="comparand2">The right <see cref="RowVersion"/> value.</param>
        /// <returns>True if the left <see cref="RowVersion"/> value is equal to the right <see cref="RowVersion"/> value.</returns>
        public static bool operator ==(RowVersion comparand1, RowVersion comparand2) => comparand1.Equals(comparand2);

        /// <summary>
        /// Checks if one <see cref="RowVersion"/> value is greater than another.
        /// </summary>
        /// <param name="comparand1">The left <see cref="RowVersion"/> value.</param>
        /// <param name="comparand2">The right <see cref="RowVersion"/> value.</param>
        /// <returns>True if the left <see cref="RowVersion"/> value is greater than the right <see cref="RowVersion"/> value.</returns>
        public static bool operator >(RowVersion comparand1, RowVersion comparand2) => comparand1.CompareTo(comparand2) > 0;

        /// <summary>
        /// Checks if one <see cref="RowVersion"/> value is greater than or equal to another.
        /// </summary>
        /// <param name="comparand1">The left <see cref="RowVersion"/> value.</param>
        /// <param name="comparand2">The right <see cref="RowVersion"/> value.</param>
        /// <returns>True if the left <see cref="RowVersion"/> value is greater than or equal to the right <see cref="RowVersion"/> value.</returns>
        public static bool operator >=(RowVersion comparand1, RowVersion comparand2) => comparand1.CompareTo(comparand2) >= 0;

        /*
        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static RowVersion operator ++(RowVersion instance)
        {
            var value = instance.value;
            value += 1;
            return new RowVersion(value);
        }
        */

        /*
        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static RowVersion operator --(RowVersion instance)
        {
            var value = instance.value;
            value -= 1;
            return new RowVersion(value);
        }
        */

        /// <summary>
        /// Converts a <see cref="RowVersion"/> into a <see cref="byte"/>[].
        /// </summary>
        /// <param name="rowVersion">A <see cref="RowVersion"/> to convert.</param>
        /// <returns>An <see cref="Array"/> of <see cref="byte"/>.</returns>
        public static byte[] To(RowVersion rowVersion) => rowVersion;

        /// <summary>
        /// Converts a <see cref="byte"/>[] into a <see cref="RowVersion"/>.
        /// </summary>
        /// <param name="value">A SQL Server RowVersion value as a <see cref="byte"/>[].</param>
        /// <returns>A <see cref="RowVersion"/> object with the given value.</returns>
        public static RowVersion FromByteArray(byte[] value) => new RowVersion(value);

        /// <summary>
        /// Converts a <see cref="long"/> into a <see cref="RowVersion"/>.
        /// </summary>
        /// <param name="value">A SQL Server RowVersion value as a <see cref="long"/>.</param>
        /// <returns>A <see cref="RowVersion"/> object with the given value.</returns>
        public static RowVersion FromInt64(long value) => new RowVersion(unchecked((ulong)value));

        /// <summary>
        /// Converts a <see cref="ulong"/> into a <see cref="RowVersion"/>.
        /// </summary>
        /// <param name="value">A SQL Server RowVersion value as a <see cref="ulong"/>.</param>
        /// <returns>A <see cref="RowVersion"/> object with the given value.</returns>
        public static RowVersion FromUInt64(ulong value) => new RowVersion(value);

        /// <summary>
        /// Gets the maximum of two <see cref="RowVersion"/> values.
        /// </summary>
        /// <param name="comparand1">The left <see cref="RowVersion"/> value.</param>
        /// <param name="comparand2">The right <see cref="RowVersion"/> value.</param>
        /// <returns>The larger <see cref="RowVersion"/> value.</returns>
        public static RowVersion Max(RowVersion comparand1, RowVersion comparand2) => comparand1.value < comparand2.value ? comparand2 : comparand1;

        /// <summary>
        /// Gets the minimum of two <see cref="RowVersion"/> values.
        /// </summary>
        /// <param name="comparand1">The left <see cref="RowVersion"/> value.</param>
        /// <param name="comparand2">The right <see cref="RowVersion"/> value.</param>
        /// <returns>The smaller <see cref="RowVersion"/> value.</returns>
        public static RowVersion Min(RowVersion comparand1, RowVersion comparand2) => comparand1.value > comparand2.value ? comparand2 : comparand1;

        /// <summary>
        /// Converts a <see cref="byte"/>[] into a <see cref="RowVersion"/>.
        /// </summary>
        /// <param name="value">A SQL Server RowVersion value as a <see cref="byte"/>[].</param>
        /// <returns>A <see cref="RowVersion"/>.</returns>
        public static RowVersion? From(byte[] value) => value is null ? (RowVersion?)null : (RowVersion)value;

        /// <summary>
        /// Compares the <see cref="RowVersion"/> value against another object.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare against.</param>
        /// <returns>0 if the values are equal, -1 if the <paramref name="obj"/> value is greater, otherwise 1.</returns>
        int IComparable.CompareTo(object obj) => obj is null ? 1 : this.CompareTo((RowVersion)obj);

        /// <summary>
        /// Checks if the <see cref="RowVersion"/> value is equal to another.
        /// </summary>
        /// <param name="other">A different <see cref="RowVersion"/> value to compare this value against.</param>
        /// <returns>True if the two <see cref="RowVersion"/> values are equal, otherwise false.</returns>
        public bool Equals(RowVersion other) => other.value == this.value;

        /// <summary>
        /// Converts a <see cref="RowVersion"/> into a <see cref="byte"/>[].
        /// </summary>
        /// <returns>A <see cref="byte"/>[] value.</returns>
        public byte[] ToByteArray()
        {
            var r = new byte[8];
            r[0] = (byte)(this.value >> 56);
            r[1] = (byte)(this.value >> 48);
            r[2] = (byte)(this.value >> 40);
            r[3] = (byte)(this.value >> 32);
            r[4] = (byte)(this.value >> 24);
            r[5] = (byte)(this.value >> 16);
            r[6] = (byte)(this.value >> 8);
            r[7] = (byte)(this.value >> 0);
            return r;
        }

        /// <summary>
        /// Compares the <see cref="RowVersion"/> value against another.
        /// </summary>
        /// <param name="other">A <see cref="RowVersion"/> to compare against.</param>
        /// <returns>0 if the values are equal, -1 if the <paramref name="other"/> value is greater, otherwise 1.</returns>
        public int CompareTo(RowVersion other) => this.value == other.value ? 0 : this.value < other.value ? -1 : 1;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is RowVersion version && this.Equals(version);

        /// <inheritdoc/>
        public override int GetHashCode() => this.value.GetHashCode();

        /// <summary>
        /// Converts the <see cref="RowVersion"/> value to a string.
        /// </summary>
        /// <returns>A <see cref="string"/> representation of the <see cref="RowVersion"/> value.</returns>
        public override string ToString() => this.value.ToString("X16", CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts a <see cref="RowVersion"/> into a <see cref="ulong"/>.
        /// </summary>
        /// <returns>A <see cref="ulong"/> value.</returns>
        public ulong ToUInt64() => this.value;
    }
}
