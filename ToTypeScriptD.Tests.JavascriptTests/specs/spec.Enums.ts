/// <reference path="../definitions.ts" />

var expect = chai.expect;

describe('Enums', function () {

    it("when calling a method with an enum parameter.", function (done) {
        var item = new ToTypeScriptD.Native.SampleEnumClass();

        var result = item.methowWithEnumParameter(ToTypeScriptD.Native.SampleEnum.a);
        expect(result).to.equal(ToTypeScriptD.Native.SampleEnum.a);

        expect(result.a).to.equal(ToTypeScriptD.Native.SampleEnum.a);
    });

});