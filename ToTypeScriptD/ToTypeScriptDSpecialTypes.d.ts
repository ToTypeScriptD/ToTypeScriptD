/*
 * This TypeScript definition file contains some core WinRT type 
 * definitions that the TypeScriptD output may need for things like async etc.
 *
 * Include this definition with the rest of your *.d.ts includes.
 *
 */


declare module ToTypeScriptD.WinRT {
    export interface IPromise<T> {
        then<U>(success?: (value: T) => IPromise<U>, error?: (error: any) => IPromise<U>, progress?: (progress: any) => void): IPromise<U>;
        then<U>(success?: (value: T) => IPromise<U>, error?: (error: any) => U, progress?: (progress: any) => void): IPromise<U>;
        then<U>(success?: (value: T) => U, error?: (error: any) => IPromise<U>, progress?: (progress: any) => void): IPromise<U>;
        then<U>(success?: (value: T) => U, error?: (error: any) => U, progress?: (progress: any) => void): IPromise<U>;
        done? <U>(success?: (value: T) => any, error?: (error: any) => any, progress?: (progress: any) => void): void;
    }
}
