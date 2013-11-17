/// <reference path="../definitions.ts" />
var expect = chai.expect;

describe('Enums', function () {
    describe("when calling a method with an enum parameter.", function () {

        var result: ToTypeScriptD.Native.SampleEnum;
        var input = ToTypeScriptD.Native.SampleEnum.b;

        before(function () {
            var enumClass = new ToTypeScriptD.Native.SampleEnumClass();
            result = enumClass.methowWithEnumParameter(input);
        });

        it("Should return the same enum value", function () {
            expect(result).to.equal(input);

            // This is odd TypeScript - the method return type of Enum and now typescript things I can take that result and pull properties off of it.
            // Reported: https://typescript.codeplex.com/workitem/1817
            expect(result.b).to.equal(undefined);
        });

        it("Should not get a value where the return type was an enum (since that was a number)", function () {
            // This is odd TypeScript - the method return type of Enum and now typescript things I can take that result and pull properties off of it.
            // Reported: https://typescript.codeplex.com/workitem/1817
            expect(result.b).to.equal(undefined);
        });

        it("Should reflect enum names correctly", function () {
            expect(Windows.UI.Text.TextScript.undefined).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.ansi).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.eastEurope).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.cyrillic).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.greek).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.turkish).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.hebrew).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.arabic).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.baltic).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.vietnamese).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.default).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.symbol).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.thai).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.shiftJis).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.gb2312).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.hangul).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.big5).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.pc437).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.oem).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.mac).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.armenian).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.syriac).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.thaana).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.devanagari).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.bengali).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.gurmukhi).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.gujarati).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.oriya).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.tamil).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.telugu).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.kannada).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.malayalam).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.sinhala).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.lao).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.tibetan).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.myanmar).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.georgian).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.jamo).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.ethiopic).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.cherokee).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.aboriginal).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.ogham).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.runic).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.khmer).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.mongolian).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.braille).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.yi).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.limbu).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.taiLe).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.newTaiLue).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.sylotiNagri).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.kharoshthi).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.kayahli).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.unicodeSymbol).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.emoji).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.glagolitic).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.lisu).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.vai).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.nko).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.osmanya).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.phagsPa).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.gothic).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.deseret).to.not.equal(undefined);
            expect(Windows.UI.Text.TextScript.tifinagh).to.not.equal(undefined);
        });

    });
});
