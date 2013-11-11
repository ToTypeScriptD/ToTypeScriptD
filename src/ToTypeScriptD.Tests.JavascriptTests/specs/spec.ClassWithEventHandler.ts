/// <reference path="../definitions.ts" />

var expect = chai.expect;

describe('ClassWithEventHandler', function () {

    it("when calling onsomethinghappened it should raise the event.", function (done) {
        var item = new ToTypeScriptD.Native.ClassWithEventHandler();

        item.onsomethinghappened = function (ev) {
            expect(ev.detail[0]).to.equal("Something happened.");
            expect(ev.detail[1]).to.equal(10);
            done();
        };

        item.doSomething();

    });

    //it("when calling doSomethingTyped it should raise the event.", function (done) {
    //    var item = new ToTypeScriptD.Native.ClassWithEventHandler();
    //    item.onsampletyped = function (ev) {
    //        done();
    //    };
    //    item.doSomethingTyped();
    //});

});