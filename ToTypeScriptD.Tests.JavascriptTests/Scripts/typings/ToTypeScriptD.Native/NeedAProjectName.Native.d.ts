declare module ToTypeScriptD.Native {

    export class Class1 implements ToTypeScriptD.Native.__IClass1PublicNonVirtuals {
        constructor();
    }

    export class ClassWithEventHandler implements ToTypeScriptD.Native.__IClassWithEventHandlerPublicNonVirtuals, ToTypeScriptD.Native.__IClassWithEventHandlerProtectedNonVirtuals {
        addEventListener(type: string, listener: EventListener): void;
        removeEventListener(type: string, listener: EventListener): void;
        onsomethinghappened(ev: any);
        constructor();
        doSomething(): void;
    }

    enum SampleEnum {
        a,
        b,
        c,
        d
    }

    enum SampleEnumNumbered {
        a,
        b,
        c,
        d
    }

    export class SomethingHappenedEventHandler {
        constructor(__param0: any, __param1: System.IntPtr);
        invoke(__param0: ToTypeScriptD.Native.ClassWithEventHandler, __param1: string): void;
    }

}

