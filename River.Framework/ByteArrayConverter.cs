using System;

namespace River.Framework {
    public static class ByteArrayConverter {
        /// <summary>
        /// 把字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="byteArray">要转换的字节数组</param>
        /// <returns>十六进制字符串</returns>
        public static unsafe string ByteArrayToHexString(byte[] byteArray) {
            Guards.ThrowIfNull(byteArray, "byteArray");

            char[] chrArr = new char[byteArray.Length << 1];

            fixed (byte* b = byteArray) {
                fixed (char* c = chrArr) {
                    byte* bp = b;
                    char* cp = c;
                    for (int i = 0; i < byteArray.Length; i++) {
                        *cp = GetHexChar((*bp & 0xF0) >> 4);
                        cp++;
                        *cp = GetHexChar(*bp & 0x0F);
                        cp++;
                        bp++;
                    }
                }
            }

            return new string(chrArr);
        }

        /// <summary>
        /// 把十六进制字符串转换为字节数组
        /// </summary>
        /// <param name="hexString">要转换的十六进制字符串</param>
        /// <returns>字节数组</returns>
        public static unsafe byte[] HexStringToByteArray(string hexString) {
            Guards.ThrowIfIsNullOrWhiteSpace(hexString, "hexString");

            char[] chrArr = hexString.ToCharArray();

            byte[] byteArray = new byte[((chrArr.Length + 1) >> 1)];

            fixed (char* c = chrArr) {
                fixed (byte* b = byteArray) {
                    char* cp = c;
                    byte* bp = b;

                    int i = (chrArr.Length & 0x01);
                    if (i == 1) { // 十六进制字符串长度是奇数， 前面补零变偶数
                        int l = GetHexValue(*c);
                        Guards.Validate(l != -1, "hexString", "hexString is not a valid string");
                        *bp = (byte)l;
                        cp++;
                        bp++;
                    }

                    for (; i < byteArray.Length; i++) {
                        int h = GetHexValue(*cp);
                        Guards.Validate(h != -1, "hexString", "hexString is not a valid string");
                        cp++;
                        int l = GetHexValue(*cp);
                        Guards.Validate(l != -1, "hexString", "hexString is not a valid string");
                        cp++;

                        *bp = (byte)((h << 4) | l);
                        bp++;
                    }
                }
            }

            return byteArray;
        }

        /// <summary>
        /// 将字节数组转换为二进制字符串
        /// </summary>
        /// <param name="byteArray">要转换的字节数组</param>
        /// <returns>二进制字符串</returns>
        public unsafe static string ByteArrayToBitString(byte[] byteArray) {
            Guards.ThrowIfNull(byteArray, "byteArray");

            char[] chrArr = new char[byteArray.Length * 8];

            fixed (byte* b = byteArray) {
                fixed (char* c = chrArr) {
                    byte* bp = b;
                    char* cp = c;
                    for (int i = 0; i < byteArray.Length; i++) {
                        for (int j = 0; j < 8; j++) {
                            *cp = GetHexChar((*bp >> (7 - j)) & 0x01);
                            cp++;
                        }
                        bp++;
                    }
                }
            }

            return new string(chrArr);
        }

        /// <summary>
        /// 设置字节数组指定索引的位
        /// </summary>
        /// <param name="byteArray">字节数组</param>
        /// <param name="bitIndex">位的索引</param>
        public static unsafe void SetBit(byte[] byteArray, int bitIndex) {
            Guards.ThrowIfNull(byteArray, "byteArray");

            int bitCount = byteArray.Length * 8;
            if (bitIndex < 0 || bitIndex > bitCount - 1)
                throw new ArgumentOutOfRangeException("bitIndex");

            fixed (byte* b = byteArray) {
                byte* bp = b;

                bp += bitIndex / 8;
                *bp |= (byte)(0x80 >> (bitIndex % 8));
            }
        }

        /// <summary>
        /// 清除字节数组指定索引的位
        /// </summary>
        /// <param name="byteArray">字节数组</param>
        /// <param name="bitIndex">位的索引</param>
        public static unsafe void ClearBit(byte[] byteArray, int bitIndex) {
            Guards.ThrowIfNull(byteArray, "byteArray");

            int bitCount = byteArray.Length * 8;
            if (bitIndex < 0 || bitIndex > bitCount - 1)
                throw new ArgumentOutOfRangeException("bitIndex");

            fixed (byte* b = byteArray) {
                byte* bp = b;

                bp += bitIndex / 8;
                *bp &= (byte)~(0x80 >> (bitIndex % 8));
            }
        }

        /// <summary>
        /// 检测指定索引的位是否设置
        /// </summary>
        /// <param name="byteArray">字节数组</param>
        /// <param name="bitIndex">位的索引</param>
        /// <returns>是否设置</returns>
        public static unsafe bool IsBitSet(byte[] byteArray, int bitIndex) {
            Guards.ThrowIfNull(byteArray, "byteArray");

            int bitCount = byteArray.Length * 8;
            if (bitIndex < 0 || bitIndex > bitCount - 1)
                throw new ArgumentOutOfRangeException("bitIndex");

            fixed (byte* b = byteArray) {
                byte* bp = b;
                bp += bitIndex / 8;

                return ((*bp << bitIndex % 8) & 0x80) == 0x80;
            }
        }

        #region private method

        private static char GetHexChar(int i) {
            if (i < 10) {
                return (char)(i + 0x30);
            }
            return (char)((i - 10) + 0x41);

        }

        private static int GetHexValue(char c) {
            if (c >= '0' && c <= '9') {
                return (byte)(c - '0');
            } else if (c >= 'A' && c <= 'F') {
                return (byte)(c - 'A' + 10);
            } else if (c >= 'a' && c <= 'f') {
                return (byte)(c - 'a' + 10);
            } else {
                return -1;
            }
        }

        #endregion
    }
}
