﻿using System.ComponentModel;

namespace XFrame.Modules.Local
{
    /// <summary>
    /// 语言
    /// </summary>
    [DefaultValue(None)]
    public enum Language
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = -1,
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        Afrikaans = 0,
        Arabic = 1,
        Basque = 2,
        Belarusian = 3,
        Bulgarian = 4,
        Catalan = 5,
        Chinese = 6,
        Czech = 7,
        Danish = 8,
        Dutch = 9,
        English = 10,
        Estonian = 11,
        Faroese = 12,
        Finnish = 13,
        French = 14,
        German = 0xF,
        Greek = 0x10,
        Hebrew = 17,
        Icelandic = 19,
        Indonesian = 20,
        Italian = 21,
        Japanese = 22,
        Korean = 23,
        Latvian = 24,
        Lithuanian = 25,
        Norwegian = 26,
        Polish = 27,
        Portuguese = 28,
        Romanian = 29,
        Russian = 30,
        SerboCroatian = 0x1F,
        Slovak = 0x20,
        Slovenian = 33,
        Spanish = 34,
        Swedish = 35,
        Thai = 36,
        Turkish = 37,
        Ukrainian = 38,
        Vietnamese = 39,
        ChineseSimplified = 40,
        ChineseTraditional = 41,
        Unknown = 42,
        Hungarian = 18
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}
