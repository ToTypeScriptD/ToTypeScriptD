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

    });
});
