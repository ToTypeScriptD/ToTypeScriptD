/// <reference path="../definitions.ts" />

var expect = chai.expect;

describe('ClassWithEventHandler', function () {

    it("when calling onsomethinghappened it should raise the event.", function (done) {
        var item = new WinmdToTypeScript.Native.ClassWithEventHandler();

        item.onsomethinghappened = function () {
            done();
        }

    });

});