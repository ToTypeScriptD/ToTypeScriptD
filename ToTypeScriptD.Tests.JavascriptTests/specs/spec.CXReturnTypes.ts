/// <reference path="../definitions.ts" />
var expect = chai.expect;

describe('CXReturnTypes', function () {
    describe("when the various methods.", function () {

        var sut: ToTypeScriptD.Native.CXReturnTypes;

        before(function () {
            sut = new ToTypeScriptD.Native.CXReturnTypes();
        });

        it("Should return a String^ as a string", function () {
            var result: string;
            result = sut.getString();
            expect(result).to.equal("Hello World!");
        });

        it("Should return an int32 as a number ", function () {
            var result: number;
            result = sut.getInt32();
            expect(result).to.equal(10);
        });

        it("Should return an uint32 as a number ", function () {
            var result: number;
            result = sut.getUInt32();
            expect(result).to.equal(10);
        });

        it("Should return an Guid as a string ", function () {
            var result: string;
            result = sut.getGuid();
            expect(result).to.equal("4d36e96e-e325-11c3-bfc1-08002be10318");
        });


        it("Should return an Boolean as a boolean", function () {
            var result: boolean;
            result = sut.getBoolean();
            expect(result).to.equal(true);
        });

        it("Should return a Byte as a number", function () {
            var result: number;
            result = sut.getByte();
            expect(result).to.equal(10);
        });

        it("Should return a Platform::Array<byte>^ as a number[]", function () {
            var result: number;
            result = sut.getBytes();
            var expected = { "0": 72, "1": 101, "2": 108, "3": 108, "4": 111, "5": 32, "6": 71, "7": 101, "8": 116, "9": 66, "10": 121, "11": 116, "12": 101, "13": 115, "14": 32, "15": 65, "16": 114, "17": 114, "18": 97, "19": 121, "20": 33, "21": 0 };
            expect(JSON.stringify(result)).to.equal(JSON.stringify(expected));
        });

    });
});
