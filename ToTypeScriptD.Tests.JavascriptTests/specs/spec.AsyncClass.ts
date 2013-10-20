/// <reference path="../definitions.ts" />

var expect = chai.expect;

describe('AsyncClass', function () {
    describe("when calling async functions", function () {

        var sut: ToTypeScriptD.Native.AsyncClass;

        before(() => {
            sut = new ToTypeScriptD.Native.AsyncClass();
        });

        it("should return a string asynchronously", function (done) {

            sut.getStringAsync("hello world").done(function (result) {
                expect(result).to.eq("hello world");
                done();
            });
        });

    });
});