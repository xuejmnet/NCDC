using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Constant.CharacterSets;

namespace NCDC.Protocol.MySql;

public static class ProtocolUtility
{
    
	public static int GetBytesPerCharacter(MySqlCharacterSetEnum characterSet)
	{
		switch (characterSet)
		{
		case MySqlCharacterSetEnum.Latin2CzechCaseSensitive:
		case MySqlCharacterSetEnum.Dec8SwedishCaseInsensitive:
		case MySqlCharacterSetEnum.Cp850GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Latin1German1CaseInsensitive:
		case MySqlCharacterSetEnum.Hp8EnglishCaseInsensitive:
		case MySqlCharacterSetEnum.Koi8rGeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Latin1SwedishCaseInsensitive:
		case MySqlCharacterSetEnum.Latin2GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Swe7SwedishCaseInsensitive:
		case MySqlCharacterSetEnum.AsciiGeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1251BulgarianCaseInsensitive:
		case MySqlCharacterSetEnum.Latin1DanishCaseInsensitive:
		case MySqlCharacterSetEnum.HebrewGeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Tis620ThaiCaseInsensitive:
		case MySqlCharacterSetEnum.Latin7EstonianCaseSensitive:
		case MySqlCharacterSetEnum.Latin2HungarianCaseInsensitive:
		case MySqlCharacterSetEnum.Koi8uGeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1251UkrainianCaseInsensitive:
		case MySqlCharacterSetEnum.GreekGeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1250GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Latin2CroatianCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1257LithuanianCaseInsensitive:
		case MySqlCharacterSetEnum.Latin5TurkishCaseInsensitive:
		case MySqlCharacterSetEnum.Latin1German2CaseInsensitive:
		case MySqlCharacterSetEnum.Armscii8GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1250CzechCaseSensitive:
		case MySqlCharacterSetEnum.Cp866GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Keybcs2GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.MacceGeneralCaseInsensitive:
		case MySqlCharacterSetEnum.MacromanGeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Cp852GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Latin7GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Latin7GeneralCaseSensitive:
		case MySqlCharacterSetEnum.MacceBinary:
		case MySqlCharacterSetEnum.Cp1250CroatianCaseInsensitive:
		case MySqlCharacterSetEnum.Latin1Binary:
		case MySqlCharacterSetEnum.Latin1GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Latin1GeneralCaseSensitive:
		case MySqlCharacterSetEnum.Cp1251Binary:
		case MySqlCharacterSetEnum.Cp1251GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1251GeneralCaseSensitive:
		case MySqlCharacterSetEnum.MacromanBinary:
		case MySqlCharacterSetEnum.Cp1256GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1257Binary:
		case MySqlCharacterSetEnum.Cp1257GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Binary:
		case MySqlCharacterSetEnum.Armscii8Binary:
		case MySqlCharacterSetEnum.AsciiBinary:
		case MySqlCharacterSetEnum.Cp1250Binary:
		case MySqlCharacterSetEnum.Cp1256Binary:
		case MySqlCharacterSetEnum.Cp866Binary:
		case MySqlCharacterSetEnum.Dec8Binary:
		case MySqlCharacterSetEnum.GreekBinary:
		case MySqlCharacterSetEnum.HebrewBinary:
		case MySqlCharacterSetEnum.Hp8Binary:
		case MySqlCharacterSetEnum.Keybcs2Binary:
		case MySqlCharacterSetEnum.Koi8rBinary:
		case MySqlCharacterSetEnum.Koi8uBinary:
		case MySqlCharacterSetEnum.Latin2Binary:
		case MySqlCharacterSetEnum.Latin5Binary:
		case MySqlCharacterSetEnum.Latin7Binary:
		case MySqlCharacterSetEnum.Cp850Binary:
		case MySqlCharacterSetEnum.Cp852Binary:
		case MySqlCharacterSetEnum.Swe7Binary:
		case MySqlCharacterSetEnum.Tis620Binary:
		case MySqlCharacterSetEnum.Geostd8GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Geostd8Binary:
		case MySqlCharacterSetEnum.Latin1SpanishCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1250PolishCaseInsensitive:
		case MySqlCharacterSetEnum.Dec8SwedishNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Cp850GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Hp8EnglishNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Koi8rGeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Latin1SwedishNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Latin2GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Swe7SwedishNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.AsciiGeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.HebrewGeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Tis620ThaiNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Koi8uGeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.GreekGeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1250GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Latin5TurkishNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Armscii8GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Cp866GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Keybcs2GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.MacCentralEuropeanGeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.MacRomanGeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Cp852GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Latin7GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.MacCentralEuropeanNoPadBinary:
		case MySqlCharacterSetEnum.Latin1NoPadBinary:
		case MySqlCharacterSetEnum.Cp1251NoPadBinary:
		case MySqlCharacterSetEnum.Cp1251GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.MacRomanNoPadBinary:
		case MySqlCharacterSetEnum.Cp1256GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Cp1257NoPadBinary:
		case MySqlCharacterSetEnum.Cp1257GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Armscii8NoPadBinary:
		case MySqlCharacterSetEnum.AsciiNoPadBinary:
		case MySqlCharacterSetEnum.Cp1250NoPadBinary:
		case MySqlCharacterSetEnum.Cp1256NoPadBinary:
		case MySqlCharacterSetEnum.Cp866NoPadBinary:
		case MySqlCharacterSetEnum.Dec8NoPadBinary:
		case MySqlCharacterSetEnum.GreekNoPadBinary:
		case MySqlCharacterSetEnum.HebrewNoPadBinary:
		case MySqlCharacterSetEnum.Hp8NoPadBinary:
		case MySqlCharacterSetEnum.Keybcs2NoPadBinary:
		case MySqlCharacterSetEnum.Koi8rNoPadBinary:
		case MySqlCharacterSetEnum.Koi8uNoPadBinary:
		case MySqlCharacterSetEnum.Latin2NoPadBinary:
		case MySqlCharacterSetEnum.Latin5NoPadBinary:
		case MySqlCharacterSetEnum.Latin7NoPadBinary:
		case MySqlCharacterSetEnum.Cp850NoPadBinary:
		case MySqlCharacterSetEnum.Cp852NoPadBinary:
		case MySqlCharacterSetEnum.Swe7NoPadBinary:
		case MySqlCharacterSetEnum.Tis620NoPadBinary:
		case MySqlCharacterSetEnum.Geostd8GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Geostd8NoPadBinary:
			return 1;

		case MySqlCharacterSetEnum.Big5ChineseCaseInsensitive:
		case MySqlCharacterSetEnum.SjisJapaneseCaseInsensitive:
		case MySqlCharacterSetEnum.EuckrKoreanCaseInsensitive:
		case MySqlCharacterSetEnum.Gb2312ChineseCaseInsensitive:
		case MySqlCharacterSetEnum.GbkChineseCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Big5Binary:
		case MySqlCharacterSetEnum.EuckrBinary:
		case MySqlCharacterSetEnum.Gb2312Binary:
		case MySqlCharacterSetEnum.GbkBinary:
		case MySqlCharacterSetEnum.SjisBinary:
		case MySqlCharacterSetEnum.Ucs2Binary:
		case MySqlCharacterSetEnum.Cp932JapaneseCaseInsensitive:
		case MySqlCharacterSetEnum.Cp932Binary:
		case MySqlCharacterSetEnum.Ucs2UnicodeCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2IcelandicCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2LatvianCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2RomanianCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2SlovenianCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2PolishCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2EstonianCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2SpanishCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2SwedishCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2TurkishCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2CzechCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2DanishCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2LithuanianCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2SlovakCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2Spanish2CaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2RomanCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2PersianCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2EsperantoCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2HungarianCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2SinhalaCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2German2CaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2CroatianCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2Unicode520CaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2VietnameseCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2GeneralMySql500CaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2CroatianCaseInsensitiveMariaDb:
		case MySqlCharacterSetEnum.Ucs2MyanmarCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2ThaiUnicode520Weight2:
		case MySqlCharacterSetEnum.Big5ChineseNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.SjisJapaneseNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.EuckrKoreanNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Gb2312ChineseNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.GbkChineseNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Big5NoPadBinary:
		case MySqlCharacterSetEnum.EuckrNoPadBinary:
		case MySqlCharacterSetEnum.Gb2312NoPadBinary:
		case MySqlCharacterSetEnum.GbkNoPadBinary:
		case MySqlCharacterSetEnum.SjisNoPadBinary:
		case MySqlCharacterSetEnum.Ucs2NoPadBinary:
		case MySqlCharacterSetEnum.Cp932JapaneseNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Cp932NoPadBinary:
		case MySqlCharacterSetEnum.Ucs2UnicodeNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Ucs2Unicode520NoPadCaseInsensitive:
			return 2;

		case MySqlCharacterSetEnum.UjisJapaneseCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3ToLowerCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3Binary:
		case MySqlCharacterSetEnum.UjisBinary:
		case MySqlCharacterSetEnum.EucjpmsJapaneseCaseInsensitive:
		case MySqlCharacterSetEnum.EucjpmsBinary:
		case MySqlCharacterSetEnum.Utf8Mb3UnicodeCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3IcelandicCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3LatvianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3RomanianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3SlovenianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3PolishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3EstonianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3SpanishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3SwedishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3TurkishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3CzechCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3DanishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3LithuanianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3SlovakCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3Spanish2CaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3RomanCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3PersianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3EsperantoCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3HungarianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3SinhalaCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3German2CaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3CroatianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3Unicode520CaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3VietnameseCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3GeneralMySql500CaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3CroatianCaseInsensitiveMariaDb:
		case MySqlCharacterSetEnum.Utf8Mb3MyanmarCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3ThaiUnicode520Weight2:
		case MySqlCharacterSetEnum.UjisJapaneseNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3NoPadBinary:
		case MySqlCharacterSetEnum.UjisNoPadBinary:
		case MySqlCharacterSetEnum.EucjpmsJapaneseNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.EucjpmsNoPadBinary:
		case MySqlCharacterSetEnum.Utf8Mb3UnicodeNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb3Unicode520NoPadCaseInsensitive:
			return 3;

		case MySqlCharacterSetEnum.Utf8Mb4GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Binary:
		case MySqlCharacterSetEnum.Utf16GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16Binary:
		case MySqlCharacterSetEnum.Utf16leGeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32GeneralCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32Binary:
		case MySqlCharacterSetEnum.Utf16leBinary:
		case MySqlCharacterSetEnum.Utf16UnicodeCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16IcelandicCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16LatvianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16RomanianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16SlovenianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16PolishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16EstonianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16SpanishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16SwedishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16TurkishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16CzechCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16DanishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16LithuanianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16SlovakCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16Spanish2CaseInsensitive:
		case MySqlCharacterSetEnum.Utf16RomanCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16PersianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16EsperantoCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16HungarianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16SinhalaCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16German2CaseInsensitive:
		case MySqlCharacterSetEnum.Utf16CroatianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16Unicode520CaseInsensitive:
		case MySqlCharacterSetEnum.Utf16VietnameseCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32UnicodeCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32IcelandicCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32LatvianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32RomanianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32SlovenianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32PolishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32EstonianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32SpanishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32SwedishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32TurkishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32CzechCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32DanishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32LithuanianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32SlovakCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32Spanish2CaseInsensitive:
		case MySqlCharacterSetEnum.Utf32RomanCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32PersianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32EsperantoCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32HungarianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32SinhalaCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32German2CaseInsensitive:
		case MySqlCharacterSetEnum.Utf32CroatianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32Unicode520CaseInsensitive:
		case MySqlCharacterSetEnum.Utf32VietnameseCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4UnicodeCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4IcelandicCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4LatvianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4RomanianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SlovenianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4PolishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4EstonianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SpanishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SwedishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4TurkishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4CzechCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4DanishCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4LithuanianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SlovakCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Spanish2CaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4RomanCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4PersianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4EsperantoCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4HungarianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SinhalaCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4German2CaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4CroatianCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Unicode520CaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4VietnameseCaseInsensitive:
		case MySqlCharacterSetEnum.Gb18030ChineseCaseInsensitive:
		case MySqlCharacterSetEnum.Gb18030Binary:
		case MySqlCharacterSetEnum.Gb18030Unicode520CaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Uca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4GermanPhonebookUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4IcelandicUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4LatvianUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4RomanianUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SlovenianUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4PolishUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4EstonianUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SpanishUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SwedishUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4TurkishUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4CaseSensitiveUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4DanishUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4LithuanianUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SlovakUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4TraditionalSpanishUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4LatinUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4EsperantoUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4HungarianUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4CroatianUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4VietnameseUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Uca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4GermanPhonebookUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4IcelandicUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4LatvianUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4RomanianUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SlovenianUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4PolishUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4EstonianUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SpanishUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SwedishUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4TurkishUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4CaseSensitiveUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4DanishUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4LithuanianUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SlovakUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4TraditionalSpanishUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4LatinUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4EsperantoUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4HungarianUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4CroatianUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4VietnameseUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4JapaneseUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4JapaneseUca900AccentSensitiveCaseSensitiveKanaSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Uca900AccentSensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4RussianUca900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4RussianUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4ChineseUca900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Uca900Binary:
		case MySqlCharacterSetEnum.Utf8Mb4NorwegianBokmal0900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4NorwegianBokmal0900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4NorwegianNynorsk0900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4NorwegianNynorsk0900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SerbianLatin0900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4SerbianLatin0900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Bosnian0900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Bosnian0900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Bulgarian0900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Bulgarian0900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Galician0900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Galician0900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4MongolianCyrillic0900AccentInsensitiveCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4MongolianCyrillic0900AccentSensitiveCaseSensitive:
		case MySqlCharacterSetEnum.Utf8Mb4CroatianCaseInsensitiveMariaDb:
		case MySqlCharacterSetEnum.Utf8Mb4MyanmarCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4ThaiUnicode520Weight2:
		case MySqlCharacterSetEnum.Utf16CroatianCaseInsensitiveMariaDb:
		case MySqlCharacterSetEnum.Utf16MyanmarCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16ThaiUnicode520Weight2:
		case MySqlCharacterSetEnum.Utf32CroatianCaseInsensitiveMariaDb:
		case MySqlCharacterSetEnum.Utf32MyanmarCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32ThaiUnicode520Weight2:
		case MySqlCharacterSetEnum.Utf8Mb4GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4NoPadBinary:
		case MySqlCharacterSetEnum.Utf16GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16NoPadBinary:
		case MySqlCharacterSetEnum.Utf16leGeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32GeneralNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32NoPadBinary:
		case MySqlCharacterSetEnum.Utf16leNoPadBinary:
		case MySqlCharacterSetEnum.Utf16UnicodeNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf16Unicode520NoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32UnicodeNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf32Unicode520NoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4UnicodeNoPadCaseInsensitive:
		case MySqlCharacterSetEnum.Utf8Mb4Unicode520NoPadCaseInsensitive:
			return 4;

		default:
			throw new NotSupportedException($"Maximum byte length of character set {characterSet} is unknown.");
		}
	}
}