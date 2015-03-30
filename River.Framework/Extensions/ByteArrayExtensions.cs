using System;
using System.Linq;
using System.Text.RegularExpressions;
using River.Framework.Validation;

namespace River.Framework.Extensions {
    public static class ByteArrayExtensions {
        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        public static string ToHexString(this byte[] byteArray) {
            return ByteArrayConverter.ByteArrayToHexString(byteArray);
        }

        /// <summary>
        /// 转换为二进制字符串
        /// </summary>
        public static string ToBitString(this byte[] byteArray) {
            return ByteArrayConverter.ByteArrayToBitString(byteArray);
        }

        /// <summary>
        /// 转换为二进制字符串(以空格为分隔符)
        /// </summary>
        /// <param name="group">是否分组</param>
        public static string ToBitString(this byte[] byteArray, bool group) {
            return ToBitString(byteArray, group, "\x20");
        }

        /// <summary>
        /// 转换为二进制字符串
        /// </summary>
        /// <param name="group">是否分组</param>
        /// <param name="separator">分隔符</param>
        public static string ToBitString(this byte[] byteArray, bool group, string separator) {
            if (group) {
                string str = ByteArrayConverter.ByteArrayToBitString(byteArray);
                var matchs = Regex.Matches(str, ".{8}", RegexOptions.Compiled); // 8个为一组
                string[] strArr = new string[matchs.Count];
                for (int i = 0; i < matchs.Count; i++) {
                    strArr[i] = matchs[i].Value;
                }
                return string.Join(separator, strArr);
            } else {
                return ToBitString(byteArray);
            }
        }

        /// <summary>
        /// 设置字节数组指定索引的位
        /// </summary>
        /// <param name="bitIndex">位的索引</param>
        public static void SetBit(this byte[] byteArray, int bitIndex) {
            ByteArrayConverter.SetBit(byteArray, bitIndex);
        }

        /// <summary>
        /// 清除字节数组指定索引的位
        /// </summary>
        /// <param name="bitIndex">位的索引</param>
        public static void ClearBit(this byte[] byteArray, int bitIndex) {
            ByteArrayConverter.ClearBit(byteArray, bitIndex);
        }

        /// <summary>
        /// 检测指定索引的位是否设置
        /// </summary>
        /// <param name="bitIndex">位的索引</param>
        /// <returns></returns>
        public static bool IsBitSet(this byte[] byteArray, int bitIndex) {
            return ByteArrayConverter.IsBitSet(byteArray, bitIndex);
        }

        /// <summary>
        /// 判断是否与指定的字节数组完全相同
        /// </summary>
        /// <param name="destByteArray">要进行比较的字节数组</param>
        /// <returns>是否相同</returns>
        public static bool AreEqual(this byte[] byteArray, byte[] destByteArray) {

            Guards.ThrowIfNull(byteArray, "firstBuf");
            Guards.ThrowIfNull(destByteArray, "secondBuf");

            return byteArray.SequenceEqual(destByteArray);
        }
    }
}
