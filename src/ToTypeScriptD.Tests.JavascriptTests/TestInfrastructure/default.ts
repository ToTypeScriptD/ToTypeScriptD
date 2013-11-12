/// <reference path="../definitions.ts" />


(function (globals) {
    "use strict";

    var activation = Windows.ApplicationModel.Activation,
        app = WinJS.Application;

    function runSpecs() {
        // configure the spec runner
        var specRunner = new Tests.SpecRunner({
            src: "src",
            specs: "specs",
            helpers: "helpers",
            errorHandler: args => {
                (<any>document.querySelector("body")).innerText = args.detail;
            }
        });

        // run the specs
        specRunner.run();
    }

    WinJS.Application.onerror = function (e) {
        var errorsList = document.querySelector("#errors");
        var pre = document.createElement('pre');

        var message = "File/Url: " + e.detail.errorUrl;
        message +=  "\n    Line: " + e.detail.errorLine;
        message +=  "\n Message: " + e.detail.errorMessage;

        pre.innerText = message;
        errorsList.appendChild(pre);

        (<any>document.querySelector("#errors")).style.display = "block";
        // By returning true, we signal that the exception was handled,
        // preventing the application from being terminated
        return true;
    };

    app.addEventListener("activated", function (args) {
        args.detail.splashScreen.ondismissed = function () {
            console.dir(arguments);
            console.log('hi');
        }

        if (args.detail.kind === activation.ActivationKind.launch) {
            args.setPromise(WinJS.UI.processAll().then(runSpecs));
        }
    }, false);

    app.start();
})(this);
