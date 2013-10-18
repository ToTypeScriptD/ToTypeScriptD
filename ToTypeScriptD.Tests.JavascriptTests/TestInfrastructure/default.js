// ===============================================================================
//  Microsoft patterns & practices
//  Hilo JS Guidance
// ===============================================================================
//  Copyright © Microsoft Corporation.  All rights reserved.
//  This code released under the terms of the 
//  Microsoft patterns & practices license (http://hilojs.codeplex.com/license)
// ===============================================================================

(function (globals) {
    "use strict";

    var activation = Windows.ApplicationModel.Activation,
        app = WinJS.Application;

    function runSpecs() {
        // configure the spec runner
        var specRunner = new Test.SpecRunner({
            src: "src",
            specs: "specs",
            helpers: "helpers"
        });

        // Handle any errors in the execution that
        // were not part of a failing test
        specRunner.addEventListener("error", function (args) {
            document.querySelector("body").innerText = args.detail;
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

        document.querySelector("#errors").style.display = "block";
        // By returning true, we signal that the exception was handled,
        // preventing the application from being terminated
        return true;
    };

    app.addEventListener("activated", function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            args.setPromise(WinJS.UI.processAll().then(runSpecs));
        }
    }, false);

    app.start();
})(this);
