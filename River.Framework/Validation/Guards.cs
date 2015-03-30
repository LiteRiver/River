﻿using System;

namespace River.Framework.Validation {
    public static class Guards {
        public static void Validate(bool condition, string name) {
            if (!condition) {
                throw new ArgumentException("无效的参数", name);
            }
        }

        public static void Validate(bool condition, string name, string message) {
            if (!condition) {
                throw new ArgumentException(message, name);
            }
        }

        public static void ThrowIfNull<T>(T value, string name) where T : class {
            if (value == null) {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfNull<T>(T value, string name, string message) where T : class {
            if (value == null) {
                throw new ArgumentNullException(name, message);
            }
        }

        public static void ThrowIfNullOrEmpty(string value, string name) {
            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(string value, string name, string message) {
            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentException(message, name);
            }
        }

        public static void ThrowIfIsNullOrWhiteSpace(string value, string name) {
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(name);
            }
        }

        public static void ThrowIfIsNullOrWhiteSpace(string value, string name, string message) {
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(message, name);
            }
        }
    }
}
