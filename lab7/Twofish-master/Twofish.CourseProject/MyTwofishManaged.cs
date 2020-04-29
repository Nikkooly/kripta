using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TwofishManaged;

namespace TwofishManaged
{

    public class MyTwofishManagedTransform
    {
        private TwofishImplementation Implementation;

        public MyTwofishManagedTransform(byte[] key)
        {

            if (key == null) { throw new ArgumentNullException("Key", "Key cannot be null."); }
            if (key.Length > 256 / 8) { throw new ArgumentOutOfRangeException("rgbKey", "Key size mismatch."); }

            byte[] paddingBuffer;
            int paddingCount = 0;
            if (key == null) { throw new ArgumentNullException("Key", "Key cannot be null."); }
            if (key.Length > 256 / 8) { throw new ArgumentOutOfRangeException("Key", "Key size mismatch."); }
            if (key.Length != 256 / 8 && key.Length != 192 / 8 && key.Length != 128 / 8)
            {
                if (key.Length < 256 / 8 && key.Length > 192 / 8)
                {
                    paddingCount = 256 / 8 - key.Length;

                }
                if (key.Length < 192 / 8 && key.Length > 128 / 8)
                {
                    paddingCount = 192 / 8 - key.Length;
                }
                if (key.Length < 128 / 8)
                {
                    paddingCount = 128 / 8 - key.Length;
                }
                paddingBuffer = new byte[paddingCount];
                List<byte> r = key.ToList();
                r.AddRange(paddingBuffer);

                key = r.ToArray();
            }


            int k = key.Length * 8 / 64;

            var key32 = new uint[2 * k];
            Buffer.BlockCopy(key, 0, key32, 0, key.Length);

            this.Implementation = new TwofishImplementation(key32);
        }


        private static byte[] RemovePadding(byte[] outputBuffer)
        {

            var padding = outputBuffer[outputBuffer.Length - 1];
            if ((padding < 1) || (padding > 16)) { throw new CryptographicException("Invalid padding."); }
            for (int i = outputBuffer.Length - padding; i < outputBuffer.Length; i++)
            {
                if (outputBuffer[i] != padding) { throw new CryptographicException("Invalid padding."); }
            }
            var newOutputBuffer = new byte[outputBuffer.Length - padding];
            Buffer.BlockCopy(outputBuffer, 0, newOutputBuffer, 0, newOutputBuffer.Length);
            return newOutputBuffer;

        }

        public byte[] Encrypt(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null) { throw new ArgumentNullException("inputBuffer", "Input buffer cannot be null."); }
            if (inputOffset < 0) { throw new ArgumentOutOfRangeException("inputOffset", "Offset must be non-negative number."); }
            if ((inputCount < 0) || (inputCount > inputBuffer.Length)) { throw new ArgumentOutOfRangeException("inputCount", "Invalid input count."); }
            if ((inputBuffer.Length - inputCount) < inputOffset) { throw new ArgumentOutOfRangeException("inputCount", "Invalid input length."); }

            int paddedLength = 0; 
            int paddedInputOffset = 0;

            paddedLength = inputCount / 16 * 16 + 16; 
            byte[] paddedInputBuffer = new byte[paddedLength]; 
            paddedInputOffset = 0;
            Buffer.BlockCopy(inputBuffer, inputOffset, paddedInputBuffer, 0, inputCount);
            var added = (byte)(paddedLength - inputCount); 
            for (var i = inputCount; i < inputCount + added; i++) 
            {
                paddedInputBuffer[i] = added; 
            }

            var outputBuffer = new byte[paddedLength];

            for (var i = 0; i < paddedLength; i += 16) 
            {
                this.Implementation.BlockEncrypt(paddedInputBuffer, paddedInputOffset + i, outputBuffer, i);
            }

            return outputBuffer;

        }

        public byte[] Decrypt(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputCount % 16 != 0) { throw new ArgumentOutOfRangeException("inputCount", "Invalid input count."); }

            var outputBuffer = new byte[inputCount];
            var outputOffset = 0;



            for (var i = 0; i < inputCount; i += 16)
            {
                this.Implementation.BlockDecrypt(inputBuffer, inputOffset + i, outputBuffer, outputOffset + i);
            }

            return RemovePadding(outputBuffer);
        }

        private class TwofishImplementation //реализация алгоритма шифрования
        {

            public TwofishImplementation(uint[] key)
            {

                this.Key = new DWord[key.Length];
                for (var i = 0; i < this.Key.Length; i++)
                {
                    this.Key[i] = (DWord)key[i];
                }

                this.ReKey();
            }

            private readonly DWord[] Key;


            private const int BlockSize = 128;
            private const int Rounds = 16;


            private const int InputWhiten = 0;
            private const int OutputWhiten = 4;

            private DWord[] SBoxKeys = new DWord[4];
            private DWord[] SubKeys = new DWord[40]; //подключи


            private const int p = 16843009;


            private void ReKey() //процедура расширения ключа
            {

                var Me = new DWord[this.Key.Length / 2];
                var Mo = new DWord[this.Key.Length / 2];

                var kCnt = this.Key.Length / 2; 
                for (var i = 0; i < kCnt; i++) 
                {
                    Me[i] = this.Key[2 * i];
                    Mo[i] = this.Key[2 * i + 1];
                    this.SBoxKeys[i] = GenericSBoxKey(Me[i], Mo[i]);
                }


                var keyLen = this.Key.Length * 4 * 8;
                for (var i = 0; i < 20; i++)
                {
                    var A = F32((DWord)(i * 2 * p), Me, keyLen);
                    var B = F32((DWord)(i * 2 * p + p), Mo, keyLen);
                    B = RotateLeft(B, 8);
                    this.SubKeys[2 * i] = A + B;
                    this.SubKeys[2 * i + 1] = RotateLeft(A + 2 * B, 9);
                }
            }

            internal void BlockEncrypt(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputBufferOffset)
            {
                var x = new DWord[BlockSize / 32];
                for (var i = 0; i < BlockSize / 32; i++) 
                {
                    x[i] = new DWord(inputBuffer, inputOffset + i * 4) ^ this.SubKeys[InputWhiten + i];
                }

                var keyLen = this.Key.Length * 4 * 8;
                for (var r = 0; r < Rounds; r++) 
                {
                    var a = F32(x[0], this.SBoxKeys, keyLen);
                    var b = F32(RotateLeft(x[1], 8), this.SBoxKeys, keyLen);

                    x[3] = RotateLeft(x[3], 1);
                    x[2] ^= a + b + this.SubKeys[8 + 2 * r];
                    x[3] ^= a + 2 * b + this.SubKeys[9 + 2 * r];
                    x[2] = RotateRight(x[2], 1);

                    if (r < Rounds - 1)
                    {
                        var tmp = x[0]; x[0] = x[2]; x[2] = tmp;
                        tmp = x[1]; x[1] = x[3]; x[3] = tmp;
                    }
                }

                for (var i = 0; i < BlockSize / 32; i++)//выходное отбеливание
                {
                    var outValue = x[i] ^ this.SubKeys[OutputWhiten + i];
                    outputBuffer[outputBufferOffset + i * 4 + 0] = outValue.B0;
                    outputBuffer[outputBufferOffset + i * 4 + 1] = outValue.B1;
                    outputBuffer[outputBufferOffset + i * 4 + 2] = outValue.B2;
                    outputBuffer[outputBufferOffset + i * 4 + 3] = outValue.B3;

                }
            }



            internal void BlockDecrypt(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputBufferOffset)
            {
                var x = new DWord[BlockSize / 32];
                var input = new DWord[BlockSize / 32];
                for (var i = 0; i < BlockSize / 32; i++)
                {
                    input[i] = new DWord(inputBuffer, inputOffset + i * 4);
                    x[i] = input[i] ^ this.SubKeys[OutputWhiten + i];
                }

                var keyLen = this.Key.Length * 4 * 8;
                for (var r = Rounds - 1; r >= 0; r--)
                {
                    var t0 = F32(x[0], this.SBoxKeys, keyLen);
                    var t1 = F32(RotateLeft(x[1], 8), this.SBoxKeys, keyLen);

                    x[2] = RotateLeft(x[2], 1);
                    x[2] ^= t0 + t1 + this.SubKeys[8 + 2 * r];
                    x[3] ^= t0 + 2 * t1 + this.SubKeys[9 + 2 * r];
                    x[3] = RotateRight(x[3], 1);

                    if (r > 0)
                    {
                        t0 = x[0]; x[0] = x[2]; x[2] = t0;
                        t1 = x[1]; x[1] = x[3]; x[3] = t1;
                    }
                }

                for (var i = 0; i < BlockSize / 32; i++)
                {
                    x[i] ^= this.SubKeys[InputWhiten + i];

                    outputBuffer[outputBufferOffset + i * 4 + 0] = x[i].B0;
                    outputBuffer[outputBufferOffset + i * 4 + 1] = x[i].B1;
                    outputBuffer[outputBufferOffset + i * 4 + 2] = x[i].B2;
                    outputBuffer[outputBufferOffset + i * 4 + 3] = x[i].B3;
                }
            }

            private static byte g01(byte x, int i)
            {

                byte a0 = (byte)(x >> 4);
                byte b0 = (byte)(x % 16);

                byte a1 = (byte)(a0 ^ b0);
                byte b1 = (byte)(a0 ^ RotateRight4(b0, 1) ^ ((8 * a0) % 16));

                byte a2 = T_g[i][0, a1];
                byte b2 = T_g[i][1, b1];

                byte a3 = (byte)(a2 ^ b2);
                byte b3 = (byte)(a2 ^ RotateRight4(b2, 1) ^ ((8 * a2) % 16));

                byte a4 = T_g[i][2, a3];
                byte b4 = T_g[i][3, b3];

                byte y = (byte)(16 * b4 + a4);

                return y;

            }
            // табличные замены для функции g
            private static byte[][,] T_g = { new byte[,] {
												{ 0x08 , 0x01 , 0x07 , 0x0D , 0x06 , 0x0F , 0x03 , 0x02 , 0x00 , 0x0B , 0x05 , 0x09 , 0x0E , 0x0C , 0x0A , 0x04},
												{ 0x0E , 0x0C , 0x0B , 0x08 , 0x01 , 0x02 , 0x03 , 0x05 , 0x0F , 0x04 , 0x0A , 0x06 , 0x07 , 0x00 , 0x09 , 0x0D},
												{ 0x0B , 0x0A , 0x05 , 0x0E , 0x06 , 0x0D , 0x09 , 0x00 , 0x0C , 0x08 , 0x0F , 0x03 , 0x02 , 0x04 , 0x07 , 0x01},
												{ 0x0D , 0x07 , 0x0F , 0x04 , 0x01 , 0x02 , 0x06 , 0x0E , 0x09 , 0x0B , 0x03 , 0x00 , 0x08 , 0x05 , 0x0C , 0x0A}
											 },
											 new byte[,] {
												{ 0x02 ,  0x08 , 0x0B ,  0x0D ,  0x0F ,  0x07 ,  0x06  , 0x0E ,  0x03 ,  0x01 ,  0x09 ,  0x04 ,  0x00 ,  0x0A ,  0x0C ,  0x05},
												{ 0x01 ,  0x0E , 0x02 ,  0x0B ,  0x04 ,  0x0C ,  0x03 ,  0x07 ,  0x06 ,  0x0D ,  0x0A ,  0x05 ,  0x0F ,  0x09 ,  0x00 ,  0x08},
												{ 0x04 ,  0x0C , 0x07 ,  0x05 ,  0x01 ,  0x06 ,  0x09 ,  0x0A ,  0x00 ,  0x0E ,  0x0D ,  0x08 ,  0x02 ,  0x0B ,  0x03 ,  0x0F},
												{ 0x0B ,  0x09 , 0x05 ,  0x01 ,  0x0C ,  0x03 ,  0x0D ,  0x0E ,  0x06 ,  0x04 ,  0x07 ,  0x0F ,  0x02 ,  0x00 ,  0x08 ,  0x0A}
											 }
											};
            //операция циклического вращения вправо 4-битных величин, т.е.раздельное вращение каждого нибла обрабатываемого байта;
            private static byte RotateRight4(byte x, int n)
            {
                var c = new BitArray(new byte[] { x });
                var temp = c[3];
                for (int i = 3; i > 0; i--)
                {
                    c[i] = c[i - 1];
                }
                c[0] = temp;
                temp = c[7];
                for (int i = 7; i > 4; i--)
                {
                    c[i] = c[i - 1];
                }
                c[4] = temp;

                x = BitArrayToByte(c);


                return x;

            }

            private static byte BitArrayToByte(BitArray arr)
            {
                byte result = 0;
                for (byte i = 0, m = 1; i < 8; i++, m *= 2)
                {
                    result += arr.Get(i) ? m : (byte)0;
                }
                return result;
            }

            //функция g

            private static DWord F32(DWord x, DWord[] k32, int keyLen)
            {
                DWord r = new DWord();
                if (keyLen == 256)
                {
                    x.B0 = (byte)(g01(x.B0, 1) ^ k32[3].B0);
                    x.B1 = (byte)(g01(x.B1, 0) ^ k32[3].B1);
                    x.B2 = (byte)(g01(x.B2, 0) ^ k32[3].B2);
                    x.B3 = (byte)(g01(x.B3, 1) ^ k32[3].B3);
                }
                if (keyLen == 192)
                {
                    x.B0 = (byte)(g01(x.B0, 0) ^ k32[2].B0);
                    x.B1 = (byte)(g01(x.B1, 0) ^ k32[2].B1);
                    x.B2 = (byte)(g01(x.B2, 1) ^ k32[2].B2);
                    x.B3 = (byte)(g01(x.B3, 1) ^ k32[2].B3);
                }
                if (keyLen == 128)
                {

                    x.B0 = (byte)(g01(x.B0, 1) ^ k32[1].B0);
                    x.B1 = (byte)(g01(x.B1, 0) ^ k32[1].B1);
                    x.B2 = (byte)(g01(x.B2, 1) ^ k32[1].B2);
                    x.B3 = (byte)(g01(x.B3, 0) ^ k32[1].B3);

                    x.B0 = (byte)(g01(x.B0, 1) ^ k32[0].B0);
                    x.B1 = (byte)(g01(x.B1, 1) ^ k32[0].B1);
                    x.B2 = (byte)(g01(x.B2, 0) ^ k32[0].B2);
                    x.B3 = (byte)(g01(x.B3, 0) ^ k32[0].B3);

                    r.B0 = (byte)(g01(x.B0, 0) * Mds[0, 0] + g01(x.B1, 1) * Mds[0, 1] + g01(x.B2, 0) * Mds[0, 2] + g01(x.B3, 1) * Mds[0, 3]);
                    r.B1 = (byte)(g01(x.B0, 0) * Mds[1, 0] + g01(x.B1, 1) * Mds[1, 1] + g01(x.B2, 0) * Mds[1, 2] + g01(x.B3, 1) * Mds[1, 3]);
                    r.B2 = (byte)(g01(x.B0, 0) * Mds[2, 0] + g01(x.B1, 1) * Mds[2, 1] + g01(x.B2, 0) * Mds[2, 2] + g01(x.B3, 1) * Mds[2, 3]);
                    r.B3 = (byte)(g01(x.B0, 0) * Mds[3, 0] + g01(x.B1, 1) * Mds[3, 1] + g01(x.B2, 0) * Mds[3, 2] + g01(x.B3, 1) * Mds[3, 3]);

                }

                return r;
            }
            //матрица М1
            private static byte[,] Mds = {  { 0x01,0xEF,0x5B,0x5B},
                                            { 0x5B,0xEF,0xEF,0x01},
                                            { 0xEF,0x5B,0x01,0xEF},
                                            { 0xEF,0x01,0xEF,0x5B}
                                         };
            private static DWord RotateLeft(DWord x, int n)
            {
                return ((x << n) | (x >> (32 - n)));
            }

            private static DWord RotateRight(DWord x, int n)
            {
                return ((x >> n) | (x << (32 - n)));
            }

            private static DWord GenericSBoxKey(DWord k0, DWord k1)
            {
                var r = new DWord();

                r.B0 = (byte)(M2[0, 0] * k0.B0 + M2[0, 1] * k0.B1 + M2[0, 2] * k0.B2 + M2[0, 3] * k0.B3
                     + M2[0, 4] * k1.B0 + M2[0, 5] * k1.B1 + M2[0, 6] * k1.B2 + M2[0, 7] * k1.B3);
                r.B1 = (byte)(M2[1, 0] * k0.B0 + M2[1, 1] * k0.B1 + M2[1, 2] * k0.B2 + M2[1, 3] * k0.B3
                     + M2[1, 4] * k1.B0 + M2[1, 5] * k1.B1 + M2[1, 6] * k1.B2 + M2[1, 7] * k1.B3);
                r.B2 = (byte)(M2[2, 0] * k0.B0 + M2[2, 1] * k0.B1 + M2[2, 2] * k0.B2 + M2[2, 3] * k0.B3
                     + M2[2, 4] * k1.B0 + M2[2, 5] * k1.B1 + M2[2, 6] * k1.B2 + M2[2, 7] * k1.B3);
                r.B3 = (byte)(M2[3, 0] * k0.B0 + M2[3, 1] * k0.B1 + M2[3, 2] * k0.B2 + M2[3, 3] * k0.B3
                     + M2[3, 4] * k1.B0 + M2[3, 5] * k1.B1 + M2[3, 6] * k1.B2 + M2[3, 7] * k1.B3);

                return r;
            }

            private static byte[,] M2 = { 
										  {0x01 , 0xA4 , 0x55 , 0x87 , 0x5A , 0x58 , 0xDB , 0x9E},
										  {0xA4 , 0x56 , 0x82 , 0xF3 , 0x1E , 0xC6 , 0x68 , 0xE5},
										  {0x02 , 0xA1 , 0xFC , 0xC1 , 0x47 , 0xAE , 0x3D , 0x19},
										  {0xA4 , 0x55 , 0x87 , 0x5A , 0x58 , 0xDB , 0x9E , 0x03}
										};

            [StructLayout(LayoutKind.Explicit)]
            private struct DWord
            {

                [FieldOffset(0)]
                public byte B0;
                [FieldOffset(1)]
                public byte B1;
                [FieldOffset(2)]
                public byte B2;
                [FieldOffset(3)]
                public byte B3;

                [FieldOffset(0)]
                private uint Value;

                private DWord(uint value)
                    : this()
                {
                    this.Value = value;
                }

                internal DWord(byte[] buffer, int offset)
                    : this()
                {
                    this.B0 = buffer[offset + 0];
                    this.B1 = buffer[offset + 1];
                    this.B2 = buffer[offset + 2];
                    this.B3 = buffer[offset + 3];
                }


                public static explicit operator uint(DWord expr)
                {
                    return expr.Value;
                }


                public static explicit operator DWord(int value)
                {
                    return new DWord((uint)value);
                }

                public static explicit operator DWord(uint value)
                {
                    return new DWord(value);
                }


                public static DWord operator +(DWord expr1, DWord expr2)
                {
                    expr1.Value += expr2.Value;
                    return expr1;
                }

                public static DWord operator *(uint value, DWord expr)
                {
                    expr.Value = value * expr.Value;
                    return expr;
                }


                public static DWord operator |(DWord expr1, DWord expr2)
                {
                    expr1.Value |= expr2.Value;
                    return expr1;
                }

                public static DWord operator ^(DWord expr1, DWord expr2)
                {
                    expr1.Value ^= expr2.Value;
                    return expr1;
                }

                public static DWord operator <<(DWord expr, int count)
                {
                    expr.Value <<= count;
                    return expr;
                }

                public static DWord operator >>(DWord expr, int count)
                {
                    expr.Value >>= count;
                    return expr;
                }

            }

        }



    }

}
