using System;

namespace River.Framework {
    public static class StringPadding {
        public static string PadLeft(string str, int length, char paddingChar, bool truncate) {
            if (length < 0) {
                throw new ArgumentOutOfRangeException("length");
            }

            var ret = str ?? string.Empty;

            if (!truncate && ret.Length > length) {
                throw new ArgumentException("str", "str is too long");
            }

            if (str.Length < length) {
                ret = ret.PadLeft(length, paddingChar);
            } else {
                if (truncate) {
                    ret = ret.Substring(0, length);
                }
            }
            return ret;
        }

        public static string PadLeft(string str, int length) {
            return PadLeft(str, length, CharConstants.Blankspace, true);
        }

        public static string PadRight(string str, int length, char paddingChar, bool truncate) {
            if (length < 0) {
                throw new ArgumentOutOfRangeException("length");
            }

            var ret = str ?? string.Empty;

            if (!truncate && ret.Length > length) {
                throw new ArgumentException("str", "str is too long");
            }

            if (str.Length < length) {
                ret = ret.PadRight(length, paddingChar);
            } else {
                if (truncate) {
                    ret = ret.Substring(0, length);
                }
            }
            return ret;
        }

        public static string PadRight(string str, int length) {
            return PadRight(str, length, CharConstants.Blankspace, true);
        }

        public static string RemovePadLeft(string str) {
            return RemovePadLeft(str, CharConstants.Blankspace);
        }

        public static string RemovePadLeft(string str, char paddingChar) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }

            return str.TrimStart(paddingChar);
        }

        public static string RemovePadLeft(string str, char paddingChar, int chrNum) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }

            var index = 0;
            for (int i = 0; i < str.Length; i++) {
                if (str[i] == paddingChar && index < chrNum) {
                    index++;
                } else {
                    break;
                }
            }
            if (index < str.Length) {
                return str.Substring(index);
            } else {
                return string.Empty;
            }
        }

        public static string RemovePadRight(string str) {
            return RemovePadRight(str, CharConstants.Blankspace);
        }

        public static string RemovePadRight(string str, char paddingChar) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }

            return str.TrimEnd(paddingChar);
        }

        public static string RemovePadRight(string str, char paddingChar, int chrNum) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }

            var index = str.Length;

            while (index > 1 && chrNum > 0 && str[index - 1] == paddingChar) {
                index--;
                chrNum--;
            }

            if (index < 0) {
                return string.Empty;
            } else {
                return str.Substring(0, index);
            }
        }
    }
}
