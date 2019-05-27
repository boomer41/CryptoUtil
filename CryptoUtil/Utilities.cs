using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoUtil
{

    internal static class Utilities
    {

        private static readonly char[] HexPalette =
        {
            '0', '1', '2', '3',
            '4', '5', '6', '7',
            '8', '9', 'a', 'b',
            'c', 'd', 'e', 'f'
        };

        internal static string EncodeHex(Span<byte> data)
        {
            var stringBuilder = new StringBuilder(data.Length * 2);

            foreach (var b in data)
            {
                stringBuilder.Append(HexPalette[(b >> 4) & 0xF]);
                stringBuilder.Append(HexPalette[b & 0xF]);
            }

            return stringBuilder.ToString();
        }

        internal static byte[] DecodeHex(string data)
        {
            if (data.Length % 2 != 0)
            {
                throw new ArgumentException($"Length of {nameof(data)} must be divisible by two.");
            }

            var b = new byte[data.Length / 2];

            for (var i = 0; i < data.Length; i += 2)
            {
                b[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }

            return b;
        }

        internal static bool SafeCompare(IList<byte> a, IList<byte> b)
        {
            byte result = 0;

            if (a.Count != b.Count)
            {
                return false;
            }

            for (var i = 0; i < a.Count; i++)
            {
                result |= (byte) (a[i] ^ b[i]);
            }

            return result == 0;
        }

    }

}